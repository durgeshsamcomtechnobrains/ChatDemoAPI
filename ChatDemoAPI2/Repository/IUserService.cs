using ChatDemoAPI2.Model;
using ChatDemoAPI2.Model.Dtos;

namespace ChatDemoAPI2.Repository
{
    public interface IUserService
    {
        Task<ServiceResult> RegisterUserAsync(RegisterModel model);
        Task<RegisterModel> AuthenticateUserAsync(string Email, string password);        
        Task<List<RegisterModel>> GetAllUsersAsync();
    }
}
