namespace QLTPApi.Authentication.Models
{
    public class RefreshToken
    {
        public string REFRESH_TOKEN { get; set; }
        public int NGUOI_DUNG_ID { get; set; }
        public string APP_VERSION { get; set; }
        public DateTime EXPERIED_TIME { get; set; }
    }
}
