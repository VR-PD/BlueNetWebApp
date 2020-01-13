using BlueNetWebApp.db;
using System;
using System.Web.Http;

namespace BlueNetWebApp.Controllers
{
    public class RegistrarController : ApiController
    {
        private readonly Entities db = new Entities();

        [HttpPost]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<Pending>")]
        public IHttpActionResult RegisterAgent(Registration registration)
        {
            try
            {
                db.Registration.Add(new db.Registration()
                {
                    deviceID = registration.DID,
                    userName = registration.UserName
                });
                db.SaveChanges();
            }
            catch (Exception)
            {
                return NotFound();
            }
            return Ok();
        }
    }

    /// <summary>
    /// Model for recieving post data containing Device ID
    /// </summary>
    public class Registration
    {
        /// <summary>
        /// Device ID
        /// </summary>
        public string DID { get; set; }

        /// <summary>
        /// Chosen nickname/ username
        /// </summary>
        public string UserName { get; set; }
    }
}
