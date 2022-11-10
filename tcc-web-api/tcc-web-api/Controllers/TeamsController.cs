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

        [HttpPut]
        [Route("addDevToTeam")]
        public IActionResult addDevToTeam(string devId, int teamId) {
            var team = _context.Teams.FirstOrDefault(t => t.Id == teamId);

            var dev = _context.Developers.FirstOrDefault(d => d.Id == devId);

            if(team == null || dev == null)
                return BadRequest();


            try {
                team.Developers.Add(dev);
                dev.Teams.Add(team);

                _context.SaveChanges();

                var teamUpdated = _context.Teams
                .Select(t => new {
                    t.Id,
                    t.TeamName,
                    t.Developers,
                    Project = _context.Projects.Where(p => p.Teams.Any(t => t.Id == teamId)).FirstOrDefault().Description
                })
                .FirstOrDefault(t => t.Id == teamId);

                return Ok(teamUpdated);
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpPut]
        [Route("removeDevFromTeam")]
        public IActionResult RemoveDevFromTeam(string devId, int teamId) {
            var team = _context.Teams.Include(t=> t.Developers).FirstOrDefault(t => t.Id == teamId);
            var dev = _context.Developers.FirstOrDefault(d => d.Id == devId);

            team.Developers.Remove(dev);

            _context.SaveChanges();
            return Ok();
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

        [HttpDelete]
        [Route("deleteTeam")]
        public IActionResult DeleteTeam(int teamId) {
            var team = _context.Teams.FirstOrDefault(d => d.Id == teamId);

            try {
                _context.Teams.Remove(team);
                _context.SaveChanges();

                return Ok();
            } catch(Exception ex) {
                return BadRequest(ex);
            }
        }
    }
}
