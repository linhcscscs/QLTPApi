namespace QLTPApi.Authentication.Models
{
    public class AccessToken
    {
        public string ACCESS_TOKEN { get; set; }
        public int NGUOI_DUNG_ID { get; set; }
        public int MA_NAM_HOC { get; set; }
        public string MA_SO_GD { get; set; }
        public string MA_PHONG_GD { get; set; }
        public string MA_TRUONG { get; set; }
        public string TEN_HIEN_THI { get; set; }
        public bool IS_ROOT { get; set; }
        public bool IS_ROOT_SYS { get; set; }
        public Guid? USER_VERSION { get; set; }
        public string APP_VERSION { get; set; }
        public DateTime EXPERIED_TIME { get; set; }
    }
}
