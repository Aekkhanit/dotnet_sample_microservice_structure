using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Helpers
{

    public interface IJwtHelper
    {
        Task<string> GenerateTokenAsync(string username);
    }
    public class JwtHelper : IJwtHelper
    {
        private readonly IConfiguration _Configuration;

        public JwtHelper(IConfiguration configuration)
        {
            _Configuration = configuration;
        }

        public async Task<string> GenerateTokenAsync(string username)
        {
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Configuration["Jwt:Key"]));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            double expireMinutes = Convert.ToDouble(_Configuration["Jwt:ExpireMinutes"]);

            var _claims = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name,username)

            });

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = _claims,
                Expires = DateTime.Now.AddMinutes(expireMinutes),
                Issuer = _Configuration["Jwt:Issuer"],
                Audience = _Configuration["Jwt:Audience"],
                SigningCredentials = creds
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return await Task.FromResult(tokenHandler.WriteToken(token));
        }

    }
}
