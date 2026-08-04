using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WareHouseSynch.Domain.DataModel;

[Table("DimCorporationStaging")]
public partial class DimCorporationStaging
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

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

    public byte? HotelChain { get; set; }

    [Precision(0)]
    public DateTime? LoadTimeStamp { get; set; }

    public int LoadStatus { get; set; }
}
