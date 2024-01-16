using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QLHN.Models.Systems
{
    public class Users
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Thông tin không được bỏ trống")]
        [RegularExpression(@"^(?=.{5,32}$)(?!.*[._-]{2})(?!.*[0-9]{5,})[a-z](?:[\w]*|[a-z\d\.]*|[a-z\d-]*)[a-z0-9]$"
            , ErrorMessage = "Username không có ký tự đặc biệt, độ dài ít nhất 5 và lớn nhất 32 ký tự")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Thông tin không được bỏ trống")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Thông tin không được bỏ trống")]
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string MaTinh { get; set; }
        public string MaHuyen { get; set; }
        public string MaXa { get; set; }

        [StringLength(10)]
        public string Status { get; set; }

        [StringLength(1)]
        public string Level { get; set; }

        public bool Sadmin { get; set; }

        [Display(Name = "Avatar")]
        public string Avatar { get; set; }

        [NotMapped]
        public IFormFile AvatarFile { get; set; }

        [NotMapped]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public string Group { get; set; }

        public string TenDonVi { get; set; }
        public string TenDonViChuQuan { get; set; }
        public string TenBoPhan { get; set; }
        public string MaDonViSDNS { get; set; }
        public string DiaDanh { get; set; }

        public string Theme { get; set; }
        public string Menu { get; set; }
        public string Content { get; set; }

        public int CountLogin { get; set; }
        
    }
}
