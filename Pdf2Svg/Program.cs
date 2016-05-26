
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

            SvgConverter.ConvertSingleFile("1010_GB02_ZG00_0000"); // Dwg.Name
            // SvgConverter.ConvertAllFiles();

            System.Console.WriteLine(System.Environment.NewLine);
            System.Console.WriteLine(" --- Press any key to continue --- ");
            System.Console.ReadKey();
        } // End Sub Main


    } // End  Class Program 


} // End Namespace Pdf2Svg 
