using System.ComponentModel.DataAnnotations;

namespace tcc_web_api.Models {
    public class User : UserAuthentication{
        public string Name { get; set; }
    }
}
