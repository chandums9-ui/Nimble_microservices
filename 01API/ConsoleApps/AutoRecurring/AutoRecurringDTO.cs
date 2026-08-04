using Common.App.Contracts;
using CoreAccounting.App.Contracts;
using CoreAccounting.Infra.DBCon;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserMgmt.Infra.DataRepos;

namespace AutoRecurrings
{
    public class AutoRecurringDTO
    {

       
        public class AuthToken
        {
            public string Token { get; set; }
            public string authID { get; set; }
            public string statusCode { get; set; }

        }
       

    }


   
}

