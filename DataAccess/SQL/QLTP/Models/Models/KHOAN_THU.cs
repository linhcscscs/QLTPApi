﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.SQL.QLTP.Models;

[Index("MA_SO_GD", "MA_TRUONG", "MA_CAP_HOC", "TEN", "MA_NAM_HOC", Name = "IX_KEY", IsUnique = true)]
[Index("MA_NAM_HOC", "MA_SO_GD", "MA_CAP_HOC", "MA_TRUONG", Name = "IX_NAM_SO_CAP_TRUONG")]
[Index("MA_NAM_HOC", "MA_SO_GD", "MA_TRUONG", Name = "IX_NAM_SO_TRUONG")]
public partial class KHOAN_THU
{
    [Key]
    public long ID { get; set; }

    [StringLength(50)]
    public string MA_SO_GD { get; set; } = null!;

    [StringLength(50)]
    public string MA_TRUONG { get; set; } = null!;

    [StringLength(50)]
    public string MA_CAP_HOC { get; set; } = null!;

    public int MA_NAM_HOC { get; set; }

    [StringLength(250)]
    public string? TEN { get; set; }

    public int DON_VI_TINH { get; set; }

    public int KY_THU { get; set; }

    public int? THOI_DIEM_THU_Q1 { get; set; }

    public int? THOI_DIEM_THU_Q2 { get; set; }

    public int? THOI_DIEM_THU_Q3 { get; set; }

    public int? THOI_DIEM_THU_Q4 { get; set; }

    public int? THOI_DIEM_THU_K1 { get; set; }

    public int? THOI_DIEM_THU_K2 { get; set; }

    public int? THOI_DIEM_THU_CN { get; set; }

    public int IS_BAT_BUOC { get; set; }

    public int IS_MIEN_GIAM { get; set; }

    public int IS_XUAT_HD { get; set; }

    public int IS_XUAT_CT { get; set; }

    public int IS_THU_NOI_BO { get; set; }

    public int IS_DIEM_DANH { get; set; }

    public int IS_KHAU_TRU { get; set; }

    public long? ID_NHOM_DIEM_DANH { get; set; }

    public long? ID_DM_KHOAN_THU { get; set; }

    public long? ID_DM_NHOM_KHOAN_THU { get; set; }

    public int TRANG_THAI { get; set; }

    [StringLength(150)]
    public string? NGUOI_TAO { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NGAY_TAO { get; set; }

    [StringLength(150)]
    public string? NGUOI_SUA { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NGAY_SUA { get; set; }

    public int IS_THU_HO_CHI_HO { get; set; }

    public int IS_THU_THOA_THUAN { get; set; }

    public int IS_THU_LIEN_KET { get; set; }

    public int IS_LAY_SL_DG_HS { get; set; }

    [StringLength(50)]
    public string? MA_DM_KHOAN_THU_SO { get; set; }

    public int? THU_TU { get; set; }

    [InverseProperty("ID_KHOAN_THUNavigation")]
    public virtual ICollection<DIEM_DANH_KHOAN_THU> DIEM_DANH_KHOAN_THU { get; set; } = new List<DIEM_DANH_KHOAN_THU>();

    [InverseProperty("ID_KHOAN_THUNavigation")]
    public virtual ICollection<DIEM_DANH_KHOAN_THU_KY> DIEM_DANH_KHOAN_THU_KY { get; set; } = new List<DIEM_DANH_KHOAN_THU_KY>();

    [InverseProperty("ID_KHOAN_THUNavigation")]
    public virtual ICollection<DIEM_DANH_KHOAN_THU_NAM> DIEM_DANH_KHOAN_THU_NAM { get; set; } = new List<DIEM_DANH_KHOAN_THU_NAM>();

    [InverseProperty("ID_KHOAN_THUNavigation")]
    public virtual ICollection<DK_KHOAN_THU_SL_DG> DK_KHOAN_THU_SL_DG { get; set; } = new List<DK_KHOAN_THU_SL_DG>();

    [InverseProperty("ID_KHOAN_THUNavigation")]
    public virtual ICollection<DK_KHOAN_THU_TN> DK_KHOAN_THU_TN { get; set; } = new List<DK_KHOAN_THU_TN>();

    [InverseProperty("ID_KHOAN_THUNavigation")]
    public virtual ICollection<DOT_THU_KHOAN_THU> DOT_THU_KHOAN_THU { get; set; } = new List<DOT_THU_KHOAN_THU>();

    [InverseProperty("ID_KHOAN_THUNavigation")]
    public virtual ICollection<KHOAN_THU_DOI_TUONG> KHOAN_THU_DOI_TUONG { get; set; } = new List<KHOAN_THU_DOI_TUONG>();

    [InverseProperty("ID_KHOAN_THUNavigation")]
    public virtual ICollection<KHOAN_THU_MIEN_GIAM> KHOAN_THU_MIEN_GIAM { get; set; } = new List<KHOAN_THU_MIEN_GIAM>();

    [InverseProperty("ID_KHOAN_THUNavigation")]
    public virtual ICollection<LICH_SU_THANH_TOAN> LICH_SU_THANH_TOAN { get; set; } = new List<LICH_SU_THANH_TOAN>();

    [InverseProperty("ID_KHOAN_THUNavigation")]
    public virtual ICollection<PHIEU_THU_CT> PHIEU_THU_CT { get; set; } = new List<PHIEU_THU_CT>();
}