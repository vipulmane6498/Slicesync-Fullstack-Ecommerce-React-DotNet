using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SliceSync.Core.DTOs;
using SliceSync.Core.Enums;
using SliceSync.Core.IdentityEntities;

namespace SliceSync.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;


        public AuthController(Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
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
                return Ok($"{registerDTO.FullName} you are sucessfully registered !");
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
                ApplicationUser user = await _userManager.FindByEmailAsync(loginDTO.Email);

                if (user != null)
                {
                    //Check if the user is with specific role if yes return OK
                    //IsInRoleAsync -> verifies the give user is with the specified role in DB
                    if (await _userManager.IsInRoleAsync(user, UserTypeOptions.Admin.ToString())) //check if the signedIn user is in Admin role
                    {
                        return Ok($"{loginDTO.Email} sucessfully loggedIn as a Admin role !!");
                    }
                    else if (await _userManager.IsInRoleAsync(user, UserTypeOptions.Customer.ToString()))//check if the signedIn user is in Customer role
                    {
                        return Ok($"{loginDTO.Email} sucessfully loggedIn as a Customer role !!");
                    }
                    else if (await _userManager.IsInRoleAsync(user, UserTypeOptions.DeliveryGuy.ToString())) //check if the signedIn user is in Delivery role
                    {
                        return Ok($"{loginDTO.Email} sucessfully loggedIn as a DeliveryGuy role !!");
                    }
                 }
            }
            return BadRequest("Enter the valid Credentials!!");

        }



        //[HttpPost("logout")]
        //[Authorize]
        //public async Task<IActionResult> Logout()
        //{
        //    //find user name
        //   var userName= User.Identity?.Name;


        //    //Check is user is authenticated
        //    if (User.Identity != null && User.Identity.IsAuthenticated)
        //    {

                
        //        await _signInManager.SignOutAsync();
        //        return Ok(new
        //        {
        //            sucess = true,
        //            message = $"{userName} Logout successful!!"
        //        });
        //    }
        //    else
        //    {
        //        return BadRequest("User is not logged In !");
        //    }

        //}
    }

}
