
namespace Pdf2Svg
{


    [System.Flags]
    public enum ExportFileType_t : int 
    { 
         svg = 1 << 0 
        ,pdf = 1 << 1
    }


    class ExportSettings
    {


        private static string InternalGetURL()
        {
            return InternalGetURL(true);
        } // End Function InternalGetURL 


        private static string InternalGetURL(bool bLocal)
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


        public static bool ExecuteExport = true; // Export ausführen 
        public static bool AskBeforeExport = false; // vor Export nachfragen 

        public static bool DateTimeInFileName = false; // Datum u. Zeitstempel im Dateinamen 
        public static bool OverwriteFileIfExists = false; // vorhandene Datei ohne Datum u. Zeitstempel im Dateinamen überschreiben 
        public static bool ExportWithSubdirectories = true; // wenn True dann Pfad mit Unterverzeichnissen für SO, GB u.s.w. sonst Datei in Haupverzeichnis abspeichern 

        public static string ExportDirectory = @"D:\stefan.steiger\Downloads\SwisscomDrawings"; // \\ads01\Wincasa_DWG_Temp$\SVGExportSwisscom ;Pfad für den Export der Dateien 
        // public static ExportFileType_t ExportFileType = ExportFileType_t.svg; // Deine Code läuft nur wenn Wert = "SVG"
        //public static ExportFileType_t ExportFileType = ExportFileType_t.pdf | ExportFileType_t.svg;
        public static ExportFileType_t ExportFileType = ExportFileType_t.pdf;
        

        public static string ApertureWebServiceUrl = InternalGetURL(); // http://vm-wincasa

        public static bool SaveToDatabase = false;
        public static bool ConfigurationCorrect = true;
        
        /*
        public static string GetURL()
        {
            return EXPUrl;
        } // End Function InternalGetURL 
        */

    }


}
