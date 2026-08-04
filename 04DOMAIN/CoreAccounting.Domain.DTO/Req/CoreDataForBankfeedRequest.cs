using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAccounting.Domain.DTO.Req
{
    public class AccountAndPurposeNamesRequest
    {
        public List<byte[]> CorporationIds { get; set; } = new();
        public List<string> AccountNames { get; set; } = new();
        public List<SplitPurposeRow> Rows { get; set; } = new();
        public string PayeeName { get; set; }
        public short PayeeType { get; set; }
    }
    public class SplitPurposeRow
    {
        public string? SplitAccountName { get; set; }
        public string? PurposeName { get; set; }
    }

    public class AccountAndPurposeIdsRequest
    {
        public List<byte[]> CorporationIds { get; set; } = new();
        public List<byte[]> AccountIds { get; set; } = new();
        public List<SplitPurposeIdRow> Rows { get; set; } = new();
        public  byte[]? PayeeId { get; set; }
        public short PayeeType { get; set; }

    }

    public class SplitPurposeIdRow
    {
        public byte[]? SplitAccountId { get; set; }
        public byte[]? PurposeId { get; set; }
    }
}
