using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WareHouseSynch.Domain.DataModel;

[Table("Dim_Account")]
[Index("CorpKey", "AccountTypeOrder", "Status", "Urlkey", Name = "Composite")]
[Index("AccountKey", "CorpKey", Name = "IDX_Dim_Account_AccountKey_CorpKey")]
public partial class DimAccount
{
    [Key]
    public long AccountKey { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string AccountName { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string AccountNumber { get; set; }

    public long CorpKey { get; set; }

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

    public long? AccountTypeKey { get; set; }

    [Column("URLKey")]
    public long Urlkey { get; set; }

    public short? AccountTypeOrder { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? OpeningBalance { get; set; }
}
