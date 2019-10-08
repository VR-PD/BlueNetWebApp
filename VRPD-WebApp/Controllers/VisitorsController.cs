using System.Collections.Generic;
using System.Web.Http;
using VRPD_WebApp.Models;

namespace VRPD_WebApp.Controllers
{
    public class VisitorsController : ApiController
    {
        public static void AddVisitor(Visitor visitor)
        {
            if (!Visitor.Visitors.Contains(visitor))
                Visitor.Visitors.Add(visitor);
        }

        public IEnumerable<Visitor> Get() => Visitor.Visitors;

        public void Post([FromBody]string id)
        {
            int i = Visitor.Visitors.FindIndex(v => v.ID == id);
            if (i > -1)
                Visitor.Visitors[i].IsConfirmed = true;
        }
    }
}
