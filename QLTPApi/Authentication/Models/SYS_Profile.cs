using DataAccess.Helper.ConfigHelper;

namespace QLTPApi.Authentication.Models
{
    public class SYS_Profile
    {
        public bool IsAuthenticated { get; set; } = false;
        public decimal? NGUOI_DUNG_ID { get; set; } = null;
        public Guid? USER_VERSION { get; set; } = null;
        public int SysTem_Nam_Hoc { get; set; } = ConfigHelper.AppSettings.NAM_HOC;
        public int SysTem_Hoc_Ky { get; set; } = LocalApi.GetKyNow();
        public string SysTem_Cap_Hoc { get; set; } = "";
        public string SysTem_Ma_So_GD { get; set; } = "";
        public decimal? SysTem_ID_Truong { get; set; } = null;
        public string SysTem_Ma_Truong { get; set; } = "";
    }
}
