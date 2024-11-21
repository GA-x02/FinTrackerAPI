using AuthenticationApi.Application.DTOs;
using FinTracker.SharedLibrary.Responses;

namespace AuthenticationApi.Application.Interfaces
{
    public interface IAppUser
    {
        Task<Response> Register(AppUserDTO appUserDTO);
        Task<Response> Login(LoginDTO loginDTO);
        Task<GetAppUserDTO> GetAppUser(Guid userId);
    }
}
