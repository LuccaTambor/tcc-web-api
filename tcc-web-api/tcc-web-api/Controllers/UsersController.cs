using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tcc_web_api.Data;
using tcc_web_api.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace tcc_web_api.Controllers {
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase {
        private TCCDbContext _context;

        public UsersController(TCCDbContext context) {
            _context = context;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("createFirstUser")]
        public IActionResult CreateUser() {
            User newUser = new User {
                Name = "Lucca Tambor",
                UserName = "tambor",
                Password = "senha123",
                ConfirmPassword = "senha123",
                LockoutEnabled = false,
                NormalizedUserName = "tambor",
                Email = "lucca.tambor@unesp.br",
                EmailConfirmed = true,
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return Ok();
        }


    }
}
