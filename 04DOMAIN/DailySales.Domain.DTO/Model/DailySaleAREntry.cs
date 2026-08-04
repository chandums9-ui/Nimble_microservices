using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailySales.Domain.DTO.Model
{
    public class DailySaleAREntry
    {
        public string ID { get; set; }
        public string DailyConfigInputID {  get; set; }
        public string LineID { get; set; }
        public decimal Amount { get; set; }
        public string CAccountID {  get; set; }
        public string DAccountID {  get; set; }
    }
}
