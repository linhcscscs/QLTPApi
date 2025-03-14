﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.SQL.QLTP.Models;

public partial class THONG_BAO_DIEU_HANH
{
    [Key]
    public long ID { get; set; }

    [StringLength(250)]
    public string TIEU_DE { get; set; } = null!;

    [StringLength(250)]
    public string? URL_ANH { get; set; }

    public int? LOAI_THONG_BAO { get; set; }

    public int? CAP_THONG_BAO { get; set; }

    [StringLength(500)]
    public string? MO_TA { get; set; }

    public string? NOI_DUNG { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NGAY_BAT_DAU { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NGAY_KET_THUC { get; set; }

    public int? IS_GUI_EMAIL { get; set; }

    public int? LUOT_XEM { get; set; }

    public int? TRANG_THAI { get; set; }

    [StringLength(250)]
    public string? NGUOI_TAO { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NGAY_TAO { get; set; }

    [StringLength(250)]
    public string? NGUOI_SUA { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NGAY_SUA { get; set; }
}