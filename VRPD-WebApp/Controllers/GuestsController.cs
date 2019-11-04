using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using VRPD_WebApp.db;
using VRPD_WebApp.Models;
using VRPD_WebApp.Utils;

namespace VRPD_WebApp.Controllers
{
    public class GuestsController : ApiController
    {
        private readonly VrpdContext db = new VrpdContext();

        public IQueryable<Guest> Get() => db.Guest;

        public void Post(byte[] raw)
        {
            List<Guest> all = db.Guest.ToList();

            QRModel data = QRModel.FromArray(Serializer.FromByteArray<object[]>(raw));

            Guest guest = all.FirstOrDefault(g => g.Keynum.SequenceEqual(data.Keynum));

            if (guest != null)
            {
                guest.IsConfirmed = true;
                db.SaveChanges();
            }
        }
    }
}
