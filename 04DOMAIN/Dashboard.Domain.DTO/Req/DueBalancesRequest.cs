using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Req
{
    public class DueBalancesRequest :ModelBaseCorporationID
    {
        public DateTime AsofDate
        {
            get
            {
                DateTime fromDate;
                return (!string.IsNullOrEmpty(ShortAsofDate) && DateTime.TryParse(ShortAsofDate, out fromDate)) ? Convert.ToDateTime(ShortAsofDate) : DateTime.Today;
            }
        } 

        public string ShortAsofDate { get;set; } = string.Empty; 
        /// <summary>
        /// Here Type is Bank or CC or All
        /// </summary>
        public short Type {  get; set; }    
        public long UrlKey { get; set; }

    }
}
