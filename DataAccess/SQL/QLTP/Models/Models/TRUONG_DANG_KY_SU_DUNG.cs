﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.SQL.QLTP.Models;

[Index("MA_NAM_HOC", "MA_SO_GD", "ID_TRUONG", Name = "IX_NAM_SO_ID_TRUONG")]
[Index("MA_NAM_HOC", "MA_SO_GD", "MA_TRUONG", Name = "IX_NAM_SO_TRUONG")]
[Index("MA_TRUONG", "MA_NAM_HOC", Name = "IX_TRUONG_DANG_KY_SU_DUNG_UNIQUE", IsUnique = true)]
public partial class TRUONG_DANG_KY_SU_DUNG
{
    [Key]
    public long ID { get; set; }

    [StringLength(50)]
    public string? MA_SO_GD { get; set; }

    [Column(TypeName = "numeric(18, 0)")]
    public decimal? ID_PHONG_GD { get; set; }

    [StringLength(20)]
    public string? MA_PHONG_GD { get; set; }

    [Column(TypeName = "numeric(18, 0)")]
    public decimal ID_TRUONG { get; set; }

    [StringLength(50)]
    public string MA_TRUONG { get; set; } = null!;

    public int MA_NAM_HOC { get; set; }

    public int IS_THU_PHI { get; set; }

    public int? IS_HOA_DON_DIEN_TU { get; set; }

    [StringLength(50)]
    public string? MA_SO_THUE { get; set; }

    [StringLength(50)]
    public string? MA_BAO_MAT { get; set; }

    [StringLength(50)]
    public string? KY_HIEU { get; set; }

    [StringLength(50)]
    public string? MA_LOAI_HOA_DON { get; set; }

    [StringLength(50)]
    public string? MA_MAU_HOA_DON { get; set; }

    [StringLength(50)]
    public string? NGUOI_TAO { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NGAY_TAO { get; set; }

    [StringLength(50)]
    public string? NGUOI_SUA { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NGAY_SUA { get; set; }

    [InverseProperty("ID_TRUONG_DANG_KY_SU_DUNGNavigation")]
    public virtual ICollection<TRUONG_DOI_TAC_THANH_TOAN> TRUONG_DOI_TAC_THANH_TOAN { get; set; } = new List<TRUONG_DOI_TAC_THANH_TOAN>();
}