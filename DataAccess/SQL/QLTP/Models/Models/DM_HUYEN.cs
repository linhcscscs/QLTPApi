﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.SQL.QLTP.Models;

[Index("MA", "MA_NAM_HOC", Name = "IX_KEY", IsUnique = true)]
[Index("MA_NAM_HOC", Name = "IX_NAM_HOC")]
[Index("MA_NAM_HOC", "MA_TINH", Name = "IX_TINH_NAM_HOC")]
public partial class DM_HUYEN
{
    [Key]
    [Column(TypeName = "numeric(18, 0)")]
    public decimal ID { get; set; }

    [StringLength(20)]
    public string MA { get; set; } = null!;

    public int MA_NAM_HOC { get; set; }

    [StringLength(20)]
    public string MA_TINH { get; set; } = null!;

    [StringLength(250)]
    public string TEN { get; set; } = null!;

    public int? THU_TU { get; set; }

    [Column(TypeName = "numeric(18, 0)")]
    public decimal? NGUOI_TAO { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NGAY_TAO { get; set; }

    [Column(TypeName = "numeric(18, 0)")]
    public decimal? NGUOI_SUA { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NGAY_SUA { get; set; }

    [StringLength(50)]
    public string? CAP { get; set; }

    [Column(TypeName = "numeric(18, 0)")]
    public decimal ID_SOURCE { get; set; }

    [InverseProperty("ID_HUYENNavigation")]
    public virtual ICollection<DM_XA> DM_XA { get; set; } = new List<DM_XA>();

    [ForeignKey("MA_TINH")]
    [InverseProperty("DM_HUYEN")]
    public virtual DM_TINH MA_TINHNavigation { get; set; } = null!;
}