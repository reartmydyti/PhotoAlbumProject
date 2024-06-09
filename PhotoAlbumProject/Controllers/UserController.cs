using Google.Apis.Auth;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;
using PhotoAlbum.Application.DTOs;
using PhotoAlbum.Application.Interfaces;
using PhotoAlbum.Domain.Entities;
using System.Security.Claims;

namespace PhotoAlbumProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;


        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                var response = await _userService.LoginAsync(dto);

                return StatusCode(response.Status, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        //[HttpPost("register")]
        //public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        //{
        //    try
        //    {
        //        var existingUser = await _userManager.FindByEmailAsync(dto.Email);
        //        if (existingUser != null)
        //        {
        //            return Conflict("User with this email already exists");
        //        }

        //        var newUser = new ApplicationUser
        //        {
        //            UserName = dto.Email,
        //            Email = dto.Email,
        //            FirstName = dto.FirstName,
        //            LastName = dto.LastName
        //        };

        //        var result = await _userManager.CreateAsync(newUser, dto.Password);
        //        if (result.Succeeded)
        //        {
                  
        //            return Ok("User registered successfully");
        //        }
        //        else
        //        {
        //            return BadRequest(result.Errors);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}




        [HttpPost("login-or-signup-google")]
        [EnableCors("AllowOrigin")]
        public async Task<IActionResult> LoginWithGoogle([FromBody] string credential)
        {
            try
            {
                var additionalAudiences = new List<string>
        {
            "760224497842-u3ikj4ddcjrhmq914hk6trpc7npr6qvo.apps.googleusercontent.com", // iosClientId
            "760224497842-lfmead4hp5qdtjb9735glb7s2ff85vvk.apps.googleusercontent.com", // webClientId
            "760224497842-lraq0eabem1al9q8cerrli8leo6eo9vi.apps.googleusercontent.com"  // androidClientId
        };

                foreach (var audience in additionalAudiences)
                {
                    var settings = new GoogleJsonWebSignature.ValidationSettings()
                    {
                        Audience = new List<string> { audience }
                    };

                    try
                    {
                        var payload = await GoogleJsonWebSignature.ValidateAsync(credential, settings);

                        var userEmail = payload.Email;
                        var userName = payload.Name;

                        var authenticationResponse = _userService.AuthenticateWithGoogle(userEmail, userName);

                        if (authenticationResponse != null)
                        {
                            return Ok(authenticationResponse);
                        }
                        else
                        {
                            return BadRequest("Authentication failed");
                        }
                    }
                    catch (InvalidJwtException)
                    {
                        continue;
                    }
                }

                return BadRequest("Invalid JWT token");
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred");
            }
        }


        [HttpGet("GetLoggedInUser")]
        [Authorize]
        public IActionResult GetLoggedInUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            return Ok(new { UserId = userId });
        }

        [HttpGet("GetUserDetails/{userId}")]
        public async Task<IActionResult> GetUserDetails(string userId)
        {
            try
            {
                var userDetails = await _userService.GetUserDetailsAsync(userId);
                if (userDetails == null)
                {
                    return NotFound(); 
                }

                return Ok(userDetails); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
