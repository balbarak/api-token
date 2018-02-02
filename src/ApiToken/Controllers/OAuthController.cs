using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ApiToken.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ApiToken.Controllers
{
    public class OAuthController : Controller
    {
        public IConfiguration Configuration { get; set; }

        public OAuthController(IConfiguration config)
        {
            Configuration = config;
        }

        public IActionResult Token()
        {

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, "Sultan"));
            claims.Add(new Claim(ClaimTypes.Sid, "39292"));
            claims.Add(new Claim(ClaimTypes.Version, "1.0"));

            //Get the key
            var keyStr = Configuration[WebConstants.TOKEN_KEY];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyStr)); 
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken jwtToken = new JwtSecurityToken
                (
                issuer: "yourdomain.com",
                audience: "yourdomain.com",
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials:creds,
                claims:claims.ToArray()
                );

            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return Ok(new
            {
                expires = DateTime.Now.AddMinutes(30),
                token
            });

        }
    }
}