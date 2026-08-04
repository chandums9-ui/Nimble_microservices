using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WareHouseSynch.Domain.DataModel;

[Table("Dim_Customization")]
public partial class DimCustomization
{
    [Key]
    public long CustomizeKey { get; set; }

    [Required]
    [StringLength(250)]
    [Unicode(false)]
    public string Name { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string GroupName { get; set; }

    /// <summary>
    /// 2-InComeDepartment,1-IncGroup,3-PayrollDept,4-Accounts,5-Stats and Others,6-Custom PL
    /// </summary>
    public byte SourceType { get; set; }

    [Column("SourceID")]
    public long? SourceId { get; set; }

    [Column("SourceBinID")]
    [MaxLength(18)]
    public byte[] SourceBinId { get; set; }

    public long? CorpKey { get; set; }

    public bool IsStatic { get; set; }

    /// <summary>
    /// 0-For all except Inc Department,For Inc Depts use Deptype
    /// </summary>
    public byte DeptType { get; set; }

    /// <summary>
    /// 4-Profit Or Loss,-5-ADR,6-Occupancy,7-RevPar,8-Rooms Sold,9-Rooms Available,10-Vacant,11-Comp,12-Out Of Order,13-Total Cost Per Occupied Room,14-Total Cost Per Available Room,15-Payroll Cost per Occupied Room,16-Payroll Cost per Available Room,Account Type Orders are Types
    /// </summary>
    public byte SubType { get; set; }
}
