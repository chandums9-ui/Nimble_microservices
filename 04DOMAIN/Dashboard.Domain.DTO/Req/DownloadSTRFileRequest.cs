using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Req
{
    public class DownloadSTRFileRequest 
    {
        public string FileUrl { get; set; }

        public string FileName { get; set; }    
    }
}
