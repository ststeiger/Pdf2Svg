
using System.Windows.Forms;


namespace Pdf2Svg
{


    static class Program
    {


        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [System.STAThread]
        static void Main()
        {
            bool bShowWindow = false;

            if (bShowWindow)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }

            // ProcessWorkbook();
            // SvgConverter.ConvertSingleFile("1010_GB02_ZG00_0000"); // Dwg.Name
            SvgConverter.ConvertAllFiles();

            System.Console.WriteLine(System.Environment.NewLine);
            System.Console.WriteLine(" --- Press any key to continue --- ");
            System.Console.ReadKey();
        } // End Sub Main




        private static string GetCellValueAsString(OfficeOpenXml.ExcelRange cell)
        {
            object objValue = null;

            if (cell == null)
                return null;

            if (cell != null)
                objValue = cell.Value;

            if (objValue == null)
                return null;

            return objValue.ToString().Trim();
        } // End Function GetCellValueAsString



        public static System.Data.DataTable ProcessWorkbook()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string fn = @"D:\username\Documents\Visual Studio 2013\Projects\Pdf2Svg\Pdf2Svg\Gebäudepläne.xlsx";

            if (System.IO.File.Exists(fn))
            using (System.IO.FileStream fs = new System.IO.FileStream(fn, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read))
            {

                // Get the file we are going to process
                // Open and read the XlSX file.
                // System.IO.FileInfo existingFile = new System.IO.FileInfo(fn);
                // using (OfficeOpenXml.ExcelPackage package = new OfficeOpenXml.ExcelPackage(existingFile))
                using (OfficeOpenXml.ExcelPackage package = new OfficeOpenXml.ExcelPackage(fs))
                {
                    // Get the work book in the file
                    OfficeOpenXml.ExcelWorkbook workBook = package.Workbook;
                    if (workBook != null)
                    {
                        // dt = ProcessingFunction(workBook);

                        // OfficeOpenXml.ExcelWorksheet ws = workBook.Worksheets.First();
                        OfficeOpenXml.ExcelWorksheet roomsWorksheet = workBook.Worksheets["ZeichnungsnamenPDFExport"];
                        //OfficeOpenXml.ExcelWorksheet UsageTypesWorksheet = workBook.Worksheets["Tabelle1"];

                        if (roomsWorksheet == null)
                            return dt;

                        // if (roomsWorksheet != null)
                        //     UsageTypesWorksheet.Hidden = OfficeOpenXml.eWorkSheetHidden.Hidden;



                        int iStartRow = roomsWorksheet.Dimension.Start.Row;
                        int iEndRow = roomsWorksheet.Dimension.End.Row;

                        int iStartColumn = roomsWorksheet.Dimension.Start.Column;
                        int iEndColumn = roomsWorksheet.Dimension.End.Column;



                        // OfficeOpenXml.ExcelRange cell = roomsWorksheet.Cells["A1"];
                        OfficeOpenXml.ExcelRange cell = null; // ewbGroupPermissionWorksheet.Cells[1, 1]; // Cells[y, x]
                        for (int j = 1; j <= iEndColumn; ++j)
                        {
                            cell = roomsWorksheet.Cells[1, j]; // Cells[y, x]
                            string title = GetCellValueAsString(cell);

                            // if (string.IsNullOrWhiteSpace(title)) continue;

                            dt.Columns.Add(title, typeof(string));
                        }

                        System.Console.WriteLine(dt.Columns.Count);


                        int ordObjekttyp = dt.Columns["Standort/Geschoss"].Ordinal + 1;
                        int ordZeichnungsname = dt.Columns["Zeichnungsname"].Ordinal + 1;

                        System.Data.DataRow dr = null;

                        // 2 - 348
                        for (int i = 2; i <= iEndRow; ++i)
                        {
                            dr = dt.NewRow();

                            for (int j = 1; j <= iEndColumn; ++j)
                            {
                                cell = roomsWorksheet.Cells[i, j]; // Cells[y, x]
                                string cellValue = GetCellValueAsString(cell);
                                dr[j - 1] = cellValue;
                            } // Next j 

                            dt.Rows.Add(dr);
                        } // Next i 

                        System.Console.WriteLine(dt.Rows.Count);
                    } // End if (workBook != null)

                } // End Using package

            } // End Using ms 

            return dt;
        }


    } // End  Class Program 


} // End Namespace Pdf2Svg 
