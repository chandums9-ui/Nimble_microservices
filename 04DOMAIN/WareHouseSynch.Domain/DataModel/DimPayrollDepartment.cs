using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WareHouseSynch.Domain.DataModel;

[Table("Dim_PayrollDepartment")]
public partial class DimPayrollDepartment
{
    [Key]
    public long PayrollDeptKey { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string Name { get; set; }

    public long CorpKey { get; set; }

    [Required]
    [Column("PayrollDeptID")]
    [MaxLength(18)]
    public byte[] PayrollDeptId { get; set; }

    public long? IncDeptKey { get; set; }

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

    [StringLength(250)]
    [Unicode(false)]
    public string JobTitle { get; set; }

    [Column("URLKey")]
    public long Urlkey { get; set; }
}
