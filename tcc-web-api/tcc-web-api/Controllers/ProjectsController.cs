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
        public IActionResult GetProject() {
            var projects = _context.Projects.Include(p => p.Manager).ToList();

            var json = JsonConvert.SerializeObject(projects);

            return Ok(json);

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

        

        
    }
}
