﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.SQL.QLTP.Models;

public partial class CH_HS
{
    [Key]
    public int ID { get; set; }

    [StringLength(50)]
    public string? TEN_COT { get; set; }

    [StringLength(50)]
    public string? TEN_COT_HIEN_THI { get; set; }

    public int? LOAI_DU_LIEU { get; set; }

    [StringLength(50)]
    public string? KIEU_DU_LIEU { get; set; }

    [StringLength(50)]
    public string? MIN_VALUE { get; set; }

    [StringLength(50)]
    public string? MAX_VALUE { get; set; }

    [StringLength(500)]
    public string? GIA_TRI { get; set; }

    [InverseProperty("ID_CH_HSNavigation")]
    public virtual ICollection<MAP_MIEN_GIAM_HS> MAP_MIEN_GIAM_HS { get; set; } = new List<MAP_MIEN_GIAM_HS>();

    [InverseProperty("ID_CH_HSNavigation")]
    public virtual ICollection<MAP_MIEN_GIAM_HS_SO> MAP_MIEN_GIAM_HS_SO { get; set; } = new List<MAP_MIEN_GIAM_HS_SO>();
}