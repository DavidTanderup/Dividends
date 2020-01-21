using System;
using System.Collections.Generic;
using System.Text;

namespace Dividends
{
    public class DataURL
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="date">Look up date for NASDAQ Dividend Calendar</param>
        /// <returns>Returns string URL to the desired date</returns>
        public string GetDivCalendar(string date)
        {
            return @"https://api.nasdaq.com/api/calendar/dividends?date=" + date;
        }

        public string GetDivHistory(string symbol)
        {
            try
            {
                return @"https://api.nasdaq.com/api/quote/" + symbol + "/dividends?assetclass=stocks";

            }
            catch (Exception)
            {
                Console.WriteLine($"{symbol} is an ETF");
                return @"https://api.nasdaq.com/api/quote/" + symbol + "/dividends?assetclass=etf";
            }
        }
    }
}
