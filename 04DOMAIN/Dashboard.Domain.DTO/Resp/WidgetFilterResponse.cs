using Common.Domain.DTO.App;
using Common.Domain.DTO.Enums;
using Common.Domain.DTO.Model.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Resp
{
    public class WidgetFilterResponse : StatusDTO
    {
        public WidgetFilterResponse()
        {
            Status = Constants.MSG_NO_DATA_FOUND;
            StatusCode = StatusCodes.Status204NoContent;
        }
        public int WidgetID { get; set; }
        public int FromDate { get; set; }
        public int ToDate { get; set; }
        public int FilterType { get; set; }
    }
    public class WidgetFilterSaveResponse : StatusDTO
    {
        public WidgetFilterSaveResponse()
        {
            Status = Constants.MSG_NO_DATA_FOUND;
            StatusCode = StatusCodes.Status204NoContent;
        }
        public short SavedFilter { get; set; }
    }
}
