using System;
using System.Collections.Generic;

namespace VRPD_WebApp.Models
{
    public class Visitor
    {
        public static List<Visitor> Visitors = new List<Visitor>();

        public Visitor(string iD)
        {
            ID = iD;
            Time = DateTime.Now;
            IsConfirmed = false;
        }

        public string ID { get; }
        public bool IsConfirmed { get; set; }

        public DateTime Time { get; }
    }
}
