using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Model
{
    public class PayableLaunchCardDetails
    {
        public int ID { get; set; }
        public decimal Amount { get; set; }
        public int TransCount { get; set; }
    }


    //public class FilterModels
    //{
    //    public string FirstSelectValue { get; set; }
    //    public List<FilterDetail> FilterDetailsList { get; set; } = new List<FilterDetail>(); // List to hold multiple filters for the same option value


    //}

    //// New class to hold individual filter details (second dropdown and input)
    //public class FilterDetail
    //{
    //    public string SecondSelectValue { get; set; }
    //    public string InputValue { get; set; }


    //}

    public class FilterModels
    {
        public int FirstSelectValue { get; set; }
        public string InputID { get; set; }
        public int SecondSelectValue { get; set; }
        public string SubTypeValue { get; set; }
        public string InputValue { get; set; }
        public string EnteredInputValue { get; set; }


        public FilterModels DeepCopy()
        {
            return new FilterModels
            {
                FirstSelectValue = this.FirstSelectValue,
                InputID= this.InputID,
                SecondSelectValue = this.SecondSelectValue,
                InputValue = this.InputValue,
                SubTypeValue = this.SubTypeValue,
                EnteredInputValue = this.EnteredInputValue,
                // Uncomment and deep copy lists if you add them back in the future
                // FilterDetailsList = this.FilterDetailsList.Select(detail => detail.DeepCopy()).ToList()
            };
        }
    }
}