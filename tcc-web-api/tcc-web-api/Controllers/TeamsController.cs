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
    }
}
