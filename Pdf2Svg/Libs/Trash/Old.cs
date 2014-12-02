
namespace Pdf2Svg.Libs.Trash
{
    class Old
    {


        public static string StripInvalidXMLCharacters(string textIn)
        {
            System.Text.StringBuilder textOut = new System.Text.StringBuilder(); // Used to hold the output.
            //char current; // Used to reference the current character.
            int current;

            if (textIn == null || textIn == string.Empty) return string.Empty; // vacancy test.
            for (int i = 0; i < textIn.Length; i++)
            {
                //current = textIn[i];

                current = System.Convert.ToInt32(textIn[i]);


                if ((current == 0x9 || current == 0xA || current == 0xD) ||
                    ((current >= 0x20) && (current <= 0xD7FF)) ||
                    ((current >= 0xE000) && (current <= 0xFFFD)) ||
                    ((current >= 0x10000) && (current <= 0x10FFFF)))
                {
                    //textOut.Append(current);
                    textOut.Append(System.Convert.ToChar(current));
                }
            }
            return textOut.ToString();
        } // End Function StripInvalidXMLCharacters


        private static void RemoveEvalVersion()
        {

            //System.Xml.Linq.XDocument xdoc = System.Xml.Linq.XDocument.Load(@"D:\TestReport.svg");
            //var result = from p in xdoc.Descendants("content")
            //             where p.Value.Contains("Aspose")
            //             select p;
            // Console.WriteLine(result);



            // Loop through all encodings
            foreach (System.Text.EncodingInfo x in System.Text.Encoding.GetEncodings())
            {
                System.Text.Encoding enc = x.GetEncoding();
            }


            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            xmlDoc.XmlResolver = null;
            xmlDoc.Load(@"D:\TestReport.svg");


            System.Xml.XmlNamespaceManager nsmgr = new System.Xml.XmlNamespaceManager(xmlDoc.NameTable);
            nsmgr.AddNamespace("dft", "http://www.w3.org/2000/svg");



            // System.Xml.XmlNodeList AllNodesText = xmlDoc.SelectNodes("//dft:*[text()]", nsmgr);
            // Console.WriteLine(AllNodesText.Count);


            // [text()='Aspose']
            // System.Xml.XmlNode textNode = xmlDoc.SelectSingleNode("/dft:svg/dft:g/dft:text", nsmgr);
            // System.Xml.XmlNode textNode = xmlDoc.DocumentElement.SelectSingleNode("/dft:svg/dft:g/dft:text[normalize-space() =\"Evaluation Only. Created with Aspose.Pdf. Copyright 2002-2014 Aspose Pty Ltd.\"]", nsmgr);
            // System.Xml.XmlNode textNode = xmlDoc.DocumentElement.SelectSingleNode("//dft:text[normalize-space() =\"Evaluation Only. Created with Aspose.Pdf. Copyright 2002-2014 Aspose Pty Ltd.\"]", nsmgr);
            // System.Xml.XmlNode textNode = xmlDoc.SelectSingleNode("/dft:svg/dft:g/dft:text[normalize-space() =\"Evaluation Only. Created with Aspose.Pdf. Copyright 2002-2014 Aspose Pty Ltd.\"]", nsmgr);
            // System.Xml.XmlNode textNode = xmlDoc.SelectSingleNode("//*[contains(translate(text(), 'ABCDEFGHIJKLMNOPQRSTUVWXYZÄÖÜÉÈÊÀÁÂÒÓÔÙÚÛÇÅÏÕÑŒ', 'abcdefghijklmnopqrstuvwxyzäöüéèêàáâòóôùúûçåïõñœ'),'evaluation only')]", nsmgr);
            // Console.WriteLine(textNode);
            // if (textNode != null && textNode.ParentNode != null)
            //     textNode.ParentNode.RemoveChild(textNode);


            // System.Xml.XmlNodeList EvalVersionTags = xmlDoc.SelectNodes("//dft:text[normalize-space() =\"Evaluation Only. Created with Aspose.Pdf. Copyright 2002-2014 Aspose Pty Ltd.\"]", nsmgr);
            // System.Xml.XmlNodeList EvalVersionTags = xmlDoc.SelectNodes("//dft:text[contains(normalize-space(),'Evaluation Only')]", nsmgr);
            // System.Xml.XmlNodeList EvalVersionTags = xmlDoc.SelectNodes("//*[contains(translate(text(), 'ABCDEFGHIJKLMNOPQRSTUVWXYZÄÖÜÉÈÊÀÁÂÒÓÔÙÚÛÇÅÏÕÑŒ', 'abcdefghijklmnopqrstuvwxyzäöüéèêàáâòóôùúûçåïõñœ'),'evaluation only')]", nsmgr);
            // System.Xml.XmlNodeList EvalVersionTags = xmlDoc.SelectNodes("//dft:text[contains(text(),'Aspose')]", nsmgr);
            System.Xml.XmlNodeList EvalVersionTags = xmlDoc.SelectNodes("//*[contains(translate(text(), 'ABCDEFGHIJKLMNOPQRSTUVWXYZÄÖÜÉÈÊÀÁÂÒÓÔÙÚÛÇÅÏÕÑŒ', 'abcdefghijklmnopqrstuvwxyzäöüéèêàáâòóôùúûçåïõñœ'),'aspose')]", nsmgr);

            foreach (System.Xml.XmlNode EvalVersionTag in EvalVersionTags)
            {
                if (EvalVersionTag.ParentNode != null)
                    EvalVersionTag.ParentNode.RemoveChild(EvalVersionTag);
            } // Next EvalVersionTag


            System.Console.WriteLine(EvalVersionTags);


            // /dft:svg/dft:g/dft:text
            xmlDoc.Save(@"d:\lol.svg");
        }


    }


}
