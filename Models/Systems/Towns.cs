using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QLHN.Models.Systems
{
    public class Towns
    {
        [Key]
        public int Id { get; set; }

        public string MaTinh { get; set; }
        [Required(ErrorMessage = "Thông tin không được bỏ trống")]
        public string MaHuyen { get; set; }

        [Required(ErrorMessage = "Thông tin không được bỏ trống")]
        public string MaXa { get; set; }

        [Required(ErrorMessage = "Thông tin không được bỏ trống")]
        public string TenXa { get; set; }
    }
}
