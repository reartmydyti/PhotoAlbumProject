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
        Task<IEnumerable<AlbumDto>> GetAllAlbumsAsync();
        Task<AlbumDto> GetAlbumByIdAsync(int id);
        Task<AlbumDto> AddAlbumAsync(AlbumDto albumDto);
        Task UpdateAlbumAsync(AlbumDto albumDto);
        Task DeleteAlbumAsync(int id);
    }

}
