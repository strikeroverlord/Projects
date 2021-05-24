using System.Collections.Generic;

namespace ForexWebApp
{

    public class CountryGroupEUR
    {
        public int RowID { get; set; }
        public string Country_Group { get; set; }
        public double Total_Amount_EUR { get; set; }
        public List<Country> CountryData { get; set; }

    }

}