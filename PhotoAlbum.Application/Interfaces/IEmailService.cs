using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAlbum.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendContactEmailAsync(string fromEmail, string firstName, string lastName, string description);
    }
}
