using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareTechnology.Models
{
    public class VaccineCentre
    {   
        [Key]
        public int ID { get; set; }
        
        public String Name { get; set; }
        
        public String Township { get; set; }
    }
}
