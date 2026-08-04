using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WareHouseSynch.Domain.DataModel;

[Table("Dim_IncGroup")]
public partial class DimIncGroup
{
    [Key]
    public long IncGroupKey { get; set; }

    [Required]
    [StringLength(250)]
    [Unicode(false)]
    public string Name { get; set; }

    public long CorpKey { get; set; }

    [Column("GroupID")]
    public long GroupId { get; set; }

    [Column("GroupID1")]
    public long? GroupId1 { get; set; }

    [Column("GroupID2")]
    public long? GroupId2 { get; set; }

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
}
