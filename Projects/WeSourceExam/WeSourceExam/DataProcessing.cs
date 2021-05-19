using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeSourceExam
{
    class DataProcessing
    {

        public static void JoinData()
        {
            var joindata = (from csvdata in ResultModel.DataCSVCollections
                            from ratedata in ResultModel.RatesDataCollections.Where(x => csvdata.Date == x.Date && csvdata.Currency == x.Base).DefaultIfEmpty()
                            select new { Date = csvdata.Date, Country = csvdata.Country, Currency = csvdata.Currency, Amount = csvdata.Amount, Rate = ratedata != null ? ratedata.Rate : 1 }).ToList();

            foreach (var cdata in joindata)
            {
                ResultModel.DataCombinedCollections.Add(new datacombined
                {
                    Date = cdata.Date,
                    Country = cdata.Country,
                    Currency = cdata.Currency,
                    Amount = cdata.Amount,
                    Rate_EUR = cdata.Rate
                });
            }
        }

        public static void ForexConvertData()
        {
            foreach (var cdata in ResultModel.DataCombinedCollections)
            {
                ResultModel.DataConvetedCollections.Add(new dataconverted
                {
                    Date = cdata.Date,
                    Country = cdata.Country,
                    Currency = cdata.Currency,
                    Amount = cdata.Amount,
                    Rate_EUR = cdata.Rate_EUR,
                    Amount_EUR = cdata.Amount / cdata.Rate_EUR
                });
            }               
        }

        public static void GroupCountyData(string country_group,List<string> countries)
        {
            foreach (var cn in countries)
            {
                var groupquery = ResultModel.DataConvetedCollections.Where(obj => (obj.Country.Trim().ToUpper() == cn));

                foreach (var gq in groupquery)
                {
                    ResultModel.DataGroupCollections.Add(new country_group { Group = country_group, Data = new dataconverted { Date = gq.Date, Country = gq.Country, Currency = gq.Currency, Amount = gq.Amount, Amount_EUR = gq.Amount_EUR } });
                }
            }

        }
                       
    }
}
