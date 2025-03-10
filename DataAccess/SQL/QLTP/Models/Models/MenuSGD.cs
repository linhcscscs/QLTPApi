﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.SQL.QLTP.Models;

public partial class MenuSGD
{
    [Key]
    public long MenuSGDID { get; set; }

    public long? ParentID { get; set; }

    [StringLength(100)]
    public string? MenuSGDCode { get; set; }

    public int? LevelItem { get; set; }

    [StringLength(120)]
    public string MenuSGDName { get; set; } = null!;

    [StringLength(150)]
    public string? Icon { get; set; }

    [StringLength(150)]
    public string? Link { get; set; }

    public int? TypeHelp { get; set; }

    [StringLength(250)]
    public string? DesHelp { get; set; }

    [StringLength(150)]
    public string? LinkYoutube { get; set; }

    [StringLength(20)]
    public string? Order { get; set; }

    public int IsView { get; set; }

    public int Status { get; set; }

    [StringLength(120)]
    public string? CreateBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateAt { get; set; }

    [StringLength(120)]
    public string? UpdateBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateAt { get; set; }

    [StringLength(120)]
    public string? AuthBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? AuthAt { get; set; }

    [StringLength(120)]
    public string? MenuSGDNameEG { get; set; }

    [InverseProperty("MenuSGD")]
    public virtual ICollection<GroupUserSGDMenuSGD> GroupUserSGDMenuSGD { get; set; } = new List<GroupUserSGDMenuSGD>();
}