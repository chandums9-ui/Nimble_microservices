using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Model.Base
{
    public class GenericLongListDTO
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public short Type { get; set; }
        public string TypeName { get; set; }
    }
}
