
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
            // URL = "ajax/PdfLegende.ashx?path=https://prod.swisscom-flaechen.ch/ApWebServices/ApDrawingPDFs.aspx?p=Swisscom_Portal$amp$d=1002_GB01_OG01_0000$amp$L=Raum$amp$S=Nutzungsart$amp$SEL=0000000002QR0006CA$amp$F=PNG$amp$uu=5&BE_ID=Administrator&legendvisible=true";

            string BaseURL = "http://vm-wincasa";
            BaseURL = "https://prod.swisscom-flaechen.ch/COR_Basic_Swisscom/";

            

            string PortalName = "Swisscom_Portal";

            if (!bLocal)
            {
                BaseURL = "https://www9.cor-asp.ch";
                PortalName = "SwisscomTest_Portal";
            }

            // return string.Format(URL, BaseURL, PortalName);
            
            // https://prod.swisscom-flaechen.ch/COR_Basic_Swisscom/ajax/PdfLegende.ashx?path=https://prod.swisscom-flaechen.ch/ApWebServices/ApDrawingPDFs.aspx?p=Swisscom_Portal$amp$d=1002_GB01_OG01_0000$amp$L=Raum$amp$S=Nutzungsart$amp$SEL=0000000002QR0006CA$amp$F=PNG$amp$uu=5&BE_ID=Administrator&legendvisible=true
            return "https://prod.swisscom-flaechen.ch/COR_Basic_Swisscom/ajax/PdfLegende.ashx?path=https://prod.swisscom-flaechen.ch/ApWebServices/ApDrawingPDFs.aspx?p=Swisscom_Portal$amp$d={0}$amp$L=Raum$amp$S=Nutzungsart$amp$SEL={1}$amp$F=PNG$amp$uu=5&BE_ID=Administrator&legendvisible=true";
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
        

        public static string ApertureWebServiceUrl111 = InternalGetURL(); // http://vm-wincasa

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
