using System.ComponentModel.DataAnnotations;

namespace tcc_web_api.Models {
    public class Task {
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Project Project { get; set; }
        public Team Team { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ExpectedDate { get; set; }
        public DateTime? StartedOn { get; set; }
        public DateTime? FinishedOn { get; set; }

        public Task() {
            CreatedOn = DateTime.UtcNow;
        }
    }
}
