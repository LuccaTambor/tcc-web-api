using System.ComponentModel.DataAnnotations;

namespace tcc_web_api.Models {
    public class User : UserAuthentication {
        public string Name { get; set; }
        public string Document { get; set; }
        public DateTime CreatedOn { get; set; }

        public User () {
            CreatedOn = DateTime.Now;
        }
    }
}
