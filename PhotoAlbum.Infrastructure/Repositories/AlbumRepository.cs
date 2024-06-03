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
                return await _context.Albums.ToListAsync();
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
                return await _context.Albums.FindAsync(id);
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

        public async Task DeleteAlbumAsync(int id)
        {
            try
            {
                var album = await _context.Albums.FindAsync(id);
                if (album != null)
                {
                    _context.Albums.Remove(album);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while deleting album with ID {id}", ex);
            }
        }
    }
}
