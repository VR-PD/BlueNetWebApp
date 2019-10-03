using System;

namespace VRPD_WebApp.Models
{
    public class Visitor : IEquatable<Visitor>
    {
        public Visitor(string iD)
        {
            ID = iD;
            Time = DateTime.Now;
            IsConfirmed = false;
        }

        public string ID { get; }
        public bool IsConfirmed { get; set; }

        public DateTime Time { get; }

        public bool Equals(Visitor other) => ID == other.ID;
    }
}
