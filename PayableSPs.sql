--exec API_Payables_BillEntryValidations @CorpID=N'49375C11AFC2C8A94788AAB115E71FD70000',
--@VenID=N'774C852A7657A68C4CF783482C4AC2AF0000',@ContractID=N'855E27120DE6E8B54EFA4A72761D418C0000',
--@JEID=default,@BillNumber=N'UT 9',@IsValidate=0,@SourceType=21,@Amount=333

CREATE PROCEDURE [dbo].[API_Payables_BillEntryValidations]
(
@CorpID VARCHAR(50)=NULL,
@VenID VARCHAR(50)=NULL,
@ContractID VARCHAR(50)=NULL,
@JEID VARCHAR(50)=NULL,
@BillNumber VARCHAR(50)=NULL,
@IsValidate BIT=0,
@SourceType INT,
@Amount DECIMAL
)
AS
BEGIN

DECLARE @duplicateBillErrorMessage VARCHAR(100) ='Duplicate number is not allowed to save for multiple bills'
BEGIN TRY  
		
		DECLARE @cID BINARY(18)=CONVERT(BINARY(18), @CorpID, 2) 	
		DECLARE @vID BINARY(18)=CONVERT(BINARY(18), @VenID, 2) 		
		DECLARE @CtID BINARY(18)=CONVERT(BINARY(18), @ContractID, 2) 

		Declare @CorpCheck BIT
		Declare @VendorCheck BIT
		Declare @ContractCheck BIT

		Select @CorpCheck=(Select CASE WHEN COUNT(C.ID) > 0 THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT)END AS [Status] from Corporation C where C.ID=@cID )
		
		IF(@CorpCheck=0)
		BEGIN
			Select 'Corporation does not exists' as [Status], 404 as [StatusCode]
			RETURN
		END		
		
		--Select @VendorCheck=(Select CASE WHEN COUNT(C.ID) > 0 THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT)END
		--from Corporation C
		--inner join Business B on B.CorporationID=C.ID where C.ID=@cID and B.ID=@vID)

		--		IF(@VendorCheck=0)
		--		BEGIN
		--			Select 'Vendor does not belong to the current Corporation' as [Status],404 as [StatusCode]
		--			RETURN
		--		END
				

		--Select @ContractCheck=(Select CASE WHEN COUNT(B.ID) > 0 THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT)END
		--from Business B
		--inner join VendorContract VC on B.ID=VC.BusinessID where B.ID=@vID AND VC.ID=@CtID)

		--IF(@ContractCheck=0 AND @ContractID IS NOT NULL)
		--	BEGIN
		--		Select 'Contract does not belong to the current Vendor' as [Status] ,404 as [StatusCode]
		--		RETURN
		--	END
		
		--Duplicate BillNumber Validations
		DECLARE @resJID BINARY(18)
		DECLARE @resOCRJID BINARY(18)
		DECLARE @JIDCount INT
		DECLARE @OCRJIDCount INT

		IF (@JEID IS NULL)
			BEGIN
				Select @JIDCount=( Select
				COUNT(JE.ID)				
				from JournalEntry JE
				INNER JOIN [Transaction] T on JE.ID=T.JournalEntryID
				where T.status<>3 AND T.parentID is null AND JE.EntryNumber=@BillNumber
				AND (JE.Status=1 OR Je.Status is NULL OR T.status=5) AND JE.CorporationID=@cID
				AND T.SourceID=@vID AND JE.SourceType=@SourceType)

				Select @OCRJIDCount=( Select
				COUNT(JE.ID)				
				from OCRJournalEntry JE
				INNER JOIN [OCRTransaction] T on JE.ID=T.JournalEntryID
				where T.status<>3 AND T.parentID is null AND JE.EntryNumber=@BillNumber
				AND (JE.Status=1 OR Je.Status is NULL OR T.status=5) AND JE.CorporationID=@cID
				AND T.SourceID=@vID AND JE.SourceType=@SourceType)	
				IF(@JIDCount >0 OR  @OCRJIDCount>0)
				BEGIN
				Select @duplicateBillErrorMessage as [Status]	,404 as [StatusCode]
				RETURN
				END	
			END
			Else 
			BEGIN
	 
				Select @resJID=(Select TOP 1
				JE.ID
				from JournalEntry JE
				INNER JOIN [Transaction] T on JE.ID=T.JournalEntryID
				where T.status<>3 AND T.parentID is null AND JE.EntryNumber=@BillNumber
				AND (JE.Status=1 OR Je.Status is NULL OR T.status=5) AND JE.CorporationID=@cID
				AND T.SourceID=@vID AND JE.SourceType=@SourceType)

				Select @resOCRJID=(Select TOP 1
				JE.ID
				from OCRJournalEntry JE
				INNER JOIN [OCRTransaction] T on JE.ID=T.JournalEntryID
				where T.status<>3 AND T.parentID is null AND JE.EntryNumber=@BillNumber
				AND (JE.Status=1 OR Je.Status is NULL OR T.status=5) AND JE.CorporationID=@cID
				AND T.SourceID=@vID AND JE.SourceType=@SourceType)
				
				IF((@resJID<>CONVERT(BINARY(18), @JEID, 2) AND (@resJID is not null)) OR (@resOCRJID<>CONVERT(BINARY(18), @JEID, 2) AND (@resOCRJID is not null)))
				BEGIN
				Select @duplicateBillErrorMessage as [Status]	,404 as [StatusCode]
				RETURN
				END		
				
			END

			IF(@IsValidate=0 AND @JEID IS NULL)
			BEGIN
				IF(@SourceType=21 OR @SourceType=38)
				BEGIN
					Select @resJID=(Select TOP 1
						JE.ID from JournalEntry JE
					inner join [Transaction] T on T.JournalEntryID=JE.ID 
					Where T.status<>3 AND T.ParentID IS NULL AND T.Amount=@Amount 
					AND (JE.Status=1 OR JE.Status is NULL OR T.status=5) 
					AND JE.CorporationID=@cID AND T.SourceID=@vID AND JE.SourceType=@SourceType
					order by JE.EntryDate desc)

					IF(@resJID is not null)
					BEGIN	
					Select  'Alert! Another bill with  invoice number ' + JE.EntryNumber + ' but same date '+ CONVERT(VARCHAR(50),CONVERT(date, JE.EntryDate)) + ' and same  invoice amount ' + CONVERT(VARCHAR(50),@AMOUNT) + ' exists '
							+ CASE WHEN JE.IsAttachment=1 THEN 'with bill attachment' ELSE 'with no bill attachment' END
					as [Status]	,409 as [StatusCode]from JournalEntry JE WHERE ID=@resJID
					RETURN
					END
				END

				IF(@SourceType=148)
				BEGIN
					Select @resJID=(Select TOP 1
						JE.ID from OCRJournalEntry JE
					inner join [OCRTransaction] T on T.JournalEntryID=JE.ID 
					Where T.status<>3 AND T.ParentID IS NULL AND T.Amount=@Amount 
					AND (JE.Status=1 OR JE.Status is NULL OR T.status=5) 
					AND JE.CorporationID=@cID AND T.SourceID=@vID AND JE.SourceType=@SourceType
					order by JE.EntryDate desc)

					IF(@resJID is not null)
					BEGIN
					Select  'Alert! Another bill with  invoice number ' + JE.EntryNumber + ' but same date '+ CONVERT(VARCHAR(50),CONVERT(date, JE.EntryDate)) + ' and same  invoice amount ' + CONVERT(VARCHAR(50),@AMOUNT) + ' exists '
							+ CASE WHEN JE.IsAttachment=1 THEN 'with bill attachment' ELSE 'with no bill attachment' END
					as [Status],409 as [StatusCode]	from OCRJournalEntry JE WHERE ID=@resJID
					RETURN
					END
				END

			END
			--IF No Errors
			SELECT NULL as [Status]	,NULL as [StatusCode]	
		END TRY
		BEGIN CATCH 
			SELECT ERROR_MESSAGE()
		END CATCH
END
GO

CREATE procedure [dbo].[API_Payables_GetBillEntryActivityLog]
(
@JID VARCHAR(50)=NULL
)
AS
BEGIN

DECLARE @journalID BINARY(18)=CONVERT(BINARY(18), @JID, 2) 

Select Comment,CommentDate, u.FirstName+' '+ u.LastName as [CommentedBy],u.ApprovalType 
from  ApprovalComments AC
inner join [User] U on u.ID=Ac.CommentedBy
where JournalEntryID=@journalID order by CommentDate

END
GO

ALTER procedure [dbo].[API_Payables_GetContractDetails]
(
@ContractID VARCHAR(50)=NULL
)
AS
BEGIN
DECLARE @cID BINARY(18)=CONVERT(BINARY(18), @ContractID, 2) 

		SELECT 
			 CONVERT(VARCHAR(50),VC.ID,2) AS ID,
			 VC.AccountNumber AS [NAME],			
			 VC.IsDefault AS [IsDefault],
			 VC.AddressID AS [AddressID],
			 A.Address1 as [Address1],
			 A.Address2 as [Address2],
			 S.Name as [State],
			 c.Name AS [Country],
			 A.ZipCode as [ZipCode]
		FROM [VendorContract] VC 
		INNER JOIN [VendorAddress] VA ON VC.AddressID = VA.ID 
		INNER JOIN [Address] A ON VA.AdressID = A.ID     
		LEFT JOIN [State] S ON S.ID = A.State  
		LEFT JOIN Country c ON c.ID = A.Country   
		WHERE VC.ID= @cID  AND vc.status=1
END
GO

-- API_Core_GetContracts 'DA350BAF7CE9F4884BD9D61EDCDB576A0000'
CREATE procedure [dbo].[API_Payables_GetContracts]
(
@VenID VARCHAR(50)=NULL
)
AS
BEGIN
		DECLARE @vID BINARY(18)=CONVERT(BINARY(18), @VenID, 2) 
		SELECT 
			CONVERT(VARCHAR(50),VC.ID,2) AS ID,
			VC.AccountNumber AS [NAME],				
			VC.IsDefault AS [IsDefault]
			from VendorContract VC where VC.BusinessID=@vID				
END
GO

CREATE procedure [dbo].[API_Payables_GetContractSplitAccountDetails]
(
@ContractID VARCHAR(50)=NULL
)
AS
BEGIN

DECLARE @cID BINARY(18)=CONVERT(BINARY(18), @ContractID, 2) 

Select AccountID,TaxPercentage from VendorTaxInfo where contractID=@cID

END
GO

--API_Payables_GetLastBillEntryJE '774C852A7657A68C4CF783482C4AC2AF0000'
CREATE procedure [dbo].[API_Payables_GetLastBillEntryJE]
(
@VenID VARCHAR(50)=NULL
)
AS
BEGIN

DECLARE @vID BINARY(18)=CONVERT(BINARY(18), @VenID, 2) 
		
Select top 1 CONVERT(VARCHAR(50),t.JournalEntryID,2) as [ID]from JournalEntry JE 
Inner Join [Transaction] T on T.JournalEntryID=JE.ID
where t.SourceID=@vID and t.Status=1 and je.SourceType=21  
order by je.transNumber desc, t.TransactionDate desc

END
GO

CREATE procedure [dbo].[API_Payables_GetUserApprovalType]
(
@UserID VARCHAR(50)=NULL
)
AS
BEGIN
		DECLARE @uID BINARY(18)=CONVERT(BINARY(18), @UserID , 2) 
		Select ApprovalType as [Status] from [User] where ID=@uID 
END
GO

--API_Payables_GetVendorDetails 'DA350BAF7CE9F4884BD9D61EDCDB576A0000'
CREATE procedure [dbo].[API_Payables_GetVendorDetails]
(
@VenID VARCHAR(50)=NULL
)
AS
BEGIN
DECLARE @vID BINARY(18)=CONVERT(BINARY(18), @VenID, 2) 

	Select 
	BI.CreditDaysID as [CreditDaysID],
	BI.PaymentMethodID as [PaymentMethodID],
	VC.ID as [DefaultContractID]
	from BusinessInfo BI
	INNER JOIN VendorContract VC on BI.ID=VC.BusinessID
	where VC.IsDefault=1 and BI.ID=@vID 
END
GO

CREATE procedure [dbo].[API_Payables_GetVendors]
(
@CorpID VARCHAR(50)=NULL
)
AS
BEGIN
		DECLARE @cID BINARY(18)=CONVERT(BINARY(18), @CorpID, 2) 	
		
		SELECT 
			CONVERT(VARCHAR(50),B.ID,2) AS ID,
			B.Name AS [NAME],
			B.Type AS [TYPE],
			CONVERT(VARCHAR(50),VC.ID,2) AS [DefaultContractID]			
			FROM Business B 	
			INNER JOIN VendorContract VC on B.ID = VC.BusinessID
			WHERE  B.CorporationID=@cID and VC.isDefault=1
END
GO

--exec API_Payables_LoadLast10Transactions '774C852A7657A68C4CF783482C4AC2AF0000'
CREATE procedure [dbo].[API_Payables_LoadLast10Transactions]
(
@VenID VARCHAR(50)=NULL
)
AS
BEGIN

DECLARE @vID BINARY(18)=CONVERT(BINARY(18), @VenID, 2) 

Select top 10 * from
((Select   
CONVERT(VARCHAR(50),JE.ID,2) as JEID,
JE.EntryNumber as [BillNumber],
JE.EntryDate as [Date],
CT.DueDate as [DueDate],
T.Amount as [Amount],
CASE WHEN BI.Outstanding=0 THEN 'Paid' 
		WHEN BI.Outstanding< BI.Amount THEN 'Partially Paid'
		WHEN JE.Status=2 THEN 'Voided'
		ELSE 'Payment Pending' END as [BillStatus],
CASE WHEN COUNT(A.AccountName) is NULL THEN '-' 
		WHEN COUNT(DISTINCT A.AccountName)=1 THEN MAX(A.AccountName)
		WHEN COUNT(DISTINCT A.AccountName) > 1 THEN '-Multiple-'
		 END as [BankAccount],
CASE WHEN MI.Name is NULL THEN '-' ELSE MI.Name END as [PaymentMethod],
--CASE WHEN BI.Outstanding=0 THEN 'Completed' 
--		WHEN BI.Amount=BI.Outstanding THEN 'Partial'
--		ELSE 'Pending' END		
		'-' as [PaymentStatus] 
from [Transaction] T 
INNER JOIN JournalEntry JE on T.JournalEntryID=JE.ID
INNER JOIN CreditTerm CT on CT.ID=JE.ID
INNER JOIN BillEntryInformation BI on BI.JournalEntryID=JE.ID
LEFT OUTER JOIN MiscInfo MI on MI.ID=JE.PaymentMethodID
LEFT OUTER JOIN BillPaymentsInformation BPI on BPI.BillInformationID=BI.ID
LEFT OUTER JOIN Account A on BPI.AccountID=A.ID
where  T.SourceID=@vID
group by JE.ID,JE.EntryNumber,JE.EntryDate,CT.DueDate,T.Amount,MI.Name,BI.outstanding,BI.Amount,JE.Status
)
UNION 
(Select 
JE.ID,
JE.EntryNumber as [BillNumber],
JE.EntryDate as [Date],
CT.DueDate as [DueDate],
T.Amount as [Amount],
CASE WHEN JE.ApprovalStatus=1 THEN 'In Verification'
	WHEN JE.ApprovalStatus=2 THEN 'In Approval'
	WHEN JE.ApprovalStatus=3 THEN 'Approved'
	WHEN JE.ApprovalStatus=4 THEN 'Rejected'
	ELSE '-'
	END as [BillStatus],
CASE WHEN COUNT(A.AccountName) is NULL THEN '-' 
	 WHEN COUNT(DISTINCT A.AccountName)=1 THEN MAX(A.AccountName)
	 WHEN COUNT(DISTINCT A.AccountName) > 1 THEN '-Multiple-'
	 END as [BankAccount],
CASE WHEN MI.Name is NULL THEN '-' ELSE MI.Name END as [PaymentMethod],
'-'  as [PaymentStatus]
from [OCRTransaction] T 
INNER JOIN OCRJournalEntry JE on T.JournalEntryID=JE.ID
INNER JOIN OCRCreditTerm CT on CT.ID=JE.ID
INNER JOIN BillEntryInformation BI on BI.JournalEntryID=JE.ID
LEFT OUTER JOIN MiscInfo MI on MI.ID=JE.PaymentMethodID
LEFT OUTER JOIN BillPaymentsInformation BPI on BPI.BillInformationID=BI.ID
LEFT OUTER JOIN Account A on BPI.AccountID=A.ID
where T.SourceID=@vID 
group by JE.ID,JE.EntryNumber,JE.EntryDate,CT.DueDate,T.Amount,MI.Name,BI.outstanding,BI.Amount,JE.ApprovalStatus
)
) as ResultantTable
order by ResultantTable.Date desc

END
GO

CREATE procedure [dbo].[API_Payables_SplitBillEntryValidation]
(
@JEID VARCHAR(50)=NULL,
@BillNumbersList varchar(1000)=NULL,
@CorpID varchar(50),
@VenID varchar(50)
)
AS
BEGIN
DECLARE @duplicateBillErrorMessage VARCHAR(100) ='Duplicate number is not allowed to save for multiple bills'
DECLARE @journalID BINARY(18)=CONVERT(BINARY(18), @JEID, 2) 
DECLARE @cID BINARY(18)=CONVERT(BINARY(18), @CorpID, 2) 
DECLARE @vID BINARY(18)=CONVERT(BINARY(18), @VenID, 2) 

Create table #tempSplit(items varchar(50))

Insert into #tempSplit
SELECT items AS BillNumberList  FROM dbo.Split(@BillNumbersList,',')

IF ((Select count(distinct(items)) from #tempSplit) <> (Select COUNT(items) from #tempSplit))
BEGIN
Select @duplicateBillErrorMessage as [Status]	
RETURN
END

Declare @mainBillNumber varchar(100)= (Select TOP 1 items from #tempSplit)

Delete TOP (1)  from #tempSplit

DECLARE @resJID BINARY(18)
DECLARE @resJIDCount INT=0
Select @resJID=(Select TOP 1
				JE.ID
				from JournalEntry JE
				INNER JOIN [Transaction] T on JE.ID=T.JournalEntryID
				where T.status<>3 AND T.parentID is null AND JE.EntryNumber=@mainBillNumber
				AND (JE.Status=1 OR Je.Status is NULL OR T.status=5) AND JE.CorporationID=@cID
				AND T.SourceID=@vID AND JE.SourceType=21)

				Select @resJIDCount=(Select COUNT(JE.ID)
				from JournalEntry JE
				INNER JOIN [Transaction] T on JE.ID=T.JournalEntryID
				where T.status<>3 AND T.parentID is null AND JE.EntryNumber IN (Select items from #tempSplit)
				AND (JE.Status=1 OR Je.Status is NULL OR T.status=5) AND JE.CorporationID=@cID
				AND T.SourceID=@vID AND JE.SourceType=21)


				IF(@resJID<>@journalID OR @resJIDCount>0)
				BEGIN 
				Select @duplicateBillErrorMessage as [Status]	
				RETURN
				END

				--IF No Validation ISsues
				SELECT NULL as [Status]		
END

GO

ALTER procedure [dbo].[API_Payables_LoadBillPaymentsList]
(
@CorpID VARCHAR(50)=NULL,
@VenID VARCHAR(50)=NULL
)
AS 
BEGIN

DECLARE @cID BINARY(18)=CONVERT(BINARY(18), @CorpID, 2) 
DECLARE @vID BINARY(18)=CONVERT(BINARY(18), @VenID, 2) 
Select 
CONVERT(varchar(50), JE.ID, 2) as [JEID],
case when COUNT(DISTINCT(T2.StoreID))>1 THEN 'Multiple' ELSE  Max(S.Name) END as [PCName],
B.Name as [PayeeName],CONVERT(varchar(50), B.ID, 2) as [PayeeID],VC.AccountNumber,JE.EntryDate as [BooksDate],CT.DueDate,JE.BillDate,EntryNumber as [BillNumber],T.Amount,BEI.Outstanding  as [AmountDue] 
,CONVERT(varchar(50), VC.ID , 2)as [ContractID]
, CASE WHEN JE.SourceType = 38 THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END as [IsDebitMemo]
from 
JournalEntry JE
Inner JOIN [Transaction] T on JE.ID=T.JournalEntryID
LEFT JOIN [Transaction] T2 on JE.ID=T2.JournalEntryID
INNER JOIN [TransactionInvoice] TI on TI.ID=T.ID
Inner Join BillEntryInformation BEI on BEI.JournalEntryID=JE.ID
INNER JOIN Account A on A.ID=T.AccountID
INNER JOIN Business B on T.SourceID=B.ID
INNER JOIN VendorContract VC on VC.BusinessID=B.ID
LEFT JOIN Store S on S.ID=T2.StoreID
LEFT JOIN CreditTerm CT on CT.ID=JE.ID

Where JE.CorporationID=@cID AND T.ParentID IS NULL
AND JE.Status=1 AND JE.SourceType IN (21,38) AND BEI.Outstanding>0 
and (@vID is NULL OR T.SourceID=@vID) AND TI.AccContractID=VC.ID
group by 
B.Name,B.ID,VC.AccountNumber,JE.EntryDate,CT.DueDate,JE.BillDate,JE.EntryNumber,T.Amount,BEI.Outstanding,JE.ID,JE.SourceType,VC.ID
order by JE.EntryDate,JE.EntryNumber

END
go

CREATE PROCEDURE [dbo].[API_Payables_LoadBillPaymentsList_New]
(
    @CorpID VARCHAR(50) = NULL,
    @VenID VARCHAR(50) = NULL,
    @SortOrder SMALLINT = -1,
    @FromDate DATETIME2 = NULL,
    @ToDate DATETIME2 = NULL,
    @DueDate DATE = NULL -- Changed to DATE for consistency with CT.DueDate,
	,@SkipJEIDs Varchar(max)=NULL
)
AS 
BEGIN
    
    DECLARE @cID BINARY(18) = CONVERT(BINARY(18), @CorpID, 2); 
    DECLARE @vID BINARY(18) = CONVERT(BINARY(18), @VenID, 2); 

    SELECT 
        CONVERT(VARCHAR(50), JE.ID, 2) AS [JEID],
        CASE 
            WHEN COUNT(DISTINCT(BEID.PCID)) > 1 THEN 'Multiple' 
            ELSE MAX(S.Name) 
        END AS [PCName],
        B.Name AS [PayeeName],
        CONVERT(VARCHAR(50), B.ID, 2) AS [PayeeID],
        VC.AccountNumber,
        JE.EntryDate AS [BooksDate],
        CT.DueDate,
        JE.BillDate,
        JE.EntryNumber AS [BillNumber],
        T.Amount,
        BEI.Outstanding AS [AmountDue],
        CONVERT(VARCHAR(50), VC.ID, 2) AS [ContractID],
        CASE 
            WHEN JE.SourceType = 38 THEN CAST(1 AS BIT) 
            ELSE CAST(0 AS BIT) 
        END AS [IsDebitMemo]
    FROM 
        JournalEntry JE
    INNER JOIN [Transaction] T ON JE.ID = T.JournalEntryID
    LEFT JOIN [Transaction] T2 ON JE.ID = T2.JournalEntryID
    INNER JOIN [TransactionInvoice] TI ON TI.ID = T.ID
    INNER JOIN BillEntryInformation BEI ON BEI.JournalEntryID = JE.ID
	INNER JOIN BillEntryInformationDetails BEID on BEID.billinfoID=BEI.ID
    INNER JOIN Account A ON A.ID = T.AccountID
    INNER JOIN Business B ON T.SourceID = B.ID
    INNER JOIN VendorContract VC ON VC.BusinessID = B.ID
    LEFT JOIN Store S ON S.ID = BEID.PCID
    LEFT JOIN CreditTerm CT ON CT.ID = JE.ID
    WHERE 
        JE.CorporationID = @cID 
        AND T.ParentID IS NULL
        AND JE.Status = 1 
        AND JE.SourceType IN (21, 38) 
        AND BEI.Outstanding > 0 
        AND (@vID IS NULL OR T.SourceID = @vID)
        AND TI.AccContractID = VC.ID 
        AND (
            (@FromDate IS NULL AND @ToDate IS NULL) 
            OR (JE.EntryDate >= @FromDate AND JE.EntryDate <= @ToDate)
        )
        AND (@DueDate IS NULL OR CT.DueDate <= @DueDate)
		AND  (@SkipJEIDs IS NULL OR  
			  JE.ID NOT IN (SELECT CONVERT(BINARY(18),'0x'+items,1) AS PCID  FROM dbo.Split(@SkipJEIDs,','))) 
    GROUP BY 
        B.Name, B.ID, VC.AccountNumber, JE.EntryDate, CT.DueDate, JE.BillDate, JE.EntryNumber, T.Amount, BEI.Outstanding, JE.ID, JE.SourceType, VC.ID
    ORDER BY 
           case  WHEN @SortOrder = 0 THEN B.Name end,
           CASE WHEN @SortOrder = 1 THEN JE.EntryDate END,
           CASE WHEN @SortOrder = 2 THEN CT.DueDate END,
           CASE WHEN @SortOrder = 3 THEN  BEI.Outstanding END,
           CASE WHEN @SortOrder = 4 THEN JE.BillDate END,
           case when  @SortOrder = -1 then JE.EntryDate end --default sorting--
         
END
GO
CREATE procedure [dbo].[API_Payables_LoadBillPaymentPCWise]
(
@JEID VARCHAR(50)=NULL
)
AS 
BEGIN

DECLARE @jID BINARY(18)=CONVERT(BINARY(18), @JEID, 2) 

Select CONVERT(varchar(50), JEID, 2) as [JEID],CONVERT(varchar(50), SID, 2) as [StoreID],
Name as [PCName],PayeeName, PayeeID,AccountNumber,EntryDate as [BooksDate],BillDate,DueDate,EntryNumber as[BillNumber],SUM(Amount) as [Amount],SUM(AmountDue) as [AmountDue] ,[IsDebitMemo],ContractID
from(
Select
JE.ID as [JEID],
S.ID as [SID],
CASE WHEN S.Name IS NULL THEN 'Unclassified' ELSE S.Name END as[Name],
 B.Name as [PayeeName],CONVERT(varchar(50), B.ID, 2) as [PayeeID],VC.AccountNumber,JE.EntryDate,JE.BillDate,CT.DueDate,JE.EntryNumber,CONVERT(varchar(50),VC.ID, 2) as ContractID,
--CASE WHEN OriginalAmount IS NULL THEN SUM(T.Amount) ELSE OriginalAmount END as [Amount],
--CASE WHEN OriginalAmount IS NULL THEN SUM(T.Amount)
--ELSE OriginalAmount+(SUM(PaidAmount)/(CASE WHEN COUNT(DISTINCT(t.StoreID)) =0 THEN 1 ELSE COUNT(DISTINCT(t.StoreID)) END )) END  as [AmountDue]
BD.Amount as [Amount],
BD.Outstanding as [AmountDue],
CASE WHEN JE.SourceType = 38 THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END as [IsDebitMemo]
from 
JournalEntry JE
Inner JOIN [Transaction] T on JE.ID=T.JournalEntryID
Inner JOIN [Transaction] T2 on JE.ID=T2.JournalEntryID
INNER JOIN [TransactionInvoice] TI on TI.ID=T2.ID
Inner Join BillEntryInformation BEI on BEI.JournalEntryID=JE.ID
INNER JOIN Business B on T2.SourceID=B.ID
INNER JOIN VendorContract VC on VC.BusinessID=B.ID
LEFT JOIN Store S on S.ID=T.StoreID
LEFT JOIN BillEntryInformationDetails  BD on S.ID=BD.PCID AND BEI.ID=BD.BillInfoID
--LEFT JOIN BillPaymentDetails BPD on BPD.JournalEntryID=JE.ID AND BPD.Amount=T.Amount
LEFT JOIN CreditTerm CT on CT.ID=JE.ID
where JE.ID=@jID 
AND JE.Status=1 AND JE.SourceType IN (21,38) AND BEI.Outstanding>0  
AND T.ParentID IS NOT NULL AND T2.ParentID IS NULL AND TI.AccContractID=VC.ID 
GROUP BY --BPD.AccountID,OriginalAmount ,
BD.Amount,BD.Outstanding,
JE.EntryNumber,JE.BillDate,CT.DueDate,B.Name,B.ID,VC.AccountNumber,JE.EntryDate,S.Name,JE.ID,S.ID,JE.SourceType,VC.ID
) AS TEMP
Group by [Name],[PayeeName],PayeeID,AccountNumber,EntryDate,BillDate,DueDate,EntryNumber,JEID,SID,IsDebitMemo,ContractID
END


GO


CREATE OR ALTER PROCEDURE [dbo].[API_Payable_GetUseTaxTransactions]
@AddressID bigint,
@BusinessID varbinary(18),
@GetDefaultTrans bit
AS
BEGIN
	IF @GetDefaultTrans=1
	BEGIN
		select CONVERT(VARCHAR(50),t1.ID,2) AS  TransactionID,je.EntryDate EntryDate 
		from [Transaction] t1
		join [Transaction] t2 on t1.ParentID=t2.ID
		join JournalEntry je on t1.journalentryid=je.id
		join TransactionInvoice ti on ti.ID=t1.ID
		join TransactionTaxInfo tti on tti.TransactionID=t1.ID
		left join VendorContract vc on ti.AccContractID=vc.ID or ti.AccContractID is null
		where t2.SourceID=@BusinessID and t1.status!=3 and tti.Status=1 and vc.AddressID=@AddressID
		union
		select CONVERT(VARCHAR(50),t1.ID,2) AS TransactionID,je.EntryDate EntryDate
		from [Transaction] t1
		join JournalEntry je on t1.journalentryid=je.id
		join TransactionInvoice ti on ti.ID=t1.ID
		join TransactionTaxInfo tti on tti.TransactionID=t1.ID
		where t1.TargetID=@BusinessID and t1.status!=3 and tti.Status=1
	END

	ELSE
	BEGIN
		select CONVERT(VARCHAR(50),t1.ID,2) As TransactionID,je.EntryDate EntryDate
		from [Transaction] t1
		join [Transaction] t2 on t1.ParentID=t2.ID
		join JournalEntry je on t1.journalentryid=je.id
		join TransactionInvoice ti on ti.ID=t1.ID
		join TransactionTaxInfo tti on tti.TransactionID=t1.ID
		left join VendorContract vc on ti.AccContractID=vc.ID 
		where t2.SourceID=@BusinessID and t1.status!=3 and tti.Status=1 and vc.AddressID=@AddressID
	END
END
GO

CREATE PROCEDURE [dbo].[API_Payables_GetBusinessBalance] 
 (
	@VenID BINARY(18)
 )
AS
BEGIN
	CREATE TABLE #tempBusinessBalance (     
      ID BINARY(18),
      JournalEntryID BINARY(18),
      Amount DECIMAL(18,2))	
	 
    DECLARE @AccountPayableOrReceivableID AS BINARY(18)=0x0FA700000000000000000000000000000012
    DECLARE @DebitCredit AS BIT=1
    DECLARE @SourceType AS SMALLINT=15
	DECLARE @VoidSourceType AS SMALLINT=163
	DECLARE @Type AS SMALLINT =1	
	DECLARE @CurrentDate AS DATETIME2(0)=GETDATE()	  
	 
		  
			  INSERT INTO #tempBusinessBalance(ID,JournalEntryID,Amount)	
			  SELECT B.ID AS ID,T.JournalEntryID,SUM(CASE WHEN T.DebitCredit=@DebitCredit THEN T.Amount ELSE -1*T.Amount END) AS Amount FROM [Transaction] T INNER JOIN  Business B ON T.SourceID=B.ID
			  INNER JOIN JournalEntry  J ON T.JournalEntryID=J.ID
			  INNER JOIN Account A ON T.AccountID=A.ID
			  WHERE J.EntryDate<=@CurrentDate AND  T.SourceType=@Type AND  T.SourceID=@VenID AND B.[Type]=@Type AND A.AccountTypeID=@AccountPayableOrReceivableID AND T.[Status]<>3 AND B.[Status]<>3
			  GROUP BY B.ID, T.JournalEntryID
			
			  INSERT INTO #tempBusinessBalance(ID,JournalEntryID,Amount)	
			  SELECT B.ID AS ID,T.JournalEntryID,SUM(CASE WHEN T.DebitCredit=@DebitCredit THEN T.Amount ELSE -1*T.Amount END) AS Amount FROM [Transaction] T 
			  INNER JOIN JournalEntry  J ON T.JournalEntryID=J.ID
			  INNER JOIN Account A ON T.AccountID=A.ID
			  INNER JOIN [Transaction] TP ON T.ParentID=TP.ID
			  INNER JOIN Business B ON TP.SourceID=B.ID
			  WHERE J.EntryDate<=@CurrentDate AND  T.SourceType=97 AND (J.SourceType =@SourceType OR J.SourceType=@VoidSourceType)  AND  TP.SourceType=@Type AND  T.SourceID=@VenID AND B.[Type]=@Type AND A.AccountTypeID=@AccountPayableOrReceivableID AND T.[Status]<>3 AND B.[Status]<>3
			  GROUP BY B.ID, T.JournalEntryID
			  
			  INSERT INTO #tempBusinessBalance(ID,JournalEntryID,Amount)	
			  SELECT B.ID AS ID,T.JournalEntryID,SUM(CASE WHEN T.DebitCredit=@DebitCredit THEN -1*T.Amount ELSE T.Amount END) AS Amount FROM [Transaction] T 
			  INNER JOIN JournalEntry  J ON T.JournalEntryID=J.ID
			  INNER JOIN Account A ON T.AccountID=A.ID
			  INNER JOIN [Transaction] TP ON T.ParentID=TP.ID
			  INNER JOIN Business B ON TP.SourceID=B.ID
			  WHERE J.EntryDate<=@CurrentDate AND  T.SourceType=85 AND (J.SourceType =@SourceType OR J.SourceType=@VoidSourceType)  AND  TP.SourceType=@Type AND  T.SourceID=@VenID AND B.[Type]=@Type  AND T.[Status]<>3 AND B.[Status]<>3
			  GROUP BY B.ID, T.JournalEntryID
			  
			  INSERT INTO #tempBusinessBalance(ID,JournalEntryID,Amount)	
			  SELECT B.ID AS ID,T.JournalEntryID,SUM(CASE WHEN T.DebitCredit=@DebitCredit THEN T.Amount ELSE -1*T.Amount END) AS Amount FROM [Transaction] T INNER JOIN  Business B ON T.TargetID=B.ID
			  INNER JOIN JournalEntry  J ON T.JournalEntryID=J.ID
			  INNER JOIN Account A ON T.AccountID=A.ID
			  WHERE J.EntryDate<=@CurrentDate AND  T.TargetType=@Type AND T.SourceID=@VenID AND B.[Type]=@Type AND A.AccountTypeID=@AccountPayableOrReceivableID  AND T.[Status]<>3 AND B.[Status]<>3
			  GROUP BY B.ID, T.JournalEntryID

			  INSERT INTO #tempBusinessBalance(ID,JournalEntryID,Amount)	
			  SELECT T1.ID AS ID,T.SourceID AS JournalEntryID,SUM(CASE WHEN T.DebitCredit=@DebitCredit THEN T.Amount ELSE -1*T.Amount END) AS Amount FROM [Transaction] T INNER JOIN #tempBusinessBalance T1 ON T.SourceID=T1.JournalEntryID
			  INNER JOIN JournalEntry  J ON T.JournalEntryID=J.ID
			  INNER JOIN Account A ON T.AccountID=A.ID
			  WHERE   J.EntryDate<=@CurrentDate   AND T.[Status]<>3
			  GROUP BY T1.ID, T.SourceID	
	
	SELECT SUM(Amount) AS Balance  FROM #tempBusinessBalance
	DROP TABLE #tempBusinessBalance
END

GO

CREATE OR ALTER PROCEDURE [dbo].[API_Payables_GetFilteredVendorIDs] 
    @userID varchar(50),
    @corpID varchar(50)=null,
	@vendorstatus smallint =-1,
	@corpname varchar(50)=null,
	@corpFilterotpion smallint =-1,
	@vendorName varchar(50)=null,
	@VendorFilterOption smallint=-1,
	@Federal varchar(50)=null,
	@FederalFilterOption smallint=-1,
	@SSN varchar(50)=null,
	@SSNFilterOption smallint=-1,
	@phonenumber varchar(50)=null,
    @phoneFilteroption smallint=-1,
	@CorpSort smallint=-1,
	@VendorSort smallint=-1,
	@FederalSort smallint=-1,
	@SSNSort smallint=-1,
	@MobileNumSort smallint =-1,
	@StatusSort smallint=-1,
	@pagenum smallint=0

AS
BEGIN
    -- Drop the temporary table if it exists
    IF OBJECT_ID('tempdb..#VendorIDs') IS NOT NULL DROP TABLE #VendorIDs;
   
	IF OBJECT_ID('tempdb..#BusinessInfo') IS NOT NULL DROP TABLE #BusinessInfo
	CREATE TABLE #BusinessInfo
	(
		RowID BIGINT IDENTITY(1,1) Primary Key,
		ID BINARY(18),
		Corporation VARCHAR(250),
		CorporationID BINARY(18),
		BusinessName VARCHAR(250),
		Federal VARCHAR(250),
		SSN VARCHAR(250),
		Is1099 BIT,
		PayMethod VARCHAR(250),
		CompanyName VARCHAR(250),
		PhoneNumber VARCHAR(250),
		BusinessType VARCHAR(250),
		[Type] SMALLINT,
		[Status] BIT,
		CreditDaysID BINARY(18),
		SortOrder SMALLINT,
		[Order] INT,
		PmSourceType INT,
		PmID BINARY(18),
		PrintType INT,
		AccountNumber VARCHAR(250),
		ContactID BINARY(18),
		TotalRecords BIGINT
	)

    -- Create the temporary table
    CREATE TABLE #VendorIDs (VID BINARY(18));

    IF @corpID IS NOT NULL
		BEGIN
			DECLARE @CID BINARY(18);
			SET @CID = CONVERT(BINARY(18), @corpID, 2);

			INSERT INTO #VendorIDs (VID)
			SELECT DISTINCT bu.ID 
			FROM Business bu 
			WHERE bu.CorporationID = @CID 
			AND bu.[Type] = 1
			AND((@vendorstatus = -1 AND bu.[Status] <> 3)
			OR (@vendorstatus <> -1 AND bu.[Status] = @vendorstatus)
		   );
		END
    ELSE
		BEGIN
			DECLARE @UID BINARY(18);
			SET @UID = CONVERT(BINARY(18), @userID, 2);

			INSERT INTO #VendorIDs (VID)
				select distinct bu.ID from Business bu 
				join Corporation c on bu.CorporationID=c.ID
				join UserCorporation uc on uc.CorporationID=c.ID
				where uc.UserID=@UID
				and c.[Status]=1 
				and bu.[Type]=1
				AND((@vendorstatus = -1 AND bu.[Status] <> 3)
					OR (@vendorstatus <> -1 AND bu.[Status] = @vendorstatus)
					);
			END
	--CorporationName Filter
	IF @corpname IS NOT NULL 
		BEGIN
			DELETE FROM #VendorIDs
			WHERE VID NOT IN (
				SELECT bu.ID
				FROM Business bu
				JOIN Corporation c ON bu.CorporationID = c.ID
				WHERE bu.ID IN (SELECT VID FROM #VendorIDs)
				  AND (
					  (@corpFilterotpion = 0 AND c.CorporationName LIKE @corpname + '%')
					  OR 
					  (@corpFilterotpion = 1 AND c.CorporationName LIKE '%' + @corpname + '%')
					  OR
					  (@corpFilterotpion = 2 AND c.CorporationName NOT LIKE '%' + @corpname + '%')
					  OR
					  (@corpFilterotpion = 3 AND c.CorporationName LIKE '%' + @corpname)
					  OR
					  (@corpFilterotpion = 4 AND c.CorporationName = @corpname)
					  OR
					  (@corpFilterotpion = 5 AND c.CorporationName <> @corpname)
				  )
			);
		END
	--Vendor Name Filter
	IF @vendorName IS NOT NULL
		BEGIN
		DELETE FROM #VendorIDs
			WHERE VID NOT IN (
				SELECT bu.ID
				FROM Business bu
				WHERE bu.ID IN (SELECT VID FROM #VendorIDs)
				  AND (
					  (@VendorFilterOption = 0 AND bu.[Name] LIKE @vendorName + '%')
					  OR 
					  (@VendorFilterOption = 1 AND bu.[Name] LIKE '%' + @vendorName + '%')
					  OR
					  (@VendorFilterOption = 2 AND bu.[Name] NOT LIKE '%' + @vendorName + '%')
					  OR
					  (@VendorFilterOption = 3 AND bu.[Name] LIKE '%' + @vendorName)
					  OR
					  (@VendorFilterOption = 4 AND bu.[Name] = @vendorName)
					  OR
					  (@VendorFilterOption = 5 AND bu.[Name] <> @vendorName)
				  )
			);
		END
    --Federal/ssn Filter
	IF @Federal IS NOT NULL
		BEGIN
		DELETE FROM #VendorIDs
			WHERE VID NOT IN (
				SELECT bu.ID
				FROM Business bu
				JOIN BusinessInfo BI ON BI.ID=BU.ID
				WHERE bu.ID IN (SELECT VID FROM #VendorIDs)
				  AND ( (BI.FederalID IS NOT NULL AND BI.FederalID not like '-%') AND 
					  (@FederalFilterOption = 0 AND BI.FederalID LIKE @Federal + '%')
					  OR 
					  (@FederalFilterOption = 1 AND BI.FederalID LIKE '%' + @Federal + '%')
					  OR
					  (@FederalFilterOption = 2 AND BI.FederalID NOT LIKE '%' + @Federal + '%')
					  OR
					  (@FederalFilterOption = 3 AND BI.FederalID LIKE '%' + @Federal)
					  OR
					  (@FederalFilterOption = 4 AND BI.FederalID = @Federal)
					  OR
					  (@FederalFilterOption = 5 AND BI.FederalID <> @Federal))
			);
		END
	IF @SSN IS NOT NULL
		BEGIN
			DELETE FROM #VendorIDs
			WHERE VID NOT IN (
				SELECT bu.ID
				FROM Business bu
				JOIN BusinessInfo BI ON BI.ID=BU.ID
				WHERE bu.ID IN (SELECT VID FROM #VendorIDs)
				AND (BI.SSN IS NOT NULL AND BI.SSN not like '-%') AND 
					  (@SSNFilterOption = 0 AND BI.SSN LIKE @SSN + '%')
					  OR 
					  (@SSNFilterOption = 1 AND BI.SSN LIKE '%' + @SSN + '%')
					  OR
					  (@SSNFilterOption = 2 AND BI.SSN NOT LIKE '%' + @SSN + '%')
					  OR
					  (@SSNFilterOption = 3 AND BI.SSN LIKE '%' + @SSN)
					  OR
					  (@SSNFilterOption = 4 AND BI.SSN = @SSN)
					  OR
					  (@SSNFilterOption = 5 AND BI.SSN <> @SSN));
		END
	IF @phonenumber IS NOT NULL
		BEGIN
		DELETE FROM #VendorIDs
			WHERE VID NOT IN (
				SELECT bu.ID
				FROM Business bu
				JOIN VendorAddress va ON va.BusinessID = BU.ID
				JOIN Contact ct ON ct.ID = va.ContactID
				WHERE bu.ID IN (SELECT VID FROM #VendorIDs)
				AND va.IsDefault=1
				AND (
				  (@phoneFilteroption = 0 AND ct.Phone1 LIKE @phonenumber + '%')
				  OR 
				  (@phoneFilteroption = 1 AND ct.Phone1 LIKE '%' + @phonenumber + '%')
				  OR
				  (@phoneFilteroption = 2 AND ct.Phone1 NOT LIKE '%' + @phonenumber + '%')
				  OR
				  (@phoneFilteroption = 3 AND ct.Phone1 LIKE '%' + @phonenumber)
				  OR
				  (@phoneFilteroption = 4 AND ct.Phone1 = @phonenumber)
				  OR
				  (@phoneFilteroption = 5 AND ct.Phone1 <> @phonenumber)
				));
		END

	DECLARE @IsLegalName INT=0
	SELECT @IsLegalName=ISNULL(COUNT(ID),0) FROM UserPreferenceSettings WHERE UserID=@UserID AND IsCorpLegalNameShow=1

	INSERT INTO #BusinessInfo(ID,CorporationID,Corporation,BusinessName,Federal,SSN,Is1099,PayMethod,[Type],[Status],CreditDaysID,SortOrder,[Order],
	PmSourceType,PmID,PrintType,AccountNumber,ContactID,TotalRecords)
	SELECT B.ID,C.ID AS CorporationID,(CASE WHEN @IsLegalName>0 THEN C.LegalName  ELSE  C.CorporationName END) AS Corporation,B.[Name] AS BusinessName
		,CASE WHEN BI.FederalID LIKE '%[0-9]%' THEN BI.FederalID ELSE NULL END
		,CASE WHEN BI.SSN LIKE '%[0-9]%' THEN BI.SSN ELSE NULL END
		,BI.Is1099,MI.Name,B.[Type],B.Status,BI.CreditDaysID,ISNULL(C.SortOrder,0) AS SortOrder,
		CASE WHEN C.SortOrder=2 THEN ISNULL(C.[Order],1000) ELSE 1000 END AS [Order],0 AS PmSourceType,BI.PaymentMethodID AS PmID,0 AS PrintType,
		NULL AS AccountNumber,B.ContactID,(SELECT COUNT(*) FROM #VendorIDs) AS TotalRecords
		FROM Business B 
		INNER JOIN BusinessInfo BI ON B.ID=BI.ID 
		INNER JOIN Corporation C ON B.CorporationID=C.ID
		LEFT OUTER JOIN MiscInfo MI ON BI.PaymentMethodID= MI.ID
		WHERE B.ID IN(SELECT VID FROM #VendorIDs)

		UPDATE B 
		SET B.PmSourceType = M.SourceType,
			B.PrintType = ISNULL(M.PrintType, 0)
			FROM #BusinessInfo B
		INNER JOIN MiscInfo M ON B.PmID = M.ID;

		UPDATE BI SET BI.CompanyName=BB.CompanyName FROM #BusinessInfo BI INNER JOIN
		(SELECT (CASE WHEN COUNT(C.ID)=1 THEN MAX(C.Email) ELSE 'Multiple' END) AS CompanyName,B.ID
		FROM #BusinessInfo B 
		INNER JOIN VendorAddress VA ON B.ID=VA.BusinessID
		INNER JOIN Contact C ON VA.ContactID=C.ID
		WHERE C.Email IS NOT NULL AND VA.[Status]<>3
		GROUP BY B.ID) AS BB ON BI.ID=BB.ID

		UPDATE BI SET BI.PhoneNumber= BB.Phone1 FROM #BusinessInfo BI INNER JOIN
		(SELECT C.Phone1,B.ID
		FROM #BusinessInfo B 
		INNER JOIN VendorAddress VA ON B.ID=VA.BusinessID
		INNER JOIN Contact C ON VA.ContactID=C.ID
		WHERE VA.IsDefault=1 AND VA.[Status]<>3
		 ) AS BB ON BI.ID=BB.ID

		 UPDATE BI SET BI.BusinessType= BB.BusinessType FROM #BusinessInfo BI INNER JOIN
		(SELECT CASE WHEN COUNT(C.ID)=1 THEN MAX(C.[Name]) ELSE 'Multiple' END  AS BusinessType,B.ID
		FROM #BusinessInfo B INNER JOIN VendorAddress VA ON B.ID=VA.BusinessID
		INNER JOIN MiscInfo C ON VA.BusinessTypeID=C.ID
		WHERE VA.[Status]<>3 AND VA.BusinessTypeID IS NOT NULL
		 GROUP BY B.ID) AS BB ON BI.ID=BB.ID

		UPDATE BI SET BI.AccountNumber=BB.AccountNumber  FROM #BusinessInfo BI INNER JOIN
		(SELECT (CASE WHEN COUNT(VA.ID)=1 THEN MAX(VA.AccountNumber) WHEN COUNT(VA.ID)>1 THEN CONCAT(COUNT(VA.ID),' Contracts') ELSE ''  END) AS AccountNumber,B.ID
		FROM #BusinessInfo B 
		INNER JOIN VendorContract VA ON B.ID=VA.BusinessID
		WHERE  VA.[Status]<>3
		GROUP BY B.ID )  AS BB ON BI.ID=BB.ID


	SELECT CONVERT(VARCHAR(50),BI.ID,2) as[VendorID],CorporationID,Corporation as [CorporationName],BusinessName as[VendorName],
	Federal as [FederalID],SSN,Is1099,PayMethod, BI.BusinessType as [BusinessType],BI.[Status],CreditDaysID,SortOrder,[Order],PmSourceType,PmID,PrintType,ISNULL(CompanyName,'') AS [Email],ISNULL(PhoneNumber,'') AS [Mobile],ISNULL(AccountNumber,'') AS AccountNumber,ISNULL(BusinessType,'') AS BusinessType 
	,CONVERT(VARCHAR(50),A.ID,2) as [AddressID],VA.ID as [AddressIDLong],TotalRecords as TotalRecords
	,COALESCE(A.Address1 + ', ', '')+ COALESCE(A.Address2 + ', ', '')+COALESCE(A.City + ', ', '')+	COALESCE(ST.Name + ', ', '')+COALESCE(A.ZipCode + ', ', '')+COALESCE(CT.Name + '', '') as [Address]
	FROM #BusinessInfo BI
	LEFT JOIN VendorAddress VA ON VA.BusinessID=BI.ID AND VA.IsDefault=1
	LEFT JOIN Address A on A.ID=VA.AdressID	
	LEFT JOIN Country CT on CT.ID=A.Country
	LEFT JOIN State ST on ST.ID=A.State
	ORDER BY 
		CASE WHEN @CorpSort = 0 THEN BI.Corporation END ASC,
		CASE WHEN @CorpSort = 1 THEN BI.Corporation END DESC,
		CASE WHEN @VendorSort = 0 THEN BI.BusinessName END ASC,
		CASE WHEN @VendorSort = 1 THEN BI.BusinessName END DESC,
		CASE WHEN @FederalSort = 0 THEN BI.Federal END ASC,
		CASE WHEN @FederalSort = 1 THEN BI.Federal END DESC,
		CASE WHEN @SSNSort = 0 THEN BI.Federal END ASC,
		CASE WHEN @SSNSort = 1 THEN BI.Federal END DESC,
		CASE WHEN @MobileNumSort = 0 THEN BI.PhoneNumber END ASC,
		CASE WHEN @MobileNumSort = 1 THEN BI.PhoneNumber END DESC,
		CASE WHEN @StatusSort = 0 THEN BI.[Status] END ASC,
		CASE WHEN @StatusSort = 1 THEN BI.[Status] END DESC
		OFFSET     (@pagenum*40) ROWS       -- skip rows according to page number
		FETCH NEXT 40 ROWS ONLY;;

	    DROP TABLE #BusinessInfo;
		DROP TABLE #VendorIDs;

END
GO

CREATE OR ALTER PROCEDURE [dbo].[API_Payable_GetBillEntryLookUpData]
@corpID VARCHAR
(50)='000000000000000000000000000000000000'

AS
BEGIN
	IF OBJECT_ID('tempdb..####LookUp') IS NOT NULL DROP TABLE #LookUp;
	CREATE TABLE #LookUp
	(
		 BillType SMALLINT,
        TypeName VARCHAR(50),
        TypeCount BIGINT,         -- Changed from BINARY(18) to BIGINT to store counts
        Balance DECIMAL(18, 2) 
	);

	--Summarry 0
	--TobeApproved 1
	--pendinpayments 2
	--debitmemo 3

	DECLARE @CID BINARY(18)= CONVERT(binary(18),@corpID,2);

	INSERT INTO #LookUp(TypeCount,BillType,TypeName,Balance)
	SELECT  COUNT( DISTINCT OJ.ID),1,'TobeApproved',SUM(OT.Amount) 
	FROM OCRJournalEntry OJ 
	INNER JOIN OCRTransaction OT ON OT.JOURNALENTRYID=OJ.ID
	WHERE (@CID= 0X000000000000000000000000000000000000 OR OJ.CorporationID=@CID) 
	AND OJ.ParentSourceType=21
	AND OJ.Status=1 
	AND OJ.ApprovalStatus IN(1,2,4) 
	AND OT.PARENTID IS NULL;

	INSERT INTO #LookUp(TypeCount,BillType,TypeName,Balance)
	SELECT  COUNT( DISTINCT JE.ID),2,'PendingPayments',SUM(bi.Outstanding) 
	FROM JournalEntry JE 
	inner join BillEntryInformation bi on bi.JournalEntryID=je.ID
	WHERE (@CID= 0X000000000000000000000000000000000000 OR JE.CorporationID=@CID) 
	AND JE.ParentSourceType=21
	AND JE.Status=1 
	AND  bi.Outstanding>0

	INSERT INTO #LookUp(TypeCount,BillType,TypeName,Balance)
	SELECT  COUNT( DISTINCT JE.ID),3,'DebitMemo',SUM(bi.Outstanding) 
	FROM JournalEntry JE 
	inner join BillEntryInformation bi on bi.JournalEntryID=je.ID
	WHERE (@CID= 0X000000000000000000000000000000000000 OR JE.CorporationID=@CID) 
	AND JE.ParentSourceType=38
	AND JE.Status=1 
	AND  bi.Outstanding>0

	INSERT INTO #LookUp(TypeCount,BillType,TypeName,Balance)
	SELECT  SUM(LK.TypeCount),0,'Summary',SUM(LK.Balance) 
	FROM #LookUp LK

	SELECT * FROM #LookUp
	DROP TABLE #LookUp
END
GO

CREATE OR ALTER PROCEDURE [dbo].[API_Payable_BillEntriesFilteredData]
	@UserID BINARY(18),
	@ReqID BIGINT,
	@BillType SMALLINT=0,--FilterType
	@CorpID BINARY(18)=0x000000000000000000000000000000000000,
	@FromDate DATETIME2(0)=NULL,
	@ToDate DATETIME2(0)=NULL,
	@viewType SMALLINT,
	@PageNo SMALLINT
AS
BEGIN
	CREATE TABLE #CorpInfo
	(
		RowID BIGINT IDENTITY(1,1),
		ID BINARY(18),
		CorporationName VARCHAR(500),
		LegalName VARCHAR(500)
	)

		IF OBJECT_ID('tempdb.. ####filterdata') IS NOT NULL DROP TABLE #filterdata;
	CREATE TABLE #filterdata 
	(
		BID BINARY(18),
		EntryType VARCHAR(50),
		ParentTransID BINARY(18),
		Memo VARCHAR(1000),
		BooksDate DateTime2,
		ModifiedDate DATETIME2,
		CorporationID BINARY(18),
		CorporationName VARCHAR(500),
		LegalName VARCHAR(500),
		BillNum VARCHAR(250),
		BillDate DATETIME2,
		VendorID BINARY(18),
		VendorName VARCHAR(500),
		AccountNumber VARCHAR(500),
		PayMethodID BINARY(18),
		PayMethodName VARCHAR(500),
		DueDate DATETIME2,
		CreatedByID BINARY(18),
		CreatedBy VARCHAR(500),
		CreatedDate DATETIME2,
		AssignedToID BINARY(18),
		AssignedTo VARCHAR(50),
		RecievedVia VARCHAR(50),
		Amount bigint,
		Outstanding BIGINT,
		[Status] VARCHAR(50),
		JournalStatus SMALLINT,
		ApprovalStatus VARCHAR(50),
		TotalCount bigint
	)

	IF OBJECT_ID('tempdb.. ###BillData') IS NOT NULL DROP TABLE #BillData;
	CREATE TABLE #BillData 
	(
		BID BINARY(18),
		EntryType VARCHAR(50),
		ParentTransID BINARY(18),
		Memo VARCHAR(1000),
		BooksDate DateTime2,
		ModifiedDate DATETIME2,
		CorporationID BINARY(18),
		CorporationName VARCHAR(500),
		LegalName VARCHAR(500),
		BillNum VARCHAR(250),
		BillDate DATETIME2,
		VendorID BINARY(18),
		VendorName VARCHAR(500),
		AccountNumber VARCHAR(500),
		PayMethodID BINARY(18),
		PayMethodName VARCHAR(500),
		DueDate DATETIME2,
		CreatedByID BINARY(18),
		CreatedBy VARCHAR(500),
		CreatedDate DATETIME2,
		AssignedToID BINARY(18),
		AssignedTo VARCHAR(50),
		RecievedVia VARCHAR(50),
		Amount bigint,
		Outstanding BIGINT,
		[Status] VARCHAR(50),
		JournalStatus SMALLINT,
		ApprovalStatus VARCHAR(50),
		TotalCount bigint
	)

	IF @CorpID=0x000000000000000000000000000000000000
	BEGIN
		INSERT INTO #CorpInfo(ID,CorporationName,LegalName)
		SELECT C.ID,C.CorporationName,C.LegalName  FROM Corporation C INNER JOIN UserCorporation UC ON C.ID=UC.CorporationID
		WHERE UC.UserID=@UserID
	END

	ELSE
	BEGIN
		INSERT INTO #CorpInfo(ID,CorporationName,LegalName)
		SELECT C.ID,C.CorporationName,C.LegalName  FROM Corporation C 
		WHERE C.ID=@CorpID
	END

	IF @BillType IN (0,1)
		BEGIN
			INSERT INTO #BillData (BID,ParentTransID,BooksDate,CorporationID,BillNum,BillDate,VendorID,PayMethodID,CreatedByID,CreatedDate,AssignedTo,Amount,ApprovalStatus,ModifiedDate)	
			SELECT DISTINCT OJ.ID, OT.ID,oj.EntryDate BooksDate,OJ.CorporationID CorporationID,OJ.EntryNumber BillNum,OJ.BillDate BillDate, OT.SourceID VendorID,
			oj.PaymentMethodID PayMethodID,OJ.CreatedBy CreatedByID ,OJ.CreatedDate CreatedDate,oj.AssignedTo AssignedTo,ot.Amount Amount,
			(CASE
				WHEN  OJ.ApprovalStatus=1 THEN 'Entry'
				WHEN OJ.ApprovalStatus=2 THEN 'InVerification'
				WHEN OJ.ApprovalStatus=4 THEN 'Rejected'
									END)ApprovalStatus,OJ.ModifiedDate  
			FROM OCRJournalEntry OJ 
			INNER JOIN #CorpInfo C ON OJ.CorporationID=C.ID
			LEFT OUTER JOIN OCRTransaction OT ON OT.JournalEntryID=OJ.ID
			AND (@FromDate IS NULL OR OJ.EntryDate>= @FromDate) AND ( @ToDate IS NULL OR  OJ.EntryDate<=@ToDate)
			AND OT.ParentID  IS NULL
			AND OJ.[Status]<>3 
			AND OJ.ParentSourceType=21 
		END
	IF @BillType IN (0,2)
		BEGIN
			INSERT INTO #BillData (BID,EntryType,ParentTransID,Memo,BooksDate,CorporationID,BillNum,BillDate,VendorID,PayMethodID,Amount,Outstanding,[Status],CreatedBy,ModifiedDate)
			SELECT JE.ID,(CASE WHEN JE.PARENTSOURCETYPE=21 THEN 'Bill' END) EntryType,T.ID ParentTransID,T.MEMO Memo,JE.ENTRYDATE BooksDate,
			JE.CORPORATIONID CorporationID,JE.ENTRYNUMBER BillNum,JE.BILLDATE BillDate,T.SOURCEID VendorID,JE.PaymentMethodID PayMethodID,
			T.Amount Amount,ABS(BI.Outstanding),(CASE	
					WHEN BI.Outstanding=0 THEN 'Paid'
					WHEN BI.Outstanding=T.Amount THEN 'UnPaid'
					WHEN BI.Outstanding < T.Amount THEN 'PartialPaid'
					END) [Status],je.CreatedBy CreatedBy,JE.ModifiedDate
			FROM JournalEntry JE
			INNER JOIN #CorpInfo C ON JE.CorporationID=C.ID
			LEFT OUTER JOIN [TRANSACTION] T ON T.JOURNALENTRYID=JE.ID
			LEFT OUTER JOIN BillEntryInformation BI ON BI.JournalEntryID=JE.ID
			WHERE JE.PARENTSOURCETYPE=21 AND  T.PARENTID IS NULL
			AND (@FromDate IS NULL OR JE.EntryDate>= @FromDate) AND ( @ToDate IS NULL OR  JE.EntryDate<=@ToDate)
			AND JE.Status NOT IN(2,3)
		END
	IF @BillType IN (0,3)
		BEGIN
			INSERT INTO #BillData (BID,ParentTransID,Memo,BooksDate,CorporationID,BillNum,BillDate,VendorID,Amount,Outstanding,[Status],CreatedBy,ModifiedDate)
			SELECT JE.ID,T.ID ParentTransID,T.MEMO Memo,JE.ENTRYDATE BooksDate,
			JE.CORPORATIONID CorporationID,JE.ENTRYNUMBER BillNum,JE.BILLDATE BillDate,T.SOURCEID VendorID,
			T.Amount Amount,ABS(BI.Outstanding),(CASE	
					WHEN BI.Outstanding=0 THEN 'Adjusted'
					WHEN abs(BI.Outstanding)= T.Amount THEN 'open-Credit'
					WHEN abs(BI.Outstanding) < T.Amount THEN 'partly-adjusted'
					END) [Status],je.CreatedBy CreatedBy,JE.ModifiedDate
			FROM JournalEntry JE
			INNER JOIN #CorpInfo C ON JE.CorporationID=C.ID
			LEFT OUTER JOIN [TRANSACTION] T ON T.JOURNALENTRYID=JE.ID
			LEFT OUTER JOIN BillEntryInformation BI ON BI.JournalEntryID=JE.ID
			WHERE JE.PARENTSOURCETYPE=38 AND  T.PARENTID IS NULL
			AND (@FromDate IS NULL OR JE.EntryDate>= @FromDate) AND ( @ToDate IS NULL OR  JE.EntryDate<=@ToDate)
			AND JE.Status NOT IN(2,3)
		END

	UPDATE #BillData SET 
	CorporationName=C.CorporationName,
	VendorName=B.[Name],
	AccountNumber=VC.AccountNumber,
	PayMethodName=MS.[Name],
	DueDate=(CASE WHEN CT.DueDate IS NULL THEN OCT.DueDate
				  WHEN OCT.DueDate IS NULL THEN CT.DueDate
				  ELSE NULL END),
	CreatedBy=U.UserName,
	AssignedTo=U1.FirstName
	FROM #BillData BD
	INNER JOIN Corporation C ON C.ID=BD.CorporationID
	INNER JOIN Business B ON B.ID= BD.VendorID
	LEFT OUTER JOIN [User] U ON U.ID=BD.CreatedByID
	LEFT OUTER JOIN OCRTransactionInvoice OTI ON OTI.ID=BD.ParentTransID
	LEFT OUTER JOIN TransactionInvoice TI ON TI.ID=BD.ParentTransID
	LEFT OUTER JOIN OCRCreditTerm OCT ON OCT.ID=BD.BID
	LEFT OUTER JOIN CreditTerm CT ON CT.ID=BD.BID
	LEFT OUTER JOIN VendorContract VC ON 
    (@BillType = 2 AND VC.ID = TI.AccContractID) OR
    (@BillType = 1 AND VC.ID = OTI.AccContractID) OR
    (@BillType in (0,3) AND VC.ID IN (TI.AccContractID, OTI.AccContractID))
	LEFT OUTER JOIN [User] U1 ON U1.ID =BD.AssignedToID
	LEFT OUTER JOIN MiscInfo MS ON MS.ID=BD.PayMethodID

	SELECT  [ID], [ReqID], [ReqSourceType], [FilterColumnType], [Value], [OperationType] INTO #ApFilterReq FROM [dbo].[APFilterReq] WHERE ReqID=@ReqID

	SELECT [ID], [ReqID], [ReqSourceType], [FilterColumnType], [Value], [OperationType] INTO #1FilterReq  FROM #ApFilterReq WHERE [FilterColumnType]=1
	IF EXISTS(SELECT 1 FROM #1FilterReq)
		BEGIN
		INSERT INTO #filterdata 
			SELECT * FROM #BillData B WHERE EXISTS(SELECT 1 FROM  #1FilterReq F  WHERE F.OperationType=0 AND B.BillNum LIKE F.[Value]+'%')  union
			SELECT * FROM #BillData B WHERE EXISTS(SELECT 1 FROM  #1FilterReq F  WHERE F.OperationType=1 AND B.BillNum LIKE '%'+F.[Value]) union
			SELECT * FROM #BillData B WHERE EXISTS(SELECT 1 FROM  #1FilterReq F  WHERE F.OperationType=2 AND B.BillNum LIKE '%'+F.[Value]+'%')union
			SELECT * FROM #BillData B WHERE EXISTS(SELECT 1 FROM  #1FilterReq F  WHERE F.OperationType=3 AND B.BillNum =F.[Value])
		END
	DROP TABLE #1FilterReq;

	SELECT [ID], [ReqID], [ReqSourceType], [FilterColumnType], [Value], [OperationType] INTO #2FilterReq  FROM #ApFilterReq WHERE [FilterColumnType]=2
	IF EXISTS(SELECT 1 FROM #2FilterReq)
		BEGIN
		INSERT INTO #filterdata 
			SELECT * FROM #BillData B WHERE EXISTS(SELECT 1 FROM  #2FilterReq F  WHERE F.OperationType=0 AND B.VendorName LIKE F.[Value]+'%')  union
			SELECT * FROM #BillData B WHERE EXISTS(SELECT 1 FROM  #2FilterReq F  WHERE F.OperationType=1 AND B.VendorName LIKE '%'+F.[Value])union
			SELECT * FROM #BillData B WHERE EXISTS(SELECT 1 FROM  #2FilterReq F  WHERE F.OperationType=2 AND B.VendorName LIKE '%'+F.[Value]+'%')union
			SELECT * FROM #BillData B WHERE EXISTS(SELECT 1 FROM  #2FilterReq F  WHERE F.OperationType=3 AND B.VendorName =F.[Value])union
			SELECT * FROM #BillData B WHERE EXISTS(SELECT 1 FROM  #2FilterReq F  WHERE F.OperationType=7 AND B.VendorID =CONVERT(BINARY(18),F.[Value],1))

		END
	DROP TABLE #2FilterReq;

	SELECT [ID], [ReqID], [ReqSourceType], [FilterColumnType], [Value], [OperationType] INTO #3FilterReq  FROM #ApFilterReq WHERE [FilterColumnType]=3
	IF EXISTS(SELECT 1 FROM #3FilterReq)
		BEGIN
		INSERT INTO #filterdata 
			SELECT * FROM #BillData B WHERE EXISTS(SELECT 1 FROM  #3FilterReq F  WHERE F.OperationType=0 AND B.AccountNumber LIKE F.[Value]+'%')  union
			SELECT * FROM #BillData B WHERE EXISTS(SELECT 1 FROM  #3FilterReq F  WHERE F.OperationType=1 AND B.AccountNumber LIKE '%'+F.[Value])union
			SELECT * FROM #BillData B WHERE EXISTS(SELECT 1 FROM  #3FilterReq F  WHERE F.OperationType=2 AND B.AccountNumber LIKE '%'+F.[Value]+'%')union
			SELECT * FROM #BillData B WHERE EXISTS(SELECT 1 FROM  #3FilterReq F  WHERE F.OperationType=3 AND B.AccountNumber =F.[Value])
		END
	DROP TABLE #3FilterReq;

	SELECT [ID], [ReqID], [ReqSourceType], [FilterColumnType], [Value], [OperationType] INTO #4FilterReq  FROM #ApFilterReq WHERE [FilterColumnType]=4
	IF EXISTS(SELECT 1 FROM #4FilterReq)
		BEGIN
			INSERT INTO #filterdata 
			SELECT * FROM #BillData B WHERE EXISTS(SELECT 1 FROM  #4FilterReq F  WHERE F.OperationType=7 AND B.PayMethodID= CONVERT(BINARY(18),F.[Value],1))  
		END
	DROP TABLE #4FilterReq;

	SELECT [ID], [ReqID], [ReqSourceType], [FilterColumnType], [Value], [OperationType] INTO #5FilterReq  FROM #ApFilterReq WHERE [FilterColumnType]=5
	IF EXISTS(SELECT 1 FROM #5FilterReq)
		BEGIN
		INSERT INTO #filterdata 
			SELECT * FROM #BillData B WHERE EXISTS(SELECT 1 FROM  #5FilterReq F  WHERE F.OperationType=0 AND B.Memo LIKE F.[Value]+'%')  union
			SELECT * FROM #BillData B WHERE EXISTS(SELECT 1 FROM  #5FilterReq F  WHERE F.OperationType=1 AND B.Memo LIKE '%'+F.[Value])union
			SELECT * FROM #BillData B WHERE EXISTS(SELECT 1 FROM  #5FilterReq F  WHERE F.OperationType=2 AND B.Memo LIKE '%'+F.[Value]+'%')union
			SELECT * FROM #BillData B WHERE EXISTS(SELECT 1 FROM  #5FilterReq F  WHERE F.OperationType=3 AND B.Memo =F.[Value])
		END
	DROP TABLE #5FilterReq;

	SELECT [ID], [ReqID], [ReqSourceType], [FilterColumnType], [Value], [OperationType] INTO #6FilterReq  FROM #ApFilterReq WHERE [FilterColumnType]=6
	IF EXISTS(SELECT 1 FROM #6FilterReq)
		BEGIN
		INSERT INTO #filterdata 
			SELECT * FROM #BillData B WHERE EXISTS(SELECT 1 FROM  #6FilterReq F  WHERE F.OperationType=4 AND B.Amount >=CONVERT(BIGINT,F.[Value]))  union
			SELECT * FROM #BillData B WHERE EXISTS(SELECT 1 FROM  #6FilterReq F  WHERE F.OperationType=5 AND B.Amount <= CONVERT(BIGINT,F.[Value]))union
			SELECT * FROM #BillData B WHERE EXISTS(SELECT 1 FROM  #6FilterReq F  WHERE F.OperationType=6 AND B.Amount = CONVERT(BIGINT,F.[Value]))
		END
	DROP TABLE #6FilterReq;

	SELECT [ID], [ReqID], [ReqSourceType], [FilterColumnType], [Value], [OperationType] INTO #7FilterReq  FROM #ApFilterReq WHERE [FilterColumnType]=7
	IF EXISTS(SELECT 1 FROM #7FilterReq)
		BEGIN
		INSERT INTO #filterdata 
			SELECT * FROM #BillData B WHERE EXISTS(SELECT 1 FROM  #7FilterReq F  WHERE F.OperationType IN(0,7) AND B.Outstanding=0)  union
			SELECT * FROM #BillData B WHERE EXISTS(SELECT 1 FROM  #7FilterReq F  WHERE F.OperationType IN(1,6) AND B.Outstanding=B.Amount)union
			SELECT * FROM #BillData B WHERE EXISTS(SELECT 1 FROM  #7FilterReq F  WHERE F.OperationType IN(2,8) AND B.Outstanding<B.Amount)union
			SELECT * FROM #BillData B WHERE EXISTS(SELECT 1 FROM  #7FilterReq F  WHERE F.OperationType=3 AND B.ApprovalStatus=2)  union
			SELECT * FROM #BillData B WHERE EXISTS(SELECT 1 FROM  #7FilterReq F  WHERE F.OperationType=4 AND B.ApprovalStatus=1)union
			SELECT * FROM #BillData B WHERE EXISTS(SELECT 1 FROM  #7FilterReq F  WHERE F.OperationType=5 AND B.ApprovalStatus=4)
		END
	DROP TABLE #7FilterReq;

	SELECT [ID], [ReqID], [ReqSourceType], [FilterColumnType], [Value], [OperationType] INTO #9FilterReq  FROM #ApFilterReq WHERE [FilterColumnType]=9
	IF EXISTS(SELECT 1 FROM #9FilterReq)
		BEGIN
		INSERT INTO #filterdata 
			SELECT * FROM #BillData B WHERE EXISTS(SELECT 1 FROM  #9FilterReq F  WHERE F.OperationType=0 AND B.CreatedBy LIKE F.[Value]+'%')  union
			SELECT * FROM #BillData B WHERE EXISTS(SELECT 1 FROM  #9FilterReq F  WHERE F.OperationType=1 AND B.CreatedBy LIKE '%'+F.[Value])union
			SELECT * FROM #BillData B WHERE EXISTS(SELECT 1 FROM  #9FilterReq F  WHERE F.OperationType=2 AND B.CreatedBy LIKE '%'+F.[Value]+'%')union
			SELECT * FROM #BillData B WHERE EXISTS(SELECT 1 FROM  #9FilterReq F  WHERE F.OperationType=3 AND B.CreatedBy =F.[Value])union
			SELECT * FROM #BillData B WHERE EXISTS(SELECT 1 FROM  #9FilterReq F  WHERE F.OperationType=7 AND B.CreatedByID =CONVERT(BINARY(18),F.[Value],1))
		END
	DROP TABLE #9FilterReq;

	SELECT [ID], [ReqID], [ReqSourceType], [FilterColumnType], [Value], [OperationType] INTO #10FilterReq  FROM #ApFilterReq WHERE [FilterColumnType]=10
	IF EXISTS(SELECT 1 FROM #10FilterReq)
		BEGIN
		INSERT INTO #filterdata 
			SELECT * FROM #BillData B WHERE EXISTS(SELECT 1 FROM  #10FilterReq F  WHERE F.OperationType=0 AND B.AssignedTo LIKE F.[Value]+'%')  union
			SELECT * FROM #BillData B WHERE EXISTS(SELECT 1 FROM  #10FilterReq F  WHERE F.OperationType=1 AND B.AssignedTo LIKE '%'+F.[Value])union
			SELECT * FROM #BillData B WHERE EXISTS(SELECT 1 FROM  #10FilterReq F  WHERE F.OperationType=2 AND B.AssignedTo LIKE '%'+F.[Value]+'%')union
			SELECT * FROM #BillData B WHERE EXISTS(SELECT 1 FROM  #10FilterReq F  WHERE F.OperationType=3 AND B.AssignedTo =F.[Value])union
			SELECT * FROM #BillData B WHERE EXISTS(SELECT 1 FROM  #10FilterReq F  WHERE F.OperationType=7 AND B.AssignedToID =CONVERT(BINARY(18),F.[Value],1))
		END
	DROP TABLE #10FilterReq;

	update #filterdata set totalcount= (select count(*) from (select distinct BID from #filterdata) as total)
	SELECT DISTINCT top 40 * FROM #filterdata
	DROP TABLE #filterdata
	DROP TABLE #BillData
	DROP TABLE #ApFilterReq

END
GO

CREATE PROCEDURE [dbo].[API_Payables_BusinessBalance] 
 (
	@UserID BINARY(18),
	@Type SMALLINT=1,
	@CorporationID BINARY(18)=NULL
 )
AS
BEGIN
	CREATE TABLE #tempBusinessBalance (     
      ID BINARY(18),
      JournalEntryID BINARY(18),
      Amount DECIMAL(18,2)
      )
	  CREATE TABLE #tempBusinessBalanceCorps (     
      RowNumber BIGINT IDENTITY(1,1) Primary Key,
	  CorporationID BINARY(18)
     
      )
	  IF @CorporationID IS NULL
	  BEGIN
		INSERT INTO #tempBusinessBalanceCorps(CorporationID)
		SELECT DISTINCT CorporationID FROM [UserCorporation]
		WHERE UserID=@UserID
	  END
    DECLARE @AccountPayableOrReceivableID AS BINARY(18)
    DECLARE @DebitCredit AS BIT=1
     DECLARE @SourceType AS SMALLINT=15
	 DECLARE @VoidSourceType AS SMALLINT=163
	IF @Type=1
	BEGIN
	   SET @AccountPayableOrReceivableID=0x0FA700000000000000000000000000000012
	END
	ELSE
	BEGIN
		 SET @DebitCredit=0
		 SET @AccountPayableOrReceivableID=0x0FA700000000000000000000000000000004 
		 SET @SourceType=33
		 SET @VoidSourceType=166
    END
	DECLARE @CurrentDate AS DATETIME2(0)=GETDATE()	  

	
	 IF @CorporationID IS NULL
		   BEGIN
			  INSERT INTO #tempBusinessBalance(ID,JournalEntryID,Amount)	
			  SELECT B.ID AS ID,T.JournalEntryID,SUM(CASE WHEN T.DebitCredit=@DebitCredit THEN T.Amount ELSE -1*T.Amount END) AS Amount FROM [Transaction] T INNER JOIN  Business B ON T.SourceID=B.ID
			  INNER JOIN JournalEntry  J ON T.JournalEntryID=J.ID 
			  INNER JOIN Account A ON T.AccountID=A.ID
			  WHERE J.EntryDate<=@CurrentDate AND  T.SourceType=@Type AND  EXISTS (SELECT 1 FROM #tempBusinessBalanceCorps TUC WHERE TUC.CorporationID=B.CorporationID) AND B.[Type]=@Type AND A.AccountTypeID=@AccountPayableOrReceivableID AND T.[Status]<>3 AND B.[Status]<>3
			  GROUP BY B.ID, T.JournalEntryID
			  --SELECT B.ID AS ID,SUM(CASE WHEN T.DebitCredit=1 THEN T.Amount ELSE -1*T.Amount END) AS Amount FROM [Transaction] T INNER JOIN  Business B ON T.AccountID=B.AccountID
			  --WHERE B.CorporationID=@CorporationID AND B.[Type]=@Type AND T.[Status]<>3 AND B.[Status]<>3
			  --GROUP BY B.ID
			 
			  INSERT INTO #tempBusinessBalance(ID,JournalEntryID,Amount)	
			  SELECT B.ID AS ID,T.JournalEntryID,SUM(CASE WHEN T.DebitCredit=@DebitCredit THEN T.Amount ELSE -1*T.Amount END) AS Amount FROM [Transaction] T 
			  INNER JOIN JournalEntry  J ON T.JournalEntryID=J.ID 
			  INNER JOIN Account A ON T.AccountID=A.ID
			  INNER JOIN [Transaction] TP ON T.ParentID=TP.ID
			  INNER JOIN Business B ON TP.SourceID=B.ID
			  WHERE J.EntryDate<=@CurrentDate AND  T.SourceType=97 AND (J.SourceType =@SourceType OR J.SourceType=@VoidSourceType) AND  TP.SourceType=@Type AND  EXISTS (SELECT 1 FROM #tempBusinessBalanceCorps TUC WHERE TUC.CorporationID=B.CorporationID) AND B.[Type]=@Type AND A.AccountTypeID=@AccountPayableOrReceivableID AND T.[Status]<>3 AND B.[Status]<>3
			  GROUP BY B.ID, T.JournalEntryID
			  
			  INSERT INTO #tempBusinessBalance(ID,JournalEntryID,Amount)	
			  SELECT B.ID AS ID,T.JournalEntryID,SUM(CASE WHEN T.DebitCredit=@DebitCredit THEN -1*T.Amount ELSE T.Amount END) AS Amount FROM [Transaction] T 
			  INNER JOIN JournalEntry  J ON T.JournalEntryID=J.ID
			  INNER JOIN Account A ON T.AccountID=A.ID
			  INNER JOIN [Transaction] TP ON T.ParentID=TP.ID
			  INNER JOIN Business B ON TP.SourceID=B.ID
			  WHERE J.EntryDate<=@CurrentDate AND  T.SourceType=85 AND (J.SourceType =@SourceType OR J.SourceType=@VoidSourceType) AND  TP.SourceType=@Type AND  EXISTS (SELECT 1 FROM #tempBusinessBalanceCorps TUC WHERE TUC.CorporationID=B.CorporationID) AND B.[Type]=@Type  AND T.[Status]<>3 AND B.[Status]<>3
			  GROUP BY B.ID, T.JournalEntryID
			  
			  INSERT INTO #tempBusinessBalance(ID,JournalEntryID,Amount)	
			  SELECT B.ID AS ID,T.JournalEntryID,SUM(CASE WHEN T.DebitCredit=@DebitCredit THEN T.Amount ELSE -1*T.Amount END) AS Amount FROM [Transaction] T INNER JOIN  Business B ON T.TargetID=B.ID
			  INNER JOIN JournalEntry  J ON T.JournalEntryID=J.ID
			  INNER JOIN Account A ON T.AccountID=A.ID
			  WHERE J.EntryDate<=@CurrentDate AND  T.TargetType=@Type AND B.[Type]=@Type AND A.AccountTypeID=@AccountPayableOrReceivableID  AND T.[Status]<>3 AND B.[Status]<>3
			  GROUP BY B.ID, T.JournalEntryID
			  
			  INSERT INTO #tempBusinessBalance(ID,JournalEntryID,Amount)	
			  SELECT T1.ID AS ID,T.SourceID AS JournalEntryID,SUM(CASE WHEN T.DebitCredit=@DebitCredit THEN T.Amount ELSE -1*T.Amount END) AS Amount FROM [Transaction] T INNER JOIN #tempBusinessBalance T1 ON T.SourceID=T1.JournalEntryID
			  INNER JOIN JournalEntry  J ON T.JournalEntryID=J.ID
			  INNER JOIN Account A ON T.AccountID=A.ID
			  WHERE    J.EntryDate<=@CurrentDate   AND T.[Status]<>3 
			  GROUP BY T1.ID, T.SourceID
			  
	
		  END
		  ELSE
		  BEGIN
			  INSERT INTO #tempBusinessBalance(ID,JournalEntryID,Amount)	
			  SELECT B.ID AS ID,T.JournalEntryID,SUM(CASE WHEN T.DebitCredit=@DebitCredit THEN T.Amount ELSE -1*T.Amount END) AS Amount FROM [Transaction] T INNER JOIN  Business B ON T.SourceID=B.ID
			  INNER JOIN JournalEntry  J ON T.JournalEntryID=J.ID
			  INNER JOIN Account A ON T.AccountID=A.ID
			  WHERE J.EntryDate<=@CurrentDate AND  T.SourceType=@Type AND  B.CorporationID=@CorporationID AND B.[Type]=@Type AND A.AccountTypeID=@AccountPayableOrReceivableID AND T.[Status]<>3 AND B.[Status]<>3
			  GROUP BY B.ID, T.JournalEntryID
			  --SELECT B.ID AS ID,SUM(CASE WHEN T.DebitCredit=1 THEN T.Amount ELSE -1*T.Amount END) AS Amount FROM [Transaction] T INNER JOIN  Business B ON T.AccountID=B.AccountID
			  --WHERE B.CorporationID=@CorporationID AND B.[Type]=@Type AND T.[Status]<>3 AND B.[Status]<>3
			  --GROUP BY B.ID
			 
			  INSERT INTO #tempBusinessBalance(ID,JournalEntryID,Amount)	
			  SELECT B.ID AS ID,T.JournalEntryID,SUM(CASE WHEN T.DebitCredit=@DebitCredit THEN T.Amount ELSE -1*T.Amount END) AS Amount FROM [Transaction] T 
			  INNER JOIN JournalEntry  J ON T.JournalEntryID=J.ID
			  INNER JOIN Account A ON T.AccountID=A.ID
			  INNER JOIN [Transaction] TP ON T.ParentID=TP.ID
			  INNER JOIN Business B ON TP.SourceID=B.ID
			  WHERE J.EntryDate<=@CurrentDate AND  T.SourceType=97 AND (J.SourceType =@SourceType OR J.SourceType=@VoidSourceType)  AND  TP.SourceType=@Type AND  B.CorporationID=@CorporationID AND B.[Type]=@Type AND A.AccountTypeID=@AccountPayableOrReceivableID AND T.[Status]<>3 AND B.[Status]<>3
			  GROUP BY B.ID, T.JournalEntryID
			  
			  INSERT INTO #tempBusinessBalance(ID,JournalEntryID,Amount)	
			  SELECT B.ID AS ID,T.JournalEntryID,SUM(CASE WHEN T.DebitCredit=@DebitCredit THEN -1*T.Amount ELSE T.Amount END) AS Amount FROM [Transaction] T 
			  INNER JOIN JournalEntry  J ON T.JournalEntryID=J.ID
			  INNER JOIN Account A ON T.AccountID=A.ID
			  INNER JOIN [Transaction] TP ON T.ParentID=TP.ID
			  INNER JOIN Business B ON TP.SourceID=B.ID
			  WHERE J.EntryDate<=@CurrentDate AND  T.SourceType=85 AND (J.SourceType =@SourceType OR J.SourceType=@VoidSourceType)  AND  TP.SourceType=@Type AND  B.CorporationID=@CorporationID AND B.[Type]=@Type  AND T.[Status]<>3 AND B.[Status]<>3
			  GROUP BY B.ID, T.JournalEntryID
			  
			  INSERT INTO #tempBusinessBalance(ID,JournalEntryID,Amount)	
			  SELECT B.ID AS ID,T.JournalEntryID,SUM(CASE WHEN T.DebitCredit=@DebitCredit THEN T.Amount ELSE -1*T.Amount END) AS Amount FROM [Transaction] T INNER JOIN  Business B ON T.TargetID=B.ID
			  INNER JOIN JournalEntry  J ON T.JournalEntryID=J.ID
			  INNER JOIN Account A ON T.AccountID=A.ID
			  WHERE J.EntryDate<=@CurrentDate AND  T.TargetType=@Type AND B.CorporationID=@CorporationID AND B.[Type]=@Type AND A.AccountTypeID=@AccountPayableOrReceivableID  AND T.[Status]<>3 AND B.[Status]<>3
			  GROUP BY B.ID, T.JournalEntryID

			  INSERT INTO #tempBusinessBalance(ID,JournalEntryID,Amount)	
			  SELECT T1.ID AS ID,T.SourceID AS JournalEntryID,SUM(CASE WHEN T.DebitCredit=@DebitCredit THEN T.Amount ELSE -1*T.Amount END) AS Amount FROM [Transaction] T INNER JOIN #tempBusinessBalance T1 ON T.SourceID=T1.JournalEntryID
			  INNER JOIN JournalEntry  J ON T.JournalEntryID=J.ID
			  INNER JOIN Account A ON T.AccountID=A.ID
			  WHERE   J.EntryDate<=@CurrentDate   AND T.[Status]<>3
			  GROUP BY T1.ID, T.SourceID
			  
		  END
	
	SELECT CONVERT(VARCHAR(50),ID,2) as [ID],SUM(Amount) AS Amount  FROM #tempBusinessBalance Group By ID
	DROP TABLE #tempBusinessBalance
END

GO

CREATE PROCEDURE [dbo].[API_Payables_LoadApprPolicyUsers]
(@CorpID VARCHAR(50))
AS
BEGIN 
DECLARE @cID BINARY(18)=CONVERT(BINARY(18), @CorpID, 2) 	
Select CONVERT(VARCHAR(50),APD.AssignedTo,2) as [ID],
U.FirstName+' '+ U.LastName +'-'+U.UserName as[Name],
APD.ApprovalOrder,APD.ApprovalType
from ApprovalPloicy AP
INNER JOIN ApprovalPloicyDetails APD ON AP.ID=APD.ApprovalPolicyID
INNER JOIN [User] U ON U.ID=APD.AssignedTo
Where AP.CorporationID=@cID and AP.TransactionType=148
END

GO
ALTER PROCEDURE [dbo].[API_Payables_LoadLastAccount]
	@CorpID varbinary(18),
	@MiscSourceType int
AS
BEGIN
SELECT TOP 1 CONVERT(VARCHAR(50), t.AccountID,2) AS AccountID,(ac.AccountNumber +' '+ ac.AccountName) AS AccountName
FROM JournalEntry je  
JOIN [Transaction] t ON je.ID = t.JournalEntryID
JOIN MiscInfo mi ON je.PaymentMethodID = mi.ID
JOIN Account ac ON ac.ID=t.AccountID
WHERE je.CorporationID = @CorpID
AND t.ParentID IS NULL 
AND je.SourceType = 15 
AND mi.SourceType = @MiscSourceType
ORDER BY t.TransactionDate DESC
END

GO
ALTER PROCEDURE [dbo].[API_Payables_LoadCorporationDefaultPaymentAccount]
	@CorpID varbinary(18)
AS
BEGIN
	SELECT CONVERT(varchar(50), CA.PaymentAccountID,2) AS AccountID, (ac.AccountNumber +' '+ ac.AccountName) AS AccountName
	FROM CorporationAccountSetting CA 
	join Account ac on CA.PaymentAccountID=ac.ID
	WHERE CorporationID=@CorpID and CA.Status=1
END
GO

CREATE PROCEDURE [dbo].[API_Payables_Forte_BillPaymentload]
	(
	@JID Binary(18),
	@Type SmallINT=0,
	@UID Binary(18)
	)
AS
BEGIN
DECLARE @isLegalNametoShow INT
DECLARE @checkNo VARCHAR(500)
DECLARE @status SMALLINT

DECLARE @isPCEnabled BIT
SET @isPCEnabled= (Select c.IsDualBrand from JournalEntry JE 
					INNER JOIN Corporation C on JE.CorporationID= C.ID
					Where JE.ID=@JID)

SELECT TOP 1 @checkNo=tr.ReferenceNumber, @status=[status] FROM [Transaction] tr WHERE JournalEntryID=@JID AND tr.ParentID IS NULL AND tr.[status]<>3

SELECT Top 1 @isLegalNametoShow=Count(U.ID) FROM UserPreferenceSettings U Where U.UserID=@UID AND ISNULL(IsCorpLegalNameShow,0)=1
	
	SELECT X.TransNumber,X.CorpID,X.CorpName,X.LockDate,X.TransactionID,X.JournalEntryID,ISNULL(X.EntryNumber,'') EntryNumber,ISNULL(X.BillEntryNumber,'') BillEntryNumber,X.AccountID,Convert(Varchar(300), X.AccountName) AccountName,ISNULL(X.ActualAmount,0) ActualAmount,X.AdjustTransactionID,X.AdjustmentAmount,X.AdjustmentAmountHide,X.AmountDue,
	X.AppliedAccountID,X.AppliedTransactionID,X.BillpaymentOutStanding,ISNULL(X.CheckNo,'')CheckNo,X.ClearedDate,ISNULL(X.DepositID,'') DepositID,X.DiscountAmount,X.DueDate,X.EntryDate,X.BillDate,CONVERT(VARCHAR(10),X.JBillDate,101) AS JBillDate,
	X.HoldPayment,X.InvoiceAmountDue,X.IsAttachment,Convert(Varchar(10), ISNULL(X.IsLocked,0)) IsLocked,Convert(Varchar(3),ISNULL(X.IsPrintCheck,0)) IsPrintCheck,CONVERT(Varchar(5), X.IsReconciled) IsReconciled,CAST(IsVoided AS BIT) IsVoided,ISNULL(X.Memo,'')Memo,Convert(SmallINT, X.NewUpdateType) NewUpdateType,X.PCNAme,X.PCID,X.PaidAmount AS PaidAmount,
	X.PayeeID,X.PaymentMethodID,ISNULL(X.PayMethodSourceType,0) PayMethodSourceType,X.PaymentMethodName,X.PaytokenID,X.PrintType,X.ReferenceMode,X.SourceID,Convert(SmallInt, X.SourceType) SourceType,X.[Status],X.VendorName,X.AccountNum,Convert(SmallINT,ISNULL(X.isSelected,0)) isSelected,
	X.DiscountAccountID,X.DiscountAccountName,X.DiscountTransactionID,X.BillEntryID,ISNULL((Select SUM(ISNULL(BEPS.PaidAmount,0)) FROM BillEntryPayments BEPS Where BillPaymentID=X.JournalEntryID),0)+ISNULL(X.ExcessPayment,0) AS HeadAmount,X.VoidDate,X.PrintChkCon,PrintCount, PCAmount,BillPaymentStatus,X.VoidRemarks
	 FROM (
	 
		SELECT  BI.EntryDate AS BillDate,J.VoidDate,J.TransNumber,C.ID AS CorpID,C.LockDate,(Case when ISNULL(@isLegalNametoShow,0)>0 THEN C.LegalName ELSE  C.CorporationName END) CorpName,BPI.JournalEntryID AS JournalEntryID,
		ISNULL(J.EntryNumber,'') EntryNumber,ISNULL((SELECT Top 1 EntryNumber FROM JournalEntry JS Where JS.ID=BI.JournalEntryID),'N/A') BillEntryNumber,1 isSelected,BPI.SourceID AS PayeeID,B.Name VendorName,
		ISNULL(VC.AccountNumber,'') AccountNum,J.EntryDate,BI.Amount AS ActualAmount,ISNULL(CT.DueDate,BI.EntryDate) AS DueDate,@checkNo AS CheckNo,
		ISNULL(S.Name,'') PCNAme,S.ID as [PCID] ,BI.RefType AS SourceType,ISNULL(@status, BI.[Status]) AS [Status],BP.Discount AS DiscountAmount,Bp.AdjustmentAmount,Bp.AdjustmentAmount AS AdjustmentAmountHide,
		T.AccountID AppliedAccountID,BP.PaidAmount,Bp.TransactioonID AS AppliedTransactionID,
		
		(Case WHEN @isPCEnabled = 1 THEN  ISNULL(BP.Outstanding,0) 
		ELSE ISNULL(BI.Outstanding,0) END)
		+ISNULL(BP.PaidAmount,0)+ISNULL(BP.Discount,0)+ISNULL(BP.AdjustmentAmount,0) AmountDue,
		
		BP.OutStanding AS BillpaymentOutStanding,
		BI.Outstanding InvoiceAmountDue,BI.JournalEntryID AS SourceID,1 As NewUpdateType,Convert(SmallINT,(CASE WHEN JB.IsAttachment=0 THEN ISNULL((SELECT CASE WHEN  COUNT(ID)>0 THEN 1 ELSE 0 END  FROM ImportDocument Where journalEntryID=BI.JournalEntryID),0)  ELSE JB.IsAttachment END)) AS  IsAttachment,J.HoldPayment
		,(CASE WHEN BPI.ReconciliationID IS NULL THEN 0 ELSE 1 END) IsReconciled,J.ReferenceMode,J.DepositID,J.PaytokenID,J.IsPrintRequired IsPrintCheck,J.PaymentMethodID,
		MI.SourceType PayMethodSourceType,MI.Name PaymentMethodName,MI.PrintType,J.ClearedDate,(CASE WHEN ISNULL(BP.[Status],ISNULL(@status,0))=2 THEN 1 ELSE 0 END) IsVoided,ISNULL(BPI.Memo,'') Memo
		,(CASE WHEN (SELECT TOP 1 Count(L.ID) FROM Locking L Where L.CorporationID=J.CorporationID AND L.FromDate>=j.EntryDate AND L.ToDate<=J.EntryDate)>0 THEN 1 ELSE 0 END) AS IsLocked,
		BPI.AdjustTransactionID,T.AccountID,(CASE WHEN A.AccountNumber IS NOT NULL THEN A.AccountNumber+' . '+A.AccountName ELSE A.AccountName END) AS AccountName,BPI.TransactionID,
		BPI.ReconciliationID,BPI.Reconstatus,CASE WHEN BPI.ReconciliationID IS NULL  THEN NULL ELSE (SELECT Top 1 R.StatementDate FROM Reconciliation R Where R.ID=BPI.ReconciliationID) END AS ReconciledDate,
		BPI.DebitCredit,BP.DiscountAccountID,BP.DiscountTransactionID,
		(CASE WHEN BP.DiscountAccountID IS NOT NULL THEN (SELECT (CASE WHEN DA.AccountNumber IS NOT NULL THEN DA.AccountNumber+' . '+DA.AccountName ELSE DA.AccountName END) FROM Account DA Where DA.ID=BP.DiscountAccountID)
		ELSE '' END) AS  DiscountAccountName,BI.JournalEntryID AS BillEntryID
		,ISNULL((SELECT SUM(-1*ISNULL(BIS.Amount,0)) FROM BillEntryInformation BIS Where BIS.JournalEntryID=BPI.JournalEntryID),0) AS ExcessPayment
		,ISNULL((SELECT J.BillDate FROM JournalEntry J Where BI.JournalEntryID=J.ID),BI.EntryDate) AS JBillDate 
		,ISNULL(BBI.PrintChkCon,0) AS PrintChkCon,
		(CASE WHEN ISNULL((SELECT COUNT(ID) FROM BatchPrint BPR Where BPR.JournalID=J.ID AND BPR.PrintCount=1),0)>=1 THEN 1 ELSE 0 END) AS PrintCount,
		BEID.Amount as PCAmount,BPI.Status as BillPaymentStatus,JEXT.voidremarks as VoidRemarks
		 FROM BillPaymentsInformation BPI 
		
		INNER JOIN JournalEntry J ON J.ID=BPI.JournalEntryID
		INNER JOIN [Transaction] T ON BPI.TransactionID=T.ID
		LEFT OUTER JOIN BillEntryPayments BP ON BP.BillPaymentID=BPI.JournalEntryID
		
		INNER JOIN Business B ON BpI.SourceID=B.ID		
		INNER JOIN BusinessInfo BBI ON B.ID=BBI.ID

		INNER JOIN Account A ON T.AccountID=A.ID
		INNER JOIN Corporation C ON J.CorporationID=C.ID		
		LEFT OUTER JOIN MiscInfo MI ON J.PaymentMethodID=MI.ID
		
		LEFT OUTER JOIN BillEntryInformation BI ON BP.BillInformationID=BI.ID 
		LEFT JOIN BillEntryInformationDetails BEID on BEID.BillinfoID=BI.ID AND ((@isPCEnabled=1 AND BP.PCID=BEID.PCID) or (@isPCEnabled=0))
		 LEFT OUTER JOIN JournalEntry JB ON BI.JournalEntryID=JB.ID
		LEFT JOIN TransactionInvoice TI ON BI.TransactionID=TI.ID
		LEFT JOIN VendorContract VC ON TI.AccContractID=VC.ID
		Left OUTER JOIN CreditTerm CT ON CT.ID=BI.JournalEntryID
		LEFT OUTER JOIN Store S ON BEID.PCID=S.ID
		left join JournalEntryExt JEXT on JEXT.journalentryid=J.ID
		Where BPI.JournalEntryID=@JID
		)X
		ORDER BY DueDate

END


GO

ALTER procedure [dbo].[API_Payables_LoadBillPaymentfromBE]
(
@JEID VARCHAR(50)=NULL
)
AS 
BEGIN

DECLARE @JID BINARY(18)=CONVERT(BINARY(18), @JEID, 2) 
Select 
CONVERT(varchar(50), JE.ID, 2) as [JEID],
case when COUNT(DISTINCT(T2.StoreID))>1 THEN 'Multiple' ELSE  Max(S.Name) END as [PCName],
B.Name as [PayeeName],CONVERT(varchar(50), B.ID, 2) as [PayeeID],VC.AccountNumber,JE.EntryDate as [BooksDate],CT.DueDate,JE.BillDate,EntryNumber as [BillNumber],T.Amount,BEI.Outstanding  as [AmountDue]  
, CASE WHEN JE.SourceType = 38 THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END as [IsDebitMemo]
,CONVERT(varchar(50), VC.ID, 2) as ContractID
from 
JournalEntry JE
Inner JOIN [Transaction] T on JE.ID=T.JournalEntryID
LEFT JOIN [Transaction] T2 on JE.ID=T2.JournalEntryID
INNER JOIN [TransactionInvoice] TI on TI.ID=T.ID
Inner Join BillEntryInformation BEI on BEI.JournalEntryID=JE.ID
INNER JOIN Account A on A.ID=T.AccountID
INNER JOIN Business B on T.SourceID=B.ID
INNER JOIN VendorContract VC on VC.BusinessID=B.ID
LEFT JOIN Store S on S.ID=T2.StoreID
LEFT JOIN CreditTerm CT on CT.ID=JE.ID
Where T.ParentID IS NULL
AND JE.Status=1 AND JE.SourceType =21 AND BEI.Outstanding>0 
AND TI.AccContractID=VC.ID
AND JE.ID=@JID
group by 
B.Name,B.ID,VC.AccountNumber,JE.EntryDate,CT.DueDate,JE.BillDate,JE.EntryNumber,T.Amount,BEI.Outstanding,JE.ID,JE.SourceType,VC.ID
order by JE.EntryDate,JE.EntryNumber

END
GO

CREATE PROCEDURE [dbo].[API_Payables_SaveVoidTransactions] 
	@JEID AS BINARY(18)=NULL,
	@SrcType AS SMALLINT=NULL, 
	@VoidDate AS DATETIME2(0)=null,
	@CRUDType AS SMALLINT=0,
	@MJEID AS VARCHAR(MAX)=NULL,
	@ISJob AS SMALLINT=0
AS
BEGIN

--SELECT @MJEID='0x0C617681CDF7D8A94AEAC57C88084EDD0000,',@SrcType=127,@VoidDate='06/10/2022'	
DECLARE @CorpID AS BINARY(18)
DECLARE @ClientID AS BINARY(18)
DECLARE @VoidEnable AS BIT
SELECT @ClientID=ClientID FROM Corporation WHERE ID =(SELECT CorporationID FROM JournalEntry WHERE ID=@JEID)
SELECT @VoidEnable=ISNULL(IsEnable,0) FROM UserPreferenceSettings WHERE UserID=@ClientID AND [Type]=27 

if(@ISJob=1)
BEGIN
UPDATE JournalEntry SET [Status]=2, VoidDate=@VoidDate WHERE ID=@JEID
END
IF(@VoidEnable=0)
BEGIN
	RETURN
END
BEGIN
DECLARE @isBill SmallINT
				SELECT @isBill=CASE WHEN DebitCredit=1 THEN 1 ELSE 2 END FROM  BillEntryInformation Where journalEntryID=@JEID;
DECLARE @ASrcType AS SMALLINT=@SrcType
IF(@VoidDate IS NULL AND @MJEID IS NULL)
BEGIN
	DELETE TransactionInvoice WHERE ID IN(SELECT ID FROM [Transaction] WHERE JournalEntryID =(SELECT ID FROM JournalEntry WHERE ParentID=@JEID))
	DELETE [Transaction] WHERE JournalEntryID =(SELECT ID FROM JournalEntry WHERE ParentID=@JEID)
	DELETE JournalEntry WHERE ParentID=@JEID
	
	IF(@CRUDType <> 3)
	BEGIN
		UPDATE BillEntryInformation SET Outstanding=Amount,Status=1
		WHERE JournalEntryID=@JEID AND [Status]=2
	END
	UPDATE BillEntryPayments SET Status=1
	WHERE BillPaymentID=@JEID AND [Status]=2
	
END
ELSE
BEGIN
SET @SrcType=CASE WHEN @SrcType=21 THEN 162 WHEN @SrcType=22 THEN 165 WHEN @SrcType=20 THEN 171 WHEN @SrcType=127 THEN 172 WHEN @SrcType=129 THEN 173 WHEN @SrcType=128 THEN 174 WHEN @SrcType=130 THEN 175 WHEN @SrcType=133 THEN 171 WHEN @SrcType=15 THEN 163  WHEN @SrcType=11 THEN 168  WHEN @SrcType=32 THEN 169 WHEN @SrcType=37 THEN 167 WHEN @SrcType=38 THEN 164 WHEN @SrcType=33 THEN 166 WHEN @SrcType=118 THEN 170 ELSE @SrcType END

	CREATE TABLE #tmpJE(
			JID BINARY(18),
			CorpID BINARY(18),
			EntryDate DATETIME2(0),
			Status SMALLINT,
			SOURCEID BINARY(18),
			SourceType SMALLINT,
			ParentSType SMALLINT,
			CReatedDate DATETIME2(0),
			linkCol VARCHAR(30),
			EntryNo VARCHAR(300),
			RefNum VARCHAR(30),
			VendName VARCHAR(150),
			ParentID BINARY(18),
			CreatedBy BINARY(18),
			ModifiedBy BINARY(18),
		)
	CREATE TABLE #tmpTran(
		TID BINARY(18),
		AccountID BINARY(18),
		JournalEntryID BINARY(18),
		Amount decimal(14, 2),
		DebitCredit BIT,
		TDate DATETIME2(0),
		Status SMALLINT,
		ParentID BINARY(18),
		StoreID BINARY(18),
		SOURCEID BINARY(18),
		SourceType SMALLINT,
		TargetID BINARY(18),
		TargetType SMALLINT,
		JEID VARCHAR(30),
		MEMO VARCHAR(350),
		TPJID BINARY(18),
		TPID BINARY(18),
		TOrder SMALLINT
	)
	CREATE TABLE #tmpTranInv(
		TID BINARY(18),
		Hrs decimal(14, 2),
		Rate decimal(14, 5),
	) 
	 IF(@MJEID IS NOT NULL)
	 BEGIN
			
			 CREATE TABLE #MJEIDs (     
			  ID INT identity,
			  JournalID BINARY(18)
			  )
		    	
			INSERT INTO #MJEIDs(JournalID)
			SELECT CONVERT(BINARY(18),items,1) AS JournalID FROM dbo.split(@MJEID,',')	
			
			IF(@VoidDate IS NULL)
			BEGIN
				DELETE TransactionInvoice WHERE ID IN(SELECT ID FROM [Transaction] WHERE JournalEntryID IN(SELECT ID FROM JournalEntry WHERE ParentID IN(SELECT JournalID FROM #MJEIDs)))
				DELETE [Transaction] WHERE JournalEntryID IN (SELECT ID FROM JournalEntry WHERE ParentID IN(SELECT JournalID FROM #MJEIDs) AND SOURCETYPE IN(170,171,172,173,174))
				DELETE JournalEntry WHERE ParentID IN(SELECT JournalID FROM #MJEIDs)				
			END
			ELSE
			BEGIN
				
				DELETE TransactionInvoice WHERE ID IN(SELECT ID FROM [Transaction] WHERE JournalEntryID IN(SELECT ID FROM JournalEntry WHERE ParentID IN(SELECT JournalID FROM #MJEIDs) AND SOURCETYPE IN(170,171,172,173,174)))
				DELETE [Transaction] WHERE JournalEntryID IN (SELECT ID FROM JournalEntry WHERE ParentID IN(SELECT JournalID FROM #MJEIDs) AND SOURCETYPE IN(170,171,172,173,174))
				DELETE JournalEntry WHERE ParentID IN(SELECT JournalID FROM #MJEIDs)	
				--SELECT ID INTO #RevEntries FROM JournalEntry WHERE ParentID IN(SELECT JournalID FROM #MJEIDs)
				--DECLARE @RevEntCnt AS INT
				--SELECT @RevEntCnt=COUNT(ID) FROM #RevEntries
				--IF(@RevEntCnt>0)
				--BEGIN
				--	UPDATE J SET EntryDate=J.EntryDate
				--	FROM JournalEntry J
				--	INNER JOIN JournalEntry JP ON J.ParentID=JP.ID		
				--	WHERE J.ID IN(SELECT ID FROM #RevEntries)
					
	 		--		UPDATE T SET Amount=TE.Amount,DebitCredit=(CASE WHEN TE.DebitCredit=1 THEN 0 ELSE 1 END) 
				--	FROM [Transaction] T
				--	INNER JOIN JournalEntry J ON T.JournalEntryID=J.ID
				--	INNER JOIN [Transaction] TE ON J.ParentID=TE.JournalEntryID
				--	WHERE T.JournalEntryID IN(SELECT ID FROM #RevEntries)
				--END
				--BEGIN
			
					SELECT * FROM JournalEntry J
					INNER JOIN #MJEIDs M ON J.ID =M.JournalID
					
					SELECT * FROM JournalEntry J
					INNER JOIN #MJEIDs M ON J.ID =M.JournalID
					INNER JOIN [Transaction] T ON J.ID=T.JournalEntryID
				
					INSERT INTO #tmpJE(JID, CorpID,J.EntryDate,SourceType,ParentSType,ParentID,J.Status,CreatedBy,ModifiedBy,EntryNo)
					SELECT CONVERT(BINARY(18),REVERSE(REPLACE(newid(), '-', ''))), J.CorporationID,@VoidDate,@SrcType,J.SourceType,J.ID,1,CreatedBy,ModifiedBy,J.EntryNumber
					FROM JournalEntry J 
					INNER JOIN #MJEIDs M ON J.ID =M.JournalID
					
					SELECT T.* INTO #fTransList FROM [Transaction] T
					INNER JOIN #tmpJE J ON T.JournalEntryID=J.ParentID
					
					INSERT INTO #tmpTran(TID,AccountID,JournalEntryID,TPJID,Amount,TDate,SOURCEID,SourceType,TargetID,TargetType,TOrder,Status,DebitCredit,StoreID,MEMO)
					SELECT CONVERT(BINARY(18),REVERSE(REPLACE(newid(), '-', ''))),AccountID,J.JID,T.JournalEntryID AS TPJID,T.Amount,T.TransactionDate,T.SourceID,T.SourceType
					,T.TargetID,T.TargetType,T.[Order],1 AS [Status]
					,CASE WHEN T.DebitCredit=1 THEN 0 ELSE 1 END AS DebitCredit,T.StoreID,'Void of '+dbo.udf_GetSourceType(@ASrcType)+' '+ISNULL(ISNULL(T.ReferenceNumber,J.EntryNo),'')
					FROM #fTransList T 
					INNER JOIN #tmpJE J ON T.JournalEntryID=J.ParentID
					WHERE T.ParentID IS NULL

					INSERT INTO #tmpTran(TID,AccountID,JournalEntryID,ParentID,Amount,TDate,SOURCEID,SourceType,TargetID,TargetType,TOrder,Status,DebitCredit,StoreID)
					SELECT CONVERT(BINARY(18),REVERSE(REPLACE(newid(), '-', ''))),T.AccountID,J.JID,ST.TID,T.Amount,T.TransactionDate,T.SourceID,T.SourceType,
					T.TargetID,T.TargetType,T.[Order],1 AS [Status] 
					,CASE WHEN T.DebitCredit=1 THEN 0 ELSE 1 END AS DebitCredit,T.StoreID
					FROM #fTransList T
					INNER JOIN #tmpJE J ON T.JournalEntryID=J.ParentID
					INNER JOIN #tmpTran ST ON ST.TPJID=T.JournalEntryID
					WHERE T.ParentID IS NOT NULL

					INSERT INTO JournalEntry(ID,CorporationID,EntryDate,ParentID,SourceType,ParentSourceType,Status,CReatedDate,CreatedBy)
					SELECT JID,CorpID,EntryDate,ParentID,SourceType,ParentSType,[Status],CReatedDate,ModifiedBy FROM #tmpJE
					
					INSERT INTO [Transaction](ID,AccountID,JournalEntryID,Amount,DebitCredit,TransactionDate,[Status],ParentID,Memo,SourceID,SourceType,TargetID,TargetType,StoreID)
					SELECT TID,AccountID,JournalEntryID,Amount,DebitCredit,TDate,[Status],ParentID,MEMO,SOURCEID,SourceType,TargetID,TargetType,StoreID FROM #tmpTran WHERE ParentID IS NULL
						
					INSERT INTO [Transaction](ID,AccountID,JournalEntryID,Amount,DebitCredit,TransactionDate,[Status],ParentID,Memo,SourceID,SourceType,TargetID,TargetType,StoreID)
					SELECT TID,AccountID,JournalEntryID,Amount,DebitCredit,TDate,[Status],ParentID,MEMO,SourceID,SourceType,TargetID,TargetType,StoreID FROM #tmpTran WHERE ParentID IS NOT NULL
					
					DROP TABLE #fTransList
			--END		
		END
	
	 END
	 ELSE
	 BEGIN
	 DECLARE @CurBillPay Decimal(18,2)=0;
	 DECLARE @IsCurBillPayAdjusted Decimal(18,2)=0;
	 DECLARE @CurTotPay Decimal(18,2)=0;
	 DECLARE @CurAdjustPay Decimal(18,2)=0;
	 DECLARE @CurDiscount Decimal(18,2)=0;
	 DECLARE @CurAdjAmount Decimal(18,2)=0;
	 DECLARE @EntryDate DateTime2(0);
	 DECLARE @CurParentID Binary(18),@BPID BINARY(18),@SourceID BINARY(18),@StoreID BINARY(18);
	 DECLARE @CurTrID Binary(18);
	 DECLARE @CurBEPID BIGINT;
	 DECLARE @CurExcessAmount Decimal(18,2);
	 DECLARE @SourceType SMALLINT;
	 
	DECLARE @VoidJEID AS BINARY(18)
	SELECT @VoidJEID =ID FROM JournalEntry WHERE ParentID=@JEID
	---exec Usp_SaveVoidTransactions @JEID=0xB2646990BC502FB34C9831434FF49CDF0000,@SrcType=38,@VoidDate=N'12/25/2023'

	IF(@VoidJEID IS NOT NULL)
	BEGIN

		--INSERT INTO #tmpJE(JID, CorpID,J.EntryDate,SourceType,ParentSType,ParentID,J.Status,CreatedBy,ModifiedBy)
		--SELECT J.ID, J.CorporationID,@VoidDate,@SrcType,J.ParentSourceType,@JEID,1,CreatedBy,ModifiedBy 
		--FROM JournalEntry J WHERE ID=@VoidJEID

		--INSERT INTO #tmpTran(TID,AccountID,JournalEntryID,TPJID,Amount,TDate,SOURCEID,SourceType,TOrder,Status,DebitCredit)
		--SELECT T.ID,AccountID,J.JID,T.JournalEntryID AS TPJID,T.Amount,T.TransactionDate,T.SourceID,T.SourceType,T.[Order],1 AS [Status]
		--,CASE WHEN T.DebitCredit=1 THEN 0 ELSE 1 END AS DebitCredit
		--FROM [Transaction] T 
		--INNER JOIN #tmpJE J ON T.JournalEntryID=J.ParentID
		--WHERE JournalEntryID=@JEID AND T.ParentID IS NULL

		--INSERT INTO #tmpTran(TID,AccountID,JournalEntryID,ParentID,Amount,TDate,SOURCEID,SourceType,TOrder,Status,DebitCredit)
		--SELECT T.ID,T.AccountID,J.JID,ST.TID,T.Amount,T.TransactionDate,T.SourceID,T.SourceType,T.[Order],1 AS [Status] 
		--,CASE WHEN T.DebitCredit=1 THEN 0 ELSE 1 END AS DebitCredit
		--FROM [Transaction] T
		--INNER JOIN #tmpJE J ON T.JournalEntryID=J.ParentID
		--INNER JOIN #tmpTran ST ON ST.TPJID=T.JournalEntryID
		--WHERE T.JournalEntryID=@JEID AND T.ParentID IS NOT NULL
	 	UPDATE J SET EntryDate=JP.VoidDate,J.ModifiedBy=JP.ModifiedBy
		FROM JournalEntry J
		INNER JOIN JournalEntry JP ON J.ParentID=JP.ID		
		WHERE J.ID=@VoidJEID
		
		DELETE TransactionInvoice WHERE ID IN(SELECT ID FROM [Transaction] WHERE JournalEntryID=@VoidJEID)
		DELETE [Transaction] WHERE JournalEntryID=@VoidJEID
	 --	UPDATE T SET Amount=TE.Amount,DebitCredit=(CASE WHEN TE.DebitCredit=1 THEN 0 ELSE 1 END) ,T.AccountID=TE.AccountID,T.SourceID=TE.SourceID,T.SourceType=TE.SourceType
		--FROM [Transaction] T
		--INNER JOIN JournalEntry J ON T.JournalEntryID=J.ID
		--INNER JOIN [Transaction] TE ON J.ParentID=TE.JournalEntryID
		--WHERE T.JournalEntryID=@VoidJEID --AND TE.ParentID IS NULL
		
		SELECT * INTO #uTrans FROM [Transaction] WHERE JournalEntryID=@JEID
		INSERT INTO #tmpTran(TID,AccountID,JournalEntryID,TPJID,Amount,TDate,SOURCEID,SourceType,TargetID,TargetType,TOrder,Status,DebitCredit,StoreID,TPID,MEMO)
		SELECT CONVERT(BINARY(18),REVERSE(REPLACE(newid(), '-', ''))),AccountID,@VoidJEID,T.JournalEntryID AS TPJID,T.Amount,T.TransactionDate,T.SourceID,T.SourceType
		,T.TargetID,T.TargetType,T.[Order],1 AS [Status]
		,CASE WHEN T.DebitCredit=1 THEN 0 ELSE 1 END AS DebitCredit,T.StoreID,T.ID,'Void of '+dbo.udf_GetSourceType(@ASrcType)+' '+ISNULL(ISNULL(T.ReferenceNumber,J.EntryNumber),'')
		FROM #uTrans T 
		INNER JOIN JournalEntry J ON T.JournalEntryID=J.ID
		WHERE JournalEntryID=@JEID AND T.ParentID IS NULL

		INSERT INTO #tmpTran(TID,AccountID,JournalEntryID,ParentID,Amount,TDate,SOURCEID,SourceType,TargetID,TargetType,TOrder,Status,DebitCredit,StoreID,TPID)
		SELECT CONVERT(BINARY(18),REVERSE(REPLACE(newid(), '-', ''))),T.AccountID,@VoidJEID,ST.TID,T.Amount,T.TransactionDate,T.SourceID,T.SourceType
		,T.TargetID,T.TargetType,T.[Order],1 AS [Status] 
		,CASE WHEN T.DebitCredit=1 THEN 0 ELSE 1 END AS DebitCredit,T.StoreID,T.ID
		FROM #uTrans T
		INNER JOIN #tmpTran ST ON ST.TPJID=T.JournalEntryID
		WHERE T.JournalEntryID=@JEID AND T.ParentID IS NOT NULL
		
		INSERT INTO #tmpTranInv(TID,Hrs,Rate)
		SELECT T.TID,-1*TI.Hrs AS HRS,TI.Rate FROM TransactionInvoice TI
		INNER JOIN #tmpTran T ON TI.ID=T.TPID
				
		INSERT INTO [Transaction](ID,AccountID,JournalEntryID,Amount,DebitCredit,TransactionDate,[Status],ParentID,Memo,SourceID,SourceType,StoreID,[Order],TargetID,TargetType)
		SELECT TID,AccountID,JournalEntryID,Amount,DebitCredit,TDate,[Status],ParentID,MEMO,SOURCEID,SourceType,StoreID,TOrder,TargetID,TargetType FROM #tmpTran WHERE ParentID IS NULL
			
		INSERT INTO [Transaction](ID,AccountID,JournalEntryID,Amount,DebitCredit,TransactionDate,[Status],ParentID,Memo,SourceID,SourceType,StoreID,[Order],TargetID,TargetType)
		SELECT TID,AccountID,JournalEntryID,Amount,DebitCredit,TDate,[Status],ParentID,MEMO,SourceID,SourceType,StoreID,TOrder,TargetID,TargetType FROM #tmpTran WHERE ParentID IS NOT NULL
		
		INSERT INTO TransactionInvoice(ID,Hrs,Rate,Status)
		SELECT TID,HRS,Rate,1 FROM #tmpTranInv
		
		DROP TABLE #uTrans
	
	END
	ELSE
	BEGIN
	
	SELECT @ASrcType
	IF(@ASrcType IN(15))
	BEGIN
		
		--DECLARE @BIInfoID BIGINT=0;
		DECLARE @BPInfoID BIGINT=0;
		--DECLARE @TotalPaid Decimal(18,2)=0;
		--DECLARE @TotalBillEntry Decimal(18,2)=0;
		--DECLARE @BillID Binary(18)=NULL;
		

		SELECT BillInformationID INTO #BillInfoIds FROM BillEntryPayments WHERE BillPaymentID=@JEID
		
		SELECT JournalEntryID INTO #BillIds FROM BillEntryInformation B
		INNER JOIN #BillInfoIds BI ON B.ID=BI.BillInformationID
		
		SELECT Top 1 @BPInfoID=BP.ID FROM BillEntryPayments BP 
		INNER JOIN BillEntryInformation BI ON BP.BillInformationID=BI.ID 
		Where  BillPaymentID=@JEID 
		
		SELECT SUM(PaidAmount) AS PaidAmount,BI.ID INTO #BillPaidAmount FROM BillEntryPayments BP 
		INNER JOIN BillEntryInformation BI ON BP.BillInformationID=BI.ID
		WHERE BI.ID IN(SELECT BillInformationID FROM #BillInfoIds) AND BP.ID<@BPInfoID 
		GROUP BY BI.ID
	
		--UPDATE BI SET BI.Outstanding=BI.Outstanding+ISNULL(BP.PaidAmount,0)+ISNULL(BP.AdjustmentAmount,0)+ISNULL(BP.Discount,0) --,[Status]=1
		--FROM BillEntryPayments BP INNER JOIN BIllEntryInformation BI ON BP.BillInformationID=BI.ID WHERE BP.BillPaymentID= @JEID

		
		UPDATE BI2 SET Outstanding= T2.Amt
		from BIllEntryInformation BI2 INNER JOIN 
		(Select BI.Outstanding+ISNULL(SUM(BP.PaidAmount),0)+ISNULL(SUM(BP.AdjustmentAmount),0)+ISNULL(SUM(BP.Discount),0) as Amt, BI.ID 
		FROM BillEntryPayments BP INNER JOIN BIllEntryInformation BI ON BP.BillInformationID=BI.ID 
		WHERE BP.BillPaymentID= @JEID
		group by BI.Outstanding,BI.ID) as T2 on T2.ID= BI2.ID

		--newly added
		UPDATE BDI SET BDI.Outstanding=  BDI.Outstanding+ISNULL(BP.PaidAmount,0)+ISNULL(BP.AdjustmentAmount,0)+ISNULL(BP.Discount,0) 
		FROM BillEntryPayments BP INNER JOIN BIllEntryInformation BI ON BP.BillInformationID=BI.ID 
		INNER JOIN BillEntryInformationDetails BDI on BDI.BillInfoID=BI.ID 
		WHERE BP.BillPaymentID= @JEID and BP.PCID= BDI.PCID
		
		SELECT Amount,ID INTO #BillEntryAmts FROM BillEntryInformation Where JournalEntryID=@JEID
		
						
		UPDATE [Transaction] SET [Status]=2 Where JournalEntryID=@JEID
		
		UPDATE BI SET [Status]= 1 
		FROM BIllEntryInformation BI 
		--INNER JOIN [Transaction] T ON BI.JournalEntryID=T.JournalEntryID 
		INNER JOIN BillEntryPayments BP ON BI.ID=BP.BillInformationID
		WHERE BP.BillPaymentID= @JEID AND BI.[Status] <>2 AND BI.Outstanding<>0
		
		UPDATE T SET [Status]= 1 
		FROM BIllEntryInformation BI 
		INNER JOIN [Transaction] T ON BI.JournalEntryID=T.JournalEntryID 
		INNER JOIN BillEntryPayments BP ON BI.ID=BP.BillInformationID
		WHERE BP.BillPaymentID= @JEID AND BI.[Status] <>2 AND BI.Outstanding<>0
		
		UPDATE J SET [Status]= 1 
		FROM BIllEntryInformation BI 
		INNER JOIN JournalEntry J ON BI.JournalEntryID=J.ID
		INNER JOIN BillEntryPayments BP ON BI.ID=BP.BillInformationID
		WHERE BP.BillPaymentID= @JEID AND BI.[Status] <>2 AND BI.Outstanding<>0
		
		DELETE BillPayOrInvoiceAdjust WHERE TransactionID IN(SELECT ID FROM [Transaction] WHERE JournalEntryID=@JEID AND ParentID IS NOT NULL)
		DELETE BillEntryInformation WHERE TransactionID IN(SELECT ID FROM [Transaction] WHERE JournalEntryID=@JEID AND ParentID IS NOT NULL)
		DELETE [Transaction] WHERE JournalEntryID=@JEID AND ParentID IS NOT NULL


		
		
		UPDATE BillEntryPayments SET BillInformationID=NULL WHERE BillPaymentID=@JEID 
		
		INSERT INTO [Transaction](ID,TransactionDate,JournalEntryID,AccountID,Amount,DebitCredit,[Order],ParentID,SourceID,SourceType,[Status])
		SELECT CONVERT(BINARY(18),REVERSE(REPLACE(newid(), '-', ''))) ID,GETDATE(),JournalEntryID,(SELECT B.AccountID FROM Business B Where ID=T.SourceID),Amount,0,
		3,ID,T.SourceID,97,2 FROM [Transaction] T	WHERE JournalEntryID=@JEID AND ParentID IS NULL
		
		--UPDATE JournalEntry SET Status=1 WHERE ID IN (SELECT JournalEntryID FROM #BillIds) AND [Status] <>2
		
		--UPDATE [Transaction] SET Status=1 WHERE JournalEntryID IN (SELECT JournalEntryID FROM #BillIds) AND [Status] <>2
		DELETE BillEntryInformation WHERE JournalEntryID=@JEID
			
		INSERT INTO BillEntryInformation(JournalEntryID,Amount,Outstanding,SourceID,DebitCredit,[Status],TransactionID,EntryDate,CreditTermID,RefType,StoreID)
		SELECT @JEID,-1*T.Amount,T.Amount OutStanding,T.SourceID,0,2,T.ID TransactioonID,J.EntryDate,NULL,J.SourceType,T.StoreID 
		FROM
		  --BillEntryPayments B INNER JOIN 
		JournalEntry J-- ON B.BillPaymentID=J.ID
		INNER JOIN [Transaction] T ON J.ID=T.JournalEntryID
		WHERE J.ID=@JEID AND T.ParentID IS NULL

		 

		
		UPDATE BP SET Bp.AdjustID=NULL,Bp.PaidAmount=0,BP.AdjustmentAmount=BP.PaidAmount,BP.Discount=0,BP.DiscountAccountID=NULL,
		BP.OutStanding=BE.Amount-PB.PaidAmount,BP.[Status]=2 FROM BillEntryPayments BP 
		LEFT JOIN #BillEntryAmts BE ON BP.BillInformationID=BE.ID
		LEFT JOIN #BillPaidAmount PB ON BP.BillInformationID=PB.ID
		Where BillPaymentID=@JEID  
			
		INSERT INTO #tmpJE(JID, CorpID,J.EntryDate,SourceType,ParentSType,ParentID,J.Status,CreatedBy,ModifiedBy)
		SELECT CONVERT(BINARY(18),REVERSE(REPLACE(newid(), '-', ''))), J.CorporationID,@VoidDate,@SrcType,J.ParentSourceType,J.ID,1,CreatedBy,ModifiedBy 
		FROM JournalEntry J WHERE ID=@JEID

		SELECT * INTO #trans FROM [Transaction] WHERE JournalEntryID=@JEID

		INSERT INTO #tmpTran(TID,AccountID,JournalEntryID,TPJID,Amount,TDate,SOURCEID,SourceType,TOrder,Status,DebitCredit,StoreID,MEMO)
		SELECT CONVERT(BINARY(18),REVERSE(REPLACE(newid(), '-', ''))),AccountID,J.JID,T.JournalEntryID AS TPJID,T.Amount,T.TransactionDate,T.SourceID,T.SourceType,T.[Order],1 AS [Status]
		,CASE WHEN T.DebitCredit=1 THEN 0 ELSE 1 END AS DebitCredit,T.StoreID,'Void of '+dbo.udf_GetSourceType(J.ParentSType)+' '+ISNULL(ISNULL(T.ReferenceNumber,J.EntryNo),'')
		FROM #trans T 
		INNER JOIN #tmpJE J ON T.JournalEntryID=J.ParentID
		WHERE JournalEntryID=@JEID AND T.ParentID IS NULL

		INSERT INTO #tmpTran(TID,AccountID,JournalEntryID,ParentID,Amount,TDate,SOURCEID,SourceType,TOrder,Status,DebitCredit,StoreID)
		SELECT CONVERT(BINARY(18),REVERSE(REPLACE(newid(), '-', ''))),T.AccountID,J.JID,ST.TID,T.Amount,T.TransactionDate,T.SourceID,T.SourceType,T.[Order],1 AS [Status] 
		,CASE WHEN T.DebitCredit=1 THEN 0 ELSE 1 END AS DebitCredit,T.StoreID
		FROM #trans T
		INNER JOIN #tmpJE J ON T.JournalEntryID=J.ParentID
		INNER JOIN #tmpTran ST ON ST.TPJID=T.JournalEntryID
		WHERE T.JournalEntryID=@JEID AND T.ParentID IS NOT NULL

		INSERT INTO JournalEntry(ID,CorporationID,EntryDate,ParentID,SourceType,ParentSourceType,Status,CReatedDate,EntryNumber,CreatedBy)
		SELECT JID,CorpID,EntryDate,ParentID,SourceType,ParentSType,[Status],CReatedDate,EntryNo,ModifiedBy FROM #tmpJE
		
		INSERT INTO [Transaction](ID,AccountID,JournalEntryID,Amount,DebitCredit,TransactionDate,[Status],ParentID,Memo,SourceID,SourceType,StoreID,[Order])
		SELECT TID,AccountID,JournalEntryID,Amount,DebitCredit,TDate,[Status],ParentID,MEMO,SOURCEID,SourceType,StoreID,TOrder FROM #tmpTran WHERE ParentID IS NULL
			
		INSERT INTO [Transaction](ID,AccountID,JournalEntryID,Amount,DebitCredit,TransactionDate,[Status],ParentID,Memo,SourceID,SourceType,StoreID,[Order])
		SELECT TID,AccountID,JournalEntryID,Amount,DebitCredit,TDate,[Status],ParentID,MEMO,SourceID,SourceType,StoreID,TOrder FROM #tmpTran WHERE ParentID IS NOT NULL
		
		SELECT TransactionID,TargetID,SourceID  INTo #TargetTrs FROM BillPayOrInvoiceAdjust Where TargetID=@JEID
		delete from BillPayOrInvoiceAdjust Where TargetID=@JEID
		UPDATE BEP SET AdjustmentAmount=(SELECT SUM(BPI.Amount) FROM BillPayOrInvoiceAdjust BPI Where BPI.SourceID=BI.JournalEntryID AND BPI.TransactionID=BEP.TransactioonID) 
		FROM #TargetTrs BPIA 
	INNER JOIN [Transaction] TR ON BPIA.TransactionID =TR.ID
	INNER JOIN BillEntryInformation BI ON BPIA.SourceID=BI.JournalEntryID
	INNER JOIN BillEntryPayments BEP ON BEP.BillPaymentID=TR.JournalEntryID AND BEP.BillInformationID=BI.ID 
	WHERE BPIA.TargetID=@JEID

	UPDATE B SET OutStanding=A.Amount-(Paid+Adjusted) ,[Status]=CASE WHEN A.Amount-(Paid+Adjusted)<=0 THEN 5 ELSE 1 END FROM (
	SELECT BI.ID,BI.[Status],BI.Amount,ISNULL((SELECT SUM(TS.Amount) FROM [Transaction] TS Where TS.SourceID=BI.JournalEntryID AND TS.SourceType<>97 AND TS.[Status]<>3),0) AS Paid,
	ISNULL((SELECT SUM(BP.Amount) FROM BillPayOrInvoiceAdjust BP Where BI.JournalEntryID=BP.SourceID) ,0) AS Adjusted
	FROM BIllENtryInformation BI INNER JOIN JournalEntry J ON BI.JournalEntryID=J.ID  
	INNER JOIN #TargetTrs T ON BI.JournalEntryID=T.SourceID
	)A
	INNER JOIN BillEntryInformation B ON A.ID=B.ID

	UPDATE TR SET [Status]=1  FROM [Transaction] TR INNER JOIN #TargetTrs T ON TR.JournalEntryID=t.SourceID
		DROP TABLE #TargetTrs 
		DROP TABLE #trans 
		DROP TABLE #BillInfoIds
		DROP TABLE #BillEntryAmts
		DROP TABLE #BillIds
		DROP TABLE #BillPaidAmount
		
	END
	ELSE IF(@ASrcType IN(33))
	BEGIN
		
		DECLARE @TotalREc Decimal(18,2)=0;
		DECLARE @TotalInv Decimal(18,2)=0;
		DECLARE @InvID Binary(18)=NULL;	
					
		UPDATE [Transaction] SET [Status]=2 Where JournalEntryID=@JEID
		
		UPDATE IT SET [Status]= CASE WHEN (SELECT SUM(Amount) FROM [Transaction] where SourceID=T.SourceID AND JournalEntryID <>@JEID) >0 THEN IT.Status ELSE 1 END
		FROM [Transaction] T 
		INNER JOIN JournalEntry J ON T.SourceID=J.ID
		INNER JOIN [Transaction] IT ON J.ID =IT.JournalEntryID		
		WHERE T.JournalEntryID= @JEID AND T.ParentID IS NOT NULL
		
		
		UPDATE J SET [Status]= CASE WHEN (SELECT SUM(Amount) FROM [Transaction] where SourceID=T.SourceID AND JournalEntryID <>@JEID) >0 THEN J.Status ELSE 1 END 
		FROM [Transaction] T 
		INNER JOIN JournalEntry J ON T.SourceID=J.ID
		WHERE T.JournalEntryID= @JEID AND T.ParentID IS NOT NULL
		
		DELETE BillPayOrInvoiceAdjust WHERE TransactionID IN(SELECT ID FROM [Transaction] WHERE JournalEntryID=@JEID AND ParentID IS NOT NULL)
		DELETE [Transaction] WHERE JournalEntryID=@JEID AND ParentID IS NOT NULL
		
		DELETE BillPayOrInvoiceAdjust WHERE TargetID=@JEID OR SourceID=@JEID
				
		INSERT INTO [Transaction](ID,TransactionDate,JournalEntryID,AccountID,Amount,DebitCredit,[Order],ParentID,SourceID,SourceType,[Status],StoreID)
		SELECT CONVERT(BINARY(18),REVERSE(REPLACE(newid(), '-', ''))) ID,GETDATE(),JournalEntryID,(SELECT B.AccountID FROM Business B Where ID=T.SourceID),Amount,1,
		3,ID,T.SourceID,97,2,StoreID FROM [Transaction] T	WHERE JournalEntryID=@JEID AND ParentID IS NULL
		
			
		INSERT INTO #tmpJE(JID, CorpID,J.EntryDate,SourceType,ParentSType,ParentID,J.Status,CreatedBy,ModifiedBy)
		SELECT CONVERT(BINARY(18),REVERSE(REPLACE(newid(), '-', ''))), J.CorporationID,@VoidDate,@SrcType,J.ParentSourceType,J.ID,1,CreatedBy,ModifiedBy 
		FROM JournalEntry J WHERE ID=@JEID

		SELECT * INTO #InvTrans FROM [Transaction] WHERE JournalEntryID=@JEID
		INSERT INTO #tmpTran(TID,AccountID,JournalEntryID,TPJID,Amount,TDate,SOURCEID,SourceType,TOrder,Status,DebitCredit,StoreID,MEMO)
		SELECT CONVERT(BINARY(18),REVERSE(REPLACE(newid(), '-', ''))),AccountID,J.JID,T.JournalEntryID AS TPJID,T.Amount,GETDATE(),T.SourceID,T.SourceType,T.[Order],1 AS [Status]
		,CASE WHEN T.DebitCredit=1 THEN 0 ELSE 1 END AS DebitCredit,T.StoreID,'Void of '+dbo.udf_GetSourceType(J.ParentSType)+' '+ISNULL(ISNULL(T.ReferenceNumber,J.EntryNo),'')
		FROM #InvTrans T 
		INNER JOIN #tmpJE J ON T.JournalEntryID=J.ParentID
		WHERE JournalEntryID=@JEID AND T.ParentID IS NULL

		INSERT INTO #tmpTran(TID,AccountID,JournalEntryID,ParentID,Amount,TDate,SOURCEID,SourceType,TOrder,Status,DebitCredit,StoreID)
		SELECT CONVERT(BINARY(18),REVERSE(REPLACE(newid(), '-', ''))),T.AccountID,J.JID,ST.TID,T.Amount,GETDATE(),T.SourceID,T.SourceType,T.[Order],1 AS [Status] 
		,CASE WHEN T.DebitCredit=1 THEN 0 ELSE 1 END AS DebitCredit,T.StoreID
		FROM #InvTrans T
		INNER JOIN #tmpJE J ON T.JournalEntryID=J.ParentID
		INNER JOIN #tmpTran ST ON ST.TPJID=T.JournalEntryID
		WHERE T.JournalEntryID=@JEID AND T.ParentID IS NOT NULL

		INSERT INTO JournalEntry(ID,CorporationID,EntryDate,ParentID,SourceType,ParentSourceType,Status,CReatedDate,EntryNumber,CreatedBy)
		SELECT JID,CorpID,EntryDate,ParentID,SourceType,ParentSType,[Status],CReatedDate,EntryNo,ModifiedBy FROM #tmpJE
		
		INSERT INTO [Transaction](ID,AccountID,JournalEntryID,Amount,DebitCredit,TransactionDate,[Status],ParentID,Memo,SourceID,SourceType,StoreID,[Order])
		SELECT TID,AccountID,JournalEntryID,Amount,DebitCredit,TDate,[Status],ParentID,MEMO,SOURCEID,SourceType,StoreID,TOrder FROM #tmpTran WHERE ParentID IS NULL
			
		INSERT INTO [Transaction](ID,AccountID,JournalEntryID,Amount,DebitCredit,TransactionDate,[Status],ParentID,Memo,SourceID,SourceType,StoreID,[Order])
		SELECT TID,AccountID,JournalEntryID,Amount,DebitCredit,TDate,[Status],ParentID,MEMO,SourceID,SourceType,StoreID,TOrder FROM #tmpTran WHERE ParentID IS NOT NULL
		
		DROP TABLE #InvTrans 
		
	END
	ELSE IF(@ASrcType=20 OR @ASrcType=133)
	BEGIN
	
			INSERT INTO #tmpJE(JID, CorpID,EntryDate,SourceType,ParentSType,ParentID,J.Status,CreatedBy,ModifiedBy)
			SELECT CONVERT(BINARY(18),REVERSE(REPLACE(newid(), '-', ''))), J.CorporationID,@VoidDate,@SrcType,J.ParentSourceType,J.ID,1,CreatedBy,ModifiedBy 
			FROM JournalEntry J WHERE ID=@JEID

			SELECT * INTO #jtransList FROM [Transaction] WHERE JournalEntryID=@JEID
			INSERT INTO #tmpTran(TID,AccountID,JournalEntryID,TPJID,Amount,TDate,SOURCEID,SourceType,TargetID,TargetType,TOrder,Status,DebitCredit,StoreID,MEMO,TPID)
			SELECT CONVERT(BINARY(18),REVERSE(REPLACE(newid(), '-', ''))),AccountID,J.JID,T.JournalEntryID AS TPJID,T.Amount,T.TransactionDate,T.SourceID,T.SourceType
			,T.TargetID,T.TargetType,T.[Order],1 AS [Status]
			,CASE WHEN T.DebitCredit=1 THEN 0 ELSE 1 END AS DebitCredit,T.StoreID,'Void of '+dbo.udf_GetSourceType(J.ParentSType)+' '+ISNULL(ISNULL(J.EntryNo,T.ReferenceNumber),''),T.ID
			FROM #jtransList T 
			INNER JOIN #tmpJE J ON T.JournalEntryID=J.ParentID
			WHERE JournalEntryID=@JEID 

			INSERT INTO #tmpTranInv(TID,Hrs,Rate)
			SELECT T.TID,-1*TI.Hrs AS HRS,TI.Rate FROM TransactionInvoice TI
			INNER JOIN #tmpTran T ON TI.ID=T.TPID
								
			INSERT INTO JournalEntry(ID,CorporationID,EntryDate,EntryNumber,ParentID,SourceType,ParentSourceType,Status,CReatedDate,CreatedBy)
			SELECT JID,CorpID,EntryDate,EntryNo,ParentID,SourceType,ParentSType,[Status],CReatedDate,ModifiedBy FROM #tmpJE
			
			INSERT INTO [Transaction](ID,AccountID,JournalEntryID,Amount,DebitCredit,TransactionDate,[Status],ParentID,Memo,SourceID,SourceType,TargetID,TargetType,StoreID,[Order])
			SELECT TID,AccountID,JournalEntryID,Amount,DebitCredit,TDate,[Status],ParentID,MEMO,SOURCEID,SourceType,TargetID,TargetType,StoreID,TOrder FROM #tmpTran WHERE ParentID IS NULL
		
			INSERT INTO TransactionInvoice(ID,Hrs,Rate,Status)
			SELECT TID,HRS,Rate,1 FROM #tmpTranInv
			
			SELECT BI.ID AS BillInformationID,BP.ID,(SeleCT SUM(-1*ISNULL(BS.Amount,0)) FROM BillEntryInformation BS Where BS.JournalEntryID=BillPaymentID) AS ExcessAmount,
			ROW_NUMBER() OVER (ORDER BY BP.ID)  RowNo,BPI.JournalEntryID JID,BP.PaidAmount AS BillPayAmount,BP.AdjustmentAmount AS AdjustAmount,BP.Discount AS DiscountAmount
			,BP.DiscountAccountID,BI.SourceID,BI.EntryDate,BI.RefType,
			BP.AdjustID,BI.StoreID,BPI.AccountID,BPI.Memo INTO #JBillEntryPayments FROM
			 BillEntryInformation BI 
			INNER JOIN BillEntryPayments BP ON BP.BillInformationID=BI.ID
			INNER JOIN BillPaymentsInformation BPI ON BP.BillPaymentID=BPI.JournalEntryID
			Where BI.JournalEntryID=@JEID
					 
			 SELECT @BPID=BP.JID,@CurBEPID=ID,@CurBillPay=ISNULL(BP.BillPayAmount,0),
			@CurAdjustPay=ISNULL(AdjustAmount,0),@CurAdjAmount=ISNULL(AdjustAmount,0),@CurDiscount=ISNULL(DiscountAmount,0), @SourceID=BP.SourceID
			,@StoreID=BP.StoreID,@EntryDate=BP.EntryDate,@SourceType=Bp.RefType,@CurExcessAmount=ISNULL(BP.ExcessAmount,0) FROM #JBillEntryPayments BP

			 SELECT @CurTrID=TR.ParentID FROM  [Transaction] TR INNER JOIN  Account A ON TR.AccountID=A.ID INNER JOIN AccountType AT ON A.AccountTypeID=AT.ID 
			Where  TR.JournalEntryID=@BPID AND AT.ID= 0x0FA700000000000000000000000000000012  AND TR.[Status]<>3

			SELECT @CurParentID=TR.ParentID FROM  [Transaction] TR  Where ParentID IS NOT NULL AND JournalEntryID=@BPID
			SET @CurBillPay=@CurBillPay+@CurExcessAmount
			SET @CurTotPay=@CurBillPay+@CurAdjustPay+@CurDiscount;
					
			INSERT INTO BillEntryInformation(JournalEntryID,Amount,Outstanding,SourceID,DebitCredit,[Status],TransactionID,EntryDate,CreditTermID,RefType,StoreID)
			SELECT @BPID,-1*@CurBillPay,@CurBillPay OutStanding,@SourceID,0,1,@CurTrID,@EntryDate,NULL,@SourceType,@StoreID
			
			UPDATE BillEntryInformation SET Outstanding=0,Status=2
			WHERE JournalEntryID=@JEID
			
			UPDATE BillEntryPayments SET Status=1,PaidAmount=0,AdjustmentAmount=0,OutStanding=0
			WHERE BillInformationID IN(SELECT ID FROM BillEntryInformation WHERE JournalEntryID= @JEID)
			
			
			UPDATE BillPaymentsInformation SET Amount=0 
			WHERE JournalEntryID IN(SELECT BillPaymentID FROM BillEntryPayments 
			WHERE BillInformationID IN(SELECT ID FROM BillEntryInformation WHERE JournalEntryID= @JEID))
			
			UPDATE BillPaymentsInformation SET Amount=0
			WHERE JournalEntryID IN(SELECT BillPaymentID FROM BillEntryPayments 
			WHERE BillInformationID IN(SELECT ID FROM BillEntryInformation WHERE JournalEntryID= @JEID))
			
			UPDATE BillEntryPayments SET AdjustmentAmount=0
			WHERE BillInformationID IN(SELECT ID FROM BillEntryInformation WHERE JournalEntryID IN 
			(select SourceID from BillPayOrInvoiceAdjust where TargetID=@JEID
			))
			
			DELETE BillPayOrInvoiceAdjust WHERE TargetID =@JEID OR SourceID=@JEID
			
			DROP TABLE #JBillEntryPayments
			DROP TABLE #jtransList
	END

	ELSE
	BEGIN
	IF(@isBill=1)
	BEGIN
	DECLARE @Discount Decimal(18,2)=0;
		SELECT @Discount=Amount FROM [Transaction] wHERE SourceID=@JEID AND [Status]<>3 AND SourceType=85

		UPDATE TR SET Amount= TR.AMount-ISNULL((SELECT SUM(Amount) FROM [Transaction] TS wHERE TR.JournalEntryID=TS.JournalEntryID AND TS.[Status]<>3 AND TS.SourceType=85),0) FROM [Transaction] TR Where SourceID=@JEID AND [Status]<>3 AND SourceType<>85 AND SourceType<>97

	END
			
			INSERT INTO #tmpJE(JID, CorpID,EntryDate,EntryNo,SourceType,ParentSType,ParentID,J.Status,CreatedBy,ModifiedBy)
			SELECT CONVERT(BINARY(18),REVERSE(REPLACE(newid(), '-', ''))), J.CorporationID,@VoidDate,EntryNumber,@SrcType,J.ParentSourceType,J.ID,1,CreatedBy,ModifiedBy 
			FROM JournalEntry J WHERE ID=@JEID

			SELECT * INTO #transList FROM [Transaction] WHERE JournalEntryID=@JEID
			INSERT INTO #tmpTran(TID,AccountID,JournalEntryID,TPJID,Amount,TDate,SOURCEID,SourceType,TargetID,TargetType,TOrder,Status,DebitCredit,StoreID,MEMO,TPID)
			SELECT CONVERT(BINARY(18),REVERSE(REPLACE(newid(), '-', ''))),AccountID,J.JID,T.JournalEntryID AS TPJID,T.Amount,T.TransactionDate,T.SourceID,T.SourceType,
			T.TargetID,T.TargetType,T.[Order],1 AS [Status]
			,CASE WHEN T.DebitCredit=1 THEN 0 ELSE 1 END AS DebitCredit,T.StoreID,'Void of '+dbo.udf_GetSourceType(J.ParentSType)+' '+ISNULL(T.ReferenceNumber,ISNULL(J.EntryNo,'')),T.ID
			FROM #transList T 
			INNER JOIN #tmpJE J ON T.JournalEntryID=J.ParentID
			WHERE JournalEntryID=@JEID AND T.ParentID IS NULL

			INSERT INTO #tmpTran(TID,AccountID,JournalEntryID,ParentID,Amount,TDate,SOURCEID,SourceType,TargetID,TargetType,TOrder,Status,DebitCredit,StoreID,TPID)
			SELECT CONVERT(BINARY(18),REVERSE(REPLACE(newid(), '-', ''))),T.AccountID,J.JID,ST.TID,T.Amount,T.TransactionDate,T.SourceID,T.SourceType
			,T.TargetID,T.TargetType,T.[Order],1 AS [Status] 
			,CASE WHEN T.DebitCredit=1 THEN 0 ELSE 1 END AS DebitCredit,T.StoreID,T.ID
			FROM #transList T
			INNER JOIN #tmpJE J ON T.JournalEntryID=J.ParentID
			INNER JOIN #tmpTran ST ON ST.TPJID=T.JournalEntryID
			WHERE T.JournalEntryID=@JEID AND T.ParentID IS NOT NULL
			
			INSERT INTO #tmpTranInv(TID,Hrs,Rate)
			SELECT T.TID,-1*TI.Hrs AS HRS,TI.Rate FROM TransactionInvoice TI
			INNER JOIN #tmpTran T ON TI.ID=T.TPID
									
			INSERT INTO JournalEntry(ID,CorporationID,EntryDate,ParentID,SourceType,ParentSourceType,Status,CReatedDate,CreatedBy)
			SELECT JID,CorpID,EntryDate,ParentID,SourceType,ParentSType,[Status],CReatedDate,ModifiedBy FROM #tmpJE
			
			INSERT INTO [Transaction](ID,AccountID,JournalEntryID,Amount,DebitCredit,TransactionDate,[Status],ParentID,Memo,SourceID,SourceType,TargetID,TargetType,StoreID,[Order])
			SELECT TID,AccountID,JournalEntryID,Amount,DebitCredit,TDate,[Status],ParentID,MEMO,SOURCEID,SourceType,TargetID,TargetType,StoreID,TOrder FROM #tmpTran WHERE ParentID IS NULL
				
			INSERT INTO [Transaction](ID,AccountID,JournalEntryID,Amount,DebitCredit,TransactionDate,[Status],ParentID,Memo,SourceID,SourceType,TargetID,TargetType,StoreID,[Order])
			SELECT TID,AccountID,JournalEntryID,Amount,DebitCredit,TDate,[Status],ParentID,MEMO,SourceID,SourceType,TargetID,TargetType,StoreID,TOrder FROM #tmpTran WHERE ParentID IS NOT NULL
			
			INSERT INTO TransactionInvoice(ID,Hrs,Rate,Status)
			SELECT TID,HRS,Rate,1 FROM #tmpTranInv
			
			SELECT BI.ID AS BillInformationID,BP.ID,(SeleCT SUM(-1*ISNULL(BS.Amount,0)) FROM BillEntryInformation BS Where BS.JournalEntryID=BillPaymentID) AS ExcessAmount,
			ROW_NUMBER() OVER (ORDER BY BP.ID)  RowNo,BPI.JournalEntryID JID,BP.PaidAmount AS BillPayAmount,BP.AdjustmentAmount AS AdjustAmount,BP.Discount AS DiscountAmount
			,BP.DiscountAccountID,BI.SourceID,BI.EntryDate,BI.RefType,
			BP.AdjustID,BI.StoreID,BPI.AccountID,BPI.Memo INTO #BillEntryPayments FROM
			 BillEntryInformation BI 
			INNER JOIN BillEntryPayments BP ON BP.BillInformationID=BI.ID
			INNER JOIN BillPaymentsInformation BPI ON BP.BillPaymentID=BPI.JournalEntryID
			Where BI.JournalEntryID=@JEID			
	 
			 SELECT @BPID=BP.JID,@CurBEPID=ID,@CurBillPay=ISNULL(BP.BillPayAmount,0),
			@CurAdjustPay=ISNULL(AdjustAmount,0),@CurAdjAmount=ISNULL(AdjustAmount,0),@CurDiscount=ISNULL(DiscountAmount,0), @SourceID=BP.SourceID
			,@StoreID=BP.StoreID,@EntryDate=BP.EntryDate,@SourceType=Bp.RefType,@CurExcessAmount=ISNULL(BP.ExcessAmount,0) FROM #BillEntryPayments BP

			 SELECT @CurTrID=TR.ParentID FROM  [Transaction] TR INNER JOIN  Account A ON TR.AccountID=A.ID INNER JOIN AccountType AT ON A.AccountTypeID=AT.ID 
			Where  TR.JournalEntryID=@BPID AND AT.ID= 0x0FA700000000000000000000000000000012  AND TR.[Status]<>3

			SELECT @CurParentID=TR.ParentID FROM  [Transaction] TR  Where ParentID IS NOT NULL AND JournalEntryID=@BPID
			SET @CurBillPay=@CurBillPay+@CurExcessAmount
			SET @CurTotPay=@CurBillPay+@CurAdjustPay+@CurDiscount;
					DELETE FROM BillEntryInformation Where JournalEntryID= @BPID
			INSERT INTO BillEntryInformation(JournalEntryID,Amount,Outstanding,SourceID,DebitCredit,[Status],TransactionID,EntryDate,CreditTermID,RefType,StoreID)
			SELECT @BPID,-1*@CurBillPay,@CurBillPay OutStanding,@SourceID,0,1,@CurTrID,@EntryDate,NULL,@SourceType,@StoreID
			
			UPDATE BillEntryInformation SET Outstanding=0,Status=2
			WHERE JournalEntryID=@JEID
			
			UPDATE BillEntryPayments SET Status=1,PaidAmount=0,AdjustmentAmount=0,OutStanding=0
			WHERE BillInformationID IN(SELECT ID FROM BillEntryInformation WHERE JournalEntryID= @JEID)
			
			
			UPDATE BillPaymentsInformation SET Amount=0 
			WHERE JournalEntryID IN(SELECT BillPaymentID FROM BillEntryPayments 
			WHERE BillInformationID IN(SELECT ID FROM BillEntryInformation WHERE JournalEntryID= @JEID))
			
			UPDATE BillPaymentsInformation SET Amount=0
			WHERE JournalEntryID IN(SELECT BillPaymentID FROM BillEntryPayments 
			WHERE BillInformationID IN(SELECT ID FROM BillEntryInformation WHERE JournalEntryID= @JEID))
			
			--UPDATE BillEntryPayments SET AdjustmentAmount=0
			--WHERE BillInformationID IN(SELECT ID FROM BillEntryInformation WHERE JournalEntryID IN 
			--(select SourceID from BillPayOrInvoiceAdjust where TargetID=@JEID
			--))
			
			DECLARE @AdjAmount Decimal(18,2)=0
				IF(@isBill=1)
				BEGIN
				SELECT TargetID  INTO #TargetIDs From BillPayOrInvoiceAdjust Where SourceID=@JEID
				DELETE BillPayOrInvoiceAdjust WHERE TargetID =@JEID OR SourceID=@JEID
				UPDATE BEP SET AdjustmentAmount=(SELECT SUM(BPI.Amount) FROM BillPayOrInvoiceAdjust BPI Where BPI.SourceID=BI.JournalEntryID AND BPI.TransactionID=BEP.TransactioonID),Discount=0 FROM BillEntryInformation BI INNER JOIN BillEntryPayments BEP ON BI.ID=BEP.BillInformationID
				Where BI.JournalEntryID=@JEID
				UPDATE B SET OutStanding=A.Amount-( Adjusted) ,[Status]=CASE WHEN A.Amount-(Adjusted)=0 THEN 5 ELSE 1 END  FROM(
				select BI.ID,bi.aMOUNT,
				ISNULL((SELECT SUM(BP.Amount) FROM BillPayOrInvoiceAdjust BP Where BI.JournalEntryID=BP.SourceID) ,0) AS Adjusted
				from BillEntryInformation BI INNER JOIN #TargetIDs BPI ON BPI.TargetID=BI.JournalEntryID
				 )A
				INNER JOIN BillEntryInformation B ON A.ID=B.ID

				UPDATE B SET OutStanding=A.Amount-(Paid+Adjusted) ,[Status]=2 FROM (
				SELECT BI.ID,BI.[Status],BI.Amount,ISNULL((SELECT SUM(TS.Amount) FROM [Transaction] TS Where TS.SourceID=BI.JournalEntryID AND TS.SourceType<>97 AND TS.[Status]<>3),0) AS Paid,
				ISNULL((SELECT SUM(BP.Amount) FROM BillPayOrInvoiceAdjust BP Where BI.JournalEntryID=BP.SourceID) ,0) AS Adjusted
				FROM BIllENtryInformation BI INNER JOIN JournalEntry J ON BI.JournalEntryID=J.ID  AND DebitCredit=1
				AND BI.JournalEntryID=@JEID)A
				INNER JOIN BillEntryInformation B ON A.ID=B.ID

				END
			
			SELECT '@isBill',@isBill
			IF(@isBill=2)
			BEGIN
			
			--EXEC [Usp_SaveVoidTransactions] 0xA6388EFF6DFBB8A04904CBF25FBD003E0000,38,'01/01/2024' 
			SELECT SourceID  INTO #SourceIDs From BillPayOrInvoiceAdjust Where TargetID=@JEID
			DELETE BillPayOrInvoiceAdjust WHERE TargetID =@JEID OR SourceID=@JEID
			 UPDATE BEP SET BEP.ADjustmentAmount=(SELECT SUM(BPI.Amount) FROM BillPayOrInvoiceAdjust BPI Where BPI.SourceID=BI.JournalEntryID AND BPI.TransactionID=BEP.TransactioonID) 
			 from BillEntryInformation BI INNER JOIN #SourceIDs BPI ON BPI.SourceID=BI.JournalEntryID
			 INNER JOIN BillEntryPayments BEP ON BEP.BillInformationID=BI.ID
			  

			UPDATE B SET OutStanding=A.Amount-(Paid+Adjusted) ,[Status]=CASE WHEN A.Amount-(Paid+Adjusted)=0 THEN 5 ELSE 1 END  FROM(
				select BI.ID,bi.aMOUNT,ISNULL((SELECT SUM(TS.Amount) FROM [Transaction] TS Where TS.SourceID=BI.JournalEntryID AND TS.SourceType<>97 AND TS.[Status]<>3),0) AS Paid,
				ISNULL((SELECT SUM(BP.Amount) FROM BillPayOrInvoiceAdjust BP Where BI.JournalEntryID=BP.SourceID) ,0) AS Adjusted from BillEntryInformation BI INNER JOIN #SourceIDs BPI ON BPI.SourceID=BI.JournalEntryID
				 )A
				INNER JOIN BillEntryInformation B ON A.ID=B.ID
				 

				UPDATE BI SET OutStanding=BI.Amount-ISNULL((SELECT SUM(BP.Amount) FROM BillPayOrInvoiceAdjust BP Where BI.JournalEntryID=BP.SourceID) ,0)    FROM BillEntryInformation BI
				Where JournalEntryID=@JEID

				UPDATE BI SET [Status]= CASE WHEN OutStanding=0 THEN 5 ELSE 1 END  FROM BillEntryInformation BI
				Where JournalEntryID=@JEID AND [STATUS]<>2

			
				--UPDATE BP SET BP.AdjustmentAmount=(ABS(BP.AdjustmentAmount)-ABS(BI.Amount)),BP.Status=1
				-- from 
				-- BillPayOrInvoiceAdjust BI 
				--INNER JOIN [Transaction] T ON BI.TransactionID=T.ID
				--INNER JOIN BillEntryPayments BP ON T.JournalEntryID=BP.BillPaymentID
				--where BI.TargetID=@JEID
			END
			 	DELETE BillPayOrInvoiceAdjust WHERE TargetID =@JEID OR SourceID=@JEID
				--UPDATE B SET OutStanding=A.Amount-(Paid+Adjusted) ,[Status]=CASE WHEN A.Amount-(Paid+Adjusted)=0 THEN 5 ELSE 1 END  FROM(
				--select BI.ID,bi.aMOUNT,ISNULL((SELECT SUM(TS.Amount) FROM [Transaction] TS Where TS.SourceID=BI.JournalEntryID AND TS.SourceType<>97 AND TS.[Status]<>3),0) AS Paid,
				--ISNULL((SELECT SUM(BP.Amount) FROM BillPayOrInvoiceAdjust BP Where BI.JournalEntryID=BP.SourceID) ,0) AS Adjusted from BillEntryInformation BI INNER JOIN BillPayOrInvoiceAdjust BPI ON BPI.SourceID=BI.JournalEntryID
				--WHERE BPI.TargetID=@JEID)A
				--INNER JOIN BillEntryInformation B ON A.ID=B.ID
				--SELECT * FROM BillEntryInformation
				
			 
			--IF(@isBill=1)
			--BEGIN
			--	UPDATE BP SET BP.AdjustmentAmount=(ABS(BP.AdjustmentAmount)-ABS(BI.Amount)),BP.Status=1
			--	 from 
			--	 BillPayOrInvoiceAdjust BI 
			--	INNER JOIN [Transaction] T ON BI.TransactionID=T.ID
			--	INNER JOIN BillEntryPayments BP ON T.JournalEntryID=BP.BillPaymentID
			--	where BI.SouceID=@JEID
			--END
			

			DROP TABLE #TargetIDs
			DROP TABLE #SourceIDs
			DROP TABLE #BillEntryPayments
			DROP TABLE #transList
	END
	
	
	END
	END
	DROP TABLE #tmpJE
	DROP TABLE #tmpTran
	DROP TABLE #tmpTranInv
	DROP TABLE #SourceIDs
	END
END
END


GO

CREATE procedure [dbo].[API_Payables_LoadBillPaymentPCWise_Mul]
(
@JEID VARCHAR(Max)=NULL
)
AS 
BEGIN

--DECLARE @jID BINARY(18)=CONVERT(BINARY(18), @JEID, 2) 

Select CONVERT(varchar(50), JEID, 2) as [JEID],CONVERT(varchar(50), SID, 2) as [StoreID],
Name as [PCName],PayeeName, PayeeID,AccountNumber,EntryDate as [BooksDate],BillDate,DueDate,EntryNumber as[BillNumber],SUM(Amount) as [Amount],SUM(AmountDue) as [AmountDue] ,[IsDebitMemo],ContractID
from(
Select
JE.ID as [JEID],
S.ID as [SID],
CASE WHEN S.Name IS NULL THEN 'Unclassified' ELSE S.Name END as[Name],
 B.Name as [PayeeName],CONVERT(varchar(50), B.ID, 2) as [PayeeID],VC.AccountNumber,JE.EntryDate,JE.BillDate,CT.DueDate,JE.EntryNumber,CONVERT(varchar(50),VC.ID, 2) as ContractID,
--CASE WHEN OriginalAmount IS NULL THEN SUM(T.Amount) ELSE OriginalAmount END as [Amount],
--CASE WHEN OriginalAmount IS NULL THEN SUM(T.Amount)
--ELSE OriginalAmount+(SUM(PaidAmount)/(CASE WHEN COUNT(DISTINCT(t.StoreID)) =0 THEN 1 ELSE COUNT(DISTINCT(t.StoreID)) END )) END  as [AmountDue]
BD.Amount as [Amount],
BD.Outstanding as [AmountDue],
CASE WHEN JE.SourceType = 38 THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END as [IsDebitMemo]
from 
JournalEntry JE
Inner JOIN [Transaction] T on JE.ID=T.JournalEntryID
Inner JOIN [Transaction] T2 on JE.ID=T2.JournalEntryID
INNER JOIN [TransactionInvoice] TI on TI.ID=T2.ID
Inner Join BillEntryInformation BEI on BEI.JournalEntryID=JE.ID
INNER JOIN Business B on T2.SourceID=B.ID
INNER JOIN VendorContract VC on VC.BusinessID=B.ID
LEFT JOIN Store S on S.ID=T.StoreID
LEFT JOIN BillEntryInformationDetails  BD on S.ID=BD.PCID AND BEI.ID=BD.BillInfoID
--LEFT JOIN BillPaymentDetails BPD on BPD.JournalEntryID=JE.ID AND BPD.Amount=T.Amount
LEFT JOIN CreditTerm CT on CT.ID=JE.ID
where JE.ID IN (SELECT CONVERT(BINARY(18),'0x'+items,1) FROM dbo.Split(@JEID,','))
AND JE.Status=1 AND JE.SourceType IN (21,38) AND BEI.Outstanding>0  
AND T.ParentID IS NOT NULL AND T2.ParentID IS NULL AND TI.AccContractID=VC.ID 
GROUP BY --BPD.AccountID,OriginalAmount ,
BD.Amount,BD.Outstanding,
JE.EntryNumber,JE.BillDate,CT.DueDate,B.Name,B.ID,VC.AccountNumber,JE.EntryDate,S.Name,JE.ID,S.ID,JE.SourceType,VC.ID
) AS TEMP
Group by [Name],[PayeeName],PayeeID,AccountNumber,EntryDate,BillDate,DueDate,EntryNumber,JEID,SID,IsDebitMemo,ContractID
END

GO


CREATE PROCEDURE [dbo].[API_Core_GetVendorNamesByCorpIdUserId]
(
@CorpId VARCHAR(200)=NULL,
@UserId VARCHAR(200)=NULL
)
AS
BEGIN

 DECLARE @CID BINARY(18)=NULL
 IF(@CorpId<>'')
	BEGIN
	SET @CID=CONVERT(BINARY(18), @CorpId, 2)
	END

IF(@CID<>'')
 BEGIN

  SELECT   
   B.Name AS [NAME]
   FROM Business B    
   INNER JOIN VendorContract VC on B.ID = VC.BusinessID 
   WHERE  B.CorporationID=@CID and VC.isDefault=1  

 END
ELSE
	BEGIN
		 DECLARE @UID BINARY(18)=CONVERT(BINARY(18), @UserId, 2)  
	   SELECT   
       B.[Name] AS [NAME]
       FROM Business B    
       INNER JOIN VendorContract VC on B.ID = VC.BusinessID 
       WHERE  
	   B.CorporationID IN(SELECT  C.ID FROM Corporation C INNER JOIN UserCorporation UC ON C.ID=UC.CorporationID    
       WHERE UC.UserID=@UID  and C.[Status]=1)
	   AND VC.isDefault=1 
	   GROUP BY B.[Name]

	END


 
END


GO 
    
ALTER PROCEDURE [dbo].[API_Payables_GetPendingEPayments](            
@CorpIds Varchar(Max),            
@VendId Varchar(100),            
@PaymentMethodId Varchar(100),            
@StartDate Datetime,            
@EndDate Datetime,
@PageNo int=NULL
)            
AS            
BEGIN            
            
SET NOCOUNT ON;            
DECLARE @vID BINARY(18) =NULL            
DECLARE @payID BINARY(18) =CONVERT(BINARY(18), @PaymentMethodId, 2);            
IF(@VendId!='')            
BEGIN            
SET @vID=CONVERT(BINARY(18), @VendId, 2);            
END            
             
CREATE TABLE #PendingBills    
(    
 Id BIGINT,    
 JournalEntryId VARCHAR(50),    
 TransactionId VARCHAR(50),    
 CorporationID VARCHAR(50),    
 PaymentDate  VARCHAR(50),
 CorpLegalName VARCHAR(200),
 PaymentMethod  VARCHAR(200),
 CorpName  VARCHAR(200),
 VendorName VARCHAR(200),
 VendorId VARCHAR(50),
 BankAccountName VARCHAR(200),
 BankAccountId VARCHAR(50),
 Amount DECIMAL(18,2),
 DueDate  VARCHAR(50),
 BillNumber VARCHAR(50),
 RowNum BIGINT,
 BillsCount BIGINT,
 EntryDate Datetime,
 TotalRecordsCount INT
)   

INSERT INTO #PendingBills(Id,JournalEntryId,TransactionId,CorporationID,PaymentDate,CorpLegalName,PaymentMethod,CorpName,VendorName,VendorId,BankAccountName,BankAccountId,Amount,DueDate,BillNumber,RowNum,BillsCount,EntryDate)
  
  SELECT Records.Id, CONVERT(varchar(50),Records.JournalEntryId,2) as JournalEntryId,CONVERT(varchar(50),Records.TransactionId,2) as TransactionId,
  CONVERT(varchar(50),Records.CorporationID,2) as CorporationID,CONVERT(VARCHAR(10), Records.PaymentDate,101) as PaymentDate,Records.CorpLegalName, Records.PaymentMethod ,          
     Records.CorpName, Records.VendorName, CONVERT(varchar(50),Records.VendorId,2) as VendorId ,Records.BankAccountName,CONVERT(varchar(50),Records.BankAccountId,2) as BankAccountId,Records.Amount,CONVERT(VARCHAR(10), Records.DueDate,101)as DueDate,  
  Records.BillNumber, Records.RowNum,Records.BillsCount,Records.PaymentDate            
 FROM (  
 SELECT   
   BPI.Id as Id,  
   BPI.JournalEntryId As JournalEntryId,           
   BPI.TransactionId As TransactionId,          
   BPI.CorporationId,            
   BPI.EntryDate As PaymentDate,            
   C.CorporationName As CorpName,            
   C.LegalName As CorpLegalName,            
   B.[Name] AS VendorName,      
   BPI.SourceID As VendorId,      
   CONCAT(A.AccountNumber ,' ',A.AccountName) As BankAccountName,           
   A.Id As BankAccountId,          
   BPI.Amount,            
   CT.DueDate,            
   Bi.EntryNumber AS BillNumber,        
   M.[Name] As PaymentMethod,       
   ROW_NUMBER() OVER (PARTITION BY            
    BPI.Id ORDER BY BPI.EntryDate) AS RowNum,            
    SUM(1) OVER (PARTITION BY BPI.ID) AS BillsCount            
   FROM                    
    BillPaymentsInformation BPI            
    JOIN BillEntryPayments BP ON BP.BillPaymentID=BPI.JournalEntryID            
    Join BillEntryInformation BI ON BI.ID=BP.BillInformationId              
    JOIN CreditTerm CT ON CT.ID=BI.JournalEntryID            
    Join Business B ON BPI.SourceId=B.Id            
    Join Account A ON A.Id=BPI.AccountId            
    JOIN Corporation C  ON BPI.CorporationId=C.ID        
    JOIN MiscInfo M ON M.ID=BPI.PayMethodId      
    LEFT JOIN EPaymentsBatchDetails EPD ON BPI.JournalEntryID =EPD.JournalEntryID            
    WHERE BPI.PaymethodID=@payID        
    AND EPD.ID IS NULL      
   AND BPI.CorporationId IN (SELECT CONVERT(BINARY(18),'0x'+items,1) AS PCID  FROM dbo.Split(@CorpIds,','))          
   AND (BPI.SourceId=@vID OR @vID IS NULL)            
    AND  (BPI.CreatedDate >= CAST(@StartDate AS DATE) AND BPI.CreatedDate  < DATEADD(day, 1, CAST(@EndDate AS DATE)))                     
   AND BPI.[Status]=3    
   Group BY  
   BPI.Id ,  
   BPI.JournalEntryId ,           
   BPI.TransactionId,          
   BPI.CorporationId,            
   BPI.EntryDate ,            
   C.CorporationName ,            
   C.LegalName ,            
   B.[Name] ,      
   BPI.SourceID ,  
   A.AccountNumber ,    
   A.AccountName,  
   A.Id ,          
   BPI.Amount,            
   CT.DueDate,            
   Bi.EntryNumber,        
   M.[Name]              
 ) as Records            
 WHERE  Records.RowNum =1  

 UPDATE #PendingBills SET TotalRecordsCount=(Select ISNULL(Count(Id),0) FROM #PendingBills)

 SELECT Id ,    
 JournalEntryId ,    
 TransactionId ,    
 CorporationID ,    
 PaymentDate  ,
 CorpLegalName,
 PaymentMethod  ,
 CorpName,
 VendorName,
 VendorId ,
 BankAccountName ,
 BankAccountId,
 Amount ,
 DueDate  ,
 BillNumber ,
 RowNum ,
 BillsCount ,
 TotalRecordsCount FROM #PendingBills order by EntryDate DESC  offset @PageNo *  40 rows fetch next 40 rows only

 DROP TABLE #PendingBills 

  
END 

GO

CREATE PROCEDURE [dbo].[API_Payables_GetEPayments]                      
(                      
@CorpIds Varchar(Max),                      
@VendId Varchar(100),                      
@PaymentMethodId Varchar(100),                      
@ActionType smallInt,                      
@StartDate Datetime,                      
@EndDate Datetime,  
@PageNo INT  
)                      
AS                      
BEGIN                      
                      
 SET NOCOUNT ON;                      
                      
 DECLARE @vID BINARY(18) =NULL                                  
IF(@VendId!='')                      
BEGIN                      
SET @vID=CONVERT(BINARY(18), @VendId, 2);                      
END                      
  
CREATE TABLE #EPayBills      
(      
 BatchId BIGINT,      
 JournalEntryId VARCHAR(50),      
 CorporationID VARCHAR(50),      
 TransactionId VARCHAR(50),      
 BatchNumber BIGINT ,  
 ExportCount  INT,  
 AccountId   VARCHAR(50),  
 CorpName VARCHAR(200),  
 CorpLegalName VARCHAR(200),  
 CreatedUserName VARCHAR(200),  
 BankAccountName VARCHAR(200),  
 CreatedDate VARCHAR(50),  
 Amount  DECIMAL(18,2),  
 PaymentStatus INT,  
 VendorName VARCHAR(150),  
 VendorId VARCHAR(50),  
 BillNumber VARCHAR(50),  
 BillDate VARCHAR(50),  
 FormatName VARCHAR(150),  
 PaymentName VARCHAR(50),  
 PaymentDate VARCHAR(50),  
 IntiationDate VARCHAR(50),  
 CheckNo VARCHAR(150),  
 RowNum BIGINT,  
 BillsCount BIGINT,  
 TotalRecordsCount BIGINT  
)  
    
  
INSERT INTO #EPayBills(BatchId ,JournalEntryId , CorporationID , TransactionId ,BatchNumber  ,ExportCount  ,AccountId  ,CorpName,CorpLegalName ,CreatedUserName ,  
 BankAccountName ,CreatedDate ,Amount ,PaymentStatus ,VendorName ,VendorId ,BillNumber ,BillDate ,FormatName ,PaymentName ,PaymentDate ,IntiationDate ,CheckNo ,RowNum ,BillsCount)  
      
SELECT Records.BatchId,CONVERT(varchar(50),Records.JournalEntryId,2) as JournalEntryId, CONVERT(varchar(50),Records.CorporationID,2) as CorporationID ,  CONVERT(varchar(50),Records.TransactionId,2) as TransactionId ,                    
 Records.BatchNumber,Records.ExportCount,CONVERT(varchar(50),Records.AccountId,2) as AccountId ,Records.CorpName,Records.CorpLegalName,                      
   Records.CreatedUserName,Records.BankAccountName,CONVERT(VARCHAR(10), Records.CreatedDate,101) as  CreatedDate,Records.Amount,Records.PaymentStatus,Records.VendorName,                    
   CONVERT(varchar(50),Records.VendorId,2) as VendorId,Records.BillNumber,   CONVERT(VARCHAR(10), Records.BillDate,101) as BillDate , Records.FormatName,                 
    Records.PaymentName,CONVERT(VARCHAR(10), Records.PaymentDate,101) as PaymentDate,CONVERT(VARCHAR(10), Records.IntiationDate,101) as IntiationDate, Records.CheckNo,    Records.RowNum,Records.BillsCount                      
 FROM (    
 Select                      
 EP.Id as BatchId,                   
 EP.BatchNumber ,                    
 EP.IssuedCount as ExportCount,                   
 EPD.JournalEntryID As JournalEntryId,    
 BPI.CorporationId As TransactionId,    
 EPD.TransactionID As CorporationID,       
 A.ID As AccountId,                      
 C.CorporationName As CorpName,                      
 C.LegalName As CorpLegalName,                      
 CONCAT(U.FirstName ,' ',U.LastName) As CreatedUserName,                      
 CONCAT(A.AccountNumber ,' ',A.AccountName) As BankAccountName,    
 EP.CreatedDate,                      
 EPD.Amount,                      
 EPD.[Status] As PaymentStatus,        
 EPD.Number as CheckNO,          
 B.[Name] AS VendorName,                    
 B.ID as VendorId,                    
 BI.EntryNumber AS BillNumber,                      
 BI.BillDate,               
 M.[Name] As PaymentName,                   
 BPI.EntryDate As PaymentDate,               
 EP.IntiationDate as IntiationDate,            
 EF.[Name] as FormatName,            
 BP.BillInformationID,    
 ROW_NUMBER() OVER (PARTITION BY                      
  EP.Id ORDER BY  EP.Id) AS RowNum,                      
 SUM(1) OVER (PARTITION BY EP.Id) AS BillsCount     
 FROM EPaymentsBatchDetails EPD                       
  JOIN EPaymentsBatch EP ON EP.ID=EPD.EPaymentsBatchID                                       
  JOIN BillPaymentsInformation BPI ON EPD.JournalEntryID=BPI.JournalEntryId                      
  JOIN BillEntryPayments BP ON BP.BillPaymentID=EPD.JournalEntryID     
   join BillEntryInformation BI ON BI.ID=BP.BillInformationId    
   JOIN Corporation C ON C.ID=BPI.CorporationID                      
  Join Business B ON B.Id=BPI.SourceId                      
  Join Account A ON A.Id=BPI.AccountId     
  JOIN MiscInfo M ON M.ID=BPI.PaymethodID     
  JOIN [User] U ON U.ID=EP.CreatedBy     
  LEFT JOIN EFTFormat EF ON EF.ID=EP.FormatId            
  WHERE                       
 BPI.PaymethodID IN (SELECT CONVERT(BINARY(18),'0x'+items,1) AS PCID  FROM dbo.Split(@PaymentMethodId,',')) --- For Repay Payments Filter data from JournalEntry Table            
   AND  (EP.CreatedDate >= CAST(@StartDate AS DATE) AND EP.CreatedDate  < DATEADD(day, 1, CAST(@EndDate AS DATE)))           
          
  AND BPI.CorporationId IN (SELECT CONVERT(BINARY(18),'0x'+items,1) AS PCID  FROM dbo.Split(@CorpIds,','))                      
  AND (BPI.SourceId=@vID OR @vID IS NULL)                      
  AND (((@ActionType = 2) AND   EPD.[Status]  in (1))                      
  OR (@ActionType <> 2 and  EPD.[Status] = @ActionType))                                    
  AND BPI.[Status]=3                       
  GROUP BY     
 EP.Id,                   
 EP.BatchNumber ,                    
 EP.IssuedCount,                   
 EPD.JournalEntryID ,    
 BPI.CorporationId ,    
 EPD.TransactionID ,       
 A.ID ,                      
 C.CorporationName ,                      
 C.LegalName ,                      
U.FirstName ,U.LastName,                      
 A.AccountNumber ,A.AccountName,    
 EP.CreatedDate,                      
 EPD.Amount,                      
 EPD.[Status] ,        
 EPD.Number ,          
 B.[Name],                    
 B.ID ,                    
 BI.EntryNumber,                      
 BI.BillDate,                    
 M.[Name] ,                   
 BPI.EntryDate ,               
 EP.IntiationDate ,            
 EF.[Name] ,            
 BP.BillInformationID    
 )as Records                      
 WHERE  Records.RowNum =1      
   
  UPDATE #EPayBills SET TotalRecordsCount=(Select ISNULL(Count(BatchId),0) FROM #EPayBills)  
   
SELECT BatchId ,JournalEntryId , CorporationID , TransactionId ,BatchNumber  ,ExportCount  ,AccountId  ,CorpName,CorpLegalName ,CreatedUserName ,  
 BankAccountName ,CreatedDate ,Amount ,PaymentStatus ,VendorName ,VendorId ,BillNumber ,BillDate ,FormatName ,PaymentName ,PaymentDate ,IntiationDate ,  
 CheckNo ,RowNum ,BillsCount, TotalRecordsCount FROM #EPayBills order by BatchId DESC  offset @PageNo *  40 rows fetch next 40 rows only  
  
 DROP TABLE #EPayBills     
END 

GO     
CREATE procedure [dbo].[API_Payables_PendingBillPaymentPCList]      
(      
@BInfoIds VARCHAR(Max)=NULL      
)      
AS       
BEGIN      
      
  
SELECT BD.BillInfoID, SUM(ISNULL(BD.Amount,0)) as Amount,  
 SUM(ISNULL(BD.OutStanding,0)) as [AmountDue],  
CONVERT(varchar(50),BD.PCID, 2) as [StoreID],  
CASE WHEN S.Name IS NULL THEN 'Unclassified' ELSE S.Name END as[PCName]  
FROM BillEntryInformationDetails  BD left JOIN Store S ON S.iD=BD.PCID  
WHERE BD.BillInfoID in(SELECT CONVERT(Bigint,items,1) FROM dbo.Split(@BInfoIds,','))  
GROUP BY  BD.BillInfoID,BD.PCID,BD.OutStanding, S.[Name]  
   
END    

GO

         
CREATE procedure [dbo].[API_Payables_LoadBillPaymentsList]      
(      
    @CorpID VARCHAR(50) = NULL,      
    @VenID VARCHAR(50) = NULL,      
    @SortOrder SMALLINT = -1,      
    @FromDate DATETIME2 = NULL,      
    @ToDate DATETIME2 = NULL,      
    @DueDate DATE = NULL -- Changed to DATE for consistency with CT.DueDate,      
 ,@SkipJEIDs Varchar(max)=NULL      
)      
AS       
BEGIN      
      
  DECLARE @cID BINARY(18) = CONVERT(BINARY(18), @CorpID, 2);       
    DECLARE @vID BINARY(18) = CONVERT(BINARY(18), @VenID, 2);       
     
    
SELECT CONVERT(varchar(50), BI.JournalEntryId, 2) as [JEID],    
BI.ID as BillInfoId,    
B.[NAME] as [PayeeName] ,    
VC.AccountNumber,    
BI.EntryDate as [BooksDate],    
CT.DueDate,    
BI.BillDate,    
BI.EntryNumber as [BillNumber],    
BI.Amount,    
BI.Outstanding  as [AmountDue],     
 CASE WHEN BI.RefType= 21 THEN CAST(0 AS BIT) ELSE CAST(1 AS BIT) END as [IsDebitMemo],    
 CONVERT(varchar(50), VC.ID, 2) as ContractID ,   
 CONVERT(varchar(50), BI.CorporationID, 2) as [CorpID],     
CONVERT(varchar(50), BI.PayMethodID, 2) as [PaymentMethodID],     
CONVERT(varchar(50), BI.SourceId, 2) as [PayeeID],     
 C.IsDualBrand    
FROM BillEntryInformation BI    
JOIN Corporation C ON C.ID=BI.CorporationID     
JOIN Business B on BI.SourceID=B.ID     
LEFT JOIN CreditTerm CT on CT.ID=BI.JournalEntryId              
LEFT JOIN VendorContract VC ON VC.ID=BI.CONTRACTID    
WHERE BI.CorporationId=@cID    
AND  (@VenID IS NULL OR BI.SourceID = @vID)      
AND BI.[Status]=1    
AND BI.Outstanding>0    
and BI.RefType  IN(21,38)    
AND BI.ApprovalStatus=3    
AND ((@FromDate IS NULL AND @ToDate IS NULL)OR (BI.EntryDate >= @FromDate AND BI.EntryDate <= @ToDate))      
AND (@DueDate IS NULL OR CT.DueDate <= @DueDate)      
AND  (@SkipJEIDs IS NULL OR        
     BI.JournalEntryId NOT IN (SELECT CONVERT(BINARY(18),'0x'+items,1) AS PCID  FROM dbo.Split(@SkipJEIDs,',')))     
ORDER BY       
           case  WHEN @SortOrder = 0 THEN B.[Name] end,      
           CASE WHEN @SortOrder = 1 THEN BI.EntryDate END,      
           CASE WHEN @SortOrder = 2 THEN CT.DueDate END,      
           CASE WHEN @SortOrder = 3 THEN  BI.Outstanding END,      
           CASE WHEN @SortOrder = 4 THEN BI.BillDate END,      
           case when  @SortOrder = -1 then BI.EntryDate end --default sorting--     
    
      
END 

GO

      
CREATE PROCEDURE [dbo].[API_Payables_EPayPendingTobeApproveCount](            
@CorpIds Varchar(Max),            
@VendId Varchar(100),            
@PaymentIds Varchar(1000),            
@StartDate Datetime,            
@EndDate Datetime            
)            
AS            
BEGIN            
            
SET NOCOUNT ON;            
DECLARE @vID BINARY(18) =NULL            
IF(@VendId!='')            
BEGIN            
SET @vID=CONVERT(BINARY(18), @VendId, 2);            
END            
             
            
       SELECT count(BPI.PaymethodID) As PaymentsCount,            
       BPI.PaymethodID,'Pending' AS PaymentStatus,        
   M.[Name],        
   M.PaymentMethodType        
   FROM  BillPaymentsInformation BPI   
  JOIN MiscInfo M ON M.ID=BPI.PaymethodID  
    LEFT JOIN EPaymentsBatchDetails EPD ON BPI.JournalEntryID =EPD.JournalEntryID          
    WHERE    
  EPD.ID IS NULL  
 AND  
    BPI.PaymethodID IN(SELECT CONVERT(BINARY(18),'0x'+items,1) AS PCID  FROM dbo.Split(@PaymentIds,','))             
   AND  
   BPI.CorporationId IN (SELECT CONVERT(BINARY(18),'0x'+items,1) AS PCID  FROM dbo.Split(@CorpIds,','))            
   AND (BPI.SourceId=@vID OR @vID IS NULL)            
    AND  (BPI.CreatedDate >= CAST(@StartDate AS DATE) AND BPI.CreatedDate  < DATEADD(day, 1, CAST(@EndDate AS DATE)))                  
   AND BPI.[Status]=3             
        
   GROUP BY BPI.PaymethodID, M.[Name], M.PaymentMethodType          
            
   UNION            
            
   Select count(BPI.PaymethodID) As PaymentsCount,            
   BPI.PaymethodID,'TobeApprove' AS PaymentStatus,        
    M.[Name],        
   M.PaymentMethodType        
  FROM EPaymentsBatchDetails EPD             
  JOIN EPaymentsBatch EP ON EP.ID=EPD.EPaymentsBatchID                     
  JOIN BillPaymentsInformation BPI ON EPd.JournalEntryID=BPI.JournalEntryId            
  JOIN MiscInfo M ON M.ID=BPI.PaymethodID        
  WHERE             
  BPI.PaymethodID IN (SELECT CONVERT(BINARY(18),'0x'+items,1) AS PCID  FROM dbo.Split(@PaymentIds,','))            
  AND BPI.CorporationId IN (SELECT CONVERT(BINARY(18),'0x'+items,1) AS PCID  FROM dbo.Split(@CorpIds,','))            
   AND  (BPI.CreatedDate >= CAST(@StartDate AS DATE) AND BPI.CreatedDate  < DATEADD(day, 1, CAST(@EndDate AS DATE)))            
  AND (EPD.VendorID=@vID OR @vID IS NULL)            
   AND EPD.[Status]  =1                  
  AND BPI.[Status]=3                      
  GROUP BY BPI.PaymethodID, M.[Name], M.PaymentMethodType          
END 