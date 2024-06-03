using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAlbum.Application.DTOs
{
    public class RatingDto
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public string UserId { get; set; }
        public int AlbumId { get; set; }
    }

}
