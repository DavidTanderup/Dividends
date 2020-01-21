using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Dividends
{
    class DailyPrice
    {
        private static WebClient web = new WebClient();
        public DailyPrice(string symbol, string startDate)
        {
            Symbol = symbol;
            StartDate = startDate;
            HistoricalData = GetHistoricalData();
        }
        private DailyPrice() { }

        // Properties
        public string Symbol { get; set; }
        public DateTime Date { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        private string StartDate { get; set; }
        public List<DailyPrice> HistoricalData { get; set; }



        public List<DailyPrice> GetHistoricalData()
        {
            List<DailyPrice> dailyPrices = new List<DailyPrice>();
            var endDate = DateTime.Now.ToShortDateString();
            var num = (Convert.ToDateTime(endDate) - Convert.ToDateTime(StartDate)).ToString();
            num = num.Remove(num.IndexOf('.'));

            // Move to DataLocation
            string url = @"https://www.wsj.com/market-data/quotes/AAPL/historical-prices/download?MOD_VIEW=page&num_rows="+num+"&range_days="+num+"&startDate="+StartDate+"&endDate="+endDate;
            var days = web.DownloadString(url).Split('\n').Skip(1).ToArray();

            foreach (var day in days)
            {
                DailyPrice daily = new DailyPrice();
                var datapoints = day.Split(',').ToArray();
                daily.Symbol = Symbol;
                daily.Date = Convert.ToDateTime(datapoints[0]);
                daily.Open = Convert.ToDouble(datapoints[1]);
                daily.High = Convert.ToDouble(datapoints[2]);
                daily.Low = Convert.ToDouble(datapoints[3]);
                daily.Close = Convert.ToDouble(datapoints[4]);
                dailyPrices.Add(daily);
            }

            return dailyPrices;
        }
    }
}
