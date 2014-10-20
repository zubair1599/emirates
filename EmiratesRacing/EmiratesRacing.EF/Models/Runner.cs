using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmiratesRacing.EF.Models
{
    public class Runner
    {
        public virtual Horse Horse { get; set; }
        
        public virtual Jockey Jockey { get; set; }

        public int RunnerID { get; set; }

        public string Equipment { get; set; }
        public string Name { get; set; }
        public virtual Trainer Trainer { get; set; }

        public string Position { get; set; }
        public int Drawn { get; set; }

        public string OR { get; set; }

        public string Margin   { get; set; }


        public virtual Race Race { get; set; }


    }
}
