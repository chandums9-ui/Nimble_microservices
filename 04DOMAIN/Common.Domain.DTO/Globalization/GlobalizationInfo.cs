using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Common.Domain.DTO.Globalization
{
   

    /// <summary>
    /// Class to hold all common functionalities related to cultureInformation used in the system
    /// </summary>
    public static class GlobalizationInfo
    {
        #region Fields

        private static string _chCurSymbol = "SFr.";
        private static string _chOverrideCurSymbol = "sFr.";
        private static string _chISOCurSymbol = "CHF";
        private static string _cultureInfoXml = string.Empty;

        #endregion Fields

        /// <summary>
        /// Gets Culture Specific Amount For Display i.e;with Currency Symbol
        /// </summary>
        /// <param name="Amount">Amount as decimal</param>
        /// <returns>Returns a string</returns>
        public static string GetCultureSpecificAmountForDisplay(decimal Amount, string CurrencySymbol = "", string CurrencyPattern = "")
        {
            //return string.Format("{0:C}", Amount);

            RegionInfo ri = new RegionInfo(CultureInfo.CurrentCulture.LCID);
            NumberFormatInfo mutableNfi = (NumberFormatInfo)CultureInfo.CurrentCulture.NumberFormat.Clone();
            int currencyPattern = -1;
            if (!int.TryParse(CurrencyPattern, out currencyPattern))//if parsing fails then currencyPattern becomes 0
                currencyPattern = -1;

            if (ri.TwoLetterISORegionName == "CH")
                mutableNfi.CurrencySymbol = _chOverrideCurSymbol;
            else
                mutableNfi.CurrencySymbol = string.IsNullOrEmpty(CurrencySymbol) ? mutableNfi.CurrencySymbol : CurrencySymbol;
            /* Possible Values for CurrencyPattern:
                * here say $ is CurrencySymbol & n is Amount
                * 0 : $n
                * 1 : n$
                * 2 : $ n
                * 3 : n $
                * */
            if (currencyPattern >= 0 && currencyPattern <= 3)
                mutableNfi.CurrencyPositivePattern = currencyPattern;
            return Amount.ToString("C", mutableNfi);
        }

        /// <summary>
        /// Gets No Culture Specific Date
        /// </summary>
        /// <param name="Date"></param>
        /// <returns>Returns Date as string</returns>
        public static string GetNoCultureSpecificDate(DateTime Date)
        {
            return Date.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// Gets No Culture Specific Date and Time
        /// </summary>
        /// <param name="Date"></param>
        /// <returns>Returns as string</returns>
        public static string GetNoCultureSpecificDateTime(DateTime Date)
        {
            return Date.ToString("yyyy-MM-dd HH:mm:ss");
        }

        //#region Culture & Region Related


        ///// <summary>
        ///// Gets CultureInfo By CountryCode
        ///// </summary>
        ///// <param name="CountryCode">CountryCode</param>
        ///// <returns>Returns CultureInfo object</returns>
        //public static CultureInfo GetCultureInfoByCountryCode(string CountryCode)
        //{
        //    //return new CultureInfo(CountryCode);//(FetchAttributeValue(CultureInfoAttr.CountryCode, CountryCode, CultureInfoAttr.Code));
        //    CultureInfo Info;
        //    var _CultureInfox = new System.Globalization.RegionInfo(CountryCode);
        //    var _CultureInfo = CultureInfo.GetCultures(CultureTypes.SpecificCultures).Where(c => c.Name.EndsWith(CountryCode)).ToList();
        //    if (_CultureInfo.Count == 0)
        //    {
        //        Info = _CultureInfo.FirstOrDefault();// new CultureInfo("en-US");
        //    }
        //    else
        //    {

        //        Info = _CultureInfo.Where(p => p.NumberFormat.CurrencySymbol == _CultureInfox.CurrencySymbol).FirstOrDefault();
        //        if (Info == null)
        //        {
        //            Info = _CultureInfo.Where(p => p.TwoLetterISOLanguageName.Equals("en")).FirstOrDefault();
        //            if (Info == null)
        //            {
        //                Info = _CultureInfo.FirstOrDefault();
        //            }
        //        }
        //    }
        //    return Info;
        //}

        ///// <summary>
        ///// Gets TwoLetter ISOLanguageName By CountryCode
        ///// </summary>
        ///// <param name="CountryCode">CountryCode</param>
        ///// <returns>returns string</returns>
        //public static string GetTwoLetterISOLanguageNameByCountryCode(string CountryCode)
        //{
        //    return GetCultureInfoByCountryCode(CountryCode).TwoLetterISOLanguageName;
        //}

        /// <summary>
        /// Gets CultureInfo By Culture Code
        /// </summary>
        /// <param name="CultureCode">Culture Code</param>
        /// <returns>Returns CultureInfo object</returns>
        public static CultureInfo GetCultureInfoByCultureCode(string CultureCode)
        {
            //return new CultureInfo( FetchAttributeValue(CultureInfoAttr.Code, CultureCode, CultureInfoAttr.Code));
            return new CultureInfo(CultureCode);
        }

        ///// <summary>
        ///// Gets RegionInfo By CountryCode
        ///// </summary>
        ///// <param name="CountryCode">Country Code</param>
        ///// <returns>Returns RegionInfo object</returns>
        //public static RegionInfo GetRegionInfoByCountryCode(string CountryCode)
        //{
        //    CultureInfo ci = null;

        //    try
        //    {
        //        ci = GetCultureInfoByCountryCode(CountryCode);
        //        return new RegionInfo(ci.LCID);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        ci = null;
        //    }
        //}

        /// <summary>
        /// Gets RegionInfo By Culture Code
        /// </summary>
        /// <param name="CultureCode">Culture Code</param>
        /// <returns>Returns RegionInfo object</returns>
        public static RegionInfo GetRegionInfoByCultureCode(string CultureCode)
        {
            CultureInfo ci = null;

            try
            {
                ci = GetCultureInfoByCultureCode(CultureCode);
                return new RegionInfo(ci.LCID);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                ci = null;
            }
        }

        #region Amount Related

        /// <summary>
        /// Gets No Culture Specific Amount For Display
        /// </summary>
        /// <param name="Amount">Amount as decimal</param>
        /// <returns>Returns a string</returns>
        public static string GetNoCultureSpecificAmountForDisplay(decimal Amount)
        {
            NumberFormatInfo f = CultureInfo.InvariantCulture.NumberFormat;

            return Amount.ToString(f);
        }

        /// <summary>
        /// Gets No Culture Specific Amount For Display
        /// </summary>
        /// <param name="Amount">Amount as double</param>
        /// <returns>Returns a string</returns>
        public static string GetNoCultureSpecificAmountForDisplay(double Amount)
        {
            NumberFormatInfo f = CultureInfo.InvariantCulture.NumberFormat;

            return Amount.ToString(f);
        }

        /// <summary>
        /// Gets No Culture Specific Amount
        /// </summary>
        /// <param name="Amount">Amount as string</param>
        /// <returns>Returns a string</returns>
        public static string GetNoCultureSpecificAmount(string Amount)
        {
            NumberFormatInfo f = CultureInfo.InvariantCulture.NumberFormat;

            return Amount.ToString(f);
        }

        /// <summary>
        /// Gets No Culture Specific Amount
        /// </summary>
        /// <param name="Amount">Amount as string</param>
        /// <returns>Returns a decimal</returns>
        public static decimal GetNoCultureSpecificAmountInDecimalFromString(string Amount)
        {
            return decimal.Parse(Amount, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets No Culture Specific Amount
        /// </summary>
        /// <param name="Amount">Amount as string</param>
        /// <returns>Returns a double</returns>
        public static double GetNoCultureSpecificAmountInDoubleFromString(string Amount)
        {
            return double.Parse(Amount, CultureInfo.InvariantCulture);
        }


        /// <summary>
        /// Gets Currency Symbol as text
        /// </summary>
        /// <param name="ri">RegionInfo</param>
        /// <param name="IsForPaymentGateway">Is for payment gateway</param>
        /// <returns>Returns a string</returns>
        public static string GetCurrencySymbolInText(RegionInfo ri, bool IsForPaymentGateway)
        {
            return ri.ISOCurrencySymbol;
        }
        /// <summary>
        /// Gets Currency Symbol as text
        /// </summary>
        /// <param name="ri">RegionInfo</param>
        /// <returns>Returns a string</returns>
        public static string GetCurrencySymbolInText()
        {
            return GetCurrencySymbolInText(GetRegionInfoByCultureCode(Thread.CurrentThread.CurrentCulture.ToString()));
        }

        /// <summary>
        /// Gets Currency Symbol as text
        /// </summary>
        /// <param name="ri">RegionInfo</param>
        /// <returns>Returns a string</returns>
        public static string GetCurrencySymbolInText(RegionInfo ri)
        {
            if (ri.ISOCurrencySymbol == _chISOCurSymbol)
                return _chOverrideCurSymbol;
            else
                return ri.ISOCurrencySymbol;
            //return ri.ISOCurrencySymbol;
        }

        /// <summary>
        /// Gets Currency Symbol By CultureCode as text
        /// </summary>
        /// <param name="CultureCode">CultureCode</param>
        /// <returns>Returns a string</returns>
        public static string GetCurrencySymbolInTextByCultureCode(string CultureCode)
        {
            return GetCurrencySymbolInText(GetRegionInfoByCultureCode(CultureCode));
        }

        /// <summary>
        /// Gets Culture Specific Amount With ISO Currency Symbol
        /// </summary>
        /// <param name="Amount">Amount as decimal</param>
        /// <returns>Returns a string</returns>
        public static string GetCultureSpecificAmountWithISOCurrencySymbol(decimal Amount)
        {
            //return GetRegionInfoByCultureCode(Thread.CurrentThread.CurrentCulture.ToString()).ISOCurrencySymbol + "&nbsp;" + Amount.ToString();
            return GetCurrencySymbolInText(GetRegionInfoByCultureCode(Thread.CurrentThread.CurrentCulture.ToString())) + "&nbsp;" + Amount.ToString();
        }

        /// <summary>
        /// Gets Culture Specific Amount by appending ISO Currency Symbol after amount
        /// </summary>
        /// <param name="Amount">Amount as decimal</param>
        /// <returns>Returns a string</returns>
        public static string GetCultureSpecificAmountWithISOCurrencySymbolAfter(decimal Amount)
        {
            //return GetRegionInfoByCultureCode(Thread.CurrentThread.CurrentCulture.ToString()).ISOCurrencySymbol + "&nbsp;" + Amount.ToString();
            return Amount.ToString() + " " + GetCurrencySymbolInText(GetRegionInfoByCultureCode(Thread.CurrentThread.CurrentCulture.ToString()));
        }

        #endregion Amount Related





    }
}
