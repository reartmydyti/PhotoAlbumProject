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

        public async Task<IEnumerable<AlbumDto>> GetAllAlbumsAsync()
        {
            var albums = await _albumRepository.GetAllAlbumsAsync();
            return _mapper.Map<IEnumerable<AlbumDto>>(albums);
        }

        public async Task<AlbumDto> GetAlbumByIdAsync(int id)
        {
            var album = await _albumRepository.GetAlbumByIdAsync(id);
            return _mapper.Map<AlbumDto>(album);
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

        public async Task DeleteAlbumAsync(int id)
        {
            await _albumRepository.DeleteAlbumAsync(id);
        }
    }
}
