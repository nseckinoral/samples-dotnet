using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MobileLoginPage.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Models.User user)
        {   
                       
            return RedirectToAction("Success", "Login");
        }

        public ActionResult Success(Models.User user)
        { 
            return View();
            
        }
    }
}
