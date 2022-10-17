using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;


namespace APIFundamentals.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        public AuthenticationController(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        private readonly IConfiguration _configuration;

        private class CityInfoUser
        {
            public int UserId { get; set; }
            public string UserName { get; set; }
            public string FirstName { get; set; }
            public string Lastname { get; set; }
            public string City { get; set; }

            public CityInfoUser(int userId, string userName, string firstName, string lastname, string city)
            {
                UserId = userId;
                UserName = userName;
                FirstName = firstName;
                Lastname = lastname;
                City = city;
            }
        }

        public class AuthenticationRequestBody
        {
            public string? UserName { get; set; }
            public string? Password { get; set; }
        }

       

        [HttpPost("authenticate")]
        public ActionResult<string> Authenticate(AuthenticationRequestBody authenticationRequestBody)
        {
            //Step1- Validation UserName and Password
            var user = ValidateUserCredentials(authenticationRequestBody.UserName, authenticationRequestBody.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            //Step 2: Token Creation
            var securityKey =
                new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Authentication:SecretForKey"]));

            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claimsforToken = new List<Claim>();
            claimsforToken.Add(new Claim("sub", user.UserId.ToString()));
            claimsforToken.Add(new Claim("given_name", user.FirstName.ToString()));
            claimsforToken.Add(new Claim("family_name", user.Lastname.ToString()));
            claimsforToken.Add(new Claim("city", user.City.ToString()));

            var jwtSecurityToken = new JwtSecurityToken(
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claimsforToken,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1),
                signingCredentials);

            var generattedToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return Ok(generattedToken);
        }

        private CityInfoUser ValidateUserCredentials(string? userName, string? password)
        {
           //In real scenario, the userInfo should come from Database 
           //This is just for testing purpose
           return new CityInfoUser(
               1,
               userName ?? "",
               "Sai",
               "Pallavi",
               "Wiesbaden"
           );
        }
    }
}
