using System;
using System.IO;

namespace WeSourceExam
{
    class HTMLView
    {
        public static void CreateHTML(string viewbody)
        {
            
            string html = WeSourceExam.Properties.Resources.Main.Replace("{BODY}", viewbody);
            File.WriteAllText(@"Webview.html",html);            
            
        }   
        
        public static string CreateMainRows(int ID, string COUNTRYGROUP,double AMOUNT)
        {
            string MainRows;

            MainRows = WeSourceExam.Properties.Resources.Row.Replace("{ROWID}",ID.ToString());
            MainRows = MainRows.Replace("{COUNTRYGROUP}", COUNTRYGROUP);
            MainRows = MainRows.Replace("{AMOUNT}", AMOUNT.ToString("0.##"));

            return MainRows;
        }

        public static string CreateHiddenRows(int ID, string COUNTRY, double AMOUNT)
        {
            string MainRows;

            MainRows = WeSourceExam.Properties.Resources.Hidden.Replace("{ROWID}", ID.ToString());
            MainRows = MainRows.Replace("{COUNTRY}", COUNTRY);
            MainRows = MainRows.Replace("{AMOUNT}", AMOUNT.ToString("0.##"));

            return MainRows;
        }

        public static void OpenHTMLInBrowser()
        {
            var runningProcess = System.Diagnostics.Process.GetProcessesByName("chrome");
            if (runningProcess.Length != 0)
            {
                System.Diagnostics.Process.Start(@"C:\Program Files\Google\Chrome\Application\chrome.exe", "Webview.html");
                return;
            }
            runningProcess = System.Diagnostics.Process.GetProcessesByName("firefox");
            if (runningProcess.Length != 0)
            {
                System.Diagnostics.Process.Start(@"C:\Program Files\Mozilla Firefox\firefox.exe", "Webview.html");
                return;
            }
            runningProcess = System.Diagnostics.Process.GetProcessesByName("msedge");
            if (runningProcess.Length != 0)
            {
                System.Diagnostics.Process.Start(@"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe", "Webview.html");
                return;
            }

            Console.WriteLine("No Open Browser Detected - You can open the file in " + Path.GetFullPath("Webview.html"));
            Console.ReadLine();
        }

    }
}
