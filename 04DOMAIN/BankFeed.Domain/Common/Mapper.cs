//using AutoMapper.Internal;
using BankFeed.Domain.DataModel;
using BankFeed.Domain.DTO;
using BankFeed.Domain.DTO.Model;
using BankFeed.Domain.DTO.Req;
using BankFeed.Domain.Enums;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Resp;
using Common.Domain.DTO.Enums;
//using DataModel.Domain.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Common.Domain.DTO.Model;
using Microsoft.SqlServer.Server;
using Common.Domain.DTO.Extensions;
using Azure.Core;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BankFeed.Domain.Common
{
    public static class Mapper
    {
        #region FeedRule
        /// <summary>
        /// Maps feedrule entry request to feedrule
        /// </summary>
        /// <param name="Entity"></param>
        /// <param name="origEntity"></param>
        /// <returns>Mapped feedrule</returns>
        public static FeedRule MapFeedrule(FeedRuleDTO Entity, FeedRule? origEntity = null)
        {
            if (origEntity != null)
            {
                origEntity.SetUpFor = Entity.SetUpFor;
                origEntity.Memo = !string.IsNullOrEmpty(Entity.Memo) ? Entity.Memo : null;
                origEntity.AutoApplyEnable = Entity.AutoApplyEnable;
                origEntity.RulePriority = (short)Entity.Priority;
                origEntity.QueryMatchType = (short)Entity.QueryMatchType;
                origEntity.IsCloneEnable = Entity.CloneEnable;

                return origEntity;
            }
            else
            {
                return new FeedRule()
                {

                    SetUpFor = Entity.SetUpFor,
                    Memo = !string.IsNullOrEmpty(Entity.Memo) ? Entity.Memo : null,
                    AutoApplyEnable = Entity.AutoApplyEnable,
                    RulePriority = (short)Entity.Priority,
                    QueryMatchType = (short)Entity.QueryMatchType,
                    BankOrCreditAccountId = !string.IsNullOrEmpty(Entity.BankOrCreditAccountID) ? new PFAID(Entity.BankOrCreditAccountID).UID : null,
                    CorporationId = !string.IsNullOrEmpty(Entity.CorpID) ? new PFAID(Entity.CorpID).UID : null,
                    FeedTransType = (short)Entity.AmountType,
                    Status = Entity.RuleStatus > 0 ? (short)Entity.RuleStatus : (short)Status.Active,
                    IsCloneEnable = Entity.CloneEnable
                };
            }
        }

        /// <summary>
        /// Maps feedrule entry request to feedrule details
        /// </summary>
        /// <param name="Entity"></param>
        /// <param name="FeedRuleEntity"></param>
        /// <param name="OrigEntity"></param>
        /// <returns>List of mapped feedruledetails</returns>
        public static List<FeedRuleDetails> MapRuleDetails(List<FeedRuleDivisinDTO> Entity, FeedRule FeedRuleEntity, List<FeedRuleDetails>? OrigEntity = null)
        {
            List<FeedRuleDetails> ruleDet = new List<FeedRuleDetails>();
            FeedRuleDetails feedRuleDetail = null;
            foreach (var rule in Entity)
            {
                if (rule.ID > 0)
                {
                    feedRuleDetail = OrigEntity.Where(e => e.Id == rule.ID).FirstOrDefault();
                    feedRuleDetail.RuleType = (short)rule.RuleOn;
                    feedRuleDetail.FilterType = (short)rule.Filter;
                    feedRuleDetail.Val = (feedRuleDetail.RuleType == Convert.ToInt16(FeedruleTypeEnum.Description)) ? rule.RuleDiscription : Convert.ToString(rule.RuleAmount);
                }
                else
                {
                    feedRuleDetail = new FeedRuleDetails();
                    feedRuleDetail.FeedRule = FeedRuleEntity;
                    feedRuleDetail.FeedRuleId = FeedRuleEntity.Id;
                    feedRuleDetail.RuleType = (short)rule.RuleOn;
                    feedRuleDetail.FilterType = (short)rule.Filter;
                    feedRuleDetail.Val = (feedRuleDetail.RuleType == Convert.ToInt16(FeedruleTypeEnum.Description)) ? rule.RuleDiscription : Convert.ToString(rule.RuleAmount);
                    feedRuleDetail.Status = (short)Status.Active;

                }
                ruleDet.Add(feedRuleDetail);
            }
            return ruleDet;
        }

        /// <summary>
        /// Maps feedrule entry request to feedrule mapping
        /// </summary>
        /// <param name="Entity"></param>
        /// <param name="FeedRuleEntity"></param>
        /// <param name="origEntity"></param>
        /// <returns>Mapped FeedRuleMapping</returns>
        public static FeedRuleMapping MapRuleMatch(FeedRuleRequest1 Entity, FeedRule FeedRuleEntity, FeedRuleMapping? origEntity = null)
        {
            if (origEntity != null)
            {
                origEntity.AccountId = !string.IsNullOrEmpty(Entity.FeedRuleDTO.MapDetails.AccountID) ? new PFAID(Entity.FeedRuleDTO.MapDetails.AccountID).UID : null;
                origEntity.NameId = new PFAID(Entity.FeedRuleDTO.MapDetails.NameID).UID;
                origEntity.ActTransType = (short)Entity.FeedRuleDTO.MapDetails.TransactionType;
                origEntity.PaymentMethodId = !string.IsNullOrEmpty(Entity.FeedRuleDTO.MapDetails.PaymentMethodID) ? new PFAID(Entity.FeedRuleDTO.MapDetails.PaymentMethodID).UID : null;
                return origEntity;
            }
            else
            {
                return new FeedRuleMapping()
                {
                    Id = FeedRuleEntity.Id,
                    //IdNavigation = FeedRuleEntity,
                    CorpId = new PFAID(Entity.FeedRuleDTO.CorpID).UID,
                    AccountId = !string.IsNullOrEmpty(Entity.FeedRuleDTO.MapDetails.AccountID) ? new PFAID(Entity.FeedRuleDTO.MapDetails.AccountID).UID : null,
                    NameId = new PFAID(Entity.FeedRuleDTO.MapDetails.NameID).UID,
                    NameType = (short)Entity.FeedRuleDTO.MapDetails.NameType,
                    ActTransType = (short)Entity.FeedRuleDTO.MapDetails.TransactionType,
                    PaymentMethodId = !string.IsNullOrEmpty(Entity.FeedRuleDTO.MapDetails.PaymentMethodID) ? new PFAID(Entity.FeedRuleDTO.MapDetails.PaymentMethodID).UID : null
                };
            }
        }

        public static FeedRuleMapping MapRuleMatch1(FeedRuleRequest Entity, FeedRule FeedRuleEntity, FeedRuleMapping? origEntity = null)
        {
            if (origEntity != null)
            {
                origEntity.AccountId = !string.IsNullOrEmpty(Entity.MapDetails.AccountID) ? new PFAID(Entity.MapDetails.AccountID).UID : null;
                origEntity.NameId = new PFAID(Entity.MapDetails.NameID).UID;
                origEntity.ActTransType = (short)Entity.MapDetails.TransactionType;
                origEntity.PaymentMethodId = !string.IsNullOrEmpty(Entity.MapDetails.PaymentMethodID) ? new PFAID(Entity.MapDetails.PaymentMethodID).UID : null;

                return origEntity;
            }
            else
            {
                return new FeedRuleMapping()
                {
                    Id = FeedRuleEntity.Id,
                    //IdNavigation = FeedRuleEntity,
                    CorpId = new PFAID(Entity.CorpID).UID,
                    AccountId = !string.IsNullOrEmpty(Entity.MapDetails.AccountID) ? new PFAID(Entity.MapDetails.AccountID).UID : null,
                    NameId = new PFAID(Entity.MapDetails.NameID).UID,
                    NameType = (short)Entity.MapDetails.NameType,
                    ActTransType = (short)Entity.MapDetails.TransactionType,
                    PaymentMethodId = !string.IsNullOrEmpty(Entity.MapDetails.PaymentMethodID) ? new PFAID(Entity.MapDetails.PaymentMethodID).UID : null

                };
            }
        }
        #endregion

        #region Feed Import

        /// <summary>
        /// To Map Import data object to InstituteInfo
        /// </summary>
        /// <param name="Entity">Import request </param>
        /// <param name="OrigEntity">for update case - original InstituteInfo data</param>
        /// <param name="ProvRegEntity"></param>
        /// <returns>InstituteInfo</returns>
        public static FeedInstitute MapFeedInstitution(FeedImportRequest Entity, ProviderRegister ProvRegEntity, FeedInstitute? OrigEntity = null)
        {
            OrigEntity = new FeedInstitute()
            {
                ProviderRegId = ProvRegEntity.Id,
                //ProviderReg = ProvRegEntity,
                InsOffName = Entity.BankName,
                InsName = Entity.BankName,
                InsProviderId = Guid.NewGuid().ToString(),
                ProviderAccessId = "Bank-Feeds-Import",
                CreatedOn = DateTime.Now,
                Status = (short)Status.Active
            };

            return OrigEntity;
        }

        /// <summary>
        /// To Map Import data object to AccountInfo
        /// </summary>
        /// <param name="Entity">Import request </param>
        /// <param name="OrigEntity">for update case - original AccountInfo data</param>
        /// <param name="InstInfoEntity"></param>
        /// <returns>AccountInfo</returns>
        public static FeedAccount MapFeedAccount(FeedImportRequest Entity, FeedInstitute InstInfoEntity, DateTime SynchFrom, FeedAccount? OrigEntity = null)
        {
            if (OrigEntity == null)
            {
                OrigEntity = new FeedAccount()
                {
                    BankAccId = Entity.AccountNumber + Entity.BankName,
                    InsId = InstInfoEntity.Id,
                    Ins = InstInfoEntity,
                    AccName = Entity.BankName,
                    AccOffName = Entity.BankName,
                    AccountType = Entity.NimbleAccountType != null ? EnumExtensions.GetEnumValue<BankAccountTypeEnum>((short)Entity.NimbleAccountType).GetDisplayName() : BankAccountTypeEnum.Other.GetDisplayName(),/*Entity.NimbleAccountType,*/
                    AccSubType = Entity.NimbleAccountType != null ? EnumExtensions.GetEnumValue<BankAccountTypeEnum>((short)Entity.NimbleAccountType).GetDisplayName() : BankAccountTypeEnum.Other.GetDisplayName(),
                    AccNumber = !string.IsNullOrEmpty(Entity.AccountNumber) && Entity.AccountNumber.Length >= 4 ? Entity.AccountNumber : string.Empty,
                    AccEditName = Entity.BankName + ((!string.IsNullOrEmpty(Entity.AccountNumber)) ? ("  " + (Entity.AccountNumber.Length > 4 ? Entity.AccountNumber.Substring(Entity.AccountNumber.Length - 4) : "")) : ""),
                    Balance = Entity.EndingBalance,
                    AvlBalance = Entity.EndingBalance,
                    AccType = Entity.NimbleAccountType,
                    CreatedOn = DateTime.Now,
                    SynchFrom = SynchFrom,
                    Status = (short)Status.Active,
                    EndDate = Entity.EndDate
                };
            }
            else
            {
                OrigEntity.Balance = Entity.EndingBalance;
                OrigEntity.AvlBalance = Entity.EndingBalance;
                //OrigEntity.SynchFrom = SynchFrom;
                OrigEntity.EndDate = Entity.EndDate.HasValue ? Entity.EndDate : OrigEntity.EndDate;
            }
            return OrigEntity;
        }

        /// <summary>
        /// To map import data object to feed transaction model
        /// </summary>
        /// <param name="Entity"></param>
        /// <param name="AccEntity"></param>
        /// <param name="FeedInsEntity"></param>
        /// <param name="OrigEntity"></param>
        /// <returns></returns>
        public static List<FeedTransactions> MapFeedTransactions(FeedImportRequest Entity, FeedAccount AccEntity, FeedInstitute FeedInsEntity, string LogSeqID, DateTime LastSynchedon, List<FeedTransactions> existingImportTrans, List<FeedTransactions>? OrigEntity = null)
        {
            OrigEntity = new List<FeedTransactions>();
            FeedTransactions newTran = null;
            // DateTime maxdate = (from ts in Entity.ImportTransactions select ts.Date).Max();
            List<FeedTransactionRequest> feedTrans = Entity.ImportTransactions;
            if (Entity != null && Entity.ImportTransactions != null && Entity.ImportTransactions.Any())
            {
                //Entity.FeedAccId > 0 -ImportTransactions case we have to consider transactions from LastSynchedon
                //Entity.ImportID > 0- re-import case we have to consider transactions from LastSynchedon, if any duplicates that has to be ignored
                if (Entity.FeedAccId > 0)// || Entity.ImportID > 0) 
                {
                    feedTrans = (from g in Entity.ImportTransactions
                                 where g.Date >= LastSynchedon
                                 select g).ToList();
                }
            }

            foreach (var tran in feedTrans)
            {
                newTran = new FeedTransactions();
                //newTran.FeedRuleId
                newTran.FeedAccId = AccEntity.Id;
                newTran.FeedAcc = AccEntity;
                newTran.TransProviderId = Guid.NewGuid().ToString();
                newTran.Description = tran.Memo;
                newTran.CheckNumber = tran.CheckNo;
                //newTran.TransName= tran.na
                newTran.TransDate = tran.Date;
                //newTran.FeedRuleId = null;
                newTran.IsAutoRuleApplied = null;
                //newTran.TransPostType
                newTran.RunningBalance = Entity.EndingBalance;
                //newTran.PageNo = null;
                //newTran.LogSeqId = FeedInsEntity.Id.ToString();
                var amtfrom = Entity.AmountFrom;
                var amtmode = Entity.AmountMode;
                decimal CRValue = 0.0M;
                decimal DRValue = 0.0M;
                decimal.TryParse(tran.CR, out CRValue);
                decimal.TryParse(tran.DR, out DRValue);

                if (amtfrom == (short)ImportAmountFromEnum.SingleLine)
                    newTran.Amount = Math.Abs(tran.Amount);
                else
                    newTran.Amount = CRValue != 0 ? Math.Abs(CRValue) : Math.Abs(DRValue);

                if (amtfrom == (short)ImportAmountFromEnum.SingleLine && amtmode == (short)ImportAmountModeEnum.WithSigns)
                {
                    if (Entity.NimbleAccountType == (short)BankAccountTypeEnum.Credit)
                        newTran.TransType = tran.Amount >= 0 ? BankFeedActualtrTypeEnum.Payments.GetDisplayName() : BankFeedActualtrTypeEnum.Receipts.GetDisplayName();
                    else
                        newTran.TransType = tran.Amount >= 0 ? BankFeedActualtrTypeEnum.Receipts.GetDisplayName() : BankFeedActualtrTypeEnum.Payments.GetDisplayName();
                }
                else if (amtfrom == (short)ImportAmountFromEnum.SingleLine && amtmode == (short)ImportAmountModeEnum.WithCRDR)
                    newTran.TransType = (tran.DRCR.ToUpper() == "CR" || tran.DRCR.ToLower().Trim() == "credit") ? BankFeedActualtrTypeEnum.Receipts.GetDisplayName() : BankFeedActualtrTypeEnum.Payments.GetDisplayName();
                else
                    newTran.TransType = CRValue != 0 ? BankFeedActualtrTypeEnum.Receipts.GetDisplayName() : BankFeedActualtrTypeEnum.Payments.GetDisplayName();

                if (amtfrom == (short)ImportAmountFromEnum.SingleLine && amtmode == (short)ImportAmountModeEnum.WithSigns)
                    newTran.TransPostType = tran.Amount >= 0 ? (short)BankFeedActualtrTypeEnum.Receipts : (short)BankFeedActualtrTypeEnum.Payments;
                else if (amtfrom == (short)ImportAmountFromEnum.SingleLine && amtmode == (short)ImportAmountModeEnum.WithCRDR)
                    newTran.TransPostType = (tran.DRCR.ToUpper() == "CR" || tran.DRCR.ToLower().Trim() == "credit") ? (short)BankFeedActualtrTypeEnum.Receipts : (short)BankFeedActualtrTypeEnum.Payments;
                else if (amtfrom == (short)ImportAmountFromEnum.DoubleLIne)
                    newTran.TransPostType = CRValue != 0 ? (short)BankFeedActualtrTypeEnum.Receipts : (short)BankFeedActualtrTypeEnum.Payments;

                newTran.Status = (short)FeedTransStatusEnum.NewFeed;
                newTran.LogSeqId = LogSeqID;
                OrigEntity.Add(newTran);
            }
            return OrigEntity;
        }

        /// <summary>
        /// Map with AccountMapping
        /// </summary>
        /// <param name="Entity">Import request </param>
        /// <param name="OrigEntity">for update case - original FeedTransactions list data</param>
        /// <param name="AccEntity">Account Info table </param>
        /// <returns>AccountMapping</returns>
        public static FeedAccountMapping MapNimbleAccountMapping(FeedImportRequest Entity, FeedAccount AccEntity, FeedAccountMapping OrigEntity = null)
        {
            if (OrigEntity == null)
            {
                OrigEntity = new FeedAccountMapping()
                {
                    Id = AccEntity.Id,
                    //FeedAccId = AccEntity.Id,
                    IdNavigation = AccEntity,
                    AccountId = new PFAID(Entity.NimbleAccountID).UID,
                    CorpId = new PFAID(Entity.CorpID).UID,
                    LastSynchedOn = Entity.LastSyncDate.HasValue ? Entity.LastSyncDate.Value : (Entity.ImportTransactions.Any() ? Entity.ImportTransactions.Max(d => d.Date) : DateTime.Now),//DateTime.Now,
                    LastUpdatedOn = (Entity.EndDate != null) ? Entity.EndDate : DateTime.Now,
                    FormatId = Entity.FormatID
                };
            }
            else
            {
                OrigEntity.LastSynchedOn = (Entity.ImportTransactions.Any() ? Entity.ImportTransactions.Max(d => d.Date) : (Entity.LastSyncDate.HasValue ? Entity.LastSyncDate.Value : DateTime.Now));
                OrigEntity.LastUpdatedOn = (Entity.EndDate != null) ? Entity.EndDate : DateTime.Now;
                OrigEntity.FormatId = Entity.FormatID;
            }
            return OrigEntity;
        }

        #endregion

        #region Format Settings

        /// <summary>
        /// To map Format setting request to ImportFormatSettings entity
        /// </summary>
        /// <param name="Entity">format request</param>
        /// <param name="OrigEntity">for update case - original ImportFormatSettings data</param>
        /// <returns>ImportFormatSettings</returns>
        public static ImportFormatSettings MapImportFormat(ImportFormatRequest Entity, ImportFormatSettings? OrigEntity = null)
        {
            if (OrigEntity != null)
            {
                OrigEntity.FormatName = Entity.FormatName;//.ToLower();
                OrigEntity.ColumnCount = Entity.ColumnCount;
                OrigEntity.TemplatePath = Entity.TemplatePath;
                OrigEntity.Date = Entity.Date;
                OrigEntity.Memo = Entity.Memo;
                OrigEntity.Num = Entity.Number;
                OrigEntity.DataReadfrom = Entity.DataReadFrom;
                OrigEntity.AmountType = Entity.AmountType;
                OrigEntity.AmountMode = Entity.AmountMode;
                OrigEntity.AmountColumn1 = Entity.AmountType == 1 ? Entity.AmountColumn : Entity.DRColumn;
                OrigEntity.AmountColumn2 = Entity.AmountType == 1 ? Entity.DRCRColumn : Entity.CRColumn;
                OrigEntity.IsHeader = Convert.ToInt16(Entity.IsHeader ? 1 : 0);
                OrigEntity.FormatRefId = new PFAID(Entity.FormatRefId).UID;
            }
            else
            {
                OrigEntity = new ImportFormatSettings()
                {
                    FormatName = Entity.FormatName,//.ToLower(),
                    ColumnCount = Entity.ColumnCount,
                    TemplatePath = Entity.TemplatePath,
                    Date = Entity.Date,
                    Memo = Entity.Memo,
                    Num = Entity.Number,
                    DataReadfrom = Entity.DataReadFrom,
                    AmountType = Entity.AmountType,
                    AmountMode = Entity.AmountMode,
                    AmountColumn1 = Entity.AmountType == 1 ? Entity.AmountColumn : Entity.DRColumn,
                    AmountColumn2 = Entity.AmountType == 1 ? Entity.DRCRColumn : Entity.CRColumn,
                    ProviderRegId = Entity.ProvRegID,
                    IsHeader = Convert.ToInt16(Entity.IsHeader ? 1 : 0),
                    FormatRefId = new PFAID().UID,
                    Status = (short)Status.Active
                    //ClientId=Entity.
                };
            }
            return OrigEntity;
        }

        #endregion

        #region Merge Settings

        /// <summary>
        /// To Map MergeSettingsDTO to FeedAccountMergeSettings
        /// </summary>
        /// <param name="Entity">contains MergeSettingsDTO details </param>
        /// <param name="FeedAccID"></param>
        /// <param name="OrigEntity">contains previous data in FeedAccountMergeSettings</param>
        /// <returns></returns>
        public static FeedAccountMergeSettings MapMergeSettings(MergeSettingsDTO Entity, long FeedAccID, FeedAccountMergeSettings? OrigEntity = null)
        {
            //update case
            if (OrigEntity != null)
            {
                OrigEntity.LineId = new PFAID(Entity.LineID).UID;
                OrigEntity.DeptType = Convert.ToInt16(Entity.DepartmentTypeID);
                OrigEntity.MergeType = Convert.ToByte(Entity.MergeType);
                OrigEntity.FeedAccId = FeedAccID;
                return OrigEntity;
            }
            //create case
            else
            {
                return new FeedAccountMergeSettings()
                {
                    LineId = new PFAID(Entity.LineID).UID,
                    DeptType = Convert.ToInt16(Entity.DepartmentTypeID),
                    MergeType = Convert.ToByte(Entity.MergeType),
                    FeedAccId = FeedAccID
                };
            }
        }
        #endregion
        #region Connections

        public static FeedAccount MapPlaidAcountsInfo(BankAccountDetails existingdata, FeedAccount? accInfo, long insID, short accStatus, DateTime synchFrom)
        {
            try
            {
                if (accInfo != null)
                {
                    accInfo.BankAccId = existingdata.account_id;
                    accInfo.AccName = existingdata.name;
                    accInfo.AccOffName = existingdata.official_name;
                    accInfo.AccEditName = (existingdata.official_name ?? existingdata.name) + (!string.IsNullOrEmpty(existingdata.mask) ? (existingdata.mask.Length > 4 ? "  " + existingdata.mask.Substring(existingdata.mask.Length - 4) : existingdata.mask.Length==4? "  "+ existingdata.mask : "") : ""); //+ ((" " + existingdata.mask.Substring(existingdata.mask.Length - 4, 4)) ?? "");
                    accInfo.AccNumber = existingdata.mask;//existingdata.mask.PadLeft(4, '*');
                    accInfo.AccSubType = existingdata.subtype;
                    accInfo.AccountType = existingdata.type;
                    accInfo.AvlBalance = existingdata.balances.available;
                    accInfo.Balance = existingdata.balances.current;
                    //accInfo.AccType = (existingdata.type.ToLower() == BankAccountTypeEnum.Depository.ToString().ToLower() ? (short)BankAccountTypeEnum.Depository : (existingdata.type.ToLower() == BankAccountTypeEnum.Credit.ToString().ToLower() ? (short)BankAccountTypeEnum.Credit : (existingdata.type.ToLower() == BankAccountTypeEnum.Loan.ToString().ToLower() ? (short)BankAccountTypeEnum.Loan : (short)BankAccountTypeEnum.Other)));
                    //  accInfo.Status = (accInfo!=null&& accInfo.Status==(short)Status.Active)? accInfo.Status : accStatus;
                    accInfo.InsId = insID;
                    //accInfo.SynchFrom = synchFrom;
                    return accInfo;
                }
                else
                    return new FeedAccount()
                    {
                        BankAccId = existingdata.account_id,
                        AccName = existingdata.name,
                        AccOffName = existingdata.official_name,
                        InsId = insID,
                        AccEditName = (existingdata.official_name ?? existingdata.name) + ((" " + existingdata.mask.PadLeft(4, '*')) ?? ""),
                        AccNumber = existingdata.mask,//existingdata.mask.PadLeft(4, '*'),
                        AccSubType = existingdata.subtype,
                        AccountType = existingdata.type,
                        AvlBalance = existingdata.balances.available,
                        Balance = existingdata.balances.current,
                        AccType = (existingdata.type.ToLower() == BankAccountTypeEnum.Depository.ToString().ToLower() ? (short)BankAccountTypeEnum.Depository : (existingdata.type.ToLower() == BankAccountTypeEnum.Credit.ToString().ToLower() ? (short)BankAccountTypeEnum.Credit : (existingdata.type.ToLower() == BankAccountTypeEnum.Loan.ToString().ToLower() ? (short)BankAccountTypeEnum.Loan : (short)BankAccountTypeEnum.Other))),
                        Status = accStatus,
                        CreatedOn = DateTime.Now,
                        SynchFrom = synchFrom
                    };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static FeedTransactions MapFeedPlaidTransactions(PlaidTransactions existingdata, FeedTransactions? transInfo, long? accID, string logSeqID)
        {
            if (transInfo != null)
            {
                if (accID != null)
                    transInfo.FeedAccId = accID;
                transInfo.Amount = existingdata.amount < 0 ? -1 * existingdata.amount : existingdata.amount;
                transInfo.TransProviderId = existingdata.transaction_id;
                transInfo.CheckNumber = existingdata.check_number;
                transInfo.Description = existingdata.original_description ?? existingdata.name;
                transInfo.TransDate = Convert.ToDateTime(existingdata.date);
                transInfo.LogSeqId = logSeqID;
                //transInfo.TransType = existingdata.transaction_type;
                transInfo.Status = transInfo.Status == ((short)FeedTransStatusEnum.Pending) ? ((short)FeedTransStatusEnum.NewFeed) : transInfo.Status;
                transInfo.TransPostType = (existingdata.amount > 0 ? (short)0 : (short)1);
                transInfo.TransType = transInfo.TransPostType == 0 ? BankFeedActualtrTypeEnum.Receipts.GetDisplayName() : BankFeedActualtrTypeEnum.Payments.GetDisplayName();
                transInfo.RunningBalance = 0;
                transInfo.TransName = existingdata.name ?? existingdata.merchant_name;
                return transInfo;
            }
            else
                return new FeedTransactions()
                {
                    //AccId = accID,
                    Amount = existingdata.amount < 0 ? -1 * existingdata.amount : existingdata.amount,
                    TransProviderId = existingdata.transaction_id,
                    CheckNumber = existingdata.check_number,
                    Description = existingdata.original_description ?? existingdata.name,
                    TransDate = Convert.ToDateTime(existingdata.date),
                    LogSeqId = logSeqID.ToString(),
                    // TransType = (!string.IsNullOrEmpty(existingdata.transaction_type))? existingdata.transaction_type:,
                    Status = !existingdata.pending ? (short)FeedTransStatusEnum.NewFeed : (short)FeedTransStatusEnum.Pending,
                    TransPostType = (existingdata.amount > 0 ? (short)0 : (short)1),
                    TransType = (existingdata.amount > 0) ? BankFeedActualtrTypeEnum.Receipts.GetDisplayName() : BankFeedActualtrTypeEnum.Payments.GetDisplayName(),
                    RunningBalance = 0,
                    TransName = existingdata.name ?? existingdata.merchant_name

                };
        }

        public static FeedAccount MapYadleeAcountsInfo(YodleeAccountDTO existingdata, FeedAccount? accInfo, long insID, short accStatus, DateTime? synchFrom)
        {
            try
            {
                if (accInfo != null)
                {
                    accInfo.BankAccId = existingdata.id.ToString();
                    accInfo.AccName = existingdata.accountName;
                    accInfo.AccOffName = existingdata.accountName;
                    accInfo.AccEditName = existingdata.accountName + (!string.IsNullOrEmpty(existingdata.accountNumber) ? (existingdata.accountNumber.Length > 4 ? "  " + existingdata.accountNumber.Substring(existingdata.accountNumber.Length - 4) : existingdata.accountNumber.Length == 4 ? "  " + existingdata.accountNumber : "") : ""); //(existingdata.official_name ?? existingdata.name) + (("-" + existingdata.mask.PadLeft(4, '*')) ?? "");
                    accInfo.AccNumber = existingdata.accountNumber;
                    //accInfo.AccSubType = existingdata.;
                    accInfo.AccountType = existingdata.accountType;
                    accInfo.AvlBalance = GetYodleeAccountAvlBalance(existingdata);
                    accInfo.Balance = GetYodleeAccountBalance(existingdata);
                    //accInfo.AvlBalance = existingdata.CONTAINER == "creditCard" ?
                    //                           existingdata.availableCredit == null ? 0 :
                    //                            Convert.ToDecimal(existingdata.availableCredit.amount) : existingdata.CONTAINER == "loan" ?
                    //                            (existingdata.originalLoanAmount != null) ? Convert.ToDecimal(existingdata.originalLoanAmount.amount) : 0 :
                    //                             existingdata.availableBalance == null ? 0 : Convert.ToDecimal(existingdata.availableBalance.amount);
                    //accInfo.Balance = 
                        
                    //    existingdata.CONTAINER != "creditCard" ?
                    //                           existingdata.currentBalance != null ?
                    //                           Convert.ToDecimal(existingdata.currentBalance.amount) :
                    //                           existingdata.balance == null ? 0 :
                    //                           existingdata.balance.amount == 0 ? 0 : Convert.ToDecimal(existingdata.balance.amount) :
                    //                           existingdata.balance == null ? 0 :
                    //                           existingdata.balance.amount == 0 ? 0 : Convert.ToDecimal(existingdata.balance.amount);
                    //accInfo.AccType = (existingdata.accountType.ToLower() == BankAccountTypeEnum.Depository.ToString().ToLower() ? (short)BankAccountTypeEnum.Depository : (existingdata.accountType.ToLower() == BankAccountTypeEnum.Credit.ToString().ToLower() ? (short)BankAccountTypeEnum.Credit : (existingdata.accountType.ToLower() == BankAccountTypeEnum.Loan.ToString().ToLower() ? (short)BankAccountTypeEnum.Loan : (short)BankAccountTypeEnum.Other)));
                    //accInfo.Status = accStatus;
                    accInfo.InsId = insID;
                    //accInfo.SynchFrom = synchFrom;
                    return accInfo;
                }
                else
                {
                    //accInfo = new FeedAccount();
                    //accInfo.BankAccId = Convert.ToString(existingdata.id); //.ToString();
                    //accInfo.AccName = existingdata.accountName;
                    //accInfo.AccOffName = existingdata.accountName;
                    //accInfo.InsId = insID;
                    //accInfo.AccEditName = existingdata.accountName;//(existingdata.official_name ?? existingdata.name) + (("-" + existingdata.mask.PadLeft(4, '*')) ?? ""),
                    //accInfo.AccNumber = existingdata.accountNumber; //existingdata.mask.PadLeft(4, '*'),
                    //                                                // AccSubType = existingdata.subtype,
                    //accInfo.AccountType = existingdata.accountType;
                    //accInfo.AvlBalance = existingdata.CONTAINER == "creditCard" ?
                    //                       existingdata.availableCredit == null ? 0 :
                    //                        Convert.ToDecimal(existingdata.availableCredit.amount) : existingdata.CONTAINER == "loan" ?
                    //                        Convert.ToDecimal(existingdata.originalLoanAmount.amount) == 0 ? 0 :
                    //                        Convert.ToDecimal(existingdata.originalLoanAmount.amount) : existingdata.availableBalance == null ? 0 : Convert.ToDecimal(existingdata.availableBalance.amount);
                    //accInfo.Balance = existingdata.CONTAINER != "creditCard" ?
                    //                   existingdata.currentBalance != null ?
                    //                   Convert.ToDecimal(existingdata.currentBalance.amount) :
                    //                   existingdata.balance == null ? 0 :
                    //                   existingdata.balance.amount == 0 ? 0 : Convert.ToDecimal(existingdata.balance.amount) :
                    //                   existingdata.balance == null ? 0 :
                    //                   existingdata.balance.amount == 0 ? 0 : Convert.ToDecimal(existingdata.balance.amount);
                    //accInfo.AccType = (existingdata.accountType.ToLower() == BankAccountTypeEnum.Depository.ToString().ToLower() ? (short)BankAccountTypeEnum.Depository : (existingdata.accountType.ToLower() == BankAccountTypeEnum.Credit.ToString().ToLower() ? (short)BankAccountTypeEnum.Credit : (existingdata.accountType.ToLower() == BankAccountTypeEnum.Loan.ToString().ToLower() ? (short)BankAccountTypeEnum.Loan : (short)BankAccountTypeEnum.Other)));
                    //accInfo.Status = accStatus;
                    //accInfo.CreatedOn = DateTime.Now;
                    ////SynchFrom = DateTime.Now
                    //return accInfo;
                    return new FeedAccount()
                    {
                        BankAccId = existingdata.id.ToString(),
                        AccName = existingdata.accountName,
                        AccOffName = existingdata.accountName,
                        InsId = insID,
                        AccEditName = existingdata.accountName + ((!string.IsNullOrEmpty(existingdata.accountNumber)) ? ((existingdata.accountNumber.Length > 4 ? "  " + existingdata.accountNumber.Substring(existingdata.accountNumber.Length - 4) : "")) : ""),//(existingdata.official_name ?? existingdata.name) + (("-" + existingdata.mask.PadLeft(4, '*')) ?? ""),
                        AccNumber = existingdata.accountNumber, //existingdata.mask.PadLeft(4, '*'),
                                                                // AccSubType = existingdata.subtype,
                        AccountType = existingdata.accountType,
                        AvlBalance = GetYodleeAccountAvlBalance(existingdata),
                        Balance = GetYodleeAccountBalance(existingdata),

                        //existingdata.CONTAINER == "creditCard" ?
                        //                       existingdata.availableCredit == null ? 0 :
                        //                        Convert.ToDecimal(existingdata.availableCredit.amount) : existingdata.CONTAINER == "loan" ?
                        //                       (existingdata.originalLoanAmount != null) ? Convert.ToDecimal(existingdata.originalLoanAmount.amount) : 0 :
                        //                       existingdata.availableBalance == null ? 0 : Convert.ToDecimal(existingdata.availableBalance.amount),




                        //existingdata.CONTAINER != "creditCard" ?
                        //                   existingdata.currentBalance != null ?
                        //                   Convert.ToDecimal(existingdata.currentBalance.amount) :
                        //                   existingdata.balance == null ? 0 :
                        //                   existingdata.balance.amount == 0 ? 0 : Convert.ToDecimal(existingdata.balance.amount) :
                        //                   existingdata.balance == null ? 0 :
                        //                   existingdata.balance.amount == 0 ? 0 : Convert.ToDecimal(existingdata.balance.amount),
                        AccType = (existingdata.accountType.ToLower() == BankAccountTypeEnum.Depository.ToString().ToLower() ? (short)BankAccountTypeEnum.Depository : (existingdata.accountType.ToLower() == BankAccountTypeEnum.Credit.ToString().ToLower() ? (short)BankAccountTypeEnum.Credit : (existingdata.accountType.ToLower() == BankAccountTypeEnum.Loan.ToString().ToLower() ? (short)BankAccountTypeEnum.Loan : (short)BankAccountTypeEnum.Other))),
                        Status = accStatus,
                        CreatedOn = DateTime.Now,
                        SynchFrom = synchFrom
                    };
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public static decimal GetYodleeAccountAvlBalance(YodleeAccountDTO existingdata)
        {
            decimal balance = existingdata.CONTAINER == "creditCard" ?
                                           existingdata.availableCredit == null ? 0 :
                                            Convert.ToDecimal(existingdata.availableCredit.amount) : existingdata.CONTAINER == "loan" ?
                                           (existingdata.originalLoanAmount != null) ? Convert.ToDecimal(existingdata.originalLoanAmount.amount) : 0 :
                                           existingdata.availableBalance == null ? 0 : Convert.ToDecimal(existingdata.availableBalance.amount);
            return balance;
        }
        public static decimal GetYodleeAccountBalance(YodleeAccountDTO existingdata)
        {
            decimal balance = existingdata.CONTAINER != "creditCard" ?
                                           existingdata.currentBalance != null ?
                                           Convert.ToDecimal(existingdata.currentBalance.amount) :
                                           existingdata.balance == null ? 0 :
                                           existingdata.balance.amount == 0 ? 0 : Convert.ToDecimal(existingdata.balance.amount) :
                                           existingdata.balance == null ? 0 :
                                           existingdata.balance.amount == 0 ? 0 : Convert.ToDecimal(existingdata.balance.amount);
            return balance;
        }
        public static FeedTransactions MapFeedYodleeTransactions(YodleeTransaction existingdata, FeedTransactions? transInfo, long? accID, string logSeqID)
        {
            if (transInfo != null)
            {
                if (accID != null)
                    transInfo.FeedAccId = accID;
                transInfo.Amount = Convert.ToDecimal(existingdata.amount.amount);
                transInfo.TransProviderId = existingdata.id.ToString();
                transInfo.CheckNumber = existingdata.checkNumber;
                transInfo.Description = existingdata.description.original.ToString();//existingdata.description ?? existingdata.name;
                transInfo.TransDate = Convert.ToDateTime(existingdata.date);
                transInfo.LogSeqId = logSeqID;
                //transInfo.TransType = existingdata.type;
                transInfo.Status = transInfo.Status == ((short)FeedTransStatusEnum.Pending) ? (short)FeedTransStatusEnum.NewFeed : transInfo.Status;
                transInfo.TransPostType = existingdata.baseType == TransactionPostType.CREDIT.GetDisplayName() ? (short)TransactionPostType.DEBIT : (short)TransactionPostType.CREDIT;
                transInfo.TransType = transInfo.TransPostType == 0 ? BankFeedActualtrTypeEnum.Receipts.GetDisplayName() : BankFeedActualtrTypeEnum.Payments.GetDisplayName();
                transInfo.RunningBalance = (existingdata.runningBalance.amount != null) ? Convert.ToDecimal(existingdata.runningBalance.amount) : 0;
                transInfo.TransName = existingdata.merchant == null ? "" : existingdata.merchant.name;//existingdata.name ?? existingdata.merchant_name;

                return transInfo;
            }
            else
            {
                //AccId = accID,
                FeedTransactions feedtra = new FeedTransactions();
                feedtra.Amount = Convert.ToDecimal(existingdata.amount.amount);
                feedtra.TransProviderId = existingdata.id.ToString();
                feedtra.CheckNumber = existingdata.checkNumber;
                feedtra.Description = existingdata.description.original.ToString();// ?? existingdata.merchant.name,
                feedtra.TransDate = Convert.ToDateTime(existingdata.date);
                feedtra.LogSeqId = logSeqID.ToString();
                //feedtra.TransType = existingdata.type;
                feedtra.Status = (existingdata.status.ToLower() != FeedTransStatusEnum.Pending.GetDisplayName().ToLower()) ? (short)FeedTransStatusEnum.NewFeed : (short)FeedTransStatusEnum.Pending;
                feedtra.TransPostType = existingdata.baseType == TransactionPostType.CREDIT.GetDisplayName() ? (short)TransactionPostType.DEBIT : (short)TransactionPostType.CREDIT;
                feedtra.TransType = (feedtra.TransPostType == 0) ? BankFeedActualtrTypeEnum.Receipts.GetDisplayName() : BankFeedActualtrTypeEnum.Payments.GetDisplayName();
                feedtra.RunningBalance = (existingdata.runningBalance.amount != null) ? Convert.ToDecimal(existingdata.runningBalance.amount) : 0;
                feedtra.TransName = existingdata.merchant == null ? "" : existingdata.merchant.name;//existingdata.name ?? existingdata.merchant.name
                return feedtra;
            }
            //return new FeedTransactions()
            //{
            //    //AccId = accID,
            //    Amount = Convert.ToDecimal(existingdata.amount.amount),
            //    TransProviderId = existingdata.id.ToString(),
            //    CheckNumber = existingdata.checkNumber,
            //    Description = existingdata.description.original.ToString(),// ?? existingdata.merchant.name,
            //    TransDate = Convert.ToDateTime(existingdata.date),
            //    LogSeqId = logSeqID.ToString(),
            //    TransType = existingdata.type,
            //    Status = (existingdata.status.ToLower() != FeedTransStatusEnum.Pending.GetDisplayName().ToLower()) ? (short)FeedTransStatusEnum.NewFeed : (short)FeedTransStatusEnum.Pending,
            //    TransPostType = existingdata.baseType == TransactionPostType.CREDIT.GetDisplayName().ToLower() ? (short)TransactionPostType.CREDIT : (short)TransactionPostType.DEBIT,
            //    RunningBalance = 0,
            //    TransName = existingdata.merchant == null ? "" : existingdata.merchant.name//existingdata.name ?? existingdata.merchant.name

            //};
        }

        public static FeedTransactions MapFeedTransactions(SynchFeedTransactionDTO existingdata, FeedTransactions? transInfo, long? accID, string logSeqID)
        {
            if (transInfo != null)
            {
                if (accID != null)
                    transInfo.FeedAccId = accID;
                transInfo.Amount = existingdata.Amount < 0 ? -1 * existingdata.Amount : existingdata.Amount; ;
                transInfo.TransProviderId = existingdata.transaction_id;
                transInfo.CheckNumber = existingdata.CheckNo;
                transInfo.Description = existingdata.Description ?? existingdata.name;
                transInfo.TransDate = Convert.ToDateTime(existingdata.Date);
                transInfo.LogSeqId = logSeqID;
                transInfo.TransPostType = existingdata.TransPostTypeID;
                transInfo.TransType = transInfo.TransPostType == 0 ? BankFeedActualtrTypeEnum.Receipts.GetDisplayName() : BankFeedActualtrTypeEnum.Payments.GetDisplayName();
                transInfo.RunningBalance = (existingdata.RunningBalance != null) ? existingdata.RunningBalance : 0;//0;
                transInfo.TransName = existingdata.name ?? existingdata.name;
                if (existingdata.Status == (short)FeedTransStatusEnum.Expired)
                {
                    transInfo.Status = (short)FeedTransStatusEnum.Expired;
                }
                return transInfo;
            }
            else
            {

                FeedTransactions feedtra = new FeedTransactions();
                feedtra.Amount = existingdata.Amount < 0 ? -1 * existingdata.Amount : existingdata.Amount;
                feedtra.TransProviderId = existingdata.transaction_id;
                feedtra.CheckNumber = existingdata.CheckNo;
                feedtra.Description = existingdata.Description ?? existingdata.name;
                feedtra.TransDate = Convert.ToDateTime(existingdata.Date);
                feedtra.LogSeqId = logSeqID;
                feedtra.Status = existingdata.Status == (short)FeedTransStatusEnum.Expired ? (short)FeedTransStatusEnum.Expired : (existingdata.Status != (short)FeedTransStatusEnum.Pending && existingdata.Status != (short)Status.Delete1) ? (short)FeedTransStatusEnum.NewFeed : (existingdata.Status == (short)FeedTransStatusEnum.Pending) ? (short)FeedTransStatusEnum.Pending : (short)Status.Delete1;
                feedtra.TransPostType = existingdata.TransPostTypeID;
                feedtra.TransType = existingdata.TransPostTypeID == 0 ? BankFeedActualtrTypeEnum.Receipts.GetDisplayName() : BankFeedActualtrTypeEnum.Payments.GetDisplayName();
                feedtra.RunningBalance = (existingdata.RunningBalance != null) ? existingdata.RunningBalance : 0;
                feedtra.TransName = existingdata.name ?? existingdata.name;
                return feedtra;
            }
        }
        public static FeedInstitute MapInstituteImport(FeedInstitute ins)
        {
            return new FeedInstitute()
            {
                InsProviderId = ins.InsProviderId,
                InsName = ins.InsName,
                ProviderRegId = ins.ProviderRegId,
                InsOffName = ins.InsOffName,
                ProviderAccessId = "Bank-Feeds-Import",
                CreatedOn = DateTime.Now,
                Status = (short)Status.Active
            };
        }

        public static FeedAccount MapFeedAccountImport(FeedAccount accInfo, long insID, short accStatus)
        {
            return new FeedAccount()
            {
                BankAccId = accInfo.BankAccId,
                AccName = accInfo.AccName,
                AccOffName = accInfo.AccOffName,
                InsId = insID,
                AccEditName = accInfo.AccEditName,
                AccNumber = accInfo.AccNumber,
                AccSubType = accInfo.AccSubType,
                AccountType = accInfo.AccountType,
                AvlBalance = accInfo.AvlBalance,
                Balance = accInfo.Balance,
                AccType = accInfo.AccType,
                Status = accStatus,
                CreatedOn = DateTime.Now
                //SynchFrom=DateTime.Now
            };
        }

        public static FeedConnectionLog MapConnectionLog(long count, string logDescription, string logSeqID, long? accID, long? formatID, DateTime? endDate)
        {
            try
            {
                FeedConnectionLog connectionLog = new FeedConnectionLog();
                if (accID.HasValue)
                    connectionLog.FeedAccId = accID;
                connectionLog.LoggedOn = DateTime.Now;
                connectionLog.TransCount = count;
                connectionLog.Status = (short)ConnectionLogStatusEnum.Active;
                connectionLog.LogDescription = logDescription;
                connectionLog.LogSeqId = logSeqID;
                connectionLog.FormatId = formatID;
                connectionLog.EndDate = endDate;
                return connectionLog;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public static ProviderInsChangeLog MapProviderInsChangeLog(long insId, long? providerRegId)
        {
            ProviderInsChangeLog ChangeLog = new ProviderInsChangeLog();

            if (insId != null)
                ChangeLog.InsId = insId;
            ChangeLog.ProviderRegId = providerRegId;
            ChangeLog.ChangedOn = DateTime.Now;
            ChangeLog.Status = ConnectionLogStatusEnum.Active.ToString();
            return ChangeLog;

        }

        public static FeedAccount MapMeldAcountsInfo(FinancialAccountDTO meldAcc, FeedAccount? feedAccount, long insID, short defaultAccStatus)
        {
            try
            {
                int initialTransDays = 30;
                if (meldAcc.serviceProviderDetails != null && meldAcc.serviceProviderDetails.Any())
                {
                    initialTransDays = !string.IsNullOrEmpty(meldAcc.serviceProviderDetails.FirstOrDefault().serviceProvider) ? (meldAcc.serviceProviderDetails.FirstOrDefault().serviceProvider.ToLower() == "finicity" ? 180 : (meldAcc.serviceProviderDetails.FirstOrDefault().serviceProvider.ToLower() == "mx" ? 90 : 30)) : 30;
                }
                var offName = (meldAcc.serviceProviderDetails != null && meldAcc.serviceProviderDetails[0].account != null && !string.IsNullOrEmpty(meldAcc.serviceProviderDetails[0].account.official_name)) ? meldAcc.serviceProviderDetails[0].account.official_name : meldAcc.name;
                var bankAccID = (meldAcc.serviceProviderDetails != null && meldAcc.serviceProviderDetails[0].account != null && !string.IsNullOrEmpty(meldAcc.serviceProviderDetails[0].account.id)) ? meldAcc.serviceProviderDetails[0].account.id : meldAcc.serviceProviderDetails[0].account.account_id;
                var truncAccNum = (!string.IsNullOrEmpty(meldAcc.truncatedAccountNumber) ? (meldAcc.truncatedAccountNumber.Length > 4 ? " " + meldAcc.truncatedAccountNumber.Substring(meldAcc.truncatedAccountNumber.Length - 4) : meldAcc.truncatedAccountNumber.Length == 4 ? " " + meldAcc.truncatedAccountNumber : "") : "");
                var avalBal = (meldAcc.balances != null && meldAcc.balances.availableAmount != null) ? Convert.ToDecimal(meldAcc.balances.availableAmount) : 0.0M;
                var balance = (meldAcc.balances != null && meldAcc.balances.currentAmount != null) ? Convert.ToDecimal(meldAcc.balances.currentAmount) : 0.0M;
                DateTime? updatedAt = (meldAcc.balances != null && meldAcc.balances.updatedAt.HasValue) ? meldAcc.balances.updatedAt.Value : null;
                var accType = (meldAcc.type.ToLower() == BankAccountTypeEnum.Depository.ToString().ToLower() ? (short)BankAccountTypeEnum.Depository : (meldAcc.type.ToLower() == BankAccountTypeEnum.Credit.ToString().ToLower() ? (short)BankAccountTypeEnum.Credit : (meldAcc.type.ToLower() == BankAccountTypeEnum.Loan.ToString().ToLower() ? (short)BankAccountTypeEnum.Loan : (short)BankAccountTypeEnum.Other)));
                var endDate = (meldAcc.balances != null && meldAcc.balances.updatedAt.HasValue) ? meldAcc.balances.updatedAt.Value : DateTime.Now;

                if (feedAccount != null)
                {
                    feedAccount.BankAccId = bankAccID;
                    feedAccount.FinancialAccId = meldAcc.id;
                    feedAccount.AccName = meldAcc.name;
                    feedAccount.AccOffName = offName;
                    feedAccount.AccEditName = offName + ((" " + meldAcc.truncatedAccountNumber.PadLeft(4, '*')) ?? "");
                    feedAccount.AccNumber = meldAcc.truncatedAccountNumber;//existingdata.mask.PadLeft(4, '*');
                    feedAccount.AccSubType = meldAcc.subtype;
                    feedAccount.AccountType = meldAcc.type;
                    feedAccount.AvlBalance = avalBal;
                    feedAccount.Balance = balance;
                    //feedAccount.AccType = accType; //it should be mapped COA type, since it is updating back to Meld account type, no need to update this while sync process, update this only while mapping COA
                    feedAccount.InsId = insID;
                    //CreatedOn
                    //feedAccount.SynchFrom = synchFrom;
                    //feedAccount.EndDate = endDate;

                    //  feedAccount.Status = (accInfo!=null&& accInfo.Status==(short)Status.Active)? accInfo.Status : defaultAccStatus;
                    feedAccount.ProviderStatus = meldAcc.status;
                    if (updatedAt.HasValue && updatedAt.Value != default(DateTime) && feedAccount.ProviderUpdatedOn != updatedAt.Value)
                        feedAccount.ProviderUpdatedOn = updatedAt.Value.ToLocalTime();
                    //HasHistoricalData
                    return feedAccount;
                }
                else
                    return new FeedAccount()
                    {
                        BankAccId = bankAccID,
                        FinancialAccId = meldAcc.id,
                        AccName = meldAcc.name,
                        AccOffName = offName,
                        AccEditName = offName + ((" " + meldAcc.truncatedAccountNumber.PadLeft(4, '*')) ?? ""),
                        AccNumber = meldAcc.truncatedAccountNumber,//existingdata.mask.PadLeft(4, '*'),
                        AccSubType = meldAcc.subtype,
                        AccountType = meldAcc.type,
                        AvlBalance = avalBal,
                        Balance = balance,
                        AccType = accType,
                        InsId = insID,
                        CreatedOn = DateTime.Now,
                        SynchFrom = endDate.AddDays(-initialTransDays),
                        EndDate = endDate,
                        
                        Status = defaultAccStatus,
                        ProviderStatus = meldAcc.status,
                        HasHistoricalData = false,
                    };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //public static BankAccountStatusEnum MapMeldStatusToFeedAcountStats(string ProviderStatus)
        //{
        //    BankAccountStatusEnum res = BankAccountStatusEnum.Pending;
        //    if (!string.IsNullOrEmpty(ProviderStatus))
        //    {
        //        switch (ProviderStatus)
        //        {
        //            case "IN_PROGRESS": res = BankAccountStatusEnum.Pending; break;
        //            case "EXPIRED": res = BankAccountStatusEnum.AccountArchived; break;
        //            case "ACTIVE": res = BankAccountStatusEnum.Active; break;
        //            case "PARTIALLY_ACTIVE": res = BankAccountStatusEnum.Active; break;
        //            case "RECONNECT_AVAILABLE": res = BankAccountStatusEnum.ReconnectAvailable; break;
        //            case "RECONNECT_REQUIRED": res = BankAccountStatusEnum.OptForRelogin; break;
        //            case "UNRECOVERABLE": res = BankAccountStatusEnum.AccountArchived; break;
        //            case "DELETED": res = BankAccountStatusEnum.Deleted; break;
        //            case "UNDETERMINED": res = BankAccountStatusEnum.OptForRelogin; break;
        //            //case "TRANSACTIONS_AGGREGATED": res = BankAccountStatusEnum.TransAggrigated; break;
        //            //case "HISTORICAL_TRANSACTIONS_AGGREGATED": res = BankAccountStatusEnum.HistoricalTransAggrigated; break;
        //            default: goto case "IN_PROGRESS";
        //        }
        //    }

        //    return res;
        //}
        #endregion

        #region NewFeedRule

        public static FeedRule MapNewFeedrule(SaveOrUpdateFeedRuleReq entity, FeedRule? origEntity = null)
        {
            if (origEntity != null)
            {
                origEntity.SetUpFor = entity.RuleName;
                origEntity.FeedTransType = (short)(entity.TransactionType ? BankFeedActualtrTypeEnum.Payments : BankFeedActualtrTypeEnum.Receipts);
                origEntity.QueryMatchType = (short)(entity.ApplyLines == 1 ? FeedruleQueryMatchTypeEnum.AllLines : FeedruleQueryMatchTypeEnum.SingleLine);
                origEntity.RuleCategoryType = (short)entity.RuleCategoryId;
                origEntity.RuleId = entity.RuleId;
                origEntity.IsBankOrCreditCard = entity.IsbankOrCreditAccount;

                if (entity.AssignRule != null)
                {
                    origEntity.ActTransType = Convert.ToInt16(entity.AssignRule.TransactionType);
                    origEntity.IsPostBill = entity.AssignRule.IsPostBill;
                }
                else
                {
                    origEntity.ActTransType = null;
                    origEntity.IsPostBill = null;
                }

                return origEntity;
            }

            FeedRule feedRule = new FeedRule
            {
                SetUpFor = entity.RuleName,
                FeedTransType = (short)(entity.TransactionType ? BankFeedActualtrTypeEnum.Payments : BankFeedActualtrTypeEnum.Receipts),
                QueryMatchType = (short)(entity.ApplyLines == 1 ? FeedruleQueryMatchTypeEnum.AllLines : FeedruleQueryMatchTypeEnum.SingleLine),
                RuleCategoryType = (short)entity.RuleCategoryId,
                RuleId = entity.RuleId,
                IsBankOrCreditCard = entity.IsbankOrCreditAccount,
                AutoApplyEnable = true,
                Status = (short)Status.Active
            };

            if (entity.AssignRule != null)
            {
                feedRule.ActTransType = Convert.ToInt16(entity.AssignRule.TransactionType);
                feedRule.IsPostBill = entity.AssignRule.IsPostBill;
            }

            return feedRule;
        }

        public static FeedRuleDetails MapFeedRuleDetails( FeedRuleConditionsRequest entity, long feedRuleId, FeedRuleDetails? origEntity = null)
        {
            if (origEntity != null)
            {
                origEntity.RuleType = (short)entity.FieldName;
                origEntity.FilterType = (short)entity.Condition;
                origEntity.Val = entity.Value;
                return origEntity;
            }

            return new FeedRuleDetails
            {
                FeedRuleId = feedRuleId,
                RuleType = (short)entity.FieldName,
                FilterType = (short)entity.Condition,
                Val = entity.Value,
                Status = (short)Status.Active
            };
        }

        public static FeedBankOrCreditAccounts MapFeedBankOrCreditAccount( long feedRuleId, byte[] corporationId,  byte[] accountId, bool isOppositeTransfer = false, FeedBankOrCreditAccounts? origEntity = null)
        {
            if (origEntity != null)
            {
                origEntity.CorporationId = corporationId;
                origEntity.AccountId = accountId;
                origEntity.IsOppositeTransfer = isOppositeTransfer;
                return origEntity;
            }
            return new FeedBankOrCreditAccounts
            {
                FeedRuleId = feedRuleId,
                CorporationId = corporationId,
                AccountId = accountId,
                IsOppositeTransfer = isOppositeTransfer
            };
        }

        public static FeedRuleMapping MapFeedRuleMapping(RuleSplitRequest entity, long feedRuleId, string corporationId, byte[] splitAccountId, byte[] purposeId, string payeeId, short payeeType, short transType, short splitLineNum, FeedRuleMapping? origEntity = null)
        {
            if (origEntity != null)
            {
                origEntity.CorpId = string.IsNullOrEmpty(corporationId) ? null : new PFAID(corporationId).UID;
                origEntity.NameId = string.IsNullOrEmpty(payeeId) ? null : new PFAID(payeeId).UID;
                origEntity.NameType = payeeType;
                origEntity.ActTransType = transType;
                origEntity.AccountId = splitAccountId;
                origEntity.DepartmentId = string.IsNullOrEmpty(entity.DepartmentId) ? null : new PFAID(entity.DepartmentId).UID;
                origEntity.PurposeId = purposeId;
                origEntity.IsAmountOrPercentage = entity.SplitType;
                origEntity.AmountOrPercentageVal = entity.SplitValue;
                origEntity.SplitLineNum = splitLineNum;
                return origEntity;
            }

            return new FeedRuleMapping
            {
                FeedRuleId = feedRuleId,
                CorpId = string.IsNullOrEmpty(corporationId) ? null : new PFAID(corporationId).UID,
                NameId = string.IsNullOrEmpty(payeeId) ? null : new PFAID(payeeId).UID,
                NameType = payeeType,
                ActTransType = transType,
                AccountId = splitAccountId,
                DepartmentId = string.IsNullOrEmpty(entity.DepartmentId) ? null : new PFAID(entity.DepartmentId).UID,
                PurposeId = purposeId,
                IsAmountOrPercentage = entity.SplitType,
                AmountOrPercentageVal = entity.SplitValue,
                SplitLineNum = splitLineNum,
                Status =(byte)Status.Active
            };
        }
        #endregion
    }


}

