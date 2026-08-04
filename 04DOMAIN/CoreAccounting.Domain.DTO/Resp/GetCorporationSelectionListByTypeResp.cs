using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAccounting.Domain.DTO.Resp
{
    public class GetCorporationSelectionListByTypeResp
    {
        public string CorporationID { get; set; }
        public string CorporationName { get; set; }
        public string LegalName { get; set; }
        public string PropertyType { get; set; }

        public string Service { get; set; }
        public string Brand { get; set; }
        public string PMS { get; set; }

    }
}
