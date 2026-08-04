using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Model
{
    public class TableResults<T>
    {
        public List<string> Headers { get; set; }
        public List<List<T>> Rows { get; set; }
    }
}
