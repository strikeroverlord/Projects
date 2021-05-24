using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace ForexWebApp
{
      
    public class Rates
    {
                
        public DateTime Date { get; set; }
        public string Quote { get; set; }
        public string Base { get; set; }
        public double Rate { get; set; }


        public static void GetRates(string FromDate, string ToDate)
        {
            string accesskey = "9379d1e280c8d1f1f4f96d2c288dc3e1";

            var daterange = GenerateDates(DateTime.Parse(FromDate), DateTime.Parse(ToDate));

            foreach (var dt in daterange)
            {
                CallWebAPI(dt.ToString("yyyy-MM-dd"), accesskey).Wait();
            }

        }

        public static List<DateTime> GenerateDates(DateTime FromDate, DateTime ToDate)
        {

           var daterange = Enumerable.Range(0, 1 + ToDate.Subtract(FromDate).Days)
          .Select(offset => FromDate.AddDays(offset))
          .ToList();

            return daterange;
        }


        public static async Task CallWebAPI(string historicaldate, string accesskey)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://data.fixer.io/api/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync(historicaldate + "?access_key=" + accesskey + "&symbols=&format=1");

                if (response.IsSuccessStatusCode)
                {
                    Json jsondata = await response.Content.ReadAsAsync<Json>(); 
                    Results.RatesDataCollections.AddRange(Json.ConvertJsonDataToRatesList(jsondata));
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }


    }

}

