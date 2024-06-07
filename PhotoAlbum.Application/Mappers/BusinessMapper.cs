using AutoMapper;
using PhotoAlbum.Application.DTOs;
using PhotoAlbum.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAlbum.Application.Mappers
{
    public class BusinessMapper : Profile
    {
        public BusinessMapper() 
        {
            CreateMap<ApplicationUser, ApplicationUserDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Album, AlbumDto>();
            CreateMap<Photo, PhotoDto>().ReverseMap();
            CreateMap<Comment, CommentDto>().ReverseMap();
            CreateMap<Comment, GetCommentsDto>().ReverseMap();
            CreateMap<Rating, RatingDto>().ReverseMap();
            CreateMap<Album, GetAlbumDto>().ReverseMap();

            CreateMap<AlbumDto, Album>()
            .ForMember(dest => dest.Photos, opt => opt.Ignore())
            .ReverseMap();
        }
    }
}
