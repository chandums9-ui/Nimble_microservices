--API_Core_GetAccountBalances
Create procedure [dbo].[API_Core_GetAccountBalances]
(
	@CorpID VARCHAR(50),
	@AccountTypeIDs VARCHAR(max)
)
AS
BEGIN
DECLARE @AccIDsList TABLE (
    AccID VARBINARY(18) not null
);
IF(@AccountTypeIDs='000000000000000000000000000000000000')
	BEGIN
	INSERT INTO @AccIDsList (AccID)
	SELECT ID
	FROM AccountType 
	END
ELSE
	BEGIN
	INSERT INTO @AccIDsList (AccID)
	SELECT CONVERT(BINARY(18),TRIM(value),2)
	FROM STRING_SPLIT(@AccountTypeIDs, ',');
	END

DECLARE @CorporationID BINARY(18)
SET @CorporationID = CONVERT(BINARY(18),@CorpID,2)

SELECT CorporationName,GETDATE() as GeneratedTime,COUNT(A.ID) OVER (PARTITION BY C.ID ORDER BY A.ID) AS TotalCount,CONVERT(VARCHAR(50),A.ID,2) AS ID,A.AccountNumber AS Number,
	A.AccountName AS [Name],AT.AccountTypeName,AT.AccountTypeOrder,Isnull( A1.AccountName,a.AccountName) AS ParentAccountName,Isnull( A1.AccountNumber,a.AccountNumber) AS ParentAccountNumber,
	A.OpeningBalance AS Balance       
	FROM Corporation C 
	INNER JOIN Account A ON A.SourceID =C.ID 
	INNER JOIN AccountType AT ON AT.ID=A.AccountTypeID  
	LEFT JOIN Account A1 ON ISNULL(A.parentaccountid,A.ID)=A1.ID 
	WHERE C.ID=@corporationID  AND A1.[Status] <>3 AND A.[Status] <>3 AND A.AccountTypeID IN (SELECT * FROM @AccIDsList)

END
GO
--API_Core_GetAccounts_Updated--
create procedure [dbo].[API_Core_GetAccounts_Updated]
( @CorpID VARCHAR(50),
	@AccountTypeIDs VARCHAR(max)='000000000000000000000000000000000000',
	@IsBalanceRequired BIT=0

	)
AS
/*
EXEC usp_AccountList_API @CorporationID=N'0xC86770B6DDB9AF8A47A02C344BFB555E0000',@AccountTypeID=N'0x0FA700000000000000000000000000000012'
*/


BEGIN
 SET NOCOUNT ON;
DECLARE @corporationID BINARY(18),@AccTypeID BINARY(18) 
	SET @corporationID = CONVERT(BINARY(18),@CorpID,2)


	CREATE TABLE #AccountypeList
	(
		ID BINARY(18)
	)
IF(@AccountTypeIDs='000000000000000000000000000000000000')
	BEGIN
	INSERT INTO #AccountypeList (ID)
	SELECT ID
	FROM AccountType 
	END
ELSE
	BEGIN
	INSERT INTO #AccountypeList (ID)
	SELECT CONVERT(BINARY(18),TRIM(value),2)  AS ID
	FROM STRING_SPLIT(@AccountTypeIDs, ',');
END

DECLARE @CorporationName VARCHAR(250)

DECLARE @ToDate DATE= GETDATE()
DECLARE @ToYear INT= YEAR(@ToDate)
DECLARE @PeriodDateStart DATE
DECLARE @PeriodDateEnd DATE
SELECT @CorporationName=CorporationName,@PeriodDateStart=CONVERT(DATE,PeriodDateStart+'/1900'),@PeriodDateEnd=CONVERT(DATE,PeriodDateEnd+'/1900') FROM Corporation WHERE ID=@CorporationID




CREATE TABLE #CharOfAccounts
(
	RowNo  INT IDENTITY(1,1) PRIMARY KEY,
	ID BINARY(18),
	AccountName VARCHAR(250),
	AccountNumber VARCHAR(100),
	OffAccountName  VARCHAR(100),
	AccountTypeName VARCHAR(250),
	AccountTypeOrder INT,
	AccountTypeID BINARY(18),
	[Status] SMALLINT,
	ParentAccountID BINARY(18),
	AccOrder VARCHAR(2000),
	AccLevel INT,
	ParentAccountName VARCHAR(250),
	ParentAccountNumber VARCHAR(100),
	AccountID1 BINARY(18), 
	AccountID2 BINARY(18), 
	AccountID3 BINARY(18), 
	AccountID4 BINARY(18), 
	Balance DECIMAL(18,2),
)
CREATE TABLE #FilteredCA
(
	RowNo  INT IDENTITY(1,1) PRIMARY KEY,
	ID BINARY(18),
	AccountName VARCHAR(250),
	AccountNumber VARCHAR(100),
	[Status] SMALLINT,
	ParentAccountID BINARY(18),
	AccountTypeID BINARY(18)
	
)


	INSERT INTO #FilteredCA(ID,AccountNumber,AccountName,[Status],ParentAccountID,AccountTypeID)
	SELECT ID,AccountNumber,AccountName,[Status],ParentAccountID,AccountTypeID   FROM Account  WHERE SourceID=@CorporationID AND [Status] NOT IN (3,12) AND  EXISTS (SELECT 1 FROM #AccountypeList A WHERE A.ID=AccountTypeID)


;WITH cteAccounts AS
	(
		SELECT ID,AccountNumber,AccountName,[Status],ParentAccountID,0 AS AccLevel,CONVERT(VARCHAR(2000),ISNULL(AccountNumber,'')) AS AccOrder,AccountTypeID,CAST(NULL AS BINARY(18)) AS AccountID1,CAST(NULL AS BINARY(18)) AS AccountID2,CAST(NULL AS BINARY(18)) AS AccountID3,CAST(NULL AS BINARY(18)) AS AccountID4 FROM #FilteredCA     WHERE  ParentAccountID IS NULL
		UNION ALL
		SELECT A.ID,A.AccountNumber,A.AccountName,A.[Status],A.ParentAccountID,CA.AccLevel+1 AS AccLevel,CONVERT(VARCHAR(2000),(ISNULL(CA.AccountNumber,'')+ISNULL(A.AccountNumber,''))) AS  AccOrder,A.AccountTypeID,(CASE WHEN CA.AccLevel+1=4 THEN CA.ID ELSE CA.AccountID1 END) AS AccountID1,(CASE WHEN CA.AccLevel+1=3 THEN CA.ID ELSE CA.AccountID2 END) AS AccountID2,(CASE WHEN CA.AccLevel+1=2 THEN CA.ID ELSE CA.AccountID3 END) AS AccountID3,(CASE WHEN CA.AccLevel+1=1 THEN CA.ID ELSE CA.AccountID4 END) AS AccountID4  FROM #FilteredCA A INNER JOIN cteAccounts CA ON A.ParentAccountID=CA.ID WHERE A.ParentAccountID IS NOT NULL

	)


	INSERT INTO #CharOfAccounts(ID,AccountNumber,OffAccountName,[Status],ParentAccountID,AccountName,AccOrder,AccLevel,AccountTypeID,AccountID1,AccountID2,AccountID3,AccountID4)
	SELECT ID,AccountNumber,AccountName AS OffAccountName,[Status],ParentAccountID,ISNULL(AccountNumber+'. ','')+(RIGHT(SPACE(AccLevel),50)+AccountName) AS AccountName,AccOrder,AccLevel,AccountTypeID,AccountID1,AccountID2,AccountID3,AccountID4 FROM cteAccounts 




UPDATE CA SET CA.AccountTypeOrder=A.AccountTypeOrder,CA.AccountTypeName=A.AccountTypeName FROM #CharOfAccounts CA INNER JOIN AccountType A ON CA.AccountTypeID=A.ID
UPDATE CA SET CA.ParentAccountName=ISNULL(A.AccountNumber+'. ','')+A.AccountName,CA.ParentAccountNumber=A.AccountNumber FROM #CharOfAccounts CA INNER JOIN Account A ON CA.ParentAccountID=A.ID
WHERE CA.ParentAccountID IS NOT NULL

IF @IsBalanceRequired=1
BEGIN
	CREATE TABLE #AccountBalance
	(
		ID INT IDENTITY(1,1) Primary Key,
		AccountID BINARY(18),
		Balance DECIMAL(18,2)
	)
	IF MONTH(@PeriodDateStart)<=MONTH(@ToDate)
		SET @ToYear=(@ToYear)-1900
	ELSE
		SET @ToYear=(@ToYear-1)-1900
	DECLARE @FromDate DATE=DATEADD(yy,@ToYear,@PeriodDateStart)
	
	DECLARE @AccTypeOrder INT=0
	IF @AccTypeID=0x000000000000000000000000000000000000
	SELECT @AccTypeOrder=ISNULL(AccountTypeOrder,0) FROM AccountType WHERE ID=@AccTypeID
	
	INSERT INTO #AccountBalance(AccountID,Balance)
	SELECT B.AccountID,SUM(Balance) AS Balance FROM 
	(SELECT CASE WHEN A.[Status]=12 THEN A.ParentAccountID ELSE T.AccountID END AS AccountID,CASE WHEN AAT.AccountTypeOrder>=9 AND  AAT.AccountTypeOrder>=18 THEN (CASE WHEN T.DebitCredit=1 THEN Amount ELSE -1*Amount END) ELSE  (CASE WHEN T.DebitCredit=0 THEN Amount ELSE -1*Amount END) END Balance    FROM [Transaction] T INNER JOIN JournalEntry J ON T.[JournalEntryID]=J.ID
	INNER JOIN Account A ON T.AccountID=A.ID
	INNER JOIN AccountType AAT ON A.AccountTypeID=AAT.ID
	WHERE J.CorporationID=@corporationID  AND T.[Status]<>3 AND ISNULL(T.SourceType,-1)<>73 AND J.EntryDate<=@ToDate  AND AAT.AccountTypeOrder<=19 ) B 
	WHERE EXISTS (SELECT 1 FROM #FilteredCA FCA WHERE FCA.ID=B.AccountID)
	GROUP BY AccountID

	IF @AccTypeOrder>=19 OR @AccTypeOrder=0
	BEGIN
		INSERT INTO #AccountBalance(AccountID,Balance)
		SELECT B.AccountID,SUM(Balance) AS Balance FROM 
		(SELECT T.AccountID AS AccountID,CASE WHEN AAT.AccountTypeOrder IN (20,23) AND  AAT.AccountTypeOrder>=18 THEN (CASE WHEN T.DebitCredit=1 THEN Amount ELSE -1*Amount END) ELSE  (CASE WHEN T.DebitCredit=0 THEN Amount ELSE -1*Amount END) END Balance    FROM [Transaction] T INNER JOIN JournalEntry J ON T.[JournalEntryID]=J.ID
		INNER JOIN Account A ON T.AccountID=A.ID
		INNER JOIN AccountType AAT ON A.AccountTypeID=AAT.ID
		WHERE J.CorporationID=@corporationID AND T.[Status]<>3 AND ISNULL(T.SourceType,-1)<>73 AND J.EntryDate>=@FromDate AND J.EntryDate<=@ToDate AND AAT.AccountTypeOrder>=19 AND AAT.AccountTypeOrder<=19) B 
		WHERE EXISTS (SELECT 1 FROM #FilteredCA FCA WHERE FCA.ID=B.AccountID)
		GROUP BY AccountID
	END

	UPDATE CA SET CA.Balance=ISNULL((A.Balance),0) FROM #CharOfAccounts CA LEFT OUTER JOIN #AccountBalance A ON CA.ID=A.AccountID
	DROP TABLE #AccountBalance
END

DECLARE @TotCount INT
SELECT @TotCount=COUNT(RowNo) FROM #CharOfAccounts
SELECT @CorporationName AS CorporationName,GETDATE() AS GeneratedTime,@TotCount AS TotalCount,CONVERT(VARCHAR(50),ID,2) AS ID,AccountNumber AS Number,AccountName AS [Name],AccountTypeName,CONVERT(VARCHAR(50),AccountTypeID,2) AS AccountTypeId,AccountTypeOrder,ParentAccountName,ParentAccountNumber,Status as AccountStatus,ISNULL(Balance,0) AS Balance
FROM #CharOfAccounts ORDER BY AccountTypeOrder,AccountTypeName, AccOrder,AccLevel,AccountName
DROP TABLE #FilteredCA
DROP TABLE #CharOfAccounts
END
Go

--API_Core_GetAccountTypes
CREATE OR ALTER PROCEDURE [dbo].[API_Core_GetAccountTypes] 
 (
  @UserID VARCHAR(50)=NULL
 )
AS
BEGIN
	SELECT DISTINCT ID,[Name] INTO #AccountTypeList FROM (
	SELECT CONVERT(VARCHAR(50),AT.ID,2) AS ID,AT.AccountTypeName AS [Name]
	FROM  AccountType AT WHERE AT.[Status] <>3)Corps
		
		SELECT ID,[Name],row_number() over (order by (select NULL)) AS SortOrder FROM #AccountTypeList ORDER BY ID, [Name] ASC

DROP TABLE #AccountTypeList

END
GO

--API_Core_GetAccounts
CREATE procedure [dbo].[API_Core_GetAccounts]
( @CorpID VARCHAR(50),
	@AccountTypeID VARCHAR(50)='000000000000000000000000000000000000'
	)
AS
BEGIN
DECLARE @corporationID BINARY(18),@AccTypeID BINARY(18) 
	SET @corporationID = CONVERT(BINARY(18),@CorpID,2)
	SET @AccTypeID=CONVERT(BINARY(18),@AccountTypeID,2)

	SELECT  C.CorporationName,GETDATE() GeneratedTime, A.ID AccountID,A.AccountNumber,
	CASE WHEN ISNULL(A.AccountNumber, '') = '' THEN A.AccountName ELSE A.AccountNumber + '. ' + A.AccountName END AS AccountName,AT.AccountTypeName,
	AT.AccountTypeOrder,	A1.AccountNumber ParentAccountNumber,
	CASE WHEN ISNULL(A1.AccountNumber, '') = '' THEN A1.AccountName ELSE A1.AccountNumber + '. ' + A1.AccountName END AS ParentAccountName,A.AccountTypeID,A.Status
INTO #AccountList	FROM  dbo.Account AS A  
				INNER JOIN dbo.Corporation C ON A.SourceID=C.ID
				INNER JOIN dbo.AccountType AS AT ON A.AccountTypeID = AT.ID 
				LEFT JOIN dbo.Account AS A1 ON A.ParentAccountID = A1.ID 
				WHERE C.ID=@corporationID
				AND (A.AccountTypeID=@AccTypeID OR @AccTypeID=0x000000000000000000000000000000000000 OR @AccTypeID IS NULL)  AND A.[Status]<> 12 AND A.[Status] <>3
SELECT CorporationName,GeneratedTime,(SELECT COUNT(*) FROM #AccountList) TotalCount,CONVERT(VARCHAR(50),AccountID,2) AS ID,AccountNumber AS Number,AccountName AS [Name],AccountTypeName,CONVERT(VARCHAR(50),AccountTypeID,2) AS AccountTypeId,AccountTypeOrder,ParentAccountName,ParentAccountNumber,Status as AccountStatus
FROM #AccountList ORDER BY AccountTypeOrder,AccountTypeName, [Name]
END
GO

--API_Core_GetCorporations
CREATE OR ALTER PROCEDURE [dbo].[API_Core_GetCorporations] 
(
@UserID VARCHAR(50)=NULL
)
AS
BEGIN

		SELECT 
			CONVERT(VARCHAR(50),C.ID,2) AS ID
			,CorporationName AS [NAME]
			,C.LegalName
			 			
		INTO #Corp
		FROM Corporation C 
		INNER JOIN UserCorporation UC ON C.ID=UC.CorporationID
		WHERE C.[Status]<>3 AND  ( UC.UserID=CONVERT(BINARY(18), @UserID, 2) or @UserID is null)

		SELECT ID,[Name],LegalName,row_number() over (order by (select NULL)) AS SortOrder FROM #Corp ORDER BY [Name] ASC

		DROP TABLE #Corp

END
GO

--API_Core_GetProfitCenters
CREATE OR ALTER PROCEDURE[dbo].[API_Core_GetProfitCenters] 
(
	@CorpID VARCHAR(50)
)
AS
BEGIN
DECLARE @CorporationID BINARY(18)
 SET @CorporationID = CONVERT(BINARY(18),@CorpID,2)

SELECT DISTINCT PC.ID,PC.[Name]	
		INTO #tempPCs
		FROM Store PC 
		INNER JOIN Corporation C ON C.ID=PC.CorporationID
		INNER JOIN UserCorporation UC ON C.ID=UC.CorporationID
		WHERE C.ID=@CorporationID --AND PC.[Status]<>3 AND C.[Status]<>3

		SELECT  CONVERT(VARCHAR(50),ID,2)AS ID,[Name],ROW_NUMBER() OVER (ORDER BY [Name]) AS SortOrder  FROM #tempPCs
END
GO

--API_Core_GetNames
CREATE OR ALTER PROCEDURE [dbo].[API_Core_GetNames] 
(
	@CorpID VARCHAR(50),
	@Type VARCHAR(10)=''
)
AS
BEGIN
	DECLARE @CorporationID BINARY(18)
	 SET @CorporationID = CONVERT(BINARY(18),@CorpID,2)
	 SELECT A.ID,A.[Name],A.[Type]
	 INTO #tmpNameList
	 FROM
	 (SELECT EM.ID
	 ,dbo.GetEmployeeFullName(EM.FirstName, EMD.MiddleName, EM.LastName) AS [Name] 
	 ,EM.[Type]
	 FROM Employee EM 
	INNER JOIN EmployeeDetails EMD on EM.ID = EMD.ID
	INNER JOIN Corporation C ON C.ID=EM.CorporationID
	WHERE C.ID=@CorporationID AND EM.[Status]<>3
	UNION
	SELECT B.ID,B.[Name],B.[Type] 
	FROM Business B 
	INNER JOIN BusinessInfo BI ON B.ID=BI.ID
	INNER JOIN Corporation C ON C.ID=B.CorporationID
	WHERE C.ID=@CorporationID  AND B.[Status]<>3)A
	
	IF  (@Type IS NULL OR @Type = '')
	BEGIN
		
		SELECT CONVERT(VARCHAR(50), ID, 2) AS ID, [Name], ROW_NUMBER() OVER (ORDER BY [Name]) AS SortOrder, [Type],case when [Type]=0 then 'Customer' when [Type]=1 then 'Vendor' when [Type]=2 then 'Employee' else 'Others' End AS TypeName
		FROM #tmpNameList
		ORDER BY [Type],[Name]
	END
	ELSE
	BEGIN
		SELECT CONVERT(VARCHAR(50), ID, 2) AS ID, [Name], ROW_NUMBER() OVER (ORDER BY [Name]) AS SortOrder, [Type],case when [Type]=0 then 'Customer' when [Type]=1 then 'Vendor' when [Type]=2 then 'Employee' else 'Others' End AS TypeName
		FROM #tmpNameList
		WHERE [Type] = @Type
		ORDER BY [Name]
	END


END
GO

--API_Core_GetPurposes
CREATE OR ALTER PROCEDURE [dbo].[API_Core_GetPurposes]
(
@CorpId varchar(50)

)
AS
BEGIN 
DECLARE @CorporationID BINARY(18)

SET @CorporationID = CONVERT(BINARY(18),@CorpID,2)
SELECT CONVERT(VARCHAR(50),ID,2)AS ID,Name ,Type FROM Purpose WHERE CorporationID=@CorporationID and type=8 and [Status]<>3
END
GO
--API_Core_GetFrequencies
CREATE OR ALTER PROCEDURE [dbo].[API_Core_GetFrequencies]
AS 
BEGIN
SELECT  CONVERT(VARCHAR(50),ID,2)AS ID,[Name],Type from frequency WHERE [Type] IN(1,2)
END
GO

--API_Core_GetRemaindDays

CREATE OR ALTER PROCEDURE[dbo].[API_Core_GetRemaindDays]
 AS 
 BEGIN 
 SELECT  CONVERT(VARCHAR(50),ID,2)AS ID,Interval FROM Remind
 END
 GO


 --API_Core_GetPayeeNames

CREATE OR ALTER PROCEDURE [dbo].[API_Core_GetPayeeNames]
(
@CorpId varchar(50)

)
AS
BEGIN 
DECLARE @CorporationID BINARY(18)

SET @CorporationID = CONVERT(BINARY(18),@CorpID,2)
SELECT CONVERT(VARCHAR(50),ID,2)AS ID,Name ,Type FROM Business WHERE CorporationID=@CorporationID and [Status]<>3
END
GO

--API_Core_GetProfitandLoss

CREATE OR ALTER PROCEDURE[dbo].[API_Core_GetProfitandLoss] 
(
	@CorpID VARCHAR(50),
	@FromDate VARCHAR(50),
	@ToDate VARCHAR(50)
)
AS
BEGIN

DECLARE @corporationID BINARY(18)   
 SET @corporationID = CONVERT(BINARY(18),@CorpID,2)
 DECLARE @fDate DATETIME   
 SET @fDate = CONVERT(DATETIME,@FromDate)
 DECLARE @tDate DATETIME   
 SET @tDate = CONVERT(DATETIME,@ToDate)


		SELECT CorporationName,GETDATE() GeneratedTime,AT.AccountTypeName,A.ID AccountID,A.AccountNumber,
		CASE WHEN ISNULL(A.AccountNumber, '') = '' THEN A.AccountName ELSE A.AccountNumber + '. ' + A.AccountName END AS AccountName,
		CASE WHEN ISNULL(A1.AccountNumber, '') = '' THEN A1.AccountName ELSE A1.AccountNumber + '. ' + A1.AccountName END AS ParentAccountName,
		A1.AccountNumber AS ParentAccountNumber,
		A.AccountTypeID,  (CASE WHEN ISNUMERIC(A.AccountNumber)=1 THEN CONVERT(Decimal,A.AccountNumber) ELSE 0 END) AS AccNumSortOrder,
		  (CASE WHEN ISNUMERIC(A1.AccountNumber)=1 THEN CONVERT(Decimal,A1.AccountNumber) ELSE 0 END) AS ParentAccNumSortOrder,
		(CASE WHEN A.AccountTypeID=0x0FA700000000000000000000000000000020 THEN 1
			 WHEN A.AccountTypeID=0x0FA700000000000000000000000000000021 THEN 2
			 WHEN A.AccountTypeID=0x0FA700000000000000000000000000000022 THEN 3
			 WHEN A.AccountTypeID=0x0FA700000000000000000000000000000023 THEN 4
			 WHEN A.AccountTypeID=0x0FA700000000000000000000000000000024 THEN 5 END) AS DetailOrder	 INTO #Accounts
			 FROM  dbo.Account A 
			INNER JOIN dbo.AccountType AT ON A.AccountTypeID = AT.ID
	    	INNER JOIN  Corporation C ON A.SourceID=C.ID
			LEFT JOIN Account A1 ON A.parentaccountid=A1.ID	
			WHERE C.ID=@corporationID AND A.[Status]<>3
			AND AT.ParentAccountTypeID=0x0FA700000000000000000000000000000019
			


SELECT  T.AccountID,SUM(CASE WHEN DebitCredit=0 THEN T.Amount ELSE 0 END) AS Debit,
		SUM(CASE WHEN DebitCredit=1 THEN T.Amount ELSE 0 END) AS Credit,
		CASE WHEN A.AccountTypeID IN(0x0FA700000000000000000000000000000020,0x0FA700000000000000000000000000000023) THEN  ISNULL(SUM(CASE WHEN DebitCredit=1 THEN Amount ELSE -1 * Amount END),0) ELSE
		ISNULL(SUM(CASE WHEN DebitCredit=0 THEN Amount ELSE -1 * Amount END),0) END AS Amount
		INTO #PandLBalances
		FROM [Transaction] T
			INNER JOIN JournalEntry J ON T.JournalEntryID = J.ID
			INNER JOIN #Accounts A ON T.AccountID = A.AccountID
			WHERE J.CorporationID=@corporationID AND (J.EntryDate>=@fDate AND J.EntryDate<=@tDate)
			AND ISNULL(J.SourceType,1) <> 73 
			AND T.[STATUS] <> 3
			GROUP BY T.AccountID,A.AccountTypeID


SELECT A.CorporationName,A.GeneratedTime,A.AccountTypeName,A.AccountID,A.AccountNumber,A.AccountName,A.ParentAccountNumber,A.ParentAccountName,AccNumSortOrder,(CASE WHEN ParentAccNumSortOrder=0 THEN AccNumSortOrder ELSE ParentAccNumSortOrder END) AS ParentAccNumSortOrder,
A.AccountTypeID,A.DetailOrder,ISNULL(PL.Debit,0) AS Debit,ISNULL(PL.Credit,0) AS Credit,ISNULL(PL.Amount,0) AS Amount  INTO #PandL
FROM #Accounts A LEFT OUTER JOIN
#PandLBalances PL ON A.AccountID=PL.AccountID

DECLARE @Profit DECIMAL(14,2),@totIncome DECIMAL(14,2),@totCogs DECIMAL(14,2),@totExpenses DECIMAL(14,2),@totOtherIncome DECIMAL(14,2),@totOtherExpenses DECIMAL(14,2)
SELECT @Profit=ISNULL((SUM(Credit)-SUM(Debit)),0) ,
 @totIncome=ISNULL(SUM(CASE WHEN AccountTypeID=0x0FA700000000000000000000000000000020 THEN Amount ELSE 0 END),0) ,
 @totCogs=ISNULL(SUM(CASE WHEN AccountTypeID=0x0FA700000000000000000000000000000021 THEN Amount ELSE 0 END),0),
 @totExpenses=ISNULL(SUM(CASE WHEN AccountTypeID=0x0FA700000000000000000000000000000022 THEN Amount ELSE 0 END),0),
 @totOtherIncome= ISNULL(SUM(CASE WHEN AccountTypeID=0x0FA700000000000000000000000000000023 THEN Amount ELSE 0 END),0),
 @totOtherExpenses=ISNULL(SUM(CASE WHEN AccountTypeID=0x0FA700000000000000000000000000000024 THEN Amount ELSE 0 END),0)
 FROM #PandL


SELECT CorporationName,GeneratedTime,(SELECT COUNT(*) FROM #PandL) TotalCount,AccountTypeName,DetailOrder AS Sortorder,CONVERT(VARCHAR(50),AccountID,2) AS ID,AccountNumber,AccountName,ParentAccountName,CONVERT(VARCHAR(50),AccountTypeID,2) AS AccountTypeID,
	ParentAccountNumber,ISNULL((SUM(Credit)-SUM(Debit)),0) AS Balance,(@totIncome-@totCogs) GrossProfit,(@totIncome-@totCogs-@totExpenses) NetOperatingIncome,
	(@totOtherIncome-@totOtherExpenses) NetOtherIncome,@Profit NetIncome,DetailOrder
		FROM #PandL
		GROUP BY CorporationName,GeneratedTime,AccountTypeName,AccountID,AccountNumber,AccountName,ParentAccountName,ParentAccountNumber,DetailOrder,AccountTypeID,AccNumSortOrder,ParentAccNumSortOrder
		ORDER BY DetailOrder,ParentAccNumSortOrder,AccNumSortOrder
DROP TABLE #PandL

END
GO

--API_Core_GetComments

CREATE OR ALTER PROCEDURE[dbo].[API_Core_GetComments]
(
 @UID varchar(50),
 @JID varchar(50)
)
AS 
BEGIN
DECLARE @Userid BINARY(18),
 @JournalEntryID BINARY(18)
SET @Userid = CONVERT(BINARY(18),@UID,2)
SET @JournalEntryID=CONVERT(BINARY(18), @JID,2)
SELECT u.FirstName,u.LastName,c.Comment,CONVERT(Varchar(50),c.CommentedBy,2)as Commentby,c.CommentDate FROM [User]u inner join ApprovalComments c on u.ID=c.CommentedBy WHERE u.ID=@Userid and c.JournalEntryID=@JournalEntryID
END 
GO

--API_Core_GetAttachments

CREATE OR ALTER PROCEDURE [dbo].[API_Core_GetAttachments]
(
  @JID varchar(50)
)
AS 
BEGIN 
DECLARE  @JournalEntryID BINARY(18)
SET @JournalEntryID=CONVERT(BINARY(18), @JID,2)
SELECT t1.ID ,t1.FolderPath,t2.FileName
FROM importdocument t1
INNER JOIN ImportDocumentDetails t2
ON t1.ID = t2.ImportDocumentID 
WHERE t1.JournalEntryID = @JournalEntryID and t2.Status <>3 and t1.Status<>3
END
GO

--API_Core_GetAddresses

CREATE OR ALTER PROCEDURE [dbo].[API_Core_GetAddresses]
(
@BId varchar(50)

)
AS
BEGIN 
DECLARE @BusinessID BINARY(18)

SET @BusinessID = CONVERT(BINARY(18),@BId,2)
SELECT CONVERT(VARCHAR(50),A.ID,2)AS ID ,A.Address1,A.ZipCode,A.City ,S.Name as StateName,C.Name as CountryName FROM ((( Address A inner Join VendorAddress VA ON A.ID=VA.AdressID) inner join State S on A.state=S.ID)) inner join Country c on A.Country=C.ID WHERE VA.BusinessID=@BusinessID
END
GO

--- Sps Used for Execute NonQueryCommand

--USP_InsertTranAuditLog

CREATE OR ALTER PROCEDURE [dbo].[API_Core_InsertTranAuditLog] 
	
	 @JID AS BINARY(18) = NULL
	
AS
BEGIN

	DECLARE @CorpID AS BINARY(18)
	DECLARE @ClientID AS BINARY(18)
	DECLARE @VoidEnable AS BIT
	SELECT @ClientID=ClientID FROM Corporation WHERE ID =(SELECT CorporationID FROM JournalEntry WHERE ID=@JID)
	SELECT @VoidEnable=ISNULL(IsEnable,0) FROM UserPreferenceSettings WHERE UserID=@ClientID AND [Type]=27 
	
	UPDATE JournalEntry SET IsUpdated=1 WHERE ID=@JID
	UPDATE JournalEntryExt SET IsUpdated=1 WHERE JournalEntryID=@JID
	INSERT INTO aosAudit_Sandbox.dbo.TransactionAuditLog(TransactionID,AccountID,AccountName,SourceID,SourceType,TargetID,TargetType,Memo,Amount,DebitCredit,
	StoreID,StoreName,ReconcileID,JournalID,CorporationID,JSourceType,ParentSourceType,EntryDate,BillDate,EntryNum,RefNum,CreatedBy,CreatedName,CreatedDate,ModifiedBy,ModifiedDate,ModifiedName,DefaultRow
	,SourceName,TargetName,[Status],ReconStatus,TransNumber,TOrder,CreatedRoleName,ModifiedRoleName,AuditType,ApprovalLevel,VoidDate)
		SELECT T.ID AS TranID,T.AccountID
		,(CASE WHEN (A.[Status] = 12) THEN
		(SELECT TOP 1 (ISNULL(al.AccountNumber + '. ', '') + al.AccountName)
		FROM  Account al
		WHERE  al.ID = A.ParentAccountID) ELSE (ISNULL(A.AccountNumber + '. ', '') + A.AccountName) END) AS AccountName
		,T.SourceID
		,T.SourceType
		,T.TargetID
		,T.TargetType
		,T.Memo
		,T.Amount
		,CASE WHEN J.Status=2 AND @VoidEnable=1 THEN (CASE WHEN T.DebitCredit=0 THEN 1 ELSE 0 END) ELSE T.DebitCredit END
		,T.StoreID
		,(SELECT TOP 1 Name FROM  Store S WHERE S.ID = T.StoreID AND s.DefaultStore = 0)
		,T.ReconciliationID
		,T.JournalEntryID
		,J.CorporationID
		,J.SourceType
		,J.ParentSourceType
		,J.EntryDate
		,(CASE WHEN J.BillDate IS NOT NULL THEN CONVERT(VARCHAR(10),J.BillDate,101) ELSE NULL END) AS BillDate
		,J.EntryNumber
		,T.ReferenceNumber
		,J.CreatedBy
		,ISNULL(U.FirstName + ' ' + (ISNULL(U.LastName, '')),'') AS CreatedName
		--,(CASE WHEN T.[Order] = 0 OR  J.SourceType in(16,111,131,132,139) THEN
		--				(SELECT     TOP 1 FirstName + ' ' + (ISNULL(LastName, ''))
		--				FROM          [User]
		--				WHERE      ID = J.CreatedBy) ELSE NULL END) AS CreatedName
		,J.CreatedDate
		,J.ModifiedBy
		,J.ModifiedDate
		,ISNULL(UM.FirstName + ' ' + (ISNULL(UM.LastName, '')),'') AS ModifiedName
		--,(CASE WHEN (T .[Order] = 0 OR J.SourceType in(16,111,131,132,139)) AND J.ModifiedBy IS NOT NULL THEN
		--				(SELECT     TOP 1 FirstName + ' ' + (ISNULL(LastName, ''))
		--				FROM          [User]
		--				WHERE      ID = J.ModifiedBy) ELSE NULL END) AS ModifiedName
		,(CASE WHEN T.ParentID IS NULL THEN 0 ELSE 1 END) AS DeafultRow
		,(CASE WHEN T.SourceType IN (0,1)
				THEN (SELECT TOP 1 CASE WHEN B.ParentID IS NOT NULL THEN B1.Name+' - '+B.Name ELSE  B.Name END AS Name FROM Business B 
				LEFT OUTER JOIN Business B1 ON B.ParentID=B1.ID
				WHERE B.ID = T.SourceID)
				WHEN T.SourceType IN (2,13) THEN (SELECT     TOP 1 ISNULL(E.FirstName, '') + ' ' + ISNULL(E.LastName, '')
				FROM Employee E
				WHERE E.ID = T.SourceID)
				WHEN T.SourceType IN (80, 66, 41, 42) THEN (SELECT     TOP 1 (CASE WHEN Name = 'Not Available' THEN '' WHEN [TYPE] IN (2,9) THEN [Description] ELSE Name END)
				FROM Purpose
				WHERE      ID = T.SourceID) ELSE NULL 
				END) SourceName
		,(CASE WHEN T.TargetType IN (0,1)
			THEN (SELECT TOP 1 CASE WHEN B.ParentID IS NOT NULL THEN B1.Name+' - '+B.Name ELSE  B.Name END AS Name FROM Business B 
			LEFT OUTER JOIN Business B1 ON B.ParentID=B1.ID
			WHERE B.ID = T.TargetID)
			WHEN T.TargetType IN (2,13) THEN (SELECT     TOP 1 ISNULL(E.FirstName, '') + ' ' + ISNULL(E.LastName, '')
			FROM Employee E
			WHERE E.ID =T.TargetID)
			ELSE NULL
			END) TargetName
		,J.[Status] 	
		,T.ReconStatus
		,J.TransNumber
		,T.[Order]
		,(CASE WHEN J.ModifiedBy IS NULL THEN (SELECT TOP 1  R.Name FROM [Role] R WHERE ID=U.RoleID) ELSE NULL END) AS CreatedRoleName
		,(SELECT TOP 1  RM.Name FROM [Role] RM WHERE ID=UM.RoleID) AS ModifiedRoleName
		,2 AS AuditType
		,'Approved'   As ApprovalLevel,J.VoidDate
		FROM [Transaction] T
		INNER JOIN JournalEntry J ON T.JournalEntryID=J.ID
		LEFT OUTER JOIN [User] U ON J.CreatedBy=U.ID
		LEFT OUTER JOIN [User] UM ON J.ModifiedBy=UM.ID
		INNER JOIN [Account] A ON T.AccountID =A.ID
		WHERE J.ID=@JID
		
		UPDATE aosAudit_Sandbox.dbo.TransactionAuditLog SET ActualStatus= j.[Status]
		FROM aosAudit_Sandbox.dbo.TransactionAuditLog td
		INNER JOIN JournalEntry J ON td.JournalID=J.ID
		WHERE JournalID=@JID
END
GO

--API_Core_Insertcomments

CREATE OR ALTER PROCEDURE [dbo].[API_Core_Insertcomments]
@JournalEntryID as varbinary(25),
@Comment as varchar(2000),
@CommentedDate as datetime2,
@CommentedBy as varbinary(25)
AS 
BEGIN 
INSERT INTO ApprovalComments (JournalEntryID,Comment,CommentedBy,CommentDate) VALUES(convert(varbinary(25),@JournalEntryID,2),@Comment,convert(varbinary(25),@CommentedBy,2),@CommentedDate)
END
GO

--SPs used for ExecuteScalarCommand

--API_Core_GetEntryNumberOrCheckNumber

CREATE OR ALTER PROCEDURE[dbo].[API_Core_GetEntryNumberOrCheckNumber]
(	
	@ID AS BINARY(18),
	@Type AS INT,
	@IsENtryORCheck SMALLINT
)
AS
BEGIN
 DECLARE @Number AS BIGINT=1
 IF @IsENtryORCheck=0
 BEGIN
	SELECT @Number=MAX(CAST(ISNULL(EntryNumber,'0') AS BIGINT))+1 FROM JournalEntry J INNER JOIN [Transaction] T ON J.ID=T.JournalEntryID
	WHERE J.CorporationID=@ID   AND J.SourceType=@Type  AND T.[Status]<>3 AND ISNUMERIC(J.EntryNumber)=1
 END
 ELSE
 BEGIN
		SELECT @Number=MAX(CAST(ISNULL(T.ReferenceNumber,'0') AS BIGINT))+1 FROM JournalEntry J INNER JOIN [Transaction] T ON J.ID=T.JournalEntryID
		WHERE T.AccountID=@ID   AND J.SourceType=@Type  AND T.[Status]<>3 AND ISNUMERIC(J.EntryNumber)=1
 END
 SELECT @Number
END
GO

--API_Core_GetUserPrivileges

CREATE OR ALTER PROCEDURE[dbo].[API_Core_GetUserPrivileges]
@ID AS BINARY(18)
AS
BEGIN
	DECLARE @MenuID AS BINARY(18)=0x010000000000000000000000000000000357
	DECLARE @SourceID AS BINARY(18)
	SELECT @SourceID=(CASE WHEN RoleID IS NULL 
	THEN ID
	ELSE RoleID 
	END) FROM [User] WHERE ID=@ID
	select  [create] as HasCreate,[update] as HasUpdate, [delete] as HasDelete from Privileges where SourceID=@SourceID and MenuID=@MenuID
	
	END
GO
--new privelages 
CREATE OR ALTER procedure [dbo].[API_Core_GetPrevilages]
(
 @SId varchar(50),
@MenuIDs varchar(MAX)
)
as 
begin


DECLARE @MenuIDsList TABLE (
	[ID] int Identity(1,1),
    [MenuID] Binary(18) not null
);
INSERT INTO @MenuIDsList ([MenuID])
SELECT CONVERT(BINARY(18),value,2)
FROM STRING_SPLIT(@MenuIDs, ',');


DECLARE @SourceID BINARY(18)
set @SourceID= CONVERT(BINARY(18),@SId,2)
select CONVERT(VARCHAR(50),menuID,2)AS MenuId ,[create],[update],[View],[delete] from Privileges where SourceID=@SourceID
and MenuID in (select MenuID from @MenuIDsList)

end 
GO
--------DailySale Sp------------------------
--API_Core_GetDailyConfigLines

CREATE OR ALTER PROCEDURE [dbo].[API_Core_GetDailyConfigLines]
	@CorpID varbinary(18),
	@StoreID varbinary(18),
	@IsOfspecificDepartmentType bit=0,
	@DebitAccID varbinary(18)=0x000000000000000000000000000000000000

AS
BEGIN


		select CONVERT(VARCHAR(50), dl.ID, 2) as LineID,dl.Name LineName,CONVERT(VARCHAR(50), dl.DepartmentID, 2) as DepartmentID,
		dd.DeptType as DepartmentType,CONVERT(VARCHAR(50), dl.CreditAccountID, 2) As CreditAccountID ,creditAcc.AccountName as 
		CreditAccount,CONVERT(VARCHAR(50), dl.DebitAccountID, 2) as DebitAccountID, debitAcc.AccountName as DebitAccount
		,cp.CorporationName as CorporationName ,S.Name StoreName into #temp1
		from DailyConfigLine as dl join DailyConfigDepartment as dd on dl.DepartmentID=dd.ID  
			INNER JOIN  Corporation as cp on cp.id=DL.CorporationID
			LEFT JOIN Store S ON (@StoreID=S.ID AND S.Status<>3)
		left JOIN Account AS creditAcc ON creditAcc.ID = dl.CreditAccountID
		left JOIN Account AS debitAcc ON debitAcc.ID = dl.DebitAccountID 
		where DL.CorporationID=@CorpID and dl.Status=1 
		AND (@StoreID = 0x000000000000000000000000000000000000 OR DL.StoreID =@StoreID) 
		
		if @IsOfspecificDepartmentType=0
		begin
			select * from #temp1 
		end
		 else if @IsOfspecificDepartmentType=1
			begin
			select * from #temp1 where  DepartmentType in(7,8) and DebitAccountID= CONVERT(VARCHAR(50), @DebitAccID, 2)
			end
END
GO


--------BillPayment Sp------------------------

CREATE or ALTER PROCEDURE [dbo].[API_Core_SaveBillPayment]

@JID as varbinary(18),
@BankTransID as varbinary(18),
@PayeeTransID as varbinary(18),
@CorpID as varbinary(18),
@PayeeID as varbinary(18),
@BankAccountID as varbinary(18),
@PayeeAccountID as varbinary(18),
@PaymentMethodID as varbinary(18),
@UserID as varbinary(18),
@EntryDate as datetime,
@ClearedDate as datetime,
@ReferenceNumber as varchar(50)=null,
@CheckNO varchar(200),
@Memo varchar(400)=null,
@Amount decimal(18,2),
@BankTransRefID bigint=null,
@HasAttachments bit=0
AS
BEGIN
Declare @BankRefID bigint;
Declare @HasBankID bit=null;
	if @BankTransRefID>0
	begin
		set @BankRefID=@BankTransRefID ;
		set @HasBankID=1;
	end
	--creating journal entry--
	INSERT INTO JournalEntry(ID,CorporationID,EntryDate,EntryNumber,SourceType,ParentSourceType,ReferenceMode,Status,CreatedBy,CreatedDate,
	PaymentMethodID,IsAttachment,ClearedDate) 
	SELECT @JID ID,@CorpID CorporationID,@EntryDate EntryDate,@ReferenceNumber EntryNumber,15 SourceType,
	15 ParentSourceType,2 ReferenceMode,1,@UserID CreatedBy,GETDATE() CreatedDate,@PaymentMethodID PaymentMethodID,@HasAttachments,@ClearedDate
	--creating transaction for bank/payeee--
	INSERT INTO [Transaction] (ID,AccountID,JournalEntryID,Amount,TransactionDate,DebitCredit,ReferenceNumber,Memo,[Status],
	SourceID,SourceType,[Order],IsBankTransactionRefId,BankTransactionRefId)
	SELECT @BankTransID ID,@BankAccountID AccountID,@JID JournalEntryID,@Amount Amount,GETDATE() TransactionDate,
	1 DebitCredit,@CheckNO ReferenceNumber,@Memo Memo,1 [Status],@PayeeID SourceID,1 SourceType,0 [Order],@HasBankID,@BankRefID

	INSERT INTO [Transaction] (ID,AccountID,JournalEntryID,ParentID,Amount,TransactionDate
	,DebitCredit,[status],SourceID,SourceType,[Order],IsBankTransactionRefId,BankTransactionRefId)
	SELECT @PayeeTransID ID,@PayeeAccountID AccountID,@JID JournalEntryID,@BankTransID ParentID,@Amount Amount,
	GETDATE() TransactionDate,0 DebitCredit,1 [status],@PayeeID SourceID,97 SourceType,1 [Order],@HasBankID,@BankRefID

	--creating billpaymentsinformation--
	INSERT INTO BillPaymentsInformation (JournalEntryID,CheckNo,Amount,SourceID,AccountID,Memo,AdjustTransactionID,TransactionID) 
	SELECT @JID JournalEntryID,@CheckNO CheckNo,@Amount Amount,@PayeeID SourceID,@BankAccountID AccountID,
	@Memo Memo,@PayeeTransID AdjustTransactionID,@BankTransID TransactionID

	--creating billEntryinformation--
	INSERT INTO BillEntryInformation (JournalEntryID,Amount,Outstanding,SourceID,DebitCredit,Status,TransactionID)
	SELECT @JID,0-@Amount Amount,@Amount Outstanding,@PayeeID SourceID,1,1,@BankTransID

	--creating billEntryPayments--
	INSERT INTO BillEntryPayments (SourceID,BillPaymentID,PaidAmount,OutStanding,AdjustmentAmount,Status,TransactioonID,AdjustTransactionID)
	SELECT @PayeeID,@JID,0-@Amount,@Amount,@Amount,1,@BankTransID,@PayeeTransID
	--DECLARE @OPID INT
	--SELECT @OPID=IDENT_CURRENT('BillPaymentsInformation')
	--SELECT IDENT_CURRENT('BillPaymentsInformation') as billID
END
GO
------------------------------------------------------------------------------------------------------------------------------------------
--------------Nimble Bill Matches ----------------------


