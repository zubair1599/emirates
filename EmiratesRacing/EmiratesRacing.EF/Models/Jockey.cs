using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmiratesRacing.EF.Models
{
    public class Jockey
    {
        public int JockeyID { get; set; }

        public string Name { get; set; }

        public string URL { get; set; }

        public decimal Weight { get; set; }

        public virtual ICollection<Runner> Runners { get; set; }


    }
}
