using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;

namespace QLHN.ViewModels.Api
{
    public class VMUserCheckSync
    {
        [Key]
        public int Id { get; set; }       
        public string Username { get; set; }        
        public string Password { get; set; }        
        public string Name { get; set; }        
        public string Email { get; set; }       
        public string Status { get; set; }        
        public string Level { get; set; }
        public bool Sadmin { get; set; }      
        public string Avatar { get; set; }
        [NotMapped]
        public IFormFile AvatarFile { get; set; }
        [NotMapped]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public int CountLogin { get; set; }
        public DateTime ExpiryTime { get; set; }
        [Required(ErrorMessage = "Thông tin không được bỏ trống")]
        public string MaTinh { get; set; }
        public string NccUserName { get; set; }
        public string ViecLamUserName { get; set; }
    }
}
