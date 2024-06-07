using PhotoAlbum.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAlbum.Application.RepositoryInterfaces
{
    public interface IUserRepository
    {
        AuthenticateResponse AuthenticateWithGoogle(string name, string fullName);
    }
}
