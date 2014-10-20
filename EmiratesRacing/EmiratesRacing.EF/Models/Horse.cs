using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmiratesRacing.EF.Models
{
    public class Horse
    {
        public int HorseID { get; set; }

        public string Name { get; set; }


        public string URL { get; set; }

        public virtual ICollection<Runner> Runners { get; set; }

    }
}
