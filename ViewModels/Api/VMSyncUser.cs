using System.ComponentModel.DataAnnotations;

namespace QLHN.ViewModels.Api
{
    public class VMSyncUser
    {
        [Required(ErrorMessage ="Tài Khoản tổng hợp không được để trống")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Mật khẩu tài khoản tổng hợp không được để trống")]
        public string Password { get; set; }
        public string UserNCC { get; set; }
        [Required(ErrorMessage = "Mật khẩu hiện tại không được để trống")]
        public string PasswordNCC { get; set; }
    }
}
