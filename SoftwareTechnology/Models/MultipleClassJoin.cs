using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareTechnology.Models
{
    public class MultipleClassJoin
    {
        public Citizen citizen { get; set; }
        public Employee employee { get; set; }
        public List<Appointment> appointmentList { get; set; }

        public Appointment appointment { get; set; }

        public VaccineCentre vaccineCentre { get; set; }
        public List<VaccineCentre> vaccineCentreList { get; set; }

        

       

        







    }
}
