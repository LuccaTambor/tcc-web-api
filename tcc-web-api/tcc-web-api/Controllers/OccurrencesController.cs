using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tcc_web_api.Data;
using tcc_web_api.Models;
using tcc_web_api.Models.Enums;
using tcc_web_api.Utils;

namespace tcc_web_api.Controllers {
    [Route("api/occurrences")]
    [ApiController]
    public class OccurrencesController : ControllerBase {
        private TCCDbContext _context;

        public OccurrencesController(TCCDbContext context) {
            _context = context;
        }

        [HttpGet]
        [Route("getOccurrences")]
        public IActionResult GetOccurrences(int projId) {
            var project = _context.Projects.FirstOrDefault(p => p.Id == projId);

            if(project == null) 
                return BadRequest("Projeto não existe.");

            var result = _context.Occurrences.Where(o => o.Project.Id == projId)
                .Select(o => new {
                    o.Id,
                    o.OccurrenceType,
                    TypeText = o.OccurrenceType.GetString(),
                    Developer = o.Developer.Name,
                    o.CreatedOn,
                    Date = o.CreatedOn.ToString("dd/MM/yyyy"),
                    o.Description
                }).OrderByDescending(x => x.CreatedOn);

            return Ok(result);
        }

        [HttpGet]
        [Route("getOccurrencesDev")]
        public IActionResult GetOccurrencesFromDev(int projId, string devId) {
            var project = _context.Projects.FirstOrDefault(p => p.Id == projId);
            var dev = _context.Developers.FirstOrDefault(d => d.Id == devId);

            if(project == null)
                return BadRequest("Projeto não existe.");

            if(dev == null)
                return BadRequest("Usuário não existe");

            var result = _context.Occurrences.Where(o => o.Project.Id == projId).Where(o => o.Developer.Id == devId)
               .Select(o => new {
                   o.Id,
                   o.OccurrenceType,
                   TypeText = o.OccurrenceType.GetString(),
                   o.CreatedOn,
                   Date = o.CreatedOn.ToString("dd/MM/yyyy"),
                   o.Description
               }).OrderByDescending(x => x.CreatedOn);

            return Ok(result);
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
