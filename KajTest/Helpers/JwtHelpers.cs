using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using KajTest.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
namespace KajTest.Helpers
{
    public class JwtHelpers
    {
        private readonly JwtSettings _Jwt;

        public JwtHelpers(IOptions<JwtSettings> Jwt)
        {
            this._Jwt = Jwt.Value;
        }

        public string GenerateToken(User user, List<string> roles)
        {
           
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Jwt.SecretKey));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim> 
            { 
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            foreach (string role in roles) 
            {
                var claim = new Claim(ClaimTypes.Role, role);
                claims.Add(claim);
            
            };

            var token = new JwtSecurityToken(
                
                claims: claims,
                signingCredentials: credentials,
                expires: DateTime.UtcNow.AddMinutes(_Jwt.ExpiryMinutes),
                issuer : _Jwt.Issuer,
                audience :_Jwt.Audience                
                );

            string generateToken = new JwtSecurityTokenHandler().WriteToken(token); 

            return generateToken;

        }
    }
}
