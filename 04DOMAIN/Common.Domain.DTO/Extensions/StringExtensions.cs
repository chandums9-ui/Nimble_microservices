using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Extensions
{
    public static class StringExtensions
    {
        public static string MaskByLength(this String input, int digits = 4)
        {
            try
            {
                if (!string.IsNullOrEmpty(input))
                {
                    if (input.Length > digits)
                    {
                        return new string('*', 4) + input.Substring(input.Length - digits);
                    }
                    else
                        return new string('*', 4) + input;
                }
                else if (digits > 0)
                    return new string('*', digits);
                else
                    return new string('*', 4);
            }
            catch (Exception ex)
            {
                return new string('*', 4);
            }
        }

        public static string TrimByLength(this String input, int maxLenghtToDisplay = 13, int dotChars = 3)
        {
            try
            {
                if (!string.IsNullOrEmpty(input))
                {
                    if (input.Length > maxLenghtToDisplay)
                    {
                        return input.Substring(0, maxLenghtToDisplay) + new string('.', dotChars);
                    }
                }
                return input;
            }
            catch { return input; }
        }
    }


}
