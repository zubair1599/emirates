using EmiratesRacing.EF;
using EmiratesRacing.EF.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EmiratesRacing.Web.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {

            //int startIndex = 2555; // 2537;
            //int endIndex = 2600;// 2567;
            //int idPerThread = (endIndex - startIndex) / 10;
            //List<Task> tsks = new List<Task>();

            //for (int i = 0; i < 10; i++)
            //{
            //    int taskEndIndex = startIndex + idPerThread;
            //    Task tk = new Task(n => Start(startIndex, taskEndIndex), 10000);
            //    tk.Start();
            //    startIndex = taskEndIndex;
            //    tsks.Add(tk);
            //}




            //Task t = new Task((n => Sum((Int32)n), 1000);


            //int tempIndex = startIndex;
            //List<Thread> allThreads = new List<Thread>();
            //for (int i = 0; i < 10; i++)
            //{
            //    int threadEndIndex = tempIndex + idPerThread;

            //    Thread t2 = new Thread(() =>
            //    {
            //        {
            //            int start = tempIndex;
            //            int end = threadEndIndex;

            //            for (int j = start; j < end; j++)
            //            {
            //                System.Diagnostics.Trace.WriteLine("Thread :" + i + " Temp Index" + tempIndex);
            //                string URL = "http://www.emiratesracing.com/node/6?id=" + (j).ToString();
            //                DownloadData(URL);

            //            }
            //        }


            //    });
                

            //    t2.Start();
            //    Thread.Sleep(1000);
            //    tempIndex = threadEndIndex;
            //    allThreads.Add(t2);
            //}

            int lowest = 1454;
            int highest = 2600;
            try
            {
                int number = lowest;
                while (number <= highest)
                {
                    System.Diagnostics.Trace.WriteLine("Processing "+number);
                    string URL = "http://www.emiratesracing.com/node/6?id=" + number.ToString();
                    //Thread t2 = new Thread(() =>
                    //{
                    DownloadData(URL);
                    //});
                    System.Diagnostics.Trace.WriteLine("Done for ID :" + number);
                    //t2.Start();

                    //allThreads.Add(t2);
                    number = number + 1;
                }

            }
            catch (Exception ex)
            {
                throw;
            
            }

            
            
            return View();
            
        }

        public void Start(int st, int end)
        {
            try
            {
                for (int i = st; i < end; i++)
                {
                    string URL = "http://www.emiratesracing.com/node/6?id=" + (i).ToString();
                    DownloadData(URL);
                    
                }
            }
            catch (Exception ex)
            {

                
            }
            
            
        }



        public async Task<HtmlDocument> GetData(string url)
        {
            try
            {
                HttpClient http = new HttpClient();
                var response = await http.GetStringAsync(url).ConfigureAwait(false);

                //String source = Encoding.GetEncoding("utf-8").GetString(response, 0, response.Length - 1);
                string source = response;
                source = WebUtility.HtmlDecode(source);
                HtmlDocument resultat = new HtmlDocument();
                resultat.LoadHtml(source);
                return resultat;  
            }
            catch (Exception ex)
            {


                throw;
            }

            return null;
        
        }

        public void DownloadData(string url)
        {
            string MAIN_URL = url;
            HtmlDocument doc;
            Task<HtmlDocument> tsk = GetData(MAIN_URL);
            doc = tsk.Result;
            Link lnk = new Link();
            lnk.URL = (MAIN_URL);

            
            HtmlNode node = doc.GetElementbyId("racecard");

            string courseandDate = node.ChildNodes[1].InnerText;


            DateTime raceDate;
            string raceLocation = "";
            if (courseandDate.Contains('-'))
            {
                raceLocation = courseandDate.Split('-').ElementAt(0).Split(',').ElementAt(0);
                var dateStr = courseandDate.Split('-').ElementAt(1).Split(',').ElementAt(1);
                raceDate = DateTime.Parse(dateStr);
            }
            else
            {

                raceLocation = courseandDate.Split(',').ElementAt(0);
                int tempLen = courseandDate.Split(',').Count();
                raceDate = DateTime.Parse(courseandDate.Split(',').ElementAt(tempLen-1));
            }


            var allRace = node.ChildNodes[5].ChildNodes.Where(m => m.OriginalName.Equals("li"));
            int totalRaces = allRace.Count();
            if (totalRaces>0)
            {
                using (var conn = new RaceContext())
                {
                    conn.Links.Add(lnk);
                    //conn.SaveChanges();
                    lnk.Races = new List<Race>();
                    var tempcontentsRaces = node.ChildNodes[7].ChildNodes[3].ChildNodes[1];
                    var contentsRaces = tempcontentsRaces.ChildNodes.Where(m => m.Name == "div");

                    List<Race> races = new List<Race>();

                    foreach (var item in contentsRaces)
                    {
                        Race race = new Race();
                        race.Date = raceDate;
                        race.Location = raceLocation;

                        var itemcontentsRace = item.ChildNodes.Where(m => m.Attributes.Contains("class") && m.Attributes["class"].Value.Contains("raceDetails")).SingleOrDefault();
                        var itemcontentsRaceRunners = item.ChildNodes.Where(m => m.Attributes.Contains("class") && m.Attributes["class"].Value.Contains("cardFields")).SingleOrDefault();

                        var detailsInfo = itemcontentsRace.ChildNodes.Where(m => m.Attributes.Contains("class") && m.Attributes["class"].Value.Contains("detailInfo")).SingleOrDefault();

                        race.Title = detailsInfo.ChildNodes[1].InnerText;
                        var tempTime = detailsInfo.ChildNodes[3].ChildNodes[1].InnerText;
                        race.Time = race.Date;
                        if (!tempTime.Equals(" : : : ") && !tempTime.Equals("  "))
                        {
                            race.Time = race.Time.AddHours(double.Parse(tempTime.Split(':')[0]));
                            race.Time = race.Time.AddMinutes(double.Parse(tempTime.Split(':')[1]));
                        }

                        var typeandclass = detailsInfo.ChildNodes[3].ChildNodes[3].InnerText;
                        race.Class = typeandclass.Split('-')[1];
                        race.Type = typeandclass.Split('-')[0];
                        race.TrackLength = detailsInfo.ChildNodes[3].ChildNodes[5].InnerText.Split('M')[0] + "M";
                        race.TrackType = detailsInfo.ChildNodes[3].ChildNodes[5].InnerText.Split('M')[1];

                        var winningPriceCurrency = detailsInfo.ChildNodes[3].ChildNodes[7].InnerText.Split(' ');
                        race.WinningCurrency = winningPriceCurrency.ElementAt(1);

                        string temprice = winningPriceCurrency.ElementAt(19).ToString().Replace(",", "");
                        race.WinningPrice = long.Parse(temprice);

                        var railSafety = detailsInfo.ChildNodes.Where(m => m.Attributes.Contains("class") && m.Attributes["class"].Value.Contains("railSafety")).SingleOrDefault();
                        race.Weather = railSafety.ChildNodes[1].ChildNodes[2].ChildNodes.Count()>0 ?railSafety.ChildNodes[1].ChildNodes[2].ChildNodes[0].InnerText : "";

                        race.TrackCondition = railSafety.ChildNodes[3].ChildNodes[2].ChildNodes.Count > 0 ? railSafety.ChildNodes[3].ChildNodes[2].ChildNodes[0].InnerText : "";

                        race.RailPosition = railSafety.ChildNodes[7].ChildNodes[2].ChildNodes.Count > 0 ? railSafety.ChildNodes[7].ChildNodes[2].ChildNodes[0].InnerText : "";

                        race.SafetyLimit = railSafety.ChildNodes[9].ChildNodes[2].ChildNodes.Count > 0 ? railSafety.ChildNodes[9].ChildNodes[2].ChildNodes[0].InnerText : "";

                        var finishTime = detailsInfo.ChildNodes.Where(m => m.Attributes.Contains("class") && m.Attributes["class"].Value.Contains("finishTime")).SingleOrDefault();
                        //finishTime.InnerText.Substring(finishTime.InnerText.IndexOf(@" " "))
                        Regex reg = new Regex("[0-9]{2}:[0-9]{2}:[0-9]{2}");
                        Match mch = reg.Match(finishTime.InnerText);
                        race.RunningTime = mch.Value;

                        //getting null string <span>(Hand Timed)</span> = <span></span>
                        reg = new Regex("[(].*[)]");
                        mch = reg.Match(finishTime.InnerHtml);
                        race.RunningTimeType = mch.Value;
                        //race.RunningTimeType = "Test";
                        var fullConditions = itemcontentsRace.ChildNodes.Where(m => m.Attributes.Contains("class") && m.Attributes["class"].Value.Contains("fullConditions")).SingleOrDefault();
                        var prizeBreak = fullConditions.ChildNodes.Where(m => m.Attributes.Contains("class") && m.Attributes["class"].Value.Contains("prizeBreak")).SingleOrDefault();


                        var prizeTable = prizeBreak.ChildNodes.Where(m => m.Name == "table").SingleOrDefault();


                        var prizeTableTr = prizeTable.ChildNodes.Where(m => m.Name == "tr");
                        List<WinningPrice> allPrizes = new List<WinningPrice>();
                        List<Runner> allRunners = new List<Runner>();

                        int[] pricePosition = { 1, 4, 2, 5, 3, 7 };
                        int tmp = 0;

                        foreach (var bodyRow in prizeTableTr)
                        {

                            WinningPrice prize = new WinningPrice();
                            WinningPrice prize2 = new WinningPrice();

                            var tablePrizestd = bodyRow.ChildNodes.Where(m => m.Name == "td");
                            prize.Position = pricePosition[tmp];
                            prize.Price = long.Parse(tablePrizestd.ElementAt(1).InnerHtml.Replace(",", ""));
                            prize.Race = race;
                            conn.WinningPrizes.Add(prize);
                            tmp = tmp + 1;

                            prize2.Position = pricePosition[tmp];
                            prize2.Price = long.Parse(tablePrizestd.ElementAt(4).InnerHtml.Replace(",", ""));
                            prize2.Race = race;

                            allPrizes.Add(prize);
                            allPrizes.Add(prize2);
                            tmp = tmp + 1;
                            conn.WinningPrizes.Add(prize2);

                        }
                        var detailedConditions = fullConditions.ChildNodes.Where(m => m.Attributes.Contains("class") && m.Attributes["class"].Value.Contains("detailedConditions")).SingleOrDefault();
                        race.Notes = detailedConditions.InnerText;

                        race.WinningPrices = allPrizes;
                        conn.Races.Add(race);

                        lnk.Races.Add(race);
                        //conn.SaveChanges();



                        var resultsTable = itemcontentsRaceRunners.ChildNodes.Where(m => m.Attributes.Contains("class") && m.Attributes["class"].Value.Contains("entriesTable resultsTable")).SingleOrDefault();
                        var resultsTableRows = resultsTable.ChildNodes.Where(m => m.Name == "tbody").SingleOrDefault().ChildNodes.Where(m => m.Name == "tr");
                        race.Runners = new List<Runner>();
                        foreach (var runnerRow in resultsTableRows)
                        {
                            Runner tempRunner = new Runner();
                            tempRunner.Name = runnerRow.Attributes["owner"].Value;
                            var tempRunnerCols = runnerRow.ChildNodes.Where(m => m.Name == "td");
                            var tempPositionValidate = tempRunnerCols.ElementAt(0).InnerText;

                            var tmpPosition = (tempPositionValidate.Contains("nd") ? tempPositionValidate.Replace("nd", "") : tempPositionValidate.Contains("st") ? tempPositionValidate.Replace("st", "") : tempPositionValidate.Contains("th") ? tempPositionValidate.Replace("th", "") : tempPositionValidate.Contains("rd") ? tempPositionValidate.Replace("rd", "") : tempPositionValidate);
                            tempRunner.Position = tmpPosition;
                            tempRunner.Margin = tempRunnerCols.ElementAt(1).InnerText.ToString();
                            tempRunner.Drawn = !string.IsNullOrEmpty(tempRunnerCols.ElementAt(2).InnerText) ? int.Parse(tempRunnerCols.ElementAt(2).InnerText) : 0;
                            tempRunner.OR = tempRunnerCols.ElementAt(3).InnerText;
                            //tempRunner. Horse 

                            string horseUrl = @"http://www.emiratesracing.com/" + tempRunnerCols.ElementAt(4).ChildNodes.Where(m => m.Name == "a").SingleOrDefault().Attributes["href"].Value;

                            Horse tempHorse = new Horse()
                            {
                                URL = horseUrl,
                                Name = tempRunnerCols.ElementAt(4).ChildNodes.Where(m => m.Name == "a").SingleOrDefault().InnerText,


                            };

                            conn.Horses.Add(tempHorse);
                            tempHorse.Runners = new List<Runner>();
                            tempHorse.Runners.Add(tempRunner);
                            tempRunner.Equipment = tempRunnerCols.ElementAt(6).InnerText;

                            string trainerURl = @"http://www.emiratesracing.com/" + tempRunnerCols.ElementAt(7).ChildNodes.Where(m => m.Name == "a").SingleOrDefault().Attributes["href"].Value;
                            Trainer tempTrainer = new Trainer()
                            {
                                Name = tempRunnerCols.ElementAt(7).ChildNodes.Where(m => m.Name == "a").SingleOrDefault().InnerText,
                                URL = trainerURl

                            };
                            tempTrainer.Runners = new List<Runner>();
                            tempTrainer.Runners.Add(tempRunner);
                            conn.Trainers.Add(tempTrainer);

                            string jockeyUrl = @"http://www.emiratesracing.com/" + tempRunnerCols.ElementAt(8).ChildNodes.Where(m => m.Name == "a").SingleOrDefault().Attributes["href"].Value;
                            Jockey tempJockey = new Jockey()
                            {
                                URL = jockeyUrl,
                                Name = tempRunnerCols.ElementAt(8).ChildNodes.Where(m => m.Name == "a").SingleOrDefault().InnerText,
                                Weight = decimal.Parse(tempRunnerCols.ElementAt(5).InnerText)


                            };

                            tempJockey.Runners = new List<Runner>();
                            tempJockey.Runners.Add(tempRunner);
                            conn.Jockeys.Add(tempJockey);
                            tempRunner.Race = race;
                            race.Runners.Add(tempRunner);

                            conn.Runners.Add(tempRunner);




                        }




                    
                        
                 
                    }
                    conn.SaveChanges();
                }
            }
               
               
            }
            
            
        }
    }
