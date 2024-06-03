using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAlbum.Domain.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int? AlbumId { get; set; }
        public Album Album { get; set; }
        public int? PhotoId { get; set; }
        public Photo Photo { get; set; }
    }

}
