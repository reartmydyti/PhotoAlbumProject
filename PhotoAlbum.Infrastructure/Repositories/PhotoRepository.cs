using Microsoft.EntityFrameworkCore;
using PhotoAlbum.Application.RepositoryInterfaces;
using PhotoAlbum.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAlbum.Infrastructure.Repositories
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly DataContext _context;

        public PhotoRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Photo>> GetAllPhotosAsync()
        {
            try
            {
                return await _context.Photos.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while retrieving photos", ex);
            }
        }

        public async Task<Photo> GetPhotoByIdAsync(int id)
        {
            try
            {
                return await _context.Photos.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while retrieving photo with ID {id}", ex);
            }
        }

        public async Task AddPhotoAsync(Photo photo)
        {
            try
            {
                await _context.Photos.AddAsync(photo);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while adding photo", ex);
            }
        }

        public async Task UpdatePhotoAsync(Photo photo)
        {
            try
            {
                _context.Photos.Update(photo);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while updating photo with ID {photo.Id}", ex);
            }
        }

        public async Task DeletePhotoAsync(int id)
        {
            try
            {
                var photo = await _context.Photos.FindAsync(id);
                if (photo != null)
                {
                    var comments = _context.Comments.Where(c => c.PhotoId == id);
                    _context.Comments.RemoveRange(comments);

                    _context.Photos.Remove(photo);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while deleting photo with ID {id}", ex);
            }
        }

    }
}
