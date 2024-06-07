using PhotoAlbum.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAlbum.Application.RepositoryInterfaces
{
    public interface IAlbumRepository
    {
        Task<IEnumerable<Album>> GetAllAlbumsAsync();
        Task<Album> GetAlbumByIdAsync(int id);
        Task AddAlbumAsync(Album album);
        Task UpdateAlbumAsync(Album album);
        Task DeleteAlbumAsync(int id, string userId);
        Task<IEnumerable<Album>> SearchAlbumsAsync(string? searchTerm, int? categoryId);
        Task<IEnumerable<Album>> GetAlbumsByCategoryAsync(int categoryId);
        Task<IEnumerable<Album>> GetAlbumsByUserIdAsync(string userId);
    }
}
