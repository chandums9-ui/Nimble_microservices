using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Model
{
   public  class AttachmentVMDTO:AttachmentDTO
   {
        /// <summary>
        /// Gets the size of the file in MB.
        /// </summary>
        public decimal FileSize { get; set; }
        public bool IsShowActions { get; set; } = true;
    }
}
