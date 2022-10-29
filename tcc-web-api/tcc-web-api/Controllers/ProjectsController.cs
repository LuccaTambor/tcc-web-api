using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tcc_web_api.Data;
using tcc_web_api.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http.Json;
using Newtonsoft.Json;

namespace tcc_web_api.Controllers {
    [Route("api/projects")]
    [ApiController]
    public class ProjectsController : ControllerBase {
        private TCCDbContext _context;

        public ProjectsController(TCCDbContext context) {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("getProjects")]
        public IActionResult GetProjects() {
            var projects = _context.Projects.Include(p => p.Manager).Include(p => p.Teams).ToList();

            var json = JsonConvert.SerializeObject(projects);

            return Ok(json);

        }

        [HttpGet]
        [Route("getProject")]
        public IActionResult GetProject(int id) {
            var project = _context.Projects
                .Include(p => p.Teams)
                .SingleOrDefault(p => p.Id == id);

            return Ok(project);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetManagerProjects")]
        public IActionResult GetProjectsFromManager(string id) {
            var result = _context.Projects.Where(m => m.Manager.Id == id).Include(p => p.Teams);

            return Ok(result);
        }


        [HttpPost]
        [AllowAnonymous]
        [Route("createProject")]
        public IActionResult CreateProject(string id, string description) {
            var manager = _context.Managers.FirstOrDefault(m => m.Id == id);

            if(manager == null)
                return NotFound();

            Project project = new Project {
                Description = description,
                StartedOn = new DateTime(2023, 01, 01),
                Manager = manager
            };

            _context.Projects.Add(project);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Route("CreateTestProject")]
        public IActionResult CreateTestProject() {
            var manager = _context.Managers.FirstOrDefault(m => m.UserName.Equals("LuccaTambor"));

            Project project = new Project {
                Description = "Project Mega Blaster",
                CreatedOn = DateTime.UtcNow,
                StartedOn = new DateTime(2023, 5, 01),
                ExpectedFinishDate = new DateTime(2026, 11, 11),
                Manager = manager
            };

            _context.Projects.Add(project);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Route("testTeam")]
        public IActionResult TestTeam() {
            var project = _context.Projects.FirstOrDefault(p => p.Id == 4);

            Team team = new Team {
                TeamName = "Time Técninco",
            };

            var dev = _context.Developers.FirstOrDefault(d => d.Id == "61e79caa-8a22-4c58-8869-6891273d5ac7");

            team.Developers.Add(dev);
            project.Teams.Add(team);
            _context.SaveChanges();

            return Ok();
        }
    }
}
