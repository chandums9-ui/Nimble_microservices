using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WareHouseSynch.Domain.DataModel;

[Table("Dim_Employee")]
public partial class DimEmployee
{
    [Key]
    public long EmpKey { get; set; }

    [Required]
    [StringLength(250)]
    [Unicode(false)]
    public string FirstName { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string LastName { get; set; }

    public long CorpKey { get; set; }

    [Required]
    [Column("EmpID")]
    [MaxLength(18)]
    public byte[] EmpId { get; set; }

    public long? PayrollDeptKey { get; set; }

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
