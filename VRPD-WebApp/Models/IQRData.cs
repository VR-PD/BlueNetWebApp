using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VRPD_WebApp.Models
{
    public interface IQRData
    {
        DateTime Created { get; set; }

        byte[] Key { get; set; }

        string UserID { get; set; }
    }
}
