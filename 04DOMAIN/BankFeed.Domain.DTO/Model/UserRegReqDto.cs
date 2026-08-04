using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankFeed.Domain.DTO.Model
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

    public class Session
    {
        public string UserSession { get; set; }
    }
    public class UserReqDTO
    {
        public string loginName { get; set; }

    }




    public class TransactionsCount
    {
        public Transaction transaction { get; set; }
    }
    public class Transaction
    {
        public Total TOTAL { get; set; }
    }
    public class Total
    {
        public int count { get; set; }
    }




}
