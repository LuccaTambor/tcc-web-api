using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tcc_web_api.Data;
using tcc_web_api.Models;

namespace tcc_web_api.Controllers {
    [Route("api/teams")]
    [ApiController]
    public class TeamsController : ControllerBase {
        private TCCDbContext _context;

        public TeamsController(TCCDbContext context) {
            _context = context;
        }

        [HttpGet]
        [Route("getTeam")]
        public IActionResult GetTeam(int teamId) {
            var team = _context.Teams
            .Select(t => new {
                t.Id,
                t.TeamName,
                t.Developers,
                Project = _context.Projects.Where(p => p.Teams.Any(t => t.Id == teamId)).FirstOrDefault().Description
            })
            .FirstOrDefault(t => t.Id == teamId);
          

            return Ok(team);
        }

        [HttpPost]
        [Route("createTeam")]
        public IActionResult CreateTeam(int projectId, string teamName) {
            var project = _context.Projects.FirstOrDefault(p => p.Id == projectId);

            if(project == null) {
                return BadRequest();
            }

            var team = new Team {
                TeamName = teamName,
            };

            try {
                project.Teams.Add(team);
                _context.SaveChanges();
                return Ok();
            } catch(Exception ex) {
                return Content(ex.Message);
            }
        }
    }
}
