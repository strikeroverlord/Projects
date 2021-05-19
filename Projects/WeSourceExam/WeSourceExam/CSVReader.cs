using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace WeSourceExam
{
    public class CSVReader
    {     
        public static void ProcessCSVFiles(List<string> CSVFiles)
        {
            foreach (string file in CSVFiles)
            {
                List<data> datalist = ReadCSV(file);
                ResultModel.DataCSVCollections.AddRange(datalist);
            }

        }

        public static List<data> ReadCSV(string SourceFile)
        {            
            var lines = File.ReadLines(SourceFile, Encoding.UTF8);
            var records = (from line in lines                           
                           let fields = line.Replace(", ", ",").Split(",")
                           where fields[0].Trim().ToUpper()!="DATE" // Ignore CSV Header
                           select new data {
                               Date=DateTime.Parse(fields[0]),
                               Country=fields[1],
                               Currency=fields[2],
                               Amount=double.Parse(fields[3])
                          }).ToList();                      
            return records;            
        }


    }
}
