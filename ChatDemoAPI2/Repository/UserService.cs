using ChatDemoAPI2.Model.Dtos;
using ChatDemoAPI2.Model;
using ChatDemoAPI2.Data;
using Microsoft.EntityFrameworkCore;

namespace ChatDemoAPI2.Repository
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult> RegisterUserAsync(RegisterModel model)
        {
            if (await _context.registerUsers.AnyAsync(u => u.Username == model.Username || u.Email == model.Email))
            {
                return new ServiceResult { Success = false, Message = "User already exists" };
            }

            model.Id = Guid.NewGuid();
            _context.registerUsers.Add(model);
            await _context.SaveChangesAsync();

            return new ServiceResult { Success = true, Message = "User registered successfully" };
        }

        public async Task<RegisterModel> AuthenticateUserAsync(string Email, string password)
        {
            return await _context.registerUsers.FirstOrDefaultAsync(u => u.Email == Email && u.Password == password);
        }

        public async Task<List<RegisterModel>> GetAllUsersAsync()
        {
            return await _context.registerUsers.ToListAsync();
        }
    }
}
