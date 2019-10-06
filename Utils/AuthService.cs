using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtTemplate.Utils;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace JwtTemplate.Services
{
    public interface IAuthService
    {
        string GenerateToken(string id, bool isAdmin);
    }

    public class AuthService : IAuthService
    {
        private readonly AppSettings appsettings;

        public AuthService(IOptions<AppSettings> _appsettings)
        {
            appsettings = _appsettings.Value;
        }

        public string GenerateToken(string id, bool isAdmin = false)
        {

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appsettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = appsettings.Issuer,
                Audience = appsettings.Issuer,
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, id),
                    new Claim(ClaimTypes.Role, isAdmin?"Admin":"Regular"),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                     SecurityAlgorithms.HmacSha256Signature)//Note that the key should be longer than 128 bit
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}