using System.ComponentModel.DataAnnotations;

namespace QLTPApi.Authentication.Models
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "User Name is required")]
        public string USERNAME { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string PASSWORD { get; set; }
        [Required(ErrorMessage = "ma_tinh is required")]
        public string MA_SO { get; set; }
        [Required(ErrorMessage = "ma_huyen is required")]
        public string MA_PHONG { get; set; }
        [Required(ErrorMessage = "ma_xa is required")]
        public string MA_TRUONG { get; set; }
        public bool REMEMBER { get; set; } = false;
    }
}
