using PhotoAlbum.Application.DTOs;
using PhotoAlbum.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAlbum.Application.Interfaces
{
    public interface IRatingService
    {
        Task<IEnumerable<RatingDto>> GetAllRatingsAsync();
        Task<RatingDto> GetRatingByIdAsync(int id);
        Task<RatingDto> AddRatingAsync(RatingDto ratingDto);
        Task UpdateRatingAsync(RatingDto ratingDto);
        Task DeleteRatingAsync(int id);
    }

}
