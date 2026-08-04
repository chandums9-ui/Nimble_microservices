using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Model
{
    public class AttachmentDTO
    {
        public int ID { get; set; }
        public string FolderPath { get; set; }
        public string FileName { get; set; }
    }
}
