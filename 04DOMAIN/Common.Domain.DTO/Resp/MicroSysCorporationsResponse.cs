using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Model;
using Common.Domain.DTO.Req;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Resp
{
    public class MicroSysCorporationsResponse
    {
        public List<MicroSysDTO> Corporations { get; set; }
    }
    public class MicroSysAccountListResponse
    {
        public List<MicroSysAccountDTO> Accounts { get; set; }
    }
    public class MicroSysVendorListResponse
    {
        public List<MicroSysVendorNameDTO> ListInfo { get; set; }
    }
    public class MicroSysPaymentDetailResponse
    {
        public List<MicroSysPaymentDetailDTO> ListInfo { get; set; }
    }
}
