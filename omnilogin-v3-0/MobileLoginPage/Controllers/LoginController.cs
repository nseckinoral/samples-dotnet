using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using XOMNI.SDK.Public;
using XOMNI.SDK.Public.Clients.OmniPlay;

namespace MobileLoginPage.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult>  Index(Models.User user)
        {
            var tenantUserName = System.Web.Configuration.WebConfigurationManager.AppSettings["TenantUsername"];
            var tenantPassword = System.Web.Configuration.WebConfigurationManager.AppSettings["TenantPassword"];
            var serviceUri = System.Web.Configuration.WebConfigurationManager.AppSettings["ServiceUri"];

            var clientcontext = new ClientContext(tenantUserName,tenantPassword,serviceUri);

            clientcontext.PIIUser = new XOMNI.SDK.Public.Models.PII.User()
            {
                Password = user.Password,
                UserName = user.Username
            };

            var deviceId = Request.QueryString["deviceId"];

            try
            {
              await clientcontext.Of<DeviceClient>().SubscribeToDevice(deviceId);
              return RedirectToAction("Success", "Login");
            }
            catch 
            {
             return View();
            }            
        }

        public ActionResult Success(Models.User user)
        { 
            return View();            
        }
    }
}
