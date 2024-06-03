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
    public class AlbumsController : ControllerBase
    {
        private readonly IAlbumService _albumService;

        public AlbumsController(IAlbumService albumService)
        {
            _albumService = albumService;
        }

        [HttpGet("GetAlbums")]
        public async Task<ActionResult<ApiResponse<IEnumerable<AlbumDto>>>> GetAlbums()
        {
            try
            {
                var albums = await _albumService.GetAllAlbumsAsync();
                return Ok(new ApiResponse<IEnumerable<AlbumDto>>(true, "Albums retrieved successfully", albums));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<IEnumerable<AlbumDto>>(false, $"Internal server error: {ex.Message}"));
            }
        }

        [HttpGet("GetAlbum/{id}")]
        public async Task<ActionResult<ApiResponse<AlbumDto>>> GetAlbum(int id)
        {
            try
            {
                var album = await _albumService.GetAlbumByIdAsync(id);
                if (album == null)
                {
                    return NotFound(new ApiResponse<AlbumDto>(false, "Album not found"));
                }
                return Ok(new ApiResponse<AlbumDto>(true, "Album retrieved successfully", album));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<AlbumDto>(false, $"Internal server error: {ex.Message}"));
            }
        }

        [HttpPost("CreateAlbum")]
        public async Task<ActionResult<ApiResponse<AlbumDto>>> CreateAlbum(AlbumDto albumDto)
        {
            try
            {
                var createdAlbum = await _albumService.AddAlbumAsync(albumDto);
                return CreatedAtAction(nameof(GetAlbum), new { id = createdAlbum.Id }, new ApiResponse<AlbumDto>(true, "Album created successfully", createdAlbum));
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
                await _albumService.DeleteAlbumAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<AlbumDto>(false, $"Internal server error: {ex.Message}"));
            }
        }
    }
}
