using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QLHN.Models.Systems
{
    public class RolesAction
    {
        [Key]
        public int Id { get; set; }
        public string Roles { get; set; }
        public string MoTa { get; set; }
        public string GhiChu { get; set; }
    }
}
