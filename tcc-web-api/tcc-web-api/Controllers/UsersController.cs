using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Drawing;
using System.Net;
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

        [HttpGet]
        [AllowAnonymous]
        [Route("GetDevelopers")]
        public IActionResult GetDevs() {
            var devs = _context.Developers.OrderBy(d => d.Name).ToList();
            return Ok(devs);
        }

        [HttpGet]
        [Route("getDevelopersNotOnTeam")]
        public IActionResult GetDevsNotOnTeam(int teamId) {

            var devs = _context.Developers.Include(d => d.Teams).Where(d => !d.Teams.Any(t => t.Id == teamId)).Select(d => new {
                d.Id,
                d.Name,
                d.Function
            });

            return Ok(devs);
        }


        [HttpPost]
        [AllowAnonymous]
        [Route("CreateDeveloper")]
        public IActionResult CreateDeveloper([FromBody] Developer newDev) {

            try {
                _context.Developers.Add(newDev);
                _context.SaveChanges();

                var devs = _context.Developers.OrderBy(d => d.Name).ToList();
                return Ok(devs);
            } catch(Exception ex) {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("CreateManager")]
        public IActionResult CreateManager() {
            Manager manager = new Manager {
                Name = "Lucca Tambor",
                Document = "48387058831",
                UserName = "LuccaTambor",
                Password = "senha123",
                ConfirmPassword = "senha123",
                Designation = "Gerente de Projetos Sênior",
                Email = "lucca.tambor@unesp.br",
                PhoneNumber = "17991228040"
            };

            try {
                _context.Managers.Add(manager);
                _context.SaveChanges();
                return Ok();
            } catch(Exception ex) {
                return Content(ex.Message);
            }
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] AuthenticationData authentication) {

            try {
                var user = _context.Users.Where(u => u.UserName == authentication.UserName).FirstOrDefault();

                if(user == null)
                    return NotFound("Não há usuário com este email");

                if(!user.Password.Equals(authentication.Password))
                    return BadRequest("Senha incorreta");

                return Ok(user);

            } catch(Exception ex) {
                return BadRequest(ex);
            }
        }

        public class AuthenticationData {
            public string UserName { get; set; }
            public string Password { get; set; }
        }
    }
}
