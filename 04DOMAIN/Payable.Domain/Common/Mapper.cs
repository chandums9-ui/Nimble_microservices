using Common.Domain.Common;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Enums;
using Common.Domain.DTO.Model;
using Common.Domain.DTO.Model.Base;
using DailySales.Domain.DTO.Enums;
using DataModel.Domain.DataModel;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Payable.Domain.DTO.Enums;
using Payable.Domain.DTO.Model;
using Payable.Domain.DTO.Model.RepayModel.v1;
using Payable.Domain.DTO.Req;
using Payable.Domain.DTO.Resp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Azure.Core.HttpHeader;
using static Payable.Domain.DTO.Model.EpaymentModels;
using DailySales.Domain.DTO.Enums;
using System.Xml.Linq;
using System.Collections;
using System.Diagnostics.Contracts;
using Payable.Domain.DTO.Model.RepayModel.v1;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;
using static Payable.Domain.DTO.Req.Setup1099Req;

namespace Payable.Domain.Common
{
    /// <summary>
    /// Mapper class for core module
    /// </summary>
    public static class Mapper
    {
        #region BillEntry Mappers

        /// <summary>
        /// Mapping BillEntryRequest to JournalEntry
        /// </summary>
        /// <param name="entity">Entity represents BillEntryRequest Properties</param>
        /// <param name="origEntity">origEntity represents JournalEntry</param>
        /// <returns>It returns Mapped JournalEntry</returns>
        public static JournalEntry MapBillEntry(BillEntryRequest entity, JournalEntry origEntity = null)
        {
            if (origEntity != null)
            {
                origEntity.EntryDate = Convert.ToDateTime(entity.EntryDate);
                origEntity.EntryNumber = entity.EntryNumber;
                origEntity.ModifiedDate = DateTime.Now;
                origEntity.BillDate = entity.BillDate;
                origEntity.HoldPayment = entity.IsHoldPayment ? (short)1 : (short)0;
                return origEntity;
            }
            else
            {
                return new JournalEntry()
                {
                    Id = new PFAID().UID,
                    CorporationId = new PFAID(entity.CorpID).UID,
                    EntryDate = Convert.ToDateTime(entity.EntryDate),
                    EntryNumber = entity.EntryNumber,
                    IsMailSent = true,
                    Status = (short)Status.Active,
                    CreatedDate = DateTime.Now,
                    BillDate = entity.BillDate,
                    HoldPayment = entity.IsHoldPayment ? (short)1 : (short)0,
                };
            }
        }

        public static JournalEntry MapBillEntryJE(BillEntryDetails entity, JournalEntry origEntity = null)
        {
            if (origEntity != null)
            {
                origEntity.EntryDate = Convert.ToDateTime(entity.BooksDate.Date);
                origEntity.EntryNumber = entity.BillNumber;
                origEntity.ModifiedDate = DateTime.Now;
                origEntity.BillDate = entity.BillDate != null && entity.BillDate != DateTime.MinValue ? entity.BillDate : Convert.ToDateTime(entity.BooksDate);
                origEntity.HoldPayment = entity.IsHoldPayment ? (short)1 : (short)0;
                origEntity.PaymentMethodId = new PFAID(entity.PaymentMethodID).UID;
                return origEntity;
            }
            else
            {
                return new JournalEntry()
                {
                    
                    Id = !string.IsNullOrEmpty(entity.DocRefID) ? new PFAID(entity.DocRefID).UID:new PFAID().UID,
                    CorporationId = new PFAID(entity.CorpID).UID,
                    EntryDate = Convert.ToDateTime(entity.BooksDate),
                    EntryNumber = entity.BillNumber,
                    IsMailSent = true,
                    Status = (short)Status.Active,
                    CreatedDate = DateTime.Now,
                    BillDate = entity.BillDate != null && entity.BillDate != DateTime.MinValue ? entity.BillDate : Convert.ToDateTime(entity.BooksDate),
                    HoldPayment = entity.IsHoldPayment ? (short)1 : (short)0,
                    SourceType = entity.IsDebitMemo ? (short)JournalSourceTypes.DebitMemo : (short)JournalSourceTypes.Bill,
                    ParentSourceType = entity.IsDebitMemo ? (short)JournalSourceTypes.DebitMemo : (short)JournalSourceTypes.Bill,
                    PaymentMethodId = new PFAID(entity.PaymentMethodID).UID
                };
            }
        }

        public static JournalEntry MapBillPaymentJE(BillPaymentDetails entity, JournalEntry origEntity = null)
        {
            if (origEntity != null)
            {
                origEntity.EntryDate = Convert.ToDateTime(entity.Date);
                origEntity.ModifiedDate = DateTime.Now;
                origEntity.PaymentMethodId = new PFAID(entity.PaymentMethodID).UID;
                origEntity.IsUpdated = 1;
                origEntity.IsPrintRequired = entity.ToBePrinted;
                origEntity.EntryNumber = entity.RefNumber;
                return origEntity;
            }

            else
            {
                return new JournalEntry()
                {
                    Id = new PFAID().UID,
                    CorporationId = new PFAID(entity.CorpID).UID,
                    EntryDate = Convert.ToDateTime(entity.Date),
                    IsMailSent = true,
                    ReferenceMode = 2,//need to check enum
                    IsPrintRequired = entity.ToBePrinted,
                    EntryNumber = entity.RefNumber,
                    CreatedDate = DateTime.Now,
                    SourceType = (short?)JournalSourceTypes.PayBill,
                    ParentSourceType = (short?)JournalSourceTypes.PayBill,
                    PaymentMethodId = new PFAID(entity.PaymentMethodID).UID,
                    IsUpdated = 1
                };
            }
        }
        public static OcrjournalEntry MapBillEntryOCRJE(BillEntryDetails entity, short? userApprovalType = null, OcrjournalEntry origEntity = null)
        {
            if (origEntity != null)
            {
                origEntity.EntryDate = Convert.ToDateTime(entity.BooksDate);
                origEntity.EntryNumber = entity.BillNumber;
                origEntity.ModifiedDate = DateTime.Now;
                origEntity.BillDate = entity.BillDate;
                origEntity.PaymentMethodId = new PFAID(entity.PaymentMethodID).UID;
                return origEntity;
            }
            else
            {
                return new OcrjournalEntry()
                {
                    Id = new PFAID().UID,
                    CorporationId = new PFAID(entity.CorpID).UID,
                    EntryDate = Convert.ToDateTime(entity.BooksDate),
                    EntryNumber = entity.BillNumber,
                    Status = (short)Status.Active,
                    CreatedDate = DateTime.Now,
                    BillDate = entity.BillDate,
                    PaymentMethodId = new PFAID(entity.PaymentMethodID).UID,
                    ApprovalStatus = userApprovalType!= null ?  (short)userApprovalType.Value:(short)ApprovalStatus.Entry

                };
            }
        }

        /// <summary>
        /// Mapping (Vendor Details in) BillEntryRequest to Transaction
        /// </summary>
        /// <param name="entity">Entity represents BillEntryRequest Properties</param>
        /// <param name="JournalEntryId"></param>
        /// <param name="origEntity">origEntity represents Transaction</param>
        /// <returns>It Returns Mapped Venodor Transaction</returns>
        public static Transaction MapBillTransaction(BillEntryRequest entity, byte[] JournalEntryId, byte[] AccId, byte[] defStore, byte[] ContractId = null, Transaction origEntity = null)
        {
            if (origEntity != null)
            {
                origEntity.AccountId = AccId;
                origEntity.SourceId = new PFAID(entity.PayeeID).UID;
                origEntity.Amount = entity.Amount;
                origEntity.TransactionDate = DateTime.Now;
                origEntity.ReferenceNumber = !string.IsNullOrEmpty(entity.ReferenceNumber) ? entity.ReferenceNumber : null;
                origEntity.Memo = !string.IsNullOrEmpty(entity.Memo) ? entity.Memo : null;
                if (origEntity.TransactionInvoice != null)
                {
                    origEntity.TransactionInvoice.AccContractId = ContractId ?? null;
                }
                else if (ContractId != null)
                {
                    origEntity.TransactionInvoice = new TransactionInvoice();
                    origEntity.TransactionInvoice.AccContractId = ContractId ?? null;
                }
                return origEntity;
            }
            else
            {
                origEntity = new Transaction
                {
                    Id = new PFAID().UID,
                    AccountId = AccId,
                    JournalEntryId = JournalEntryId,
                    Amount = entity.Amount,
                    DebitCredit = Convert.ToBoolean(TransactionDebitCredit.Credit),
                    TransactionDate = DateTime.Now,
                    ReferenceNumber = !string.IsNullOrEmpty(entity.ReferenceNumber) ? entity.ReferenceNumber : null,
                    Memo = !string.IsNullOrEmpty(entity.Memo) ? entity.Memo : null,
                    Status = (short)Status.Active,
                    SourceId = new PFAID(entity.PayeeID).UID,
                    SourceType = (short)TransactionSourceType.Vendor,
                    StoreId = defStore,
                };
                if (ContractId != null && !entity.IsPreVoid)
                {
                    origEntity.TransactionInvoice = new TransactionInvoice();
                    origEntity.TransactionInvoice.Id = origEntity.Id;
                    origEntity.TransactionInvoice.AccContractId = ContractId ?? null;
                }
                return origEntity;
            }
        }

        public static Transaction MapBillEntryTransaction(BillEntryDetails entity, byte[] JournalEntryId, byte[] AccId, byte[] defStore, byte[] ContractId = null, Transaction origEntity = null)
        {
            if (origEntity != null)
            {
                origEntity.AccountId = AccId;
                origEntity.SourceId = new PFAID(entity.VenID).UID;
                origEntity.Amount = entity.Amount;
                origEntity.TransactionDate = DateTime.Now;
                origEntity.ReferenceNumber = !string.IsNullOrEmpty(entity.RefNumber) ? entity.RefNumber : null;
                origEntity.Memo = !string.IsNullOrEmpty(entity.Memo) ? entity.Memo : null;
               
                return origEntity;
            }
            else
            {
                origEntity = new Transaction
                {
                    Id = new PFAID().UID,
                    AccountId = AccId,
                    JournalEntryId = JournalEntryId,
                    Amount = entity.Amount,
                    DebitCredit = entity.IsDebitMemo ? Convert.ToBoolean(TransactionDebitCredit.Debit) : Convert.ToBoolean(TransactionDebitCredit.Credit),
                    TransactionDate = DateTime.Now,
                    ReferenceNumber = !string.IsNullOrEmpty(entity.RefNumber) ? entity.RefNumber : null,
                    Memo = !string.IsNullOrEmpty(entity.Memo) ? entity.Memo : null,
                    Status = (short)Status.Active,
                    SourceId = new PFAID(entity.VenID).UID,
                    SourceType = (short)TransactionSourceType.Vendor,
                    StoreId = defStore,
                };
                
                return origEntity;
            }
        }
        public static TransactionInvoice MapBillEntryTransactionInvoiceContract(BillEntryDetails entity, byte[] ContractId , Transaction origEntity,TransactionInvoice transInvoice=null)
        {

                if (transInvoice == null)
                {
                    transInvoice = new TransactionInvoice();
                    transInvoice.Id = origEntity.Id;
                }
                    transInvoice.AccContractId = ContractId ?? null;
                return transInvoice;
            
        }
        public static OcrtransactionInvoice MapBillEntryOCrTransactionInvoiceContract(BillEntryDetails entity, byte[] ContractId, Ocrtransaction origEntity, OcrtransactionInvoice transInvoice = null)
        {

            if (transInvoice == null)
            {
                transInvoice = new OcrtransactionInvoice();
                transInvoice.Id = origEntity.Id;
            }
            transInvoice.AccContractId = ContractId ?? null;
            return transInvoice;

        }
        public static Pctransactions MapPCTransaction(decimal amount, decimal outstanding, byte[] pcID, bool debitCredit, byte[] tID, byte[] refID = null)
        {
            Pctransactions pcTran = new Pctransactions();
            pcTran.TransactionId = tID;
            pcTran.RefId = refID;
            pcTran.Amount = amount;
            pcTran.Oustanding = outstanding;
            pcTran.Pcid = pcID;
            // pcTran.BillInformationId = billInfoID;
            pcTran.DebitCredit = debitCredit;
            return pcTran;
        }

        public static BillEntryInformationDetails MapBEIDetails(decimal amount, decimal outstanding, byte[] pcID)
        {
            BillEntryInformationDetails beid = new BillEntryInformationDetails();
            // beid.BillInfoId = billInfoID;
            beid.Amount = amount;
            beid.Outstanding = outstanding;
            beid.Pcid = pcID;
            return beid;
        }

        public static Transaction MapBillPaymentTransaction(BillPaymentDetails entity, byte[] JournalEntryId, byte[] defStore, Transaction origEntity = null)
        {
            if (origEntity != null)
            {
                origEntity.AccountId = new PFAID(entity.BankAccountID).UID;
                origEntity.SourceId = new PFAID(entity.BillPaymentDivisions[0].PayeeID).UID;
                origEntity.Amount = entity.BillPaymentDivisions.Where(x => x.IsDebitMemo == false).Sum(x => x.AmountPaid) - entity.BillPaymentDivisions.Where(x => x.IsDebitMemo == true).Sum(x => x.AmountPaid);
                origEntity.TransactionDate = DateTime.Now;
                origEntity.ReferenceNumber = entity.ToBePrinted ? "To be Printed" : entity.CheckNumber;
                origEntity.Memo = !string.IsNullOrEmpty(entity.Memo) ? entity.Memo : null;
                return origEntity;
            }
            else
            {
                origEntity = new Transaction
                {
                    Id = new PFAID().UID,
                    AccountId = new PFAID(entity.BankAccountID).UID,
                    JournalEntryId = JournalEntryId,
                    Amount = entity.BillPaymentDivisions.Where(x => x.IsDebitMemo == false).Sum(x => x.AmountPaid) - entity.BillPaymentDivisions.Where(x => x.IsDebitMemo == true).Sum(x => x.AmountPaid),
                    DebitCredit = Convert.ToBoolean(TransactionDebitCredit.Credit),
                    TransactionDate = DateTime.Now,
                    Memo = !string.IsNullOrEmpty(entity.Memo) ? entity.Memo : null,
                    Status = (short)Status.Active,
                    SourceId = new PFAID(entity.BillPaymentDivisions[0].PayeeID).UID,
                    SourceType = (short)TransactionSourceType.Vendor,
                    StoreId = defStore,
                    ReferenceNumber = entity.ToBePrinted ? "To be Printed" : entity.CheckNumber
                };
                return origEntity;
            }
        }

        //OCR
        public static Ocrtransaction MapBillEntryOCRTransaction(BillEntryDetails entity, byte[] OCRJournalEntryId, byte[] AccId, byte[] defStore, byte[] ContractId = null, Ocrtransaction origEntity = null)
        {
            if (origEntity != null)
            {
                origEntity.AccountId = AccId;
                origEntity.SourceId = new PFAID(entity.VenID).UID;
                origEntity.Amount = entity.Amount;
                origEntity.TransactionDate = DateTime.Now;
                origEntity.ReferenceNumber = !string.IsNullOrEmpty(entity.RefNumber) ? entity.RefNumber : null;
                origEntity.Memo = !string.IsNullOrEmpty(entity.Memo) ? entity.Memo : null;

              
                return origEntity;
            }
            else
            {
                origEntity = new Ocrtransaction
                {
                    Id = new PFAID().UID,
                    AccountId = AccId,
                    JournalEntryId = OCRJournalEntryId,
                    Amount = entity.Amount,
                    DebitCredit = entity.IsDebitMemo ? Convert.ToBoolean(TransactionDebitCredit.Debit) : Convert.ToBoolean(TransactionDebitCredit.Credit),
                    TransactionDate = DateTime.Now,
                    ReferenceNumber = !string.IsNullOrEmpty(entity.RefNumber) ? entity.RefNumber : null,
                    Memo = !string.IsNullOrEmpty(entity.Memo) ? entity.Memo : null,
                    Status = (short)Status.Active,
                    SourceId = new PFAID(entity.VenID).UID,
                    SourceType = (short)TransactionSourceType.Vendor,
                    StoreId = defStore,
                };
                if (ContractId != null && !entity.IsVoid)
                {
                    origEntity.OcrtransactionInvoice = new OcrtransactionInvoice();
                    origEntity.OcrtransactionInvoice.Id = origEntity.Id;
                    origEntity.OcrtransactionInvoice.AccContractId = ContractId ?? null;
                }
                return origEntity;
            }
        }

        /// <summary>
        /// Mapping (split details in )JournalDivision DTO to Transaction
        /// </summary>
        /// <param name="entity">Entity represents JournalDivisionDTO Properties</param>
        /// <param name="VendorTrans">VendorTrans Represents Vendor transaction</param>
        /// <param name="origEntity">origEntity represents Transaction</param>
        /// <returns>It Returns Mapped split Transactions</returns>
        public static Transaction MapSplitBillTransaction(JournalDivisionDTO entity, Transaction VendorTrans, byte[] defPc, byte[] defPurpose, Transaction origEntity = null)
        {
            if (origEntity != null)
            {
                origEntity.AccountId = new PFAID(entity.AccountID).UID;
                origEntity.Amount = entity.Debit;
                origEntity.TransactionDate = DateTime.Now;
                origEntity.ReferenceNumber = VendorTrans.ReferenceNumber ?? null;
                origEntity.Memo = entity.Description ?? null;
                origEntity.SourceId = !string.IsNullOrEmpty(entity.SourceID) ? new PFAID(entity.SourceID).UID : defPurpose;
                origEntity.StoreId = !string.IsNullOrEmpty(entity.StoreID) ? new PFAID(entity.StoreID).UID : defPc;
                origEntity.TargetId = !string.IsNullOrEmpty(entity.TargetID) ? new PFAID(entity.TargetID).UID : null;
                origEntity.TargetType = (short)TransactionSourceType.Customer;
                origEntity.TransactionInvoice.Rate = Convert.ToDecimal(entity.Debit);
                origEntity.TransactionInvoice.Hrs = entity.Statistics;
                return origEntity;
            }
            else
            {
                origEntity = new Transaction();
                origEntity.Id = new PFAID().UID;
                origEntity.AccountId = new PFAID(entity.AccountID).UID;
                origEntity.JournalEntryId = VendorTrans.JournalEntryId;
                origEntity.ParentId = VendorTrans.Id;
                origEntity.Amount = entity.Debit;
                origEntity.DebitCredit = Convert.ToBoolean(TransactionDebitCredit.Debit);
                origEntity.TransactionDate = DateTime.Now;
                origEntity.ReferenceNumber = VendorTrans.ReferenceNumber ?? null;
                origEntity.Memo = !string.IsNullOrEmpty(entity.Description) ? entity.Description : null;
                origEntity.Status = (short)Status.Active;
                origEntity.SourceId = !string.IsNullOrEmpty(entity.SourceID) ? new PFAID(entity.SourceID).UID : defPurpose;
                origEntity.SourceType = (short)TransactionSourceType.Purpose;
                origEntity.StoreId = !string.IsNullOrEmpty(entity.StoreID) ? new PFAID(entity.StoreID).UID : defPc;
                origEntity.TargetId = !string.IsNullOrEmpty(entity.TargetID) ? new PFAID(entity.TargetID).UID : null;
                origEntity.TargetType = (short)TransactionSourceType.Customer;
                origEntity.Billable = false;
                origEntity.TransactionInvoice = new TransactionInvoice();
                origEntity.TransactionInvoice.Id = origEntity.Id;
                origEntity.TransactionInvoice.Rate = Convert.ToDecimal(entity.Debit);
                origEntity.TransactionInvoice.Status = Convert.ToByte(Status.Active);
                origEntity.TransactionInvoice.Hrs = entity.Statistics;
                return origEntity;
            }
        }
        public static Transaction MapSplitBillEntryTransaction(bool isDebitMemo, TransactionDivisionDetails entity, Transaction VendorTrans, byte[] defPc, byte[] defPurpose, Transaction origEntity = null, TransactionInvoice originalInvoice = null)
        {

            bool? debitCredit = null;
            if (isDebitMemo)
            {
                debitCredit = entity.Amount > 0 ? Convert.ToBoolean(TransactionDebitCredit.Credit) : Convert.ToBoolean(TransactionDebitCredit.Debit);
            }
            else
            {
                debitCredit = entity.Amount > 0 ? Convert.ToBoolean(TransactionDebitCredit.Debit) : Convert.ToBoolean(TransactionDebitCredit.Credit);
            }

            if (origEntity != null)
            {
                origEntity.AccountId = new PFAID(entity.AccountID).UID;
                origEntity.Amount = Math.Abs(entity.Amount);
                origEntity.TransactionDate = DateTime.Now;
                origEntity.ReferenceNumber = VendorTrans.ReferenceNumber ?? null;
                origEntity.Memo = entity.Description ?? null;
                origEntity.SourceId = !string.IsNullOrEmpty(entity.PurposeID) ? new PFAID(entity.PurposeID).UID : defPurpose;
                origEntity.StoreId = !string.IsNullOrEmpty(entity.StoreID) ? new PFAID(entity.StoreID).UID : defPc;
                origEntity.TargetType = (short)TransactionSourceType.Customer;
                origEntity.DebitCredit = debitCredit.Value;

    
            }
            else
            {
                origEntity = new Transaction
                {
                    Id = new PFAID().UID,
                    AccountId = new PFAID(entity.AccountID).UID,
                    JournalEntryId = VendorTrans.JournalEntryId,
                    ParentId = VendorTrans.Id,
                    Amount = Math.Abs(entity.Amount),
                    DebitCredit = debitCredit.Value,
                    TransactionDate = DateTime.Now,
                    ReferenceNumber = VendorTrans.ReferenceNumber ?? null,
                    Memo = !string.IsNullOrEmpty(entity.Description) ? entity.Description : null,
                    Status = (short)Status.Active,
                    SourceId = !string.IsNullOrEmpty(entity.PurposeID) ? new PFAID(entity.PurposeID).UID : defPurpose,
                    SourceType = (short)TransactionSourceType.Purpose,
                    StoreId = !string.IsNullOrEmpty(entity.StoreID) ? new PFAID(entity.StoreID).UID : defPc,
                    TargetType =null,
                    Billable = false,

                };

               
            }
            if (originalInvoice == null)
                originalInvoice = new TransactionInvoice();
            originalInvoice.Rate = Convert.ToDecimal(entity.Amount);
            originalInvoice.Hrs = entity.Statistics;
            origEntity.TransactionInvoice = originalInvoice;
            return origEntity;
        }
        public static TransactionInvoice MapSplitBillEntryTransactionInvoice(TransactionDivisionDetails entity,Transaction origEntity, TransactionInvoice originalInvoice = null)
        {
            if (originalInvoice == null)
            {
                originalInvoice = new TransactionInvoice();
                originalInvoice.Id = origEntity.Id;
            }
            originalInvoice.Rate = Convert.ToDecimal(entity.Amount);
            originalInvoice.Hrs = entity.Statistics;
            return originalInvoice;
        }

        public static Transaction MapSplitBillPaymentPaidTransaction(BillPaymentDetails entity, Transaction VendorTrans, byte[] accID, byte[] defPc, Transaction origEntity = null)
        {
            if (origEntity != null)
            {
                origEntity.AccountId = accID;
                origEntity.Amount = (entity.BillPaymentDivisions.Where(x => x.IsDebitMemo == false).Sum(x => x.AmountPaid) - entity.BillPaymentDivisions.Where(x => x.IsDebitMemo == true).Sum(x => x.AmountPaid)) +
                     (entity.BillPaymentDivisions.Where(x => x.IsDebitMemo == false).Sum(x => x.DiscountAmount));
                origEntity.TransactionDate = DateTime.Now;
                origEntity.ReferenceNumber = VendorTrans.ReferenceNumber ?? null;
                origEntity.SourceId = new PFAID(entity.JEID).UID;
                origEntity.StoreId = !string.IsNullOrEmpty(entity.BillPaymentDivisions.FirstOrDefault().StoreID) ? new PFAID(entity.BillPaymentDivisions.FirstOrDefault().StoreID).UID : defPc;
                return origEntity;
            }
            else
            {
                origEntity = new Transaction();
                origEntity.Id = new PFAID().UID;
                origEntity.AccountId = accID;
                origEntity.JournalEntryId = VendorTrans.JournalEntryId;
                origEntity.ParentId = VendorTrans.Id;
                //TO Verify : Discount amount & Debit Memo's are coming with minus sign 
                origEntity.Amount = (entity.BillPaymentDivisions.Where(x => x.IsDebitMemo == false).Sum(x => x.AmountPaid) - entity.BillPaymentDivisions.Where(x => x.IsDebitMemo == true).Sum(x => x.AmountPaid)) +
                     (entity.BillPaymentDivisions.Where(x => x.IsDebitMemo == false).Sum(x => x.DiscountAmount));
                origEntity.DebitCredit = Convert.ToBoolean(TransactionDebitCredit.Debit);
                origEntity.TransactionDate = DateTime.Now;
                origEntity.ReferenceNumber = VendorTrans.ReferenceNumber ?? null;
                origEntity.SourceId = new PFAID(entity.JEID).UID;//BillEntryJE
                origEntity.SourceType = (short)JournalSourceTypes.Bill;
                origEntity.StoreId = !string.IsNullOrEmpty(entity.BillPaymentDivisions.FirstOrDefault()?.StoreID) ? new PFAID(entity.BillPaymentDivisions.FirstOrDefault().StoreID).UID : defPc;
                origEntity.Status = (short)Status.Active;
                return origEntity;
            }
        }
        public static Transaction MapSplitBillPaymentDiscountTransaction(BillPaymentDetails entity, Transaction VendorTrans, string checkno, byte[] defPc, Transaction origEntity = null)
        {
            var discAccount = entity.BillPaymentDivisions.Where(x => x.DiscountAccount != null).Select(x => x.DiscountAccount).FirstOrDefault();
            if (origEntity != null)
            {
                origEntity.AccountId = new PFAID(discAccount).UID;
                origEntity.Amount = entity.BillPaymentDivisions.Where(x => x.IsDebitMemo == false).Sum(x => x.DiscountAmount);
                origEntity.TransactionDate = DateTime.Now;
                origEntity.ReferenceNumber = VendorTrans.ReferenceNumber;
                origEntity.SourceId = new PFAID(entity.JEID).UID;
                //origEntity.StoreId = !string.IsNullOrEmpty(entity.StoreID) ? new PFAID(entity.StoreID).UID : defPc;               
                return origEntity;
            }
            else
            {
                origEntity = new Transaction();
                origEntity.Id = new PFAID().UID;
                origEntity.AccountId = new PFAID(discAccount).UID;
                origEntity.JournalEntryId = VendorTrans.JournalEntryId;
                origEntity.ParentId = VendorTrans.Id;
                origEntity.Amount = entity.BillPaymentDivisions.Where(x => x.IsDebitMemo == false).Sum(x => x.DiscountAmount);
                origEntity.DebitCredit = Convert.ToBoolean(TransactionDebitCredit.Credit);
                origEntity.TransactionDate = DateTime.Now;
                origEntity.ReferenceNumber = checkno;
                origEntity.Status = (short)Status.Active;
                origEntity.SourceId = new PFAID(entity.JEID).UID;
                origEntity.SourceType = (short)TransactionSourceType.Discount;
                //origEntity.StoreId = !string.IsNullOrEmpty(entity.StoreID) ? new PFAID(entity.StoreID).UID : defPc;
                return origEntity;
            }
        }

        public static BillEntryPayments MapSplitBillEntryPaymentTransaction(BillPaymentDivisions entity, Transaction VendorTrans, Transaction paidTran, Transaction discountTran, BillEntryInformation bei, byte[] pcid, BillEntryPayments? origEntity = null)
        {
            if (origEntity != null)
            {
                origEntity.PaidAmount = entity.AmountPaid;
                origEntity.OutStanding = entity.AmountDue - entity.AmountPaid - entity.DiscountAmount;
                origEntity.AdjustmentAmount = 0;
                origEntity.Status = origEntity.OutStanding == 0 ? (short)TransactionStatus.Completed : (short)TransactionStatus.Active;
                if (!entity.IsDebitMemo)
                {
                    origEntity.DiscountAccountId = new PFAID(entity.DiscountAccount).UID;
                    origEntity.Discount = entity.DiscountAmount;
                }
            }
            else
            {
                origEntity = new BillEntryPayments();
                origEntity.TransactioonId = paidTran.Id;
                origEntity.SourceId = VendorTrans.SourceId;
                origEntity.BillInformationId = bei.Id;
                origEntity.BillPaymentId = VendorTrans.JournalEntryId;
                origEntity.PaidAmount = entity.AmountPaid;
                //TODO : Outstanding bill entry information outstanding
                origEntity.OutStanding = entity.AmountDue - entity.AmountPaid - entity.DiscountAmount;
                origEntity.AdjustmentAmount = 0;
                origEntity.Status = origEntity.OutStanding == 0 ? (short)TransactionStatus.Completed : (short)TransactionStatus.Active;
                origEntity.Pcid = pcid;
                if (!entity.IsDebitMemo)
                {
                    origEntity.Discount = entity.DiscountAmount;
                    origEntity.DiscountAccountId = new PFAID(entity.DiscountAccount).UID;
                    origEntity.DiscountTransactionId = discountTran == null ? null : discountTran.Id;
                }
            }
            return origEntity;
        }

        public static BillPaymentsInformation MapBillPaymentInformation(BillPaymentDetails entity, Transaction VendorTrans, long billInfoID, byte[] accID, byte[] defPc, BillPaymentsInformation? origEntity = null)
        {
            if (origEntity != null)
            {
                origEntity.AccountId = new PFAID(entity.BankAccountID).UID;
                origEntity.Amount = entity.BillPaymentDivisions.Where(x => x.IsDebitMemo == false).Sum(x => x.AmountPaid) -
                                    entity.BillPaymentDivisions.Where(x => x.IsDebitMemo == true).Sum(x => x.AmountPaid);
                origEntity.Memo = entity.Memo;
                origEntity.CheckNo = entity.ToBePrinted ? "To be Printed" : entity.CheckNumber;
                origEntity.StoreId = defPc;
                return origEntity;
            }
            else
            {
                origEntity = new BillPaymentsInformation();
                origEntity.AccountId = new PFAID(entity.BankAccountID).UID;
                origEntity.JournalEntryId = VendorTrans.JournalEntryId;
                origEntity.Amount = entity.BillPaymentDivisions.Where(x => x.IsDebitMemo == false).Sum(x => x.AmountPaid) -
                                   entity.BillPaymentDivisions.Where(x => x.IsDebitMemo == true).Sum(x => x.AmountPaid);
                origEntity.SourceId = VendorTrans.SourceId;
                origEntity.StoreId = defPc;
                origEntity.Memo = entity.Memo;
                origEntity.TransactionId = VendorTrans.Id;
                origEntity.CheckNo = entity.ToBePrinted ? "To be Printed" : entity.CheckNumber;
                //origEntity.BillInformationId = billInfoID;
            }
            return origEntity;
        }

        public static BillPayOrInvoiceAdjust MapBillPayOrInvoiceAdjust(BillPaymentDetails entity, Transaction VendorTrans, BillPayOrInvoiceAdjust? origEntity = null)
        {
            if (origEntity != null)
            {
                //origEntity.AccountId = accID;
                //origEntity.Amount = entity.DiscountAmount;
                //origEntity.TransactionDate = DateTime.Now;
                //origEntity.ReferenceNumber = VendorTrans.ReferenceNumber ?? null;
                //origEntity.SourceId = new PFAID(entity.JEID).UID;
                //origEntity.StoreId = !string.IsNullOrEmpty(entity.StoreID) ? new PFAID(entity.StoreID).UID : defPc;
                //return origEntity;
            }
            else
            {
                origEntity = new BillPayOrInvoiceAdjust();
                origEntity.Amount = entity.BillPaymentDivisions[0].AmountPaid;
                origEntity.SourceId = VendorTrans.JournalEntryId;
                origEntity.TargetId = new PFAID(entity.BillPaymentDivisions[0].JEID).UID;
                origEntity.TargetType = (short?)JournalSourceTypes.PayBill;
                origEntity.TransactionId = VendorTrans.Id;
                origEntity.Status = (short)Status.Active;
                origEntity.AdjustmentDate = DateTime.Now;
            }
            return origEntity;
        }

        //OCR
        public static Ocrtransaction MapSplitBillEntryOCRTransaction(TransactionDivisionDetails entity, Ocrtransaction VendorTrans, byte[] defPc, byte[] defPurpose, Ocrtransaction origEntity = null)
        {
            bool? debitCredit = null;
            if (VendorTrans.DebitCredit == Convert.ToBoolean(TransactionDebitCredit.Debit))
            {
                debitCredit = entity.Amount > 0 ? Convert.ToBoolean(TransactionDebitCredit.Credit) : Convert.ToBoolean(TransactionDebitCredit.Debit);
            }
            else
            {
                debitCredit = entity.Amount > 0 ? Convert.ToBoolean(TransactionDebitCredit.Debit) : Convert.ToBoolean(TransactionDebitCredit.Credit);
            }

            if (origEntity != null)
            {
                origEntity.AccountId = new PFAID(entity.AccountID).UID;
                origEntity.Amount = Math.Abs(entity.Amount);
                origEntity.TransactionDate = DateTime.Now;
                origEntity.ReferenceNumber = VendorTrans.ReferenceNumber ?? null;
                origEntity.Memo = entity.Description ?? null;
                origEntity.DebitCredit = debitCredit;
                origEntity.SourceId = !string.IsNullOrEmpty(entity.PurposeID) ? new PFAID(entity.PurposeID).UID : defPurpose;
                origEntity.StoreId = !string.IsNullOrEmpty(entity.StoreID) ? new PFAID(entity.StoreID).UID : defPc;
                return origEntity;
            }
            else
            {
                origEntity = new Ocrtransaction();
                origEntity.Id = new PFAID().UID;
                origEntity.AccountId = new PFAID(entity.AccountID).UID;
                origEntity.JournalEntryId = VendorTrans.JournalEntryId;
                origEntity.ParentId = VendorTrans.Id;
                origEntity.Amount = Math.Abs(entity.Amount);
                origEntity.DebitCredit = debitCredit;
                origEntity.TransactionDate = DateTime.Now;
                origEntity.ReferenceNumber = VendorTrans.ReferenceNumber ?? null;
                origEntity.Memo = !string.IsNullOrEmpty(entity.Description) ? entity.Description : null;
                origEntity.Status = (short)Status.Active;
                origEntity.SourceId = !string.IsNullOrEmpty(entity.PurposeID) ? new PFAID(entity.PurposeID).UID : defPurpose;
                origEntity.SourceType = (short)TransactionSourceType.Purpose;
                origEntity.StoreId = !string.IsNullOrEmpty(entity.StoreID) ? new PFAID(entity.StoreID).UID : defPc;
                return origEntity;
            }
        }
        public static OcrtransactionInvoice MapSplitBillEntryOcrTransactionInvoice(TransactionDivisionDetails entity, Ocrtransaction origEntity, OcrtransactionInvoice originalInvoice = null)
        {
            if (originalInvoice == null)
            {
                originalInvoice = new OcrtransactionInvoice();
                originalInvoice.Id = origEntity.Id;
            }
            originalInvoice.Rate = Convert.ToDecimal(entity.Amount);
            originalInvoice.Hrs = entity.Statistics;
            return originalInvoice;
        }

        /// <summary>
        /// Mapping BillEntryRequest data to CreditTerm
        /// </summary>
        /// <param name="entity">Entity represents BillEntryRequest Properties</param>
        /// <param name="JournalEntryId"></param>
        /// <param name="origEntity">origEntity represents CreditTerm</param>
        /// <returns>It Returns Mapped CreditTerm</returns>
        public static CreditTerm MapCreditTerm(BillEntryRequest entity, byte[] JournalEntryId, CreditTerm origEntity = null)
        {
            if (origEntity != null)
            {
                origEntity.CreditDays = !string.IsNullOrEmpty(entity.CreditDaysID) ? new PFAID(entity.CreditDaysID).UID : null;
                origEntity.DueDate = entity.DueDate;
                return origEntity;
            }
            else
            {
                return new CreditTerm()
                {
                    Id = JournalEntryId,
                    CreditDays = !string.IsNullOrEmpty(entity.CreditDaysID) ? new PFAID(entity.CreditDaysID).UID : null,
                    DueDate = entity.DueDate,
                };
            }
        }
        public static CreditTerm MapBillEntryCreditTerm(BillEntryDetails entity, byte[] JournalEntryId, CreditTerm? origEntity = null)
        {
            if (origEntity != null)
            {
                origEntity.CreditDays = !string.IsNullOrEmpty(entity.CreditDaysID) ? new PFAID(entity.CreditDaysID).UID : null;
                if (entity.DueDate != null)
                    origEntity.DueDate = (DateTime)entity.DueDate;
                return origEntity;
            }
            else
            {
                var ct = new CreditTerm();
                ct.Id = JournalEntryId;
                ct.CreditDays = !string.IsNullOrEmpty(entity.CreditDaysID) ? new PFAID(entity.CreditDaysID).UID : null;
                if (entity.DueDate != null)
                    ct.DueDate = (DateTime)entity.DueDate;
                return ct;
            }
        }
        //OCR
        public static OcrcreditTerm MapBillEntryOCRCreditTerm(BillEntryDetails entity, byte[] OCRJournalEntryId, OcrcreditTerm? origEntity = null)
        {
            if (origEntity != null)
            {
                origEntity.CreditDays = !string.IsNullOrEmpty(entity.CreditDaysID) ? new PFAID(entity.CreditDaysID).UID : null;
                if (entity.DueDate != null)
                    origEntity.DueDate = (DateTime)entity.DueDate;
                return origEntity;
            }
            else
            {
                var ocrCT = new OcrcreditTerm();
                ocrCT.Id = OCRJournalEntryId;
                ocrCT.CreditDays = !string.IsNullOrEmpty(entity.CreditDaysID) ? new PFAID(entity.CreditDaysID).UID : null;
                if (entity.DueDate != null)
                    ocrCT.DueDate = (DateTime)entity.DueDate;
                return ocrCT;
            }
        }

        /// <summary>
        /// Mapping BillEntryRequest data to BilEntryInformation
        /// </summary>
        /// <param name="entity">Entity represents BillEntryRequest Properties</param>
        /// <param name="tran">tran represents Vendor Transaction</param>
        /// <param name="JournalId"></param>
        /// <param name="origEntity">origEntity represents BillEntryInformation</param>
        /// <returns>It Returns Mapped BillEntryInformation</returns>
        public static BillEntryInformation MapBillEntryInformation(BillEntryRequest entity, Transaction tran, byte[] JournalId, BillEntryInformation? origEntity = null)
        {
            if (origEntity != null)
            {
                origEntity.Amount = entity.Amount;
                origEntity.Outstanding = entity.Amount;
                origEntity.SourceId = new PFAID(entity.PayeeID).UID;
                origEntity.EntryDate = entity.EntryDate;
                return origEntity;
            }
            else
            {
                return new BillEntryInformation()
                {
                    JournalEntryId = JournalId,
                    Amount = entity.Amount,
                    Outstanding = entity.Amount,
                    SourceId = new PFAID(entity.PayeeID).UID,
                    DebitCredit = (short)TransactionDebitCredit.Credit,
                    Status = (short)Status.Active,
                    TransactionId = tran.Id,
                    EntryDate = entity.EntryDate,
                    CreditTermId = JournalId,
                    StoreId = tran.StoreId,
                };
            }
        }

        public static BillEntryInformation MapBillEntryInfo(BillEntryDetails entity, Transaction tran, byte[] JournalId, byte[] userID, BillEntryInformation? origEntity = null)
        {

            if (origEntity != null)
            {
                origEntity.Outstanding = origEntity.Amount * (entity.IsDebitMemo ? -1 : 1) == origEntity.Outstanding ?
                                            entity.Amount :
                                            origEntity.Outstanding + (entity.Amount - origEntity.Amount * (entity.IsDebitMemo ? -1 : 1));
                origEntity.Amount = entity.IsDebitMemo ? -1 * entity.Amount : entity.Amount;
                origEntity.SourceId = new PFAID(entity.VenID).UID;
                origEntity.EntryDate = entity.BooksDate.Date;
                origEntity.BillDate = entity.BillDate != null ? entity.BillDate.Value.Date : entity.BillDate;
                origEntity.CreditTermId = new PFAID(entity.CreditDaysID).UID;
                origEntity.ContractId = string.IsNullOrEmpty(entity.ContractID) ? null : new PFAID(entity.ContractID).UID;
                origEntity.DebitCredit = entity.IsDebitMemo ? (short)TransactionDebitCredit.Debit : (short)TransactionDebitCredit.Credit;
                origEntity.RefType = entity.IsDebitMemo ? (short)JournalSourceTypes.DebitMemo : (short)JournalSourceTypes.Bill;
                origEntity.CorporationId = new PFAID(entity.CorpID).UID;
                origEntity.EntryNumber = entity.BillNumber;
                origEntity.ModifiedBy = userID;
                origEntity.PayMethodId = new PFAID(entity.PaymentMethodID).UID;
                origEntity.VoidDate =  entity.VoidDate.Date;
                origEntity.ModifiedDate = DateTime.Now;
                origEntity.Memo = entity.Memo;
                origEntity.TransactionId=tran.Id;
                if (origEntity.Outstanding > 0 && !entity.IsVoid) origEntity.Status = (short)Status.Active;
                return origEntity;
            }
            else
            {
                return new BillEntryInformation()
                {
                    JournalEntryId = JournalId,
                    Amount = entity.IsDebitMemo ? -1 * entity.Amount : entity.Amount,
                    Outstanding = entity.Amount,
                    SourceId = new PFAID(entity.VenID).UID,
                    DebitCredit = entity.IsDebitMemo ? (short)TransactionDebitCredit.Debit : (short)TransactionDebitCredit.Credit,
                    Status = entity.Amount == 0 ? (short)TransactionStatus.Completed : (short)Status.Active,
                    TransactionId = tran.Id,
                    EntryDate = entity.BooksDate.Date,
                    BillDate = entity.BillDate != null ? entity.BillDate.Value.Date : entity.BillDate,
                    CreditTermId = new PFAID(entity.CreditDaysID).UID,
                    RefType = entity.IsDebitMemo ? (short)JournalSourceTypes.DebitMemo : (short)JournalSourceTypes.Bill,
                    ContractId = string.IsNullOrEmpty(entity.ContractID)?null:new PFAID(entity.ContractID).UID,
                    CorporationId = new PFAID(entity.CorpID).UID,
                    EntryNumber = entity.BillNumber,
                    CreatedBy = userID,
                    PayMethodId = new PFAID(entity.PaymentMethodID).UID,
                    CreatedDate = DateTime.Now,
                    Memo = entity.Memo,
                    EntryType = entity.BillEntryType
                };
            }
        }

        //OCR
        public static BillEntryInformation MapBillEntryOCRInfo(BillEntryDetails entity, Ocrtransaction tran, byte[] OCRJournalId, byte[] userID, short? approvalType = null, BillEntryInformation? origEntity = null)
        {
            if (origEntity != null)
            {
                origEntity.Amount = entity.IsDebitMemo ? -1 * entity.Amount : entity.Amount;
                origEntity.Outstanding = entity.Amount;
                origEntity.SourceId = new PFAID(entity.VenID).UID;
                origEntity.DebitCredit = entity.IsDebitMemo ? (short)TransactionDebitCredit.Debit : (short)TransactionDebitCredit.Credit;
                origEntity.EntryDate = entity.BooksDate;
                origEntity.BillDate = entity.BillDate;
                origEntity.ContractId = string.IsNullOrEmpty(entity.ContractID)?null: new PFAID(entity.ContractID).UID;
                origEntity.CorporationId = new PFAID(entity.CorpID).UID;
                origEntity.EntryNumber = entity.BillNumber;
                origEntity.ModifiedBy = userID;
                origEntity.TransactionId= tran.Id;  
                origEntity.Status = (short)Status.InActive;
                origEntity.PayMethodId = new PFAID(entity.PaymentMethodID).UID;
                origEntity.ModifiedDate = DateTime.Now;
                origEntity.Memo = entity.Memo;
                return origEntity;
            }
            else
            {
                return new BillEntryInformation()
                {
                    JournalEntryId = OCRJournalId,
                    Amount = entity.IsDebitMemo ? -1 * entity.Amount : entity.Amount,
                    Outstanding = entity.Amount,
                    SourceId = new PFAID(entity.VenID).UID,
                    DebitCredit = entity.IsDebitMemo ? (short)TransactionDebitCredit.Debit : (short)TransactionDebitCredit.Credit,
                    TransactionId = tran.Id,
                    EntryDate = entity.BooksDate,
                    BillDate = entity.BillDate,
                    CreditTermId = OCRJournalId,
                    RefType = entity.IsDebitMemo ? (short)JournalSourceTypes.DebitMemoApproval : (short)JournalSourceTypes.BillImportOrApproval,
                    Status = (short)Status.InActive,
                    ContractId = new PFAID(entity.ContractID).UID,
                    CorporationId = new PFAID(entity.CorpID).UID,
                    EntryNumber = entity.BillNumber,
                    CreatedBy = userID,
                    PayMethodId = new PFAID(entity.PaymentMethodID).UID,
                    CreatedDate = DateTime.Now,
                    Memo = entity.Memo,
                    ApprovalStatus = approvalType ?? (short)ApprovalStatus.Entry,
                    EntryType= entity.BillEntryType
                };
            }
        }

        /// <summary>
        /// Mapping BillEntryRequest data to ApprovalComments
        /// </summary>
        /// <param name="entity">Entity represents BillEntryRequest Properties</param>
        /// <param name="JournalId"></param>
        /// <param name="UserId"></param>
        /// <returns>It Returns Mapped Approval Comments</returns>
        public static ApprovalComments MapApprovalComments(BillEntryRequest entity, byte[] JournalId)
        {
            return new ApprovalComments()
            {
                JournalEntryId = JournalId,
                Comment = entity.Comment,
                CommentDate = DateTime.Now,
            };
        }

        /// <summary>
        /// Mapping BillEntryRequest to JournalEntryExt
        /// </summary>
        /// <param name="JournalEntryId"></param>
        /// <param name="VoidReason"></param>
        /// <param name="origEntity">origEntity represents JournalEntryExt</param>
        /// <returns>It returns mapped JournalEntryExt</returns>
        public static JournalEntryExt MapJournalEntryExt(byte[] JournalEntryId, string ChkMemo = null, string VoidReason = null, JournalEntryExt origEntity = null)
        {
            if (origEntity != null)
            {
                origEntity.VoidRemarks = VoidReason ?? null;
                origEntity.CheckMemo = ChkMemo ?? null;
                return origEntity;
            }
            else
            {
                return new JournalEntryExt
                {
                    JournalEntryId = JournalEntryId,
                    VoidRemarks = VoidReason ?? null,
                    CheckMemo = ChkMemo,
                };
            }
        }

        #region Repetitive

        /// <summary>
        /// Mapping BillEntryRequest data to Repetitive
        /// </summary>
        /// <param name="entity">Entity represents BillEntryRequest Properties</param>
        /// <returns>It returns Mapped Repetitve</returns>
        public static Repetitive MapRepetitive(short EntryType, BillEntryRequest entity = null, Repetitive OrigEntity = null)
        {
            if (OrigEntity != null)
            {
                OrigEntity.SourceType = EntryType;
                OrigEntity.FrequencyId = new PFAID(entity.RecurringData.FrequencyID).UID;
                OrigEntity.RemindId = !string.IsNullOrEmpty(entity.RecurringData.RemaindID) ? new PFAID(entity.RecurringData.RemaindID).UID : null;
                OrigEntity.RemindStatus = !string.IsNullOrEmpty(entity.RecurringData.RemaindID) ? true : false;
                OrigEntity.ForMonthEndDate = entity.RecurringData.IsMonthEndDate;

            }
            else
            {
                return new Repetitive()
                {
                    Id = new PFAID().UID,
                    SourceType = EntryType,
                    SourceId = new PFAID().UID,
                    Name = entity.RecurringData.RecurringName,
                    FrequencyId = new PFAID(entity.RecurringData.FrequencyID).UID,
                    RemindId = !string.IsNullOrEmpty(entity.RecurringData.RemaindID) ? new PFAID(entity.RecurringData.RemaindID).UID : null,
                    RemindStatus = !string.IsNullOrEmpty(entity.RecurringData.RemaindID) ? true : false,
                    NextDate = entity.RecurringData.NextDate,
                    EndDate = entity.RecurringData.EndDate != default(DateTime) ? entity.RecurringData.EndDate : null,
                    ForMonthEndDate = entity.RecurringData.IsMonthEndDate,
                    SendEmail = true,
                    IsAutomatic = entity.IsAutomatic ? true : false,
                    Status = (short)Status.Active,
                    CorporationId = new PFAID(entity.CorpID).UID,
                    Email = entity.RecurringData.RecurringMail != null ? entity.RecurringData.RecurringMail.ToString() : null,
                };
            }
            return OrigEntity;
        }

        public static Repetitive MapRepetitive(BillEntryDetails entity, Repetitive OrigEntity = null)
        {
            if (OrigEntity != null)
            {
                OrigEntity.Name = entity.RecurringInfo.RecurringName;
                OrigEntity.FrequencyId = string.IsNullOrEmpty(entity.RecurringInfo.FrequencyID) ? OrigEntity.FrequencyId : new PFAID(entity.RecurringInfo.FrequencyID).UID;
                OrigEntity.RemindId = !string.IsNullOrEmpty(entity.RecurringInfo.RemaindID) ? new PFAID(entity.RecurringInfo.RemaindID).UID : null;
                OrigEntity.RemindStatus = !string.IsNullOrEmpty(entity.RecurringInfo.RemaindID) ? true : false;
                OrigEntity.NextDate = entity.RecurringInfo.NextDate;
                OrigEntity.EndDate = entity.RecurringInfo.EndDate != default(DateTime) ? entity.RecurringInfo.EndDate : null;
                OrigEntity.ForMonthEndDate = entity.RecurringInfo.IsMonthEndDate;
                OrigEntity.IsAutomatic = entity.RecurringInfo.IsAutomatic ? true : false;
                OrigEntity.Status = (short)Status.Active;
                OrigEntity.CorporationId = new PFAID(entity.CorpID).UID;
                OrigEntity.Email = entity.RecurringInfo.RecurringMail != null ? entity.RecurringInfo.RecurringMail.ToString() : null;
                OrigEntity.PaymentMethodId = !string.IsNullOrEmpty(entity.PaymentMethodID) ? new PFAID(entity.PaymentMethodID).UID : null;
                OrigEntity.ContractId = string.IsNullOrEmpty(entity.ContractID) ? null : new PFAID(entity.ContractID).UID;

            }
            else
            {

                return new Repetitive()
                {

                    Id = new PFAID().UID,
                    SourceType = entity.IsDebitMemo ? (short)JournalSourceTypes.DebitMemo : (short)JournalSourceTypes.Bill,
                    SourceId = new PFAID().UID,
                    Name = entity.RecurringInfo.RecurringName,
                    FrequencyId = new PFAID(entity.RecurringInfo.FrequencyID).UID,
                    RemindId = !string.IsNullOrEmpty(entity.RecurringInfo.RemaindID) ? new PFAID(entity.RecurringInfo.RemaindID).UID : null,
                    RemindStatus = !string.IsNullOrEmpty(entity.RecurringInfo.RemaindID) ? true : false,
                    NextDate = entity.RecurringInfo.NextDate,
                    EndDate = entity.RecurringInfo.EndDate != default(DateTime) ? entity.RecurringInfo.EndDate : null,
                    ForMonthEndDate = entity.RecurringInfo.IsMonthEndDate,
                    SendEmail = true,
                    IsAutomatic = entity.RecurringInfo.IsAutomatic ? true : false,
                    Status = (short)Status.Active,
                    CorporationId = new PFAID(entity.CorpID).UID,
                    Email = entity.RecurringInfo.RecurringMail != null ? entity.RecurringInfo.RecurringMail.ToString() : null,
                    PaymentMethodId = !string.IsNullOrEmpty(entity.PaymentMethodID) ? new PFAID(entity.PaymentMethodID).UID : null,
                    ContractId = string.IsNullOrEmpty(entity.ContractID) ? null : new PFAID(entity.ContractID).UID
                };
            }
            return OrigEntity;
        }

        /// <summary>
        /// Mapping (Vendor details in) BillEntryRequest data to RepetitiveTransaction
        /// </summary>
        /// <param name="entity">Entity represents BillEntryRequest Properties</param>
        /// <param name="RepId"></param>
        /// <param name="origEntity">origEntity represents RepetitiveTransaction</param>
        /// <returns>It returns mapped Vendor RepetitiveTransaction</returns>
        public static RepetitiveTransaction MapRepetitiveTransaction(BillEntryRequest entity, byte[] RepId, byte[] AccId, byte[] defStore)
        {
            RepetitiveTransaction origEntity = new RepetitiveTransaction
            {
                Id = new PFAID().UID,
                AccountId = AccId,
                TargetId = RepId,
                Amount = entity.Amount,
                DebitCredit = Convert.ToBoolean(TransactionDebitCredit.Credit),
                TransactionDate = DateTime.Now,
                ReferenceNumber = !string.IsNullOrEmpty(entity.ReferenceNumber) ? entity.ReferenceNumber : null,
                Memo = !string.IsNullOrEmpty(entity.Memo) ? entity.Memo : null,
                Status = (short)Status.Active,
                SourceId = new PFAID(entity.PayeeID).UID,
                SourceType = (short)TransactionSourceType.Vendor,
                EntryDate = entity.RecurringData.NextDate,
                EntryNumber = entity.EntryNumber,
                StoreId = defStore
            };
            return origEntity;
        }

        public static RepetitiveTransaction MapBillEntryRepetitiveTransaction(BillEntryDetails entity, byte[] RepId, byte[] AccId,DateTime NextDate, byte[] defStore = null, RepetitiveTransaction OrigEntity = null)
        {
            if (OrigEntity != null)
            {
                OrigEntity.AccountId = AccId != null ? AccId : OrigEntity.AccountId;
                OrigEntity.Amount = entity.Amount;
                OrigEntity.DebitCredit = Convert.ToBoolean(TransactionDebitCredit.Credit);
                OrigEntity.ReferenceNumber = !string.IsNullOrEmpty(entity.RefNumber) ? entity.RefNumber : null;
                OrigEntity.Memo = !string.IsNullOrEmpty(entity.Memo) ? entity.Memo : null;
                OrigEntity.Status = (short)Status.Active;
                OrigEntity.SourceId = new PFAID(entity.VenID).UID;
                OrigEntity.SourceType = (short)TransactionSourceType.Vendor;
                OrigEntity.EntryDate = NextDate;
                OrigEntity.EntryNumber = entity.BillNumber;
            }
            else
            {
                return new RepetitiveTransaction
                {
                    Id = new PFAID().UID,
                    AccountId = AccId,
                    TargetId = RepId,
                    Amount = entity.Amount,
                    DebitCredit = Convert.ToBoolean(TransactionDebitCredit.Credit),
                    TransactionDate = DateTime.Now,
                    ReferenceNumber = !string.IsNullOrEmpty(entity.RefNumber) ? entity.RefNumber : null,
                    Memo = !string.IsNullOrEmpty(entity.Memo) ? entity.Memo : null,
                    Status = (short)Status.Active,
                    SourceId = new PFAID(entity.VenID).UID,
                    SourceType = (short)TransactionSourceType.Vendor,
                    EntryDate = NextDate,
                    EntryNumber = entity.BillNumber,
                    StoreId = defStore
                };
            }
            return OrigEntity;
        }

        /// <summary>
        /// Mapping (split details in) JournalDivisionDTO data to RepetitiveTransaction
        /// </summary>
        /// <param name="entity">Entity represents JournalDivisionDTO Properties</param>
        /// <param name="RepId"></param>
        /// <param name="VendorTransaction">represents Vendor repetitive entry</param>
        /// <param name="origEntity">origEntity represents RepetitiveTransaction</param>
        /// <returns>It returns mapped split RepetitiveTransactions</returns>
        public static RepetitiveTransaction MapSplitRepetitiveTransaction(JournalDivisionDTO entity, byte[] RepId, byte[] defPurpose, RepetitiveTransaction VendorTransaction)
        {
            RepetitiveTransaction origEntity = new RepetitiveTransaction();
            origEntity.Id = new PFAID().UID;
            origEntity.AccountId = new PFAID(entity.AccountID).UID;
            origEntity.TargetId = RepId;
            origEntity.ParentId = VendorTransaction.Id;
            origEntity.Amount = entity.Debit;
            origEntity.DebitCredit = Convert.ToBoolean(TransactionDebitCredit.Debit);
            origEntity.TransactionDate = DateTime.Now;
            origEntity.ReferenceNumber = VendorTransaction.ReferenceNumber ?? null;
            origEntity.Memo = !string.IsNullOrEmpty(entity.Description) ? entity.Description : null;
            origEntity.EntryDate = VendorTransaction.EntryDate;
            origEntity.Status = (short)Status.Active;
            origEntity.SourceId = !string.IsNullOrEmpty(entity.SourceID) ? new PFAID(entity.SourceID).UID : defPurpose;
            origEntity.SourceType = (short)TransactionSourceType.Purpose;
            origEntity.StoreId = !string.IsNullOrEmpty(entity.StoreID) ? new PFAID(entity.StoreID).UID : VendorTransaction.StoreId;
            origEntity.RepetitiveTransactionInvoice = new RepetitiveTransactionInvoice();
            origEntity.RepetitiveTransactionInvoice.Id = origEntity.Id;
            origEntity.RepetitiveTransactionInvoice.Rate = origEntity.Amount;
            origEntity.RepetitiveTransactionInvoice.Hrs = entity.Statistics;
            return origEntity;
        }

        public static RepetitiveTransaction MapBillEntrySplitRepetitiveTransaction(bool isDebitMemo, TransactionDivisionDetails entity, byte[] RepId, byte[] defPurpose = null, RepetitiveTransaction VendorTransaction = null, RepetitiveTransaction OrigEntity = null)
        {
            if (OrigEntity != null)
            {
                if (isDebitMemo)
                {
                    OrigEntity.DebitCredit = entity.Amount > 0 ? Convert.ToBoolean(TransactionDebitCredit.Credit) : Convert.ToBoolean(TransactionDebitCredit.Debit);
                }
                else
                {
                    OrigEntity.DebitCredit = entity.Amount > 0 ? Convert.ToBoolean(TransactionDebitCredit.Debit) : Convert.ToBoolean(TransactionDebitCredit.Credit);
                }
                OrigEntity.AccountId = new PFAID(entity.AccountID).UID;
                OrigEntity.TargetId = RepId;
                OrigEntity.Amount = Math.Abs(entity.Amount);
                OrigEntity.ReferenceNumber = VendorTransaction.ReferenceNumber ?? null;
                OrigEntity.Memo = !string.IsNullOrEmpty(entity.Description) ? entity.Description : null;
                OrigEntity.EntryDate = VendorTransaction.EntryDate;
                OrigEntity.Status = (short)Status.Active;
                OrigEntity.SourceId = !string.IsNullOrEmpty(entity.PurposeID) ? new PFAID(entity.PurposeID).UID : defPurpose;
                OrigEntity.SourceType = (short)TransactionSourceType.Purpose;
                OrigEntity.RepetitiveTransactionInvoice = new RepetitiveTransactionInvoice();
                OrigEntity.RepetitiveTransactionInvoice.Id = OrigEntity.Id;
                OrigEntity.RepetitiveTransactionInvoice.Rate = OrigEntity.Amount;
                OrigEntity.RepetitiveTransactionInvoice.Hrs = entity.Statistics;

            }
            else
            {
                OrigEntity = new RepetitiveTransaction();
                OrigEntity.Id = new PFAID().UID;
                OrigEntity.AccountId = new PFAID(entity.AccountID).UID;
                OrigEntity.TargetId = RepId;
                OrigEntity.ParentId = VendorTransaction.Id;
                OrigEntity.Amount = Math.Abs(entity.Amount);
                OrigEntity.TransactionDate = DateTime.Now;
                OrigEntity.ReferenceNumber = VendorTransaction.ReferenceNumber ?? null;
                OrigEntity.Memo = !string.IsNullOrEmpty(entity.Description) ? entity.Description : null;
                OrigEntity.EntryDate = VendorTransaction.EntryDate;
                OrigEntity.Status = (short)Status.Active;
                OrigEntity.SourceId = !string.IsNullOrEmpty(entity.PurposeID) ? new PFAID(entity.PurposeID).UID : defPurpose;
                OrigEntity.SourceType = (short)TransactionSourceType.Purpose;
                OrigEntity.StoreId = !string.IsNullOrEmpty(entity.StoreID) ? new PFAID(entity.StoreID).UID : VendorTransaction.StoreId;
                OrigEntity.RepetitiveTransactionInvoice = new RepetitiveTransactionInvoice();
                OrigEntity.RepetitiveTransactionInvoice.Id = OrigEntity.Id;
                OrigEntity.RepetitiveTransactionInvoice.Rate = OrigEntity.Amount;
                OrigEntity.RepetitiveTransactionInvoice.Hrs = entity.Statistics;
                if (isDebitMemo)
                {
                    OrigEntity.DebitCredit = entity.Amount > 0 ? Convert.ToBoolean(TransactionDebitCredit.Credit) : Convert.ToBoolean(TransactionDebitCredit.Debit);
                }
                else
                {
                    OrigEntity.DebitCredit = entity.Amount > 0 ? Convert.ToBoolean(TransactionDebitCredit.Debit) : Convert.ToBoolean(TransactionDebitCredit.Credit);
                }

            }
            return OrigEntity;
        }

        /// <summary>
        /// Mapping repetitive Id and creditdays to RepetitiveCreditTerm
        /// </summary>
        /// <param name="repId"></param>
        /// <param name="crdaysId"></param>
        /// <param name="conId"></param>
        /// <returns>It returns mapped RepetitiveCreditTerm</returns>
        public static RepetitiveCreditTerm MapRepetitiveCreditTerm(byte[] repId, string crdaysId, byte[] conId = null, RepetitiveCreditTerm OrigEntity = null)
        {
            if (OrigEntity != null)
            {
                OrigEntity.CreditDays = !string.IsNullOrEmpty(crdaysId) ? new PFAID(crdaysId).UID : OrigEntity.CreditDays;
                OrigEntity.AccContractId = conId != null ? conId : null;
            }
            else
            {
                return new RepetitiveCreditTerm()
                {
                    Id = repId,
                    CreditDays = !string.IsNullOrEmpty(crdaysId) ? new PFAID(crdaysId).UID : null,
                    AccContractId = conId ?? null,
                };
            }
            return OrigEntity;
        }

        #endregion
        #endregion BillEntry Mappers


        #region DebitMemo Mappers

        #endregion DebitMemo Mappers

        #region VendorMaster Mappers

        /// <summary>
        /// Maps the vendoradress data to Address 
        /// </summary>
        /// <param name="Entity">represents vendoradress</param>
        /// <param name="OrigEntity">represents adsress</param>
        /// <returns>It returns mapped Address data object</returns>
        public static Address MapAddress(VendorAdress Entity, Address OrigEntity = null)
        {
            if (OrigEntity != null)
            {
                OrigEntity.Address1 = Entity.Adress ?? null;
                OrigEntity.Address2 = Entity.AddressName ?? null;
                OrigEntity.Country = Entity.CountryID;
                OrigEntity.ZipCode = !string.IsNullOrEmpty(Entity.ZipCode) ? Entity.ZipCode : null;
                OrigEntity.City = Entity.City ?? null;
                OrigEntity.State = !string.IsNullOrEmpty(Entity.StateID) ? Convert.ToInt64(Entity.StateID) : null;
            }
            else
            {
                OrigEntity = new Address();
                OrigEntity.Id = new PFAID().UID;
                OrigEntity.Address1 = Entity.Adress ?? null;
                OrigEntity.Address2 = Entity.AddressName ?? null;
                OrigEntity.Country = Entity.CountryID;
                OrigEntity.ZipCode = Entity.ZipCode ?? null;
                OrigEntity.City = Entity.City ?? null;
                OrigEntity.State = !string.IsNullOrEmpty(Entity.StateID) ? Convert.ToInt64(Entity.StateID) : null;
            }
            return OrigEntity;
        }

        /// <summary>
        /// Maps the Vendoradress contact details to contact
        /// </summary>
        /// <param name="Entity">represents vendoradress</param>
        /// <param name="OrigEntity">represents address</param>
        /// <returns>It returns mapped Contact data object</returns>
        public static Contact MapContact(VendorAdress Entity, Contact OrigEntity = null)
        {
            if (OrigEntity != null)
            {
                OrigEntity.Phone1 = Entity.MobileNum ?? null;
                OrigEntity.Phone2 = Entity.AlternativeNum ?? null;
                OrigEntity.Phone3 = Entity.WorkNum ?? null;
                OrigEntity.AlternateNum = Entity.AlternativeNum ?? null;
                OrigEntity.Fax = Entity.FaxNum ?? null;
                OrigEntity.Email = Entity.EmailID ?? null;
                OrigEntity.Website = Entity.Website ?? null;
            }
            else
            {
                return new Contact()
                {
                    Id = new PFAID().UID,
                    Phone1 = Entity.MobileNum ?? null,
                    Phone2 = Entity.AlternativeNum ?? null,
                    Phone3 = Entity.WorkNum ?? null,
                    AlternateNum = Entity.AlternativeNum ?? null,
                    Fax = Entity.FaxNum ?? null,
                    Email = Entity.EmailID ?? null,
                    Website = Entity.Website ?? null
                };
            }
            return OrigEntity;
        }

        /// <summary>
        /// Maps the vendor details to business 
        /// </summary>
        /// <param name="Entity">represents details to be saved</param>
        /// <param name="AccountID"></param>
        /// <param name="UserID"></param>
        /// <param name="OrigEntity"></param>
        /// <returns>It returns mapped Business data object</returns>
        public static Business MapBusiness(BusinessDTO Entity, byte[] UserID, byte[] AccountID = null, Business OrigEntity = null)
        {
            if (OrigEntity != null)
            {
                OrigEntity.Name = Entity.Name;
                OrigEntity.CompanyName = string.IsNullOrEmpty(Entity.CompanyName) ? null : Entity.CompanyName;
                OrigEntity.Status = Entity.VendorStatus == true ? (short)Status.Active : (short)Status.InActive;
                OrigEntity.ModifiedBy = new PFAID(UserID).UID;
                OrigEntity.AccountId = AccountID == null ? OrigEntity.AccountId : AccountID;
                OrigEntity.ModifiedDateBy = DateTime.Now;
            }
            else
            {
                OrigEntity = new Business
                {
                    Id = new PFAID().UID,
                    Type = (short)TransactionSourceType.Vendor,
                    Name = Entity.Name,
                    CompanyName = string.IsNullOrEmpty(Entity.CompanyName) ? null : Entity.CompanyName,
                    CorporationId = new PFAID(Entity.CorporationID).UID,
                    AccountId = AccountID,
                    Status = (short)Status.Active,
                    CreatedBy = new PFAID(UserID).UID,
                    CreatedDateBy = DateTime.Now
                };
            }
            return OrigEntity;
        }

        /// <summary>
        /// Maps the vendor details to businessinfo
        /// </summary>
        /// <param name="Entity">represents details to be saved</param>
        /// <param name="BusinessID"></param>
        /// <param name="OrigEntity"></param>
        /// <returns>It returns mapped BusinessInfo data object</returns>
        public static BusinessInfo MapBusinessInfo(BusinessInfoDTO Entity, byte[] BusinessID, MiscInfo DefPayMethod = null, BusinessInfo OrigEntity = null)
        {
            if (OrigEntity != null)
            {
                OrigEntity.FederalId = string.IsNullOrEmpty(Entity.FederalID) ? null : Entity.FederalID;
                OrigEntity.PrintCheckAs = string.IsNullOrEmpty(Entity.PrintCheckAs) ? null : Entity.PrintCheckAs;
                OrigEntity.DoingBusinessAs = string.IsNullOrEmpty(Entity.DoingBusinessAs) ? null : Entity.DoingBusinessAs;
                OrigEntity.Salutation = Convert.ToByte(Entity.Salutation);
                OrigEntity.FirstName = string.IsNullOrEmpty(Entity.FirstName) ? null : Entity.FirstName;
                OrigEntity.MiddleName = string.IsNullOrEmpty(Entity.MiddleName) ? null : Entity.MiddleName;
                OrigEntity.LastName = string.IsNullOrEmpty(Entity.LastName) ? null : Entity.LastName;
                OrigEntity.CreditDaysId = string.IsNullOrEmpty(Entity.CreditDaysID) ? null : new PFAID(Entity.CreditDaysID).UID;
                OrigEntity.SendMethodId = string.IsNullOrEmpty(Entity.SendMethodID) ? null : new PFAID(Entity.SendMethodID).UID;
                OrigEntity.PaymentMethodId = string.IsNullOrEmpty(Entity.PaymentMethodID) ? DefPayMethod.Id : new PFAID(Entity.PaymentMethodID).UID;
                OrigEntity.CreditCardNo = string.IsNullOrEmpty(Entity.CreditCarNo) ? null : Entity.CreditCarNo;
                OrigEntity.CreditCardExpiryDate = string.IsNullOrEmpty(Entity.CreditCardExpiryDate) ? null : Entity.CreditCardExpiryDate;
                OrigEntity.NameOnCreditCard = string.IsNullOrEmpty(Entity.NameonCredit) ? null : Entity.NameonCredit;
                OrigEntity.Notes = string.IsNullOrEmpty(Entity.Notes) ? null : Entity.Notes;
                OrigEntity.Is1099 = Entity.Is1099;
                OrigEntity.Print1099As = string.IsNullOrEmpty(Entity.Print1099As) ? null : Entity.Print1099As;
                OrigEntity.IsInterCompany = Entity.IsInterCompany;
                OrigEntity.Ssn = string.IsNullOrEmpty(Entity.SSN) ? null : Entity.SSN;
                OrigEntity.DefaultAccount = string.IsNullOrEmpty(Entity.DefaultAccount) ? null : new PFAID(Entity.DefaultAccount).UID;
                OrigEntity.CheckTemplate = Entity.CheckTemplate;
                OrigEntity.PrintCheck = Entity.PrintCheck;
                OrigEntity.ToBePrinted = Entity.ToBePrinted;
                OrigEntity.IsAutoBill = Entity.IsAutoBill;
                OrigEntity.PostDays = Entity.IsAutoBill ? Convert.ToByte(Entity.PostDays) : null;
                OrigEntity.PrintChkCon = Entity.PrintCheckonContract;
                OrigEntity.UseTaxId = Entity.UseTaxID > 0 ? Entity.UseTaxID : null;
            }
            else
            {
                OrigEntity = new BusinessInfo
                {
                    Id = BusinessID,
                    FederalId = string.IsNullOrEmpty(Entity.FederalID) ? null : Entity.FederalID,
                    PrintCheckAs = string.IsNullOrEmpty(Entity.PrintCheckAs) ? null : Entity.PrintCheckAs,
                    Print1099As = string.IsNullOrEmpty(Entity.Print1099As) ? null : Entity.Print1099As,
                    DoingBusinessAs = string.IsNullOrEmpty(Entity.DoingBusinessAs) ? null : Entity.DoingBusinessAs,
                    Salutation = Convert.ToByte(Entity.Salutation),
                    FirstName = string.IsNullOrEmpty(Entity.FirstName) ? null : Entity.FirstName,
                    MiddleName = string.IsNullOrEmpty(Entity.MiddleName) ? null : Entity.MiddleName,
                    LastName = string.IsNullOrEmpty(Entity.LastName) ? null : Entity.LastName,
                    CreditDaysId = string.IsNullOrEmpty(Entity.CreditDaysID) ? null : new PFAID(Entity.CreditDaysID).UID,
                    SendMethodId = string.IsNullOrEmpty(Entity.SendMethodID) ? null : new PFAID(Entity.SendMethodID).UID,
                    PaymentMethodId = string.IsNullOrEmpty(Entity.PaymentMethodID) ? DefPayMethod.Id : new PFAID(Entity.PaymentMethodID).UID,
                    CreditCardNo = string.IsNullOrEmpty(Entity.CreditCarNo) ? null : Entity.CreditCarNo,
                    CreditCardExpiryDate = string.IsNullOrEmpty(Entity.CreditCardExpiryDate) ? null : Entity.CreditCardExpiryDate,
                    NameOnCreditCard = string.IsNullOrEmpty(Entity.NameonCredit) ? null : Entity.NameonCredit,
                    Notes = string.IsNullOrEmpty(Entity.Notes) ? null : Entity.Notes,
                    Is1099 = Entity.Is1099,
                    IsInterCompany = Entity.IsInterCompany,
                    Ssn = string.IsNullOrEmpty(Entity.SSN) ? null : Entity.SSN,
                    DefaultAccount = string.IsNullOrEmpty(Entity.DefaultAccount) ? null : new PFAID(Entity.DefaultAccount).UID,
                    CheckTemplate = Entity.CheckTemplate,
                    PrintCheck = Entity.PrintCheck,
                    ToBePrinted = Entity.ToBePrinted,
                    IsAutoBill = Entity.IsAutoBill,
                    PostDays = Convert.ToByte(Entity.PostDays),
                    PrintChkCon = Entity.PrintCheckonContract,
                    UseTaxId = Entity.UseTaxID > 0 ? Entity.UseTaxID : null
                };
            }
            return OrigEntity;
        }

        /// <summary>
        /// It maps the directdepositdetails to VendorDirectDepositDetails
        /// </summary>
        /// <param name="Entity">represents details to be saved</param>
        /// <param name="BusinessID"></param>
        /// <param name="OrigEntity"></param>
        /// <returns>It returns mapped VendorDirectDepositDetails data object</returns>
        public static VendorDirectDepositDetails MapDDDetails(VendorDirectDepositDTO Entity, byte[] BusinessID, VendorDirectDepositDetails? OrigEntity = null)
        {
            if (OrigEntity != null)
            {

                OrigEntity.DdcurrencyLoc = Convert.ToByte(Entity.DDCurrencyLOC);
                OrigEntity.DdbranchNo = string.IsNullOrEmpty(Entity.DDBranchNumber) ? null : Entity.DDBranchNumber;
                OrigEntity.DdinstitutionNo = string.IsNullOrEmpty(Entity.DDInstituteNo) ? null : Entity.DDInstituteNo;
                OrigEntity.DdaccountNo = string.IsNullOrEmpty(Entity.DDAccountNo) ? null : Entity.DDAccountNo;
                OrigEntity.CustomEftref = string.IsNullOrEmpty(Entity.CustomEFTRef) ? null : Entity.CustomEFTRef;
                OrigEntity.PayerUniqueNo = string.IsNullOrEmpty(Entity.PayerUniqueNo) ? null : Entity.PayerUniqueNo;
            }

            else
            {
                OrigEntity = new VendorDirectDepositDetails
                {
                    Id = BusinessID,
                    DdcurrencyLoc = Convert.ToByte(Entity.DDCurrencyLOC),
                    DdbranchNo = string.IsNullOrEmpty(Entity.DDBranchNumber) ? null : Entity.DDBranchNumber,
                    DdinstitutionNo = string.IsNullOrEmpty(Entity.DDInstituteNo) ? null : Entity.DDInstituteNo,
                    DdaccountNo = string.IsNullOrEmpty(Entity.DDAccountNo) ? null : Entity.DDAccountNo,
                    CustomEftref = string.IsNullOrEmpty(Entity.CustomEFTRef) ? null : Entity.CustomEFTRef,
                    PayerUniqueNo = string.IsNullOrEmpty(Entity.PayerUniqueNo) ? null : Entity.PayerUniqueNo
                };
            }
            return OrigEntity;
        }
        //for Vendor Import Address Maping
        public static List<VendorAddressSaveRequest> MapVendorSaveAddressRequest(List<VendorImportDTO> VendorDetails, VendorSaveRequest VendorSaveReq, List<State> States, CountryLoadResponse Countries, MiscInfoLoadResponse BusinessTypes)
        {
            List<string> uniqueAdds = VendorDetails.Select(x => x.Address.Trim()).Distinct().ToList();
            VendorSaveReq.VendorAdress = new List<VendorAddressSaveRequest>();
            foreach (var item1 in uniqueAdds)
            {
                var item = VendorDetails.FirstOrDefault(x => x.Address == item1);
                var CountryID = Convert.ToInt64(Countries.CountryList.FirstOrDefault(x => x.Name.ToLower() == item.CountryName.ToLower())?.ID);
                var StateId = string.IsNullOrEmpty(item.StateName) ? null : States.FirstOrDefault(x => x.Name.ToLower() == item.StateName.ToLower() && x.CountryId == CountryID)?.Id.ToString();
                VendorSaveReq.VendorAdress.Add(new VendorAddressSaveRequest
                {
                    Adress = string.IsNullOrEmpty(item.Address) ? null : item.Address,
                    City = string.IsNullOrEmpty(item.City) ? null : item.City,
                    ZipCode = string.IsNullOrEmpty(item.ZipCode) ? null : item.ZipCode,
                    MobileNum = string.IsNullOrEmpty(item.MobileNum) ? null : item.MobileNum,
                    AlternativeNum = string.IsNullOrEmpty(item.AlternativeNum) ? null : item.AlternativeNum,
                    FaxNum = string.IsNullOrEmpty(item.FaxNum) ? null : item.FaxNum,
                    EmailID = string.IsNullOrEmpty(item.EmailID) ? null : item.EmailID,
                    StateID = string.IsNullOrEmpty(StateId) ? null : StateId,
                    BusinessTypeID = string.IsNullOrEmpty(item.BusinessType) ? null : BusinessTypes.MiscInfoList.FirstOrDefault(x => x.Name.ToLower() == item.BusinessType.ToLower())?.ID,
                    WorkNum = string.IsNullOrEmpty(item.WorkNum) ? null : item.WorkNum,
                    CountryID = CountryID,
                    StateName = string.IsNullOrEmpty(item.StateName) || string.IsNullOrEmpty(StateId) ? null : item.StateName,
                    CountryName = string.IsNullOrEmpty(item.CountryName) && CountryID ==0  ? null : item.CountryName,
                    AddressString = (string.IsNullOrEmpty(item.Address) ? string.Empty : item.Address + ",")
                                                      + (string.IsNullOrEmpty(item.City) ? string.Empty : item.City + ",")
                                                      + (string.IsNullOrEmpty(item.StateName) ? string.Empty : item.StateName + ",")
                                                     + (string.IsNullOrEmpty(item.ZipCode) ? string.Empty : item.ZipCode + ",")
                                                     + (string.IsNullOrEmpty(item.CountryName) ? string.Empty : item.CountryName)
                });
            }
            return VendorSaveReq.VendorAdress;
        }
        //For Vendor Import Contract Mapping
        public static VendorContractSaveRequest MapVendorSaveContractRequest(VendorImportDTO vendor, List<byte[]> taxlineaccs)
        {
            VendorContractSaveRequest vendorContract = new VendorContractSaveRequest();
            vendorContract.AccountNumber = vendor.AccountNumber;
            vendorContract.ContractStartDate = string.IsNullOrEmpty(vendor.ContractStartDate) ? null : Convert.ToDateTime(vendor.ContractStartDate);
            vendorContract.ContractExpiryDate = string.IsNullOrEmpty(vendor.ContractexpirationDate) ? null : Convert.ToDateTime(vendor.ContractexpirationDate);
            vendorContract.ContractAddress = (string.IsNullOrEmpty(vendor.Address) ? string.Empty : vendor.Address + ",")
                                                      + (string.IsNullOrEmpty(vendor.City) ? string.Empty : vendor.City + ",")
                                                      + (string.IsNullOrEmpty(vendor.StateName) ? string.Empty : vendor.StateName + ",")
                                                     + (string.IsNullOrEmpty(vendor.ZipCode) ? string.Empty : vendor.ZipCode + ",")
                                                     + (string.IsNullOrEmpty(vendor.CountryName) ? string.Empty : vendor.CountryName);
            vendorContract.Notes = vendor.VendorNotes ?? string.Empty;
            //vendorContract.ContractAddress = vendor.ContractAddress;
            //vendorContract.IsDefault = true;
            foreach (var lineacc in taxlineaccs)
            {
                if (lineacc != null)
                {
                    vendorContract.VendorTaxInfo.Add(new VendorTaxInfoDTO
                    {
                        NimbleAccountID = new PFAID(lineacc).ToString(),
                    });
                }
            }
            return vendorContract;
        }

        #endregion

        #region EPayments Mappers
        public static RepayConfig MapRepayConfig(Repayconfig Entity, RepayConfig OrigEntity = null)
        {
            if (OrigEntity != null)
            {
                OrigEntity.ClientId = Entity.ClientId;
                OrigEntity.ClientSecretId = Entity.ClientSecretId;
                OrigEntity.Status = Convert.ToByte(Status.Active);
            }
            else
            {
                OrigEntity = new RepayConfig
                {
                    CorporationId = new PFAID(Entity.CorporationId).UID,
                    AccountId = new PFAID(Entity.PaymentAccountId).UID,
                    ClientId = Entity.ClientId,
                    ClientSecretId = Entity.ClientSecretId,
                    Status = Convert.ToByte(Status.Active)
                };
            }
            return OrigEntity;
        }
        public static RepayAuditInfo MapRepayAuditInfo(Repayconfig Entity, MasterLongAuditing longaudit)
        {
            return new RepayAuditInfo
            {
                Audit = longaudit,
                AccountId = new PFAID(Entity.PaymentAccountId).UID,
                ClientId = Entity.ClientId,
                ClientSecret = Entity.ClientSecretId,
                UpdateColumns = string.IsNullOrEmpty(Entity.UpdatedColumnIds) ? null : Entity.UpdatedColumnIds
            };
        }
        public static MasterLongAuditing MapMasterLongAuditing(long EntityId, string UserId, short ActionType, short ProviderType)
        {
            return new MasterLongAuditing
            {
                EntityId = EntityId,
                EntityType = ProviderType == (short)EpayProviderTypesEnum.Repay ? (short)ProviderEntityTypeEnum.Repay : (short)ProviderEntityTypeEnum.EFT,
                ActionDate = DateTime.Now,
                ActionType = ActionType,
                ActionBy = new PFAID(UserId).UID
            };
        }
        public static MasterBinAuditing MapMasterBinAuditing(string EntityId, string UserId, short ActionType, short ProviderType)
        {
            return new MasterBinAuditing
            {
                EntityId = new PFAID(EntityId).UID,
                EntityType = ProviderType == (short)ProviderEntityTypeEnum.vendor ? (short)ProviderEntityTypeEnum.vendor : (short)ProviderEntityTypeEnum.DirectDeposit,
                ActionDate = DateTime.Now,
                ActionType = ActionType,
                ActionBy = new PFAID(UserId).UID
            };
        }
        public static VendorAudit MapVendorAudit(MasterBinAuditing binAudit, VendorDirectDepositDetails ddDetails, string businessName, BusinessInfo businessInfo, short? IsActive, string Columns = null)
        {
            return new VendorAudit
            {
                Audit = binAudit,
                AuditId = binAudit.Id,
                Name = businessName,
                FederalId = businessInfo != null ? (string.IsNullOrEmpty(businessInfo.FederalId) ? null : businessInfo.FederalId) : null,
                Ssn = businessInfo != null ? (string.IsNullOrEmpty(businessInfo.Ssn) ? null : businessInfo.Ssn) : null,
                PrintCheckAs = businessInfo != null ? (string.IsNullOrEmpty(businessInfo.PrintCheckAs) ? null : businessInfo.PrintCheckAs) : null,
                IsAutoBill = businessInfo != null ? (businessInfo.IsAutoBill == null ? false : businessInfo.IsAutoBill == true ? true : false) : false,
                Terms = businessInfo != null ? (businessInfo.CreditDaysId != null ? businessInfo.CreditDaysId : null) : null,
                PaymentMethodId = businessInfo != null ? (businessInfo.PaymentMethodId != null ? businessInfo.PaymentMethodId : null) : null,
                DdbranchNo = ddDetails != null ? (!string.IsNullOrEmpty(ddDetails.DdbranchNo) ? ddDetails.DdbranchNo : null) : null,
                DdinstitutionNo = ddDetails != null ? (!string.IsNullOrEmpty(ddDetails.DdinstitutionNo) ? ddDetails.DdinstitutionNo : null) : null,
                DdaccountNo = ddDetails != null ? (!string.IsNullOrEmpty(ddDetails.DdaccountNo) ? ddDetails.DdaccountNo : null) : null,
                UpdatedColumns = string.IsNullOrEmpty(Columns) ? null : Columns,
                Status = (IsActive ?? (short)Status.Active) == (short)Status.Active ? (short)Status.Active :
                 (IsActive == (short)Status.InActive) ? (short)Status.InActive :
                 (IsActive == (short)Status.Delete) ? (short)Status.Delete :
                 IsActive
            };
        }
        public static DirectDepositAuditInfo MapDDAuditInfo(DDConfig Entity, MasterBinAuditing binAudit)
        {
            return new DirectDepositAuditInfo
            {
                AuditId = binAudit.Id,
                Audit = binAudit,
                CorporationLegalName = Entity.CorpLegalName,
                BankName = Entity.BankName,
                OriginatorNo = Entity.OriginatorNo,
                DataCenterNo = Entity.DataCentreNo,
                UpdatedColumns = string.IsNullOrEmpty(Entity.UpdatedColumns) ? null : Entity.UpdatedColumns,
            };
        }
        public static DirectDepositBankDetails MapDDBankConfig(DDConfig Entity, string UserID, DirectDepositBankDetails OrigEntity = null)
        {
            if (OrigEntity != null)
            {
                OrigEntity.CorporationId = new PFAID(Entity.CorporationID).UID;
                OrigEntity.AccountId = new PFAID(Entity.AccoutnID).UID;
                OrigEntity.CorpLegalName = Entity.CorpLegalName;
                OrigEntity.CorpShortName = Entity.CorpShortName;
                OrigEntity.DdbankName = Entity.BankName;
                OrigEntity.DdoriginatorNo = Entity.OriginatorNo;
                OrigEntity.DdinstitutionNo = !string.IsNullOrEmpty(Entity.InstitutionNo) ? Entity.InstitutionNo : null;
                OrigEntity.DdaccountNo = !string.IsNullOrEmpty(Entity.AccountNo) ? Entity.AccountNo : null;
                OrigEntity.DataCentreNo = Entity.DataCentreNo;
                OrigEntity.ResFieldsForOrg = !string.IsNullOrEmpty(Entity.ReservedFields) ? Entity.ReservedFields : null;
                OrigEntity.SettlementCode = !string.IsNullOrEmpty(Entity.SettlementCode) ? Entity.SettlementCode : null;
                OrigEntity.Status = Convert.ToByte(Status.Active);
                OrigEntity.BankAndPaymentsIn = Convert.ToByte(Entity.BankPaymenntsIn);
                OrigEntity.DdbankTemplate = Convert.ToByte(Entity.BankTemplate);
                OrigEntity.PayableTransCode = Entity.PayableCode;
                OrigEntity.UpdatedOn = DateTime.Now;
                OrigEntity.UpdatedBy = new PFAID(UserID).UID;
                OrigEntity.AttachedOn = OrigEntity.IsAttached == Convert.ToInt16(true) ? OrigEntity.AttachedOn : Entity.HasAttachments ? DateTime.Now : null;
                OrigEntity.AttachedBy = OrigEntity.IsAttached == Convert.ToInt16(true) ? OrigEntity.AttachedBy : Entity.HasAttachments ? new PFAID(UserID).UID : null;
                OrigEntity.IsAttached = OrigEntity.IsAttached == Convert.ToInt16(true) ? OrigEntity.IsAttached : Convert.ToInt16(Entity.HasAttachments);
                OrigEntity.Ddtype = Convert.ToByte(Entity.DDType);
                OrigEntity.IsActive = Entity.IsActive;
            }
            else
            {
                OrigEntity = new DirectDepositBankDetails
                {
                    Id = new PFAID().UID,
                    CorporationId = new PFAID(Entity.CorporationID).UID,
                    AccountId = new PFAID(Entity.AccoutnID).UID,
                    BankAndPaymentsIn = Convert.ToByte(Entity.BankPaymenntsIn),
                    CorpLegalName = Entity.CorpLegalName,
                    CorpShortName = Entity.CorpShortName,
                    DdbankName = Entity.BankName,
                    DdoriginatorNo = Entity.OriginatorNo,
                    DdinstitutionNo = !string.IsNullOrEmpty(Entity.InstitutionNo) ? Entity.InstitutionNo : null,
                    DdaccountNo = !string.IsNullOrEmpty(Entity.AccountNo) ? Entity.AccountNo : null,
                    DataCentreNo = Entity.DataCentreNo,
                    ResFieldsForOrg = !string.IsNullOrEmpty(Entity.ReservedFields) ? Entity.ReservedFields : null,
                    SettlementCode = !string.IsNullOrEmpty(Entity.SettlementCode) ? Entity.SettlementCode : null,
                    PayableTransCode = Entity.PayableCode,
                    DdbankTemplate = Convert.ToByte(Entity.BankTemplate),
                    Status = Convert.ToByte(Status.Active),
                    CreatedOn = DateTime.Now,
                    CreatedBy = new PFAID(UserID).UID,
                    IsAttached = Convert.ToInt16(Entity.HasAttachments),
                    AttachedBy = Entity.HasAttachments ? new PFAID(UserID).UID : null,
                    AttachedOn = Entity.HasAttachments ? DateTime.Now : null,
                    Ddtype = Convert.ToByte(Entity.DDType),
                    IsActive = Entity.IsActive
                };
            }
            return OrigEntity;
        }
        public static Eftconfig MapEFTConfig(EFTConfig Entity, Eftconfig OrigEntity = null)
        {
            if (OrigEntity != null)
            {
                OrigEntity.EftformatId = Entity.EFTFromatId;
                OrigEntity.Status = Convert.ToByte(Status.Active);
            }
            else
            {
                OrigEntity = new Eftconfig
                {
                    CorporationId = new PFAID(Entity.CorporationId).UID,
                    AccountId = new PFAID(Entity.AccountId).UID,
                    EftformatId = Entity.EFTFromatId,
                    Status = Convert.ToByte(Status.Active)
                };
            }
            return OrigEntity;
        }
        public static EftformatDetails MapEFTFormatDetails(EFTFormatFields Entity, Eftformat FormatData, EftformatDetails OrigEntity = null)
        {
            if (OrigEntity != null)
            {
                OrigEntity.ColumnNameCust = Entity.FieldName;
                OrigEntity.ExcelCol = Entity.ExcelCol;
                OrigEntity.Status = (short)Status.Active;
            }
            else
            {
                OrigEntity = new EftformatDetails
                {
                    Eftformat = FormatData,
                    ColumnNameCust = Entity.FieldName,
                    ExcelCol = Entity.ExcelCol,
                    Status = (short)Status.Active,
                    EftcolumnId = Entity.ColumnID
                };
            }
            return OrigEntity;
        }
       public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string GetPhoneNumberFormat(string Phone, string contry)
        {
            string Phno = Phone;
            if (!string.IsNullOrEmpty(Phone))
            {
                if (contry == "CA" || contry == "UK" || contry == "US")
                {
                    Phno = new string(Phone.Where(char.IsDigit).ToArray());
                    Phno = Regex.Replace(Phno, @"-", "");
                    Phno = Regex.Replace(Phno, @"\s", "");
                    Phno = string.Format($"1{Phno}");
                    return Phno;
                }
                else if (contry == "CN")
                {
                    Phno = new string(Phone.Where(char.IsDigit).ToArray());
                    Phno = Regex.Replace(Phno, @"-", "");
                    Phno = Regex.Replace(Phno, @"\s", "");
                    Phno = string.Format($"86{Phno}");
                    return Phno;
                }
                else if (contry == "DE")
                {
                    Phno = new string(Phone.Where(char.IsDigit).ToArray());
                    Phno = Regex.Replace(Phno, @"-", "");
                    Phno = Regex.Replace(Phno, @"\s", "");
                    Phno = string.Format($"49{Phno}");
                    return Phno;
                }
                else if (contry == "NO")
                {
                    Phno = new string(Phone.Where(char.IsDigit).ToArray());
                    Phno = Regex.Replace(Phno, @"-", "");
                    Phno = Regex.Replace(Phno, @"\s", "");
                    Phno = string.Format($"47{Phno}");
                    return Phno;
                }
                else if (contry == "NL")
                {
                    Phno = new string(Phone.Where(char.IsDigit).ToArray());
                    Phno = Regex.Replace(Phno, @"-", "");
                    Phno = Regex.Replace(Phno, @"\s", "");
                    Phno = string.Format($"31{Phno}");
                    return Phno;
                }
                else if (contry == "SG")
                {
                    Phno = new string(Phone.Where(char.IsDigit).ToArray());
                    Phno = Regex.Replace(Phno, @"-", "");
                    Phno = Regex.Replace(Phno, @"\s", "");
                    Phno = string.Format($"65{Phno}");
                    return Phno;
                }
                else if (contry == "IN")
                {
                    Phno = new string(Phone.Where(char.IsDigit).ToArray());
                    Phno = Regex.Replace(Phno, @"-", "");
                    Phno = Regex.Replace(Phno, @"\s", "");
                    Phno = string.Format($"91{Phno}");
                    return Phno;
                }
                else
                {
                    Phno = new string(Phone.Where(char.IsDigit).ToArray());
                    Phno = Regex.Replace(Phno, @"-", "");
                    Phno = Regex.Replace(Phno, @"\s", "");
                    Phno = string.Format($"1{Phno}");
                    return Phno;
                }
            }
            else
            {
                return Phno;
            }

        }
        public static RepayCreateOrderRequest MapRepayOrder(string CustId ,EpaymentsBatchDetails paymentInfo, List<RepayPaymentAndVendorDetails> vendorDetails, List<BillEntryPayments> billInfoList,List<BillEntryInformation> invoiceNumList,List<CreditTerm> CreditList,long BatchNumber,string groupName=null,List<string> DocumentListJids=null,string attachmentUrl=null)
        {
            RepayCreateOrderRequest orderrequest = new RepayCreateOrderRequest();
            var vendorInfo = vendorDetails.Where(s => s.VID.ToUpper() == new PFAID(paymentInfo.VendorId).ToString().ToUpper()).FirstOrDefault();
            if (vendorInfo != null)
            {
               
                var billInvoiceList = billInfoList.Where(s => new PFAID(s.BillPaymentId).ToString().ToUpper() == new PFAID(paymentInfo.JournalEntryId).ToString().ToUpper()).ToList();

               
                Repaygroup group = new Repaygroup();
                group.name = !string.IsNullOrEmpty(groupName) ? groupName : Guid.NewGuid().ToString();
              


                orderrequest.group = group;
                orderrequest.overNightCheck = null;
                orderrequest.custId = CustId;
                orderrequest.paymentNumber =paymentInfo.Number;
                orderrequest.comments = "";
                orderrequest.misc1 = "";
              
               
               
                RepayVendor ven = new RepayVendor();
                ven.vendorName1 = vendorInfo.Name;
                ven.contactEmail = vendorInfo.Email;

                ven.vendorPhone = GetPhoneNumberFormat(vendorInfo.Phone, vendorInfo.CountryCode);
                if(!string.IsNullOrEmpty(paymentInfo.VendorRefId))
                {
                    ven.vendorNumber=paymentInfo.VendorRefId;
                }
                else
                {
                    if (!string.IsNullOrEmpty(vendorInfo.SSN) && !string.IsNullOrEmpty(vendorInfo.FederalID))
                    {
                        ven.vendorNumber = vendorInfo.SSN.Replace("-", string.Empty);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(vendorInfo.SSN))
                        {
                            ven.vendorNumber = vendorInfo.SSN.Replace("-", string.Empty);
                        }
                        else
                        {
                            ven.vendorNumber = vendorInfo.FederalID.Replace("-", string.Empty);
                        }
                    }
                }
                
                ven.locationCode = "";
                RepayVendorAddress venAdd = new RepayVendorAddress();
                venAdd.state = vendorInfo.StateCode;
                if(!string.IsNullOrEmpty(vendorInfo.Address1))
                {
                    string[] addrlines = vendorInfo.Address1.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                    venAdd.address1 = string.Join(",", addrlines);
                    if (venAdd.address1.Length > 200)
                    {
                        venAdd.address1 = venAdd.address1.Substring(0, 200);
                        // sDesc = sDesc + "...";
                    }
                }
                
                venAdd.zip = vendorInfo.ZipCode;
                venAdd.city = vendorInfo.City;
                venAdd.countryCode = vendorInfo.CountryCode;

                ven.address = venAdd;
                List<VenorInvoices> venInvoiceList = new List<VenorInvoices>();
                foreach(var item in billInvoiceList)
                {
                   var invoideNo= invoiceNumList.Where(s => s.Id == item.BillInformationId).FirstOrDefault();
                    var creditInfo = CreditList.Where(s => new PFAID(s.Id).ToString().ToUpper() == new PFAID(invoideNo.JournalEntryId).ToString().ToUpper()).FirstOrDefault();
                    string duedate = "";
                    if (creditInfo!=null)
                    {
                        duedate = creditInfo.DueDate.ToString("yyyy-MM-dd");
                    }
                    VenorInvoices venInvoice = new VenorInvoices();
                    venInvoice.invoiceDate = invoideNo.BillDate!=null? invoideNo.BillDate.Value.ToString("yyyy-MM-dd"):DateTime.Now.ToString("yyyy-MM-dd");
                    venInvoice.invoiceNumber = invoideNo.EntryNumber;
                    venInvoice.netAmount =item.PaidAmount.Value;
                    venInvoice.dueDate = duedate;
                    venInvoice.totalAmount = (item.PaidAmount??0)+(item.Discount??0);
                    venInvoice.adjustAmount = item.Discount ?? 0;
                    if(DocumentListJids!=null)
                    {
                       var docexist= DocumentListJids.Where(s => s == new PFAID(invoideNo.JournalEntryId).ToString()).FirstOrDefault();
                        if(docexist!=null)
                        {
                            string Id= group.name + "&&" + new PFAID(invoideNo.JournalEntryId).ToString();
                            if(!string.IsNullOrEmpty(attachmentUrl))
                            {
                                venInvoice.misc1 = string.Format($"{attachmentUrl}/{Base64Encode(Id)}");
                            }
                            
                        }
                    }
                    orderrequest.vendor = ven;
                    venInvoiceList.Add(venInvoice);
                }
                
                orderrequest.invoices = venInvoiceList;
            }
            return orderrequest;
        }
        public static RepayCreateOrderRequest MapRepayOrder1(RePayKeyInfo repayKeyInfo, EpaymentsBatchDetails paymentInfo, List<RepayPaymentAndVendorDetails> vendorDetails, List<BillEntryPayments> billInfoList, List<BillEntryInformation> invoiceNumList, List<CreditTerm> CreditList, long BatchNumber,string groupname)
        {
            RepayCreateOrderRequest orderrequest = new RepayCreateOrderRequest();
            var vendorInfo = vendorDetails.Where(s => s.VID.ToUpper() == new PFAID(paymentInfo.VendorId).ToString().ToUpper()).FirstOrDefault();
            if (vendorInfo != null)
            {

                var billInvoiceList = billInfoList.Where(s => new PFAID(s.BillPaymentId).ToString().ToUpper() == new PFAID(paymentInfo.JournalEntryId).ToString().ToUpper()).ToList();


                Repaygroup group = new Repaygroup();
                group.name = groupname;
                orderrequest.group = group;
                orderrequest.overNightCheck = null;
               // orderrequest.custId = repayKeyInfo.CustomerId;
                orderrequest.paymentNumber = paymentInfo.Number;
                orderrequest.comments = "";
                orderrequest.misc1 = "";

                RepayVendor ven = new RepayVendor();
                ven.vendorName1 = vendorInfo.Name;
                //if (vendorInfo.Name.ToLower() == "vendortwo")
                //{
                //    ven.vendorNumber = "1111112345";
                //}
                //else if (vendorInfo.Name.ToLower() == "vendorthree")
                //{
                //    ven.vendorNumber = "1111112346";
                //}
                //else
                //{
                //    ven.vendorNumber = vendorInfo.SSN;
                //}
                ven.vendorNumber = vendorInfo.SSN.Replace("-", string.Empty);
                ven.locationCode = "";
                RepayVendorAddress venAdd = new RepayVendorAddress();
                venAdd.state = vendorInfo.StateCode;
                venAdd.address1 = vendorInfo.Address1;
                venAdd.zip = vendorInfo.ZipCode;
                venAdd.city = vendorInfo.City;
                venAdd.countryCode = vendorInfo.CountryCode;
                ven.address = venAdd;
                List<VenorInvoices> venInvoiceList = new List<VenorInvoices>();
                foreach (var item in billInvoiceList)
                {
                    var invoideNo = invoiceNumList.Where(s => s.Id == item.BillInformationId).FirstOrDefault();
                    var creditInfo = CreditList.Where(s => new PFAID(s.Id).ToString().ToUpper() == new PFAID(invoideNo.JournalEntryId).ToString().ToUpper()).FirstOrDefault();
                    string duedate = "";
                    if (creditInfo != null)
                    {
                        duedate = creditInfo.DueDate.ToString("yyyy-MM-dd");
                    }
                    VenorInvoices venInvoice = new VenorInvoices();
                    venInvoice.invoiceDate = invoideNo.BillDate != null ? invoideNo.BillDate.Value.ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd");
                    venInvoice.invoiceNumber = invoideNo.EntryNumber;
                    venInvoice.netAmount = item.PaidAmount.Value;
                    venInvoice.dueDate = duedate;
                    venInvoice.totalAmount = (item.PaidAmount ?? 0) + (item.Discount ?? 0);
                    venInvoice.adjustAmount = item.Discount ?? 0;
                    orderrequest.vendor = ven;
                    venInvoiceList.Add(venInvoice);
                }

                orderrequest.invoices = venInvoiceList;
            }
            return orderrequest;
        }
        #endregion EPayment Mappers

        #region CommonMappers
        public static MiscInfo MapMiscInfoEntry(string Name, byte Status, string CLientID, short PaymethodType, short SourceType = 0, MiscInfo OrigEntity = null)
        {
            if (OrigEntity != null)
            {

            }

            else
            {
                OrigEntity = new MiscInfo
                {
                    Id = new PFAID().UID,
                    Name = Name,
                    Description = Name,
                    SourceType = SourceType > 0 ? SourceType : (short)MiscMasterType.Online,
                    Status = Status,
                    ClientId = new PFAID(CLientID).UID,
                    PaymentMethodType = PaymethodType
                };

            }
            return OrigEntity;
        }
        #endregion CommonMappersEnd
        public static JournalEntry MapBillPaymentJE_V1(BillPaymentDetails entity, JournalEntry origEntity = null)
        {
            if (origEntity != null)
            {
                origEntity.EntryDate = entity.Date.Date;
                origEntity.ModifiedDate = DateTime.Now;
                origEntity.PaymentMethodId = new PFAID(entity.PaymentMethodID).UID;
                origEntity.IsUpdated = 1;
                origEntity.IsPrintRequired = entity.ToBePrinted;
                origEntity.EntryNumber = entity.RefNumber;
                return origEntity;
            }

            else
            {
                return new JournalEntry()
                {
                    Id = new PFAID().UID,
                    CorporationId = new PFAID(entity.CorpID).UID,
                    EntryDate = entity.Date.Date,
                    IsMailSent = true,
                    ReferenceMode = 2,//need to check enum
                    IsPrintRequired = entity.ToBePrinted,
                    EntryNumber = entity.RefNumber,
                    CreatedDate = DateTime.Now,
                    SourceType = (short?)JournalSourceTypes.PayBill,
                    ParentSourceType = (short?)JournalSourceTypes.PayBill,
                    PaymentMethodId = new PFAID(entity.PaymentMethodID).UID,
                    IsUpdated = 1,
                    Status = (short)Status.Active

                };
            }
        }
        public static JournalEntry MapBillPaymentJournal(BillPaymentDetails entity, byte[] userID, byte? entryType=null, string feedDate = null, JournalEntry origEntity = null)
        {
            if (origEntity != null)
            {
                origEntity.EntryDate = entity.Date.Date;
                origEntity.ModifiedDate = DateTime.Now;
                origEntity.ModifiedBy = new PFAID(userID).ToString() == Constants.SystemUser ? null : userID;
                origEntity.PaymentMethodId = new PFAID(entity.PaymentMethodID).UID;
                origEntity.IsUpdated = 1;
                origEntity.IsPrintRequired = entity.ToBePrinted;
                origEntity.EntryNumber = entity.RefNumber;
                origEntity.ClearedDate = !string.IsNullOrEmpty(feedDate) ? Convert.ToDateTime(feedDate) : null;
                return origEntity;
            }

            else
            {
                return new JournalEntry()
                {
                    Id = new PFAID().UID,
                    CorporationId = new PFAID(entity.CorpID).UID,
                    EntryDate = entity.Date.Date,
                    IsMailSent = true,
                    ReferenceMode = 2,//need to check enum
                    IsPrintRequired = entity.ToBePrinted,
                    EntryNumber = entity.RefNumber,
                    CreatedDate = DateTime.Now,
                    CreatedBy = Constants.SystemUser == new PFAID(userID).ToString() ? null : userID,
                    SourceType = (short?)JournalSourceTypes.PayBill,
                    ParentSourceType = (short?)JournalSourceTypes.PayBill,
                    PaymentMethodId = new PFAID(entity.PaymentMethodID).UID,
                    IsUpdated = 1,
                    Status = (short)Status.Active,
                    EntryType= (byte)entryType,
                    ClearedDate = !string.IsNullOrEmpty(feedDate) ? Convert.ToDateTime(feedDate) : null

                };
            }
        }

        public static Transaction MapBillPaymentTransaction_V1(BillPaymentDetails entity, byte[] JournalEntryId, byte[] defStore, List<BillPaymentDivisions> bpd, string CheckNumber, Transaction origEntity = null)
        {
            if (origEntity != null)
            {
                origEntity.AccountId = new PFAID(entity.BankAccountID).UID;
                origEntity.SourceId = new PFAID(bpd[0].PayeeID).UID;
                origEntity.Amount = bpd.Where(x => x.IsDebitMemo == false).Sum(x => x.AmountPaid) - bpd.Where(x => x.IsDebitMemo == true).Sum(x => x.AmountPaid);
                origEntity.TransactionDate = DateTime.Now;
                origEntity.ReferenceNumber = entity.ToBePrinted ? "To be Printed" : CheckNumber;
                origEntity.Memo = !string.IsNullOrEmpty(entity.Memo) ? entity.Memo : null;
                
                return origEntity;
            }
            else
            {
                origEntity = new Transaction
                {
                    Id = new PFAID().UID,
                    AccountId = new PFAID(entity.BankAccountID).UID,
                    JournalEntryId = JournalEntryId,
                    Amount = bpd.Where(x => x.IsDebitMemo == false).Sum(x => x.AmountPaid) - bpd.Where(x => x.IsDebitMemo == true).Sum(x => x.AmountPaid),
                    DebitCredit = Convert.ToBoolean(TransactionDebitCredit.Credit),
                    TransactionDate = DateTime.Now,
                    Memo = !string.IsNullOrEmpty(entity.Memo) ? entity.Memo : null,
                    Status = (short)Status.Active,
                    SourceId = new PFAID(bpd[0].PayeeID).UID,
                    SourceType = (short)TransactionSourceType.Vendor,
                    StoreId = defStore,
                    ReferenceNumber = entity.ToBePrinted ? "To be Printed" : CheckNumber
                };
                return origEntity;
            }
        }
        public static Transaction MapBillPaymentParentTransaction(BillPaymentDetails entity, byte[] JournalEntryId, byte[] defStore, byte[] vendorID, decimal amount, string CheckNumber, string feedTransID = null, Transaction origEntity = null)
        {
           
            if (origEntity != null)
            {
                origEntity.AccountId = new PFAID(entity.BankAccountID).UID;
                origEntity.SourceId = vendorID;
                origEntity.Amount = amount;
                origEntity.TransactionDate = DateTime.Now;
                origEntity.ReferenceNumber = CheckNumber;
                origEntity.Memo = !string.IsNullOrEmpty(entity.Memo) ? entity.Memo : null;
                origEntity.BankTransactionRefId = !string.IsNullOrEmpty(feedTransID) ? feedTransID : null;
                return origEntity;
            }
            else
            {
                origEntity = new Transaction
                {
                    Id = new PFAID().UID,
                    AccountId = new PFAID(entity.BankAccountID).UID,
                    JournalEntryId = JournalEntryId,
                    Amount = amount,
                    DebitCredit = Convert.ToBoolean(TransactionDebitCredit.Credit),
                    TransactionDate = DateTime.Now,
                    Memo = !string.IsNullOrEmpty(entity.Memo) ? entity.Memo : null,
                    Status = (short)Status.Active,
                    SourceId = vendorID,
                    SourceType = (short)TransactionSourceType.Vendor,
                    StoreId = null,
                    ReferenceNumber = CheckNumber,
                    Order = 0,
                    BankTransactionRefId= !string.IsNullOrEmpty(feedTransID)? feedTransID : null
                };
                return origEntity;
            }
        }

        public static Transaction MapSplitBillPaymentPaidTransaction_V1(BillPaymentDetails entity, Transaction VendorTrans, byte[] accID, byte[] defPc, List<BillPaymentDivisions> bpd, Transaction origEntity = null)
        {
            if (origEntity != null)
            {
                origEntity.AccountId = accID;
                origEntity.Amount = (bpd.Where(x => x.IsDebitMemo == false).Sum(x => x.AmountPaid) - bpd.Where(x => x.IsDebitMemo == true).Sum(x => x.AmountPaid)) +
                     (bpd.Where(x => x.IsDebitMemo == false).Sum(x => x.DiscountAmount));
                origEntity.TransactionDate = DateTime.Now;
                origEntity.ReferenceNumber = VendorTrans.ReferenceNumber ?? null;
                origEntity.SourceId = new PFAID(entity.JEID).UID;
                origEntity.StoreId = !string.IsNullOrEmpty(bpd.FirstOrDefault().StoreID) ? new PFAID(bpd.FirstOrDefault().StoreID).UID : defPc;
                return origEntity;
            }
            else
            {
                origEntity = new Transaction();
                origEntity.Id = new PFAID().UID;
                origEntity.AccountId = accID;
                origEntity.JournalEntryId = VendorTrans.JournalEntryId;
                origEntity.ParentId = VendorTrans.Id;
                //TO Verify : Discount amount & Debit Memo's are coming with minus sign 
                origEntity.Amount = (bpd.Where(x => x.IsDebitMemo == false).Sum(x => x.AmountPaid) - bpd.Where(x => x.IsDebitMemo == true).Sum(x => x.AmountPaid)) +
                     (bpd.Where(x => x.IsDebitMemo == false).Sum(x => x.DiscountAmount));
                origEntity.DebitCredit = Convert.ToBoolean(TransactionDebitCredit.Debit);
                origEntity.TransactionDate = DateTime.Now;
                origEntity.ReferenceNumber = VendorTrans.ReferenceNumber ?? null;
                origEntity.SourceId = new PFAID(entity.JEID).UID;//BillEntryJE
                origEntity.SourceType = (short)JournalSourceTypes.Bill;
                origEntity.StoreId = !string.IsNullOrEmpty(bpd.FirstOrDefault()?.StoreID) ? new PFAID(bpd.FirstOrDefault().StoreID).UID : defPc;
                origEntity.Status = (short)Status.Active;
                return origEntity;
            }
        }

        public static Transaction MapSplitBillPaymentDiscountTransaction_V1(BillPaymentDetails entity, Transaction VendorTrans, string checkno, byte[] defPc, List<BillPaymentDivisions> bpd, Transaction origEntity = null)
        {
            var discAccount = entity.BillPaymentDivisions.Where(x => x.DiscountAccount != null).Select(x => x.DiscountAccount).FirstOrDefault();
            if (origEntity != null)
            {
                origEntity.AccountId = new PFAID(discAccount).UID;
                origEntity.Amount = bpd.Where(x => x.IsDebitMemo == false).Sum(x => x.DiscountAmount);
                origEntity.TransactionDate = DateTime.Now;
                origEntity.ReferenceNumber = VendorTrans.ReferenceNumber;
                origEntity.SourceId = new PFAID(entity.JEID).UID;
                //origEntity.StoreId = !string.IsNullOrEmpty(entity.StoreID) ? new PFAID(entity.StoreID).UID : defPc;               
                return origEntity;
            }
            else
            {
                origEntity = new Transaction();
                origEntity.Id = new PFAID().UID;
                origEntity.AccountId = new PFAID(discAccount).UID;
                origEntity.JournalEntryId = VendorTrans.JournalEntryId;
                origEntity.ParentId = VendorTrans.Id;
                origEntity.Amount = bpd.Where(x => x.IsDebitMemo == false).Sum(x => x.DiscountAmount);
                origEntity.DebitCredit = Convert.ToBoolean(TransactionDebitCredit.Credit);
                origEntity.TransactionDate = DateTime.Now;
                origEntity.ReferenceNumber = checkno;
                origEntity.Status = (short)Status.Active;
                origEntity.SourceId = new PFAID(entity.JEID).UID;
                origEntity.SourceType = (short)TransactionSourceType.Discount;
                //origEntity.StoreId = !string.IsNullOrEmpty(entity.StoreID) ? new PFAID(entity.StoreID).UID : defPc;
                return origEntity;
            }
        }


        public static BillEntryPayments MapSplitBillEntryPaymentTransaction_V1(BillPaymentDivisions entity, Transaction VendorTrans, Transaction paidTran, Transaction discountTran, BillEntryInformation bei, byte[] pcid, BillEntryPayments? origEntity = null)
        {
            if (origEntity != null)
            {
                origEntity.PaidAmount = entity.AmountPaid;
                origEntity.OutStanding = entity.AmountDue - (entity.AmountPaid + entity.DiscountAmount);
                origEntity.AdjustmentAmount = 0;
                origEntity.Status = origEntity.OutStanding == 0 ? (short)TransactionStatus.Completed : (short)TransactionStatus.Active;
                if (!entity.IsDebitMemo)
                {
                    origEntity.DiscountAccountId = new PFAID(entity.DiscountAccount).UID;
                    origEntity.Discount = entity.DiscountAmount;
                }
            }
            else
            {
                origEntity = new BillEntryPayments();
                origEntity.TransactioonId = paidTran.Id;
                origEntity.SourceId = VendorTrans.SourceId;
                origEntity.BillInformationId = bei.Id;
                origEntity.BillPaymentId = VendorTrans.JournalEntryId;
                origEntity.PaidAmount = entity.AmountPaid;
                //TODO : Outstanding bill entry information outstanding
                origEntity.OutStanding = entity.AmountDue - (entity.AmountPaid + entity.DiscountAmount);
                origEntity.AdjustmentAmount = 0;
                origEntity.Status = origEntity.OutStanding == 0 ? (short)TransactionStatus.Completed : (short)TransactionStatus.Active;
                origEntity.Pcid = pcid;
                if (!entity.IsDebitMemo)
                {
                    origEntity.Discount = entity.DiscountAmount;
                    origEntity.DiscountAccountId = new PFAID(entity.DiscountAccount).UID;
                    origEntity.DiscountTransactionId = discountTran == null ? null : discountTran.Id;
                }
            }
            return origEntity;
        }


        public static Pctransactions MapPCTransaction_V1(long? billInfoID, decimal amount, decimal outstanding, byte[] pcID, bool debitCredit, byte[] tID, byte[] refID = null)
        {
            Pctransactions pcTran = new Pctransactions();
            pcTran.TransactionId = tID;
            pcTran.RefId = refID;
            pcTran.Amount = amount;
            pcTran.Oustanding = outstanding;
            pcTran.Pcid = pcID;
            // pcTran.BillInformationId = billInfoID;
            pcTran.DebitCredit = debitCredit;
            return pcTran;
        }

        public static BillPaymentsInformation MapBillPaymentInformationBPI(JournalEntry j, Transaction VendorTrans, short BillPayType, BillPaymentsInformation? origEntity = null)
        {
            if (origEntity != null)
            {
                origEntity.AccountId = VendorTrans.AccountId;
                origEntity.Amount = VendorTrans.Amount;
                origEntity.Memo = VendorTrans.Memo;
                origEntity.StoreId = VendorTrans.StoreId;
                origEntity.PayMethodId = j.PaymentMethodId;
                origEntity.ModifiedBy = j.ModifiedBy;
                origEntity.ModifiedDate = j.ModifiedDate;
                origEntity.Status = (short)j.Status;
                origEntity.CheckNo = VendorTrans.ReferenceNumber;
                origEntity.EntryDate = j.EntryDate.Date;
                return origEntity;
            }
            else
            {
                origEntity = new BillPaymentsInformation();
                origEntity.AccountId = VendorTrans.AccountId;
                origEntity.JournalEntryId = VendorTrans.JournalEntryId;
                origEntity.Amount = VendorTrans.Amount;
                origEntity.SourceId = VendorTrans.SourceId;
                origEntity.StoreId = VendorTrans.StoreId;
                origEntity.Memo = VendorTrans.Memo;
                origEntity.TransactionId = VendorTrans.Id;
                origEntity.Status = (short)Status.Active;
                origEntity.CorporationId = j.CorporationId;
                origEntity.PayMethodId = j.PaymentMethodId;
                origEntity.CreatedBy = j.CreatedBy;
                origEntity.CreatedDate = DateTime.Now;
                origEntity.EntryDate = j.EntryDate.Date;
                origEntity.CheckNo = VendorTrans.ReferenceNumber;
                origEntity.EntryType = BillPayType;
               
            }


            return origEntity;
        }


        public static BillPaymentsInformation MapBillPaymentInformation_V1(BillPaymentDetails entity, Transaction VendorTrans, long billInfoID, byte[] accID, byte[] defPc, List<BillPaymentDivisions> bpd, string CheckNumber, BillPaymentsInformation? origEntity = null)
        {
            if (origEntity != null)
            {
                origEntity.AccountId = new PFAID(entity.BankAccountID).UID;
                origEntity.Amount = bpd.Where(x => x.IsDebitMemo == false).Sum(x => x.AmountPaid) -
                                    bpd.Where(x => x.IsDebitMemo == true).Sum(x => x.AmountPaid);
                origEntity.Memo = entity.Memo;

                origEntity.StoreId = defPc;
                return origEntity;
            }
            else
            {
                origEntity = new BillPaymentsInformation();
                origEntity.AccountId = new PFAID(entity.BankAccountID).UID;
                // origEntity.JournalEntryId = VendorTrans.JournalEntryId;
                origEntity.Amount = bpd.Where(x => x.IsDebitMemo == false).Sum(x => x.AmountPaid) -
                                   bpd.Where(x => x.IsDebitMemo == true).Sum(x => x.AmountPaid);
                origEntity.SourceId = VendorTrans.SourceId;
                origEntity.StoreId = defPc;
                origEntity.Memo = entity.Memo;
                origEntity.TransactionId = VendorTrans.Id;



            }
            return origEntity;
        }
       
        #region EPayMapper
        public static EpaymentsBatchDetails MapEPaymentBatchDetails(EpaymentsBatchDetails entity, BillPaymentsInformation ePayInfo = null, string checkNo = null, long? issuedCount = 0, short? ActionType = null, string userId = null)
        {
            if (entity == null)
            {
                entity = new EpaymentsBatchDetails();
                entity.Amount = ePayInfo.Amount.Value;
                entity.JournalEntryId = new PFAID(ePayInfo.JournalEntryId).UID;
                entity.Status = ActionType.Value;
                entity.TransactionId = new PFAID(ePayInfo.TransactionId).UID;
                entity.CreatedBy = new PFAID(userId).UID;
                entity.CreatedDate = DateTime.Now;
                entity.VendorId = new PFAID(ePayInfo.SourceId).UID;
                entity.EntryDate = ePayInfo.EntryDate;

                entity.Number = checkNo;
                return entity;
            }
            else
            {
                entity.Number = checkNo;
                entity.Status = ActionType.Value;
                entity.ModifiedDate = DateTime.Now;
                entity.ModifiedBy = new PFAID(userId).UID;
                return entity;
            }
        }

        public static EpaymentsBatch MapEPaymentBatch(EpaymentsBatch entity, short? approvalLevel = null, DateTime? intiationDate = null, string userID = null, string accountId = null, long? batchNo = null,
            string corporationId = null, byte[] paymentMethodId = null, short? status = null, long? BatchIssueId = null, long? FormatID = null)
        {
            if (entity == null)
            {
                entity = new EpaymentsBatch();
                entity.Status = (short)status;
                entity.ApprovalLevel = approvalLevel.Value;
                entity.BatchNumber = batchNo.Value;
                entity.CreatedBy = new PFAID(userID).UID;
                entity.CreatedDate = DateTime.Now;
                entity.IntiationDate = intiationDate.GetValueOrDefault();
                entity.AssignedTo = new PFAID("000000000000000000000000000000000000").UID;
                entity.BatchIssueId = BatchIssueId;
                entity.FormatId = FormatID;

                return entity;
            }
            else
            {
                entity.Status = (short)status;
                entity.ApprovalLevel = approvalLevel.Value;
                entity.IntiationDate = intiationDate.Value;
                entity.ModifiedDate = DateTime.Now;
                entity.ModifiedBy = new PFAID(userID).UID;
                return entity;
            }
        }

        public static EpaymentsBatchLog MapEpaymentsBatchLog(short status, short actionType, string userID, EpaymentsBatchLog entity = null)
        {
            entity = new EpaymentsBatchLog();
            entity.Status = status;
            entity.ActionType = actionType;
            entity.CreatedBy = new PFAID(userID).UID;
            entity.CreatedDate = DateTime.Now;
            return entity;

        }
        public static EpaymentsBatchVoidDeleteLog MapEPaymentsBatchVoidDeleteLog(short status, byte[] JournalId, byte[] TransactionId, EpaymentsBatchVoidDeleteLog entity = null)
        {
            entity = new EpaymentsBatchVoidDeleteLog();
            entity.Status = status;
            entity.JournalEntryId = JournalId;
            entity.TransactionId = TransactionId;
            return entity;

        }

        public static EPaymentBatchNoGenerationRequest MapEPayGenerateBatchNo(string accountId, string corpID, string paymentId, short issueType, short createNew, string clientId, EPaymentBatchNoGenerationRequest entity = null)
        {
            entity = new EPaymentBatchNoGenerationRequest();
            entity.AccountID = accountId;
            entity.ClientId = clientId;
            entity.CorpID = corpID;
            entity.PaymentMethod = paymentId;
            entity.IssueType = issueType;
            entity.Type = createNew;

            return entity;

        }
        public static DBListOFEntries<Transaction> MapSplitBillPaymentAPAndDiscTransaction_V1(Transaction VendorTrans, string checkno, byte[] defPc, byte[] vendorAccID, List<BillPaymentDivisions> bpd, List<Transaction> TransOriganls = null)
        {


            DBListOFEntries<Transaction> tlist = new DBListOFEntries<Transaction>();

            decimal appliedAmount = bpd.Sum(s => (decimal?)s.AmountPaid) ?? 0;
            string transID = bpd.Select(s => s.TID).FirstOrDefault();
            bool isTransExist = TransOriganls != null && TransOriganls.Count() > 0;
            Transaction origEntity = !string.IsNullOrEmpty(transID) && isTransExist ? TransOriganls.Where(S => new PFAID(S.Id).ToString() == transID).FirstOrDefault() : null;
            if (origEntity != null)
            {
                origEntity.AccountId = vendorAccID;
                origEntity.Amount = appliedAmount;
                origEntity.TransactionDate = DateTime.Now;
                origEntity.ReferenceNumber = VendorTrans.ReferenceNumber ?? null;
                origEntity.SourceId = VendorTrans.JournalEntryId;
                origEntity.StoreId = !string.IsNullOrEmpty(bpd.FirstOrDefault().StoreID) ? new PFAID(bpd.FirstOrDefault().StoreID).UID : defPc;
                tlist.ItemsTobeUpdated.Add(origEntity);
            }
            else
            {
                origEntity = new Transaction();
                origEntity.Id = new PFAID().UID;
                origEntity.AccountId = vendorAccID;
                origEntity.JournalEntryId = VendorTrans.JournalEntryId;
                origEntity.ParentId = VendorTrans.Id;
                //TO Verify : Discount amount & Debit Memo's are coming with minus sign 
                origEntity.Amount = (bpd.Where(x => x.IsDebitMemo == false).Sum(x => x.AmountPaid) - bpd.Where(x => x.IsDebitMemo == true).Sum(x => x.AmountPaid)) +
                     (bpd.Where(x => x.IsDebitMemo == false).Sum(x => x.DiscountAmount));
                origEntity.DebitCredit = Convert.ToBoolean(TransactionDebitCredit.Debit);
                origEntity.TransactionDate = DateTime.Now;
                origEntity.ReferenceNumber = VendorTrans.ReferenceNumber ?? null;
                //origEntity.SourceId = VendorTrans.JournalEntryId;//BillEntryJE
                origEntity.SourceType = (short)JournalSourceTypes.Bill;
                origEntity.StoreId = !string.IsNullOrEmpty(bpd.FirstOrDefault()?.StoreID) ? new PFAID(bpd.FirstOrDefault().StoreID).UID : defPc;
                origEntity.Status = (short)Status.Active;
                origEntity.Order = (short?)((VendorTrans.Order ?? 0) + 1);
                tlist.ItemsTobeInserted.Add(origEntity);
            }

            List<string> discountAccountIDs = bpd.Select(s => s.DiscountAccount).ToList();
            var discountTranIds = bpd.Where(s => discountAccountIDs.Contains(s.DiscountAccount)).Select(s => new { s.DiscountAccount, s.DTID }).ToList();
            int i = origEntity.Order ?? 1;

            foreach (string disAccID in discountAccountIDs)
            {
                byte[] dAccID = new PFAID(disAccID).UID;
                decimal discAmount = bpd.Where(s => s.DiscountAccount == disAccID).Sum(s => (decimal?)s.AmountPaid) ?? 0;
                string discTransID = discountTranIds.Where(s => s.DiscountAccount == disAccID).Select(s => s.DTID).FirstOrDefault();
                Transaction origDiscEntity = !string.IsNullOrEmpty(discTransID) && isTransExist ? TransOriganls.Where(S => new PFAID(S.Id).ToString() == discTransID).FirstOrDefault() : null;
                if (origDiscEntity != null)
                {
                    origDiscEntity.AccountId = dAccID;
                    origDiscEntity.Amount = discAmount;
                    origDiscEntity.TransactionDate = DateTime.Now;
                    origDiscEntity.ReferenceNumber = VendorTrans.ReferenceNumber;
                    origDiscEntity.SourceId = VendorTrans.JournalEntryId;
                    short? max = TransOriganls.Max(s => s.Order);
                    i = max ?? i;
                    tlist.ItemsTobeUpdated.Add(origDiscEntity);

                }
                else
                {
                    origDiscEntity = new Transaction();
                    origDiscEntity.Id = new PFAID().UID;
                    origDiscEntity.AccountId = dAccID;
                    origDiscEntity.JournalEntryId = VendorTrans.JournalEntryId;
                    origDiscEntity.ParentId = VendorTrans.Id;
                    origDiscEntity.Amount = discAmount;
                    origDiscEntity.DebitCredit = Convert.ToBoolean(TransactionDebitCredit.Credit);
                    origDiscEntity.TransactionDate = DateTime.Now;
                    origDiscEntity.ReferenceNumber = checkno;
                    origDiscEntity.Status = (short)Status.Active;
                    // origDiscEntity.SourceId = VendorTrans.JournalEntryId;
                    origDiscEntity.SourceType = (short)TransactionSourceType.Discount;
                    i = i + 1;
                    origDiscEntity.Order = (short?)i;
                    tlist.ItemsTobeInserted.Add(origDiscEntity);
                }

            }



            return tlist;
        }

        public static DBListOFEntries<Transaction> MapSplitBillPaymentAPAndDiscTransactions(Transaction VendorTrans, string checkno, byte[] defPc, byte[] vendorAccID, List<BillPaymentDivisions> bpd, List<BillEntryInformation> beiList, IEnumerable<Transaction> TransOriganls = null)
        {


            DBListOFEntries<Transaction> tlist = new DBListOFEntries<Transaction>();
            decimal appliedAmount = bpd.Sum(s => (decimal?)s.AmountPaid) ?? 0;
            string transID = bpd.Select(s => s.TID).FirstOrDefault();
            bool isTransExist = TransOriganls != null && TransOriganls.Count() > 0;
            Transaction origEntity = !string.IsNullOrEmpty(transID) && isTransExist ? TransOriganls.Where(S => new PFAID(S.Id).ToString() == transID).FirstOrDefault() : null;
            if (origEntity != null)
            {
                origEntity.AccountId = vendorAccID;
                origEntity.Amount = (bpd.Where(x => x.IsDebitMemo == false).Sum(x => x.AmountPaid) + bpd.Where(x => x.IsDebitMemo == true).Sum(x => x.AmountPaid)) +
                     (bpd.Where(x => x.IsDebitMemo == false).Sum(x => x.DiscountAmount));
                origEntity.TransactionDate = DateTime.Now;
                origEntity.ReferenceNumber = VendorTrans.ReferenceNumber ?? null;
                origEntity.SourceId = VendorTrans.JournalEntryId;
                var StoreID = bpd.FirstOrDefault()?.StoreID;
                origEntity.StoreId = !string.IsNullOrEmpty(StoreID) && StoreID != Constants.SystemUser ? new PFAID(StoreID).UID : null;
                tlist.ItemsTobeUpdated.Add(origEntity);
            }
            else
            {
                origEntity = new Transaction();
                origEntity.Id = new PFAID().UID;
                origEntity.AccountId = vendorAccID;
                origEntity.JournalEntryId = VendorTrans.JournalEntryId;
                origEntity.ParentId = VendorTrans.Id;
                //TO Verify : Discount amount & Debit Memo's are coming with minus sign 
                origEntity.Amount = (bpd.Where(x => x.IsDebitMemo == false).Sum(x => x.AmountPaid) + bpd.Where(x => x.IsDebitMemo == true).Sum(x => x.AmountPaid)) +
                     (bpd.Where(x => x.IsDebitMemo == false).Sum(x => x.DiscountAmount));
                origEntity.DebitCredit = Convert.ToBoolean(TransactionDebitCredit.Debit);
                origEntity.TransactionDate = DateTime.Now;
                origEntity.ReferenceNumber = VendorTrans.ReferenceNumber ?? null;
                //origEntity.SourceId = VendorTrans.JournalEntryId;//BillEntryJE
                origEntity.SourceType = (short)JournalSourceTypes.Bill;
                var StoreID = bpd.FirstOrDefault()?.StoreID;
                origEntity.StoreId = !string.IsNullOrEmpty(StoreID) && StoreID!=Constants.SystemUser ? new PFAID(StoreID).UID : null;
                origEntity.Status = (short)Status.Active;
                origEntity.Order = (short?)((VendorTrans.Order ?? 0) + 1);
                tlist.ItemsTobeInserted.Add(origEntity);
            }

            List<string> discountAccountIDs = bpd.Where(s => s.DiscountAmount! != 0).Select(s => s.DiscountAccount).ToList();//!string.IsNullOrEmpty(s.DiscountAccount)||
            if (discountAccountIDs.Count > 0)
            {


                var discountInfo = (from s in bpd
                                    where discountAccountIDs.Contains(s.DiscountAccount)
                                    group s by new { s.DiscountAccount, s.DTID, s.DiscountMemo, s.BillInfoId } into g
                                    select new
                                    {
                                        DiscountAccount = g.Key.DiscountAccount,
                                        DiscountMemo = g.Key.DiscountMemo,
                                        DTID = g.Key.DTID,
                                        BillInfoID = g.Key.BillInfoId,
                                        Amount = g.Sum(x => (decimal?)x.DiscountAmount) ?? 0,
                                    }).ToList();


                var discountTranIds = bpd.Where(s => discountAccountIDs.Contains(s.DiscountAccount)).Select(s => new { s.DiscountAccount, s.DTID }).ToList();
                int i = origEntity.Order ?? 1;

                foreach (var disc in discountInfo)
                {
                    byte[] dAccID = new PFAID(disc.DiscountAccount).UID;
                    decimal discAmount = disc.Amount;
                    string discTransID = disc.DTID;
                    byte[] billJournalEntryID = beiList.Where(s => s.Id == disc.BillInfoID).Select(s => s.JournalEntryId).FirstOrDefault();
                    Transaction origDiscEntity = !string.IsNullOrEmpty(discTransID) && isTransExist ? TransOriganls.Where(S => new PFAID(S.Id).ToString() == discTransID).FirstOrDefault() : null;
                    if (origDiscEntity != null)
                    {
                        origDiscEntity.JournalEntryId = VendorTrans.JournalEntryId;
                        origDiscEntity.AccountId = dAccID;
                        origDiscEntity.Amount = discAmount;
                        origDiscEntity.Memo = disc.DiscountMemo;
                        origDiscEntity.TransactionDate = DateTime.Now;
                        //origDiscEntity.ReferenceNumber = VendorTrans.ReferenceNumber;
                        origDiscEntity.SourceId = billJournalEntryID;

                        tlist.ItemsTobeUpdated.Add(origDiscEntity);

                    }
                    else
                    {
                        origDiscEntity = new Transaction();
                        origDiscEntity.Id = new PFAID().UID;
                        origDiscEntity.AccountId = dAccID;
                        origDiscEntity.JournalEntryId = VendorTrans.JournalEntryId;
                        origDiscEntity.ParentId = VendorTrans.Id;
                        origDiscEntity.Amount = discAmount;
                        origDiscEntity.Memo = disc.DiscountMemo;
                        origDiscEntity.DebitCredit = Convert.ToBoolean(TransactionDebitCredit.Credit);
                        origDiscEntity.TransactionDate = DateTime.Now;
                        //origDiscEntity.ReferenceNumber = checkno;
                        origDiscEntity.Status = (short)Status.Active;
                        origDiscEntity.SourceId = billJournalEntryID;
                        origDiscEntity.SourceType = (short)TransactionSourceType.Discount;
                        i = i + 1;
                        origDiscEntity.Order = (short?)i;
                        tlist.ItemsTobeInserted.Add(origDiscEntity);
                    }

                }
            }



            return tlist;
        }
        public static BillEntryPayments MapSplitBillEntryPaymentUpdatedTransaction(BillPaymentDivisions entity, BillEntryInformation bei, byte[] billPayID, BillEntryPayments? origEntity = null)
        {
            if (origEntity != null)
            {
                origEntity.PaidAmount = entity.AmountPaid;
                // origEntity.OutStanding = entity.AmountDue - entity.AmountPaid - entity.DiscountAmount;
                origEntity.AdjustmentAmount = 0;
                origEntity.Status = (short)TransactionStatus.Active;
                if (!entity.IsDebitMemo && entity.DiscountAmount > 0)
                {
                    origEntity.DiscountAccountId = new PFAID(entity.DiscountAccount).UID;
                    origEntity.Discount = entity.DiscountAmount;
                    origEntity.DiscountMemo = !string.IsNullOrEmpty(entity.DiscountMemo) ? entity.DiscountMemo : string.Empty;
                }
                else
                    origEntity.Discount = 0;
            }
            else
            {
                origEntity = new BillEntryPayments();

                origEntity.SourceId = new PFAID(entity.PayeeID).UID;
                origEntity.BillInformationId = bei.Id;
                origEntity.PaidAmount = entity.AmountPaid;
                origEntity.OutStanding = entity.AmountDue - entity.AmountPaid - entity.DiscountAmount;
                origEntity.AdjustmentAmount = 0;
                origEntity.Status = (short)TransactionStatus.Active;
                origEntity.Pcid = string.IsNullOrWhiteSpace(entity.StoreID)?null: new PFAID(entity.StoreID).UID;
                origEntity.BillPaymentId = billPayID;
                if (!entity.IsDebitMemo && entity.DiscountAmount > 0)
                {
                    origEntity.Discount = entity.DiscountAmount;
                    origEntity.DiscountAccountId = new PFAID(entity.DiscountAccount).UID;
                    origEntity.DiscountMemo = !string.IsNullOrEmpty(entity.DiscountMemo) ? entity.DiscountMemo : string.Empty;
                }
                else
                    origEntity.Discount = 0;
            }
            return origEntity;
        }
        //public static DeptConfig MapDeptConfig(DepartmentRequest Entity,string ClientId,DeptConfig OrigEntity = null)
        //       {
        //           if (OrigEntity != null)
        //           {
        //               OrigEntity.ClientId = new PFAID(ClientId).UID;
        //               OrigEntity.DeptName = Entity.DepartmentName;
        //               OrigEntity.DeptType = Convert.ToInt16((DepartmentTypeEnum)Entity.ApplicabletoDailysales);
        //               OrigEntity.DeptOrder = Entity.Order;
        //               OrigEntity.Status = Convert.ToByte(Status.Active);
        //           }
        //           else
        //           {
        //               OrigEntity = new DeptConfig
        //               {
        //                   DeptName = Entity.DepartmentName,
        //                   DeptType= Entity.ApplicabletoDailysales!=null?Entity.ApplicabletoDailysales:(short)DepartmentTypeEnum.NotApplicable,
        //                   DeptOrder = Entity.Order,
        //                   ClientId=new PFAID(ClientId).UID,
        //                   Status = Convert.ToByte(Status.Active)
        //               };
        //           }
        //           return OrigEntity;
        //       }
        #endregion DepartmentMaster

        #region 1099setup Mapper

        public static _1099miscExcludeSettings MapExcludeSettings(ExcludeSettingsSaveReq Entity, _1099miscExcludeSettings OrigEntity = null)
        {
            if (OrigEntity != null)
            {
                OrigEntity.ClientId = new PFAID(Entity.ClientID).UID;
                OrigEntity.Name = Entity.Name;
                OrigEntity.Status = Entity.Status;
            }
            else
            {
                OrigEntity = new _1099miscExcludeSettings
                {

                    ClientId = new PFAID(Entity.ClientID).UID,
                    Name = Entity.Name,
                    Status = Convert.ToByte(Status.Active)
                };
            }
            return OrigEntity;
        }


        public static _1099changeMappingTempTable MapChangeMappingsTemp(COAMappingwithBoxLines Entity, _1099changeMappingTempTable OrigEntity = null)
        {
            if (OrigEntity != null)
            {
                //OrigEntity.Id = Entity.id;

                OrigEntity.ReqId = Entity.ReqID;
                OrigEntity.VendorId = new PFAID(Entity.VendorID).UID;
                OrigEntity.BoxLineId = Entity.BoxLineID != 0 ? Entity.BoxLineID : 1;
                OrigEntity.AccountId = new PFAID(Entity.COA).UID;
                OrigEntity.JournalEntryId = null;


            }
            else
            {
                OrigEntity = new _1099changeMappingTempTable
                {

                    ReqId = Entity.ReqID,
                    VendorId = new PFAID(Entity.VendorID).UID,
                    BoxLineId = Entity.BoxLineID != 0 ? Entity.BoxLineID : 1,
                    AccountId = new PFAID(Entity.COA).UID,

                };
            }
            return OrigEntity;
        }

        #endregion

    }
}
