using ChatDemoAPI2.Data;
using ChatDemoAPI2.Hubs;
using ChatDemoAPI2.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatDemoAPI2.Controllers
{    
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<RegisterModel>>> GetUsers()
        {            
            var users = await _context.registerUsers.ToListAsync();
            return users;
        }
        //public IActionResult GetAll()
        //{

        //    List<string> users = new List<string>();
        //    users = UserHandler.ConnectedIds.Keys.ToList();
        //    string current_user = User.Identity.Name;
        //    users.Remove(current_user);
        //    return Ok(new { users });
        //}
    }
}
