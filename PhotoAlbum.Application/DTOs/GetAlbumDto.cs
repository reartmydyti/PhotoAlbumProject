using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAlbum.Application.DTOs
{
    public class GetAlbumDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int? CategoryId { get; set; }
        public CategoryDto? Category { get; set; }
        public ICollection<PhotoDto>? Photos { get; set; }
        public ICollection<CommentDto>? Comments { get; set; }
        public ICollection<RatingDto>? Ratings { get; set; }
        public double? AverageRating { get; set; }
        public string? UserId { get; set; }
    }
}
