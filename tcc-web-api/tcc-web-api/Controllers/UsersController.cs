﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
        public IActionResult GetDevs(string id) {
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
                Name = "Makoto",
                Document = "12345678",
                UserName = "makoto_01",
                Password = "luckyhope",
                ConfirmPassword = "luckyhope",
                LockoutEnabled = false,
                Email = "makoto.teste@mail.com",
                EmailConfirmed = true,
                Function = "Ultimate Lucky Student"
            };

            _context.Developers.Add(newUser);
            _context.SaveChanges();

            return Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("CreateManager")]
        public IActionResult CreateManager(Manager manager) {
            if(!ModelState.IsValid) {
                return Content("Erro ao criar Gerente");
            }

            _context.Managers.Add(manager);

            try {
                _context.SaveChanges();
                return Ok();
            } catch(Exception ex) {
                return Content(ex.Message);
            }
        }


    }
}
