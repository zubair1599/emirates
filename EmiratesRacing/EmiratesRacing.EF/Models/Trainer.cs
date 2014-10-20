using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmiratesRacing.EF.Models
{
    public class Trainer
    {
        public string Name { get; set; }
        public string URL { get; set; }
        public int TrainerID { get; set; }

        public virtual ICollection<Runner> Runners { get; set; }

    }
}
