using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailySales.Domain.DTO.Model
{
    public class DailySalePMSDTO
    {
        public long index { get; set; }
        public byte[] DailySaleID { get; set; }
        public long ID { get; set; }
        public long PMSID { get; set; }
        public DateTime Date { get; set; }
        public byte[] CorpID { get; set; }
        public byte[] PCID { get; set; }
        public string PMS { get; set; }
        public short PMSType { get; set; }
        public string FacilityID { get; set; }
        public string SEQ { get; set; }
        public string TransactionType { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string LineItemDescription { get; set; }
        public string Department { get; set; }
        public string ClassLabel { get; set; }
        public long? stat { get; set; }
        public string ReportName { get; set; }
        public string PSCurrentStat { get; set; }
        public bool IsConfigMismatch { get; set; }
        public decimal? ActualTodayDebits { get; set; }
        public decimal? AdjustedCredits { get; set; }
        public decimal? NetAmount { get; set; }
        public decimal? BalanceAmount { get; set; }
        public byte[] LineID { get; set; }
        public short DeptType { get; set; }
        public short? Type { get; set; }
        public short? IsEnding { get; set; }
        public short status { get; set; }
        public long Order { get; set; }
        public short? ImportType { get; set; }
        public short? DebitCreditMapping { get; set; }
        public short? SignMapping { get; set; }
        public short? IsNotRequired { get; set; }
        public byte[] ClassLabelLineID { get; set; }
        public short ClassLabelDeptType { get; set; }
        public short? ClassLabelIsEnding { get; set; }
        public string ClassLabelDeptName { get; set; }
        public string ClassLabelLineName { get; set; }

        public decimal? PayloadActualTodayDebits { get; set; }
        public decimal? PayloadAdjustedCredits { get; set; }

        public List<AliasLabels> AliasLabels { get; set; }

    }
    //public class AliasLabels
    //{
    //    public string AliasClassLabel { get; set; }
    //    public string AliasClassLabelLineID { get; set; }
    //    public string AliasClassLabelDeptType { get; set; }
    //    public string AliasDeptName { get; set; }
    //    public string AliasSubDeptName { get; set; }
    //}

    public class DailySaleLinesDTO
    {
        public byte[] ID { get; set; }
        public short DeptType { get; set; }
        public string Name { get; set; }
        public string DeptName { get; set; }
        public byte[] creditaccountID { get; set; }
        public byte[] debitaccountID { get; set; }
        public short arLedgerType { get; set; }


    }
    public class VerificationDetailsDTO
    {
        public decimal Amount { get; set; }

        public string AccountID { get; set; }
        public string LineID { get; set; }
        public string Name { get; set; }
        public short IsEnding { get; set; }
        public short IsGuestLedger { get; set; }

    }
}
