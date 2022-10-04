using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareTechnology.Models
{
    public class Admin
    {
        [Key]
       
        public String username { get; set; }
        
        public String password { get; set; }
    }
}
