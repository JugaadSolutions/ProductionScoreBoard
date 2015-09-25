using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSB_OperatorApp.Models
{
    public class PSBContext : DbContext
    {
        public DbSet<Plan> Plans { get; set; }
        public DbSet<ProductModel> ProductModels { get; set; }
        public DbSet<Actual> Actuals { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<Break> Breaks { get; set; }

        public PSBContext()
            : base("name=DBConnectionString")
        {
            
        }
        
    }
}
