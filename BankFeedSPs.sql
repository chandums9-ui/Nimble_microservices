
CREATE PROCEDURE [dbo].[Nimble_BillMatches]
@ReqID varchar(200),
@feedtransIDs varchar(MAX),
@ClientID varchar(100)

AS 
BEGIN

DECLARE @ClientBinID AS BINARY(18) = CONVERT(BINARY(18),@ClientID,1)
--get admin prefrences
DECLARE @MatchesBasedOn smallint,@DateRange int,@IncludeReconsile smallint
select  @MatchesBasedOn=PosibleMatchBasedOn,@DateRange=TransDateRange,@IncludeReconsile=IncludeReconcileTransactions FROM FeedSettings WHERE ClientID=@ClientBinID and [Status]<>3

DECLARE @TransIDsList TABLE (
	[ID] int Identity(1,1),
    [TranID] VARCHAR(255) not null
);
INSERT INTO @TransIDsList ([TranID])
SELECT value
FROM STRING_SPLIT(@feedtransIDs, ',');

declare @index smallint=1
declare @NoOfIds int
select @NoOfIds=COUNT(TranID) from @TransIDsList
declare @tranID bigint;
	while @index<=@NoOfIds
	BEGIN
		--for avoiding duplicate matches
		--delete FeedPossibleTrans_Sharing where CONVERT(BINARY(18),TransID,1) in (select TransactionID from FeedTransactionPossibleMatches where Status<>3)

	select @tranID=TRY_CONVERT(bigint,TranID) from @TransIDsList where ID=@index
	IF OBJECT_ID('tempdb..#TempMatchedTrans') IS NOT NULL DROP TABLE #TempMatchedTrans
	CREATE TABLE #TempMatchedTrans
	(
		[ID] [bigint]  NOT NULL,
		[ReqID] [varchar](100) NULL,
		[TransDate] [datetime2](7) NULL,
		[CheckNO] [varchar](100) NULL,
		[Description] [varchar](100) NULL,
		[PayeeName] [varchar](100) NULL,
		[Amount] [decimal](18, 2) NULL,
		[TransactionTypeName] [varchar](100) NULL,
		[TransactionType] [tinyint] NULL,
		[IsDailySale] [bit] NULL,
		[TransID] [varchar](50) NULL,
		[JournalEntryID] [varchar](50) NULL,
		[MatchLevel] smallint
	)

	insert into #TempMatchedTrans
	select Trans.ID,tranShare.ReqID,tranShare.TransDate,tranShare.CheckNO,tranShare.[Description],tranShare.PayeeName,tranShare.Amount,tranShare.TransactionTypeName,tranShare.TransactionType,tranShare.IsDailySale,tranShare.TransID,tranShare.JournalEntryID,1 
	from FeedPossibleTrans_Sharing tranShare inner join FeedTransactions Trans on tranShare.Amount=Trans.Amount
	--inner join FeedPossibleTrans_Sharing tranShareA on tranShareA.TransactionTypeName='Bill' and tranShareA.Amount=tranShare.Amount
	where Trans.ID=@tranID AND tranShare.ReqID=@ReqID AND Trans.TransPostType = 0 and tranShare.TransactionTypeName='Bill Payment'
		
		IF (SELECT COUNT(*) FROM #TempMatchedTrans)>0
		BEGIN

		IF OBJECT_ID('tempdb..#TempNextMatch') IS NOT NULL DROP TABLE #TempNextMatch
		CREATE TABLE #TempNextMatch
		(
			[ID] [bigint]  NOT NULL,
			[ReqID] [varchar](100) NULL,
			[TransDate] [datetime2](7) NULL,
			[CheckNO] [varchar](100) NULL,
			[Description] [varchar](100) NULL,
			[PayeeName] [varchar](100) NULL,
			[Amount] [decimal](18, 2) NULL,
			[TransactionTypeName] [varchar](100) NULL,
			[TransactionType] [tinyint] NULL,
			[IsDailySale] [bit] NULL,
			[TransID] [varchar](50) NULL,
			[JournalEntryID] [varchar](50) NULL,
			[MatchLevel] smallint
		)
				
		--filter transactions based on Description
		insert into #TempNextMatch 
		select feedTrans.ID,tranShare.ReqID,tranShare.TransDate,tranShare.CheckNO,tranShare.[Description],tranShare.PayeeName,tranShare.Amount,tranShare.TransactionTypeName,tranShare.TransactionType,tranShare.IsDailySale,tranShare.TransID,tranShare.JournalEntryID,2
		from #TempMatchedTrans tranShare inner join FeedTransactions  feedTrans 
		on tranShare.[Description]=feedTrans.Description
		WHERE feedTrans.ID=@tranID

		update #TempMatchedTrans set [MatchLevel]=2  where JournalEntryID in (select JournalEntryID from #TempNextMatch)
		delete #TempNextMatch

		--set transaction status to Bill Matched
		update FeedTransactions set [Status]=6 where ID in (select ID from #TempMatchedTrans)	

		--filter transactions based on Name
		insert into #TempNextMatch 
		select feedTrans.ID,tranShare.ReqID,tranShare.TransDate,tranShare.CheckNO,tranShare.[Description],tranShare.PayeeName,tranShare.Amount,tranShare.TransactionTypeName,tranShare.TransactionType,tranShare.IsDailySale,tranShare.TransID,tranShare.JournalEntryID,3
		from #TempMatchedTrans tranShare inner join FeedTransactions  feedTrans 
		on tranShare.PayeeName=feedTrans.TransName
		WHERE feedTrans.ID=@tranID
			
		update #TempMatchedTrans set [MatchLevel]=3  where JournalEntryID in (select JournalEntryID from #TempNextMatch)
		delete #TempNextMatch
		
		--most possible match insert with status 2
		declare @MostPossibleMatchTranID varchar(100)
		set @MostPossibleMatchTranID= ( select top(1)TransID from #TempMatchedTrans order by MatchLevel DESC)
			
		update FeedTransactionPossibleMatches set Status=3 where FeedTransID=@tranID

		insert into FeedTransactionPossibleMatches
		select @tranID, CONVERT(BINARY(18),JournalEntryID,1) ,CONVERT(BINARY(18),TransID,1),2,TransDate,CheckNO,Description,Amount,TransactionTypeName,PayeeName from #TempMatchedTrans where TransID=@MostPossibleMatchTranID
		
		delete  #TempMatchedTrans  where TransID=@MostPossibleMatchTranID

			--based on client prefrences FIFO/LIFO
			IF @MatchesBasedOn=0
			BEGIN
				insert into FeedTransactionPossibleMatches
				select @tranID, CONVERT(BINARY(18),JournalEntryID,1) ,CONVERT(BINARY(18),TransID,1),1,TransDate,CheckNO,Description,Amount,TransactionTypeName,PayeeName from #TempMatchedTrans order by TransDate ASC -- for FIFO
			END
			ELSE
			BEGIN
				insert into FeedTransactionPossibleMatches
				select @tranID, CONVERT(BINARY(18),JournalEntryID,1) ,CONVERT(BINARY(18),TransID,1),1,TransDate,CheckNO,Description,Amount,TransactionTypeName,PayeeName from #TempMatchedTrans order by TransDate DESC -- for LIFO
			END
			
			drop table #TempNextMatch
		END

	SET @index=@index+1
	END

--Output
select * from FeedTransactionPossibleMatches where FeedTransID in (select ID from @TransIDsList)

--deleting Possible matches
--delete FeedTransactionPossibleMatches where FeedTransID in (select Convert(bigint,TranID) from @TransIDsList)
--deleting Nimble sharing transactions 
--delete FeedPossibleTrans_Sharing where ReqID =@ReqID
drop table #TempMatchedTrans

END



CREATE PROCEDURE [dbo].[Nimble_PossibleMatches]

@ReqID varchar(200),
@feedtransIDs varchar(MAX),
@ClientID varchar(100)

 as 
 Begin
 DECLARE @ClientBinID AS BINARY(18) = CONVERT(BINARY(18),@ClientID,1)
 DECLARE @TransIDsList TABLE (
	[ID] int Identity(1,1),
    [TranID] VARCHAR(255) not null
)
INSERT INTO @TransIDsList ([TranID])
SELECT value
FROM STRING_SPLIT(@feedtransIDs, ',');

declare @index smallint=1
declare @NoOfIds int
select @NoOfIds=COUNT(TranID) from @TransIDsList
declare @feedtranID bigint;
while @index<=@NoOfIds
BEGIN

	select @feedtranID=TRY_CONVERT(bigint,[TranID]) from @TransIDsList where ID=@index

	--for avaoiding duplicate matches
	--delete FeedPossibleTrans_Sharing where CONVERT(BINARY(18),TransID,1) in (select TransactionID from FeedTransactionPossibleMatches where Status<>3)

	 --tables to store matched transactions based on criteria
	IF OBJECT_ID('tempdb..#TempMatchedTrans') IS NOT NULL DROP TABLE #TempMatchedTrans
	CREATE TABLE #TempMatchedTrans
	(
		[ID] [bigint]  NOT NULL,
		[ReqID] [varchar](100) NULL,
		[TransDate] [datetime2](7) NULL,
		[CheckNO] [varchar](100) NULL,
		[Description] [varchar](100) NULL,
		[PayeeName] [varchar](100) NULL,
		[Amount] [decimal](18, 2) NULL,
		[TransactionTypeName] [varchar](100) NULL,
		[TransactionType] [tinyint] NULL,
		[IsDailySale] [bit] NULL,
		[TransID] [varchar](50) NULL,
		[JournalEntryID] [varchar](50) NULL,
		[MatchLevel] smallint
	)
	--get admin preferences
	Declare @MatchesBasedOn smallint,@DateRange int
	select  @MatchesBasedOn=PosibleMatchBasedOn,@DateRange=TransDateRange FROM FeedSettings WHERE ClientID=@ClientBinID and [Status]<>3
	
	--filter transactions based on Amount
	insert into #TempMatchedTrans 
	select feedTrans.ID,tranShare.ReqID,tranShare.TransDate,tranShare.CheckNO,tranShare.[Description],tranShare.PayeeName,tranShare.Amount,tranShare.TransactionTypeName,tranShare.TransactionType,tranShare.IsDailySale,tranShare.TransID,tranShare.JournalEntryID,1
	from FeedPossibleTrans_Sharing tranShare inner join FeedTransactions  feedTrans on tranShare.Amount=feedTrans.Amount
	where  feedTrans.ID=@feedtranID and tranShare.ReqID=@ReqID AND tranShare.TransactionType=feedTrans.TransPostType  AND tranShare.TransactionTypeName not in ('Bill','Bill Payment','Bill Payment Void')

	IF (SELECT COUNT(*) FROM #TempMatchedTrans)>0
	BEGIN
	
		IF OBJECT_ID('tempdb..#TempNextMatch') IS NOT NULL DROP TABLE #TempNextMatch
		CREATE TABLE #TempNextMatch
(
	[ID] [bigint]  NOT NULL,
	[ReqID] [varchar](100) NULL,
	[TransDate] [datetime2](7) NULL,
	[CheckNO] [varchar](100) NULL,
	[Description] [varchar](100) NULL,
	[PayeeName] [varchar](100) NULL,
	[Amount] [decimal](18, 2) NULL,
	[TransactionTypeName] [varchar](100) NULL,
	[TransactionType] [tinyint] NULL,
	[IsDailySale] [bit] NULL,
	[TransID] [varchar](50) NULL,
	[JournalEntryID] [varchar](50) NULL,
	[MatchLevel] smallint
)
				
			--filter transactions based on Description
		insert into #TempNextMatch 
		select feedTrans.ID,tranShare.ReqID,tranShare.TransDate,tranShare.CheckNO,tranShare.[Description],tranShare.PayeeName,tranShare.Amount,tranShare.TransactionTypeName,tranShare.TransactionType,tranShare.IsDailySale,tranShare.TransID,tranShare.JournalEntryID,2  
		from #TempMatchedTrans tranShare inner join FeedTransactions  feedTrans 
		on tranShare.[Description]=feedTrans.Description
		WHERE feedTrans.ID=@feedtranID
			
			update #TempMatchedTrans set [MatchLevel]=2  where JournalEntryID in (select JournalEntryID from #TempNextMatch)
			delete #TempNextMatch
			
			--filter transactions based on Name
			insert into #TempNextMatch 
		select feedTrans.ID,tranShare.ReqID,tranShare.TransDate,tranShare.CheckNO,tranShare.[Description],tranShare.PayeeName,tranShare.Amount,tranShare.TransactionTypeName,tranShare.TransactionType,tranShare.IsDailySale,tranShare.TransID,tranShare.JournalEntryID,3
		from #TempMatchedTrans tranShare inner join FeedTransactions  feedTrans 
		on tranShare.PayeeName=feedTrans.TransName
		WHERE feedTrans.ID=@feedtranID
			
			update #TempMatchedTrans set [MatchLevel]=3  where JournalEntryID in (select JournalEntryID from #TempNextMatch)
			delete #TempNextMatch
		
			--filter transactions based on Check No
			insert into #TempNextMatch 
		select feedTrans.ID,tranShare.ReqID,tranShare.TransDate,tranShare.CheckNO,tranShare.[Description],tranShare.PayeeName,tranShare.Amount,tranShare.TransactionTypeName,tranShare.TransactionType,tranShare.IsDailySale,tranShare.TransID,tranShare.JournalEntryID,4
		from #TempMatchedTrans tranShare inner join FeedTransactions  feedTrans 
		on tranShare.CheckNO=feedTrans.CheckNumber 
		where feedTrans.ID=@feedtranID
			
			update #TempMatchedTrans set [MatchLevel]=4 where JournalEntryID in (select JournalEntryID from #TempNextMatch)
			delete #TempNextMatch
		
			--most possible match insert with status 2
			declare @MostPossibleMatchTranID varchar(100)
			set @MostPossibleMatchTranID= ( select top(1)TransID from #TempMatchedTrans order by MatchLevel DESC)
			
			update FeedTransactionPossibleMatches set Status=3 where FeedTransID=@feedtranID

			insert into FeedTransactionPossibleMatches
			select @feedtranID, CONVERT(BINARY(18),JournalEntryID,1) ,CONVERT(BINARY(18),TransID,1),2,TransDate,CheckNO,Description,Amount,TransactionTypeName,PayeeName from #TempMatchedTrans where TransID=@MostPossibleMatchTranID
			
			--set transaction status to Matched
			update FeedTransactions set [Status]=4 where ID in (select ID from #TempMatchedTrans)


			delete  #TempMatchedTrans  where TransID=@MostPossibleMatchTranID
			
		--based on client prefrences FIFO/LIFO
		IF @MatchesBasedOn=0
		BEGIN
			insert into FeedTransactionPossibleMatches
			select @feedtranID, CONVERT(BINARY(18),JournalEntryID,1) ,CONVERT(BINARY(18),TransID,1) ,1,TransDate,CheckNO,Description,Amount,TransactionTypeName,PayeeName from #TempMatchedTrans order by TransDate ASC -- for FIFO
		END
		ELSE
		BEGIN
			insert into FeedTransactionPossibleMatches 
			select @feedtranID, CONVERT(BINARY(18),JournalEntryID,1) ,CONVERT(BINARY(18),TransID,1),1,TransDate,CheckNO,Description,Amount,TransactionTypeName,PayeeName from #TempMatchedTrans order by TransDate DESC -- for LIFO
		END
		drop table #TempNextMatch
	END
	SET @index=@index+1
END

--output
select * from FeedTransactionPossibleMatches where FeedTransID in (select ID from #TempMatchedTrans)
--deleting Possible matches
--delete FeedTransactionPossibleMatches where FeedTransID in (select Convert(bigint,TranID) from @TransIDsList)
--deleting Nimble sharing transactions 
--delete FeedPossibleTrans_Sharing where ReqID =@ReqID
drop table #TempMatchedTrans

END


--------------------------------------------------------------------------------------------------------------------------------------------------------
--------------- Clone Feed Rule ---------------------------------

CREATE PROCEDURE [dbo].[Nimble_CloneRule] 
	(	
	@RuleID BIGINT,
	@CorporationID VARCHAR(50),
	@FeedAccountID BIGINT,
	@Priority INT
	)
--exec [Nimble_CloneRule] 28,'0x29DFA629CD79ADAB4BDB58E0CA77B5D50000',1,2
AS
BEGIN
	DECLARE @cnt BIGINT=0,@index BIGINT=1,@MaxRuleID  BIGINT=0
	DECLARE @CorpBinID AS BINARY(18) = CONVERT(BINARY(18),@CorporationID,1)
	IF OBJECT_ID('tempdb..#tmpAccounts') IS NOT NULL DROP TABLE #tmpAccounts  
	CREATE TABLE #tmpAccounts  
		(  
		 Num BIGINT identity(1,1),  
		 FeedAccountID BIGINT
		)  
	  
	INSERT INTO #tmpAccounts(FeedAccountID)  
	SELECT  A.ID FROM FeedAccount A left join FeedAccountMapping fm on A.ID=fm.ID 
	WHERE A.ID<>@FeedAccountID AND A.[Status]<>3 AND fm.CorpID=@CorpBinID 
	SELECT @cnt=Count(Num) FROM #tmpAccounts
	 
	IF OBJECT_ID('tempdb..#tmpRules') IS NOT NULL DROP TABLE #tmpRules
	CREATE TABLE #tmpRules  
	(  
	[ID] [bigint]  NOT NULL,
	[FeedAccID] [bigint] NULL,
	[SetUpFor] [varchar](2000) NULL,
	[Memo] [varchar](2000) NULL,
	[AutoApplyEnable] [bit] NULL,
	[FeedTransType] [smallint] NULL,
	[RulePriority] [smallint] NULL,
	[QueryMatchType] [smallint] NULL,
	[Status] [smallint] NULL
	)    
	IF OBJECT_ID('tempdb..#tmpRuleDetails') IS NOT NULL DROP TABLE #tmpRuleDetails
	CREATE TABLE #tmpRuleDetails  
	 (  
		[ID] [bigint]  NOT NULL,
		[FeedRuleID] [bigint] NULL,
		[RuleType] [smallint] NULL,
		[FilterType] [smallint] NULL,
		[Val] [varchar](2000) NULL,
		[Status] [smallint] NULL,
	 )
	IF OBJECT_ID('tempdb..#tmpRuleMapping') IS NOT NULL DROP TABLE #tmpRuleMapping
	CREATE TABLE #tmpRuleMapping
	(
	[ID] [bigint] NOT NULL,
	[CorpID] [binary](18) NULL,
	[AccountID] [binary](18) NULL,
	[NameID] [binary](18) NULL,
	[NameType] [smallint] NULL,
	[ActTransType] [smallint] NULL,
	)

	  INSERT INTO #tmpRules 
	  SELECT f.ID,f.FeedAccID,f.SetUpFor,f.Memo,f.AutoApplyEnable,f.FeedTransType,f.RulePriority,f.QueryMatchType,f.[Status]  FROM FeedRule as f left join FeedAccountMapping as fm on f.FeedAccID=fm.ID where fm.CorpID=@CorpBinID AND f.FeedAccID=@FeedAccountID and f.ID=@RuleID
	  
	  INSERT INTO #tmpRuleDetails  
	  SELECT  * FROM FeedRuleDetails WHERE FeedRuleID=@RuleID
	  
	  INSERT INTO #tmpRuleMapping
	  SELECT * FROM FeedRuleMapping WHERE ID=@RuleID
	  
	  BEGIN Tran T1
	
	      While @index<=@cnt 
	      BEGIN
			DECLARE @AccountRuleCount BIGINT=0
			DECLARE @NextAccountID BIGINT;
			SELECT @NextAccountID=FeedAccountID FROM #tmpAccounts WHERE Num=@index
			SELECT @AccountRuleCount=Count(f.ID) FROM FeedRule f left join FeedAccountMapping as Fm on f.ID=Fm.ID WHERE f.[Status]<>3 AND fm.ID=@NextAccountID AND f.RulePriority=@Priority
			IF(@AccountRuleCount>0)
			BEGIN
				SET @index=@index+1;
				Continue;
			END
			INSERT INTO FeedRule(FeedAccID,SetupFor,Memo,AutoApplyEnable,FeedTransType,RulePriority,QueryMatchType,[Status])
			SELECT  @NextAccountID AS MappedAccountID,SetupFor,Memo,AutoApplyEnable,FeedTransType,RulePriority,QueryMatchType,1 AS [Status] FROM #tmpRules 
			SELECT @MaxRuleID=MAX(ID) FROM FeedRule
			
			INSERT INTO FeedRuleDetails(FeedRuleID,RuleType,FilterType,Val,[Status])
			SELECT @MaxRuleID AS RuleID,RuleType,FilterType,Val,1 as [Status] FROM #tmpRuleDetails
			
			INSERT INTO FeedRuleMapping (ID,CorpID,AccountID,NameID,NameType,ActTransType)
			SELECT @MaxRuleID,CorpID,AccountID,NameID,NameType,ActTransType FROM #tmpRuleMapping
				   
			SET @index=@index+1;
		END

	 Commit Tran T1;
	    select 1          
	    Drop Table #tmpAccounts
		Drop Table #tmpRules
	    Drop Table #tmpRuleDetails
		Drop Table #tmpRuleMapping
END