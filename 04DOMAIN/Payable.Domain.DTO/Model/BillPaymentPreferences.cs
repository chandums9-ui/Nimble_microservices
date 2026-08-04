using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Model
{
    public class BillPaymentPreferences
    {
        public bool? VoidDate { get; set; }
        public short? PreviewPanel { get; set; }
        public short PageCount { get; set; }
    }
}
