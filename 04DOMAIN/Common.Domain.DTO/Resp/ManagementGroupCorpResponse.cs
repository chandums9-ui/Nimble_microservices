using Common.Domain.DTO.App;
using Common.Domain.DTO.Model;
using Common.Domain.DTO.Model.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Resp
{
    public class ManagementGroupCorpResponse : StatusDTO
    {
        public ManagementGroupCorpResponse()
        {
            Status = Constants.MSG_ENDPOINT_ERROR;
            StatusCode = StatusCodes.Status500InternalServerError;
        }

        public List<CorporationDTO> Corporations { get; set; }
    }
    //public class ManagementCorpDbResponse
    //{
    //   public string CorpID {  get; set; }
    //   public string CorporationName { get; set;}
    //   public int PropertyType {  get; set;}   
    //}
}
