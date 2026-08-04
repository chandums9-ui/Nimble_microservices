using Common.Domain.DTO.App;
using Common.Domain.DTO.Enums;
using Common.Domain.DTO.Req;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel.Domain.DataModel;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Model;
using Microsoft.IdentityModel.Tokens;

namespace Common.Domain.Common
{
    /// <summary>
    /// Mapper class for common 
    /// </summary>
    public static class Mapper
    {
        #region JournalEntry

        /// <summary>
        /// Mapping JournalEntryRequest to JournalEntry
        /// </summary>
        /// <param name="entity">entity represents JournalEntryRequest </param>
        /// <param name="origEntity">origEntity represents JournalEntry </param>
        /// <returns>It returns mapped JournalEntry</returns>
        public static JournalEntry MapJournalEntry(JournalEntryRequest entity, JournalEntry? origEntity = null)
        {
            if (origEntity != null)
            {
                //origEntity.CorporationId = new PFAID(entity.CorpID).UID;
                origEntity.EntryDate = Convert.ToDateTime(entity.EntryDate);
                origEntity.EntryNumber = entity.EntryNumber;
                origEntity.IsAccrual = (entity.IsAccrual) ? Convert.ToInt16(entity.IsAccrual) : origEntity.IsAccrual;
                origEntity.IsPrintRequired = (entity.IsPrintEntry) ? entity.IsPrintEntry : origEntity.IsPrintRequired;
                origEntity.ModifiedDate = DateTime.Now;
                origEntity.IsAttachment = (entity.HasAttachments) ? Convert.ToInt16(entity.HasAttachments) : origEntity.IsAttachment;
                //origEntity.SourceType = (short)JournalSourceTypes.Journal;
                //origEntity.ParentSourceType = (short)JournalSourceTypes.Journal;
                //origEntity.Status = (short)Status.Active;
                //origEntity.ClearedDate = !string.IsNullOrEmpty(entity.ClearedDate) ? Convert.ToDateTime(entity.ClearedDate) : null;

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
                    IsAccrual = Convert.ToInt16(entity.IsAccrual),
                    IsPrintRequired = entity.IsPrintEntry,
                    CreatedDate = DateTime.Now,
                    IsAttachment = Convert.ToInt16(entity.HasAttachments),
                    SourceId = (!string.IsNullOrEmpty(entity.SourceID)) ? new PFAID(entity.SourceID).UID : null,
                    SourceType = (short)JournalSourceTypes.Journal,
                    ParentSourceType = (short)JournalSourceTypes.Journal,
                    Status = (short)Status.Active,
                    ClearedDate = !string.IsNullOrEmpty(entity.ClearedDate) ? Convert.ToDateTime(entity.ClearedDate) : null,
                };
            }
        }

        /// <summary>
        /// Mapping JournalEntryRequest to OCR JournalEntry
        /// </summary>
        /// <param name="entity">entity represents JournalEntryRequest </param>
        /// <param name="origEntity">origEntity represents JournalEntry </param>
        /// <returns>It returns mapped JournalEntry</returns>
        public static OcrjournalEntry MapOCRJournalEntry(JournalEntryRequest entity, short? userApprovalType = null, OcrjournalEntry? origEntity = null)
        {
            if (origEntity != null)
            {
                origEntity.EntryDate = Convert.ToDateTime(entity.EntryDate);
                origEntity.EntryNumber = entity.EntryNumber;
                origEntity.ModifiedDate = DateTime.Now;
                if (entity.HasAttachments)
                    origEntity.IsAttachment = Convert.ToInt16(entity.HasAttachments);
                origEntity.SourceType = (short)JournalSourceTypes.UnApproveJournal;
                origEntity.ParentSourceType = (short)JournalSourceTypes.Journal;
                //origEntity.Status = (short)Status.Active;

                return origEntity;
            }
            else
            {
                return new OcrjournalEntry()
                {
                    Id = (!string.IsNullOrEmpty(entity.ID)) ? new PFAID(entity.ID).UID : new PFAID().UID,
                    CorporationId = new PFAID(entity.CorpID).UID,
                    EntryDate = Convert.ToDateTime(entity.EntryDate),
                    EntryNumber = entity.EntryNumber,
                    CreatedDate = DateTime.Now,
                    IsAttachment = Convert.ToInt16(entity.HasAttachments),
                    SourceId = (!string.IsNullOrEmpty(entity.SourceID)) ? new PFAID(entity.SourceID).UID : null,
                    SourceType = (short)JournalSourceTypes.UnApproveJournal,
                    ParentSourceType = (short)JournalSourceTypes.Journal,
                    Status = (short)Status.Active,
                    ApprovalStatus = userApprovalType != null ? (short)userApprovalType.Value : (short)EntryApprovalStatus.Entry
                };
            }
        }
        /// <summary>
        /// Mapping JournalEntryRequest to JournalEntryExt
        /// </summary>
        /// <param name="JournalEntryId"></param>
        /// <param name="VoidReason"></param>
        /// <param name="origEntity">origEntity represents JournalEntryExt</param>
        /// <returns>It returns mapped JournalEntryExt</returns>
        public static JournalEntryExt MapJournalEntryExt(byte[] JournalEntryId, string VoidReason, JournalEntryExt? origEntity = null)
        {
            if (origEntity != null)
            {
                origEntity.VoidRemarks = VoidReason;
                return origEntity;
            }
            else
            {
                return new JournalEntryExt
                {
                    JournalEntryId = JournalEntryId,
                    VoidRemarks = VoidReason,
                };
            }
        }

        #region Transaction
        /// <summary>
        /// Mapping JournalDivision to Transaction
        /// </summary>
        /// <param name="entity">entity represents JournalDivision</param>
        /// <param name="JournalEntryId"></param>
        /// <param name="ReferenceNumber"></param>
        /// <param name="origEntity">origEntity represents Transaction</param>
        /// <returns>It returns Mapped Transaction</returns>
        public static Transaction MapTransaction(JournalDivisionDTO entity, byte[] JournalEntryId, byte[] DefaultPurposeID, byte[] DefaultStoreID, string? ReferenceNumber = null, Transaction? origEntity = null)
        {
            if (origEntity != null)
            {
                //origEntity.Id;
                //origEntity.JournalEntryId;
                origEntity.AccountId = new PFAID(entity.AccountID).UID;
                origEntity.Memo = entity.Description;
                origEntity.TargetId = string.IsNullOrEmpty(entity.TargetID) ? null : new PFAID(entity.TargetID).UID;
                origEntity.TargetType = entity.TargetType;
                origEntity.SourceId = string.IsNullOrEmpty(entity.SourceID) ? DefaultPurposeID : new PFAID(entity.SourceID).UID;
                origEntity.SourceType = entity.SourceType.HasValue ? entity.SourceType.Value : (short)TransactionSourceType.Purpose;
                origEntity.StoreId = string.IsNullOrEmpty(entity.StoreID) ? DefaultStoreID : new PFAID(entity.StoreID).UID;
                origEntity.TransactionDate = DateTime.Now; //y?
                origEntity.ReferenceNumber = ReferenceNumber;
                origEntity.DebitCredit = (entity.Debit >= 0 && entity.Credit == 0) ? false : true;
                origEntity.Amount = (entity.Debit >= 0 && entity.Credit == 0) ? entity.Debit : entity.Credit;
                origEntity.IsBankTransactionRefId = Convert.ToByte(entity.IsBankTransactionRefID);
                origEntity.BankTransactionRefId = !string.IsNullOrEmpty(entity.BankTransactionRefID) ? entity.BankTransactionRefID : null;
                origEntity.Status = (short)Status.Active;

                if (origEntity.TransactionInvoice == null)
                {
                    origEntity.TransactionInvoice = new TransactionInvoice();
                    origEntity.TransactionInvoice.Id = origEntity.Id;
                }
                origEntity.TransactionInvoice.Rate = origEntity.Amount;
                origEntity.TransactionInvoice.Hrs = entity.Statistics;
                origEntity.TransactionInvoice.Status = Convert.ToByte(Status.Active);

                return origEntity;
            }
            else
            {
                origEntity = new Transaction();
                origEntity.Id = (!string.IsNullOrEmpty(entity.ID)) ? new PFAID(entity.ID).UID : new PFAID().UID;
                origEntity.JournalEntryId = JournalEntryId;
                origEntity.AccountId = new PFAID(entity.AccountID).UID;
                origEntity.Memo = entity.Description;
                origEntity.TargetId = string.IsNullOrEmpty(entity.TargetID) ? null : new PFAID(entity.TargetID).UID;
                origEntity.TargetType = entity.TargetType;
                //if(entity.SourceID)
                origEntity.SourceId = string.IsNullOrEmpty(entity.SourceID) ? DefaultPurposeID : new PFAID(entity.SourceID).UID;
                origEntity.StoreId = string.IsNullOrEmpty(entity.StoreID) ? DefaultStoreID : new PFAID(entity.StoreID).UID;
                origEntity.TransactionDate = DateTime.Now;
                origEntity.ReferenceNumber = ReferenceNumber;
                origEntity.DebitCredit = (entity.Debit >= 0 && entity.Credit == 0) ? false : true;
                origEntity.Amount = (entity.Debit >= 0 && entity.Credit == 0) ? entity.Debit : entity.Credit;
                origEntity.Status = (short)Status.Active;
                origEntity.SourceType = string.IsNullOrEmpty(new PFAID(DefaultPurposeID).ToString())? entity.SourceType: (short)TransactionSourceType.Purpose;
                
                origEntity.IsBankTransactionRefId = Convert.ToByte(entity.IsBankTransactionRefID);
                origEntity.BankTransactionRefId = !string.IsNullOrEmpty(entity.BankTransactionRefID) ? entity.BankTransactionRefID : null;

                origEntity.TransactionInvoice = new TransactionInvoice();
                origEntity.TransactionInvoice.Id = origEntity.Id;
                origEntity.TransactionInvoice.Hrs = entity.Statistics;
                origEntity.TransactionInvoice.Rate = origEntity.Amount;
                origEntity.TransactionInvoice.Status = Convert.ToByte(Status.Active);

                return origEntity;
            }
        }

        public static Ocrtransaction MapOCRTransaction(JournalDivisionDTO entity, byte[] JournalEntryId, byte[] DefaultPurposeID, byte[] DefaultStoreID, string? ReferenceNumber = null, Ocrtransaction? origEntity = null)
        {
            if (origEntity != null)
            {
                //origEntity.Id;
                //origEntity.JournalEntryId;
                origEntity.AccountId = new PFAID(entity.AccountID).UID;
                origEntity.Memo = !string.IsNullOrEmpty(entity.Description) ? entity.Description : null; 
                origEntity.TargetId = string.IsNullOrEmpty(entity.TargetID) ? null : new PFAID(entity.TargetID).UID;
                origEntity.TargetType = entity.TargetType;
                origEntity.SourceId = string.IsNullOrEmpty(entity.SourceID) ? DefaultPurposeID : new PFAID(entity.SourceID).UID;
                origEntity.SourceType = entity.SourceType.HasValue?entity.SourceType.Value: (short)TransactionSourceType.Purpose; 
                origEntity.StoreId = string.IsNullOrEmpty(entity.StoreID) ? DefaultStoreID : new PFAID(entity.StoreID).UID;
                origEntity.TransactionDate = DateTime.Now; //y?
                origEntity.ReferenceNumber = ReferenceNumber;
                origEntity.DebitCredit = (entity.Debit >= 0 && entity.Credit == 0) ? false : true;
                origEntity.Amount = (entity.Debit >= 0 && entity.Credit == 0) ? entity.Debit : entity.Credit;
                origEntity.Status = (short)Status.Active;

                if (origEntity.OcrtransactionInvoice == null)
                {
                    origEntity.OcrtransactionInvoice = new OcrtransactionInvoice();
                    origEntity.OcrtransactionInvoice.Id = origEntity.Id;
                }
                origEntity.OcrtransactionInvoice.Rate = origEntity.Amount;
                origEntity.OcrtransactionInvoice.Hrs = entity.Statistics;
                origEntity.OcrtransactionInvoice.Status = Convert.ToByte(Status.Active);

                return origEntity;
            }
            else
            {
                origEntity = new Ocrtransaction();
                origEntity.Id = (!string.IsNullOrEmpty(entity.ID)) ? new PFAID(entity.ID).UID : new PFAID().UID;
                origEntity.JournalEntryId = JournalEntryId;
                origEntity.AccountId = new PFAID(entity.AccountID).UID;
                origEntity.Memo = entity.Description;
                origEntity.TargetId = string.IsNullOrEmpty(entity.TargetID) ? null : new PFAID(entity.TargetID).UID;
                origEntity.TargetType = entity.TargetType;
                //if(entity.SourceID)
                origEntity.SourceId = string.IsNullOrEmpty(entity.SourceID) ? DefaultPurposeID : new PFAID(entity.SourceID).UID;//purpose
                origEntity.StoreId = string.IsNullOrEmpty(entity.StoreID) ? DefaultStoreID : new PFAID(entity.StoreID).UID;
                origEntity.TransactionDate = DateTime.Now;
                origEntity.ReferenceNumber = ReferenceNumber;
                origEntity.DebitCredit = (entity.Debit >= 0 && entity.Credit == 0) ? false : true;
                origEntity.Amount = (entity.Debit >= 0 && entity.Credit == 0) ? entity.Debit : entity.Credit;
                origEntity.Status = (short)Status.Active;
                origEntity.SourceType = (short)TransactionSourceType.Purpose;//string.IsNullOrEmpty(new PFAID(DefaultPurposeID).ToString()) ? entity.SourceType : 

                origEntity.OcrtransactionInvoice = new OcrtransactionInvoice();
                origEntity.OcrtransactionInvoice.Id = origEntity.Id;
                origEntity.OcrtransactionInvoice.Hrs = entity.Statistics;
                origEntity.OcrtransactionInvoice.Rate = origEntity.Amount;
                origEntity.OcrtransactionInvoice.Status = Convert.ToByte(Status.Active);

                return origEntity;
            }
        }

        public static Transaction MapTransaction(Ocrtransaction entity)
        {
            var origEntity = new Transaction();
            origEntity.Id = entity.Id;
            origEntity.JournalEntryId = entity.JournalEntryId;
            origEntity.AccountId = new PFAID(entity.AccountId).UID;
            origEntity.Memo = entity.Memo;
            origEntity.TargetId = entity.TargetId;
            origEntity.TargetType = entity.TargetType;
            //if(entity.SourceID)
            origEntity.SourceId = entity.SourceId;
            origEntity.StoreId = entity.StoreId;
            origEntity.TransactionDate = entity.TransactionDate.Value;
            origEntity.ReferenceNumber = entity.ReferenceNumber;
            origEntity.DebitCredit = entity.DebitCredit.HasValue ? entity.DebitCredit.Value : false;
            origEntity.Amount = entity.Amount.HasValue ? entity.Amount.Value : 0;
            origEntity.Status = entity.Status.HasValue ? entity.Status.Value : (short)Status.Active;
            origEntity.SourceType = entity.SourceType.HasValue ? entity.SourceType.Value : (short)TransactionSourceType.Purpose;
            if (entity.OcrtransactionInvoice != null)
            {
                origEntity.TransactionInvoice = new TransactionInvoice();
                origEntity.TransactionInvoice.Id = entity.OcrtransactionInvoice.Id;
                origEntity.TransactionInvoice.Hrs = entity.OcrtransactionInvoice.Hrs;
                origEntity.TransactionInvoice.Rate = entity.OcrtransactionInvoice.Rate;
                origEntity.TransactionInvoice.Status = entity.OcrtransactionInvoice.Status;
            }
            return origEntity;
        }

        public static Transaction MapSplitTransaction(JournalDivisionDTO entity, Transaction tran, Transaction? origEntity = null)
        {
            if (origEntity != null)
            {
                origEntity.AccountId = new PFAID(entity.AccountID).UID;
                // origEntity.JournalEntryId = tran.JournalEntryId;
                origEntity.ParentId = tran.Id ?? null;
                origEntity.Amount = entity.Debit;
                origEntity.DebitCredit = Convert.ToBoolean(TransactionDebitCredit.Debit);
                origEntity.TransactionDate = DateTime.Now;
                origEntity.ReferenceNumber = tran.ReferenceNumber != null ? tran.ReferenceNumber : null;
                origEntity.Memo = entity.Description;
                origEntity.Status = (short)Status.Active;
                origEntity.SourceId = entity.SourceID != null ? new PFAID(entity.SourceID).UID : null;
                origEntity.SourceType = entity.SourceType.HasValue ? entity.SourceType.Value : (short)TransactionSourceType.Purpose;
                origEntity.StoreId = string.IsNullOrEmpty(entity.StoreID) ? null : new PFAID(entity.StoreID).UID;
               // origEntity.TargetId = entity.TargetID == null ? null : new PFAID(entity.TargetID).UID;
                origEntity.TargetType = entity.TargetType == null ? null : entity.TargetType;
                if (origEntity.TransactionInvoice != null)
                {
                    origEntity.TransactionInvoice = new TransactionInvoice();
                    origEntity.TransactionInvoice.Id = origEntity.Id;
                }
                origEntity.TransactionInvoice.Rate = Convert.ToDecimal(entity.Debit);
                origEntity.TransactionInvoice.Status = Convert.ToByte(Status.Active);
                origEntity.IsBankTransactionRefId = Convert.ToByte(entity.IsBankTransactionRefID);
                origEntity.BankTransactionRefId = !string.IsNullOrEmpty(entity.BankTransactionRefID) ? entity.BankTransactionRefID : null;
                return origEntity;
            }
            else
            {
                origEntity = new Transaction();
                origEntity.Id = (!string.IsNullOrEmpty(entity.ID)) ? new PFAID(entity.ID).UID : new PFAID().UID;
                origEntity.AccountId = new PFAID(entity.AccountID).UID;
                origEntity.JournalEntryId = tran.JournalEntryId;
                origEntity.ParentId = tran.Id ?? null;
                origEntity.Amount = entity.Debit;
                origEntity.DebitCredit = Convert.ToBoolean(TransactionDebitCredit.Debit);
                origEntity.TransactionDate = DateTime.Now;
                origEntity.ReferenceNumber = tran.ReferenceNumber != null ? tran.ReferenceNumber : null;
                origEntity.Memo = entity.Description;
                origEntity.Status = (short)Status.Active;
                origEntity.SourceId = !string.IsNullOrEmpty(entity.SourceID) ? new PFAID(entity.SourceID).UID : null;
                origEntity.SourceType = (short)TransactionSourceType.Purpose;
                origEntity.StoreId = string.IsNullOrEmpty(entity.StoreID) ? null : new PFAID(entity.StoreID).UID;
                origEntity.TargetId = entity.TargetID == null ? null : new PFAID(entity.TargetID).UID;
                origEntity.TargetType = entity.TargetType == null ? null : entity.TargetType;
                origEntity.TransactionInvoice = new TransactionInvoice();
                origEntity.TransactionInvoice.Id = origEntity.Id;
                origEntity.TransactionInvoice.Rate = Convert.ToDecimal(entity.Debit);
                origEntity.TransactionInvoice.Status = Convert.ToByte(Status.Active);
                //origEntity.IsBankTransactionRefId = Convert.ToByte(entity.IsBankTransactionRefID);
                //origEntity.BankTransactionRefId = !string.IsNullOrEmpty(entity.BankTransactionRefID) ? entity.BankTransactionRefID : null;

                return origEntity;
            }
        }

        #endregion

        #region RepetitiveTransaction

        /// <summary>
        /// Mappping JournalDivision to RepetitiveTransaction
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="RepetativeId"></param>
        /// <param name="ReferenceNumber"></param>
        /// <param name="origEntity"></param>
        /// <returns>It returns Mapped RepetitiveTransaction</returns>
        public static RepetitiveTransaction MapRepetitiveTransaction(JournalDivisionDTO entity, Repetitive RepetativeInfo, string? ReferenceNumber = null, string? EntryNumber = null, RepetitiveTransaction? origEntity = null)
        {
            if (origEntity != null)
            {
                origEntity.AccountId = new PFAID(entity.AccountID).UID;
                origEntity.Memo = entity.Description;
                // origEntity.TargetId = RepetativeId;
                origEntity.SourceId = string.IsNullOrEmpty(entity.SourceID) ? null : new PFAID(entity.SourceID).UID;
                origEntity.StoreId = string.IsNullOrEmpty(entity.StoreID) ? null : new PFAID(entity.StoreID).UID;
                origEntity.TransactionDate = DateTime.Now;
                if (!string.IsNullOrEmpty(ReferenceNumber))
                    origEntity.ReferenceNumber = ReferenceNumber;
                if (!string.IsNullOrEmpty(EntryNumber))
                    origEntity.EntryNumber = EntryNumber;
                origEntity.DebitCredit = (entity.Debit >= 0 && entity.Credit == 0) ? false : true;
                origEntity.Amount = (entity.Debit >= 0 && entity.Credit == 0) ? entity.Debit : entity.Credit;
                if (origEntity.RepetitiveTransactionInvoice == null)
                {
                    origEntity.RepetitiveTransactionInvoice = new RepetitiveTransactionInvoice();
                    origEntity.RepetitiveTransactionInvoice.Id = origEntity.Id;
                }
                origEntity.RepetitiveTransactionInvoice.Rate = origEntity.Amount;
                origEntity.RepetitiveTransactionInvoice.Hrs = entity.Statistics;
                origEntity.RepetitiveTransactionInvoice.Status = Convert.ToByte(Status.Active);
                //origEntity.Status = entity.st(short)Status.Active;
                //origEntity.Order
                origEntity.SourceType = entity.SourceType.HasValue ? entity.SourceType.Value : (short)TransactionSourceType.Purpose;
                origEntity.EntryDate = RepetativeInfo.NextDate;
                origEntity.XmlMemo = entity.TargetID != null ? new PFAID(entity.TargetID).ToString() : null;

                return origEntity;
            }
            else
            {
                origEntity = new RepetitiveTransaction();
                origEntity.Id = new PFAID().UID;
                origEntity.AccountId = new PFAID(entity.AccountID).UID;
                origEntity.Memo = entity.Description;
                origEntity.SourceId = string.IsNullOrEmpty(entity.SourceID) ? null : new PFAID(entity.SourceID).UID;
                origEntity.StoreId = string.IsNullOrEmpty(entity.StoreID) ? null : new PFAID(entity.StoreID).UID;
                origEntity.TransactionDate = entity.TransactionDate;
                origEntity.ReferenceNumber = ReferenceNumber;
                origEntity.EntryNumber = EntryNumber;
                origEntity.DebitCredit = (entity.Debit >= 0 && entity.Credit == 0) ? false : true;
                origEntity.Amount = (entity.Debit >= 0 && entity.Credit == 0) ? entity.Debit : entity.Credit;
                origEntity.Status = (short)Status.Active;
                origEntity.SourceType = (short)TransactionSourceType.Purpose;
                origEntity.RepetitiveTransactionInvoice = new RepetitiveTransactionInvoice();
                origEntity.RepetitiveTransactionInvoice.Id = origEntity.Id;
                origEntity.RepetitiveTransactionInvoice.Hrs = entity.Statistics;
                origEntity.RepetitiveTransactionInvoice.Rate = origEntity.Amount;
                origEntity.RepetitiveTransactionInvoice.Status = Convert.ToByte(Status.Active);

                origEntity.EntryDate = RepetativeInfo.NextDate;
                origEntity.TargetId = RepetativeInfo.Id;
                origEntity.XmlMemo = entity.TargetID != null ? new PFAID(entity.TargetID).ToString() : null;

                return origEntity;
            }
        }

        #endregion

        //OCR
        public static BillEntryInformation MapBillEntryInfo(JournalEntry entity, Transaction tran, byte[] OCRJournalId, byte[] userID, short? approvalType = null, BillEntryInformation? origEntity = null)
        {
            if (origEntity != null)
            {
                //origEntity.TransactionId = tran.Id;
                origEntity.Amount = tran.DebitCredit ? -1 * tran.Amount : tran.Amount;
                origEntity.Outstanding = tran.Amount;
                origEntity.SourceId = new PFAID(tran.TargetId).UID;
                origEntity.DebitCredit = tran.DebitCredit ? (short)TransactionDebitCredit.Debit : (short)TransactionDebitCredit.Credit;
                origEntity.EntryDate = entity.EntryDate;
                //origEntity.BillDate = entity.BillDate;
                //origEntity.ContractId = new PFAID(entity.ContractID).UID;
                origEntity.StoreId = new PFAID(tran.StoreId).UID;
                origEntity.CorporationId = new PFAID(entity.CorporationId).UID;
                origEntity.EntryNumber = entity.EntryNumber;
                origEntity.ModifiedBy = userID;
                origEntity.Status = (short)Status.InActive;
                //origEntity.PayMethodId = new PFAID(entity.PaymentMethodID).UID;
                origEntity.ModifiedDate = DateTime.Now;
                //origEntity.Memo = entity.;
                return origEntity;
            }
            else
            {
                return new BillEntryInformation()
                {
                    JournalEntryId = OCRJournalId,
                    Amount = tran.DebitCredit ? -1 * tran.Amount : tran.Amount,
                    Outstanding = tran.Amount,
                    SourceId = new PFAID(tran.TargetId).UID,
                    DebitCredit = tran.DebitCredit ? (short)TransactionDebitCredit.Debit : (short)TransactionDebitCredit.Credit,
                    TransactionId = tran.Id,
                    EntryDate = entity.EntryDate,
                    //BillDate = entity.BillDate,
                    CreditTermId = OCRJournalId,
                    RefType = (short)JournalSourceTypes.Journal,
                    Status = (short)Status.InActive,
                    //ContractId = new PFAID(entity.ContractID).UID,
                    StoreId = new PFAID(tran.StoreId).UID,
                    CorporationId = new PFAID(entity.CorporationId).UID,
                    EntryNumber = entity.EntryNumber,
                    CreatedBy = userID,
                    CreatedDate = DateTime.Now,
                    //ModifiedBy = userID,
                    //PayMethodId = new PFAID(entity.PaymentMethodID).UID,
                    //Memo = entity.Memo,
                    //ApprovalStatus = approvalType ?? (short)ApprovalStatus.Entry
                };
            }
        }
        #region Reccuring 
        public static Repetitive MapRepetitive(JournalEntryDTO entity, Repetitive? origEntity = null)
        {
            if (origEntity != null)
            {
                origEntity.Name = string.IsNullOrEmpty(entity.RecurringData.RecurringName) ? null : entity.RecurringData.RecurringName;
                origEntity.CorporationId = new PFAID(entity.CorpID).UID;
                origEntity.NextDate = entity.RecurringData.NextDate;
                origEntity.EndDate = entity.RecurringData.EndDate != default(DateTime) ? entity.RecurringData.EndDate : null;
                origEntity.FrequencyId = string.IsNullOrEmpty(entity.RecurringData.FrequencyID) ? null : new PFAID(entity.RecurringData.FrequencyID).UID;
                origEntity.RemindId = string.IsNullOrEmpty(entity.RecurringData.RemaindID) ? null : new PFAID(entity.RecurringData.RemaindID).UID;
                origEntity.RemindStatus = !string.IsNullOrEmpty(entity.RecurringData.RemaindID) ? true : false;
                origEntity.SourceId = string.IsNullOrEmpty(entity.RecurringData.SourceID) ? null : new PFAID(entity.RecurringData.SourceID).UID;
                origEntity.Email = !string.IsNullOrEmpty(entity.RecurringData.RecurringMail) ? entity.RecurringData.RecurringMail : null; 
                //origEntity.SendEmail = !string.IsNullOrEmpty(entity.RecurringData.RecurringMail) ? true : false;
                origEntity.Description = entity.RecurringData.Description;
                origEntity.IsAutomatic = entity.IsAutomatic;
                origEntity.ForMonthEndDate = entity.RecurringData.IsMonthEndDate;
                //Status = (short)Status.Active

                return origEntity;
            }
            else
            {
                return new Repetitive()
                {
                    Id = new PFAID().UID,
                    Name = string.IsNullOrEmpty(entity.RecurringData.RecurringName) ? null : entity.RecurringData.RecurringName,
                    SourceId = new PFAID().UID,
                    SourceType = (short)JournalSourceTypes.Journal,
                    CorporationId = new PFAID(entity.CorpID).UID,
                    NextDate = entity.RecurringData.NextDate,
                    EndDate = entity.RecurringData.EndDate != default(DateTime) ? entity.RecurringData.EndDate : null,
                    FrequencyId = string.IsNullOrEmpty(entity.RecurringData.FrequencyID) ? null : new PFAID(entity.RecurringData.FrequencyID).UID,
                    RemindId = (entity.RecurringData.IsRemindmeBefore == true && !string.IsNullOrEmpty(entity.RecurringData.RemaindID))? new PFAID(entity.RecurringData.RemaindID).UID:null,
                    RemindStatus = !string.IsNullOrEmpty(entity.RecurringData.RemaindID) ? true : false,
                    Email = !string.IsNullOrEmpty(entity.RecurringData.RecurringMail) ? entity.RecurringData.RecurringMail : null,
                    SendEmail = !string.IsNullOrEmpty(entity.RecurringData.RecurringMail) ? true : false,
                    Description = entity.RecurringData.Description,
                    ForMonthEndDate = entity.RecurringData.IsMonthEndDate,
                    IsAutomatic = entity.IsAutomatic,
                    Status = (short)Status.Active

                };
            }
        }

        /// <summary>
        /// Mapping JournalEntryRequest to Repetitive
        /// </summary>
        /// <param name="entity">entity represents JournalEntryRequest</param>
        /// <param name="origEntity">entity represents Repetitive</param>
        /// <returns>It returns Mapped Repetative</returns>
        public static Repetitive MapRepetitive(JournalEntryRequest entity, Repetitive? origEntity = null)
        {
            return MapRepetitive((JournalEntryDTO)entity, origEntity);
        }

        #endregion

        #endregion
    }
}
