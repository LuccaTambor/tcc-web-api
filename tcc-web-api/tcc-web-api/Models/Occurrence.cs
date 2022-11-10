using System.ComponentModel.DataAnnotations;
using tcc_web_api.Models.Enums;

namespace tcc_web_api.Models {
    public class Occurrence {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public Developer Developer { get; set; }
        public Project Project { get; set; }
        public Team Team { get; set; }
        public OccurrenceType OccurrenceType { get; set; }

        public Occurrence() {
            CreatedOn = DateTime.UtcNow;
        }
        
    }
}
