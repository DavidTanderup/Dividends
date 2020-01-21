using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Dividends
{
    class Calendar
    {
        private static WebClient web = new WebClient();
        private static DataURL data = new DataURL();

        List<string> CompanyData = new List<string>() { "companyName", "symbol", "dividend_Ex_Date",
                                                        "payment_Date", "record_Date", "dividend_Rate",
                                                        "indicated_Annual_Dividend", "announcement_Date" };
        public string CompanyName { get; set; }
        public string Symbol { get; set; }
        public DateTime ExDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime RecordDate { get; set; }
        public double DividendRate { get; set; }
        public double AnnualDividend { get; set; }
        public DateTime AnouncementDate { get; set; }
        private string SearchDate { get; set; }

        private string URL
        {
            get
            {
                return data.GetDivCalendar(SearchDate);
            }
        }

        public Calendar(string date)
        {
            SearchDate = date;
        }
        private Calendar() { }


        public List<Calendar> GetCalendar()
        {
            List<Calendar> calendars = new List<Calendar>();
            try
            {
                var doc = web.DownloadString(URL);
                var start = doc.IndexOf('[');
                var end = doc.IndexOf(']');
                var rows = doc.Substring(start, end - start).Replace('{', ' ').Replace('[', ' ').Split("},").ToArray();



                foreach (var row in rows)
                {
                    Calendar calendar = new Calendar();
                    for (int i = 0; i < 8; i++)
                    {
                        int startCategory = row.IndexOf(CompanyData[i], 0);
                        int divider = row.IndexOf(':', startCategory) + 1;
                        int endCategory;
                        if (i + 1 < 8)
                        {
                            endCategory = row.IndexOf(CompanyData[i + 1]);
                        }
                        else
                        {
                            endCategory = row.Length;
                        }
                        switch (i)
                        {
                            case 0:
                                calendar.CompanyName = row.Substring(divider, endCategory - divider).Replace('"', ' ').Replace(',', ' ').TrimStart().TrimEnd();
                                break;
                            case 1:
                                calendar.Symbol = row.Substring(divider, endCategory - divider).Replace('"', ' ').Replace(',', ' ').TrimStart().TrimEnd();
                                break;
                            case 2:
                                calendar.ExDate = Convert.ToDateTime(row.Substring(divider, endCategory - divider).Replace('"', ' ').Replace(',', ' ').TrimStart().TrimEnd());
                                break;
                            case 3:
                                calendar.PaymentDate = Convert.ToDateTime(row.Substring(divider, endCategory - divider).Replace('"', ' ').Replace(',', ' ').TrimStart().TrimEnd());
                                break;
                            case 4:
                                calendar.RecordDate = Convert.ToDateTime(row.Substring(divider, endCategory - divider).Replace('"', ' ').Replace(',', ' ').TrimStart().TrimEnd());
                                break;
                            case 5:
                                calendar.DividendRate = Convert.ToDouble(row.Substring(divider, endCategory - divider).Replace('"', ' ').Replace(',', ' ').TrimStart().TrimEnd());
                                break;
                            case 6:
                                calendar.AnnualDividend = Convert.ToDouble(row.Substring(divider, endCategory - divider).Replace('"', ' ').Replace(',', ' ').TrimStart().TrimEnd());
                                break;
                            case 7:
                                calendar.AnouncementDate = Convert.ToDateTime(row.Substring(divider, endCategory - divider).Replace('"', ' ').Replace(',', ' ').Replace('}', ' ').TrimStart().TrimEnd());
                                break;

                        }
                    }
                    calendars.Add(calendar);
                }


            }
            catch (ArgumentOutOfRangeException)
            {

                Console.WriteLine($"Nothing Scheduled for {SearchDate}");
            }
            return calendars;

        }


    }
}
