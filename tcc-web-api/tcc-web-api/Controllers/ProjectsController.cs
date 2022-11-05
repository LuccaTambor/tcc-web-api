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
                .Select(p => new {
                    p.Id,
                    p.CreatedOn,
                    p.Description,
                    p.ExpectedFinishDate,
                    p.StartedOn,
                    Teams = p.Teams.Select(t => new {
                        t.Id,
                        t.TeamName,
                        Developers = t.Developers.Select(d => d.Name),
                        TaskNum = t.Tasks.Where(task => task.StartedOn.HasValue && !task.FinishedOn.HasValue).Count()
                    }),
                    OccurrencesInProject = p.Occurrences.Count()
                })
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
                    t.TeamName,
                    Developers = t.Developers.Select(d => d.Name),
                    TaskNum = t.Tasks.Where(task => task.StartedOn.HasValue && !task.FinishedOn.HasValue).Count()
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

        [HttpPost]
        [Route("CreateProject")]
        public IActionResult CreateProject([FromBody] ProjectModel project) {
            if(project == null)
                return BadRequest();

            var manager = _context.Managers.FirstOrDefault(m => m.Id == project.ManagerId);

            if(manager == null)
                return NotFound();

            Project newProject = new Project {
                Description = project.Description,
                ExpectedFinishDate = project.ExpectedFinishDate,
                StartedOn = project.StartedOn,
                Manager = manager
            };

            try {
                _context.Projects.Add(newProject);
                _context.SaveChanges();

                var result = _context.Projects.Include(p => p.Teams).Where(m => m.Manager.Id == project.ManagerId).Select(p => new {
                    p.Id,
                    p.Description,
                    p.ExpectedFinishDate,
                    p.StartedOn,
                    p.Teams
                });

                return Ok(result);
            } 
            catch(Exception ex) {
                return BadRequest(ex);
            }
           
        }

        [HttpDelete]
        [Route("removeProject")]
        public IActionResult RemoveProjet(int projId) {
            var project = _context.Projects.FirstOrDefault(p => p.Id == projId);

            if(project == null)
                return NotFound();

            _context.Projects.Remove(project);
            _context.SaveChanges();

            return Ok();
        }

        public class ProjectModel {
            public string Description { get; set; }
            public DateTime? StartedOn { get; set; }
            public DateTime? ExpectedFinishDate { get; set; }
            public string ManagerId { get; set; }
        }
    }
}
