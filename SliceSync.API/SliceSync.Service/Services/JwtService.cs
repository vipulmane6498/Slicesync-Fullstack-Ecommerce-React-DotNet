using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SliceSync.Core.DTOs;
using SliceSync.Core.IdentityEntities;
using SliceSync.Core.ServiceContracts;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SliceSync.Service.Services
{
    // This class is responsible for creating JWT Tokens
    // JWT Token is used after login to identify the user securely
    public class JwtService : IJwtService
    {
        // STEP 1:
        // IConfiguration is used to read values from appsettings.json
        // Example:
        // Jwt Key
        // Jwt Issuer
        // Token Expiration Time
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;

        // STEP 2:
        // Constructor Injection
        // IConfiguration object is automatically provided by ASP.NET Core
        public JwtService(IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        // STEP 3:
        // This method creates JWT Token for logged-in user
        public async Task<AuthenticationResponseDTO> CreateJwtToken(ApplicationUser applicationUser)
        {
            // STEP 4:
            // Set token expiration time
            // Example:
            // If expiration time is 30 minutes,
            // token becomes invalid after 30 minutes
            DateTime expiration = DateTime.UtcNow.AddMinutes(
                Convert.ToDouble(_configuration["Jwt:EXPIRATION_MINUTES"])
            );

            // STEP 5:
            // Claims are user details stored inside token
            // These details help identify the user
            var roles = await _userManager.GetRolesAsync(applicationUser);

            var claims = new List<Claim>
            {
                // Store unique User ID
                new Claim(
                    JwtRegisteredClaimNames.Sub,
                    applicationUser.Id.ToString()
                ),

                new Claim(
                    ClaimTypes.NameIdentifier,
                    applicationUser.Id.ToString()
                ),

                // Create unique ID for every token
                new Claim(
                    JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid().ToString()
                ),

                // Store token creation time
                new Claim(
                    JwtRegisteredClaimNames.Iat,
                    DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()
                ),

                // Store user email
                new Claim( 
                    ClaimTypes.Email,
                    applicationUser.Email
                ),

                // Store user's full name
                new Claim(
                    ClaimTypes.Name,
                    applicationUser.FullName
                )
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // STEP 6:
            // Create Secret Security Key
            // Secret key comes from appsettings.json
            // This key is used to secure the token
            SymmetricSecurityKey securityKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])
                );

            // STEP 7:
            // Create signing credentials
            // HmacSha256 is encryption algorithm used for token security
            SigningCredentials signingCredentials =
                new SigningCredentials(
                    securityKey,
                    SecurityAlgorithms.HmacSha256
                );

            // STEP 8:
            // Create JWT Token object
            JwtSecurityToken tokenGenerator = new JwtSecurityToken(

                // Who created the token
                _configuration["Jwt:Issuer"],

                // Who can use the token
                _configuration["Jwt:Audience"],

                // User information stored inside token
                claims,

                // Token expiry time
                expires: expiration,

                // Digital signature for security
                signingCredentials: signingCredentials
            );

            // STEP 9:
            // JwtSecurityTokenHandler helps convert token object into string
            JwtSecurityTokenHandler tokenHandler =
                new JwtSecurityTokenHandler();

            // STEP 10:
            // Convert token object into string format
            // This token is sent to frontend/client after login
            string token = tokenHandler.WriteToken(tokenGenerator);

            // STEP 11:
            // Return token and user details to client
            return new AuthenticationResponseDTO()
            {
                UserId = applicationUser.Id,
                JwtToken = token,
                Email = applicationUser.Email,
                PersonName = applicationUser.FullName,
                JwtTokenExpiration = expiration,
                JwtRefreshToken = GenerateJwtRefreshToken(),
                JwtRefreshTokenExpirationDateTime = DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["RefreshToken:EXPIRATION_MINUTES"]))
            };
        }


        /// <summary>
        /// Generates a cryptographically secure random refresh token.
        /// </summary>
        /// <returns>A Base64-encoded 64-byte random string to be used as a refresh token.</returns>
        private string GenerateJwtRefreshToken()
        {
            // Allocate a 64-byte buffer to hold the random bytes
            // (64 bytes = 512 bits of entropy, making brute-force attacks infeasible)
            byte[] bytes = new byte[64];

            // Create a cryptographically strong random number generator
            // (uses OS-level entropy source, unlike System.Random which is predictable)
            var randomNumberGenerator = RandomNumberGenerator.Create();

            // Fill the buffer with cryptographically secure random bytes
            randomNumberGenerator.GetBytes(bytes);

            // Convert the random bytes to a Base64 string for safe storage/transmission
            // Result is an 88-character URL-safe string
            return Convert.ToBase64String(bytes);
        }



        /// <summary>
        /// Method Name: GetPrincipalfromJwtToken
        /// Purpose:
        /// This method is used to read and validate a JWT token.
        /// It extracts the user's information (claims) from the token.
        ///
        /// Important:
        /// Even if the JWT token is expired, we still want to read the user details.
        /// That is why ValidateLifetime = false is used.
        /// </summary>
        /// <returns>
        /// ClaimsPrincipal -> Contains user information like:
        /// - User Id
        /// - Email
        /// - Roles     
        // If token is invalid, method throws exception.
        /// </returns>
        public ClaimsPrincipal? GetPrincipalfromJwtToken(string? token)
        {
            // STEP 1:
            // Set JWT validation rules

            
            
            var tokenValidationParameter = new TokenValidationParameters()
            {
                // Check token audience
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],

                // Check token issuer
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],

                // Check token signature using secret key
                ValidateIssuerSigningKey = true,

                // Secret key used to validate token
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])
                ),

                // Ignore token expiry time
                // because we want to read expired token
                ValidateLifetime = false,
            };



            // STEP 2:
            // Create JWT token handler object

            JwtSecurityTokenHandler jwtSecurityTokenHandler =
                new JwtSecurityTokenHandler();



            // STEP 3:
            // Validate token and extract claims

            ClaimsPrincipal principal =
                jwtSecurityTokenHandler.ValidateToken(
                    token,
                    tokenValidationParameter,
                    out SecurityToken securityToken
                );



            // STEP 4:
            // Check token algorithm for extra security

            if (
                securityToken is not JwtSecurityToken jwtSecurityToken
                || !jwtSecurityToken.Header.Alg.Equals(
                    SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase
                )
            )
            {
                throw new SecurityTokenException("Invalid Token");
            }



            // STEP 5:
            // Return extracted user details

            return principal;
        }

    }
}