using System.IO;
using System.Web.Mvc;
using VRPD_WebApp.Models;

namespace VRPD_WebApp.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Overview()
        {
            return View(new GameOverview());
        }

        public FileContentResult RenderImage(int id)
        {
            if (id == -1)
            {
                System.Drawing.Image img = System.Drawing.Image.FromFile(@"C:\DEV\Repos\VRPD-WebApp\VRPD-WebApp\Image\image-placeholder.png");
                using (MemoryStream ms = new MemoryStream())
                {
                    img.Save(ms, img.RawFormat);
                    return File(ms.ToArray(), "image/png");
                }
            }
            else
            {
                return null;
            }
        }
    }
}
