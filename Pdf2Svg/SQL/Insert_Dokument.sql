
-- USE COR_Basic_Swisscom
-- USE COR_Basic_SwisscomTest
-- GO


--SELECT * 
-- DELETE FROM T_AP_Dokumente WHERE DK_Objekt_UID = '00000000-0000-0000-0000-000000000000';



-- DECLARE @__in_DK_UID uniqueidentifier
-- DECLARE @__in_DK_Objekt_UID uniqueidentifier
-- DECLARE @__in_DK_DKAT_UID uniqueidentifier
-- DECLARE @__in_DK_Bezeichnung varchar(50)
-- DECLARE @__in_DK_Datei varchar(255)
-- DECLARE @__in_DK_Dateiformat varchar(25)
-- DECLARE @__in_DK_Status int
-- DECLARE @__in_DK_IsUpload bit
-- DECLARE @__in_DK_IsDefault bit
-- DECLARE @__in_DK_File varbinary(max)
-- DECLARE @__in_DK_BE_ID int
-- DECLARE @__in_DK_Mut_Date datetime
-- DECLARE @__in_DK_DK_UID uniqueidentifier

	
	
-- SET @__in_DK_UID = NEWID() --  uniqueidentifier
-- SET @__in_DK_Objekt_UID = '00000000-0000-0000-0000-000000000000' --  uniqueidentifier
-- SET @__in_DK_DKAT_UID = NULL --  uniqueidentifier
-- SET @__in_DK_Bezeichnung = 'SVGs' --  varchar(50)
-- SET @__in_DK_Datei = 'Filename' --  varchar(255)
-- SET @__in_DK_Dateiformat = '.pdf' --  varchar(25)
-- SET @__in_DK_Status = 1 --  int
-- SET @__in_DK_IsUpload = 1 --  bit
-- SET @__in_DK_IsDefault = 0 --  bit
-- SET @__in_DK_File = NULL --  varbinary(max)
-- SET @__in_DK_BE_ID = 12768 --  int
-- SET @__in_DK_Mut_Date = GETDATE() --  datetime
-- SET @__in_DK_DK_UID = NULL --  uniqueidentifier



INSERT INTO T_AP_Dokumente
(
	 DK_UID
	,DK_Objekt_UID
	,DK_DKAT_UID
	,DK_Bezeichnung
	,DK_Datei
	,DK_Dateiformat
	,DK_Status
	,DK_IsUpload
	,DK_IsDefault
	,DK_File
	,DK_BE_ID
	,DK_Mut_Date
	,DK_DK_UID
)
VALUES
(
	 @__in_DK_UID -- uniqueidentifier
	,@__in_DK_Objekt_UID -- uniqueidentifier
	,@__in_DK_DKAT_UID -- uniqueidentifier
	,@__in_DK_Bezeichnung -- varchar(50)
	,@__in_DK_Datei -- varchar(255)
	,@__in_DK_Dateiformat -- varchar(25)
	,@__in_DK_Status -- int
	,@__in_DK_IsUpload -- bit
	,@__in_DK_IsDefault -- bit
	,@__in_DK_File -- varbinary(max)
	,@__in_DK_BE_ID -- int
	,@__in_DK_Mut_Date -- datetime
	,@__in_DK_DK_UID -- uniqueidentifier
)
;

