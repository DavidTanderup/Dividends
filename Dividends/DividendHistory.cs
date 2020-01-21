using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dividends
{
    public class DividendHistory
    {
        public DateTime ExDate { get; set; }
        public DateTime Payment { get; set; }
        public string Type { get; set; }
        public double Amount { get; set; }
        public DateTime Declaration { get; set; }
        public DateTime Record { get; set; }

        List<string> Categories = new List<string>()
        { "exOrEffDate","type","amount","recordDate","paymentDate"};


        public List<DividendHistory> GetHistory(string history)
        {
            int start = history.IndexOf('[', 0);
            int end = history.IndexOf(']', start);
            var rows = history.Substring(start, end - start).Split('}').ToArray();
            List<DividendHistory> dividends = new List<DividendHistory>();
            foreach (var row in rows)
            {
                DividendHistory dividend = new DividendHistory();
                for (int i = 0; i < 5; i++)
                {
                    if (row.Contains(Categories[i]))
                    {
                        var stepOne = row.IndexOf(':', row.IndexOf(Categories[i])) + 2;
                        var stepTwo = row.IndexOf('"', stepOne);
                        var dataEntry = row.Substring(stepOne, stepTwo - stepOne).Replace('$', ' ').Replace('"', ' ').Replace('}', ' ').TrimStart().TrimEnd();
                        switch (i)
                        {
                            case 0:
                                dividend.ExDate = Convert.ToDateTime(DateClean(dataEntry));
                                break;
                            case 1:
                                dividend.Type = dataEntry;
                                break;
                            case 2:
                                dividend.Amount = Convert.ToDouble(dataEntry);
                                break;
                            case 3:
                                dividend.Record = Convert.ToDateTime(DateClean(dataEntry));
                                break;
                            case 4:
                                dividend.Payment = Convert.ToDateTime(DateClean(dataEntry));
                                break;

                        }

                    }

                }
                dividends.Add(dividend);
            }
            return dividends;
        }
        private string DateClean(string date)
        {
            try
            {
                Convert.ToDateTime(date);
                return date;
            }
            catch (Exception)
            {
                //Console.WriteLine("Didn't find the date");
                return "01/23/4567";
            }
        }

    }

}
