using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WareHouseSynch.Domain.DataModel;

[Table("Dim_Corporation")]
[Index("CorporationId", "CorpDbaname", "CorpLegalName", "Urlkey", Name = "Composite")]
public partial class DimCorporation
{
    [Key]
    public long CorpKey { get; set; }

    [Required]
    [Column("CorpDBAName")]
    [StringLength(250)]
    [Unicode(false)]
    public string CorpDbaname { get; set; }

    [Required]
    [StringLength(250)]
    [Unicode(false)]
    public string CorpLegalName { get; set; }

    [Precision(0)]
    public DateTime? PeriodFrom { get; set; }

    [Precision(0)]
    public DateTime? PeriodTo { get; set; }

    public long? ClientKey { get; set; }

    [Column("ISPCEnabled")]
    public bool Ispcenabled { get; set; }

    [Required]
    [Column("CorporationID")]
    [MaxLength(18)]
    public byte[] CorporationId { get; set; }

    public short? Status { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string LoadedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LoadedDate { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string UpdatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdatedDate { get; set; }

    [Column("URLKey")]
    public long Urlkey { get; set; }

    public int? Order { get; set; }

    public byte? PropertyType { get; set; }

    [Column("ClientID")]
    [MaxLength(18)]
    public byte[] ClientId { get; set; }

    public int? BrandKey { get; set; }
}
