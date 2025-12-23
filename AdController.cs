using AuthRole.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace AuthRole.Controllers
{
    [ApiController]
    [Route("api/ad")]
    [Authorize(Roles = "Admin")]
    public class AdController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok(_context.meet.ToList());
        }

        [HttpPost]
        public IActionResult CreateUser(Meet user)
        {
            _context.meet.Add(user);
            _context.SaveChanges();
            return Ok(user);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, Meet user)
        {
            var existingUser = _context.meet.Find(id);
            if (existingUser == null)
                return NotFound();

            existingUser.UserName = user.UserName;
            existingUser.Password = user.Password;
            existingUser.Role = user.Role;

            _context.SaveChanges();
            return Ok(existingUser);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _context.meet.Find(id);
            if (user == null)
                return NotFound();

            _context.meet.Remove(user);
            _context.SaveChanges();
            return Ok("User Deleted");
        }
    }
}
