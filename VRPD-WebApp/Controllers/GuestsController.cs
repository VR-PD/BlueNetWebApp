using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using VRPDWebApp.db;
using VRPDWebApp.Models;
using VRPDWebApp.Utils;

namespace VRPDWebApp.Controllers
{
    public class GuestsController : ApiController
    {
        private readonly Entities db = new Entities();

        public IQueryable<Guest> Get() => db.Guest;

        public void Post(byte[] raw)
        {
            List<Guest> all = db.Guest.ToList();

            QRModel data = QRModel.FromArray(Serializer.FromByteArray<object[]>(raw));

            Guest guest = all.FirstOrDefault(g => g.Keynum.SequenceEqual(data.GetKeynum()));

            if (guest != null)
            {
                guest.IsConfirmed = true;
                db.SaveChanges();
            }
        }
    }
}
