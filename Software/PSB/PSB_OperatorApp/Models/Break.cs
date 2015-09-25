using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSB_OperatorApp.Models
{
    public class Break : Session
    {
        public int BreakID { get; set; }

        public int ShiftID { get; set; }
        public virtual Shift Shift { get; set; }

        public Break()
        {

        }

        public Break(DateTime start, DateTime end)
        {
            this.Start = start;
            this.End = end;
        }


    }
}
