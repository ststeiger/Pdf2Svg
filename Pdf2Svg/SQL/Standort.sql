SELECT 
	 T_ZO_AP_Standort_DWG.ZO_SODWG_UID
	,T_ZO_AP_Standort_DWG.ZO_SODWG_SO_UID
	,T_ZO_AP_Standort_DWG.ZO_SODWG_ApertureDWG
	,T_ZO_AP_Standort_DWG.ZO_SODWG_DatumVon
	,T_ZO_AP_Standort_DWG.ZO_SODWG_DatumBis
	,T_ZO_AP_Standort_DWG.ZO_SODWG_ApertureObjID
FROM T_ZO_AP_Standort_DWG
WHERE T_ZO_AP_Standort_DWG.ZO_SODWG_Status = 1 