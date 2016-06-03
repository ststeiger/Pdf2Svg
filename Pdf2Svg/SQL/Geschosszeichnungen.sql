
SELECT 
	 T_ZO_AP_Geschoss_DWG.ZO_GSDWG_ApertureDWG 
	 /*
	,
	REPLACE 
	( 
		REPLACE 
		( 
			REPLACE 
			( 
				 T_AP_Geschoss.GS_ApertureKey 
				,'000-' 
				,'' 
			) 
			,'-' 
			,'_' 
		)
		,'.00' 
		,'' 
	) AS GS_DisplayKey 
	*/
	
	,
	ISNULL
	(
		MAX
		(
			CASE WHEN T_AP_Geschoss.GS_IsAussengeschoss = 1 
				THEN CAST(NULL AS varchar(36)) 
				ELSE CAST(T_AP_Geschoss.GS_UID AS varchar(36))
			END 
		)
		, MAX( CAST(T_AP_Geschoss.GS_UID AS varchar(36)) )
	) 
	AS GS_UID 
	
	
	,RIGHT('0000' + T_AP_Standort.SO_Nr, 4) AS SO_Nr
	,'GB' + RIGHT('00' + T_AP_Gebaeude.GB_Nr, 2) AS GB_Nr 
	,T_AP_Ref_Geschosstyp.GST_Kurz_DE 
	,RIGHT('00' + T_AP_Geschoss.GS_Nr, 2) AS GS_Nr 
	
	,T_AP_Ref_Geschosstyp.GST_Kurz_DE + RIGHT('00' + T_AP_Geschoss.GS_Nr, 2) AS GS_DisplayNr 
	
	--,ROW_NUMBER() OVER(ORDER BY SO_Nr, GB_Nr, GS_IsAussengeschoss, GST_Sort, GST_GS_NrMultiplikator * GS_Nr, GST_ZG_Sort) AS RPT_GS_Sort 
	,DENSE_RANK() OVER(PARTITION BY SO_Nr, GB_Nr ORDER BY SO_Nr, GB_Nr, /*GS_IsAussengeschoss,*/ GST_Sort, GST_GS_NrMultiplikator * GS_Nr, GST_ZG_Sort) AS RPT_GS_Sort 
	
	
	--,T_AP_Geschoss.GS_IsAussengeschoss
	,T_AP_Ref_Geschosstyp.GST_Sort
	,T_AP_Ref_Geschosstyp.GST_GS_NrMultiplikator
	,T_AP_Ref_Geschosstyp.GST_ZG_Sort
	
	,T_AP_Standort.SO_Bezeichnung 
	,T_AP_Gebaeude.GB_Bezeichnung 
	,T_AP_Ref_Geschosstyp.GST_Lang_DE 
	
	--,T_ZO_AP_Geschoss_DWG.ZO_GSDWG_UID
	--,T_ZO_AP_Geschoss_DWG.ZO_GSDWG_DatumVon
	--,T_ZO_AP_Geschoss_DWG.ZO_GSDWG_DatumBis
	,COALESCE(T_AP_LinkDWG.LI_ApertureObjID, T_ZO_AP_Geschoss_DWG.ZO_GSDWG_ApertureObjID) AS ZO_GSDWG_ApertureObjID 
FROM T_ZO_AP_Geschoss_DWG 

LEFT JOIN T_AP_Geschoss 
	ON T_AP_Geschoss.GS_UID = ZO_GSDWG_GS_UID 
	
LEFT JOIN T_AP_Ref_Geschosstyp 
	ON T_AP_Ref_Geschosstyp.GST_UID = T_AP_Geschoss.GS_GST_UID 
	
LEFT JOIN T_AP_LinkDWG 
	ON T_AP_LinkDWG.LI_ApertureDWG = T_ZO_AP_Geschoss_DWG.ZO_GSDWG_ApertureDWG 
	
LEFT JOIN T_AP_Gebaeude 
	ON T_AP_Gebaeude.GB_UID = T_AP_Geschoss.GS_GB_UID 
	
LEFT JOIN T_AP_Standort 
	ON T_AP_Standort.SO_UID = T_AP_Gebaeude.GB_SO_UID 
	
WHERE T_ZO_AP_Geschoss_DWG.ZO_GSDWG_Status = 1 
-- AND {fn curdate()} BETWEEN CAST(T_ZO_AP_Geschoss_DWG.ZO_GSDWG_DatumVon AS date) AND CAST(T_ZO_AP_Geschoss_DWG.ZO_GSDWG_DatumBis AS date) 

AND T_ZO_AP_Geschoss_DWG.ZO_GSDWG_ApertureDWG IN 
(
	SELECT -- ZeichnungsTyp, 
		Zeichnungsname 
	FROM ___SwisscomExportListe 

)

GROUP BY 
 SO_Nr, SO_Bezeichnung
,GB_Nr, GB_Bezeichnung
,T_AP_Ref_Geschosstyp.GST_Kurz_DE, T_AP_Ref_Geschosstyp.GST_Lang_DE, T_AP_Geschoss.GS_Nr 
,T_AP_Ref_Geschosstyp.GST_Sort, T_AP_Ref_Geschosstyp.GST_GS_NrMultiplikator, T_AP_Ref_Geschosstyp.GST_ZG_Sort
,T_ZO_AP_Geschoss_DWG.ZO_GSDWG_ApertureDWG 
,T_ZO_AP_Geschoss_DWG.ZO_GSDWG_ApertureObjID,T_AP_LinkDWG.LI_ApertureObjID 

ORDER BY SO_Nr, GB_Nr, RPT_GS_Sort 
