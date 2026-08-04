using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Req
{

    public class LoadByIDRequest : ModelBaseIDString
    {
        [DefaultValue(false)]
        public bool IsValidate { get; set; } //To Check Corporation Lock
    }

    public class LoadByIDLongIDRequest : ModelBaseIDString
    {
        public long LongID { get; set; }
    }
    public class LoadByLongIDRequest : ModelBaseIDInt64
    {

    }
    public class LoadByLongIDsRequest 
    {
        public List<long> IDs { get; set; }

    }

    /// <summary>
    /// To get Auto Sync Time details for a client
    /// </summary>
    public class LoadByClientID : ModelBaseClientID
    {

    }

    public class ModelBaseSearchRequest : ModelBaseIDString
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }

        [DefaultValue(0)]
        public int Count { get; set; }

        [DefaultValue(0)]
        public int Offset { get; set; }
    }


    public class DateRangeDTO 
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
    public class FromdateandTodate
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }

    public class CheckOrVoucherNumValidationReq
    {
        public string AccountID { get; set; }
        public string PaymethodID { get; set; }
        public string Number { get; set; }
    }
    public class WareHouseBinReq :ModelBaseIDString
    {
       
        public string CorpID { get; set; }
        [DefaultValue(false)]
        public bool IsUpdatePrevious { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string CloneID { get; set; }

        public short? EventType { get; set; }
    }


    public class  GeneralWareReq: ModelBaseIDString
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
    public class WareHouselongReq : ModelBaseIDInt64
    {

        public string CorpID { get; set; }
      

    }
    public class ServerAnlyticsGroup
    {
        public string ServerName { get; set; }
        public string clients { get; set; }


    }
   

}
