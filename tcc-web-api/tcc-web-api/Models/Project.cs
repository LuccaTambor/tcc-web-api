﻿using System.ComponentModel.DataAnnotations;

namespace tcc_web_api.Models {
    public class Project {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? StartedOn { get; set; }
        public DateTime? ExpectedFinishDate { get; set; }

        public Project() { 
            CreatedOn = DateTime.Now;
        }
    }
}
