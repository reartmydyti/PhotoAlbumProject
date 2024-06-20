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
    public class AlbumRepository : IAlbumRepository
    {
        private readonly DataContext _context;

        public AlbumRepository(DataContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<Album>> GetAllAlbumsAsync()
        {
            try
            {
                return await _context.Albums
                    .Include(a => a.Category)
                    .Include(a => a.Photos)
                    .Include(a => a.Comments)
                    .Include(a => a.Ratings)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while retrieving albums", ex);
            }
        }

        public async Task<Album> GetAlbumByIdAsync(int id)
        {
            try
            {
                return await _context.Albums
                    .Include(a => a.Category)
                    .Include(a => a.Photos)
                    .Include(a => a.Comments)
                    .Include(a => a.Ratings)
                    .FirstOrDefaultAsync(a => a.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while retrieving album with ID {id}", ex);
            }
        }

        public async Task AddAlbumAsync(Album album)
        {
            try
            {
                await _context.Albums.AddAsync(album);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while adding album", ex);
            }
        }

        public async Task UpdateAlbumAsync(Album album)
        {
            try
            {
                _context.Albums.Update(album);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while updating album with ID {album.Id}", ex);
            }
        }

        public async Task DeleteAlbumAsync(int id, string userId)
        {
            try
            {
                var album = await _context.Albums.FindAsync(id);
                if (album != null)
                {
                    if (album.UserId == userId)
                    {
                        var photos = await _context.Photos.Where(p => p.AlbumId == id).ToListAsync();

                        var photoIds = photos.Select(p => p.Id).ToList();
                        var comments = await _context.Comments.Where(c => photoIds.Contains(c.PhotoId.Value)).ToListAsync();

                        _context.Comments.RemoveRange(comments);

                        _context.Photos.RemoveRange(photos);

                        _context.Albums.Remove(album);

                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        throw new UnauthorizedAccessException("User does not have permission to delete this album.");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while deleting album with ID {id}", ex);
            }
        }








        public async Task<IEnumerable<Album>> SearchAlbumsAsync(string? searchTerm, int? categoryId)
        {
            try
            {
                IQueryable<Album> query = _context.Albums
                    .Include(a => a.Category)
                    .Include(a => a.Photos)
                    .Include(a => a.Comments)
                    .Include(a => a.Ratings);

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    query = query.Where(a => a.Name.Contains(searchTerm));
                }

                if (categoryId.HasValue)
                {
                    query = query.Where(a => a.CategoryId == categoryId.Value);
                }

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while searching albums", ex);
            }
        }


        public async Task<IEnumerable<Album>> GetAlbumsByCategoryAsync(int categoryId)
        {
            try
            {
                return await _context.Albums
                    .Include(a => a.Category)
                    .Include(a => a.Photos)
                    .Include(a => a.Comments)
                    .Include(a => a.Ratings)
                    .Where(a => a.CategoryId == categoryId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while retrieving albums for category ID {categoryId}", ex);
            }
        }


        public async Task<IEnumerable<Album>> GetAlbumsByUserIdAsync(string userId)
        {
            try
            {
                return await _context.Albums
                    .Include(a => a.Category)
                    .Include(a => a.Photos)
                    .Include(a => a.Comments)
                    .Include(a => a.Ratings)
                    .Where(a => a.UserId == userId) 
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while retrieving albums for user with ID {userId}", ex);
            }
        }

    }
}
