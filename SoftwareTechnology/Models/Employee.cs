using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareTechnology.Models
{
    public class Employee 
    {
        
        public String Name { get; set; }
        
        public String Surname { get; set; }
        
        public String ContactNumber { get; set; }
        [Key]
        public String AFM { get; set; }

        
        public String username { get; set; }
        
        public String password { get; set; }
       
        public int  vaccineCentreID { get; set; }
    }
}
