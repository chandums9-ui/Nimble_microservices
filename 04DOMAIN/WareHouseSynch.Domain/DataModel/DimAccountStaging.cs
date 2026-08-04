using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WareHouseSynch.Domain.DataModel;

[Table("DimAccountStaging")]
public partial class DimAccountStaging
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string AccountName { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string AccountNumber { get; set; }

    [Required]
    [Column("CorporationID")]
    [MaxLength(18)]
    public byte[] CorporationId { get; set; }

    [Required]
    [Column("AccountID")]
    [MaxLength(18)]
    public byte[] AccountId { get; set; }

    public long? IncDeptKey { get; set; }

    public long? IncGroupKey { get; set; }

    public long? CustomizeKey { get; set; }

    public byte? Status { get; set; }

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

    [Column("AccountID1")]
    [MaxLength(18)]
    public byte[] AccountId1 { get; set; }

    [Column("AccountID2")]
    [MaxLength(18)]
    public byte[] AccountId2 { get; set; }

    [Column("AccountID3")]
    [MaxLength(18)]
    public byte[] AccountId3 { get; set; }

    [Column("AccountID4")]
    [MaxLength(18)]
    public byte[] AccountId4 { get; set; }

    [Column("URLKey")]
    public long Urlkey { get; set; }

    [Precision(0)]
    public DateTime? LoadTimeStamp { get; set; }

    public int LoadStatus { get; set; }

    [Column("AccountTypeID")]
    [MaxLength(18)]
    public byte[] AccountTypeId { get; set; }
}
