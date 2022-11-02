using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tcc_web_api.Data;
using tcc_web_api.Models;
using tcc_web_api.Models.Enums;

namespace tcc_web_api.Controllers {
    [Route("api/occurrences")]
    [ApiController]
    public class OccurrencesController : ControllerBase {
        private TCCDbContext _context;

        public OccurrencesController(TCCDbContext context) {
            _context = context;
        }

        [HttpPost]
        [Route("createOccurrence")]
        public IActionResult CreateOccurrence([FromBody] OccurrenceDataModel occurrenceModel) {
            if(occurrenceModel == null)
                return BadRequest();

            var project = _context.Projects.FirstOrDefault(p => p.Teams.Any(t => t.Id == occurrenceModel.TeamId));
            var developer = _context.Developers.FirstOrDefault(d => d.Id == occurrenceModel.DeveloperId);

            if( project == null|| developer == null)
                return BadRequest();

            Occurrence newOccurrence = new Occurrence {
                Description = occurrenceModel.Description,
                OccurrenceType = occurrenceModel.OccurrenceType,
                Project = project,
                Developer = developer
            };

            _context.Occurrences.Add(newOccurrence);
            _context.SaveChanges();

            return Ok();
        }

        public class OccurrenceDataModel {
            public string Description { get; set; }
            public OccurrenceType OccurrenceType { get; set; }
            public int TeamId { get; set; }
            public string DeveloperId { get; set; }
        }
    }
}
