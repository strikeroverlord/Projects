using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;


namespace ForexWebApp
{
    public class DataCSV
    {
        public DateTime Date { get; set; }
        public string Country { get; set; }
        public string Currency { get; set; }
        public double Amount { get; set; }


        public static void ProcessCSVFiles(List<string> CSVFiles)
        {
            foreach (string file in CSVFiles)
            {
                List<DataCSV> datalist = ReadCSV(file);
                Results.DataCSVCollections.AddRange(datalist);
            }

        }

        public static List<DataCSV> ReadCSV(string SourceFile)
        {
            var lines = File.ReadLines(SourceFile, Encoding.UTF8);
            var records = (from line in lines
                           let fields = line.Replace(", ", ",").Split(",")
                           where fields[0].Trim().ToUpper() != "DATE" // Ignore CSV Header
                           select new DataCSV
                           {
                               Date = DateTime.Parse(fields[0]),
                               Country = fields[1],
                               Currency = fields[2],
                               Amount = double.Parse(fields[3])
                           }).ToList();
            return records;
        }

    }
}
