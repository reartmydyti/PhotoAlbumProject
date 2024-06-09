using PhotoAlbum.Application.DTOs;
using PhotoAlbum.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAlbum.Application.Interfaces
{
    public interface IUserService
    {
        Task<ApiResponse> LoginAsync(LoginDto dto);
        AuthenticateResponse AuthenticateWithGoogle(string name, string fullName);
        Task<UserDetailsResponse> GetUserDetailsAsync(string userId);

    }
}
