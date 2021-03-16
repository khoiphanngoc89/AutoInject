using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApi.Mvc.Models;

namespace WebApi.Mvc.Controllers
{
    public class HomeController : Controller
    {
        IIndustry _service;
        public HomeController(IIndustry service)
        {
            _service = service;
            _service.Show();
        }
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
