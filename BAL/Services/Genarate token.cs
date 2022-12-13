using BAL.Abstraction;
using Entities.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BAL.Services
{
    public class Genarate_token : IGenarate
    {
        private readonly DbSportsBuzzContext _dbContext;
        private readonly IConfiguration _config;

        public Genarate_token(DbSportsBuzzContext dbContext, IConfiguration configuration)
           {
            _dbContext= dbContext;
            _config= configuration;
            }
        public string GenerateToken(TblUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            TblUserRole role = _dbContext.TblUserRoles.Where(x => x.UserRoleId == user.UserRole).FirstOrDefault()!;
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Email!),
                 new Claim(ClaimTypes.NameIdentifier,user.Password!),
                  new Claim(ClaimTypes.Role, role.UserRole!),

            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
