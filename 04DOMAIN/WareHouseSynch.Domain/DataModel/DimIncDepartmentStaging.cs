using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WareHouseSynch.Domain.DataModel;

[Table("DimIncDepartmentStaging")]
public partial class DimIncDepartmentStaging
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Required]
    [StringLength(250)]
    [Unicode(false)]
    public string Name { get; set; }

    [Required]
    [Column("CorporationID")]
    [MaxLength(18)]
    public byte[] CorporationId { get; set; }

    public short? DeptType { get; set; }

    [Column("DeptID")]
    public long DeptId { get; set; }

    [Column("DepartmentID1")]
    public long? DepartmentId1 { get; set; }

    public byte? Status { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string LoadedBy { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string UpdatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdatedDate { get; set; }

    public byte? Order { get; set; }

    public short? IncOrExpType { get; set; }

    public byte? ShowInIncome { get; set; }

    public byte? ShowSubDep { get; set; }

    public byte? SubDepSeperationEnable { get; set; }

    public byte? Type { get; set; }

    [MaxLength(18)]
    public byte[] DailyConfigDeptId { get; set; }

    [Column("URLKey")]
    public long Urlkey { get; set; }

    [Precision(0)]
    public DateTime? LoadTimeStamp { get; set; }

    public int LoadStatus { get; set; }
}
