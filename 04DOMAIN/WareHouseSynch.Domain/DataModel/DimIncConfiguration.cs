using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WareHouseSynch.Domain.DataModel;

[Table("Dim_IncConfiguration")]
[Index("CorpKey", "IncDeptKey", "AccountKey", "Type", "Urlkey", Name = "CompositeKey")]
public partial class DimIncConfiguration
{
    [Key]
    public long IncCongDetailKey { get; set; }

    public long CorpKey { get; set; }

    public long? IncGroupKey { get; set; }

    public long IncDeptKey { get; set; }

    public long AccountKey { get; set; }

    public byte Type { get; set; }

    public byte? Order { get; set; }

    [Column("IncConfigDetailID")]
    public long IncConfigDetailId { get; set; }

    public short? DetailOrder { get; set; }

    public short? PayrollItemType { get; set; }

    public short? HoursType { get; set; }

    public short? EnableStatitics { get; set; }

    public short Status { get; set; }

    [Column("URLKey")]
    public long? Urlkey { get; set; }

    public DateTime? LoadedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    [Column("IncConfigID")]
    public long IncConfigId { get; set; }
}
