using Common.Domain.DTO.Enums;
using Common.Domain.DTO.Req;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Model.Base
{
    public class UserPreferenceDTO
    {
        public bool IsBillDate { get; set; } = false;
        public bool IsVoid { get; set; } = false;
        public bool IsStatistics { get; set; } = false;
        public bool IsShowCorpLegalName { get; set; } = false;

        public bool IsBillNumber { get; set; } = false;
        public bool IsAccountNumber { get; set; } = false;
        public bool IsCheckMemo { get; set; } = false;
        public bool IsManualMemo { get; set; } = false;
        public short ViewGridPageSize { get; set; } = 40;
        public short ViewGridRange { get; set; } = 0;//defaukt is IsDefault =0
        public bool IsEnableHeadAccount { get; set; } = false;
        //public bool ViewGridDefault { get; set; } = false;
        //public UserPrivilegesDTO Privileges { get; set; }

    }
    public class UserPrivilegesDTO
    {
        public bool HasCreate { get; set; }
        public bool HasUpdate { get; set; }
        public bool HasDelete { get; set; }
    }
}

