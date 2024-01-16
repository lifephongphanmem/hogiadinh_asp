using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QLHN.Models.Systems
{
    public class GroupPermission
    {
        [Key]
        public int Id { get; set; }
        public string KeyLink { get; set; }
        public string GroupName { get; set; }
        public string MoTa { get; set; }
    }
}
