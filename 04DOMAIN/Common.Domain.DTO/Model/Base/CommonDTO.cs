using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Model.Base
{
    public class GenericLongListRespDTO : StatusDTO
    {
        public List<GenericLongListDTO> ListInfo { get; set; }
    }
    public class GenericIDNameListDTO
    {
        public List<ModelBaseIDNameDTO> ListInfo { get; set; }
    }
    public class MemoryCacheSettings
    {
        public int SlidingExpirationMinutes { get; set; }
        public int AbsoluteExpirationMinutes { get; set; }
        
    }
    public class ExpireTokenTime
    {
        public bool UseDays { get; set; }
        public int Days { get; set; }
        public bool UseMinutes { get; set; }
        public int Minutes { get; set; }
    }
    public class LoggerSettings
    {
        public bool EnableFileLog { get; set; } = true;
        public bool EnableInfoLog { get; set; } = true;
    }
    public class GenericIntervalListDTO
    {
        public List<ModelBaseIDIntervalDTO> ListInfo { get; set; }
    }
    public class GenericLongIDDTO
    {
        public long ID { get; set; }
    }

    public class GenericLongStringDTO: GenericLongStringResDTO
    {
        public string providerAccountID { get; set; }
    }

    public class GenericLongStringResDTO : StatusDTO
    {
        public long ID { get; set; }
        public string User_Token { get; set; }
    }
    public class GenericLongRespDTO : StatusDTO
    {
        public long ID { get; set; }
    }
    
    public class PageDTO
    {
        /// <summary>
        /// Current page number
        /// </summary>
        public int Offset { get; set; } = 1;

        /// <summary>
        /// How many records to skip
        /// </summary>
        public int SkipRecords
        {
            get
            {
                var skipRecords = (Offset - 1) * PageCount;
                if (skipRecords < 0)
                    skipRecords = 0;
                return skipRecords;
            }
            private set { }
        }

        /// <summary>
        /// Total records
        /// </summary>
        public int TotalCount { get; set; } = 0;

        /// <summary>
        /// Records per page
        /// </summary>
        public int PageCount { get; set; } = 40;

        /// <summary>
        /// Total number of pages
        /// </summary>
        public int TotalPages
        {
            get
            {
                if (TotalCount != 0 && PageCount != 0)
                {
                    var totalPages = TotalCount / PageCount;
                    if (TotalCount % PageCount > 0)
                        totalPages++;

                    return totalPages;
                }
                else
                {
                    return 0;
                }
            }

            private set { }
        }

        public SortingRequest SortingRequest { get; set; }=new SortingRequest();
        public SearchRequest SearchRequest { get; set; } = new SearchRequest();


    }

    public class SortingRequest
    {
        public string ColumnName { get; set; }

        public bool IsAscending { get; set; } = false;

        public bool IsSelected { get; set; }

    }
    public class SearchRequest
    {
        public string FilterType { get; set; }

        public string SearchText { get; set; }

        public string ColumnName { get; set; }

        public List<string> CorpIds { get; set; }
        public List<string> BankCreditIds { get; set; }
        public List<string> COAIds { get; set; }
        public bool IsLastSelectedColumn { get; set; }

    }

    public class MultipleSearchRequest
    {
        public List<SearchRequest> SearchReqs { get; set; }
        public int VendorStatusFilter { get; set; } = 1;

        public int VendorStatusFilterSort { get; set; } = -1;
        public int PageNumber { get; set; }

        public bool IsPageChange { get; set; } = false;
    }
}
