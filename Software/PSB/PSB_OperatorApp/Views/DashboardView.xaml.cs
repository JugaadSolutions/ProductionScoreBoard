using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PSB_OperatorApp.Models;
using System.Windows.Threading;
using Andonmanager;
using System.Diagnostics;
using System.Timers;

namespace PSB_OperatorApp.Views
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class DashboardView : UserControl
    {
        Plan ActivePlan;
        List<Plan> PlansList;

        List<Actual> ActualsList;

        Plans Plans;

        Actuals Actuals;
        AndonManager AndonManager;
        Queue<int> StationList;

        List<Shift> Shifts;
        List<Break> Breaks;


        Shift CurrentShift;

        Timer AppTimer;

        Stopwatch EfficiencyWatch;

        public DashboardView()
        {
            InitializeComponent();

            AndonManager = new AndonManager(StationList, null, Andonmanager.AndonManager.MODE.MASTER);

            AndonManager.start();
            StationList = new Queue<int>();

            Plans = new Plans();
            PlanGrid.DataContext = Plans;

            Actuals = new Models.Actuals();
            ActualGrid.DataContext = Actuals;

            AppTimer = new Timer(1000);
            AppTimer.AutoReset = false;
            AppTimer.Elapsed += AppTimer_Elapsed;

            EfficiencyWatch = new Stopwatch();

            using (PSBContext DBContext = new PSBContext())
            {

                Shifts = DBContext.Shifts.ToList();


                foreach (Shift s in Shifts)
                {
                    s.Update();
                }

            }

            AppTimer.Start();

        }

        void AppTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            AppTimer.Stop();
            if (CurrentShift == null)
            {
                AppTimer.Interval = 30000;
            }

            Shift newShift = FindCurrentShift();
            if (CurrentShift == null || (newShift != CurrentShift))
            {
                AndonManager.addTransaction(1, AndonCommand.CMD_SYNC, null);
                AndonManager.addTransaction(1, AndonCommand.CMD_SYNC, null);

                CurrentShift = newShift;

                Initialize();

            }

            UpdateEfficiency();

            AppTimer.Start();
        }


        void Initialize()
        {

            

           


            using (PSBContext DBContext = new PSBContext())
            {

                Breaks = DBContext.Breaks.Where(b => b.ShiftID == CurrentShift.ShiftID).ToList();

                foreach (Break b in Breaks)
                {
                    b.Update();
                }

            }

            UpdatePlansActuals();

            UpdateReference();

            UpdateQuantity();
        }



        void UpdatePlansActuals()
        {
            if (PlansList != null)
            {
                PlansList.Clear();
            }

            if (ActualsList != null)
            {
                ActualsList.Clear();
            }

            this.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                Plans.Clear();
                Actuals.Clear();
            }));
            using (PSBContext DBContext = new PSBContext())
            {

                PlansList = DBContext.Plans.Include("Actuals")
                                             .Where(p => p.ShiftID == CurrentShift.ShiftID
                                                 && (p.CreatedTimestamp >= CurrentShift.Start) 
                                                 && (p.CreatedTimestamp < CurrentShift.End))
                                                 .ToList();

                foreach (Plan p in PlansList)
                {
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                     {
                         Plans.Add(p);
                     }));
                    if (p.Active == true)
                    {
                        ActivePlan = p;
                    }
                }



                ActualsList = DBContext.Actuals
                                         .Where(a => (a.EndTimestamp >= CurrentShift.Start) && (a.EndTimestamp < CurrentShift.End))
                                         .OrderBy(b => b.EndTimestamp)
                                         .ToList();

                foreach (Actual a in ActualsList)
                {
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                    {
                        Actuals.Add(a);
                    }));

                }

                this.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                {
                    PlanGrid.DataContext = Plans;
                    ActualGrid.DataContext = Actuals;

                }));
            }

        }


        private void UpdateReference()
        {
            String Reference = String.Empty;
            String Operators = String.Empty;

            foreach (Plan p in PlansList)
            {
                if (p.Active == true)
                {
                    Reference = p.Reference +"    ";
                    Operators = p.Operators.ToString("00");

                }
            }
            AndonManager.addTransaction(1, AndonCommand.CMD_SET_REFERENCE, Encoding.ASCII.GetBytes(Reference).ToList());
            AndonManager.addTransaction(1, AndonCommand.CMD_SET_OPERATOR, Encoding.ASCII.GetBytes(Operators).ToList());

        }


        void UpdateQuantity()
        {
            int pQty = 0, aQty = 0;
            String PlanQty = String.Empty;
            String ActualQty = String.Empty;

            foreach (Plan p in PlansList)
            {


                pQty += p.Quantity;
               


            
            }

            aQty += ActualsList.Count;
            PlanQty = pQty.ToString("0000");
            ActualQty = aQty.ToString("0000");

            AndonManager.addTransaction(1, AndonCommand.CMD_SET_PLANQTY, Encoding.ASCII.GetBytes(PlanQty).ToList());
            AndonManager.addTransaction(1, AndonCommand.CMD_SET_ACTUALQTY, Encoding.ASCII.GetBytes(ActualQty).ToList());
        }

        void UpdateEfficiency()
        {

            String HourlyKE = String.Empty;
            String ShiftKE = String.Empty;

            DateTime now = DateTime.Now;

            HourlyKE = ((int)(CalculateEfficiency(now.AddSeconds(-3600), now) * 100)).ToString("00000");
            ShiftKE = ((int)(CalculateEfficiency(CurrentShift.Start, now) * 100)).ToString("00000");



            AndonManager.addTransaction(1, AndonCommand.CMD_SET_HOURLYKE, Encoding.ASCII.GetBytes(HourlyKE).ToList());
            AndonManager.addTransaction(1, AndonCommand.CMD_SET_SHIFTKE, Encoding.ASCII.GetBytes(ShiftKE).ToList());
        }

        Shift FindCurrentShift()
        {
            foreach (Shift s in Shifts)
            {
                if ((DateTime.Now >= s.Start) && DateTime.Now <= s.End)
                {

                    return s;
                }


            }
            return null;
        }



        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ReferenceTextBox.Focus();
        }

        double CalculateEfficiency(DateTime StartReference, DateTime EndReference)
        {
            double Efficiency = 0.0;
            double TotalUnitTime = 0.0;
            double ActualUnitTime = 0.0;

            double ActualTime = 0.0;

            double DesignTime = 0.0;

            List<Actual> orderedActualList;

            using (PSBContext db = new PSBContext())
            {
                orderedActualList = db.Actuals
                                    .Where(a => (a.EndTimestamp >= StartReference) && (a.EndTimestamp < EndReference))
                                    .OrderBy(b => b.EndTimestamp)
                                    .ToList();
            }

            foreach (Actual a in orderedActualList)
            {
                TotalUnitTime = GetUsefulTime(a.StartTimestamp, a.EndTimestamp.Value);
                if (a.StartTimestamp < StartReference)
                {
                    ActualUnitTime += GetUsefulTime(StartReference, a.EndTimestamp.Value);

                }
                else ActualUnitTime = TotalUnitTime;

                foreach (Plan p in Plans)
                {
                    if (a.PlanID == p.PlanID)
                    {
                        DesignTime += p.DT * (ActualUnitTime / TotalUnitTime);
                        break;
                    }
                }
            }

            ActualTime = GetUsefulTime(StartReference, EndReference);

            Efficiency = (DesignTime / ActualTime) * 100;
            return Efficiency;


        }


        //double CalculateEfficiency(DateTime StartReference, DateTime EndReference)
        //{
        //    double Efficiency = 0.0;
        //    DateTime TimeReference = StartReference;

        //    List<Actual> orderedActualList;

        //    double ActualTime = 0.0;
        //    double DesignTime = 0.0;

        //    using (PSBContext db = new PSBContext())
        //    {
        //        orderedActualList = db.Actuals
        //                            .Where(a => (a.EndTimestamp >= StartReference) && (a.EndTimestamp < EndReference))
        //                            .OrderBy(b => b.EndTimestamp)
        //                            .ToList();
        //    }


        //    foreach (Actual a in orderedActualList)
        //    {


        //        foreach (Plan p in Plans)
        //        {
        //            if (a.PlanID == p.PlanID)
        //            {
        //                DesignTime += p.DT;
        //                break;
        //            }
        //        }
        //        TimeReference = a.EndTimestamp.Value;


        //    }


        //    if (DesignTime == 0)
        //    { return 0; }

        //    ActualTime = GetUsefulTime(StartReference, EndReference);

        //    Efficiency = DesignTime / ActualTime * 100;

        //    return Efficiency;

        //}


        double GetUsefulTime(DateTime Start, DateTime End)
        {
            double UT = (End - Start).TotalSeconds;
            foreach (Break b in Breaks)
            {
                if (b.Start >= Start && b.End <= End)
                {
                    UT -= (b.End - b.Start).TotalSeconds;
                }
                else if (b.Start < Start
                    && (b.Start <= End) && (b.End >= Start) && (b.End <= b.End))
                {
                    UT -= (Start - b.End).TotalSeconds;
                }

                else if ((b.Start >= Start) && (b.Start <= End) && (b.End >= b.Start) && (b.End > b.End))
                {
                    UT -= (b.Start - b.End).TotalSeconds;
                }
            }

            return UT;
        }

        #region PLAN

        private void AddPlanButton_Click(object sender, RoutedEventArgs e)
        {
            String reference = ReferenceTextBox.Text;
            int operators = Convert.ToInt32(OperatorTextBox.Text);
            int quantity = Convert.ToInt32(TargetQtyTextBox.Text);

            using (PSBContext db = new PSBContext())
            {
                var p = db.ProductModels.SingleOrDefault(pm => pm.Reference == ReferenceTextBox.Text);
                if (p == null)
                {
                    MessageBox.Show("Reference Not Found. Please Verify", "Application Message", MessageBoxButton.OK,
                        MessageBoxImage.Exclamation);
                    return;
                }
                Plan newPlan = new Plan(reference, quantity, operators);
                newPlan.CreatedTimestamp = DateTime.Now;
                newPlan.ShiftID = CurrentShift.ShiftID;
                newPlan.DT = p.DT / (0.75 * operators);
                db.Plans.Add(newPlan);
                db.SaveChanges();
                Plans.Add(newPlan);

            }






        }

        private void ActivateButton_Click(object sender, RoutedEventArgs e)
        {
            if (Plans.Count <= 0)
                return;

            if (PlanGrid.SelectedIndex == -1)
            {
                MessageBox.Show("Please Select Plan", "Application Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }



            using (PSBContext DB = new PSBContext())
            {


                int planIndex = PlanGrid.SelectedIndex;

                if (Plans[planIndex].Active == true)
                {

                    MessageBox.Show("Plan Already Activated", "Application Info",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    return;

                }

                int planid = Plans[planIndex].PlanID;

                var newActivePlan = DB.Plans.Include("Actuals").Single(p => p.PlanID == planid);
                if (ActivePlan != null)
                {
                    ActivePlan = DB.Plans.Include("Actuals").Single(p => p.PlanID == ActivePlan.PlanID);
                    ActivePlan.Active = false;




                    DB.SaveChanges();
                }

                ActivePlan = newActivePlan;

                ActivePlan.Active = true;

                ActivePlan.StartTimestamp = DateTime.Now;

                Actual newActual = new Actual("", ActivePlan.Reference, DateTime.Now, ActivePlan.PlanID);

                DB.Actuals.Add(newActual);

                DB.SaveChanges();



            }

            UpdatePlansActuals();

            UpdateReference();
            

        }


        #endregion

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                            new Action(() =>
                            {
                                ReferenceTextBox.Clear();
                                OperatorTextBox.Clear();
                                TargetQtyTextBox.Clear();
                                ReferenceTextBox.Focus();
                            }));

        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                           new Action(() =>
                           {
                               if (BaseTab.SelectedIndex == 0)
                               {
                                   ActualReferenceTextBox.Clear();
                                   SerialNoTextBox.Clear();
                                   ActualReferenceTextBox.Focus();

                               }

                               if (BaseTab.SelectedIndex == 1)
                               {
                                   ClearButton_Click(this, new RoutedEventArgs());
                               }
                           }));

        }


        #region ACTUAL


        private void SerialNoTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SerialNoTextBox.Text.Length == 12)
            {

                using (PSBContext db = new PSBContext())
                {
                    foreach (Plan p in Plans)
                    {
                        if (p.Reference == ActualReferenceTextBox.Text)
                        {
                            var actual = db.Actuals.Single(a => a.Reference == p.Reference 
                                && (a.PlanID == p.PlanID) 
                                && (a.EndTimestamp == null) && (a.SerialNo == String.Empty )
                                && (a.StartTimestamp >= CurrentShift.Start) && a.StartTimestamp <= CurrentShift.End);

                            actual.SerialNo = SerialNoTextBox.Text;
                            actual.EndTimestamp = DateTime.Now;

                            Actual newActual = new Actual("", p.Reference, actual.EndTimestamp.Value, p.PlanID);

                            db.Actuals.Add(newActual);
                            db.SaveChanges();

                            break;
                            
                        }
                    }
                }

                UpdatePlansActuals();

                UpdateQuantity();
                

                SerialNoTextBox.Clear();
                ActualReferenceTextBox.Clear();
            }
        }

        #endregion

       
        private void ActualReferenceTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ActualReferenceTextBox.Text.Length < 10)
                return;

            using (PSBContext db = new PSBContext())
            {
                var plan = db.Plans.SingleOrDefault(p => p.Reference == ActualReferenceTextBox.Text
                    && (p.CreatedTimestamp >= CurrentShift.Start) && (p.CreatedTimestamp < CurrentShift.End));
                   
                if (plan == null)
                    return;
                else
                    SerialNoTextBox.Focus();

            }
        }

    }
}
