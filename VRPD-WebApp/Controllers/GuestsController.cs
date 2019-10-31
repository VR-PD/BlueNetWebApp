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

        public void Post(byte[] raw)
        {
            List<Guest> all = db.Guest.ToList();

            object[] data = Serializer.FromByteArray<object[]>(raw);

            // Validate post data
            if (!ValidPostData(data))
                return;

            byte[] key = data[0] as byte[];

            Guest guest = all.FirstOrDefault(g => g.Keynum.SequenceEqual(key));

            if (guest != null)
            {
                guest.IsConfirmed = true;
                db.SaveChanges();
            }
        }

        private bool ValidPostData(object[] data)
        {
            try
            {
                if (!(data[0] is byte[] && data[1] is DateTime && data[2] is string))
                    return false;
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
            return true;
        }
    }
}
