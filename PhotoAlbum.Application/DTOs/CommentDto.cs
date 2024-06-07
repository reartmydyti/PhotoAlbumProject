using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAlbum.Application.DTOs
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
        public int? AlbumId { get; set; }
        public int? PhotoId { get; set; }
        public DateTime? DatePosted { get; set; }

    }


    public class GetCommentsDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
        public int? AlbumId { get; set; }
        public int? PhotoId { get; set; }
        public DateTime? DatePosted { get; set; }
        public ApplicationUserDto? User { get; set; }
    }

}
