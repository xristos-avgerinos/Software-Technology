using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftwareTechnology.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareTechnology.Controllers
{
    public class AdminsController : Controller
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public Admin admin { get; set; }
        [BindProperty]
        public VaccineCentre vaccineCentre { get; set; }
        public AdminsController(ApplicationDbContext db)
        {
            _db = db;
        }
        

        [HttpGet]
        public IActionResult AdminLogIn()
        {
            HttpContext.Session.Clear();
            return View();
        }

        [HttpPost]
        public IActionResult AdminLogIn(Admin model)
        {
            if (ModelState.IsValid)
            {
                admin = _db.Admins.FirstOrDefault(admin => admin.username == model.username
                 && admin.password == model.password);

                if (admin != null)
                {
                    HttpContext.Session.SetString("admin",admin.username);

                    return RedirectToAction("AdminHome");
                }

                ModelState.AddModelError(string.Empty, "Δέν βρέθηκε χρήστης με αυτά τα στοιχεία");
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.Any, NoStore = true)]
        public IActionResult AdminHome()
        {
            if (HttpContext.Session.GetString("admin") != null)
            {
                ViewBag.admin=HttpContext.Session.GetString("admin");
                int countcit = _db.Citizens.Count();
                ViewBag.countCIT = countcit;
                int countemp = _db.Employees.Count();
                ViewBag.countEMP = countemp;
                int countvc = _db.VaccineCentres.Count();
                ViewBag.countVC = countvc;

                var result = _db.VaccineCentres.ToList();
                return View(result);

            }
            else
            {
                return RedirectToAction("AdminLogIn");
            }

           
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.Any, NoStore = true)]
        public IActionResult AddVaccineCentre()
        {
            if (HttpContext.Session.GetString("admin") != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("AdminLogIn");
            }

            
        }
        [HttpPost]
        public IActionResult AddVaccineCentre(VaccineCentre model)
        {
            if (ModelState.IsValid)
            {
                VaccineCentre vc= _db.VaccineCentres.FirstOrDefault(vaccineCentre => vaccineCentre.Name.Equals(model.Name) && vaccineCentre.Township.Equals(model.Township));
                if (vc==null)
                {
                    _db.VaccineCentres.Add(model);
                    _db.SaveChanges();
                   
                    return RedirectToAction("AdminHome");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Το Εμβολιαστικό Κέντρο υπάρχει ήδη");
                }



            }

            return View();
        }

        public IActionResult LogOut()
        {
            return RedirectToAction("AdminLogIn");
        }

        public IActionResult DeleteVaccineCentre(int id)
        {
            vaccineCentre = _db.VaccineCentres.FirstOrDefault(vc => vc.ID == id);
            int count = _db.Appointments.Count(ap => ap.vaccineCentreID == vaccineCentre.ID);
            if (count== 0)
            {
                _db.VaccineCentres.Remove(vaccineCentre);
                _db.SaveChanges();
            }
            else
            {
                HttpContext.Session.SetString("msg", "true");
            }
            
            return RedirectToAction("AdminHome");

        }
    }
}
