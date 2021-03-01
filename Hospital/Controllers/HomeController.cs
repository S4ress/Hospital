using Hospital.Context;
using Hospital.Interfaces;
using Hospital.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeRepository _repository;


        public HomeController(ILogger<HomeController> logger, IHomeRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public IActionResult Index()
        {
            ViewData["CurPage"] = "0";
            return View();
        }

        public IActionResult Privacy()
        {
            ViewData["CurPage"] = "2";
            return View();
        }

        public IActionResult Doctors(string doctorName)
        {
            return Doctors(new DoctorFilter { currentPage = 1, doctorName = doctorName });
        }

        public IActionResult AllDoctors()
        {
            ViewData["DoctorFilter"] = null;
            return RedirectToAction("Doctors");
        }

        [HttpGet("doctors/{currentPage}/{doctorName}")]
        public IActionResult Doctors(DoctorFilter filter)
        {
            ViewBag.CurPage = filter.currentPage.ToString();
            if (filter.currentPage != 1)
            {
                ViewBag.NextPage = (filter.currentPage + 1).ToString();
                ViewBag.PreviousPage = (filter.currentPage - 1).ToString();
            }
            else
            {
                ViewBag.NextPage = 2;
            }
            
            IEnumerable<Doctors> doctors = null;
            if (filter.doctorName != null)
            {
                //filter.currentPage = 1;
                ViewData["DoctorFilter"] = filter.doctorName;
                doctors = _repository.GetDoctorByName(filter.doctorName);
            }
            else
            {
                doctors = _repository.GetAllDoctors();
            }

            if(doctors.Count() % 6 != 0)
                ViewBag.LastPage = ((doctors.Count() / 6)+1).ToString();
            else
                ViewBag.LastPage = (doctors.Count() / 6).ToString();
            var x = doctors.Skip((filter.currentPage - 1) * 6).Take(6).ToList();
            return View(x);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
