using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Dividends
{
    class Rating
    {
        public double Speed { get; set; }
        public double Reliability { get; set; }
        public double Grade { get; set; }
        private string Symbol { get; set; }
        public Rating(string symbol)
        {
            Symbol = symbol;
        }
        private Rating() { }

        DataURL data = new DataURL();
        WebClient web = new WebClient();
        public Rating GetRating()
        {
            Rating FinalRating = new Rating();

            // Get Dividend History
            var divHistory = new DividendHistory().GetHistory(web.DownloadString(data.GetDivHistory(Symbol)));

            // Check # Entries  i => 10 continue
            bool tenOrMore = divHistory.Count >= 10;

            if (tenOrMore)
            {
                var topTen = divHistory.GetRange(0, 10);
                DailyPrice daily = new DailyPrice(Symbol, topTen[9].ExDate.AddDays(-1).ToShortDateString());

                // Get Historical Data: Start date == index 9
                var dailyPrice = daily.GetHistoricalData();


                List<Rating> RatingList = new List<Rating>();
                //Iterate through Exdates
                for (int i = 0; i < dailyPrice.Count; i++)
                {
                    //Console.WriteLine($"{i}");
                    for (int L = 9; L > 0; L--)
                    {
                        try
                        {
                            if (dailyPrice[i].Date == topTen[L].ExDate.AddDays(-1))
                            {
                                //Console.WriteLine($"{dailyPrice[i].Date}");
                                Rating rating = new Rating();
                                var range = dailyPrice.GetRange(i - 6, 5);

                                for (int j = 4; j >= 0; j--)
                                {
                                    if (range[j].High >= dailyPrice[i].Close)
                                    {
                                        switch (j)
                                        {
                                            case 0:
                                                rating.Speed = 1;
                                                break;
                                            case 1:
                                                rating.Speed = 2;
                                                break;
                                            case 2:
                                                rating.Speed = 3;
                                                break;
                                            case 3:
                                                rating.Speed = 4;
                                                break;
                                            case 4:
                                                rating.Speed = 5;
                                                break;
                                            default:
                                                rating.Speed = 0;
                                                break;
                                        }
                                        break;
                                    }
                                }
                                RatingList.Add(rating);

                            }

                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            Console.WriteLine("Out of Range");

                        }
                        //Console.WriteLine($"{topTen[L].ExDate}");

                    }

                }

                FinalRating.Reliability = 0;
                foreach (var ratingItem in RatingList)
                {
                    if (ratingItem.Speed > 0)
                    {
                        FinalRating.Reliability += 1;
                    }
                }
                FinalRating.Speed = SpeedAverage(RatingList);
                FinalRating.Grade = GetGrade(FinalRating.Reliability, FinalRating.Speed);

                // Stock recovery in 5 days

            }
            return FinalRating;
        }


        private double SpeedAverage(List<Rating> ratingsList)
        {
            double total = 0;
            foreach (var item in ratingsList)
            {
                total += item.Speed;
            }
            return total / ratingsList.Count;
        }

        private double GetGrade(double reliability, double speed)
        {
            var grade = reliability + speed;

            if (reliability == 10)
            {
                grade += 3;
            }
            else if (reliability == 9)
            {
                grade += 2;
            }
            else if (reliability == 8)
            {
                grade += 1;
            }
            else if (reliability < 7)
            {
                grade -= 5;
            }
            return grade;
        }
    }
}
