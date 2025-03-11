using DataAccess.Helper.ConfigHelper;

namespace QLTPApi
{
    public static class LocalApi
    {
        public static int GetYearNow()
        {
            #region Nhận dạng năm học, học kỳ
            try
            {
                int verion = ConfigHelper.AppSettings.NAM_HOC;
                return verion;
            }
            catch { }
            return 2021;

            #endregion
        }
        public static int GetKyNow()
        {
            #region Nhận dạng năm học, học kỳ

            int ky = 1;
            int mon = DateTime.Now.Month;
            int dky2 = ConfigHelper.AppSettings.ThangDauKy2,
                cky2 = ConfigHelper.AppSettings.ThangCuoiKy2;
            if (mon >= dky2 && mon <= cky2) ky = 2;
            else ky = 1;
            return ky;

            #endregion
        }
    }
}
