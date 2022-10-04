using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareTechnology.Models
{
    public class Appointment
    {   [Key]
        public int ID { get; set; }
        [Required]
        public  DateTime Date { get; set; }
        [Required]
        public TimeSpan Time { get; set; }
        [Required]
        public String citizenAMKA { get; set; }
        [Required]
        public int vaccineCentreID { get; set; }

        
       
    }
}
