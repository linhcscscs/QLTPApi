﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.SQL.QLTP.Models;

[Index("MA_SO_GD", "MA_NAM_HOC", "MA_BC", Name = "IX_KEY", IsUnique = true)]
public partial class BCS_BIEU_MAU_BC
{
    [Key]
    public long ID { get; set; }

    [StringLength(50)]
    public string MA_SO_GD { get; set; } = null!;

    public int MA_NAM_HOC { get; set; }

    [StringLength(50)]
    public string MA_BC { get; set; } = null!;

    [StringLength(250)]
    public string TEN_BC { get; set; } = null!;

    public int? THU_TU { get; set; }

    public int IS_CAP_MN { get; set; }

    public int IS_CAP_TH { get; set; }

    public int IS_CAP_THCS { get; set; }

    public int IS_CAP_THPT { get; set; }

    public int IS_CAP_GDTX { get; set; }

    public int TRANG_THAI { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NGAY_TAO { get; set; }

    [StringLength(150)]
    public string? NGUOI_TAO { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NGAY_SUA { get; set; }

    [StringLength(150)]
    public string? NGUOI_SUA { get; set; }
}