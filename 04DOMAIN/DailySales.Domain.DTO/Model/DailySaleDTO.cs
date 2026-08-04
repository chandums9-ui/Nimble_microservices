using Common.Domain.DTO.Model.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailySales.Domain.DTO.Model
{
    public class DailySaleDTO : ModelBaseCorporationID
    {
        public string ID { get; set; }
        public DateTime SaleDate { get; set; }
        public string AssignedTo { get; set; }
        public string Comment { get; set; }
        public string Pcid { get; set; }
        [DefaultValue(false)]
        public bool Isupdate { get; set; } = false;
        [DefaultValue(false)]
        public bool Rejected { get; set; } = false;
        public bool? IsConfigMisMatch { get; set; }
        public string userid { get; set; }
    }

    public class VerificationLines
    {
        public string LineID { get; set; }
        public decimal? Amount { get; set; }
        public short Type { get; set; }
        public short IsEndnig { get; set; }
        public decimal? Stats { get; set; }
        public short? SignMapping { get; set; }
        public string SEQ { get; set; }
    }




}
