using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QLHN.Models.Systems
{
    public class Permissions
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Roles { get; set; }

        public bool Index { get; set; }

        public bool Create { get; set; }

        public bool Edit { get; set; }

        public bool Delete { get; set; }

        public bool Approve { get; set; }

        public string Status { get; set; }

        [NotMapped]
        public string MoTa { get; set; }
    }
}
