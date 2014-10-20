using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmiratesRacing.EF.Models
{
    public class WinningPrice
    {
        public int WinningPriceID { get; set; }
        public Race Race { get; set; }
        public int RaceId { get; set; }
        public int Position { get; set; }
        public Int64 Price { get; set; }

    }
}
