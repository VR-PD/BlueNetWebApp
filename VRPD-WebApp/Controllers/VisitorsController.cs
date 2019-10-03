using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VRPD_WebApp.Models;

namespace VRPD_WebApp.Controllers
{
    public class VisitorsController : ApiController
    {
        private static readonly List<Visitor> visitors = new List<Visitor>();

        public static void AddVisitor(Visitor visitor)
        {
            if (!visitors.Contains(visitor))
                visitors.Add(visitor);
        }

        public IEnumerable<Visitor> Get() => visitors;

        public void Post([FromBody]string id)
        {
            int i = visitors.FindIndex(v => v.ID == id);
            if (i > -1)
                visitors[i].IsConfirmed = true;
        }
    }
}
