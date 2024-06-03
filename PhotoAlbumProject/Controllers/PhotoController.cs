using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoAlbum.Application.DTOs;
using PhotoAlbum.Application.Interfaces;
using PhotoAlbumProject.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhotoAlbumProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IPhotoService _photoService;

        public PhotosController(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        [HttpGet("GetPhotos")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PhotoDto>>>> GetPhotos()
        {
            try
            {
                var photos = await _photoService.GetAllPhotosAsync();
                return Ok(new ApiResponse<IEnumerable<PhotoDto>>(true, "Photos retrieved successfully", photos));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<IEnumerable<PhotoDto>>(false, $"Internal server error: {ex.Message}"));
            }
        }

        [HttpGet("GetPhoto/{id}")]
        public async Task<ActionResult<ApiResponse<PhotoDto>>> GetPhoto(int id)
        {
            try
            {
                var photo = await _photoService.GetPhotoByIdAsync(id);
                if (photo == null)
                {
                    return NotFound(new ApiResponse<PhotoDto>(false, "Photo not found"));
                }
                return Ok(new ApiResponse<PhotoDto>(true, "Photo retrieved successfully", photo));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<PhotoDto>(false, $"Internal server error: {ex.Message}"));
            }
        }

        [HttpPost("CreatePhoto")]
        public async Task<ActionResult<ApiResponse<PhotoDto>>> CreatePhoto(PhotoDto photoDto)
        {
            try
            {
                var createdPhoto = await _photoService.AddPhotoAsync(photoDto);
                return CreatedAtAction(nameof(GetPhoto), new { id = createdPhoto.Id }, new ApiResponse<PhotoDto>(true, "Photo created successfully", createdPhoto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<PhotoDto>(false, $"Internal server error: {ex.Message}"));
            }
        }

        [HttpPut("UpdatePhoto/{id}")]
        public async Task<IActionResult> UpdatePhoto(int id, PhotoDto photoDto)
        {
            try
            {
                if (id != photoDto.Id)
                {
                    return BadRequest(new ApiResponse<PhotoDto>(false, "ID mismatch"));
                }

                await _photoService.UpdatePhotoAsync(photoDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<PhotoDto>(false, $"Internal server error: {ex.Message}"));
            }
        }

        [HttpDelete("DeletePhoto/{id}")]
        public async Task<IActionResult> DeletePhoto(int id)
        {
            try
            {
                await _photoService.DeletePhotoAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<PhotoDto>(false, $"Internal server error: {ex.Message}"));
            }
        }
    }
}
