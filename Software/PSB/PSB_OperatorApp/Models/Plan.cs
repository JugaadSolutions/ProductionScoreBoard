using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSB_OperatorApp.Models
{
    public class Plan : INotifyPropertyChanged
    {
        public int PlanID { get; set; }

        [MaxLength(100)]
        
        public String Reference { get; set; }

        public int Quantity { get; set; }
        public int Operators {get;set;}
        public Nullable<DateTime> CreatedTimestamp { get; set; }
        public Nullable<DateTime> StartTimestamp {get;set;}

        public double DT { get; set; }

        public bool Active { get; set; }

        public int ShiftID { get; set; }
        public virtual Shift Shift { get; set; }

      
        public virtual ICollection<Actual> Actuals { get; set; }


        public Plan(String reference,int quantity,int operators)
        {
            Reference = reference;
            Quantity = quantity;
            Operators = operators;
            Actuals = new Actuals();

        }

        public Plan()
        {

        }

        #region INotifyPropetyChangedHandler
        public event PropertyChangedEventHandler PropertyChanged;
        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        #endregion
        
    }

    public class Plans:ObservableCollection<Plan>
    {

    }
}
