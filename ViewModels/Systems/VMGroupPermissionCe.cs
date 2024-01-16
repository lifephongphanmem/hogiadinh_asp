using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using QLHN.Models.Systems;

namespace QLHN.ViewModels.Systems
{
    public class VMGroupPermissionCe
    {
        public int Id { get; set; }
        public string KeyLink { get; set; }
        [Required(ErrorMessage = "Thông tin không được bỏ trống")]
        public string GroupName { get; set; }
        public string MoTa { get; set; }
        public List<Permissions> Permissions { get; set; }
    }
}
