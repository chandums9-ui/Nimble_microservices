using Common.Domain.DTO.Model.Base.Contracts;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Resp
{
    public class CheckLoadResponse : JournalEntryBaseDTO, IStatusDTO
    {

        public decimal Amount { get; set; }
        public string Memo { get; set; }
        public string AccountID { get; set; }
        public string PaymentMethodID { get; set; }
        public string PyeeID { get; set; } // SOURCEID IN TRANSACTION TABLE
        public string CheckNum { get; set; }

        public List<AttachmentDTO> Attachments { get; set; }
        public List<AddressDTO> Address { get; set; }

        public int StatusCode { get; set; }
        public string Status { get; set; }
    }
}
