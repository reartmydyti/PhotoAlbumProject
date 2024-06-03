using AutoMapper;
using PhotoAlbum.Application.DTOs;
using PhotoAlbum.Application.Interfaces;
using PhotoAlbum.Application.RepositoryInterfaces;
using PhotoAlbum.Domain.Entities;

namespace PhotoAlbum.Application.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly IPhotoRepository _photoRepository;
        private readonly IMapper _mapper;

        public PhotoService(IPhotoRepository photoRepository, IMapper mapper)
        {
            _photoRepository = photoRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PhotoDto>> GetAllPhotosAsync()
        {
            var photos = await _photoRepository.GetAllPhotosAsync();
            return _mapper.Map<IEnumerable<PhotoDto>>(photos);
        }

        public async Task<PhotoDto> GetPhotoByIdAsync(int id)
        {
            var photo = await _photoRepository.GetPhotoByIdAsync(id);
            return _mapper.Map<PhotoDto>(photo);
        }

        public async Task<PhotoDto> AddPhotoAsync(PhotoDto photoDto)
        {
            var photoEntity = _mapper.Map<Photo>(photoDto);
            await _photoRepository.AddPhotoAsync(photoEntity);
            var createdPhotoDto = _mapper.Map<PhotoDto>(photoEntity);
            return createdPhotoDto;
        }


        public async Task UpdatePhotoAsync(PhotoDto photoDto)
        {
            var photo = _mapper.Map<Photo>(photoDto);
            await _photoRepository.UpdatePhotoAsync(photo);
        }

        public async Task DeletePhotoAsync(int id)
        {
            await _photoRepository.DeletePhotoAsync(id);
        }
    }
}
