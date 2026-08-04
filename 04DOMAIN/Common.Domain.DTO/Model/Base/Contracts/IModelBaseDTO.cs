using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Model.Base.Contracts
{
    public interface IModelBaseIDString
    {
        public string ID { get; set; }
    }
    public interface IModelBaseIDInt64
    {
        public long ID { get; set; }
    }
    public interface IModelBaseIDByte
    {
        public byte ID { get; set; }
    }
    public interface IModelBaseCorporationID
    {
        public string CorpID { get; set; }
    }
    public interface IModelBaseCorporation : IModelBaseCorporationID
    {
        public string CorpID { get; set; }
        public string? CorpName { get; set; }
        public long CorpSortOrder { get; set; }
    }
    public interface IModelBaseClientID
    {
        public string ClientID { get; set; }
    }
    public interface IModelBaseUserID
    {
        public string UserID { get; set; }
    }
    public interface IModelBaseSearchRequest : IModelBaseIDString
    {
        public string ID { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }

        [DefaultValue(0)]
        public int Count { get; set; }

        [DefaultValue(0)]
        public int Offset { get; set; }
    }

    public interface IModelBaseIDNameDTO
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public string TypeName { get; set; }
    }

    public interface IModelBaseIDNameSortOrderDTO : IModelBaseIDNameDTO
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public string TypeName { get; set; }
        public long SortOrder { get; set; }
    }
    public interface IModelBaseIDIntervalDTO
    {
        public string ID { get; set; }
        public string Interval { get; set; }

    }
    public interface IValidateTokenReqDTO
    {
        public string Token { get; set; }


    }
}
