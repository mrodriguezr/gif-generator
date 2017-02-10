using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GiftMaker.Models;

namespace GiftMaker.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public PartialViewResult GiftMaker(GiftViewModel data)
        {
            var result = new GiftModel(data.Message);
            result.GenerateGif();
            return PartialView(result);
        }
        
    }
}