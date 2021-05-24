using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForexWebApp
{
    public static class Results
    {
        public static List<Rates> RatesDataCollections = new List<Rates>();
        public static List<DataCSV> DataCSVCollections = new List<DataCSV>();
        public static List<DataCombined> DataCombinedCollections = new List<DataCombined>();
        public static List<DataConverted> DataConvetedCollections = new List<DataConverted>();
        public static List<CountryGroup> DataGroupCollections = new List<CountryGroup>();


        public static void JoinData()
        {
            var joindata = (from csvdata in DataCSVCollections
                            from ratedata in RatesDataCollections.Where(x => csvdata.Date == x.Date && csvdata.Currency == x.Base).DefaultIfEmpty()
                            select new { Date = csvdata.Date, Country = csvdata.Country, Currency = csvdata.Currency, Amount = csvdata.Amount, Rate = ratedata != null ? ratedata.Rate : 1 }).ToList();

            foreach (var cdata in joindata)
            {
                DataCombinedCollections.Add(new DataCombined
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
            foreach (var cdata in DataCombinedCollections)
            {
                DataConvetedCollections.Add(new DataConverted
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

        public static void GroupCuntriesByCategories()
        {
            GroupCountyData("EU", new List<string>(new string[] { "AUSTRIA", "ITALY", "BELGIUM", "LATVIA" }));
            GroupCountyData("ROW", new List<string>(new string[] { "CHILE", "QATAR", "UNITED ARAB EMIRATES", "UNITED STATES OF AMERICA" }));
            GroupCountyData("United Kingdom", new List<string>(new string[] { "UNITED KINGDOM" }));
            GroupCountyData("Australia", new List<string>(new string[] { "AUSTRALIA" }));
            GroupCountyData("South Africa", new List<string>(new string[] { "SOUTH AFRICA" }));
                        
        }

        public static void GroupCountyData(string country_group, List<string> countries)
        {
            foreach (var cn in countries)
            {
                var groupquery = DataConvetedCollections.Where(obj => (obj.Country.Trim().ToUpper() == cn));

                foreach (var gq in groupquery)
                {
                    DataGroupCollections.Add(new CountryGroup { Group = country_group, Data = new DataConverted { Date = gq.Date, Country = gq.Country, Currency = gq.Currency, Amount = gq.Amount, Amount_EUR = gq.Amount_EUR } });
                }
            }

        }

               
        public static List<CountryGroupEUR> ConvertGroupToEur()
        {
            var qresult = from datagroup in DataGroupCollections
                          group datagroup by datagroup.Group into countryGroup
                          select new CountryGroupEUR
                          {
                              Country_Group = countryGroup.Key,
                              Total_Amount_EUR = countryGroup.Sum(x => x.Data.Amount_EUR),
                              CountryData = new List<Country>()
            
                          };

            var sorted = qresult.OrderBy(x => x.Total_Amount_EUR).ToList();

            return sorted;
                                    
        }
        
        public static List<CountryGroupEUR> AppendCountryData(List<CountryGroupEUR> sortedData)
        {

            for(var row = 0; row < sortedData.Count(); row++)
            {

                var sqresult = from datagroup in DataGroupCollections
                               where datagroup.Group == sortedData[row].Country_Group
                               group datagroup by datagroup.Data.Country into country
                               select new
                               {
                                   Country = country.Key,
                                   Total_Amount_EUR = country.Sum(x => x.Data.Amount_EUR)
                               };

                    foreach (var t in sqresult)
                    {
                        sortedData[row].CountryData.Add(new Country {CountryName=t.Country, Total_Amount_EUR =t.Total_Amount_EUR});                    
                    }

            }

            return sortedData;
        }

    }
}
