using System;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http;
using VRPD_WebApp.db;

namespace VRPD_WebApp.Controllers
{
    public class GuestsController : ApiController
    {
        private VrpdContext db = new VrpdContext();

        public IQueryable<Guest> Get() => db.Guest;

        public void Post([FromBody]byte[] raw)
        {
            string s = Convert.ToBase64String(raw);
            Guest guest = db.Guest.First(g => g.Keynum == s);
            if (guest != null)
            {
                guest.IsConfirmed = true;
                db.SaveChanges();
            }
        }
    }
}
