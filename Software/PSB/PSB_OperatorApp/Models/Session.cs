﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSB_OperatorApp.Models
{
 
    public class Session
    {
       
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public void Update()
        {
            Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                Start.Hour, Start.Minute, Start.Second);

            End = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                End.Hour, End.Minute, End.Second);
            if (End < Start)
            {
                End = End.AddDays(1);
            }
        }
    }
}
