using PhotoAlbum.Application.DTOs;
using PhotoAlbum.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAlbum.Application.Interfaces
{
    public interface IPhotoService
    {
        Task<IEnumerable<PhotoDto>> GetAllPhotosAsync();
        Task<PhotoDto> GetPhotoByIdAsync(int id);
        Task<PhotoDto> AddPhotoAsync(PhotoDto photoDto);
        Task UpdatePhotoAsync(PhotoDto photoDto);
        Task DeletePhotoAsync(int id);
    }

}
