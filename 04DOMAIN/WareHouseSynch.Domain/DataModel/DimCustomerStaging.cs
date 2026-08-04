using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WareHouseSynch.Domain.DataModel;

[Keyless]
[Table("DimCustomerStaging")]
public partial class DimCustomerStaging
{
    [Column("ID")]
    public long Id { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string CustomerName { get; set; }

    [Required]
    [Column("CustomerID")]
    [MaxLength(18)]
    public byte[] CustomerId { get; set; }

    public short? Status { get; set; }

    [Required]
    [Column("CorporationID")]
    [MaxLength(18)]
    public byte[] CorporationId { get; set; }

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

    [Precision(0)]
    public DateTime? LoadTimeStamp { get; set; }

    public int LoadStatus { get; set; }
}
