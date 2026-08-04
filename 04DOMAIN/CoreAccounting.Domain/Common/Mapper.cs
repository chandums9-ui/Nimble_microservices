using Common.Domain.DTO.App;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Req;
using Common.Domain.DTO.Enums;
using CoreAccounting.Domain.DTO.Req;
using DataModel.Domain.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Domain.DTO.Resp;
using CommonMapper = Common.Domain.Common.Mapper;
using CoreAccounting.Domain.DTO.Resp;
using Common.Domain.DTO.Model;
using Common.Domain.DTO.Req;
using Common.Domain.Mapper;

namespace CoreAccounting.Domain.Common
{
    /// <summary>
    /// Mapper class for core module
    /// </summary>
    public static class Mapper
    {
        #region CheckMappers

        /// <summary>
        /// Mapping CheckEntryRequest to Repetitive
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="origEntity"></param>
        /// <returns>It returns Mapped Repetative</returns>
        public static Repetitive MapRepetitive(CheckEntryRequest entity, Repetitive? origEntity = null)
        {
            return CommonMapper.MapRepetitive((JournalEntryDTO)entity, origEntity);
        }

        /// <summary>
        /// mappng CheckEntryRequest to RepetitiveTransaction
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="RepetativeId"></param>
        /// <param name="ReferenceNumber"></param>
        /// <param name="origEntity"></param>
        /// <returns>It will return Mapped VendorRepetativeTransaction</returns>
        public static RepetitiveTransaction MapRepetativeVendorTransaction(CheckEntryRequest entity, byte[] RepetativeId, string? ReferenceNumber = null, RepetitiveTransaction? origEntity = null)
        {
            if (origEntity != null)
            {
                return origEntity;
            }
            else
            {
                origEntity = new RepetitiveTransaction();
                origEntity.Id = (!string.IsNullOrEmpty(entity.ID)) ? new PFAID(entity.ID).UID : new PFAID().UID;
                origEntity.AccountId = new PFAID(entity.AccountID).UID;
                origEntity.TargetId = RepetativeId;
                origEntity.Amount = entity.Amount;
                origEntity.DebitCredit = Convert.ToBoolean(TransactionDebitCredit.Credit);
                origEntity.TransactionDate = DateTime.Now;
                origEntity.ReferenceNumber = entity.ReferenceNumber;
                origEntity.Memo = entity.Memo != null ? entity.Memo : null;
                origEntity.Status = (short)Status.Active;
                origEntity.SourceId = new PFAID(entity.PayeeID).UID;
                origEntity.SourceType = (short)TransactionSourceType.Vendor;
                //origEntity.RepetitiveTransactionInvoice = new RepetitiveTransactionInvoice();
                //origEntity.RepetitiveTransactionInvoice.Id = origEntity.Id;

                return origEntity;
            }
        }
        public static JournalEntry MapCheckEntry(CheckEntryRequest entity, JournalEntry? origEntity = null)
        {
            if (origEntity != null)
            {
                //origEntity.Id;

                origEntity.CorporationId = new PFAID(entity.CorpID).UID;
                origEntity.EntryDate = Convert.ToDateTime(entity.EntryDate);
                origEntity.EntryNumber = entity.EntryNumber;
                //origEntity.IsAccrual = Convert.ToInt16(entity.IsAccrual);
                origEntity.IsPrintRequired = entity.IsPrintEntry;
                origEntity.ModifiedDate = DateTime.Now;
                //origEntity.SourceType = (short)JournalSourceTypes.GenPayment;
                //origEntity.ParentSourceType = (short)JournalSourceTypes.GenPayment;
                origEntity.IsAttachment = Convert.ToInt16(entity.HasAttachments);
                origEntity.Status = (short)Status.Active;
                origEntity.ClearedDate = !string.IsNullOrEmpty(entity.ClearedDate) ? Convert.ToDateTime(entity.ClearedDate) : null;
                return origEntity;
            }
            else
            {
                return new JournalEntry()
                {
                    Id = (!string.IsNullOrEmpty(entity.ID)) ? new PFAID(entity.ID).UID : new PFAID().UID,
                    CorporationId = new PFAID(entity.CorpID).UID,
                    EntryDate = Convert.ToDateTime(entity.EntryDate),
                    EntryNumber = entity.EntryNumber,
                    PaymentMethodId = string.IsNullOrEmpty(entity.PaymentMethodID) ? null : new PFAID(entity.PaymentMethodID).UID, //new PFAID(entity.PaymentMethodID).UID ?? null,
                    // IsAccrual = Convert.ToInt16(entity.IsAccrual),
                    IsPrintRequired = entity.IsPrintEntry,
                    //ModifiedDate = DateTime.Now,
                    SourceType = (short)JournalSourceTypes.GenPayment,
                    ParentSourceType = (short)JournalSourceTypes.GenPayment,
                    Status = (short)Status.Active,
                    IsAttachment = Convert.ToInt16(entity.HasAttachments),
                    ReferenceMode = 2,
                    IsMailSent = true,
                    ClearedDate = !string.IsNullOrEmpty(entity.ClearedDate) ? Convert.ToDateTime(entity.ClearedDate) : null,
                    CreatedDate = DateTime.Now
                };
            }
        }
        public static Transaction MapCheckTransaction(CheckEntryRequest entity, byte[] JournalEntryId, Transaction? origEntity = null)
        {
            if (origEntity != null)
            {
                origEntity.AccountId = new PFAID(entity.AccountID).UID;
                origEntity.Memo = entity.Memo;
                origEntity.SourceId = string.IsNullOrEmpty(entity.PayeeID) ? null : new PFAID(entity.PayeeID).UID;
                origEntity.SourceType = entity.PayeeType;
                origEntity.TransactionDate = DateTime.Now;
                origEntity.ReferenceNumber = (!string.IsNullOrEmpty(entity.ReferenceNumber)) ? (entity.ReferenceNumber.Length > 50 ? entity.ReferenceNumber.Substring(0, 50) : entity.ReferenceNumber) : entity.ReferenceNumber; //Max length is 50
                origEntity.Amount = entity.Amount;
                origEntity.IsBankTransactionRefId = Convert.ToByte(entity.IsBankTransactionRefID);
                origEntity.BankTransactionRefId = !string.IsNullOrEmpty(entity.BankTransactionRefID) ? entity.BankTransactionRefID : null;
                //if (origEntity.TransactionInvoice == null)
                //{
                //    origEntity.TransactionInvoice = new TransactionInvoice();
                //    origEntity.TransactionInvoice.Id = origEntity.Id;
                //}
                //origEntity.TransactionInvoice.Rate = origEntity.Amount;
                ////origEntity.TransactionInvoice.Hrs = entity.;
                //origEntity.TransactionInvoice.Status = Convert.ToByte(Status.Active);
                return origEntity;
            }
            else
            {
                origEntity = new Transaction();
                origEntity.Id = (!string.IsNullOrEmpty(entity.ID)) ? new PFAID(entity.ID).UID : new PFAID().UID;
                origEntity.AccountId = new PFAID(entity.AccountID).UID;
                origEntity.JournalEntryId = JournalEntryId;
                origEntity.Amount = entity.Amount;
                origEntity.DebitCredit = Convert.ToBoolean(TransactionDebitCredit.Credit);
                origEntity.TransactionDate = DateTime.Now;
                origEntity.ReferenceNumber = (!string.IsNullOrEmpty(entity.ReferenceNumber)) ? (entity.ReferenceNumber.Length > 50 ? entity.ReferenceNumber.Substring(0, 50) : entity.ReferenceNumber) : entity.ReferenceNumber; //Max length is 50;
                origEntity.Memo = entity.Memo != null ? entity.Memo : null;
                origEntity.Status = (short)Status.Active;
                origEntity.SourceId = string.IsNullOrEmpty(entity.PayeeID) ? null : new PFAID(entity.PayeeID).UID;
                origEntity.SourceType = entity.PayeeType;
                origEntity.IsBankTransactionRefId = Convert.ToByte(entity.IsBankTransactionRefID);
                origEntity.BankTransactionRefId = !string.IsNullOrEmpty(entity.BankTransactionRefID) ? entity.BankTransactionRefID : null;
                //origEntity.TransactionInvoice = new TransactionInvoice();
                //origEntity.TransactionInvoice.Id = origEntity.Id;
                //origEntity.TransactionInvoice.Rate = entity.Amount;
                //origEntity.TransactionInvoice.AccContractId = creditid;

                return origEntity;
            }
        }

        #endregion CheckMappers

        #region FundtransferMappers

        // for other mapper for fundtransfer
        /// <summary>
        /// maping FundTransferReq feilds to JournalEntry
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="origEntity"></param>
        /// <returns>It returns Mapped JournalEntry</returns>
        public static JournalEntry MapFundTransfer(FundTransferRequest entity, JournalEntry? origEntity = null)
        {
            if (origEntity != null)
            {

                return origEntity;
            }
            else
            {
                return new JournalEntry()
                {
                    Id = new PFAID().UID,
                    CorporationId = !string.IsNullOrEmpty(entity.CorpID) ? new PFAID(entity.CorpID).UID : null,
                    EntryDate = entity.TransactionDate,
                    EntryNumber = entity.EntryNum,
                    CreatedDate = DateTime.Now,
                    SourceType = (short)JournalSourceTypes.FundTransfer,
                    ParentSourceType = (short)JournalSourceTypes.InterCompany,
                    TransferMode = (short)IntercompanyTransferMode.InterCompanies,
                    Status = (short)Status.Active,
                    IsAttachment = Convert.ToInt16(entity.HasAttachments),
                    ClearedDate = Convert.ToDateTime(entity.ClearedDate)

                };
            }
        }

        public static Transaction MapFundTransferTransaction(FundTransferRequest entity, byte[] JournalEntryId, Transaction? origEntity = null)
        {
            if (origEntity != null)
            {
                return origEntity;
            }
            else
            {
                return new Transaction()
                {
                    Id = new PFAID().UID,
                    AccountId = new PFAID(entity.CreditAccount).UID,
                    JournalEntryId = JournalEntryId,
                    Amount = entity.Amount,
                    DebitCredit = Convert.ToBoolean(TransactionDebitCredit.Credit),
                    TransactionDate = DateTime.Now,
                    ReferenceNumber = (!string.IsNullOrEmpty(entity.RefNumber)) ? (entity.RefNumber.Length > 50 ? entity.RefNumber.Substring(0, 50) : entity.RefNumber) : entity.RefNumber, //Max length is 50,
                    Memo = entity.Memo != null ? entity.Memo : null,
                    Status = (short)Status.Active,
                    SourceType = (short)JournalSourceTypes.FundTransfer,
                    IsBankTransactionRefId = Convert.ToBoolean(entity.IsBankTransactionRefID) ? Convert.ToByte(Status.Active) : Convert.ToByte(Status.Pending),
                    BankTransactionRefId = Convert.ToString(entity.BankTransactionRefID)
                };
            }
        }
        public static Transaction MapFundTransferSplit(FundTransferDivisionDTO entity, byte[] JournalEntryId, Transaction? origEntity = null)
        {
            if (origEntity != null)
            {

                return origEntity;
            }
            else
            {
                return new Transaction()
                {
                    Id = new PFAID().UID,
                    AccountId = new PFAID(entity.DebitFromAccount).UID,
                    JournalEntryId = JournalEntryId,
                    //origEntity.ParentId = tran.Id ?? null;
                    Amount = entity.SplitAmount,
                    DebitCredit = Convert.ToBoolean(TransactionDebitCredit.Credit),
                    TransactionDate = DateTime.Now,
                    ReferenceNumber = !string.IsNullOrEmpty(entity.Memo) ? (entity.Memo.Length > 50 ? entity.Memo.Substring(0, 50) : entity.Memo) : entity.Memo,//entity.Memo.Length < 25 ? entity.Memo : entity.Memo.Substring(0, 24),
                    Memo = entity.Memo,
                    Status = (short)Status.Active,
                    StoreId = string.IsNullOrEmpty(entity.ProfitcenterID) ? null : new PFAID(entity.ProfitcenterID).UID,
                    IsBankTransactionRefId = Convert.ToBoolean(entity.IsBankTransactionRefID) ? Convert.ToByte(Status.Active) : Convert.ToByte(Status.Pending),
                    BankTransactionRefId = Convert.ToString(entity.BankTransactionRefID)
                };
            }
        }

        //Fund Transfer Mappers
        public static JournalEntry MapFundTransfer(SaveOrEditFundTransferReq entity, JournalEntry? origEntity = null)
        {
            if (origEntity != null)
            {
                return origEntity;
            }
            else
            {
                return new JournalEntry()
                {
                    Id = new PFAID().UID,
                    //CorporationId = !string.IsNullOrEmpty(entity.CorpID) ? new PFAID(entity.CorpID).UID : null,
                    EntryDate = entity.TransactionDate ?? DateTime.Now,
                    EntryNumber = entity.EntryNum,
                    CreatedDate = DateTime.Now,
                    SourceType = (short)JournalSourceTypes.FundTransfer,
                    ParentSourceType = (short)JournalSourceTypes.InterCompany,
                    TransferMode = (short)IntercompanyTransferMode.InterCompanies,
                    Status = (short)Status.Active,
                    IsAttachment = Convert.ToInt16(entity.HasAttachments),
                    ClearedDate = entity.ClearedDate

                };
            }
        }
        public static Transaction MapFundTransferTransaction(SaveOrEditFundTransferReq entity, byte[] JournalEntryId, Transaction? origEntity = null)
        {
            if (origEntity != null)
            {
                return origEntity;
            }
            else
            {
                return new Transaction()
                {
                    Id = new PFAID().UID,
                   // AccountId = new PFAID(entity.CreditAccount).UID,
                    JournalEntryId = JournalEntryId,
                    Amount = 0,
                    DebitCredit = Convert.ToBoolean(TransactionDebitCredit.Credit),
                    TransactionDate = DateTime.Now,
                    ReferenceNumber = (!string.IsNullOrEmpty(entity.RefNumber)) ? (entity.RefNumber.Length > 50 ? entity.RefNumber.Substring(0, 50) : entity.RefNumber) : entity.RefNumber, //Max length is 50,
                    Memo = entity.Memo != null ? entity.Memo : null,
                    Status = (short)Status.Active,
                    SourceType = (short)JournalSourceTypes.FundTransfer,
                   // IsBankTransactionRefId = Convert.ToBoolean(entity.IsBankTransactionRefID) ? Convert.ToByte(Status.Active) : Convert.ToByte(Status.Pending),
                   // BankTransactionRefId = Convert.ToString(entity.BankTransactionRefID)
                };
            }
        }

        public static Transaction MapFundTransferSplit(MultipleSplitRows entity, byte[] JournalEntryId, Transaction? origEntity = null)
        {
            if (origEntity != null)
            {

                return origEntity;
            }
            else
            {
                return new Transaction()
                {
                    Id = new PFAID().UID,
                   // AccountId = new PFAID(entity.DebitFromAccount).UID,
                    JournalEntryId = JournalEntryId,
                    //origEntity.ParentId = tran.Id ?? null;
                    Amount = entity.SplitAmount,
                    DebitCredit = Convert.ToBoolean(TransactionDebitCredit.Credit),
                    TransactionDate = DateTime.Now,
                    ReferenceNumber = !string.IsNullOrEmpty(entity.Memo) ? (entity.Memo.Length > 50 ? entity.Memo.Substring(0, 50) : entity.Memo) : entity.Memo,//entity.Memo.Length < 25 ? entity.Memo : entity.Memo.Substring(0, 24),
                    Memo = entity.Memo,
                    Status = (short)Status.Active,
                    StoreId = string.IsNullOrEmpty(entity.ProfitcenterID) ? null : new PFAID(entity.ProfitcenterID).UID,
                    //IsBankTransactionRefId = Convert.ToBoolean(entity.IsBankTransactionRefID) ? Convert.ToByte(Status.Active) : Convert.ToByte(Status.Pending),
                   // BankTransactionRefId = Convert.ToString(entity.BankTransactionRefID)
                };
            }
        }

        //Return Transfer Mappers
        public static JournalEntry MapReturnTransfer(SaveOrEditFundTransferReq entity,JournalEntry? origEntity = null)
        {
            if (origEntity != null)
            {
                return origEntity;
            }

            return new JournalEntry
            {
                Id = new PFAID().UID,
                EntryDate = entity.TransactionDate ?? DateTime.Now,
                EntryNumber = entity.EntryNum,
                CreatedDate = DateTime.Now,
                SourceType = (short)JournalSourceTypes.ReturnTransfer,   // 129
                ParentSourceType = (short)JournalSourceTypes.InterCompany,
                TransferMode = (short)IntercompanyTransferMode.InterCompanies,
                Status = (short)Status.Active,
                IsAttachment = Convert.ToInt16(entity.HasAttachments),
                ClearedDate = entity.ClearedDate
            };
        }

        public static Transaction MapReturnTransferRoot(SaveOrEditFundTransferReq entity,byte[] journalEntryId,Transaction? origEntity = null)
        {
            if (origEntity != null)
            {
                return origEntity;
            }

            return new Transaction
            {
                Id = new PFAID().UID,
                JournalEntryId = journalEntryId,
                Amount = 0,
                DebitCredit = Convert.ToBoolean(TransactionDebitCredit.Debit),
                TransactionDate = DateTime.Now,
                ReferenceNumber =(!string.IsNullOrEmpty( entity.RefNumber))?(entity.RefNumber.Length>50?entity.RefNumber.Substring(0,50):entity.RefNumber):entity.RefNumber,
                Memo = entity.Memo != null ? entity.Memo : null,
                Status = (short)Status.Active,
                SourceType = (short)JournalSourceTypes.ReturnTransfer // 129
            };
        }

        public static Transaction MapReturnTransferSplit(MultipleSplitRows entity, byte[] journalEntryId,Transaction? origEntity = null)
        {
            if (origEntity != null)
            {
                return origEntity;
            }

            return new Transaction
            {
                Id = new PFAID().UID,
                JournalEntryId = journalEntryId,
                Amount = entity.SplitAmount,
                DebitCredit = Convert.ToBoolean(TransactionDebitCredit.Credit),
                TransactionDate = DateTime.Now,
                ReferenceNumber = !string.IsNullOrEmpty(entity.Memo) ? (entity.Memo.Length > 50 ? entity.Memo.Substring(0, 50) : entity.Memo) : entity.Memo,//entity.Memo.Length < 25 ? entity.Memo : entity.Memo.Substring(0, 24),
                Memo = entity.Memo,
                Status = (short)Status.Active,
                StoreId = string.IsNullOrEmpty(entity.ProfitcenterID) ? null: new PFAID(entity.ProfitcenterID).UID
            };
        }
        /// <summary>
        /// Mapping Fundtransferrequest to JournalEntryExt
        /// </summary>
        /// <param name="JournalEntryId"></param>
        /// <param name="VoidReason"></param>
        /// <param name="origEntity">origEntity represents JournalEntryExt</param>
        /// <returns>It returns mapped JournalEntryExt</returns>
        public static JournalEntryExt MapJournalEntryExt(byte[] JournalEntryId, string VoidReason = null, JournalEntryExt origEntity = null)
        {
            if (origEntity != null)
            {
                origEntity.VoidRemarks = VoidReason ?? null;
                return origEntity;
            }
            else
            {
                return new JournalEntryExt
                {
                    JournalEntryId = JournalEntryId,
                    VoidRemarks = VoidReason ?? null,
                };
            }
        }




        #endregion

        #region BankFeedMappers

        /// <summary>
        /// maps postreq data to checkentryrequest
        /// </summary>
        /// <param name="PostReq"></param>
        /// <returns>It returns CheckEntryRequest </returns>
        public static CheckEntryRequest MapEntryRequest(SinglePostRequest PostReq, byte[] DefPurposeID, byte[] DefAccID)
        {
            CheckEntryRequest chekReq = new CheckEntryRequest();
            chekReq.CorpID = PostReq.CorporationID;
            chekReq.EntryDate = PostReq.EntryDate.Date;
            chekReq.EntryNumber = PostReq.CheckNo;
            chekReq.IsMultiplePost = PostReq.IsMultiplePost;
            //OrigEntity.PaymentMethodID =;need to get it through miscinfo table through clientID
            chekReq.AccountID = !string.IsNullOrEmpty(PostReq.NimbleAccountID) ? PostReq.NimbleAccountID : PostReq.AccountID;
            chekReq.Amount = PostReq.Amount;
            chekReq.ReferenceNumber = PostReq.CheckNo;
            chekReq.PayeeID = !string.IsNullOrEmpty(PostReq.NameID) ? PostReq.NameID : null;
            chekReq.PayeeType = !string.IsNullOrEmpty(PostReq.NameType) ? Convert.ToInt16(PostReq.NameType) : null;
            chekReq.ClearedDate = Convert.ToString(PostReq.ClearedDate);
            chekReq.PaymentMethodID = PostReq.PaymentMethodID;
            chekReq.IsBankTransactionRefID = true;
            chekReq.BankTransactionRefID = Convert.ToString(PostReq.FeedTransactionID);
            chekReq.HasAttachments = PostReq.HasAttachments;
            chekReq.Memo = PostReq.Memo;
            chekReq.ClientID= PostReq.ClientID;
            //chekReq.sourcety
            JournalDivisionDTO divs = new JournalDivisionDTO();
            divs.AccountID = !string.IsNullOrEmpty(PostReq.AccountID) ? PostReq.AccountID : new PFAID(DefAccID).ToString();
            divs.Debit = PostReq.Amount;
            divs.Description = PostReq.Memo;
            divs.StoreID = PostReq.ProfitCenter;
            //divs.IsBankTransactionRefID = true;
            divs.SourceID = new PFAID(DefPurposeID).ToString();
            if (PostReq.AccountTypeID != new PFAID(EnumUtils.stringValueOf(AccountTypesEnum.AccountsReceiveble)).ToString() && PostReq.AccountTypeID != new PFAID(EnumUtils.stringValueOf(AccountTypesEnum.AccountsPayable)).ToString())
            {
                divs.TargetID = null;
                divs.TargetType = null;
            }
            else
            {
                divs.TargetID = !string.IsNullOrEmpty(PostReq.NameID) ? PostReq.NameID : null;
                divs.TargetType = !string.IsNullOrEmpty(PostReq.NameType) ? Convert.ToInt16(PostReq.NameType) : null;

            }
            //divs.TargetID = !string.IsNullOrEmpty(PostReq.NameID) ? PostReq.NameID : null;
            //divs.TargetType = !string.IsNullOrEmpty(PostReq.NameType) ? Convert.ToInt16(PostReq.NameType) : null;
            //divs.BankTransactionRefID = Convert.ToString(PostReq.FeedTransactionID);
            chekReq.Divisions.Add(divs);

            return chekReq;
        }

        /// <summary>
        /// maps postreq data to JournalEntryRequest
        /// </summary>
        /// <param name="PostReq"></param>
        /// <returns>It returns JournalEntryRequest</returns>
        public static JournalEntryRequest MapJournalRequest(SinglePostRequest PostReq, byte[] DefPurposeID, byte[] DefAccID)
        {
            JournalEntryRequest journalReq = new JournalEntryRequest();
            journalReq.CorpID = PostReq.CorporationID;
            journalReq.EntryDate = PostReq.EntryDate.Date;
            journalReq.EntryNumber = PostReq.CheckNo;
            journalReq.ClearedDate = Convert.ToString(PostReq.ClearedDate);
            journalReq.HasAttachments = PostReq.HasAttachments;
            journalReq.IsMultiplePost = PostReq.IsMultiplePost;
            journalReq.ClientID = PostReq.ClientID;
            JournalDivisionDTO div = new JournalDivisionDTO();
            div.AccountID = PostReq.NimbleAccountID;
            div.Credit = PostReq.Amount;
            div.SourceID= !string.IsNullOrEmpty(PostReq.NameID) ? PostReq.NameID : null;
            div.SourceType = !string.IsNullOrEmpty(PostReq.NameType) ? Convert.ToInt16(PostReq.NameType) : null;
           // div.TargetID =  !string.IsNullOrEmpty(PostReq.NameID) ? PostReq.NameID : null;
           // div.TargetType = !string.IsNullOrEmpty(PostReq.NameType) ? Convert.ToInt16(PostReq.NameType) : null;
            div.Description = PostReq.Memo;
            div.BankTransactionRefID = PostReq.FeedTransactionID.ToString();
            div.IsBankTransactionRefID = true;
            journalReq.Divisions.Add(div);
            JournalDivisionDTO div1 = new JournalDivisionDTO();
            div1.AccountID = !string.IsNullOrEmpty(PostReq.AccountID) ? PostReq.AccountID : new PFAID(DefAccID).ToString();
            div1.Debit = PostReq.Amount;
            div1.Description = PostReq.Memo;
            div1.StoreID = PostReq.ProfitCenter;
            div1.SourceID = new PFAID(DefPurposeID).ToString();
            if(PostReq.AccountTypeID!= new PFAID(EnumUtils.stringValueOf(AccountTypesEnum.AccountsReceiveble)).ToString()&& PostReq.AccountTypeID != new PFAID(EnumUtils.stringValueOf(AccountTypesEnum.AccountsPayable)).ToString())
            {
                div1.TargetID = null;
                div1.TargetType= null;  
            }
            else
            {
                div1.TargetID = !string.IsNullOrEmpty(PostReq.NameID) ? PostReq.NameID : null;
                div1.TargetType = !string.IsNullOrEmpty(PostReq.NameType) ? Convert.ToInt16(PostReq.NameType) : null;

            }
            //div1.BankTransactionRefID = PostReq.FeedTransactionID.ToString();
           // div1.IsBankTransactionRefID = true;
            journalReq.Divisions.Add(div1);
            return journalReq;
        }

        /// <summary>
        /// maps postreq data to BillPaymentRequest
        /// </summary>
        /// <param name="PostReq"></param>
        /// <returns>It returns BillPaymentRequest</returns>
        // TODO : need to move to payable module
        public static BillPaymentRequest MapBillPayRequest(SinglePostRequest PostReq, byte[] DefAcc)
        {
            BillPaymentRequest billReq = new BillPaymentRequest();
            billReq.CorpID = PostReq.CorporationID;
            billReq.PayeeID = PostReq.NameID;
            billReq.BankAccountID = PostReq.NimbleAccountID;
            billReq.PayeeAccountID = new PFAID(DefAcc).ToString();
            billReq.PaymentMethodID = PostReq.PaymentMethodID;
            billReq.EntryDate = PostReq.EntryDate.Date.ToString();
            billReq.CheckNumber = PostReq.CheckNo;
            billReq.Memo = PostReq.Memo;
            billReq.Amount = PostReq.Amount;
            billReq.ReferenceNumber = string.Empty;
            billReq.BankTransRefID = PostReq.FeedTransactionID;
            billReq.HasAttachments = PostReq.HasAttachments;
            billReq.ClearedDate=PostReq.ClearedDate.ToString();
            billReq.BillInfoID= PostReq.BillInfoID;
            billReq.IsPossiblematch= PostReq.IsPossiblematch;
            return billReq;
        }

        /// <summary>
        /// maps PostReq to CustomerRecieptRequest
        /// </summary>
        /// <param name="PostReq"></param>
        /// <returns></returns>
        public static CustomReceiptsRequest MapCustomerReceiptRequest(SinglePostRequest PostReq, byte[] DefAccID)
        {
            return new CustomReceiptsRequest()
            {
                CororationID = PostReq.CorporationID,
                CreatedDate = PostReq.EntryDate.Date,
                EntryNumber = PostReq.CheckNo,
                AccountID = PostReq.NimbleAccountID,
                ReceivedFrom = PostReq.NameID,
                Amount = PostReq.Amount,
                ReferenceNumber = PostReq.CheckNo,
                Memo = PostReq.Memo,
                IsBankTransRefID = true,
                BankTransRefID = PostReq.FeedTransactionID,
                HasAttachments = PostReq.HasAttachments,
                IsMultiplePost = Convert.ToBoolean(PostReq.IsMultiplePost),
                ClearedDate = PostReq.ClearedDate,
                ClientID = PostReq.ClientID,
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PostReq"></param>
        /// <returns></returns>
        public static FundTransferRequest MapFundtransferRequest(SinglePostRequest PostReq)
        {
            FundTransferRequest fundTransferReq = new FundTransferRequest();
            fundTransferReq.CorpID = PostReq.CorporationID;
            fundTransferReq.CreditAccount = !string.IsNullOrEmpty(PostReq.NimbleAccountID) ? PostReq.NimbleAccountID : PostReq.AccountID;
            fundTransferReq.Amount = PostReq.Amount;
            fundTransferReq.RefNumber = PostReq.CheckNo;
            fundTransferReq.EntryNum = PostReq.CheckNo;
            fundTransferReq.TransactionDate = PostReq.EntryDate.Date;
            fundTransferReq.ClearedDate = Convert.ToString(PostReq.ClearedDate);
            fundTransferReq.IsBankTransactionRefID = true;
            fundTransferReq.BankTransactionRefID = Convert.ToString(PostReq.FeedTransactionID);
            fundTransferReq.Memo = PostReq.Memo;
            fundTransferReq.IsMultiplePost = PostReq.IsMultiplePost;
            fundTransferReq.HasAttachments = PostReq.HasAttachments;
            fundTransferReq.ClientID = PostReq.ClientID;
            FundTransferDivisionDTO divs = new FundTransferDivisionDTO();
            divs.DebitFromAccount = PostReq.AccountID;
            divs.SplitAmount = PostReq.Amount;
            divs.Memo = PostReq.Memo;
            divs.IsBankTransactionRefID = true;
            divs.BankTransactionRefID = PostReq.FeedTransactionID;
            divs.ProfitcenterID = PostReq.ProfitCenter;
            fundTransferReq.FundTransferDivisions.Add(divs);
            return fundTransferReq;


        }

        /// <summary>
        /// maps postre and checkresp data to SinglePostResponse
        /// </summary>
        /// <param name="PostRequest"></param>
        /// <param name="CheckResponse"></param>
        /// <returns>It returns SinglePostResponse </returns>
        public static SinglePostResponse MapPostResponse(SinglePostRequest PostRequest, SinglePostData CheckResponse, byte[] DefAccID)
        {
            SinglePostResponse postResp = new SinglePostResponse();
            postResp.JournalID = CheckResponse.ID;
            postResp.TransactionID = new PFAID(CheckResponse.TransactionID).ToString();
            postResp.FeedTransactionID = PostRequest.FeedTransactionID;
            postResp.TransactionType = PostRequest.TransactionPostType;
            postResp.NameID = !string.IsNullOrEmpty(PostRequest.NameID) ? PostRequest.NameID : null;
            postResp.NameType = !string.IsNullOrEmpty(PostRequest.NameType) ? PostRequest.NameType : null;
            postResp.Status = Constants.MSG_POST_SUC;
            postResp.StatusCode = CheckResponse.StatusCode;
            postResp.NimbleAccountID = !string.IsNullOrEmpty(PostRequest.AccountID) ? PostRequest.AccountID : new PFAID(DefAccID).ToString();
            postResp.ProfitCenterID = !string.IsNullOrEmpty(PostRequest.ProfitCenter) ? PostRequest.ProfitCenter : null;
            postResp.Amount = PostRequest.Amount;
            postResp.CheckNO= PostRequest.CheckNo;
            postResp.Description = PostRequest.Memo;
            postResp.EntryDate = PostRequest.EntryDate.Date;
            return postResp;
        }
        #endregion

        #region Customerreceiptmapper 
        /// <summary>
        /// Mapping CustomerReceiptRequest feilds to JournalEntry
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="origEntity"></param>
        /// <returns>It returns mapped JournalEntry</returns>
        public static JournalEntry MapCustomerReceipt(CustomReceiptsRequest CustomerReq)
        {
            return new JournalEntry()
            {
                Id = new PFAID().UID,
                CorporationId = !string.IsNullOrEmpty(CustomerReq.CororationID) ? new PFAID(CustomerReq.CororationID).UID : null,
                EntryDate = CustomerReq.CreatedDate.Date,
                EntryNumber = CustomerReq.EntryNumber,
                CreatedDate = DateTime.Now,
                SourceType = (short)JournalSourceTypes.InvoiceReceipt,
                ParentSourceType = (short)JournalSourceTypes.InvoiceReceipt,
                Status = (short)Status.Active,
                ClearedDate = CustomerReq.ClearedDate,
                ReferenceMode = 2,
                IsAttachment = Convert.ToInt16(CustomerReq.HasAttachments)
            };
        }

        /// <summary>
        /// Mapping CustomerReceiptRequest  feilds to Transaction 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="JournalEntryId"></param>
        /// <param name="origEntity"></param>
        /// <returns>It returns Mapped Transaction</returns>
        public static Transaction MapTransaction(CustomReceiptsRequest entity, byte[] JournalEntryId, Transaction? origEntity = null)
        {
            if (origEntity != null)
            {
                return origEntity;
            }
            else
            {
                return new Transaction()
                {
                    Id = new PFAID().UID,
                    AccountId = new PFAID(entity.AccountID).UID,
                    SourceId = new PFAID(entity.ReceivedFrom).UID,
                    SourceType = Convert.ToInt16(TransactionSourceType.Customer),
                    JournalEntryId = JournalEntryId,
                    Amount = entity.Amount,
                    DebitCredit = false,
                    Status = (short)Status.Active,
                    TransactionDate = DateTime.Now,
                    Memo = entity.Memo,
                    ReferenceNumber = !string.IsNullOrEmpty(entity.ReferenceNumber) ? (entity.ReferenceNumber.Length > 50 ? entity.ReferenceNumber.Substring(0, 50) : entity.ReferenceNumber) : entity.ReferenceNumber,// !string.IsNullOrEmpty(entity.ReferenceNumber) ? entity.ReferenceNumber : null,
                    IsBankTransactionRefId = (entity.IsBankTransRefID) ? Convert.ToByte(Status.Active) : Convert.ToByte(Status.Pending),
                    BankTransactionRefId = Convert.ToString(entity.BankTransRefID)
                };

            }
        }
        #endregion



    }
}
