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
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;

        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [HttpGet("GetRatings")]
        public async Task<ActionResult<ApiResponse<IEnumerable<RatingDto>>>> GetRatings()
        {
            try
            {
                var ratings = await _ratingService.GetAllRatingsAsync();
                return Ok(new ApiResponse<IEnumerable<RatingDto>>(true, "Ratings retrieved successfully", ratings));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<IEnumerable<RatingDto>>(false, $"Internal server error: {ex.Message}"));
            }
        }

        [HttpGet("GetRating/{id}")]
        public async Task<ActionResult<ApiResponse<RatingDto>>> GetRating(int id)
        {
            try
            {
                var rating = await _ratingService.GetRatingByIdAsync(id);
                if (rating == null)
                {
                    return NotFound(new ApiResponse<RatingDto>(false, "Rating not found"));
                }
                return Ok(new ApiResponse<RatingDto>(true, "Rating retrieved successfully", rating));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<RatingDto>(false, $"Internal server error: {ex.Message}"));
            }
        }

        [HttpPost("CreateRating")]
        public async Task<ActionResult<ApiResponse<RatingDto>>> CreateRating(RatingDto ratingDto)
        {
            try
            {
                var createdRating = await _ratingService.AddRatingAsync(ratingDto);
                return CreatedAtAction(nameof(GetRating), new { id = createdRating.Id }, new ApiResponse<RatingDto>(true, "Rating created successfully", createdRating));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<RatingDto>(false, $"Internal server error: {ex.Message}"));
            }
        }

        [HttpPut("UpdateRating/{id}")]
        public async Task<IActionResult> UpdateRating(int id, RatingDto ratingDto)
        {
            try
            {
                if (id != ratingDto.Id)
                {
                    return BadRequest(new ApiResponse<RatingDto>(false, "ID mismatch"));
                }

                await _ratingService.UpdateRatingAsync(ratingDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<RatingDto>(false, $"Internal server error: {ex.Message}"));
            }
        }

        [HttpDelete("DeleteRating/{id}")]
        public async Task<IActionResult> DeleteRating(int id)
        {
            try
            {
                await _ratingService.DeleteRatingAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<RatingDto>(false, $"Internal server error: {ex.Message}"));
            }
        }
    }
}
