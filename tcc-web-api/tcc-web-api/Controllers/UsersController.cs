﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
            var devs = _context.Developers.ToList();
            return Ok(devs);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetManagerProjects")]
        public IActionResult GetProjectsFromManager(string id) {
            var result = _context.Projects.FirstOrDefault(m => m.Manager.Id == id);

            return Ok(result);
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

            //created user
            _context.Users.Add(newUser);
            _context.SaveChanges();

            return Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("CreateDeveloper")]
        public IActionResult CreateDeveloper() {
            Developer newUser = new Developer {
                Name = "Kyoko",
                Document = "987654321",
                UserName = "detective_girl",
                Password = "ultimate_dec",
                ConfirmPassword = "ultimate_dec",
                LockoutEnabled = false,
                Email = "kyoko.teste@mail.com",
                EmailConfirmed = true,
                Function = "Ultimate Detective"
            };

            _context.Developers.Add(newUser);
            _context.SaveChanges();

            return Ok();
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


        [HttpGet]
        [Route("login")]
        public IActionResult Login(string userName, string password) {

            try {
                var user = _context.Users.Where(u => u.UserName == userName).FirstOrDefault();

                if(user == null)
                    return NotFound("Não há usuário com este email");

                if(!user.Password.Equals(password))
                    return BadRequest("Senha incorreta");

                return Ok(user);

            } catch(Exception ex) {
                return BadRequest(ex);
            }    
        }
    }
}
