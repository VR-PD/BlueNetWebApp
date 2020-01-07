using BlueNetWebApp.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace BlueNetWebApp.Controllers
{
    public class GuestsController : ApiController
    {
        private readonly Entities db = new Entities();

        public IQueryable<Guest> Get() => db.Guest;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>")]
        public IHttpActionResult Post(byte[] raw)
        {
            try
            {
                QRModel data = QRModel.FromArray(Serializer.FromByteArray<object[]>(raw));

                if (!IsAuthorizedDevice(data))
                    return Unauthorized();

                List<Guest> all = db.Guest.ToList();
                Guest guest = all.FirstOrDefault(g => g.Keynum.SequenceEqual(data.GetKeynum()));

                if (guest != null)
                {
                    guest.IsConfirmed = true;
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
                return NotFound();
            }
            return Ok();
        }

        private bool IsAuthorizedDevice(QRModel data)
        {
            if (data.UserID == null || data.UserID.Length == 0)
                return false;

            return db.Registration.Any(r => r.deviceID == data.UserID);
        }
    }
}
