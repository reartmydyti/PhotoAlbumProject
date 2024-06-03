using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PhotoAlbum.Domain.Entities
{
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int AlbumId { get; set; }
        public Album Album { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
