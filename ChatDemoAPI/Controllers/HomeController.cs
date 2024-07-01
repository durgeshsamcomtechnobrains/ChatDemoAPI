using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace ChatDemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {

        [HttpGet("getAll")]
        public IActionResult GetAll()
        {
            List<string> users = new List<string>();
            users = UserHandler.ConnectedIds.Keys.ToList();
            string current_user = User.Identity.Name;
            users.Remove(current_user);

            return Ok(new { users });
        }
    }
}
