using System.ComponentModel.DataAnnotations;

namespace QLTPApi.Authentication.Models
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "USERNAME is required")]
        public string USERNAME { get; set; }

        [Required(ErrorMessage = "PASSWORD is required")]
        public string PASSWORD { get; set; }
        [Required(ErrorMessage = "MA_SO_GD is required")]
        public string MA_SO_GD { get; set; }
        public string MA_PHONG_GD { get; set; }
        [Required(ErrorMessage = "MA_CAP_HOC is required")]
        public string MA_CAP_HOC { get; set; }
        [Required(ErrorMessage = "MA_TRUONG is required")]
        public string MA_TRUONG { get; set; }
        public bool REMEMBER { get; set; } = false;
    }
}
