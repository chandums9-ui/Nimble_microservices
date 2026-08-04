    using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailySales.Domain.DTO.Enums
{
    public enum ImportType
    {
        Imported = 0,
        Automatic = 1,
        Manual = 2,
    }

    public enum ApprovalType
    {
        Entry = 1,
        Verification = 2,
        Approval = 3,
    }
    public enum ApprovalStatus
    {
        Entry = 1,
        Verification = 2,
        Approval = 3,
        Rejected = 4,
        UnApproved = 5,
    }
    public enum DSCashCheckFilterEnum
    {
        [Display(Name ="Profit Center")]
        ProfitCenter = 1,
        [Display(Name = "Total Amount")]
        TotalAmount,
        [Display(Name = "Total Deposited")]
        TotalDeposited,
        [Display(Name = "Difference")]
        Difference,
        [Display(Name = "Created by")]
        Createdby,
        [Display(Name = "Status")]
        Status
    }
    public enum AmountEnum
    {
        [Display(Name = "=")]
        EqualTo =1,
        [Display(Name = ">=")]
        GreaterthanOrEqual ,
        [Display(Name = "<=")]
        LessthanOrEqual     

    }

    public enum NameFilterEnum
    {
        [Display(Name = "Starts With")]
        StartsWith = 1,
        [Display(Name = "Ends With")]
        EndsWith,
        [Display(Name = "Contains")]
        Contains,
        [Display(Name = "Equals")]
        Equals
    }

    public enum MerchantReconStatusEnum
    {
        Pending = 1,
        AutoReconciled = 2,
        Delete = 3,
        ManualReconciled = 4

    }
    public enum OTBRoomTypes
    {
        Transcient = 1,
        Group
    }
}
