using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PhotoAlbum.Application.RepositoryInterfaces;
using PhotoAlbum.Application.Responses;
using PhotoAlbum.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAlbum.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public UserRepository (DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public AuthenticateResponse AuthenticateWithGoogle(string name, string fullName)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(x => x.Email == name);

                if (user == null)
                {
                    var newUser = new ApplicationUser
                    {
                        Email = name,
                        FirstName = fullName,
                        LastName = "",
                        DateOfBirth = null,
                        EmailConfirmed = true,
                        UserName = name
                    };

                    _context.Users.Add(newUser);
                    _context.SaveChanges();

                    user = newUser;
                }

                var token = generateJwtToken(user);

                return new AuthenticateResponse(user, token);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while authenticating user with Google: {ex.Message}");
                throw;
            }
        }



        public string generateJwtToken(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:SecretKey"]);

            var claimList = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub,  user.Id),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _configuration["JwtSettings:Audience"],
                Issuer = _configuration["JwtSettings:Issuer"],
                Subject = new ClaimsIdentity(claimList),
                Expires = DateTime.UtcNow.AddDays(21),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        public async Task<UserDetailsResponse> GetUserDetailsAsync(string userId)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                    return null; 

                var userDetails = new UserDetailsResponse
                {
                    UserId = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    RoleId = user.RoleId 
                };

                return userDetails;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching user details: {ex.Message}");
                throw; 
            }
        }
    }
}
