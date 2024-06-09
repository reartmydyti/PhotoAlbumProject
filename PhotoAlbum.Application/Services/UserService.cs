using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PhotoAlbum.Application.DTOs;
using PhotoAlbum.Application.Interfaces;
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

namespace PhotoAlbum.Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;

        public UserService(UserManager<ApplicationUser> userManager, IConfiguration configuration, IUserRepository userRepository) 
        {
            _userManager = userManager;
            _configuration = configuration;
            _userRepository = userRepository;
        }


        public async Task<ApiResponse> LoginAsync(LoginDto dto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(dto.Email);
                if (user == null)
                    return new ApiResponse(400, "No User Associated with this Email.");

                var result = await _userManager.CheckPasswordAsync(user, dto.Password);
                if (!result)
                    return new ApiResponse(400, "Invalid Password.");

                var token = GenerateJwtToken(user);

                return new ApiResponse(200, token);
            }
            catch (Exception ex)
            {
                return new ApiResponse(400, "Error occurred: " + ex.Message);
            }
        }


        public string GenerateJwtToken(ApplicationUser user)
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





        public AuthenticateResponse AuthenticateWithGoogle(string name, string fullName)
        {
            try
            {



                return _userRepository.AuthenticateWithGoogle(name, fullName);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
            }
        }

        public async Task<UserDetailsResponse> GetUserDetailsAsync(string userId)
        {
            try
            {
                var userDetails = await _userRepository.GetUserDetailsAsync(userId);
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
