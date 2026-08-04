using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel.Domain.DataModel;
using Common.Domain.DTO.Resp;
namespace Common.Domain.Mapper
{
    //used in singlePost
    public class SinglePostData : JournalResponse
    {
        //these are added for BankFeedPosting Response Purpose
        public byte[] TransactionID { get; set; }
        public short? NameType { get; set; }

        public DateTime EntryDate { get; set; }
        public string CheckNO { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }
    }

    //used in multiplePost
    public class MultiPostData : SinglePostData
    {
        public List<Transaction> Transactions = new List<Transaction>();
        public JournalEntry JournalEntry;
        public List<InterCompanyReferences> InterCompanyReferences = new List<InterCompanyReferences>();
    }
}
