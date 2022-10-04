using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareTechnology.Models
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Citizen> Citizens { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<VaccineCentre> VaccineCentres { get; set; }


        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Admin> Admins { get; set; }

       


    }
}
