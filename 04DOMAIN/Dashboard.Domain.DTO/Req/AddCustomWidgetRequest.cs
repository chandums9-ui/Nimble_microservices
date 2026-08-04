using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Req
{
    public class AddCustomWidgetRequest
    {
        [WidgetNameValidation]
        public string WidgetName { get; set; }
        public Int16 WidgetType { get; set; }
        public List<string> ParentUserIDs {  get; set; }
    }
    public class WidgetNameValidation : ValidationAttribute
    {
        // Method to perform validation
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // Check if value is null or empty
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return new ValidationResult("Enter Custom Widget Name"); // Return validation error message
            }

            return ValidationResult.Success; // Return success if validation passes
        }
    }
}
