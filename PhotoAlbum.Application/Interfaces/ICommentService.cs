using PhotoAlbum.Application.DTOs;
using PhotoAlbum.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAlbum.Application.Interfaces
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentDto>> GetAllCommentsAsync();
        Task<CommentDto> GetCommentByIdAsync(int id);
        Task<CommentDto> AddCommentAsync(CommentDto commentDto);
        Task UpdateCommentAsync(CommentDto commentDto);
        Task DeleteCommentAsync(int id);
        Task<IEnumerable<GetCommentsDto>> GetCommentsByAlbumIdAsync(int albumId);
        Task<IEnumerable<GetCommentsDto>> GetCommentsByPhotoIdAsync(int photoId);
    }

}
