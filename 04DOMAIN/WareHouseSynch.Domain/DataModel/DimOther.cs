using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WareHouseSynch.Domain.DataModel;

[Table("Dim_Others")]
public partial class DimOther
{
    [Key]
    public long OtherKey { get; set; }

    public long CorpKey { get; set; }

    [Required]
    [Column("OtherID")]
    [MaxLength(18)]
    public byte[] OtherId { get; set; }

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

    [Required]
    [StringLength(250)]
    [Unicode(false)]
    public string FirstName { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string LastName { get; set; }

    [Column("URLKey")]
    public long Urlkey { get; set; }
}
