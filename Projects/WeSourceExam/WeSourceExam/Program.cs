using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.IO;

namespace WeSourceExam
{
    class Program
    {            

        static void Main(string[] args)
        {

            // Task 1 - Extract rates from 1st October 2019 and 31st October 2019
            ApiClient.GetRates("2019-10-01", "2019-10-31");  // Time-Series is unavailable for free subscription so need to call historical dates one by one
            
            // Task 2 - Load data files and merge into one data.         
            string datafilespath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName+@"\data\";
            var CSVFiles = new List<string>(new string[] { datafilespath+"data1.csv", datafilespath + "data2.csv", datafilespath + "data3.csv" });
            CSVReader.ProcessCSVFiles(CSVFiles);

            //Task 3 - Merge the data with the rates dataset                        
            DataProcessing.JoinData();

            //Task 4 - Add a new column or property called amount_eur, with the values in the column amount converted to Euros.
            DataProcessing.ForexConvertData();

            //Task 5 - Group Countries into the specified Categories.
            DataProcessing.GroupCountyData("EU", new List<string>(new string[] { "AUSTRIA", "ITALY", "BELGIUM", "LATVIA" }));
            DataProcessing.GroupCountyData("ROW", new List<string>(new string[] { "CHILE", "QATAR", "UNITED ARAB EMIRATES", "UNITED STATES OF AMERICA" }));
            DataProcessing.GroupCountyData("United Kingdom", new List<string>(new string[] { "UNITED KINGDOM" }));
            DataProcessing.GroupCountyData("Australia", new List<string>(new string[] { "AUSTRALIA" }));
            DataProcessing.GroupCountyData("South Africa", new List<string>(new string[] { "SOUTH AFRICA" }));


            //Task 6 - Calculate the total amount in Euro for each country group
            var qresult = from datagroup in ResultModel.DataGroupCollections
                          group datagroup by datagroup.Group into countryGroup
                          select new
                          {
                              Country_Group = countryGroup.Key,
                              Total_Amount_EUR = countryGroup.Sum(x => x.Data.Amount_EUR)
                          };

            var sorted = qresult.OrderBy(x => x.Total_Amount_EUR).ToList();


            // Task 7 -  Display on web view as a formatted HTML table
            string viewbody="";
            int rowid = 0;

            foreach(var s in sorted)
            {
                rowid++;
                viewbody += HTMLView.CreateMainRows(rowid, s.Country_Group, s.Total_Amount_EUR);

                var sqresult = from datagroup in ResultModel.DataGroupCollections
                               where datagroup.Group==s.Country_Group
                               group datagroup by datagroup.Data.Country into country
                              select new
                              {
                                  Country = country.Key,
                                  Total_Amount_EUR = country.Sum(x => x.Data.Amount_EUR)
                              };

                foreach(var t in sqresult)
                {
                    viewbody += HTMLView.CreateHiddenRows(rowid, t.Country, t.Total_Amount_EUR);                    
                }
            }

            HTMLView.CreateHTML(viewbody);
            HTMLView.OpenHTMLInBrowser();                        


        }

    }
}
