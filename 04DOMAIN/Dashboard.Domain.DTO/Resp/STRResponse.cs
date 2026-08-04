using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Model.Base.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Resp
{
    public class STRResponse : IStatusDTO
    {
        public List<BaseSTRData> StrGroupList { get; set; }
        public int StatusCode { get; set; }
        public string Status { get; set; }
       
    }
    public class STROptions : BaseSTRData, IStatusDTO
    {
        public int StatusCode { get; set; }
        public string Status { get; set; }
    }
    public class BaseSTRData
    {
        public string corporation_name { get; set; }
        public string corporation_id { get; set; }
        public string profitcneter_id { get; set; }
        public string profitcenter_name { get; set; }

        public string str_id { get; set; }
        public List<GroupData> GroupsData { get; set; }

        
    }
    public class GroupData
    {

        public string GroupName { get; set; }
        public List<STROptionDetails> Options { get; set; }
    }

    public class STROptionDetails
    {
        public DateTime timestamp { get; set; }
        public Metadata metadata { get; set; }
        public string label { get; set; }
        public object change { get; set; }
        public object change_rate { get; set; }
        public List<DateTime> week_range { get; set; }
        public string tag_type { get; set; }
        public int year { get; set; }
    }

    public class ExternalApiSTRResponse
    {
        public string corporation_name { get; set; }
        public string corporation_id { get; set; }
        public string profitcenter_id { get; set; }
        public string profitcenter_name { get; set; }

        public string str_id { get; set; }
        public int status_code { get; set; }
        public string detail { get; set; }
        public string sheet { get; set; }

        public List<STROptionDetails> data { get; set; }

        public List<STROptionDetails> adr { get; set; }
        public List<STROptionDetails> occupancy { get; set; }
        public List<STROptionDetails> revpar { get; set; }
    }


    public class MultiCorpExternalApiSTRResponse
    {
        public List<ExternalApiSTRResponse> data { get; set; }
        public string detail { get; set; }
        public int status_code { get; set; }
    }

    public class GetWeeksOfYearResponse : StatusDTO
    {
        public Dictionary<string, List<string>> WeeksOfYear { get; set; } = new Dictionary<string, List<string>>();
    }

    public class Datum
    {
        public string file_name { get; set; }
        public string s3_key { get; set; }
        public DateTime upload_date { get; set; }
        public int delete_status { get; set; }
        public string corporation_name { get; set; }
        public string str_id { get; set; }
        public string corporation_id { get; set; }
        public string profit_center_id { get; set; }
        public string user_id { get; set; }
        public string client_id { get; set; }
        public string url { get; set; }
        public string report_type { get; set; }
        public List<DateTime> date_range { get; set; }

        #region Dispose
        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~WidgetPrivilegeResponse()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }

    public class LatestAvailableDateResponse : StatusDTO
    {
        public List<Datum> data { get; set; }
        public int status_code { get; set; }
        public string detail { get; set; }

        #region Dispose
        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~WidgetPrivilegeResponse()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
