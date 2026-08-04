using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WareHouseSynch.Domain.DataModel;

[Table("Dim_Vendor")]
public partial class DimVendor
{
    [Key]
    public long VendorKey { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string VendorName { get; set; }

    public long CorpKey { get; set; }

    [Required]
    [Column("VendorID")]
    [MaxLength(18)]
    public byte[] VendorId { get; set; }

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
