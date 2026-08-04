using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DTO.Extensions
{
    public static class MethodExtensions
    {
        #region Exception Extension Methods

        public static string DeepParseMessage(this Exception ex, bool deepParse = true, bool includeStackTrace = true)
        {
            StringBuilder sbMessage = new StringBuilder();
            string stackTrace = null;
            try
            {
                if (ex != null)
                {
                    if (includeStackTrace)
                        stackTrace = ex.StackTrace;

                    sbMessage.Append(ex.Message+ Environment.NewLine);
                    ex = ex.InnerException;

                    if (deepParse)
                    {
                        while (ex != null)
                        {
                            sbMessage.Append(ex.Message+ Environment.NewLine);
                            ex = ex.InnerException;
                        }
                    }

                    if (includeStackTrace)
                        sbMessage.Append(stackTrace + Environment.NewLine);
                }

                return sbMessage.ToString();
            }
            catch { return sbMessage.ToString(); }
            finally
            {
                if (sbMessage != null)
                    sbMessage.Clear();
            }
        }

        #endregion Exception Extension Methods
    }
}
