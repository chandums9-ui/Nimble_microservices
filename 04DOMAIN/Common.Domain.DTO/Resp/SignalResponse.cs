using Common.Domain.DTO.Model.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Resp
{
    public class SignalResponse : StatusDTO
    {
        public string ID { get; set; }
        public short IsDelVoidStatus { get; set; }
        public SignalRData Data { get; set; }
    }
    public class SignalRequest : ModelBaseIDString
    {
        public short IsDelVoidStatus { get; set; }
        public SignalRData Data { get; set; }
    }
    public class SignalRData
    {
        public string Id { get; set; }
        public string Value { get; set; }
        public string TypeID { get; set; }
    }

}
