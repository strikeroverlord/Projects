using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeSourceExam
{
    public class data
    {
        public DateTime Date { get; set; }
        public string Country { get; set; }
        public string Currency { get; set; }
        public double Amount { get; set; }
    }

    public class datacombined : data
    {
        public double Rate_EUR { get; set; }
    }

    public class dataconverted : datacombined
    {
        public double Amount_EUR { get; set; }
    }

    public class country_group {
        public string Group { get; set; }
        public double Total_Amount_EUR { get; set; }
        public dataconverted Data { get; set; }
        
    }
}
