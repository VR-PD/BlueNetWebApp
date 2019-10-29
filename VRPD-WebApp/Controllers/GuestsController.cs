using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using VRPD_WebApp.db;
using VRPD_WebApp.Utils;

namespace VRPD_WebApp.Controllers
{
    public class GuestsController : ApiController
    {
        private VrpdContext db = new VrpdContext();

        public IQueryable<Guest> Get() => db.Guest;

        public void Post([FromBody]byte[] raw)
        {
            object[] data = Serializer.FromByteArray<object[]>(raw);
            List<Guest> all = db.Guest.ToList();

            Guest guest = all.FirstOrDefault(g => g.Keynum.SequenceEqual(data[0] as byte[]));

            if (guest != null)
            {
                guest.IsConfirmed = true;
                db.SaveChanges();
            }
        }
    }
}
