SELECT 
	 T_ZO_AP_Gebaeude_DWG.ZO_GBDWG_UID
	,T_ZO_AP_Gebaeude_DWG.ZO_GBDWG_GB_UID
	,T_ZO_AP_Gebaeude_DWG.ZO_GBDWG_ApertureDWG
	,T_ZO_AP_Gebaeude_DWG.ZO_GBDWG_DatumVon
	,T_ZO_AP_Gebaeude_DWG.ZO_GBDWG_DatumBis
	,T_ZO_AP_Gebaeude_DWG.ZO_GBDWG_ApertureObjID 
FROM T_ZO_AP_Gebaeude_DWG 
WHERE T_ZO_AP_Gebaeude_DWG.ZO_GBDWG_Status = 1 