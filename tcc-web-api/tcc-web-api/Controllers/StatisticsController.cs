using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using tcc_web_api.Data;
using tcc_web_api.Models;
using tcc_web_api.Utils;

namespace tcc_web_api.Controllers {
    [Route("api/statistics")]
    [ApiController]
    public class StatisticsController : ControllerBase {
        private TCCDbContext _context;

        public StatisticsController(TCCDbContext context) {
            _context = context;
        }

        [HttpGet]
        [Route("getStatistics")]
        public IActionResult GetStatisticsFromManager(string managerId) {
            var manager = _context.Managers.FirstOrDefault(m => m.Id == managerId);

            if(manager == null)
                return NotFound();

            DateTime start = DateTime.UtcNow.AddDays(-30);
            DateTime end = DateTime.UtcNow;

            var last30Days = Enumerable.Range(0, 1 + end.Subtract(start).Days)
              .Select(offset => start.AddDays(offset).Date)
              .ToArray();

            var result = _context.Projects
                .Include(p => p.Manager)
                .Where(p => p.Manager.Id == managerId)
                .Select(p => new {
                    p.Id,
                    ProjectName = p.Description,
                    Occurrences = p.Occurrences.Select(o => new {
                        o.Description,
                        o.OccurrenceType,
                        OccurrenceTypeString = o.OccurrenceType.GetString(),
                        Date = o.CreatedOn.Date,
                        DeveloperCreator = o.Developer.Name
                    }),
                    OccurrencesNumber = p.Occurrences.Count(),
                    Teams = p.Teams.Select(t => new { 
                        t.TeamName,  
                        Devs =  t.Developers.Select(d => d.Name)
                    }),
                    ExpectedDate = p.ExpectedFinishDate
                    
                });

            return Ok(result);
        }

    }
}
