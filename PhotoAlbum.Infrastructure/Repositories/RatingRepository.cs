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
    public class RatingRepository : IRatingRepository
    {
        private readonly DataContext _context;

        public RatingRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Rating>> GetAllRatingsAsync()
        {
            try
            {
                return await _context.Ratings.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while retrieving ratings", ex);
            }
        }

        public async Task<Rating> GetRatingByIdAsync(int id)
        {
            try
            {
                return await _context.Ratings.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while retrieving rating with ID {id}", ex);
            }
        }

        public async Task AddRatingAsync(Rating rating)
        {
            try
            {
                await _context.Ratings.AddAsync(rating);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while adding rating", ex);
            }
        }

        public async Task UpdateRatingAsync(Rating rating)
        {
            try
            {
                _context.Ratings.Update(rating);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while updating rating with ID {rating.Id}", ex);
            }
        }

        public async Task DeleteRatingAsync(int id)
        {
            try
            {
                var rating = await _context.Ratings.FindAsync(id);
                if (rating != null)
                {
                    _context.Ratings.Remove(rating);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while deleting rating with ID {id}", ex);
            }
        }
    }
}
