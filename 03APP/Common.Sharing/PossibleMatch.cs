using Azure;
using Azure.Core;
using BankFeed.App.Contracts;
using BankFeed.App.Services;
using BankFeed.Domain.DataModel;
using BankFeed.Domain.DTO.Model;
using BankFeed.Domain.DTO.Req;
using BankFeed.Domain.DTO.Resp;
using BankFeed.Infra.DataRepos;
using BankFeed.Infra.DBCon;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Enums;
using Common.Domain.DTO.Req;
using Common.Domain.DTO.Resp;
using CoreAccounting.Domain.DTO.Req;
using CoreAccounting.Domain.DTO.Resp;
using DataModel.Domain.DataModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Sharing
{
    public class PossibleMatch
    {
        #region Fields
        // public readonly IUnitOfWork uow;
        private readonly string connectionString;
        //private readonly IUnitOfWork uow;
        //private readonly ICoreProperty coreProp;
        //private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly IConfiguration configuration;
        #endregion

        #region Ctor
        public PossibleMatch(string dataShareingConstr)
        {
            connectionString = dataShareingConstr;
        }

        #endregion

        public IUnitOfWork CreateContext()
        {
            BankFeedsContext bankFeedsContext;
            var services = new ServiceCollection();
            services.AddDbContext<BankFeedsContext>(options => options.UseSqlServer(connectionString));
            var serviceProvider = services.BuildServiceProvider();
            bankFeedsContext = serviceProvider.GetService<BankFeedsContext>();
            IUnitOfWork unitOfWork = new UnitOfWork(bankFeedsContext);
            return unitOfWork;
        }

        public async Task<string> PossibleMatches(List<TransList> tlist, TransactionListRequest Request,string requestID)
        {
            List<FeedPossibleTransSharing> possibleMatches = new List<FeedPossibleTransSharing>();
            //string posReqID = Guid.NewGuid().ToString();
            try
            {
                var dbContext = CreateContext();
                foreach (var tran in tlist)
                {
                    FeedPossibleTransSharing possibleMatch = new FeedPossibleTransSharing();
                    possibleMatch.ReqId = requestID;
                    possibleMatch.TransDate = tran.Date;
                    possibleMatch.CheckNo = tran.CheckNO;
                    possibleMatch.Description = tran.Description;
                    possibleMatch.PayeeName = tran.PayeeName;
                    possibleMatch.Amount = tran.Amount;
                    possibleMatch.TransactionTypeName = tran.TransactionTypeName;
                    possibleMatch.TransactionType = tran.TransactionType;
                    possibleMatch.DebitCredit = tran.DebitCredit;
                    possibleMatch.IsDailySale = tran.IsDailySale;
                    possibleMatch.TransId = tran.TransactionID;
                    possibleMatch.JournalEntryId = tran.JournalEntryID;
                    possibleMatch.BillNum = tran.TransactionType==21? tran.BillNum:null;
                    possibleMatch.BillDate = tran.TransactionType == 21 ? tran.BillDate:null;
                    possibleMatch.DueDate = tran.TransactionType == 21 ? tran.DueDate : null;
                    possibleMatch.AdjustmentLinkId = tran.AdjustmentLinkID;
                    possibleMatch.IsAdjEntry = tran.IsAdjEntry;
                    possibleMatch.MergRefId = tran.MergeRefID;
                    possibleMatch.MergeLineId = tran.MergeLineId;
                    possibleMatches.Add(possibleMatch);
                }
                await dbContext.FeedPossibleTransSharings.BulkInsert(possibleMatches);
                int c = await dbContext.SaveAsync();
               
                return null;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                possibleMatches = null;
            }
        }

        public async Task<List<TransactionDetailRespone>> GetTransactionDetailsBased(List<long?> FeedTransIDs)
        {
            try
            {
                if (FeedTransIDs.Any())
                {
                    List<TransactionDetailRespone> res = new List<TransactionDetailRespone>();
                    var dbContext = CreateContext();

                    var getFeedTransactionMap = (await dbContext.FeedTransactionMappings.GetAll(x => FeedTransIDs.Contains(x.FeedTransId) && x.Status!=(int)Status.Delete)).ToList();
                    if (getFeedTransactionMap != null && getFeedTransactionMap.Any())
                    {
                        foreach (var item in getFeedTransactionMap)
                        {
                            TransactionDetailRespone detail = new TransactionDetailRespone();
                            detail.TransactionID = item.TransactionId;
                            detail.FeedTransactionID = item?.FeedTransId ?? 0;
                            res.Add(detail);
                        }
                        return res;
                    }
                }
                return new List<TransactionDetailRespone>();
            }
            catch(Exception ex)
            {
                throw;
            }
        }


        public async Task<string> PossibleMatches(List<TransList> tlist)
        {
            List<FeedPossibleTransSharing> possibleMatches = new List<FeedPossibleTransSharing>();
            string posReqID = Guid.NewGuid().ToString();
            try
            {
                var dbContext = CreateContext();
                foreach (var tran in tlist)
                {
                    FeedPossibleTransSharing possibleMatch = new FeedPossibleTransSharing();
                    possibleMatch.ReqId = posReqID;
                    possibleMatch.TransDate = tran.Date;
                    possibleMatch.CheckNo = tran.CheckNO;
                    possibleMatch.Description = tran.Description;
                    possibleMatch.PayeeName = tran.PayeeName;
                    possibleMatch.Amount = tran.Amount;
                    possibleMatch.TransactionTypeName = tran.TransactionTypeName;
                    possibleMatch.TransactionType = tran.TransactionType;
                    possibleMatch.DebitCredit = tran.DebitCredit;
                    possibleMatch.IsDailySale = tran.IsDailySale;
                    possibleMatch.TransId = tran.TransactionID;
                    possibleMatch.JournalEntryId = tran.JournalEntryID;
                    possibleMatch.BillNum = tran.BillNum;
                    possibleMatch.BillDate = tran.BillDate;
                    possibleMatch.DueDate = tran.DueDate;

                    possibleMatches.Add(possibleMatch);
                }
                await dbContext.FeedPossibleTransSharings.BulkInsert(possibleMatches);
                int c = await dbContext.SaveAsync();
                if (c > 0)
                {
                    return posReqID;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                possibleMatches = null;
            }
        }
        public async Task<FeedTranResponse> GetAutoRuleTrans(LoadByLongIDRequest Request)
        {
            FeedTranResponse response = new FeedTranResponse();
            List<FeedTransactionandMappingDTO> TransList = new List<FeedTransactionandMappingDTO>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                var dbContext = CreateContext();

                sqlParamList.Add(new SqlParameter("@FeedAccID", Request.ID));
                TransList = await dbContext.ExecuteSqlCommand<FeedTransactionandMappingDTO>(sqlParamList, "usp_GetAutoRulleTransactions");

                response.TransList = (from item in TransList
                                      select new FeedTransactionandMappingDTO()
                                      {
                                          FeedTransactionID = item.ID,
                                          NimbleAccountID = item.NimbleAccountID,
                                          TransactionPostType = item.TransactionPostType,
                                          AccountID = item.AccountID,
                                          NameID = item.NameID,
                                          NameType = item.NameType,
                                          CheckNo = item.CheckNo,
                                          Memo = item.Memo,
                                          EntryDate = item.EntryDate,
                                          Amount = item.Amount,
                                          TransactionType = item.TransactionType

                                      }).ToList();
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { sqlParamList = null; }
        }

        public async Task<string> FeedTranSharing(MultiplePostResponse Request)
        {
            List<FeedTransSharing> FeedTranSharing = new List<FeedTransSharing>();
            string posReqID = Guid.NewGuid().ToString();
            try
            {
                var dbContext = CreateContext();
                if (Request.StatusCode == StatusCodes.Status200OK)
                {
                    
                    foreach (var tran in Request.responses)
                    {
                        FeedTransSharing tranSharing = new FeedTransSharing();
                        tranSharing.ReqId = posReqID;
                        tranSharing.EntryDate = tran.EntryDate;
                        tranSharing.RefNumber = tran.CheckNO;
                        tranSharing.Description = tran.Description;
                        tranSharing.NameId = new PFAID(tran.NameID).UID;
                        tranSharing.Amount = tran.Amount;
                        tranSharing.TransactionType = tran.TransactionType;
                        tranSharing.JournalEntryId = new PFAID(tran.JournalID).UID;
                        tranSharing.TransactionId = new PFAID(tran.TransactionID).UID;
                        tranSharing.AccountId = new PFAID(tran.NimbleAccountID).UID;
                        tranSharing.NameType = Convert.ToInt16(tran.NameType);
                        tranSharing.FeedTransId = tran.FeedTransactionID;
                        tranSharing.Pcid = new PFAID(tran.ProfitCenterID).UID;
                        FeedTranSharing.Add(tranSharing);
                    }
                    dbContext.FeedTranscationSharing.BulkInsert(FeedTranSharing);

                }
                int c = await dbContext.SaveAsync();

                if (c > 0)
                {
                    return posReqID;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                FeedTranSharing = null;
            }
        }

        public async Task<bool> PostTransFromSharing(string Request)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                var dbContext = CreateContext();

                sqlParamList.Add(new SqlParameter("@ReqID", Request));
                 var res= await dbContext.ExecuteSqlNonQueryCommand(sqlParamList, "usp_PostingMultipleTrans");
                if (res > 0)//int res
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
            finally
            {
                sqlParamList = null;
            }

        }

        
        public async Task<bool> RemoveFeedTranSharing(string ReqID)
        {
            List<FeedTransSharing> removedFeedTranSharing = new List<FeedTransSharing>();
            try
            {
                var dbContext = CreateContext();
                if(ReqID!=null)
                {
                    removedFeedTranSharing=(await dbContext.FeedTranscationSharing.GetAll(e=>e.ReqId==ReqID)).ToList();
                }

                dbContext.FeedTranscationSharing.BulkDelete(removedFeedTranSharing);
                int c = await dbContext.SaveAsync();
                if (c > 0)
                  return true;
                else
                    return false;
               
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                removedFeedTranSharing = null;
            }
        }
    }
}