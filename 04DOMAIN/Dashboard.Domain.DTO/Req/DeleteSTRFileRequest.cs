using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Req
{
    public class DeleteSTRFileRequest
    {

        public string fileId { get; set; }
        public string fileType { get; set; }
        public string userId { get; set; }

    }
}
