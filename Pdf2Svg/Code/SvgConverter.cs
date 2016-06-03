
// http://en.wikipedia.org/wiki/Wikipedia:Graphics_Lab/Resources/PDF_conversion_to_SVG#Conversion_with_Inkscape
// http://www.cityinthesky.co.uk/opensource/pdf2svg/
// pdf2svg input.pdf output_page%d.svg all
// pdf2svg input.pdf output.svg
// http://www.aspose.com/docs/display/pdfnet/Convert%20PDF%20to%20SVG%20format

// http://stackoverflow.com/questions/10288065/convert-pdf-to-clean-svg

// msiexec /a Aspose.Pdf_9.5.0.msi /qb TARGETDIR=D:\temp\test



// PDF 2 SVG:
// http://www.aspose.com/docs/display/pdfnet/Convert%20PDF%20to%20SVG%20format

// SVG 2 PDF:
// http://www.aspose.com/docs/display/pdfnet/Convert%20SVG%20file%20to%20PDF%20format

// http://www.aspose.com/docs/display/pdfnet/PDF+to+HTML+-+Avoid+Saving+Images+in+SVG+Format


using System.Windows.Forms;


namespace Pdf2Svg
{


    class SvgConverter
    {


        public static byte[] DownloadPdf(string strURL)
        {
            byte[] imageBytes = null;

            using (System.Net.WebClient webClient = new System.Net.WebClient())
            {
                // webClient.Headers.Add("Content-Type", "multipart/form-data");
                // webClient.Headers["Content-Type"] = "application/json";
                // webClient.Headers.Add("Content-Type", "application/pdf");
                webClient.Headers["User-Agent"] = "Lord Vishnu/Transcendental (Vaikuntha;Supreme Personality of Godness)";
                webClient.Headers["Accept-Language"] = System.Globalization.CultureInfo.CurrentCulture.Name;

                imageBytes = webClient.DownloadData(strURL);

                string strContentType = webClient.ResponseHeaders["content-type"];
                System.Diagnostics.Debug.WriteLine(strContentType);

                if (!System.StringComparer.OrdinalIgnoreCase.Equals(strContentType, "application/pdf"))
                {
                    throw new System.FormatException("Content-type returned from ApertureVWS is not \"application/pdf\".");
                } // End if (!StringComparer.OrdinalIgnoreCase.Equals(strContentType, "application/pdf"))

            } // End Using webClient

            return imageBytes;
        } // End Function DownloadPdf



        public static string BeautifyXML(System.Xml.XmlDocument doc)
        {
            string strRetValue = null;
            System.Text.Encoding enc = System.Text.Encoding.UTF8;
            // enc = new System.Text.UTF8Encoding(false);

            System.Xml.XmlWriterSettings xmlWriterSettings = new System.Xml.XmlWriterSettings();
            xmlWriterSettings.Encoding = enc;
            xmlWriterSettings.Indent = true;
            xmlWriterSettings.IndentChars = "    ";
            xmlWriterSettings.NewLineChars = "\r\n";
            xmlWriterSettings.NewLineHandling = System.Xml.NewLineHandling.Replace;
            //xmlWriterSettings.OmitXmlDeclaration = true;
            xmlWriterSettings.ConformanceLevel = System.Xml.ConformanceLevel.Document;


            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                using (System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(ms, xmlWriterSettings))
                {
                    doc.Save(writer);
                    writer.Flush();
                    ms.Flush();

                    writer.Close();
                } // End Using writer

                ms.Position = 0;
                using (System.IO.StreamReader sr = new System.IO.StreamReader(ms, enc))
                {
                    // Extract the text from the StreamReader.
                    strRetValue = sr.ReadToEnd();

                    sr.Close();
                } // End Using sr

                ms.Close();
            } // End Using ms


            /*
            System.Text.StringBuilder sb = new System.Text.StringBuilder(); // Always yields UTF-16, no matter the set encoding
            using (System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(sb, settings))
            {
                doc.Save(writer);
                writer.Close();
            } // End Using writer
            strRetValue = sb.ToString();
            sb.Length = 0;
            sb = null;
            */

            xmlWriterSettings = null;
            return strRetValue;
        } // End Function BeautifyXML




        public static string XmlEscape(string unescaped)
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            System.Xml.XmlNode node = doc.CreateElement("root");
            node.InnerText = unescaped;
            return node.InnerXml;
        } // End Function XmlEscape


        public static string XmlUnescape(string escaped)
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            System.Xml.XmlNode node = doc.CreateElement("root");
            node.InnerXml = escaped;
            return node.InnerText;
        } // End Function XmlUnescape


        public static System.Text.RegularExpressions.Regex re = new System.Text.RegularExpressions.Regex(@"(<text\s*.*?>)(.*?)(</text>)", System.Text.RegularExpressions.RegexOptions.Singleline | System.Text.RegularExpressions.RegexOptions.Compiled);


        public static string SanitizeTextTags(System.Text.RegularExpressions.Match match)
        {
            string val1 = match.Groups[1].Value;
            string val2 = match.Groups[2].Value;
            string val3 = match.Groups[3].Value;

            // <text transform="matrix(0.5 0 0 0.5 1601 803.6625)" x="0" y="0" style="fill:#000000;font-family:WKIRTS+Helvetica;" font-size="9">
            //      ZR Ø30  <- 1%
		    // </text>

            // Ein Name darf nicht mit dem Zeichen '-', hexadezimaler Wert 0x2D, beginnen. Zeile 3423, Position 11.

            //return val1 + val2.Replace("<", "&lt;").Replace(">", "&gt;") + val3;
            return val1 + XmlEscape(val2) + val3;
        } // End Function SanitizeTextTags 


        public static string SanitizeXml(string FileName)
        {
            //string str = System.IO.File.ReadAllText(FileName, System.Text.Encoding.GetEncoding("ISO-8859-1"));
            string str = System.IO.File.ReadAllText(FileName, System.Text.Encoding.UTF8);

            // Convert Encoding:
            // byte[] utf8 = null;
            // byte[] converted = System.Text.Encoding.Convert(System.Text.Encoding.GetEncoding(28591), System.Text.Encoding.UTF8, utf8);


            // Remove invalid NULL characters
            str = str.Replace(System.Convert.ToChar(0).ToString(), "");

            // Fix: <text> bla < bla </text>
            str = re.Replace(str, new System.Text.RegularExpressions.MatchEvaluator(SanitizeTextTags));
            //str = System.Text.RegularExpressions.Regex.Replace(str, @"(<text\s*.*?>)(.*?)(</text>)", Callback, System.Text.RegularExpressions.RegexOptions.Singleline | System.Text.RegularExpressions.RegexOptions.Compiled);

            return str;
        } // End Function SanitizeXml 


        // <text x="20" y="105" style="fill:#FF0064;font-family:Times New Roman;" font-size="56">
        //      Evaluation Only. Created with Aspose.Pdf. Copyright 2002-2014 Aspose Pty Ltd.
        // </text>
        public static string RemoveEvalString(string FileName, string URL)
        {
            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            xmlDoc.XmlResolver = null;
            xmlDoc.PreserveWhitespace = true;
            // xmlDoc.Load(FileName);

            string sanitizedContent = SanitizeXml(FileName);
            try
            {
                xmlDoc.LoadXml(sanitizedContent);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.Clipboard.SetText(sanitizedContent);
                System.Console.WriteLine(URL);
                System.Windows.Forms.MessageBox.Show(ex.Message);
                throw;
            }


            System.Xml.XmlAttribute attr = xmlDoc.DocumentElement.Attributes["xmlns"];
            string strDefaultNamespace = null;

            if (attr != null)
                strDefaultNamespace = attr.Value;

            if (string.IsNullOrEmpty(strDefaultNamespace))
                strDefaultNamespace = "http://www.w3.org/2000/svg";

            System.Xml.XmlNamespaceManager nsmgr = new System.Xml.XmlNamespaceManager(xmlDoc.NameTable);
            nsmgr.AddNamespace("dft", strDefaultNamespace);


            System.Xml.XmlNodeList EvalVersionTags = xmlDoc.SelectNodes("//dft:text[contains(translate(text(), 'ABCDEFGHIJKLMNOPQRSTUVWXYZÄÖÜÉÈÊÀÁÂÒÓÔÙÚÛÇÅÏÕÑŒ', 'abcdefghijklmnopqrstuvwxyzäöüéèêàáâòóôùúûçåïõñœ'),'aspose')]", nsmgr);
            //System.Xml.XmlNodeList EvalVersionTags = xmlDoc.SelectNodes("//dft:text[contains(text(),'Aspose')]", nsmgr);

            foreach (System.Xml.XmlNode EvalVersionTag in EvalVersionTags)
            {
                if (EvalVersionTag.ParentNode != null)
                    EvalVersionTag.ParentNode.RemoveChild(EvalVersionTag);
            } // Next EvalVersionTag

            // string str = xmlDoc.OuterXml;
            string str = BeautifyXML(xmlDoc);

            xmlDoc = null;
            return str;
        } // End Sub RemoveEvalString


        public class cConversionResult
        {
            public byte[] PDF;
            public string SVG;

            public cConversionResult()
            {
                this.PDF = null;
                this.SVG = null;
            }

        }


        public static cConversionResult ConvertFile(string drawingName, string polygonID)
        {
            cConversionResult ConversionResult = new cConversionResult();
            string URL = null;



            try
            {
                URL = string.Format(ExportSettings.ApertureWebServiceUrl111, drawingName, polygonID);

                // string RemoteURL = "https://www9.cor-asp.ch/ApWebServices/ApDrawingPDFs.aspx?p=SwisscomTest_Portal&d={0}&L=Z_Export&S=Z_Export";
                // string LocalURL = "http://vm-wincasa/ApWebServices/ApDrawingPDFs.aspx?p=Swisscom_Portal&d={0}&L=Z_Export&S=Z_Export";

                //baReturnValue = DownloadPdf("https://www9.cor-asp.ch/ApWebServices/ApDrawingPDFs.aspx?p=SwisscomTest_Portal&d=1002_GB01_OG01_0000&L=Z_Export");
                //baReturnValue = DownloadPdf("http://vm-wincasa/ApWebServices/ApDrawingPDFs.aspx?p=Swisscom_Portal&d=1002_GB01_OG01_0000&L=Z_Export");
                //baReturnValue = DownloadPdf("http://vm-wincasa/ApWebServices/ApDrawingPDFs.aspx?p=Swisscom_Portal&d=1010_GB01_UG02_0000&L=Z_Export");
                // http://vm-wincasa/ApWebServices/ApDrawingPDFs.aspx?p=Swisscom_Portal&d=1010_GB01_UG02_0000&L=Z_Export&S=Z_Export

                ConversionResult.PDF = DownloadPdf(URL);
            }
            catch (System.Net.WebException we)
            {
                System.Console.WriteLine(we.Message);
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }


#if WITH_ASPOSE 

            string temporaryOutFileName = System.IO.Path.GetTempFileName();

            using (System.IO.Stream ms = new System.IO.MemoryStream(ConversionResult.PDF))
            {
                // using (Aspose.Pdf.Document doc = new Aspose.Pdf.Document(@"dwg17.pdf"))
                using (Aspose.Pdf.Document doc = new Aspose.Pdf.Document(ms))
                {
                    Aspose.Pdf.SvgSaveOptions saveOptions = new Aspose.Pdf.SvgSaveOptions();

                    // do not compress SVG image to Zip archive
                    saveOptions.CompressOutputToZipArchive = false;

                    doc.Save(temporaryOutFileName, saveOptions);
                } // End Using doc
                
                ms.Close();
            } // End Using ms

            ConversionResult.SVG = RemoveEvalString(temporaryOutFileName, URL);
            if (System.IO.File.Exists(temporaryOutFileName))
                System.IO.File.Delete(temporaryOutFileName);
#endif

            return ConversionResult;
        } // End Function ConvertFile


        // BE 12768
        public static void InsertDocument(int BE_ID, string GS_UID, byte[] baDestFile, string strDrawing, string strFileName)
        {
            using (System.Data.IDbCommand cmd = SQL.CreateCommandFromFile("Insert_Dokument.sql"))
            {
                SQL.AddParameter(cmd, "__in_DK_UID", System.Guid.NewGuid());
                SQL.AddParameter(cmd, "__in_DK_Objekt_UID", GS_UID);
                SQL.AddParameter(cmd, "__in_DK_DKAT_UID", null);
                SQL.AddParameter(cmd, "__in_DK_Bezeichnung", strDrawing);
                SQL.AddParameter(cmd, "__in_DK_Datei", strFileName);
                SQL.AddParameter(cmd, "__in_DK_Dateiformat", ".svg");
                SQL.AddParameter(cmd, "__in_DK_Status", 1);
                SQL.AddParameter(cmd, "__in_DK_IsUpload", 1);
                SQL.AddParameter(cmd, "__in_DK_IsDefault", 0);
                SQL.AddParameter(cmd, "__in_DK_File", baDestFile);
                SQL.AddParameter(cmd, "__in_DK_BE_ID", BE_ID);
                SQL.AddParameter(cmd, "__in_DK_Mut_Date", System.DateTime.Now);
                SQL.AddParameter(cmd, "__in_DK_DK_UID", null); //System.Guid.Empty);

                SQL.ExecuteNonQuery(cmd);
            } // End Using cmd 

        } // End Sub InsertDocument 


        public static void SaveSVG(string strGS_UID, string strDrawing, string polygonId, string strSO_Nr, string strGB_Nr)
        {
            if (!ExportSettings.ConfigurationCorrect)
                return;

            if (!ExportSettings.ExecuteExport) // ExecuteExport: Export ausführen 
                return;

            // EXPType_ Export-Typ - Deine Code läuft nur wenn Wert = "SVG"
            if (
               !(
                        ExportSettings.ExportFileType.IsFlagSet(ExportFileType_t.svg)
                    ||  ExportSettings.ExportFileType.IsFlagSet(ExportFileType_t.pdf)
                )
            )
                return;


            if (ExportSettings.AskBeforeExport) // ASCExp: vor Export nachfragen 
            {
                DialogResult diagRes = MessageBox.Show("Wollen Sie die Datei aktualisieren ?", "Aktualisieren", MessageBoxButtons.YesNo);
                if (diagRes != DialogResult.Yes)
                {
                    return;
                }
                else if (diagRes == DialogResult.No)
                {
                    //do something else
                    return;
                }
            } // End if (SvgSettings.AskBeforeExport)


            string strPath = ExportSettings.ExportDirectory;
            if (ExportSettings.ExportWithSubdirectories)
                strPath = Helpers.CombinePaths(strPath, strSO_Nr, strGB_Nr); //, strGS_DisplayNr);

            if (!System.IO.Directory.Exists(strPath))
                System.IO.Directory.CreateDirectory(strPath);

            string dateToAppend = System.DateTime.Now.ToString("'_'yyyyMMdd_HHmmss");
            string strRawFileName = strDrawing;
            string strFileName = strRawFileName;


            // DateTimeExp: Datum u. Zeitstempel im Dateinamen 
            // DWGReplace: vorhandene Datei ohne Datum u. Zeitstempel im Dateinamen überschreiben 
            if (ExportSettings.DateTimeInFileName)
                strFileName += dateToAppend;

            string saveFileNameWithDate = System.IO.Path.Combine(strPath, strFileName);
            string saveFileNameWithoutDate = System.IO.Path.Combine(strPath, strRawFileName);


            cConversionResult ConversionResult = ConvertFile(strDrawing, polygonId);



            if (ExportSettings.DateTimeInFileName)
            {
                if (ExportSettings.ExportFileType.IsFlagSet(ExportFileType_t.svg))
                    if (ConversionResult.SVG != null)
                        System.IO.File.WriteAllText(saveFileNameWithDate + ".svg", ConversionResult.SVG, System.Text.Encoding.UTF8);

                if (ExportSettings.ExportFileType.IsFlagSet(ExportFileType_t.pdf))
                    if (ConversionResult.PDF != null)
                        System.IO.File.WriteAllBytes(saveFileNameWithDate + ".pdf", ConversionResult.PDF);
            }


            if (!ExportSettings.DateTimeInFileName || ExportSettings.DateTimeInFileName && ExportSettings.OverwriteFileIfExists)
            {
                string svgFile = saveFileNameWithoutDate + ".svg";
                string pdfFile = saveFileNameWithoutDate + ".pdf";


                if (ExportSettings.ExportFileType.IsFlagSet(ExportFileType_t.svg))
                    if (ConversionResult.SVG != null)
                        System.IO.File.WriteAllText(svgFile, ConversionResult.SVG, System.Text.Encoding.UTF8);

                if (ExportSettings.ExportFileType.IsFlagSet(ExportFileType_t.pdf))
                    if (ConversionResult.PDF != null)
                        System.IO.File.WriteAllBytes(pdfFile, ConversionResult.PDF);
            }

            if (ConversionResult.SVG != null)
            {

                if (ExportSettings.ExportFileType.IsFlagSet(ExportFileType_t.svg) && ExportSettings.SaveToDatabase)
                {
                    byte[] bom = System.Text.Encoding.UTF8.GetPreamble();
                    byte[] baSVG = System.Text.Encoding.UTF8.GetBytes(ConversionResult.SVG);
                    byte[] baDestFile = new byte[baSVG.Length + bom.Length];
                    System.Array.Copy(bom, baDestFile, bom.Length);
                    System.Array.Copy(baSVG, 0, baDestFile, bom.Length, baSVG.Length);

                    int BE_ID = 12768;
                    InsertDocument(BE_ID, strGS_UID, baDestFile, strDrawing, strFileName);
                } // End if (SvgSettings.SaveToDatabase)

            } // End if (svg != null)

        } // End Sub SaveSVG



        private class cDwgInfo
        {
            public bool HaveInfo = false;

            public string ZO_GSDWG_ApertureDWG = null;
            public string SO_Nr = null;
            public string GB_Nr = null;
            //public string GST_Kurz_DE;
            //public string GS_Nr;
            //public string GS_DisplayNr;
            //public string GS_IsAussengeschoss;
            public string SO_UID = null;
            public string GB_UID = null;
            public string GS_UID = null;
            //public string ZO_GSDWG_UID;
            //public string ZO_GSDWG_DatumVon;
            //public string ZO_GSDWG_DatumBis;
            public string ZO_GSDWG_ApertureObjID;
        } // End Class cDwgInfo


        private static cDwgInfo GetDwgInfo(string strApertureDWG)
        {
            cDwgInfo DwgInfo = new cDwgInfo();

            using (System.Data.IDbCommand cmd = SQL.CreateCommandFromFile("Geschoss.sql"))
            {
                SQL.AddParameter(cmd, "__in_ZO_GSDWG_ApertureDWG", strApertureDWG);

                using (System.Data.DataTable dt = SQL.GetDataTable(cmd))
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        System.Data.DataRow dr = dt.Rows[0];

                        DwgInfo.GS_UID = System.Convert.ToString(dr["GS_UID"]);
                        DwgInfo.ZO_GSDWG_ApertureDWG = System.Convert.ToString(dr["ZO_GSDWG_ApertureDWG"]);
                        DwgInfo.SO_Nr = System.Convert.ToString(dr["SO_Nr"]);
                        DwgInfo.GB_Nr = System.Convert.ToString(dr["GB_Nr"]);
                        DwgInfo.ZO_GSDWG_ApertureObjID = System.Convert.ToString(dr["ZO_GSDWG_ApertureObjID"]);
                        

                        DwgInfo.HaveInfo = true;
                    } // End if (dt != null && dt.Rows.Count > 0)

                } // End Using dt

            } // End Using cmd

            return DwgInfo;
        } // End Function GetDwgInfo


        public static void ConvertSingleFile(string strApertureDWG)
        {
            cDwgInfo DwgInfo = GetDwgInfo(strApertureDWG);
            if (DwgInfo.HaveInfo)
                SaveSVG(DwgInfo.GS_UID, DwgInfo.ZO_GSDWG_ApertureDWG, DwgInfo.ZO_GSDWG_ApertureObjID, DwgInfo.SO_Nr, DwgInfo.GB_Nr);
        } // End Sub ConvertSingleFile


        public static void ConvertAllFiles()
        {
            using (System.Data.DataTable dt = SQL.GetDataTableFromEmbeddedRessource("Geschosszeichnungen.sql"))
            {
                foreach (System.Data.DataRow dr in dt.Rows)
                {
                    string strGS_UID = System.Convert.ToString(dr["GS_UID"]);
                    string strDrawing = System.Convert.ToString(dr["ZO_GSDWG_ApertureDWG"]);


                    string strSO_Nr = System.Convert.ToString(dr["SO_Nr"]);
                    string strGB_Nr = System.Convert.ToString(dr["GB_Nr"]);
                    // string strGS_Nr = System.Convert.ToString(dr["GS_Nr"]);
                    // string strGS_DisplayNr = System.Convert.ToString(dr["GS_DisplayNr"]);

                    string polygonId = System.Convert.ToString(dr["ZO_GSDWG_ApertureObjID"]);

                    SaveSVG(strGS_UID, strDrawing, polygonId, strSO_Nr, strGB_Nr);
                } // Next dr 

            } // End Using dt 

        } // End Sub ConvertAllFiles


    } // End Class SvgConverter


} // End Namespace Pdf2Svg
