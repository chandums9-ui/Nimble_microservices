using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankFeed.Domain.DTO.Req
{
    public class BankAccountMergeSettings : BankAccountMergeSettingsDefault
    {
     
        public string LineID { get; set; }
        public short DeptType { get; set; }
        public byte MergeType { get; set; }

    }
    public class BankAccountMergeSettingsDefault 
    {
        public string CorporationID { get; set; }
        public string AccountID { get; set; }
       

    }
    public class BankAccountMergeSettingsReq
    {
        public List<BankAccountMergeSettings> MergeSettings { get; set; }
    }
    public class BankMergeSettingsResponse :StatusDTO
    {
        
    }
}
