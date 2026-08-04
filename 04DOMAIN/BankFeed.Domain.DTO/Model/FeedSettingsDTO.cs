using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankFeed.Domain.DTO.Model
{
    public class FeedSettingsDTO
    {
        public string ClientID { get; set; }
        /// <summary>
        /// 0-Disable/false ,1 - Enable/true
        /// </summary>
        [DefaultValue(false)]
        public bool? EnablePendingTrans { get; set; }
        [DefaultValue(180)]
        [Range(1,365)]
        public int? TransDateRange { get; set; }
        /// <summary>
        /// 0 - FIFO/false ,1 - LIFO/true
        /// </summary>
        [DefaultValue(true)]
        public bool? PossibleMatchesBasedOn { get; set; }
        /// <summary>
        /// 0-Exclude ,1 - Include
        /// </summary>
        [DefaultValue(false)]
        public bool? IncludeReconcileTransactions { get; set; }
        /// <summary>
        /// 0- Disable ,1 - Enable 
        /// </summary>
        [DefaultValue(false)]
        public bool? EnablePostFeedRule { get; set; }

        public bool? DsCashCheckEnable { get; set;}
    }
}
