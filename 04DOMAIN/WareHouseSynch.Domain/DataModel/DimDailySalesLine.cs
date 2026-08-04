using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WareHouseSynch.Domain.DataModel;

[Table("Dim_DailySalesLines")]
[Index("CorpKey", "Pckey", "DeptType", "IsRoomsSold", "IsRoomsAvailable", "IsComp", "IsVacant", "IsOutOfOrder", "AccountKey", Name = "CompositeKey")]
[Index("DeptType", "CorpKey", "LineKey", Name = "IDX_Dim_DailySalesLines_DeptType_CorpKey_LineKey")]
public partial class DimDailySalesLine
{
    [Key]
    public long LineKey { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    public long? CorpKey { get; set; }

    public short? IsRoomsSold { get; set; }

    public short? IsRoomsAvailable { get; set; }

    public short? IsComp { get; set; }

    public short? IsVacant { get; set; }

    public short? IsOutOfOrder { get; set; }

    public short? IsAvgCnt { get; set; }

    public long DailySaleDeptKey { get; set; }

    [Required]
    [Column("LineID")]
    [MaxLength(18)]
    public byte[] LineId { get; set; }

    public long? IncDeptKey { get; set; }

    public long? IncGroupKey { get; set; }

    public long? AccountKey { get; set; }

    [Column("PCKey")]
    public long? Pckey { get; set; }

    public bool? IsGuestLedger { get; set; }

    public bool? IsOtherLedger { get; set; }

    public short? DeptType { get; set; }

    public long? CustomizeKey { get; set; }

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

    public short? RoomType { get; set; }

    [Column("EnableADRORAVG")]
    public short? EnableAdroravg { get; set; }

    [Column("AccountID")]
    [MaxLength(18)]
    public byte[] AccountId { get; set; }

    [Column("CorporationID")]
    [MaxLength(18)]
    public byte[] CorporationId { get; set; }

    [Column("StoreID")]
    [MaxLength(18)]
    public byte[] StoreId { get; set; }

    [Column("ARLedgerType")]
    public short? ArledgerType { get; set; }

    [Column("URLKey")]
    public long Urlkey { get; set; }

    public short? AdjustLedgerType { get; set; }

    public short? CardDeptType { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string CardTypeName { get; set; }

    public short? CardTypeOrder { get; set; }
}
