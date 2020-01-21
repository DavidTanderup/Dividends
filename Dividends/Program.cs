using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace Dividends
{
    class Program
    {
        static void Main(string[] args)
        {
            //HtmlWeb web = new HtmlWeb();
            WebClient webClient = new WebClient();
            DividendHistory dividend = new DividendHistory();
            DataURL data = new DataURL();


            List<string> Week = new List<string>() { "2020-01-21", "2020-01-22", "2020-01-23", "2020-01-24", "2020-01-27", "2020-01-28", "2020-01-29", "2020-01-30", "2020-01-31" };

            List<string> fileList = new List<string>();
            foreach (var day in Week)
            {
                Console.WriteLine(day);
                Calendar calendar = new Calendar(day);
                var stocks = calendar.GetCalendar();




                //DailyPrice daily = new DailyPrice("aapl", "01/01/2018");

                // daily.GetHistoricalData();

                fileList.Add("Ex-Date,Symbol, Speed, Reliability, Grade, Dividend, Yield");
                foreach (var stock in stocks)
                {

                    CurrentPrice currentPrice = new CurrentPrice(stock.Symbol);
                    Rating rating = new Rating(stock.Symbol);
                    var url = data.GetDivHistory(stock.Symbol);
                    var history = webClient.DownloadString(url);
                    var records = dividend.GetHistory(history);

                    var final = rating.GetRating();
                    fileList.Add($"{stock.ExDate},{stock.Symbol},{Math.Round(final.Speed, 1)},{Math.Round(final.Reliability, 1)},{final.Grade},{records[0].Amount:C},{records[0].Amount / currentPrice.GetInfo():P}");
                    Console.WriteLine(stock.Symbol);
                }

                //if (final.Reliability > 6 && records[0].Amount / currentPrice.GetInfo() > .02)
                //{

                //}

            }
            File.WriteAllLines("Dividend.csv", fileList);
            Console.ReadLine();
        }




    }
}
