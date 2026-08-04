using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankFeed.Domain.DTO.Model
{
    public class FeedAccountDeleteDTO 
    {
        public FeedAccountDeleteDTO()
        {
            FeedAccountMapping = new List<FeedAccountMappingDTO>();
            AccIDs = new List<long>();
        }
        public List<long> AccIDs { get; set; }
        public bool IsDelete { get; set; } = false;
        public bool IsDisconnected { get; set; } = false;
        public bool IsUnMapAccount { get; set; } = false;
        public List<FeedAccountMappingDTO> FeedAccountMapping { get; set; }

    }
}
