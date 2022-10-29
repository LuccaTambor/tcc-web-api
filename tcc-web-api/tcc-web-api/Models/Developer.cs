namespace tcc_web_api.Models {
    public class Developer : User {
        public string Function { get; set; }
        public ICollection<Team> Teams { get; set; }

        public Developer() : base() {
            Teams = new HashSet<Team>();
        }
    }
}
