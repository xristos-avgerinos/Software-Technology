using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareTechnology.Models
{
    public class Citizen
    {
        
        public String Name { get; set; }
        
        public String Surname { get; set; }
        [Key]
        public String AMKA { get; set; }
        public String ContactNumber { get; set; }

        public String Township { get; set; }

        
        public int Dose { get; set; } = 0;

        
        public String username { get; set; }
        
        public String password { get; set; }



       

    }

   
}
