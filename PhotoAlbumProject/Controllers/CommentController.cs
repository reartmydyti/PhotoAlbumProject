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
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet("GetComments")]
        public async Task<ActionResult<ApiResponse<IEnumerable<CommentDto>>>> GetComments()
        {
            try
            {
                var comments = await _commentService.GetAllCommentsAsync();
                return Ok(new ApiResponse<IEnumerable<CommentDto>>(true, "Comments retrieved successfully", comments));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<IEnumerable<CommentDto>>(false, $"Internal server error: {ex.Message}"));
            }
        }

        [HttpGet("GetComment/{id}")]
        public async Task<ActionResult<ApiResponse<CommentDto>>> GetComment(int id)
        {
            try
            {
                var comment = await _commentService.GetCommentByIdAsync(id);
                if (comment == null)
                {
                    return NotFound(new ApiResponse<CommentDto>(false, "Comment not found"));
                }
                return Ok(new ApiResponse<CommentDto>(true, "Comment retrieved successfully", comment));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<CommentDto>(false, $"Internal server error: {ex.Message}"));
            }
        }

        [HttpPost("CreateComment")]
        public async Task<ActionResult<ApiResponse<CommentDto>>> CreateComment(CommentDto commentDto)
        {
            try
            {
                var createdComment = await _commentService.AddCommentAsync(commentDto);
                return CreatedAtAction(nameof(GetComment), new { id = createdComment.Id }, new ApiResponse<CommentDto>(true, "Comment created successfully", createdComment));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<CommentDto>(false, $"Internal server error: {ex.Message}"));
            }
        }

        [HttpPut("UpdateComment/{id}")]
        public async Task<IActionResult> UpdateComment(int id, CommentDto commentDto)
        {
            try
            {
                if (id != commentDto.Id)
                {
                    return BadRequest(new ApiResponse<CommentDto>(false, "ID mismatch"));
                }

                await _commentService.UpdateCommentAsync(commentDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<CommentDto>(false, $"Internal server error: {ex.Message}"));
            }
        }

        [HttpDelete("DeleteComment/{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            try
            {
                await _commentService.DeleteCommentAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<CommentDto>(false, $"Internal server error: {ex.Message}"));
            }
        }
    }
}
