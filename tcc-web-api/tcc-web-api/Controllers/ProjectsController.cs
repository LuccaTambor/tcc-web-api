using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tcc_web_api.Data;
using tcc_web_api.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace tcc_web_api.Controllers {
    [Route("api/projects")]
    [ApiController]
    public class ProjectsController : ControllerBase {
        private TCCDbContext _context;

        public ProjectsController(TCCDbContext context) {
            _context = context;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("createFirstProject")]
        public IActionResult CreateProject() {
            Project project = new Project {
                Description = "Glory For All",
                StartedOn = new DateTime(2023, 01, 01),
            };

            _context.Projects.Add(project);
            _context.SaveChanges();
            return Ok();
        }

        
    }
}
