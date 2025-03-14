﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.SQL.QLTP.Models;

[Index("MA_SO_GD", "ID_TRUONG", "MA_TRUONG", "MA_CAP_HOC", "MA_NAM_HOC", "ID_KHOAN_THU", "MA_KHOI", "MA_NHOM_TUOI_MN", "ID_LOP", "MA_LOP", "ID_HOC_SINH", "MA_HOC_SINH", Name = "IX_KEY", IsUnique = true)]
[Index("MA_NAM_HOC", "MA_SO_GD", "MA_CAP_HOC", "MA_TRUONG", Name = "IX_NAM_SO_CAP_TRUONG")]
[Index("MA_SO_GD", "MA_TRUONG", "MA_CAP_HOC", "MA_NAM_HOC", "MA_HOC_SINH", Name = "IX_NAM_SO_CAP_TRUONG_HOC_SINH")]
[Index("MA_SO_GD", "MA_TRUONG", "MA_CAP_HOC", "MA_NAM_HOC", "ID_HOC_SINH", Name = "IX_NAM_SO_CAP_TRUONG_ID_HOC_SINH")]
[Index("MA_NAM_HOC", "MA_SO_GD", "ID_TRUONG", Name = "IX_NAM_SO_ID_TRUONG")]
[Index("MA_NAM_HOC", "MA_SO_GD", "MA_TRUONG", Name = "IX_NAM_SO_TRUONG")]
public partial class KHOAN_THU_DOI_TUONG
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

    public long ID_KHOAN_THU { get; set; }

    [StringLength(50)]
    public string? MA_KHOI { get; set; }

    [StringLength(50)]
    public string? MA_NHOM_TUOI_MN { get; set; }

    [Column(TypeName = "numeric(18, 0)")]
    public decimal? ID_LOP { get; set; }

    [StringLength(50)]
    public string? MA_LOP { get; set; }

    [Column(TypeName = "numeric(18, 0)")]
    public decimal? ID_HOC_SINH { get; set; }

    [StringLength(50)]
    public string? MA_HOC_SINH { get; set; }

    [Column(TypeName = "numeric(18, 2)")]
    public decimal MUC_THU { get; set; }

    [StringLength(150)]
    public string? NGUOI_TAO { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NGAY_TAO { get; set; }

    [StringLength(150)]
    public string? NGUOI_SUA { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NGAY_SUA { get; set; }

    [ForeignKey("ID_HOC_SINH")]
    [InverseProperty("KHOAN_THU_DOI_TUONG")]
    public virtual HOC_SINH? ID_HOC_SINHNavigation { get; set; }

    [ForeignKey("ID_KHOAN_THU")]
    [InverseProperty("KHOAN_THU_DOI_TUONG")]
    public virtual KHOAN_THU ID_KHOAN_THUNavigation { get; set; } = null!;

    [ForeignKey("ID_LOP")]
    [InverseProperty("KHOAN_THU_DOI_TUONG")]
    public virtual LOP? ID_LOPNavigation { get; set; }

    [ForeignKey("ID_TRUONG")]
    [InverseProperty("KHOAN_THU_DOI_TUONG")]
    public virtual TRUONG ID_TRUONGNavigation { get; set; } = null!;

    [ForeignKey("MA_CAP_HOC")]
    [InverseProperty("KHOAN_THU_DOI_TUONG")]
    public virtual DM_CAP_HOC MA_CAP_HOCNavigation { get; set; } = null!;

    [ForeignKey("MA_NAM_HOC")]
    [InverseProperty("KHOAN_THU_DOI_TUONG")]
    public virtual NAM_HOC MA_NAM_HOCNavigation { get; set; } = null!;
}