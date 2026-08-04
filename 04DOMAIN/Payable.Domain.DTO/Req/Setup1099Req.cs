using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Req
{
    public class Setup1099Req
    {
        public class Load1099SetupVendorsReq
        {
            public string CorporationId { get; set; }
        }
        public class LoadExcludeSettings
        {
            public string ClientID { get; set; }

            // public string Name { get; set; }
        }
        //public class SaveExcludeSettingsReq
        //{
        //    public List<ExcludeSettings> ExcludesettingsList = new List<ExcludeSettings>();
        //}
        public class ExcludeSettingsSaveReq
        {
            public Int32 ID { get; set; }
            public string ClientID { get; set; }

            public string Name { get; set; }
            public short Status { get; set; }

        }

        public class VendorDetailsOf1099Req
        {
            public string CorpID { get; set; }
            public List<string> VendorsList { get; set; }

            public short SortBy { get; set; }

            public short OrderBy { get; set; }
            public short FormType { get; set; }

            public short AmountFiltetType { get; set; }

            public decimal AmountToFilter { get; set; }

            public string ClientID { get; set; }

            public DateTime? FromDate { get; set; }

            public DateTime? ToDate { get; set; }
            public short VendorStatus { get; set; }

        }
        public class BoxLinesReq
        {
            public short? Type { get; set; }///Misc or NEC
        }
        public class UpdateCOAMappingwithBoxLinesReq
        {
            public bool IsFromChangeMapp { get; set; }

            public string Number { get; set; }

            public string RequestID { get; set; }


            // public bool isNew { get; set; }

            public List<COAMappingwithBoxLines> COAChangeMappings { get; set; } = new List<COAMappingwithBoxLines>();
        }

        public class COAMappingwithBoxLines
        {
            public short? Type { get; set; }///Misc or NEC
            public string COA { get; set; }
            public string COAName { get; set; }

            public string COAType { get; set; }
            public int BoxLineID { get; set; }
            public string BoxLineName
            {
                get; set;

            }
            public string ParentID { get; set; }
            public string ReqID { get; set; }
            public string VendorID { get; set; }

        }

        public class DeleteChangeMapTempReq
        {
            public string RequestID { get; set; }
        }
        public class ThresholdReq
        {
            public string ClientID { get; set; }

            public short? Type { get; set; } ///Misc or NEC

            public decimal Amount { get; set; }
            public int BoxLineID { get; set; }
        }

        public class UpdateThresholdReq
        {
            public short? Type { get; set; } ///Misc or NEC
            public List<Threshold> UpdateThresholds { get; set; } = new List<Threshold>();
        }
        public class Threshold
        {
            public string ID { get; set; }
            public string ClientID { get; set; }

            public short? Type { get; set; } ///Misc or NEC

            public decimal Amount { get; set; }
            public int BoxLineID { get; set; }
            public DateTime? CreatedOn { get; set; }
            public string CreatedBy { get; set; }
            public DateTime? ModifiedOn { get; set; }
            public string ModifiedBy { get; set; }

            public short? Status { get; set; }            
                
        }

        public class ChangeMappingTempReq
        {
            // public List<Mapping> TempChangeMappings { get; set; } = new List<Mapping>();
            public string VendorID { get; set; }

            public string AccountID { get; set; }
            public int BoxLineID { get; set; }
        }
        public class Mapping
        {
            public string RequestID { get; set; }

            public string VendorID { get; set; }

            public string AccountID { get; set; }
            public int BoxLineID { get; set; }

        }
        public class ExcelExportReq
        {
            public string FileName { get; set; }
            public List<ColumnDto> Columns { get; set; }
            public List<List<DataDto>> Data { get; set; } = new List<List<DataDto>>();
            public DateTime? FromDate
            {

                get
                {
                    if (string.IsNullOrWhiteSpace(ShortFromDate))
                        return null;

                    if (DateTime.TryParseExact(ShortFromDate, "dd/MM/yyyy",
                                               CultureInfo.InvariantCulture,
                                               DateTimeStyles.None, out var fromDate))
                    {
                        return fromDate;
                    }

                    return null;
                }
            }
            public DateTime? ToDate
            {
                get
                {
                    if (string.IsNullOrWhiteSpace(ShortToDate))
                        return null;

                    if (DateTime.TryParseExact(ShortToDate, "dd/MM/yyyy",
                                               CultureInfo.InvariantCulture,
                                               DateTimeStyles.None, out var toDate))
                    {
                        return toDate;
                    }

                    return null;
                }
            }
            public string ShortFromDate { get; set; } = string.Empty;
            public string ShortToDate { get; set; } = string.Empty;
            public string Filter { get; set; }
            public string ManagementGroup { get; set; }
            public int AddFormula { get; set; }
            public int DateFormate { get; set; }
            public int ColumnFreezeNumber { get; set; }
            public bool IsFirstRequest { get; set; } = true;
            public bool IsCombinationOfExport { get; set; } = false;
        }


        public class ColumnDto
        {
            public string Item1 { get; set; }
            public int? Item2 { get; set; }
        }

        public class DataDto
        {
            public object? Item1 { get; set; }
            public object? Item2 { get; set; }
        }

    }

}