using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SliceSync.Core.DTOs;
using SliceSync.Core.Enums;
using SliceSync.Core.IdentityEntities;
using SliceSync.Core.ServiceContracts;
using System.Security.Claims;

namespace SliceSync.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class AuthController : ControllerBase
    {
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtService _jwtService;


        public AuthController(Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, SignInManager<ApplicationUser> signInManager, IJwtService jwtService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
        }

        
        [HttpPost("register")]
        public async Task<IActionResult> Registeration(RegisterDTO registerDTO)
        {

            if (ModelState.IsValid == false)
            {
                //if (ModelState.ContainsKey("UserTypeOptions"))
                //{
                //    return BadRequest("Invalid Role! Select from: Customer, Admin, DeliveryGuy");
                //}
                return BadRequest(ModelState);
            }

            //if (!Enum.TryParse<UserTypeOptions>(registerDTO.userTypeOptions, true, out var role))
            //{
            //    return BadRequest("Invalid Role!");
            //}

            ApplicationUser user = new ApplicationUser()
            {
                FullName = registerDTO.FullName,
                UserName = registerDTO.Email,
                Email = registerDTO.Email,
                PhoneNumber = registerDTO.PhoneNumber,

            };


            Microsoft.AspNetCore.Identity.IdentityResult result = await _userManager.CreateAsync(user, registerDTO.Password);

            //if the user sucessfully created assign the role to the customer.
            if (result.Succeeded)
            {

                //when the user sucessfully created add him a role he confirmed
                //A. Customer Role
                //1. Check user provided role is exist in our enum
                if (registerDTO.userTypeOptions == Core.Enums.UserTypeOptions.Customer)
                {
                    //2. Check if the Customer role exists in the Database if does not exists create Customer role.
                    if (await _roleManager.FindByNameAsync(Core.Enums.UserTypeOptions.Customer.ToString()) is null)
                    {

                        //3. assign the role to the ApplicationRole Name property
                        ApplicationRole role = new ApplicationRole()
                        {
                            Name = registerDTO.userTypeOptions.ToString(),
                        };
                        //4. Create Customer role
                        await _roleManager.CreateAsync(role);
                    }
                    //5. Assign role to user
                    await _userManager.AddToRoleAsync(user, Core.Enums.UserTypeOptions.Customer.ToString());
                }

                //B. Admin Role
                //1. Check user provided role is exist in our enum
                else if (registerDTO.userTypeOptions == Core.Enums.UserTypeOptions.Admin)
                {

                    //2. Check if the Admin role exists in the Database if does not exists create Admin role.
                    if (await _roleManager.FindByNameAsync(UserTypeOptions.Admin.ToString()) is null)
                    {

                        //3. assign the Admin role to the ApplicationRole Name property
                        ApplicationRole role = new ApplicationRole()
                        {
                            Name = registerDTO.userTypeOptions.ToString(),
                        };
                        //4. Create Admin role
                        await _roleManager.CreateAsync(role);
                    }
                    //5. Assign Admin role to user
                    await _userManager.AddToRoleAsync(user, UserTypeOptions.Admin.ToString());

                }

                //C. DeliveryGuy Role
                //1. Check user provided role is exist in our enum
                else if (registerDTO.userTypeOptions == Core.Enums.UserTypeOptions.DeliveryGuy)
                {

                    //2. Check if the DeliveryGuy role exists in the Database if does not exists create DeliveryGuy role.
                    if (await _roleManager.FindByNameAsync(UserTypeOptions.DeliveryGuy.ToString()) is null)
                    {

                        //3. assign the role to the ApplicationRole Name property
                        ApplicationRole role = new ApplicationRole()
                        {
                            Name = registerDTO.userTypeOptions.ToString(),
                        };
                        //4. Create DeliveryGuy role
                        await _roleManager.CreateAsync(role);
                    }
                    //5. Assign DeliveryGuy role to user
                    await _userManager.AddToRoleAsync(user, UserTypeOptions.DeliveryGuy.ToString());
                }

                //D. if provided role not exists return error
                else
                {
                    return BadRequest("provided role does not exist in the system. Select Role from: Customer, Admin and DeliveryGuy !");
                }

                await _signInManager.SignInAsync(user, isPersistent: false);


                //call JWT method and store the token with user details in variable and return to client
                var authenticationResponse =  _jwtService.CreateJwtToken(user);

                // Store the newly generated refresh token on the user record
                user.JwtRefreshToken = authenticationResponse.JwtRefreshToken;

                // Store the expiration time so we can validate it on future refresh requests
                user.JwtRefreshTokenExpirationDateTime = authenticationResponse.JwtRefreshTokenExpirationDateTime;

                // Persist the updated refresh token and expiration to the database
                await _userManager.UpdateAsync(user);

                // Return 200 OK with the full authentication response (JWT + refresh token) to the client
                return Ok(authenticationResponse);
            }
            else
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("Register", error.Description);
                }
                return BadRequest("Failed");
            }
        }


        [HttpGet("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
            {
            if (ModelState.IsValid == false)
            {
                return BadRequest("invalid Creds");
            }

            //verify and signIn the user
            var result = await _signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, isPersistent: false, lockoutOnFailure: false);


            //If yes
            if (result.Succeeded)
            {
                //check if user is present in DB
                ApplicationUser? user = await _userManager.FindByEmailAsync(loginDTO.Email);

                if (user == null) {
                    Problem("Please add creds !!");
                }

                //if (user != null)
                //{
                ////Check if the user is with specific role if yes return OK
                ////IsInRoleAsync -> verifies the give user is with the specified role in DB
                //if (await _userManager.IsInRoleAsync(user, UserTypeOptions.Admin.ToString())) //check if the signedIn user is in Admin role
                //{
                //    return Ok($"{loginDTO.Email} sucessfully loggedIn as a Admin role !!");
                //}
                //else if (await _userManager.IsInRoleAsync(user, UserTypeOptions.Customer.ToString()))//check if the signedIn user is in Customer role
                //{
                //    return Ok($"{loginDTO.Email} sucessfully loggedIn as a Customer role !!");
                //}
                //else if (await _userManager.IsInRoleAsync(user, UserTypeOptions.DeliveryGuy.ToString())) //check if the signedIn user is in Delivery role
                //{
                //    return Ok($"{loginDTO.Email} sucessfully loggedIn as a DeliveryGuy role !!");
                //}
                // }


                //call JWT method and store the token with user details in variable and return to client
                var authenticationResponse =  _jwtService.CreateJwtToken(user);

                // Store the newly generated refresh token on the user record table
                user.JwtRefreshToken = authenticationResponse.JwtRefreshToken;

                // Store the expiration time in user table so we can validate it on future refresh requests
                user.JwtRefreshTokenExpirationDateTime = authenticationResponse.JwtRefreshTokenExpirationDateTime;

                // Persist the updated refresh token and expiration to the database
                await _userManager.UpdateAsync(user);

                // Return 200 OK with the full authentication response (JWT + refresh token) to the client
                return Ok(authenticationResponse);
            }
            else
            {
                return Problem("Enter the valid Credentials!!");
            }

        }



        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            //find user name
            var userName = User.Identity?.Name;


            //Check is user is authenticated
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                await _signInManager.SignOutAsync();
                return Ok(new
                {
                    sucess = true,
                    message = $"{userName} Logout successful!!"
                });
            }
            else
            {
                return BadRequest("User is not logged In !");
            }

        }

        // This API generates new JWT token
        // using expired JWT token and valid refresh token.
        //after every expiration of token, Client will send seperate API request for generate new jwt token 

        [HttpPost("generate-new-jwt-token")]
        public async Task<IActionResult> GenerateNewJwtAccessToken(TokenDTO tokenDTO)
        {
            // STEP 1:
            // Check if request data is null
            if (tokenDTO == null)
            {
                return BadRequest("Invalid Client request !!");
            }

            // STEP 2:
            // Extract user details from expired JWT token with help of GetPrincipalfromJwtToken() method
            ClaimsPrincipal? principal =
                _jwtService.GetPrincipalfromJwtToken(tokenDTO.Token);

            // STEP 3:
            // Check if JWT token is invalid
            if (principal == null)
            {
                return BadRequest("Invalid Jwt access toke !!");
            }

            // STEP 4:
            // Get email from JWT claims
            string? email =
                principal.FindFirstValue(ClaimTypes.Email);

            // STEP 5:
            // Find user using email
            ApplicationUser? user =
                await _userManager.FindByEmailAsync(email);

            // STEP 6:
            // Validate refresh token
            // Checks:
            // 1. User exists
            // 2. Refresh token matches
            // 3. Refresh token is not expired

            if (user == null
                || tokenDTO.RefreshToken != user.JwtRefreshToken
                || user.JwtRefreshTokenExpirationDateTime <= DateTime.Now)
            {
                return BadRequest("Invalid Refresh Token !!");
            }

            // STEP 7:
            // Generate new JWT token and refresh token
            var authenticationResponse =
                _jwtService.CreateJwtToken(user);

            // STEP 8:
            // Store new refresh token in database
            user.JwtRefreshToken =
                authenticationResponse.JwtRefreshToken;

            user.JwtRefreshTokenExpirationDateTime =
                authenticationResponse.JwtRefreshTokenExpirationDateTime;

            // STEP 9:
            // Return newly generated tokens
            return Ok(authenticationResponse);
        }
    }

}
