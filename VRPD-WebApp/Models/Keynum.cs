using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VRPD_WebApp.Models
{
    public class Keynum
    {
        public DateTime Created;

        public Keynum(byte[] key, DateTime created)
        {
            Key = key;
            Created = created;
        }

        public byte[] Key { get; set; }
        public new string ToString => Convert.ToBase64String(Key);
    }
}
