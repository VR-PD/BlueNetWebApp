using System;
using System.Collections.Generic;

namespace VRPD_WebApp.Models
{
    public class Visitor
    {
        public static List<Visitor> Visitors = new List<Visitor>();

        public Visitor(string key)
        {
            Key = key;
            Time = DateTime.Now;
            IsConfirmed = false;
        }

        public bool IsConfirmed { get; set; }
        public string Key { get; }
        public DateTime Time { get; }
    }
}
