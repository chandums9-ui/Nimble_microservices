using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAccounting.Domain.DTO.Req
{
    public class FundTransferVoidRequest

    {
        public string FromJournalEntryId { get; set; }

        public DateTime? VoidDate { get; set; }

        public string VoidRemarks { get; set; }

        public bool isVoid { get; set; }

        [DefaultValue(true)]

        public bool IsSave { get; set; }

        [DefaultValue(false)]

        public bool IsFromFT { get; set; }

        public bool IsValidate { get; set; } = false;

    }
}
