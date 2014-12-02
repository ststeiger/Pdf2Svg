
namespace Pdf2Svg
{


    class SvgSettings
    {


        public static string InternalGetURL()
        {
            return InternalGetURL(true);
        } // End Function InternalGetURL 


        public static string InternalGetURL(bool bLocal)
        {
            string URL = "{0}/ApWebServices/ApDrawingPDFs.aspx?p={1}&d={{0}}&L=Z_Export&S=Z_Export";

            string BaseURL = "http://vm-wincasa";
            string PortalName = "Swisscom_Portal";

            if (!bLocal)
            {
                BaseURL = "https://www9.cor-asp.ch";
                PortalName = "SwisscomTest_Portal";
            }

            return string.Format(URL, BaseURL, PortalName);
        } // End Function InternalGetURL 


        public static bool DOExp = true; // Export ausführen 
        public static bool ASCExp = true; // vor Export nachfragen 

        public static bool DateTimeExp = true; // Datum u. Zeitstempel im Dateinamen 
        public static bool DWGReplace = true; // vorhandene Datei ohne Datum u. Zeitstempel im Dateinamen überschreiben 
        public static bool EXPPathSchema = true; // wenn True dann Pfad mit Unterverzeichnissen für SO, GB u.s.w. sonst Datei in Haupverzeichnis abspeichern 
        public static string EXPPath = @"D:\stefan.steiger\Downloads\SwisscomDrawings"; // \\ads01\Wincasa_DWG_Temp$\SVGExportSwisscom ;Pfad für den Export der Dateien 
        public static string EXPType = "SVG"; // Export-Typ Deine Code läuft nur wenn Wert = "SVG"
        public static string EXPUrl = InternalGetURL(); // http://vm-wincasa

        public static bool SaveToDatabase = false;
        public static bool ConfigurationCorrect = true;
        
        
        public static string GetURL()
        {
            return EXPUrl;
        } // End Function InternalGetURL 


    }
}
