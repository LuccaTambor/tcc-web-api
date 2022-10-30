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
    }
}
