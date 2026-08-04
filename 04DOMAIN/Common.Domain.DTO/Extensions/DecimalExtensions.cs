using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Extensions
{
    public static class DecimalExtensions
    {
        /// <summary>
        /// This is a Counting Shorthand that is put after numerals to make it simpler to count. For instance, 1k, 20k, and etc. 1k equals one thousand rupees
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string CountingShorthand(this decimal input, bool isNumber = false)
        {
            // Apply notation based on the magnitude of the absolute value
            decimal absValue = Decimal.Add(Math.Abs(input), 0.00M);

            if (isNumber) //number case then as is return double value
                return Convert.ToDouble(input).ToString();
            else
            {
                if (absValue >= 1000000000000000000m)
                    return (input < 0 ? "-" : "") + Math.Round(absValue / 1000000000000000000m, 2) + "Q";
                else if (absValue >= 1000000000000000m)
                    return (input < 0 ? "-" : "") + Math.Round(absValue / 1000000000000000m, 2) + "T";
                else if (absValue >= 1000000000000m)
                    return (input < 0 ? "-" : "") + Math.Round(absValue / 1000000000000m, 2) + "B";
                else if (absValue >= 1000000000m)
                    return (input < 0 ? "-" : "") + Math.Round(absValue / 1000000000m, 2) + "M";
                else if (absValue >= 1000000m)
                    return (input < 0 ? "-" : "") + Math.Round(absValue / 1000000m, 2) + "M";
                else if (absValue >= 1000m)
                    return (input < 0 ? "-" : "") + Math.Round(absValue / 1000m, 2) + "K";
                else //if (!isNumber)
                    return (input < 0 ? "-" : "") + Math.Round(absValue, 2).ToString();//Convert.ToDouble(input).ToString();
            }
        }

    }
}
