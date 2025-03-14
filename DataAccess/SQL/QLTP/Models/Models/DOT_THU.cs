﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.SQL.QLTP.Models;

[Index("MA_SO_GD", "ID_TRUONG", "MA_TRUONG", "MA_CAP_HOC", "MA_NAM_HOC", "HOC_KY", "THANG", "NAM", "TEN", Name = "IX_KEY", IsUnique = true)]
[Index("MA_NAM_HOC", "MA_SO_GD", "MA_CAP_HOC", "MA_TRUONG", Name = "IX_NAM_SO_CAP_TRUONG")]
[Index("MA_NAM_HOC", "MA_SO_GD", "ID_TRUONG", Name = "IX_NAM_SO_ID_TRUONG")]
[Index("MA_NAM_HOC", "MA_SO_GD", "MA_TRUONG", Name = "IX_NAM_SO_TRUONG")]
public partial class DOT_THU
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

    public int HOC_KY { get; set; }

    public int THANG { get; set; }

    public int NAM { get; set; }

    [StringLength(250)]
    public string TEN { get; set; } = null!;

    public int IS_KHAU_TRU { get; set; }

    public int IS_TRA_TUNG_PHAN { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NGAY_BAT_DAU { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NGAY_KET_THUC { get; set; }

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

    [Column(TypeName = "datetime")]
    public DateTime? NGAY_KET_THUC_TTOL { get; set; }

    [InverseProperty("ID_DOT_THUNavigation")]
    public virtual ICollection<DOT_THU_DOI_TUONG> DOT_THU_DOI_TUONG { get; set; } = new List<DOT_THU_DOI_TUONG>();

    [InverseProperty("ID_DOT_THUNavigation")]
    public virtual ICollection<DOT_THU_KHOAN_THU> DOT_THU_KHOAN_THU { get; set; } = new List<DOT_THU_KHOAN_THU>();

    [ForeignKey("ID_TRUONG")]
    [InverseProperty("DOT_THU")]
    public virtual TRUONG ID_TRUONGNavigation { get; set; } = null!;

    [InverseProperty("ID_DOT_THUNavigation")]
    public virtual ICollection<LICH_SU_THANH_TOAN> LICH_SU_THANH_TOAN { get; set; } = new List<LICH_SU_THANH_TOAN>();

    [ForeignKey("MA_CAP_HOC")]
    [InverseProperty("DOT_THU")]
    public virtual DM_CAP_HOC MA_CAP_HOCNavigation { get; set; } = null!;

    [ForeignKey("MA_NAM_HOC")]
    [InverseProperty("DOT_THU")]
    public virtual NAM_HOC MA_NAM_HOCNavigation { get; set; } = null!;
}