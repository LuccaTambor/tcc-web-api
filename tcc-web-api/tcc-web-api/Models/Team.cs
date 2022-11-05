using System.ComponentModel.DataAnnotations;

namespace tcc_web_api.Models {
    public class Team {
        [Key]
        public int Id { get; set; }
        public string TeamName { get; set; }
        public ICollection<Developer> Developers { get; set; }
        public ICollection<Task> Tasks { get; set; }

        public Team() {
            Developers =  new HashSet<Developer>();
        }
    }
}
