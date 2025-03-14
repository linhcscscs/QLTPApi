﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.SQL.QLTP.Models;

public partial class TAI_KHOAN_GUI_TIN
{
    [Key]
    public long ID { get; set; }

    [StringLength(50)]
    public string MA_SO_GD { get; set; } = null!;

    [StringLength(50)]
    public string? MA_PHONG_GD { get; set; }

    [StringLength(50)]
    public string? MA_TRUONG { get; set; }

    [StringLength(50)]
    public string MA_CAP_DON_VI { get; set; } = null!;

    [StringLength(50)]
    public string TAI_KHOAN { get; set; } = null!;

    [StringLength(250)]
    public string MAT_KHAU { get; set; } = null!;

    public int TRANG_THAI { get; set; }

    [StringLength(500)]
    public string? MO_TA { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NGAY_BAT_DAU { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NGAY_KET_THUC { get; set; }

    public int IS_CAP_MN { get; set; }

    public int IS_CAP_TH { get; set; }

    public int IS_CAP_THCS { get; set; }

    public int IS_CAP_THPT { get; set; }

    public int IS_CAP_GDTX { get; set; }

    [StringLength(500)]
    public string IP_ACCESS { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? NGAY_TAO { get; set; }

    [StringLength(150)]
    public string? NGUOI_TAO { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NGAY_SUA { get; set; }

    [StringLength(150)]
    public string? NGUOI_SUA { get; set; }
}