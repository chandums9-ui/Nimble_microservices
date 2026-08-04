using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Enums;
using Common.Domain.DTO.Model.Base;
using Dashboard.Domain.DTO.Resp;
using Microsoft.AspNetCore.Http;

namespace Dashboard.Domain.DTO.Resp
{
    public class GetcorporationsResp : StatusDTO
    {

       
        public string ClientId { get; set; }      
        public List<GetcorporationsDbResp> Corporations { get; set; } = new  List<GetcorporationsDbResp> ();

    }
    public class GetcorporationsDbResp
    {

        public string CorporationID { get; set; }
        public string CorpDBAName { get; set; }
        public string LegalName { get; set; }
        public string PropertyType { get; set; }
        public string BrandName {  get; set; }
        public string ServiceType { get; set; }
        public string PMSType { get; set; }

    }
}






