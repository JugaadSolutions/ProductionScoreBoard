using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSB_OperatorApp.Models
{
    public class ProductModel
    {
        public int ProductModelID { get; set; }


        [MaxLength(100)]
        [Index("ReferenceIndex", IsUnique = true)]
        public String Reference { get; set; }


        public double DT { get; set; }

        public ProductModel()
        {

        }

     


    }
}
