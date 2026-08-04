using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAccounting.Domain.DTO.Model
{
    
        public class WrkSpotProviderInfo
        {
            public string WrkSpotURL { get; set; }
        }
        public class WrkSpotKeyInfo
        {
            public string VendorCode { get; set; }
            public string SecretKey { get; set; }

        }
        public class WrkSpotEndPoints
        {
            public string AuthToken { get; set; }
            public string Summary { get; set; }
            public string Properties { get; set; }

        }
        public class token
        {
            public string access_token { get; set; }
            public string scope { get; set; }
            public string token_type { get; set; }
            public int expires_in { get; set; }
        }
        public class Summary
        {
            public string city { get; set; }
            public string clientCode { get; set; }
            public List<DepartmentSummary> departmentSummary { get; set; }
            public string propertyName { get; set; }
            public string siteCode { get; set; }
            public string startDate { get; set; }
            public string endDate { get; set; }
            public string state { get; set; }
        }
        public class DepartmentSummary
        {
            public string deptName { get; set; }
            public string minsWorked { get; set; }
            public string wages { get; set; }
        }

        public class PropetiesList
        {
            public List<Property> properties { get; set; }
        }
        public class Property
        {
            public string siteCode { get; set; }
            public string clientCode { get; set; }
            public string propertyName { get; set; }
            public string propertyType { get; set; }
            public string city { get; set; }
            public string stateCode { get; set; }
            public string timezone { get; set; }
            public string status { get; set; }

        }
    
}
