using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Model
{
    /// <summary>
    /// Represents a vendor entity.
    /// </summary>
    public class VendorDTO
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public string DefaultContractID { get; set; }

    }
}
