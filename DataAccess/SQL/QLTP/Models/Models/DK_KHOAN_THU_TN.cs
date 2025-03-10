﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.SQL.QLTP.Models;

[Index("MA_SO_GD", "ID_TRUONG", "MA_TRUONG", "MA_CAP_HOC", "MA_NAM_HOC", "MA_KHOI", "ID_HOC_SINH", "MA_HOC_SINH", "ID_KHOAN_THU", Name = "IX_KEY", IsUnique = true)]
[Index("MA_NAM_HOC", "MA_SO_GD", "MA_CAP_HOC", "MA_TRUONG", Name = "IX_NAM_SO_CAP_TRUONG")]
[Index("MA_SO_GD", "MA_TRUONG", "MA_CAP_HOC", "MA_NAM_HOC", "MA_HOC_SINH", Name = "IX_NAM_SO_CAP_TRUONG_HOC_SINH")]
[Index("MA_SO_GD", "MA_TRUONG", "MA_CAP_HOC", "MA_NAM_HOC", "ID_HOC_SINH", Name = "IX_NAM_SO_CAP_TRUONG_ID_HOC_SINH")]
[Index("MA_NAM_HOC", "MA_SO_GD", "ID_TRUONG", Name = "IX_NAM_SO_ID_TRUONG")]
[Index("MA_NAM_HOC", "MA_SO_GD", "MA_TRUONG", Name = "IX_NAM_SO_TRUONG")]
public partial class DK_KHOAN_THU_TN
{
    [Key]
    public long ID { get; set; }

    [StringLength(50)]
    public string MA_SO_GD { get; set; } = null!;

    [Column(TypeName = "numeric(18, 0)")]
    public decimal ID_TRUONG { get; set; }

    [StringLength(50)]
    public string MA_TRUONG { get; set; } = null!;

    [StringLength(50)]
    public string MA_CAP_HOC { get; set; } = null!;

    public int MA_NAM_HOC { get; set; }

    [StringLength(50)]
    public string MA_KHOI { get; set; } = null!;

    [StringLength(50)]
    public string? MA_NHOM_TUOI_MN { get; set; }

    [Column(TypeName = "numeric(18, 0)")]
    public decimal ID_LOP { get; set; }

    [StringLength(50)]
    public string MA_LOP { get; set; } = null!;

    [Column(TypeName = "numeric(18, 0)")]
    public decimal ID_HOC_SINH { get; set; }

    [StringLength(50)]
    public string MA_HOC_SINH { get; set; } = null!;

    public long ID_KHOAN_THU { get; set; }

    public int DK_NAM { get; set; }

    public int DK_K1 { get; set; }

    public int DK_K2 { get; set; }

    public int DK_T1 { get; set; }

    public int DK_T2 { get; set; }

    public int DK_T3 { get; set; }

    public int DK_T4 { get; set; }

    public int DK_T5 { get; set; }

    public int DK_T6 { get; set; }

    public int DK_T7 { get; set; }

    public int DK_T8 { get; set; }

    public int DK_T9 { get; set; }

    public int DK_T10 { get; set; }

    public int DK_T11 { get; set; }

    public int DK_T12 { get; set; }

    [StringLength(250)]
    public string? GHI_CHU { get; set; }

    [StringLength(150)]
    public string? NGUOI_TAO { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NGAY_TAO { get; set; }

    [StringLength(150)]
    public string? NGUOI_SUA { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NGAY_SUA { get; set; }

    [ForeignKey("ID_HOC_SINH")]
    [InverseProperty("DK_KHOAN_THU_TN")]
    public virtual HOC_SINH ID_HOC_SINHNavigation { get; set; } = null!;

    [ForeignKey("ID_KHOAN_THU")]
    [InverseProperty("DK_KHOAN_THU_TN")]
    public virtual KHOAN_THU ID_KHOAN_THUNavigation { get; set; } = null!;

    [ForeignKey("ID_LOP")]
    [InverseProperty("DK_KHOAN_THU_TN")]
    public virtual LOP ID_LOPNavigation { get; set; } = null!;

    [ForeignKey("ID_TRUONG")]
    [InverseProperty("DK_KHOAN_THU_TN")]
    public virtual TRUONG ID_TRUONGNavigation { get; set; } = null!;

    [ForeignKey("MA_CAP_HOC")]
    [InverseProperty("DK_KHOAN_THU_TN")]
    public virtual DM_CAP_HOC MA_CAP_HOCNavigation { get; set; } = null!;

    [ForeignKey("MA_NAM_HOC")]
    [InverseProperty("DK_KHOAN_THU_TN")]
    public virtual NAM_HOC MA_NAM_HOCNavigation { get; set; } = null!;
}