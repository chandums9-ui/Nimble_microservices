using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Req
{
    public class EntryNumberRequest: CorpIDRequest
    {
        public int Type { get; set; }

        [DefaultValue(0)]
        public int IsEntryNumber { get; set; }
    }
}
