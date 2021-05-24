using System;
using System.Collections.Generic;

namespace ForexWebApp
{
    public class Country
    {
       public string CountryName { get; set; }
       public double Total_Amount_EUR { get; set; }

        public static implicit operator List<object>(Country v)
        {
            throw new NotImplementedException();
        }
    }

}