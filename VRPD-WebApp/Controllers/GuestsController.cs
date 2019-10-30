using System;
using System.Collections.Generic;
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
            IEnumerable<byte> b = raw.AsEnumerable();
            List<Guest> all = db.Guest.ToList();

            Guest guest = all.FirstOrDefault(g => g.Keynum.SequenceEqual(b));

            if (guest != null)
            {
                guest.IsConfirmed = true;
                db.SaveChanges();
            }
        }
    }
}
