using AutoMapper;
using PhotoAlbum.Application.DTOs;
using PhotoAlbum.Application.Interfaces;
using PhotoAlbum.Application.RepositoryInterfaces;
using PhotoAlbum.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhotoAlbum.Application.Services
{
    public class AlbumService : IAlbumService
    {
        private readonly IAlbumRepository _albumRepository;
        private readonly IMapper _mapper;

        public AlbumService(IAlbumRepository albumRepository, IMapper mapper)
        {
            _albumRepository = albumRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetAlbumDto>> GetAllAlbumsAsync()
        {
            var albums = await _albumRepository.GetAllAlbumsAsync();

            var albumDtos = _mapper.Map<IEnumerable<GetAlbumDto>>(albums);

            foreach (var albumDto in albumDtos)
            {
                // Calculate average rating
                if (albumDto.Ratings.Any())
                {
                    albumDto.AverageRating = albumDto.Ratings.Average(r => r.Score);
                }
                else
                {
                    albumDto.AverageRating = 0; 
                }
            }

            return albumDtos;
        }


        public async Task<GetAlbumDto> GetAlbumByIdAsync(int id)
        {
            var album = await _albumRepository.GetAlbumByIdAsync(id);
            if (album == null)
            {
                return null;
            }

            var albumDto = _mapper.Map<GetAlbumDto>(album);

            if (album.Ratings.Any())
            {
                albumDto.AverageRating = album.Ratings.Average(r => r.Score);
            }
            else
            {
                albumDto.AverageRating = 0; 
            }

            return albumDto;
        }


        public async Task<AlbumDto> AddAlbumAsync(AlbumDto albumDto)
        {
            var albumEntity = _mapper.Map<Album>(albumDto);

            await _albumRepository.AddAlbumAsync(albumEntity);

            var createdAlbumDto = _mapper.Map<AlbumDto>(albumEntity);

            return createdAlbumDto;
        }


        public async Task UpdateAlbumAsync(AlbumDto albumDto)
        {
            var album = _mapper.Map<Album>(albumDto);
            await _albumRepository.UpdateAlbumAsync(album);
        }

        public async Task DeleteAlbumAsync(int id, string userId)
        {
            await _albumRepository.DeleteAlbumAsync(id, userId);
        }

        public async Task<IEnumerable<GetAlbumDto>> SearchAlbumsAsync(string? searchTerm, int? categoryId)
        {
            var albums = await _albumRepository.SearchAlbumsAsync(searchTerm, categoryId);
            return _mapper.Map<IEnumerable<GetAlbumDto>>(albums);
        }


        public async Task<IEnumerable<GetAlbumDto>> GetAlbumsByCategoryAsync(int categoryId)
        {
            var albums = await _albumRepository.GetAlbumsByCategoryAsync(categoryId);
            var albumDtos = _mapper.Map<IEnumerable<GetAlbumDto>>(albums);

            foreach (var albumDto in albumDtos)
            {
                if (albumDto.Ratings.Any())
                {
                    albumDto.AverageRating = albumDto.Ratings.Average(r => r.Score);
                }
                else
                {
                    albumDto.AverageRating = 0;
                }
            }

            return albumDtos;
        }

        public async Task<IEnumerable<GetAlbumDto>> GetAlbumsByUserIdAsync(string userId)
        {
            var albums = await _albumRepository.GetAlbumsByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<GetAlbumDto>>(albums);
        }

    }
}
