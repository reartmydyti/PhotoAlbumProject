using PhotoAlbum.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAlbum.Application.RepositoryInterfaces
{
    public interface IPhotoRepository
    {
        Task<IEnumerable<Photo>> GetAllPhotosAsync();
        Task<Photo> GetPhotoByIdAsync(int id);
        Task AddPhotoAsync(Photo Photo);
        Task UpdatePhotoAsync(Photo Photo);
        Task DeletePhotoAsync(int id);
    }
}
