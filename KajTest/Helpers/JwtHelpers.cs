using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using KajTest.Models;
using Microsoft.IdentityModel.Tokens;
namespace KajTest.Helpers
{
    public static class JwtHelpers
    {
        public static string GenetereToken(User user, List<string> roles)
        {
            string secretkey = "ndsf;kew3r$$2dfnbv23;k1234567890ABCDEF";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretkey));

            var credientials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

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
                signingCredentials: credientials,
                expires: DateTime.UtcNow.AddHours(1)
                
                );

            string generateToken = new JwtSecurityTokenHandler().WriteToken(token); 

            return generateToken;

        }
    }
}
