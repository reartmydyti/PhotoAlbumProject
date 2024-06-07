using PhotoAlbum.Application.DTOs;
using PhotoAlbum.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAlbum.Application.Interfaces
{
    public interface IAlbumService
    {
        Task<IEnumerable<GetAlbumDto>> GetAllAlbumsAsync();
        Task<GetAlbumDto> GetAlbumByIdAsync(int id);
        Task<AlbumDto> AddAlbumAsync(AlbumDto albumDto);
        Task UpdateAlbumAsync(AlbumDto albumDto);
        Task DeleteAlbumAsync(int id, string userId);
        Task<IEnumerable<GetAlbumDto>> SearchAlbumsAsync(string? searchTerm, int? categoryId);
        Task<IEnumerable<GetAlbumDto>> GetAlbumsByCategoryAsync(int categoryId);
        Task<IEnumerable<GetAlbumDto>> GetAlbumsByUserIdAsync(string userId);

    }

}
