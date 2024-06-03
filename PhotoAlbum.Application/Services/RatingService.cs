using AutoMapper;
using PhotoAlbum.Application.DTOs;
using PhotoAlbum.Application.Interfaces;
using PhotoAlbum.Application.RepositoryInterfaces;
using PhotoAlbum.Domain.Entities;

namespace PhotoAlbum.Application.Services
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IMapper _mapper;

        public RatingService(IRatingRepository ratingRepository, IMapper mapper)
        {
            _ratingRepository = ratingRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RatingDto>> GetAllRatingsAsync()
        {
            var ratings = await _ratingRepository.GetAllRatingsAsync();
            return _mapper.Map<IEnumerable<RatingDto>>(ratings);
        }

        public async Task<RatingDto> GetRatingByIdAsync(int id)
        {
            var rating = await _ratingRepository.GetRatingByIdAsync(id);
            return _mapper.Map<RatingDto>(rating);
        }

        public async Task<RatingDto> AddRatingAsync(RatingDto ratingDto)
        {
            var ratingEntity = _mapper.Map<Rating>(ratingDto);
            await _ratingRepository.AddRatingAsync(ratingEntity);
            var createdRatingDto = _mapper.Map<RatingDto>(ratingEntity);
            return createdRatingDto;
        }


        public async Task UpdateRatingAsync(RatingDto ratingDto)
        {
            var rating = _mapper.Map<Rating>(ratingDto);
            await _ratingRepository.UpdateRatingAsync(rating);
        }

        public async Task DeleteRatingAsync(int id)
        {
            await _ratingRepository.DeleteRatingAsync(id);
        }
    }
}
