using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSB_OperatorApp.Models
{
    public class Actual
    {

        public int ActualID { get; set; }
        [MaxLength(100)]
        public String Reference { get; set; }

        public String SerialNo { get; set; }

        public DateTime StartTimestamp { get; set; }
        public Nullable<DateTime> EndTimestamp { get; set; }


        public Actual(String serialNo,String reference, DateTime ts,int plan)
        {
            SerialNo = serialNo;
            Reference = reference;
            StartTimestamp = ts;
            PlanID = plan;
        }

        public Actual()
        {

        }

        public int? PlanID { get; set; }
        public virtual Plan Plan{get;set;}

    }

    public class Actuals : ObservableCollection<Actual>
    {

    }
}
