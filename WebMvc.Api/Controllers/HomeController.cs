using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMvc.Model;

namespace WebMvc.Api.Controllers
{
    public class HomeController : Controller
    {
        private readonly IIndustry _industry;
        public HomeController(IIndustry industry)
        {
            _industry = industry;
        }
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            _industry.Error();
            return View();
        }
    }
}
