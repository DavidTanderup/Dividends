using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Dividends
{
    class CurrentPrice
    {
        WebClient web = new WebClient();
        public CurrentPrice(string symbol)
        {
            Symbol = symbol;
        }

        public string Symbol { get; set; }

        public double GetInfo()
        {
            try
            {
                var url = @"https://api.nasdaq.com/api/quote/" + Symbol + "/info?assetclass=stocks";
                var data = web.DownloadString(url);
                return Parse(data);
            }
            catch (Exception)
            {
                Console.WriteLine($"{Symbol} is an ETF");
                var url = @"https://api.nasdaq.com/api/quote/" + Symbol + "/info?assetclass=etf";
                var data = web.DownloadString(url);
                return Parse(data);
            }
        }

        private double Parse(string data)
        {
            var start = data.IndexOf("lastSalePrice", 0)+16;
            var end = data.IndexOf('"', start);
            return Convert.ToDouble(data.Substring(start, end - start).Replace('$', ' ').Replace('"', ' ').TrimStart().TrimEnd());

        }
    }
}
