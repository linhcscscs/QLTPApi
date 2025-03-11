namespace QLTPApi.Authentication.Models
{
    public class PrepareTokenModel
    {
        public decimal NGUOI_DUNG_ID { get; set; }
        public Guid? USER_VERSION { get; set; }
        public string APP_VERSION { get; set; } = "";
        public int MA_NAM_HOC { get; set; }
        public int HOC_KY { get; set; }
        public string MA_CAP_HOC { get; set; } = "";
        public string MA_SO_GD { get; set; } = "";
        public decimal ID_TRUONG { get; set; }
        public string MA_TRUONG { get; set; } = "";
        public bool REMEMBER { get; set; } = false;
    }
}
