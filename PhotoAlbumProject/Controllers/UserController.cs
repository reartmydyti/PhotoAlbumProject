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
        private readonly IEmailService _emailService;

        public UserController(IUserService userService, IEmailService emailService)
        {
            _userService = userService;
            _emailService = emailService;
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
            var roleId = User.FindFirstValue(ClaimTypes.Role); 

            if (userId == null)
            {
                return Unauthorized();
            }

            return Ok(new { UserId = userId, RoleId = roleId });
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

        [HttpPost("SendContactEmail")]
        public async Task<IActionResult> SendContactEmail([FromBody] ContactFormDto contactFormDto)
        {
            try
            {
                await _emailService.SendContactEmailAsync(
                    contactFormDto.Email,
                    contactFormDto.FirstName,
                    contactFormDto.LastName,
                    contactFormDto.Description
                );
                return Ok(new { message = "Email sent successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
