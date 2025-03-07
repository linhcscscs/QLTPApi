using AutoMapper;
using DataAccess.Caching.Interface;
using DataAccess.SQL.QLTP.Context;
using DataAccess.SQL.QLTP.Models;
using DataAccess.SQL.QLTP.Repository.BaseRepository;
using DataAccess.Values;
using Force.DeepCloner;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DataAccess.SQL.QLTP.Repository
{
    public interface INhanSuRepository : IQLTPRepository<NHAN_SU>
    {
        public NHAN_SU? getByMaBasic(QLTPConnection connection, int maNamHoc, string maTruong, string maNhanSu);
        public NHAN_SU? getThongTinHieuTruong(QLTPConnection connection, decimal idTruong, int maNamHoc, string maLoaiCanBo, string ma_cap_hoc);
        public List<NHAN_SU> GetListNhanSu(QLTPConnection connection, int maNamHoc, string maSoGD, string maPhongGD = "", string maTruong = "", string maCapHoc = "", string maNhanSu = "", string tenNhanSu = "");
        public List<NHAN_SU> GetListNhanSuByTrangThai(QLTPConnection connection, int maNamHoc, string maSoGD, string maPhongGD, string maTruong, string maCapHoc, string maNhanSu = "", string tenNhanSu = "");
    }
    public class NhanSuRepository : QLTPRepository<NHAN_SU>, INhanSuRepository
    {
        public NhanSuRepository(IQLTPContextFactory contextFactory, ICacheProvider cache, IMapper mapper, IServiceProvider serviceProvider) : base(contextFactory, cache, mapper, serviceProvider)
        {
        }
        #region Gets
        public NHAN_SU? getByMaBasic(QLTPConnection connection, int maNamHoc, string maTruong, string maNhanSu)
        {
            return _cache.GetByKey(
                getDataSource: () =>
                {
                    using (var context = _contextFactory.GetContext(connection))
                    {
                        return (from p in context.NHAN_SU
                                where p.MA_NAM_HOC == maNamHoc && p.MA_TRUONG == maTruong && p.MA == maNhanSu
                                select p).FirstOrDefault();
                    }
                },
                key: _cache.BuildCachedKey("NHAN_SU", "getByMaBasic", maNamHoc, maTruong, maNhanSu)
                );
        }

        public NHAN_SU? getThongTinHieuTruong(QLTPConnection connection, decimal idTruong, int maNamHoc, string maLoaiCanBo, string ma_cap_hoc)
        {
            return _cache.GetByKey(
                        getDataSource: () =>
                        {
                            using (var context = _contextFactory.GetContext(connection))
                            {
                                return (from ns in context.NHAN_SU
                                        where ns.ID_TRUONG == idTruong && ns.MA_NAM_HOC == maNamHoc
                                         && ns.MA_NHOM_CAN_BO == "01"
                                         && ((ns.IS_MN == null && ns.IS_GDTX == null && ns.IS_C1 == null && ns.IS_C2 == null && ns.IS_C3 == null)
                                             || (ma_cap_hoc == "01" && ns.IS_MN == 1)
                                             || (ma_cap_hoc == "02" && ns.IS_C1 == 1)
                                             || (ma_cap_hoc == "03" && ns.IS_C2 == 1)
                                             || (ma_cap_hoc == "04" && ns.IS_C3 == 1)
                                             || (ma_cap_hoc == "05" && ns.IS_GDTX == 1)
                                             || ns.MA_CAP_HOC_CHINH == ma_cap_hoc
                                             )
                                         && ns.MA_LOAI_CAN_BO == maLoaiCanBo
                                         && (ns.MA_TRANG_THAI_CAN_BO == "01" || ns.MA_TRANG_THAI_CAN_BO == "04" || ns.MA_TRANG_THAI_CAN_BO == "07")
                                        select ns).FirstOrDefault();
                            }
                        },
            key: _cache.BuildCachedKey("NHAN_SU", "getThongTinHieuTruong", idTruong, maNamHoc, maLoaiCanBo, ma_cap_hoc)
            );
        }
        public List<NHAN_SU> GetListNhanSu(QLTPConnection connection, int maNamHoc, string maSoGD, string maPhongGD = "", string maTruong = "", string maCapHoc = "", string maNhanSu = "", string tenNhanSu = "")
        {
            return _cache.GetByKey(
                    getDataSource: () =>
                    {
                        using (var context = _contextFactory.GetContext(connection))
                        {
                            //query
                            string strQuery = string.Format(@"SELECT ns.*
                                                        FROM dbo.NHAN_SU AS ns
                                                        WHERE ns.MA_SO_GD = @MA_SO_GD
                                                              AND ns.MA_NAM_HOC = @MA_NAM_HOC
                                                              AND ns.MA_TRUONG = @MA_TRUONG");
                            //add prams
                            List<object> lstParam = new List<object>();
                            lstParam.Add(new SqlParameter("@MA_SO_GD", maSoGD));
                            lstParam.Add(new SqlParameter("@MA_NAM_HOC", maNamHoc));
                            lstParam.Add(new SqlParameter("@MA_TRUONG", maTruong));
                            string is_cap_gv = string.Empty;
                            if (maCapHoc == SysCapHoc.MamNon)
                            {
                                is_cap_gv = " ns.IS_MN = 1";
                            }
                            else if (maCapHoc == SysCapHoc.C1)
                            {
                                is_cap_gv = " ns.IS_C1 = 1";
                            }
                            else if (maCapHoc == SysCapHoc.C2)
                            {
                                is_cap_gv = " ns.IS_C2 = 1";
                            }
                            else if (maCapHoc == SysCapHoc.C3)
                            {
                                is_cap_gv = " ns.IS_C3 = 1";
                            }
                            else if (maCapHoc == SysCapHoc.GDTX)
                            {
                                is_cap_gv = " ns.IS_GDTX = 1";
                            }
                            if (!string.IsNullOrEmpty(maCapHoc))
                            {
                                strQuery += string.Format(@"
                                                    and (case
                                                            when ns.MA_NHOM_CAN_BO is null then 1
                                                            when  ns.MA_CAP_HOC is null then 1
                                                            when {0} then 1
                                                            else 0 end) = 1", is_cap_gv);
                            }
                            if (!string.IsNullOrEmpty(maNhanSu))
                            {
                                strQuery += " AND ns.MA=@MA";
                                lstParam.Add(new SqlParameter("@MA", maNhanSu));
                            }

                            if (!string.IsNullOrEmpty(tenNhanSu))
                            {
                                strQuery += " AND UPPER(ns.HO_TEN) like N'%'+UPPER(@HO_TEN)+'%'";
                                lstParam.Add(new SqlParameter("@HO_TEN", tenNhanSu));
                            }
                            //order
                            strQuery += " order by ns.TEN,ns.HO_TEN asc";
                            //execute
                            return context.Database.SqlQueryRaw<NHAN_SU>(strQuery, lstParam.ToArray()).ToList();
                        }
                    },
                    key: _cache.BuildCachedKey("NHAN_SU", "GetListNhanSu", maNamHoc, maSoGD, maPhongGD, maTruong, maCapHoc, maNhanSu, tenNhanSu)
                    ) ?? new List<NHAN_SU>();
        }
        public List<NHAN_SU> GetListNhanSuByTrangThai(QLTPConnection connection, int maNamHoc, string maSoGD, string maPhongGD, string maTruong, string maCapHoc, string maNhanSu = "", string tenNhanSu = "")
        {
            return _cache.GetByKey(
                        getDataSource: () =>
                        {
                            using (var context = _contextFactory.GetContext(connection))
                            {
                                //query
                                string strQuery = string.Format(@"SELECT ns.*
                                                        FROM dbo.NHAN_SU AS ns
                                                        LEFT JOIN DM_NHOM_CAN_BO ncb ON ncb.MA = ns.MA_NHOM_CAN_BO
                                                        WHERE ns.MA_SO_GD = @MA_SO_GD
                                                            AND ns.MA_NAM_HOC = @MA_NAM_HOC
                                                            AND ns.MA_TRUONG = @MA_TRUONG 
                                                            AND ns.MA_TRANG_THAI_CAN_BO in ('01', '02', '04', '07') 
                                                            AND ncb.MA IN ('01', '03', '06')");
                                //add prams
                                List<object> lstParam = new List<object>();
                                lstParam.Add(new SqlParameter("@MA_SO_GD", maSoGD));
                                lstParam.Add(new SqlParameter("@MA_NAM_HOC", maNamHoc));
                                lstParam.Add(new SqlParameter("@MA_TRUONG", maTruong));
                                string is_cap_gv = string.Empty;
                                if (maCapHoc == SysCapHoc.MamNon)
                                {
                                    is_cap_gv = " ns.IS_MN = 1";
                                }
                                else if (maCapHoc == SysCapHoc.C1)
                                {
                                    is_cap_gv = " ns.IS_C1 = 1";
                                }
                                else if (maCapHoc == SysCapHoc.C2)
                                {
                                    is_cap_gv = " ns.IS_C2 = 1";
                                }
                                else if (maCapHoc == SysCapHoc.C3)
                                {
                                    is_cap_gv = " ns.IS_C3 = 1";
                                }
                                else if (maCapHoc == SysCapHoc.GDTX)
                                {
                                    is_cap_gv = " ns.IS_GDTX = 1";
                                }
                                if (!string.IsNullOrEmpty(maCapHoc))
                                {
                                    strQuery += string.Format(@"
                                                    and (case
                                                            when ns.MA_NHOM_CAN_BO is null then 1
                                                            when  ns.MA_CAP_HOC is null then 1
                                                            when {0} then 1
                                                            else 0 end) = 1", is_cap_gv);
                                }
                                if (!string.IsNullOrEmpty(maNhanSu))
                                {
                                    strQuery += " AND ns.MA=@MA";
                                    lstParam.Add(new SqlParameter("@MA", maNhanSu));
                                }

                                if (!string.IsNullOrEmpty(tenNhanSu))
                                {
                                    strQuery += " AND UPPER(ns.HO_TEN) like N'%'+UPPER(@HO_TEN)+'%'";
                                    lstParam.Add(new SqlParameter("@HO_TEN", tenNhanSu));
                                }
                                //order
                                strQuery += " order by ns.TEN,ns.HO_TEN asc";
                                //execute
                                return context.Database.SqlQueryRaw<NHAN_SU>(strQuery, lstParam.ToArray()).ToList();
                            }
                        },
                        key: _cache.BuildCachedKey("NHAN_SU", "DM_NHOM_CAN_BO", "GetListNhanSuByTrangThai", maNamHoc, maSoGD, maPhongGD, maTruong, maCapHoc, maNhanSu, tenNhanSu)
                        ) ?? new List<NHAN_SU>();
        }
        #endregion
    }
}
