using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftwareTechnology.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace SoftwareTechnology.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _db;
        
        public Employee employee { get; set; }
        
        public VaccineCentre vaccineCentre { get; set; }
        public EmployeesController(ApplicationDbContext db)
        {
            _db = db;
        }
        
 
        [HttpGet]
        public IActionResult EmployeeLogIn()
        {
            HttpContext.Session.Clear();
            return View();
        }

        [HttpPost]
        public IActionResult EmployeeLogIn(Employee model)
        {
            
            if (ModelState.IsValid)
            {
                
                employee = _db.Employees.FirstOrDefault(emp => emp.username == model.username
                 && emp.password == model.password);
          


                if (employee != null)
                {
                    

                    HttpContext.Session.SetString("empAFM", employee.AFM);
                    
                    
                    return RedirectToAction("EmployeeHome");
                }

                ModelState.AddModelError(string.Empty, "Δέν βρέθηκε χρήστης με αυτά τα στοιχεία");
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.Any, NoStore = true)]
        public IActionResult EmployeeHome()
        {
            if (HttpContext.Session.GetString("empAFM") != null)
            {
                string empafm = HttpContext.Session.GetString("empAFM");
                employee = _db.Employees.FirstOrDefault(emp => emp.AFM == empafm);

                ViewBag.username = employee.username;
                ViewBag.afm = employee.AFM;
                ViewBag.cont = employee.ContactNumber;
                ViewBag.name = employee.Name;
                ViewBag.surname = employee.Surname;

                int countemp = _db.Employees.Count(emp => emp.vaccineCentreID == employee.vaccineCentreID);
                ViewBag.countEMP = countemp;
                var result = (

                                    from e in _db.Employees
                                    join a in _db.Appointments
                                    on e.vaccineCentreID equals a.vaccineCentreID
                                    join c in _db.Citizens
                                    on a.citizenAMKA equals c.AMKA
                                    join v in _db.VaccineCentres
                                    on e.vaccineCentreID equals v.ID
                                    where e.AFM == empafm
                                    select new MultipleClassJoin
                                    {

                                        employee = e,
                                        citizen = c,
                                        appointment = a,
                                        vaccineCentre = v

                                    }
                                    );

                vaccineCentre = _db.VaccineCentres.FirstOrDefault(vc => vc.ID == employee.vaccineCentreID);
                
                ViewBag.VcName = vaccineCentre.Name + " " + vaccineCentre.Township;

                return View(result.ToList());
               
            }
            else
            {
                return RedirectToAction("EmployeeLogIn");
            }
            
            
        }


        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("EmployeeLogIn");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.Any, NoStore = true)]
        public IActionResult EditEmployee()
        {
            if (HttpContext.Session.GetString("empAFM") != null)
            {
                string tmp = HttpContext.Session.GetString("empAFM");
                employee = _db.Employees.FirstOrDefault(employee => employee.AFM == tmp);


                return View(employee);
            }
            else
            {
                return RedirectToAction("EmployeeLogIn");
            }
        }

        [HttpPost]
        public IActionResult EditEmployee(Employee model)
        {
            if (ModelState.IsValid)
            {
                Employee emp =_db.Employees.FirstOrDefault(User => User.username == model.username && User.AFM!=model.AFM);
                if (emp!=null)
                {
                    ModelState.AddModelError(string.Empty, "Το όνομα χρήστη υπάρχει ήδη");
                }
                else
                {
                    _db.Employees.Update(model);
                    _db.SaveChanges();

                    return RedirectToAction("EmployeeHome");
                }

                
            }

            return View(model);
            
        }




    }

}

