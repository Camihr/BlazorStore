using AdaStore.Shared.DTOs;
using AdaStore.Shared.Models;
using AdaStore.UI.Repositories;

namespace AdaStore.UI.Interfaces
{
    public interface IUsersRepository
    {
        Task<HttpResponseBase<AuthResponse>> RegisterUser(UserRegister user);
        Task<HttpResponseBase<AuthResponse>> Login(LoginCredentials user);
    }
}
