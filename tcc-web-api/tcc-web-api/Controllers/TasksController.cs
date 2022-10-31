using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using tcc_web_api.Data;
using tcc_web_api.Models;
using Task = tcc_web_api.Models.Task;

namespace tcc_web_api.Controllers {
    [Route("api/tasks")]
    [ApiController]
    public class TasksController : ControllerBase {
        private TCCDbContext _context;

        public TasksController(TCCDbContext context) {
            _context = context;
        }

        [HttpGet]
        [Route("getOnGoingTask")]
        public IActionResult GetOnGoingTasks(int teamId) {
            var onGoingTasks = _context.Tasks.Include(t => t.Team).Where(t => t.Team.Id == teamId).Where(t => t.StartedOn.HasValue && !t.FinishedOn.HasValue)
                .Select(t => new {
                    t.Id,
                    t.Description,
                    t.StartedOn,
                    t.ExpectedDate,
                    t.Code,
                    t.Title
                });

            return Ok(onGoingTasks);
        }

        [HttpPost]
        [Route("createNewTask")]
        public IActionResult CreateTask([FromBody] TaskModel task, int teamId) {
            if(task == null) {
                return BadRequest();
            }
      

            Team team = _context.Teams.FirstOrDefault(t => t.Id == teamId);

            if(team == null) {
                return NotFound();
            }

            Task newTask = new Task {
                Code = task.Code,
                Description = task.Description,
                ExpectedDate = task.ExpectedDate,
                Title = task.Title,
            };

            newTask.Team = team;
            newTask.Project = _context.Projects.FirstOrDefault(p => p.Teams.Any(t => t.Id == teamId));

            _context.Tasks.Add(newTask);
            _context.SaveChanges();

            return Ok();

        }

        [HttpPut]
        [Route("markAsFinished")]
        public IActionResult MarkTaskAsFinished(int taskId) {

            var task = _context.Tasks.FirstOrDefault(t => t.Id == taskId);

            if(task == null)
                return NotFound();
            
            task.FinishedOn = DateTime.UtcNow;
            _context.SaveChanges();

            var onGoingTasks = _context.Tasks.Include(t => t.Team).Where(t => t.Team.Id == task.Team.Id).Where(t => t.StartedOn.HasValue && !t.FinishedOn.HasValue)
                .Select(t => new {
                    t.Id,
                    t.Description,
                    t.StartedOn,
                    t.ExpectedDate,
                    t.Code,
                    t.Title
                });

            return Ok(onGoingTasks);
        }


        public class TaskModel {
            public string Code { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public DateTime ExpectedDate { get; set; }
        }
    }
}
