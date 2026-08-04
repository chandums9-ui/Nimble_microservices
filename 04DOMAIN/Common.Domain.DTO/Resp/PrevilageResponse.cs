using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Resp
{
    public  class PrevilageResponse
    {
        public List<PrevilageDetails> Details { get; set; } 
    }
    public class PrevilageDetails
    {
        public string MenuID { get; set; }
        public bool Create { get; set; } = false;
        public bool View { get; set; } = false;
        public bool Update { get; set; } = false;
        public bool Delete { get; set; } = false;
    }
}
