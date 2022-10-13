namespace tcc_web_api.Models {
    public class Manager : User {
        public string Designation { get; set; }
        public ICollection<Project> Projects { get; set; }
        public Manager() : base () {
            Projects = new HashSet<Project>();
        }
    }
}
