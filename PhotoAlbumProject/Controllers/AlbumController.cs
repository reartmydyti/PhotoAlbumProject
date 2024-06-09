using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoAlbum.Application.DTOs;
using PhotoAlbum.Application.Interfaces;
using PhotoAlbumProject.Responses;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PhotoAlbumProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumsController : ControllerBase
    {
        private readonly IAlbumService _albumService;
        private readonly IPhotoService _photoService;
        private readonly HttpClient _httpClient;

        public AlbumsController(IAlbumService albumService, IPhotoService photoService, HttpClient httpClient)
        {
            _albumService = albumService;
            _photoService = photoService;
            _httpClient = httpClient;
        }

        [HttpGet("GetAlbums")]
        public async Task<ActionResult<ApiResponse<IEnumerable<GetAlbumDto>>>> GetAlbums()
        {
            try
            {
                var albums = await _albumService.GetAllAlbumsAsync();
                return Ok(new ApiResponse<IEnumerable<GetAlbumDto>>(true, "Albums retrieved successfully", albums));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<IEnumerable<GetAlbumDto>>(false, $"Internal server error: {ex.Message}"));
            }
        }

        [HttpGet("GetAlbumById/{id}")]
        public async Task<ActionResult<ApiResponse<GetAlbumDto>>> GetAlbumById(int id)
        {
            try
            {
                var album = await _albumService.GetAlbumByIdAsync(id);
                if (album == null)
                {
                    return NotFound(new ApiResponse<GetAlbumDto>(false, "Album not found"));
                }
                return Ok(new ApiResponse<GetAlbumDto>(true, "Album retrieved successfully", album));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<GetAlbumDto>(false, $"Internal server error: {ex.Message}"));
            }
        }

        [HttpPost("CreateAlbum")]
        public async Task<ActionResult<ApiResponse<AlbumDto>>> CreateAlbum([FromForm] AlbumDto albumDto)
        {
            try
            {
              
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userId == null)
                {
                    return Unauthorized(new ApiResponse<AlbumDto>(false, "User ID not found in token"));
                }

                albumDto.UserId = userId;

                var createdAlbum = await _albumService.AddAlbumAsync(albumDto);

                foreach (var photo in albumDto.Photos)
                {
                    if (photo.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
                        var filePath = Path.Combine("PhotoUploads", fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await photo.CopyToAsync(stream);
                        }

                        var photoDto = new PhotoDto
                        {
                            AlbumId = createdAlbum.Id,
                            Url = $"{Request.Scheme}://{Request.Host}/PhotoUploads/{fileName}"
                        };

                        await _photoService.AddPhotoAsync(photoDto);
                    }
                }

                return CreatedAtAction(nameof(GetAlbumById), new { id = createdAlbum.Id }, new ApiResponse<AlbumDto>(true, "Album created successfully", createdAlbum));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<AlbumDto>(false, $"Internal server error: {ex.Message}"));
            }
        }





        [HttpPut("UpdateAlbum/{id}")]
        public async Task<IActionResult> UpdateAlbum(int id, AlbumDto albumDto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                albumDto.UserId = userId;
                if (id != albumDto.Id)
                {
                    return BadRequest(new ApiResponse<AlbumDto>(false, "ID mismatch"));
                }

                await _albumService.UpdateAlbumAsync(albumDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<AlbumDto>(false, $"Internal server error: {ex.Message}"));
            }
        }

        [HttpDelete("DeleteAlbum/{id}")]
        public async Task<IActionResult> DeleteAlbum(int id)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                await _albumService.DeleteAlbumAsync(id, userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<AlbumDto>(false, $"Internal server error: {ex.Message}"));
            }
        }



        [HttpGet("SearchAlbums")]
        public async Task<ActionResult<ApiResponse<IEnumerable<GetAlbumDto>>>> SearchAlbums(string? searchTerm, int? categoryId)
        {
            try
            {
                var albums = await _albumService.SearchAlbumsAsync(searchTerm, categoryId);
                return Ok(new ApiResponse<IEnumerable<GetAlbumDto>>(true, "Albums retrieved successfully", albums));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<IEnumerable<GetAlbumDto>>(false, $"Internal server error: {ex.Message}"));
            }
        }


        [HttpGet("GetAlbumsByCategory/{categoryId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<GetAlbumDto>>>> GetAlbumsByCategory(int categoryId)
        {
            try
            {
                var albums = await _albumService.GetAlbumsByCategoryAsync(categoryId);
                return Ok(new ApiResponse<IEnumerable<GetAlbumDto>>(true, "Albums retrieved successfully", albums));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<IEnumerable<GetAlbumDto>>(false, $"Internal server error: {ex.Message}"));
            }
        }


        [HttpGet("GetAlbumsByUserId/{userId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<GetAlbumDto>>>> GetAlbumsByUserId(string userId)
        {
            try
            {
                 userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var albums = await _albumService.GetAlbumsByUserIdAsync(userId);
                return Ok(new ApiResponse<IEnumerable<GetAlbumDto>>(true, "Albums retrieved successfully", albums));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<IEnumerable<GetAlbumDto>>(false, $"Internal server error: {ex.Message}"));
            }
        }


        [HttpGet("DownloadPhoto/{photoId}")]
        public async Task<IActionResult> DownloadPhoto(int photoId)
        {
            try
            {
                var photo = await _photoService.GetPhotoByIdAsync(photoId);
                var photoUrl = photo.Url;

                var response = await _httpClient.GetAsync(photoUrl);

                if (response.IsSuccessStatusCode)
                {
                    Stream photoStream = await response.Content.ReadAsStreamAsync();

                    var fileExtension = Path.GetExtension(photoUrl);

                    var contentType = "image/jpeg";
                    if (fileExtension != null)
                    {
                        contentType = fileExtension switch
                        {
                            ".png" => "image/png",
                            ".jpg" or ".jpeg" => "image/jpeg",
                            ".jfif" => "image/jpeg", 
                            _ => contentType 
                        };
                    }

                    return File(photoStream, contentType, $"photo{photoId}{fileExtension}");
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
