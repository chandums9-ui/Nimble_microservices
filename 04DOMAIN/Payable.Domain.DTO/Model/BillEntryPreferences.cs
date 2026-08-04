using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Model
{
    public class BillEntryPreferences

    {

        public bool? BillDate { get; set; }

        public bool? VoidDate { get; set; }

        public bool? UseTax { get; set; }

        public bool? Statistics { get; set; }

        public bool? TaxEnabledinVendorMaster { get; set; }

        public bool? SplitLineMemoCopyforCOA { get; set; }

        public bool PreviewPanel { get; set; }

        public bool AmountAutoDistribution { get; set; }

        public short? IsVendorNameDisplayonAddNew { get; set; }

        public bool? IsDebitMemoAlertRequired { get; set; }

        public bool? IsLegalName { get; set; }

        public short PageCount { get; set; }

        public string CorporationMailID { get; set; }

        public bool IsEnableEmailPreference { get; set; }

        public bool IsCheckPrintingEnabled { get; set; }

        public int UserApprovalType { get; set; }

        public int ViewGridRange { get; set; }

        public bool UserHasPrintNow { get; set; }

        public string LegalName { get;set; }

    }

}
