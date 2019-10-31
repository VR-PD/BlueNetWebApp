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

            object[] data = Serializer.FromByteArray<object[]>(raw);

            // Validate post data
            if (!ValidPostData(data))
                return;

            byte[] key = data[QRData.Keynum] as byte[];

            Guest guest = all.FirstOrDefault(g => g.Keynum.SequenceEqual(key));

            if (guest != null)
            {
                guest.IsConfirmed = true;
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Checks if the object array is valid qr data received from a client
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool ValidPostData(object[] data)
        {
            try
            {
                if (!(data[QRData.Keynum] is byte[] && data[QRData.Created] is DateTime && data[QRData.UserID] is string))
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
