
-- DECLARE @__in_ZO_GSDWG_ApertureDWG varchar(50) 
-- SET @__in_ZO_GSDWG_ApertureDWG = '1010_GB02_ZG00_0000'


SELECT TOP 1 
	 T_ZO_AP_Geschoss_DWG.ZO_GSDWG_ApertureDWG 
	,RIGHT('0000' + T_AP_Standort.SO_Nr, 4) AS SO_Nr 
	,'GB' + RIGHT('00' + T_AP_Gebaeude.GB_Nr, 2) AS GB_Nr 
	,T_AP_Ref_Geschosstyp.GST_Kurz_DE 
	,RIGHT('00' + T_AP_Geschoss.GS_Nr, 2) AS GS_Nr 
	
	,T_AP_Ref_Geschosstyp.GST_Kurz_DE + RIGHT('00' + T_AP_Geschoss.GS_Nr, 2) AS GS_DisplayNr 
	
	,GS_IsAussengeschoss
	
	,SO_UID 
	,GB_UID 
	,GS_UID 
	,T_ZO_AP_Geschoss_DWG.ZO_GSDWG_UID
	,T_ZO_AP_Geschoss_DWG.ZO_GSDWG_DatumVon
	,T_ZO_AP_Geschoss_DWG.ZO_GSDWG_DatumBis
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
AND 
( 
	T_ZO_AP_Geschoss_DWG.ZO_GSDWG_ApertureDWG = @__in_ZO_GSDWG_ApertureDWG 
	OR 
	@__in_ZO_GSDWG_ApertureDWG  = '00000000-0000-0000-0000-000000000000' 
) 

ORDER BY SO_Nr, GB_Nr, GST_Kurz_DE, GS_Nr, GS_IsAussengeschoss 
