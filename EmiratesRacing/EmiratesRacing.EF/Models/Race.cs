using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmiratesRacing.EF.Models
{
    public class Race
    {
        public int RaceID { get; set; }

        public virtual Link Link { get; set; }
        public string Title { get; set; }
        
        public DateTime Date { get; set; }
        
        public DateTime Time { get; set; }

        public string Type { get; set; }
        public string Class { get; set; }

        public string TrackLength { get; set; }
        public string TrackType { get; set; }

        public Int64 WinningPrice { get; set; }

        public string WinningCurrency { get; set; }

        public string Weather { get; set; }
        public string TrackCondition { get; set; }
        public string RailPosition { get; set; }
        public string SafetyLimit { get; set; }

        public virtual ICollection<WinningPrice> WinningPrices { get; set; }

        public virtual ICollection<Runner> Runners { get; set; }
        public string RunningTime { get; set; }

        public string RunningTimeType { get; set; }
        public string Location { get; set; }

        public string Notes { get; set; }

       


    }
}
