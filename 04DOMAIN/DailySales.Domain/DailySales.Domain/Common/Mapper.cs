using Common.Domain.DTO.App;
using Common.Domain.DTO.Enums;
using DailySales.Domain.DataModel;
using DailySales.Domain.DTO.Enums;
using DailySales.Domain.DTO.Model;
using DailySales.Domain.DTO.Req;
using DataModel.Domain.DataModel;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using DsdepositInfoDetails = DailySales.Domain.DataModel.DsdepositInfoDetails;

namespace DailySales.Domain.Common
{
    public class Mapper
    {
        public static DailyConfigInputEntry MapDailyConfigInputEntry(DailySaleDTO data, byte[] userID, short? impType, DailyConfigInputEntry dailyConfigInputEntry = null)
        {
            if (dailyConfigInputEntry != null)
            {
                //dailyConfigInputEntry.ApprovalLevel = Convert.ToInt16(data.ApprovalStatus);
                dailyConfigInputEntry.ModofiedBy = userID;
            }
            else
            {
                dailyConfigInputEntry = dailyConfigInputEntry != null ? dailyConfigInputEntry : new DailyConfigInputEntry();
                dailyConfigInputEntry.Id = new PFAID(data.ID).UID;
                dailyConfigInputEntry.CorporationId = new PFAID(data.CorpID).UID;
                dailyConfigInputEntry.SaleDate = data.SaleDate;
                dailyConfigInputEntry.StoreId = !string.IsNullOrEmpty(data.Pcid) && data.Pcid != "0" ? new PFAID(data.Pcid).UID : null;
                dailyConfigInputEntry.CreatedDate = DateTime.Now;
                dailyConfigInputEntry.ModifiedDate = DateTime.Now;
                dailyConfigInputEntry.AssignedTo = !string.IsNullOrEmpty(data.AssignedTo) ? null : new PFAID(data.AssignedTo).UID;
                dailyConfigInputEntry.CreatedBy = userID;
                //dailyConfigInputEntry.ModofiedBy = userID;
                dailyConfigInputEntry.Status = (short)ApprovalStatus.UnApproved;
                dailyConfigInputEntry.ApprovalStatus = (short)ApprovalStatus.Entry;
                dailyConfigInputEntry.ImportType = 2;//impType == 0 ? dailyConfigInputEntry.ImportType : impType;
                dailyConfigInputEntry.IsAttachment = 1;
                dailyConfigInputEntry.IsAutomatic = impType == 2 ? (short?)ImportType.Automatic : (short?)ImportType.Imported;
            }
            return dailyConfigInputEntry;
        }

        public static DailyConfigInputEntryAr MapDailyConfigInputEntryAr(DailyConfigLine data, decimal? Amount, byte[] DailyConfigInputID, DailyConfigInputEntryAr dailyConfigInputEntryAr = null)
        {
            if (dailyConfigInputEntryAr != null)
            {
                dailyConfigInputEntryAr.Id = dailyConfigInputEntryAr.Id;
                dailyConfigInputEntryAr.DailyConfigInputId = DailyConfigInputID;
                dailyConfigInputEntryAr.Amount = Amount ?? 0;
                dailyConfigInputEntryAr.SourceId = data.Id;
                dailyConfigInputEntryAr.Status = Convert.ToInt16(ApprovalType.Entry);
                dailyConfigInputEntryAr.CaccountId = data.CreditAccountId;
                dailyConfigInputEntryAr.DaccountId = data.DebitAccountId;
                dailyConfigInputEntryAr.Darid = dailyConfigInputEntryAr.Darid;
            }
            else
            {
                dailyConfigInputEntryAr = new DailyConfigInputEntryAr();
                dailyConfigInputEntryAr.Id = new PFAID().UID;
                dailyConfigInputEntryAr.DailyConfigInputId = DailyConfigInputID;
                dailyConfigInputEntryAr.Amount = Amount ?? 0;
                dailyConfigInputEntryAr.SourceId = data.Id;
                dailyConfigInputEntryAr.Status = Convert.ToInt16(ApprovalType.Entry);
                dailyConfigInputEntryAr.CaccountId = data.CreditAccountId;
                dailyConfigInputEntryAr.DaccountId = data.DebitAccountId;
            }
            return dailyConfigInputEntryAr;
        }


        public static DailyConfigInputEntryRevenue MapDailyConfigInputRevenue(DailySaleEntryDTO data, string ID, DailyConfigInputEntryRevenue revenue = null)
        {
            if (revenue != null)
            {
                revenue.Amount = (short?)(data.Amount) ?? revenue.Amount;
            }
            else
            {
                revenue = new DailyConfigInputEntryRevenue();
                revenue.Id = new PFAID().UID;
                revenue.Amount = data.Amount;
                revenue.DailyConfigInputId = new PFAID(ID).UID;
                revenue.LineId = new PFAID(data.LineID).UID;
                revenue.Order = 0;
            }
            return revenue;
        }

        public static DailyConfigVerification MapDailyConfigVerification(byte[] lineID, decimal? Amount, byte[] DailyConfigInputID, short type, short isEnding, DailyConfigVerification verifcation = null)
        {
            if (verifcation != null)
            {
                verifcation.Amount = Amount ?? 0;
            }
            else
            {
                verifcation = verifcation == null ? new DailyConfigVerification() : verifcation;
                verifcation.DailyConfigInputId = DailyConfigInputID;
                verifcation.Type = type;
                verifcation.LineId = new PFAID(lineID).UID;
                verifcation.Amount = Amount ?? 0;
                verifcation.Status = (short)Status.Active;
                verifcation.IsEnding = isEnding;
            }
            return verifcation;
        }

        public static DailyConfigInputEntryRevenue MapDailyConfigInputEntryRevenue(byte[] ID, byte[] DailyConfigInputID, byte[] lineID, decimal? Amount, byte[] CAccountID, byte[] DAccountID, DailyConfigInputEntryRevenue revenue = null)
        {
            if (revenue != null)
            {
                //revenue.Id = revenue.Id;
                //revenue.DailyConfigInputId = DailyConfigInputID;
                revenue.LineId = lineID;
                revenue.Amount = Amount ?? 0;
                //revenue.Order = 0;
                //revenue.Status = Convert.ToInt16(ApprovalType.Entry);
                revenue.CaccountId = CAccountID != null && CAccountID == null ? null : CAccountID ?? null;
                revenue.DaccountId = DAccountID != null && DAccountID == null ? null : DAccountID ?? null;
            }
            else
            {
                revenue = new DailyConfigInputEntryRevenue();
                revenue.Id = ID;
                revenue.DailyConfigInputId = DailyConfigInputID;
                revenue.LineId = lineID;
                revenue.Amount = Amount ?? 0;
                revenue.Order = 0;
                revenue.Status = Convert.ToInt16(ApprovalType.Entry);
                revenue.CaccountId = CAccountID != null && CAccountID == null ? null : CAccountID ?? null;
                revenue.DaccountId = DAccountID != null && DAccountID == null ? null : DAccountID ?? null;
            }
            return revenue;
        }
        public static DailyConfigInputEntryReceipt MapDailyConfigInputEntryReceipt(byte[] ID, byte[] DailyConfigInputID, byte[] lineID, decimal? Amount, byte[] CAccountID, byte[] DAccountID, DailyConfigInputEntryReceipt receipt = null)
        {
            if (receipt != null)
            {
                // receipt.Id = receipt.Id;
                //receipt.DailyConfigInputId = DailyConfigInputID;
                receipt.SourceId = lineID;
                receipt.Amount = Amount ?? 0;
                // receipt.Status = Convert.ToInt16(ApprovalType.Entry);
                receipt.CaccountId = CAccountID != null && CAccountID == null ? null : CAccountID ?? null; /*CAccountID;*/
                receipt.DaccountId = DAccountID != null && DAccountID == null ? null : DAccountID ?? null; /*DAccountID;*/
                //receipt.Drid = receipt.Drid;
            }
            else
            {
                receipt = new DailyConfigInputEntryReceipt();
                receipt.Id = ID;
                receipt.DailyConfigInputId = DailyConfigInputID;
                receipt.SourceId = lineID;
                receipt.Amount = Amount ?? 0;
                receipt.Status = Convert.ToInt16(ApprovalType.Entry);
                receipt.CaccountId = CAccountID != null && CAccountID == null ? null : CAccountID ?? null; /*CAccountID;*/
                receipt.DaccountId = DAccountID != null && DAccountID == null ? null : DAccountID ?? null; /*DAccountID;*/
            }
            return receipt;
        }

        public static DailyConfigInputEntryAr MapDailyConfigInputEntryAr(byte[] ID, byte[] DailyConfigInputID, byte[] lineID, decimal? Amount, byte[] CAccountID, byte[] DAccountID, DailyConfigInputEntryAr ardetails = null)
        {
            if (ardetails != null)
            {
                //ardetails.Id = ardetails.Id;
                //ardetails.DailyConfigInputId = DailyConfigInputID;
                ardetails.SourceId = lineID;
                ardetails.Amount = Amount ?? 0;
                //ardetails.Status = Convert.ToInt16(ApprovalType.Entry);
                ardetails.CaccountId = CAccountID != null && CAccountID == null ? null : CAccountID ?? null;
                ardetails.DaccountId = DAccountID != null && DAccountID == null ? null : DAccountID ?? null;
                //ardetails.Darid = ardetails.Darid;
            }
            else
            {
                ardetails = new DailyConfigInputEntryAr();
                ardetails.Id = ID;
                ardetails.DailyConfigInputId = DailyConfigInputID;
                ardetails.SourceId = lineID;
                ardetails.Amount = Amount ?? 0;
                ardetails.Status = Convert.ToInt16(ApprovalType.Entry);
                ardetails.CaccountId = CAccountID != null && CAccountID == null ? null : CAccountID ?? null;
                ardetails.DaccountId = DAccountID != null && DAccountID == null ? null : DAccountID ?? null;
            }
            return ardetails;
        }

        public static DailyConfigInputEntryStatistics MapDailyConfigInputEntryStatistics(byte[] ID, byte[] DailyConfigInputID, byte[] lineID, decimal? Amount, byte[] CAccountID, byte[] DAccountID, DailyConfigInputEntryStatistics statistics = null)
        {
            if (statistics != null)
            {
                //statistics.Id = statistics.Id;
                //statistics.DailyConfigInputId = DailyConfigInputID;
                statistics.LineId = lineID;
                statistics.Amount = Amount ?? 0;
                //statistics.Status = Convert.ToInt16(ApprovalType.Entry);
                statistics.CaccountId = CAccountID != null && CAccountID == null ? null : CAccountID ?? null;
                statistics.DaccountId = DAccountID != null && DAccountID == null ? null : DAccountID ?? null;
                //statistics.Dsid = statistics.Dsid;
            }
            else
            {
                statistics = new DailyConfigInputEntryStatistics();
                statistics.Id = ID;
                statistics.DailyConfigInputId = DailyConfigInputID;
                statistics.LineId = lineID;
                statistics.Amount = Amount ?? 0;
                statistics.Status = Convert.ToInt16(ApprovalType.Entry);
                statistics.CaccountId = CAccountID != null && CAccountID == null ? null : CAccountID ?? null;
                statistics.DaccountId = DAccountID != null && DAccountID == null ? null : DAccountID ?? null;

            }
            return statistics;
        }

        public static Pmsinfo Mappmsinfo(Pmsinfo pmsInfo, PMSInfoDTO pinfo)
        {
            pmsInfo.Pmsname = pinfo.pmsName;
            pmsInfo.Status = (short)Status.Active;
            return pmsInfo;
        }


        public static PmsclientInfo Mappmsclientinfo(PmsclientInfo pmscinfo, PMSInfoDTO pinfo, long id)
        {
            if (pmscinfo == null)
                pmscinfo = new PmsclientInfo();
            //
            //if (pmscinfo.Id == 0)
            //{
            //    //pmscinfo.Id = id;
            //    pmscinfo.ClientId = new PFAID(pinfo.ClientID).UID;
            //    pmscinfo.UrlId = pinfo.URLID;
            //    pmscinfo.Status = (short)Status.Active;
            //}
            //else
            //{
            //    //pmscinfo.id = id;
            //    pmscinfo.ClientId = new PFAID(pinfo.ClientID).UID;
            //    pmscinfo.UrlId = pmscinfo.UrlId;
            //    pmscinfo.Status = (short)Status.Active;
            //}
            pmscinfo.ClientId = new PFAID(pinfo.ClientID).UID;
            pmscinfo.UrlId = pinfo.URLID;
            pmscinfo.Status = (short)Status.Active;
            return pmscinfo;
        }


        public static PmsfacilityMap Mappmsfacmap(PmsfacilityMap pmsfacmap, PMSFacilityMapDTO pinfo, long pmsinfoID)
        {
            if (pmsfacmap == null)
                pmsfacmap = new PmsfacilityMap();

            pmsfacmap.FacilityId = pinfo.FacilitityID;

            pmsfacmap.PmsclientInfoId = pinfo.PMSClientInfoID;
            pmsfacmap.FacilityId = pinfo.FacilitityID;
            pmsfacmap.CorporationId = new PFAID(pinfo.CorpID).UID;
            pmsfacmap.Pcid = pinfo != null && pinfo.StoreID != null ? pinfo.StoreID : null;
            pmsfacmap.Status = (short)Status.Active;
            pmsfacmap.PmsinfoId = pmsinfoID;
            pmsfacmap.Id = pinfo.ID;
            return pmsfacmap;
        }


        public static PmsfacilityMapDetails MappmsfacmapDetails(PmsfacilityMapDetails pmsfacmap, PMSFacilityMapDetailsDTO pinfo, long id)
        {
            //if (pmsfacmap == null)
            //    pmsfacmap = new PmsfacilityMapDetails();
            pmsfacmap.PmsfacilityMapId = id;

            pmsfacmap.FileName = pinfo.FileName;
            pmsfacmap.Status = (short)Status.Active;
            return pmsfacmap;
        }

        //public static PmscorporationMapping Mappmscorpmap(PmscorporationMapping pmscmap, PMSInfoDTO pinfo)
        //{
        //    PmscorporationMapping pmscmapp = new PmscorporationMapping();
        //    if (pmscmap == null)
        //    {
        //        pmscmapp.CorporationId = new PFAID(pinfo.CorpID).UID;
        //        pmscmapp.FacilityId = pinfo.FacilitityID;
        //        pmscmapp.Status = (short)Status.Active;
        //        //pmscmap.Type = (short)PMS.Opera;
        //        pmscmapp.Type = (short)(EnumExtensions.GetEnumValue<PMS>(pinfo.Name));
        //        //pmscmap.StoreId = null;
        //    }
        //    else
        //    {
        //        pmscmapp.Id = pmscmap.Id;
        //        pmscmapp.CorporationId = pmscmap.CorporationId;
        //        pmscmapp.FacilityId = pmscmap.FacilityId == null ? pinfo.FacilitityID : pmscmap.FacilityId;
        //        pmscmapp.Status = pmscmap.Status;
        //        pmscmapp.Type = pmscmap.Type;
        //        pmscmapp.StoreId = pmscmap.StoreId;
        //    }
        //    return pmscmapp;
        //}

        public static PmscorporationMappindDetails MapPmsCorpMapdetails(PmscorporationMappindDetails pmscmap, DailySalePMSDTO dspmsdto, long pmsmapid)
        {
            pmscmap = pmscmap == null ? new PmscorporationMappindDetails() : pmscmap;
            pmscmap.PmscorpMappingId = pmsmapid;
            pmscmap.LineId = dspmsdto.LineID;
            pmscmap.AccountId = dspmsdto.ReportName;
            pmscmap.AccountDescription = dspmsdto.LineItemDescription;
            pmscmap.PmscurrencyType = dspmsdto.PSCurrentStat;
            pmscmap.Type = dspmsdto.Type;
            pmscmap.DeptType = dspmsdto.DeptType;
            pmscmap.IsEnding = dspmsdto.IsEnding;
            pmscmap.Seq = dspmsdto.SEQ;
            pmscmap.Status = dspmsdto.status;
            pmscmap.Category = dspmsdto.Category;
            pmscmap.SubCategory = dspmsdto.SubCategory;
            pmscmap.IsConfigMismatch = dspmsdto.IsConfigMismatch;
            pmscmap.SignMapping = dspmsdto.SignMapping;
            return pmscmap;
        }
        public static UnMappedDailyConfigInputEntry unMapDailyConfigEntry(DateTime SaleDate, long pmsmapId, long? pmsmapdetId, Decimal? Amount, decimal? NetAmount, decimal? BalanceAmount)
        {
            UnMappedDailyConfigInputEntry unmapdcent = new UnMappedDailyConfigInputEntry();
            unmapdcent.SaleDate = SaleDate;
            unmapdcent.PmscorpMapId = pmsmapId;
            unmapdcent.PmscorpMapDetailsId = pmsmapdetId;
            unmapdcent.Amount = Amount;
            unmapdcent.Status = (short)Status.Active;
            unmapdcent.NetAmount = NetAmount;
            unmapdcent.BalanceAmount = BalanceAmount;
            return unmapdcent;
        }

        //public static DailyConfigInputEntry MapDailyConfigInputEntry(DailySaleDTO data, DailyConfigInputEntry dailyConfigInputEntry = null, string[] excludedProperties = null)
        //{
        //    if (dailyConfigInputEntry == null)
        //    {
        //        dailyConfigInputEntry = new DailyConfigInputEntry();
        //        dailyConfigInputEntry.Id = new PFAID().UID;
        //        dailyConfigInputEntry.CorporationId = new PFAID(data.CorpID).UID;
        //        dailyConfigInputEntry.SaleDate = DateTime.Now;
        //        dailyConfigInputEntry.RevenueJournalId = new PFAID(data.RevenuveJournalID).UID;
        //        dailyConfigInputEntry.Status = Convert.ToInt16(ApprovalType.Entry);
        //        dailyConfigInputEntry.CreatedDate = DateTime.Now;
        //        dailyConfigInputEntry.AssignedTo = new PFAID(data.AssignedTo).UID;
        //        dailyConfigInputEntry.ApprovalLevel = 1;
        //    }

        //    // Properties to exclude (e.g., "DCID")
        //    excludedProperties = excludedProperties ?? new string[0];

        //    foreach (var property in typeof(DailyConfigInputEntry).GetProperties())
        //    {
        //        if (!excludedProperties.Contains(property.Name) && property.Name != "DCID")
        //        {
        //            var dtoProperty = typeof(DailySaleDTO).GetProperty(property.Name);
        //            if (dtoProperty != null)
        //            {
        //                var value = dtoProperty.GetValue(data);
        //                property.SetValue(dailyConfigInputEntry, value);
        //            }
        //        }
        //    }

        //    return dailyConfigInputEntry;
        //}

        public static Dsdeposits MapDsDeposit(DsCashChecksReq req, string UserId, Dsdeposits original = null)
        {
            Dsdeposits result;

            if (original == null)
            {
                result = new Dsdeposits
                {
                    CorporationId = new PFAID(req.CorporationID).UID,
                    FromDate = req.FromDate,
                    ToDate = req.ToDate,
                    DepositDate = req.DepositeDate,
                    Memo = req.Memo,
                    TotalAmount = req.Totalamount,
                    TotalDeposited = req.TotalDeposite,
                    Difference = req.difference,
                    CreatedBy = new PFAID(UserId).UID,
                    CreatedOn = DateTime.Now,
                    IsAttachment = req.IsAttachment,
                    AttachmentId = new PFAID().UID,
                    Status = (short)Status.InActive,
                    StoreId = new PFAID(req.PCID).UID,
                };
            }
            else
            {
                result = original;
                result.Memo = req.Memo;
                result.TotalAmount = req.Totalamount;
                result.TotalDeposited = req.TotalDeposite;
                result.DepositDate = req.DepositeDate;
                result.Difference = req.difference;
                result.ModifiedBy = new PFAID(UserId).UID;
                result.ModifiedOn = DateTime.Now;
                result.IsAttachment = req.IsAttachment;
            }

            return result;
        }


        public static DsdepositInfo MapDsDepositInfo(Depositeinfo Req, DsdepositInfo original = null)
        {
            DsdepositInfo result;
            if (original == null)
            {
                result = new DsdepositInfo
                {
                    DailyConfigInputId = new PFAID(Req.DailyConfigInputId).UID,
                    Difference = Req.Difference,
                    Comments = Req.Comments,
                    Status = (short)Status.Active,
                    SaleDate = Req.SaleDate,
                    AccountId = string.IsNullOrEmpty(Req.AdjustmentAccountId) ? null : new PFAID(Req.AdjustmentAccountId).UID
                };
            }
            else
            {
                result = original;
                result.Difference = Req.Difference;
                result.Comments = Req.Comments;
                result.AccountId = string.IsNullOrEmpty(Req.AdjustmentAccountId) ? null : new PFAID(Req.AdjustmentAccountId).UID;
            }

            return result;

        }

        public static DsdepositInfoDetails MapDsDepositInfoDetails(InfoDetail Req, DsdepositInfoDetails original = null)
        {
            DsdepositInfoDetails result;
            if (original == null)
            {
                result = new DsdepositInfoDetails
                {
                    DailySalesReceiptId = string.IsNullOrEmpty(Req.ReceiptId) ? null : new PFAID(Req.ReceiptId).UID,
                    LineId = new PFAID(Req.LineId).UID,
                    LineName = Req.LineName,
                    LineOrder = Req.LineOrder,
                    Amount = Req.EditAmount,
                    Status = (short)Status.Active,
                    Difference = Req.Difference

                };
            }
            else if (original != null && !Req.IsReconciled)
            {
                result = original;
                result.DailySalesReceiptId = string.IsNullOrEmpty(Req.ReceiptId) ? null : new PFAID(Req.ReceiptId).UID;
                result.LineId = new PFAID(Req.LineId).UID;
                result.LineName = Req.LineName;
                result.LineOrder = Req.LineOrder;
                result.Amount = Req.EditAmount;
                result.Difference = Req.Difference;
            }
            else
            {
                result = original;
            }
            return result;
        }
        //TODO : Removed async as it should be used when await is there.
        public static PostJournalEntryReq MapJournalEntry(InfoDetail info, string CorpID, string PCID, string UserID, string AccountId, byte[] AttachmentId, DateTime SaleDate)
        {
            PostJournalEntryReq req = new PostJournalEntryReq();
            if (!string.IsNullOrEmpty(info.JournalEntryid) && info.IsReconciled)
            {
                return null;
            }
            else
            {

                req.CorporationID = CorpID;
                req.PCID = PCID;
                req.JournalEntryId = string.IsNullOrEmpty(info.JournalEntryid) ? null : new PFAID(info.JournalEntryid).UID;
                req.AccountId = AccountId;
                req.ReceiptId = string.IsNullOrEmpty(info.ReceiptId) ? null : info.ReceiptId;
                req.Amount = info.Difference;
                req.LineName = info.LineName;
                req.LineId = info.LineId;
                req.UserId = UserID;
                req.AttachmentId = AttachmentId;
                req.SaleDate = SaleDate;
                return req;
            }


        }


    }


}
