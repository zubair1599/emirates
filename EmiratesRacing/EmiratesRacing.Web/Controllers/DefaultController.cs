using EmiratesRacing.EF;
using EmiratesRacing.EF.Models;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using System.Xml.Linq;

namespace EmiratesRacing.Web.Controllers
{
    public class DefaultController : Controller
    {


        public ActionResult HighChart()
        {


            return View();


        }

        public ActionResult Races()
        {
            return View();

        }

        public ActionResult DownloadHighChartHtml()
        {
            string serverPath = Server.MapPath("~/phantomjs/");
            string filename = DateTime.Now.ToString("ddMMyyyy_hhmmss") + ".pdf";
            string Url = "http://stagebelweb.azurewebsites.net/horse/216444";

            new Thread(new ParameterizedThreadStart(x =>
            {
                ExecuteCommand(string.Format("cd {0} & E: & phantomjs rasterize.js {1} {2} \"A4\"", serverPath, Url, filename));
            })).Start();

            var filePath = Path.Combine(Server.MapPath("~/phantomjs/"), filename);

            var stream = new MemoryStream();
            byte[] bytes = DoWhile(filePath);

            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=Image.pdf");
            Response.OutputStream.Write(bytes, 0, bytes.Length);
            Response.End();
            return RedirectToAction("HighChart");
        }



        private void ExecuteCommand(string Command)
        {
            try
            {
                ProcessStartInfo ProcessInfo;
                Process Process;

                ProcessInfo = new ProcessStartInfo("cmd.exe", "/K " + Command);
               
                ProcessInfo.CreateNoWindow = true;
                ProcessInfo.UseShellExecute = false;

                Process = Process.Start(ProcessInfo);
            }
            catch { }
        }


        private byte[] DoWhile(string filePath)
        {
            byte[] bytes = new byte[0];
            bool fail = true;

            while (fail)
            {
                try
                {
                    using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        bytes = new byte[file.Length];
                        file.Read(bytes, 0, (int)file.Length);
                    }

                    fail = false;
                }
                catch
                {
                    Thread.Sleep(1000);
                }
            }

            System.IO.File.Delete(filePath);
            return bytes;
        }

       

        public ViewResult FileToPDF(int id)
        {
            //var viewModel = file.Get(id);
            return View();
        }

        




















        // GET: Default
        [HttpPost , ValidateInput(false)]

        public ActionResult Image(string svgchart)
        {
            
            
            try
            {
                FileStream fs = new FileStream(Server.MapPath("~/temp.svg"), FileMode.Create, FileAccess.Write);
                StreamWriter s = new StreamWriter(fs);
                s.WriteLine(svgchart);
                s.Close();
                fs.Close();

                System.Diagnostics.Process objProcess = null;
                objProcess = new System.Diagnostics.Process();
                objProcess.StartInfo.Arguments = "-f " + Server.MapPath("~/") + "temp.svg" + " -e " + Server.MapPath("~/") + "temp.png";
                objProcess.StartInfo.FileName = @"C:\Program Files (x86)\Inkscape\inkscape.exe";
                objProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                objProcess.Start();
                //Wait until it's finished
                objProcess.WaitForExit();
                //Exitcode as String
                Console.WriteLine(objProcess.ExitCode.ToString());
                objProcess.Close();


            }
            catch (Exception ex)
            {
               
            }

           
            return View();
        }

        public ActionResult RenderIndexTemp() {

            WebClient cli = new WebClient();
            var html = cli.DownloadString("http://localhost:8120/default/HighChart/");

            

            //string html = RenderRazorViewToString("HighChart", null);
            var pdf = GeneratePdf(html, PaperKind.Legal);
            return File(pdf, "application/pdf");
            //return Content(html, "text/html");
        }



        [HttpPost, ValidateInput(false)]
        public ActionResult ReadHTML(string html)
        {



            //var pdf = GeneratePdf(html, PaperKind.Legal);
            //return File(pdf, "application/pdf");
            return RedirectToAction("Test");

        }
        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                                                                         viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
        public static byte[] GeneratePdf(string htmlstring, System.Drawing.Printing.PaperKind paperKind)
        {
            var converter = new PDFConverter();

            return converter.Convert(htmlstring, paperKind, PDFConverter.PDFOrientation.Portrait);
        }
        public ActionResult Test() { return View(); }
        
        public ActionResult Search() {
            return View();
        }

        public ActionResult Index()
        {

           
            return View();
            int lowest = 2000;
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
                    DownloadData(URL,number);
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

            
            
            
            
        }



        public ActionResult BelWeb()
        {
            for (int i = 0; i < 1000; i++)
            {
                string url = "https://stagebelweb.azurewebsites.net/Horse/711282";
                Task<HtmlDocument> tsk = GetData(url);
                HtmlDocument doc = tsk.Result;

                string text = doc.DocumentNode.InnerHtml;

                ViewBag.text = text;
            }
                
            return View();

        }


        [HttpGet]
        public JsonResult Horses() {

            try
            {
                var conn = new RaceContext();
                List<string> horses = (from a in conn.Horses select a.Name).ToList();
                return Json(horses, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                
                throw;
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

        public void DownloadData(string url, int nmbr)
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
                            string horseUrl = @"http://www.emiratesracing.com/" + tempRunnerCols.ElementAt(4).ChildNodes.Where(m => m.Name == "a").SingleOrDefault().Attributes["href"].Value;
                            Task<HtmlDocument> horseInformation = GetData(horseUrl);
                            var tempPositionValidate = tempRunnerCols.ElementAt(0).InnerText;

                            var tmpPosition = (tempPositionValidate.Contains("nd") ? tempPositionValidate.Replace("nd", "") : tempPositionValidate.Contains("st") ? tempPositionValidate.Replace("st", "") : tempPositionValidate.Contains("th") ? tempPositionValidate.Replace("th", "") : tempPositionValidate.Contains("rd") ? tempPositionValidate.Replace("rd", "") : tempPositionValidate);
                            tempRunner.Position = tmpPosition;
                            tempRunner.Margin = tempRunnerCols.ElementAt(1).InnerText.ToString();
                            tempRunner.Drawn = !string.IsNullOrEmpty(tempRunnerCols.ElementAt(2).InnerText) ? int.Parse(tempRunnerCols.ElementAt(2).InnerText) : 0;
                            tempRunner.OR = tempRunnerCols.ElementAt(3).InnerText;
                            //tempRunner. Horse 


                            //System.Diagnostics.Trace.WriteLine("Processing In Progress " + nmbr);

                            Horse tempHorse = new Horse()
                            {
                                URL = horseUrl,
                                Name = tempRunnerCols.ElementAt(4).ChildNodes.Where(m => m.Name == "a").SingleOrDefault().InnerText,


                            };




                            HtmlDocument docHorse = horseInformation.Result;
                            var allDivs = docHorse.DocumentNode.ChildNodes[2].ChildNodes[3].ChildNodes[3].ChildNodes[3].ChildNodes[1].ChildNodes[3];
                            tempHorse.Age = allDivs.ChildNodes[1].ChildNodes[0].InnerText;
                            tempHorse.Gender = allDivs.ChildNodes[1].ChildNodes[1].InnerText;
                            tempHorse.Color = allDivs.ChildNodes[1].ChildNodes[2].InnerText;
                            tempHorse.Notes = allDivs.ChildNodes[1].ChildNodes[3].InnerText;
                            var parentsDetails = allDivs.ChildNodes[3].ChildNodes[0].InnerText;
                            if (parentsDetails.Contains('-'))
                            {
                                tempHorse.Father = parentsDetails.Split('-')[0];
                                if (parentsDetails.Split('-')[1].Contains("by"))
                                {
                                    var motherandGrand = parentsDetails.Split('-')[1];
                                    int byStart = motherandGrand.IndexOf("by");
                                    tempHorse.Mother = motherandGrand.Substring(0, byStart).Trim();
                                    tempHorse.GrandFather = motherandGrand.Substring(byStart + 2, motherandGrand.Length- (byStart+2));

                                } 
                            }
                            tempHorse.Breeder =allDivs.ChildNodes[7].ChildNodes.Count()==2 ? allDivs.ChildNodes[7].ChildNodes[1].InnerText:"";
                            Owner own = new Owner ();
                            own.Horses = new List<Horse>();
                            own.URL =  @"http://www.emiratesracing.com/"+ allDivs.ChildNodes[5].ChildNodes[2].Attributes["href"].Value;
                            own.Name = allDivs.ChildNodes[5].ChildNodes[2].InnerText;
                            tempHorse.Owner = own;

                            Trainer horseTrainer = new Trainer();
                            horseTrainer.Name = allDivs.ChildNodes[9].ChildNodes[1].InnerText;
                            horseTrainer.URL = @"http://www.emiratesracing.com/" + allDivs.ChildNodes[9].ChildNodes[1].Attributes["href"].Value;
                            horseTrainer.Horses = new List<Horse>();
                            horseTrainer.Horses.Add(tempHorse);
                            tempHorse.Trainer = horseTrainer;


                            //System.Diagnostics.Trace.WriteLine("Processing In Progress Near to Complete " + nmbr);
                            conn.Trainers.Add(horseTrainer);
                            own.Horses.Add(tempHorse);
                            conn.Owners.Add(own);
                            conn.Horses.Add(tempHorse);
                            tempHorse.Runners = new List<Runner>();
                            tempHorse.Runners.Add(tempRunner);
                            tempRunner.Equipment = tempRunnerCols.ElementAt(6).InnerText;

                            string trainerURl = @"http://www.emiratesracing.com/" + tempRunnerCols.ElementAt(7).ChildNodes.Where(m => m.Name == "a").SingleOrDefault().Attributes["href"].Value;
                            Trainer tempTrainer = new Trainer ()
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

        [HttpGet]
        public JsonResult SearchResults(string word)
        {
            try
            {
                if (!string.IsNullOrEmpty(word))
                {
                    var cont = new RaceContext();

                    //List<string> ty = (from a in cont.Horses
                    //          where a.Name.Contains(word) select a.Name)
                    //          .ToList();

                    var ty = (from a in cont.Horses
                              where a.Name.StartsWith(word)
                              select new { name = a.Name.Trim(), owner = a.Owner.Name.Trim(), breed = a.Breeder.Trim() , value = a.HorseID })
                              .ToList();

                    //var obj = new Array[]
                    //{
                    //    new object(){
                             
                              
                    //              value: "jquery",
                    //              label: "jQuery",
                    //              desc: "the write less, do more, JavaScript library",
                    //              icon: "jquery_32x32.png"
                              
                              
                    
                    //    }
                    //}

                  
                   
                    var test = Json(ty, JsonRequestBehavior.AllowGet);
                    return test;
                }
                else return null;
                
            }
            catch (Exception)
            {

                return null;
            }
            
            
        }


        }

    public class HorseData {
        public string Name { get; set; }
        public string Breed { get; set; }
        public string Owner { get; set; }
        

    }
    
    }
