using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tcc_web_api.Data;
using tcc_web_api.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http.Json;
using Newtonsoft.Json;
using System.Linq;

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
        [Route("getProjectAsDev")]
        public IActionResult GetProjectAsDev(int id, string devId) {
            var proj = _context.Projects.Where(p => p.Id == id).Select(p => new {
                p.Id,
                p.Description,
                p.StartedOn,
                p.ExpectedFinishDate,
                Teams = p.Teams.Where(t => t.Developers.Any(d => d.Id == devId)).Select(t => new {
                    t.Id,
                    t.TeamName
                })
            }).FirstOrDefault();

            return Ok(proj);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetManagerProjects")]
        public IActionResult GetProjectsFromManager(string id) {
            var result = _context.Projects.Where(m => m.Manager.Id == id).Include(p => p.Teams);

            return Ok(result);
        }

        [HttpGet]
        [Route("GetDevProjects")]
        public IActionResult GetProjectsFromDev(string id) {
            var teamsIdsDev = _context.Developers.Include(d => d.Teams).FirstOrDefault(d => d.Id == id).Teams.Select(t => t.Id);

            var projs = _context.Projects.Where(p => p.Teams.Select(t => t.Id).Any(v => teamsIdsDev.Contains(v))).Select(p => new {
                p.Id,
                p.Description,
                p.StartedOn,
                p.ExpectedFinishDate,
                Teams = p.Teams.Select(t => new {
                    t.Id,
                    t.TeamName
                })
            });

            return Ok(projs);
        }
    }
}
