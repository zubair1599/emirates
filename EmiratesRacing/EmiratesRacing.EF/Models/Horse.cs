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
        public string Age { get; set; }
        public string Gender { get; set; }

        public string Color { get; set; }

        public string Breeder { get; set; }
        public string Notes { get; set; }

        public string Father { get; set; }

        public string Mother { get; set; }

        public string GrandFather { get; set; }

        public virtual Trainer Trainer { get; set; }

        public virtual Owner Owner { get; set; }


    }
}
