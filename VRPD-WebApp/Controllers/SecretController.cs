using System.Web.Mvc;

namespace BlueNetWebApp.Controllers
{
    /// <summary>
    /// Flag for pentest to find
    /// </summary>
    public class SecretController : Controller
    {
        private const string flag = "***********";

        /// <summary>
        /// Flag for pentest hidden behind Authorizer
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Flag()
        {
            return Content(flag);
        }
    }
}
