using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VRPDWebApp.db;

namespace VRPDWebApp.Controllers
{
    public class RegistrarController : ApiController
    {
        private readonly Entities db = new Entities();

        [HttpPost]
        public IHttpActionResult RegisterAgent(Registration registration)
        {
            db.Registration.Add(new VRPDWebApp.db.Registration()
            {
                deviceID = registration.DID,
                userName = registration.UserName
            });
            db.SaveChanges();
            return Ok();
        }

        /// <summary>
        /// Model for recieving post data containing Device ID
        /// </summary>
        public struct Registration
        {
            /// <summary>
            /// Device ID
            /// </summary>
            public string DID;

            /// <summary>
            /// Chosen nickname/ username
            /// </summary>
            public string UserName;
        }
    }
}
