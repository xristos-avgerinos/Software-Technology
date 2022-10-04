using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SoftwareTechnology.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;


namespace SoftwareTechnology.Controllers
{
    
    public class CitizensController : Controller
    {

        private readonly ApplicationDbContext _db;
        [BindProperty]
        public Citizen citizen { get; set; }
        [BindProperty]
        public MultipleClassJoin multipleClass { get; set; }
        public CitizensController(ApplicationDbContext db)
        {
            _db = db;
            
        }

        
        public IActionResult CitizenLogIn()
        {
            HttpContext.Session.Clear();
            return View();
        }

        [HttpPost]
        public IActionResult CitizenLogIn(Citizen model)
        {
            
            if (ModelState.IsValid)
            {
                citizen = _db.Citizens.FirstOrDefault(cit => cit.username== model.username
                 && cit.password == model.password);

                

                if (citizen != null)
                {
                    HttpContext.Session.SetString("citizenAMKA", citizen.AMKA);
                    
                    return RedirectToAction("CitizenHome");
                }

                ModelState.AddModelError(string.Empty, "Δέν βρέθηκε χρήστης με αυτά τα στοιχεία");
            }

            return View(model);
        }

        public IActionResult CitizenSignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CitizenSignUp(Citizen model)
        {
            if (ModelState.IsValid)
            {
                Citizen citizen = _db.Citizens.FirstOrDefault(cit => cit.AMKA.Equals(model.AMKA));

                if (citizen == null)
                {
                    Citizen citizen1= _db.Citizens.FirstOrDefault(cit => cit.username.Equals(model.username));
                    if (citizen1 == null)
                    {
                        _db.Citizens.Add(model);
                        _db.SaveChanges();
                        return RedirectToAction("CitizenLogIn");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Το Όνομα χρήστη υπάρχει ήδη");
                        return View();
                    }

                    
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Το ΑΜΚΑ υπάρχει ήδη");
                }

            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.Any, NoStore = true)]
        public IActionResult CitizenHome()
        {
           
            if (HttpContext.Session.GetString("citizenAMKA") != null)
            {
                multipleClass = new MultipleClassJoin();
                string amka = HttpContext.Session.GetString("citizenAMKA");
                citizen = _db.Citizens.FirstOrDefault(cit => cit.AMKA.Equals(amka));

                multipleClass.citizen = citizen;
                if (citizen.Dose < 2)
                {

                    if (citizen.Dose == 1)
                    {
                        citizen.Dose--;
                        _db.SaveChanges();
                    }


                    var vcList = _db.VaccineCentres.Where(vc => vc.Township == citizen.Township).ToList();


                    multipleClass.vaccineCentreList = vcList;

                }
                else
                {

                    List<Appointment> aplist = _db.Appointments.Where(cit => cit.citizenAMKA == citizen.AMKA).ToList();
                    List<VaccineCentre> vaccineCentres = new List<VaccineCentre>();

                    foreach (var item in aplist)
                    {
                        vaccineCentres.Add((VaccineCentre)_db.VaccineCentres.FirstOrDefault(vc => vc.ID == item.vaccineCentreID));
                    }

                    multipleClass.appointmentList = aplist;
                    multipleClass.vaccineCentreList = vaccineCentres;

                }



                return View(multipleClass);
            }
            else
            {
                return RedirectToAction("CitizenLogIn");
            }

            
        }
        [HttpPost]
        public IActionResult CitizenHome(int Selected,String firstdose,String secdose)
        {

            
            ViewBag.Selected = Selected;
            multipleClass = new MultipleClassJoin();
            string amka = HttpContext.Session.GetString("citizenAMKA");
            citizen = _db.Citizens.FirstOrDefault(cit => cit.AMKA.Equals(amka));



            if (secdose != null)
            {

                HttpContext.Session.SetString("secDose", secdose);
                HttpContext.Session.SetInt32("vc2", Selected);
                

                if (citizen.Dose == 1)
                {
                    DateTime dt1 = DateTime.Parse(HttpContext.Session.GetString("firstDose"), null, System.Globalization.DateTimeStyles.RoundtripKind);

                    Appointment ap1 = new Appointment();
                    ap1.citizenAMKA = citizen.AMKA;
                    ap1.Date = dt1.Date;
                    ap1.Time = dt1.TimeOfDay;
                    ap1.vaccineCentreID = (int)HttpContext.Session.GetInt32("vc1");
                    _db.Appointments.Add(ap1);

                    DateTime dt2 = DateTime.Parse(HttpContext.Session.GetString("secDose"), null, System.Globalization.DateTimeStyles.RoundtripKind);

                    Appointment ap2 = new Appointment();
                    ap2.citizenAMKA = citizen.AMKA;
                    ap2.Date = dt2.Date;
                    ap2.Time = dt2.TimeOfDay;
                    ap2.vaccineCentreID = Selected;
                    _db.Appointments.Add(ap2);
                    citizen.Dose++;
                    _db.SaveChanges();
                }

                
                

            }
            else if (firstdose != null)
            {
                
                if (citizen.Dose==0)
                {  
                    HttpContext.Session.SetString("firstDose",firstdose);
                    
                    HttpContext.Session.SetInt32("vc1", Selected);
                    citizen.Dose++;
                    _db.SaveChanges();
                 
                    
                }


            }

            var vcList = _db.VaccineCentres.Where(vc => vc.Township == citizen.Township).ToList();
            var resap = (

               from vc in _db.VaccineCentres
               join ap in _db.Appointments
               on vc.ID equals ap.vaccineCentreID
               where vc.ID == Selected
               select new Appointment
               {
                   ID = ap.ID,
                   citizenAMKA = ap.citizenAMKA,
                   Date = ap.Date,
                   Time = new TimeSpan(ap.Time.Hours, ap.Time.Minutes, ap.Time.Seconds),
                   vaccineCentreID = ap.vaccineCentreID

               }


           ).ToList();


            
           
            
                
            
            multipleClass.citizen = citizen;
            if (citizen.Dose < 2)
            {
                multipleClass.appointmentList = resap;
                multipleClass.vaccineCentreList = vcList;
            }
            else
            {
                List<VaccineCentre> vclist2 = new List<VaccineCentre>();
                var aplist = _db.Appointments.Where(cit => cit.citizenAMKA == citizen.AMKA).ToList();
                
               
                vclist2.Add(_db.VaccineCentres.FirstOrDefault(vc => vc.ID == aplist[0].vaccineCentreID));
                vclist2.Add(_db.VaccineCentres.FirstOrDefault(vc => vc.ID == aplist[1].vaccineCentreID));
                
                
                multipleClass.appointmentList = aplist;
                multipleClass.vaccineCentreList = vclist2;
            }

            return View(multipleClass);
        }

        public IActionResult LogOut()
        {
            return RedirectToAction("CitizenLogIn");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.Any, NoStore = true)]
        public IActionResult EditCitizen()
        {
            if (HttpContext.Session.GetString("citizenAMKA") != null)
            {
                string tmp = HttpContext.Session.GetString("citizenAMKA");
                citizen = _db.Citizens.FirstOrDefault(citizen => citizen.AMKA.Equals(tmp));


                return View(citizen);
            }
            else
            {
                return RedirectToAction("CitizenLogIn");
            }
        }

        [HttpPost]
        public IActionResult EditCitizen(Citizen model)
        {
            if (ModelState.IsValid)
            {
                Citizen cit = _db.Citizens.FirstOrDefault(cit => cit.username == model.username && cit.AMKA != model.AMKA);
                if (cit != null)
                {
                    ModelState.AddModelError(string.Empty, "Το όνομα χρήστη υπάρχει ήδη");
                }
                else
                {
                    _db.Citizens.Update(model);
                    _db.SaveChanges();

                    return RedirectToAction("CitizenHome");
                }


            }
            return View(model);
        }


        public IActionResult DeleteAppointment()
        {
            string amka = HttpContext.Session.GetString("citizenAMKA");
            citizen = _db.Citizens.FirstOrDefault(cit => cit.AMKA.Equals(amka));

            List<Appointment> aplist = _db.Appointments.Where(cit => cit.citizenAMKA == citizen.AMKA).ToList();

            string app1 = aplist[0].Date.ToString("yyyy-MM-dd") + " "+ aplist[0].Time.ToString();
            string app2 = aplist[1].Date.ToString("yyyy-MM-dd") + " " + aplist[1].Time.ToString();
            DateTime dt1 = DateTime.ParseExact(app1, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            DateTime dt2 = DateTime.ParseExact(app2, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            if(dt1>DateTime.Now && dt2 > DateTime.Now)
            {
                _db.Appointments.RemoveRange(aplist);
                citizen.Dose = 0;

                _db.SaveChanges();
                HttpContext.Session.Remove("firstDose");
                HttpContext.Session.Remove("secDose");

            }
            else
            {
                HttpContext.Session.SetString("msg", "true");
            }
            return RedirectToAction("CitizenHome");
        }

    }
}
