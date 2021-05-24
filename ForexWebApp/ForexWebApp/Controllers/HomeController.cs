using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;

namespace ForexWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            // Task 1 - Extract rates from 1st October 2019 and 31st October 2019
            Rates.GetRates("2019-10-01", "2019-10-31");  // Time-Series is unavailable for free subscription so need to call historical dates one by one

            // Task 2 - Load data files and merge into one data.                     
            string datafilespath = Directory.GetParent(Directory.GetCurrentDirectory()).FullName + @"\ForexWebApp\wwwroot\data\";
            var CSVFiles = new List<string>(new string[] { datafilespath + "data1.csv", datafilespath + "data2.csv", datafilespath + "data3.csv" });
            DataCSV.ProcessCSVFiles(CSVFiles);

            // Task 3
            Results.JoinData();

            //Task 4 - Add a new column or property called amount_eur, with the values in the column amount converted to Euros.
            Results.ForexConvertData();

            //Task 5 - Group Countries into the specified Categories.
            Results.GroupCuntriesByCategories();

            //Task 6 - Calculate the total amount in Euro for each country group            
            var sortedData = Results.ConvertGroupToEur();

            // Task 7 -  Display on web view as a formatted HTML table                       
            var countryData = Results.AppendCountryData(sortedData);
            var viewForexData = new ViewForexData()
            {
                SortedCountryGroup = countryData
                
            };
                        
            return View(viewForexData);
        }

    }

 }

