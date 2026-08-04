using BankFeed.App.Contracts;
using BankFeed.Infra.DataRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankFeed.Domain.DataModel;
using BankFeed.Domain.DTO.Model;
using BankFeed.Domain.DTO.Req;
using BankFeed.Domain.DTO.Resp;
using BankFeed.Domain.Enums;
using Common.Domain.DTO.Enums;
using Microsoft.Identity.Client;
using System.Net;
using BankFeed.Infra.DBCon;
using BankFeed.App.Services;
//using Azure;
using Microsoft.AspNetCore.Http;
using System.Collections;
using DataModel.Domain.DataModel;
using Common.Domain.DTO.App;
using CoreAccounting.Infra;
using CoreAccounting.App;
using System.Data;
using CoreAccounting.App.Contracts;
using Azure.Core;
using CoreAccounting.Domain.DTO.Req;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Common.Domain.DTO.Req;
using Common.Domain.DTO.Resp;

namespace AutoSynchFeeds
{
    public class AutoSynchFeed
    {

        #region Ctor     
        private readonly BankFeed.App.Contracts.IUnitOfWork unitOfWork;
        private readonly IFeedAccountService feedAccountService;
        private readonly IPlaidService plaidService;
        private readonly IYodleeService yodleeService;
        private readonly IMeldService meldService;
        private readonly IJournalService journalService;
        private readonly IFeedTransactionService feedTransactionService;
        private readonly IConfiguration configuration;
        private readonly IFeedRuleService feedRuleSrv;
        //private readonly IUnitOfWork uow;
        private readonly IBankFeedPostingService bankFeedPostingService;
        #endregion

        #region Ctor
        public AutoSynchFeed(BankFeed.App.Contracts.IUnitOfWork unitOfWork, IFeedAccountService feedAccountService, IPlaidService plaidService, IYodleeService yodleeService, IMeldService meldService, IJournalService journalService, IFeedTransactionService feedTransactionService, IBankFeedPostingService bankFeedPostingService)
        {
            this.unitOfWork = unitOfWork;
            this.feedAccountService = feedAccountService;
            this.plaidService = plaidService;
            this.yodleeService = yodleeService;
            this.meldService = meldService;
            this.journalService = journalService;
            this.feedTransactionService = feedTransactionService;
            this.bankFeedPostingService = bankFeedPostingService;
        }
        #endregion

        /// <summary>
        /// Get Active Accounts
        /// </summary>
        /// <param name="autoSynchTime"></param>
        /// <returns>Plaid and Yodlee Active Accounts</returns>
        public async Task<PossibleMatchResponse> AutoSynchFeeds(string env)
        {
            var autoSynchTime = "10";// DateTime.UtcNow.ToString("HH");
            ConnectionStatus resPlaid, resYodlee, resMeld = null;
            ConnectionRequest placonnreq, meldconnreq = null;
            YodleeConnectionReqDTO yodconnreq = null;
            PossibleMatchResponse res = null;
            List<FeedTransactions> feedtrans = null;
            FeedSettings feedSett = null;
            ActiveFeedAccountDTO ClientID = null;
            List<long> feedIds = null;
            ActiveFeedAccountResponse accounts = null;
            ActiveFeedAccountDTO accessToken = null;
            List<ActiveFeedAccountDTO> PlaidbankAccountIDs, YodleebankAccountIDs, MeldbankAccountIDs = null;
            byte[] feedaccmapid = null;
            string defaultAccountID = "363642453536374145434244343632413430";
            try
            {
                //string clientID = (string)HttpContext.Items["ClientId"];

                //string userId = (string)HttpContext.Items["UserId"];

                accounts = new ActiveFeedAccountResponse();
                accessToken = new ActiveFeedAccountDTO();
                PlaidbankAccountIDs = new List<ActiveFeedAccountDTO>();
                YodleebankAccountIDs = new List<ActiveFeedAccountDTO>();
                //Get Acctive FeedAccounts with accessToken and InstID
                  accounts = await feedAccountService.GetActiveFeedAccounts(autoSynchTime); 
                if (accounts != null )
                {
                    // accessToken and InstId info
                    
                    PlaidbankAccountIDs = (from a in accounts.ActiveAccounts
                                           where a.ProviderRegID == 1
                                           select new ActiveFeedAccountDTO()
                                           {
                                               account_id = a.account_id,
                                               ProvicerAccessID = a.ProvicerAccessID,
                                               InstID = a.InstID,
                                               ClientID = a.ClientID,
                                               ClientName = a.ClientName,

                                           }).ToList();
                    YodleebankAccountIDs = (from a in accounts.ActiveAccounts
                                            where a.ProviderRegID == 2
                                            select new ActiveFeedAccountDTO()
                                            {
                                                account_id = a.account_id,
                                                ProvicerAccessID = a.ProvicerAccessID,
                                                InstID = a.InstID,
                                                ClientID = a.ClientID,
                                                ClientName = a.ClientName,

                                            }).ToList();
                    MeldbankAccountIDs = (from a in accounts.ActiveAccounts
                                            where a.ProviderRegID == 2
                                            select new ActiveFeedAccountDTO()
                                            {
                                                account_id = a.account_id,
                                                ProvicerAccessID = a.ProvicerAccessID,
                                                InstID = a.InstID,
                                                ClientID = a.ClientID,
                                                ClientName = a.ClientName,

                                            }).ToList();

                    //Post the BankAccountDetails,PlaidStatus and publictoken for getting Transactions,Save Balences and Transaction Related Information
                    if (PlaidbankAccountIDs.Any())
                    {
                        List<long> ids = PlaidbankAccountIDs.Select(s=>s.InstID).Distinct().ToList();
                        foreach(long id in ids)
                        {
                            resPlaid = new ConnectionStatus();
                            placonnreq = new ConnectionRequest();
                            placonnreq.institution_id = id.ToString();
                            placonnreq.ProviderID = Convert.ToInt64(ProviderEnum.AutoSync);
                            List< ActiveFeedAccountDTO> lstPlaidAccounts = PlaidbankAccountIDs.Where(s=>s.InstID== id).ToList();
                            resPlaid = await plaidService.GetTransactionsByAccountID(placonnreq, lstPlaidAccounts, (short)ProviderEnum.AutoSync);
                        }

                    }
                    if (YodleebankAccountIDs.Any())
                    {
                        List<long> ids = YodleebankAccountIDs.Select(s => s.InstID).Distinct().ToList();
                        foreach (long id in ids)
                        {
                            resYodlee = new ConnectionStatus();
                            yodconnreq = new YodleeConnectionReqDTO();
                            yodconnreq.ProviderID = Convert.ToInt64(ProviderEnum.AutoSync);
                            yodconnreq.InsId = id;
                            List<ActiveFeedAccountDTO> lstYodleeAccounts = YodleebankAccountIDs.Where(s => s.InstID == id).ToList();
                            resYodlee = await yodleeService.GetTransactionsByAccountID(yodconnreq, 0, null, (short)ProviderEnum.AutoSync, lstYodleeAccounts);
                        }
                    }
                    if (MeldbankAccountIDs.Any())
                    {
                        List<long> ids = MeldbankAccountIDs.Select(s => s.InstID).Distinct().ToList();
                        foreach (long id in ids)
                        {
                            resMeld = new ConnectionStatus();
                            meldconnreq = new ConnectionRequest();
                            meldconnreq.ProviderID = Convert.ToInt64(ProviderEnum.AutoSync);
                            meldconnreq.institution_id = id.ToString();
                            resMeld = await meldService.GetTransactionsByAccountID(meldconnreq, (short)ProviderEnum.AutoSync, MeldbankAccountIDs.Where(s => s.InstID == id).ToList());
                        }
                    }

                    if (env != "0" && accounts.ActiveAccounts.Count>0)
                    {
                        //ID equals to Feed Account ID
                        feedIds = accounts.ActiveAccounts.Select(s => s.ID).ToList();
                        //Possible and Bill Matches

                        MultipleRuleApplyResponse multipleRuleApplyResponse = new MultipleRuleApplyResponse();
                        MultipleApplyRuleRequest multipleApplyRuleReq = new MultipleApplyRuleRequest();
                        multipleApplyRuleReq.ApplyRuleRequests = new List<ApplyRuleRequest>();
                        foreach (var feedaccid in feedIds)
                        {
                            feedtrans = new List<FeedTransactions>();
                            ClientID = new ActiveFeedAccountDTO();
                            feedSett = new FeedSettings();

                            feedaccmapid = unitOfWork.FeedAccountMappings.GetAll(s => s.Id == feedaccid).Result.Select(x => x.AccountId).FirstOrDefault();
                            feedtrans = unitOfWork.FeedTransactions.GetAll(s => s.FeedAccId == feedaccid).Result.ToList();
                            ClientID = (from a in accounts.ActiveAccounts
                                        where a.ProviderRegID != (short)NimbleProvidersEnum.Import
                                        select new ActiveFeedAccountDTO()
                                        {
                                            ClientID = (a.ClientID).Remove(0,2),
                                        }).FirstOrDefault();
                            feedSett = unitOfWork.FeedSettings.GetAll(s => s.ClientId == new PFAID(ClientID.ClientID).UID).Result.FirstOrDefault();

                            //var Possibleresult = await journalService.PostTransactionsToBankFeed(new TransactionListRequest()
                            //{
                            //    NimbleAccountID = new PFAID(feedaccmapid).ToString(),
                            //    NumberofDays = 0,
                            //    FromDate = feedtrans.Select(e => e.TransDate.Value).Min().AddDays(-feedSett.TransDateRange.Value),
                            //    ToDate = feedtrans.Select(e => e.TransDate.Value).Max(),
                            //    HideReconcile = feedSett.IncludeReconcileTransactions.Value,
                            //    SourceType = 0,
                            //    PageCount = 999,
                            //    PageOffSet = 0
                            //});

                            //if (Possibleresult.StatusCode == StatusCodes.Status200OK && !string.IsNullOrEmpty(Possibleresult.RequestID))
                            //{
                            //    res = new PossibleMatchResponse();
                            //    var response = await feedTransactionService.GetPossibleMatches(new PossibleMatchRequest()
                            //    {
                            //        ClientID = "0x" + ClientID.ClientID,
                            //        RequestID = Possibleresult.RequestID,
                            //        FeedTranIDs = feedtrans.Select(s => s.Id).ToList(),
                            //    });

                            //    res = await feedTransactionService.GetBillMatches(new PossibleMatchRequest()
                            //    {
                            //        ClientID = "0x" + ClientID.ClientID,
                            //        RequestID = Possibleresult.RequestID,
                            //        FeedTranIDs = feedtrans.Select(s => s.Id).ToList()
                            //    });

                            //}


                            FeedTransactionLookUpResponse feedTransRes = null;
                            long insId = 0;
                            if (accounts.ActiveAccounts.ToList().Any())
                                insId = accounts.ActiveAccounts.ToList().Where(a => a.ID == feedaccid).Select(a => a.InstID).FirstOrDefault();
                            if (insId != 0)
                            {

                                feedTransRes = await feedTransactionService.GetFeedAccountTransactionsLookUp(ClientID.ClientID, new FeedTransactionLookupRequest()
                                {
                                    FeedAccountID = feedaccid,
                                    InstitutionID = insId,
                                    IsGetTransactions = true
                                });

                                if (feedTransRes != null && feedTransRes.FeedTransactions != null && feedTransRes.FeedTransactions.Any() && feedTransRes.StatusCode == StatusCodes.Status200OK)
                                {
                                    multipleApplyRuleReq.ApplyRuleRequests.Add(new ApplyRuleRequest()
                                    {
                                        FeedAccountID = feedTransRes.AccountSummary.FeedAccID,
                                        InstutionID = insId,
                                        NimbleAccID = feedTransRes.AccountSummary.NimbleAccID
                                    });
                                }

                            }


                        }
                        if (multipleApplyRuleReq.ApplyRuleRequests.Any())
                        {
                            multipleRuleApplyResponse = await feedTransactionService.MultipleApplyFeedRulesonFeedTransactions(new MultipleApplyRuleRequest()
                            {

                                ApplyRuleRequests = multipleApplyRuleReq.ApplyRuleRequests
                            });

                        }
                        if (multipleRuleApplyResponse.responses.Any())
                        {
                            MultiplePostRequest multiPostReq = new MultiplePostRequest();
                            multiPostReq.requests = new List<SinglePostRequest>();
                            foreach (var applyRes in multipleRuleApplyResponse.responses)
                            {
                                foreach (var t in applyRes.FeedTransaction)
                                {
                                    multiPostReq.requests.Add(new SinglePostRequest()
                                    {
                                        CorporationID = t.CorpID,
                                        FeedTransactionID = t.ID,
                                        NimbleAccountID = t.ParentNimbleAccID,
                                        TransactionPostType = (short)t.TransMapTypeID,
                                        NameID = t.PayeeID,
                                        Memo = t.Description,
                                        EntryDate = t.Date.HasValue ? t.Date.Value : DateTime.Now,
                                        CheckNo = t.CheckNo,
                                        IsMultiplePost = true,
                                        NameType = Convert.ToString(t.PayeeType),
                                        AccountID = !string.IsNullOrEmpty(t.NimbleAccID) ? t.NimbleAccID : defaultAccountID,
                                        AccountTypeID = t.NimAccType,
                                        ClearedDate = Convert.ToDateTime(t.TranClearedDate),
                                        Amount = t.Amount,
                                        TransactionType = Convert.ToInt16(t.TransPostTypeID),
                                        ClientID= ClientID.ClientID
                                    });
                                }
                            }
                            //if (multiPostReq != null && multiPostReq.requests.Any())
                            //    //await bankFeedPostingService.PostingAutoRuleTrans(new MultiplePostRequest()
                            //    //{ requests = multiPostReq.requests });


                        }
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                resPlaid = null;
                resYodlee = null;
                placonnreq = null;
                yodconnreq = null;
                res = null;
                feedtrans = null;
                feedSett = null;
                ClientID = null;
                feedaccmapid = null;
                feedIds = null;
                accounts = null;
                accessToken = null;
                PlaidbankAccountIDs = null;
                YodleebankAccountIDs = null;
            }

        }


    }
}
