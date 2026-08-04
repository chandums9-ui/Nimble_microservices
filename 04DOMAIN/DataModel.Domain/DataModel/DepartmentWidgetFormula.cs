using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Domain.DataModel
{
    public partial class DepartmentWidgetFormula
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }

        string CustomLabel { get; set; }
        short Order { get; set; }
        string DeptType { get; set; }

        string Formula { get; set; }

        string GroupFor { get; set; }

        [Required]
        [Column("ClientID")]
        [MaxLength(18)]
        public byte[] ClientId { get; set; }

        [Required]
        [Column("UserID")]
        [MaxLength(18)]
        public byte[] UserId { get; set; }
    }
}
