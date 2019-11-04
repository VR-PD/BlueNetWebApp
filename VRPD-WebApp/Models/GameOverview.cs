using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace VRPD_WebApp.Models
{
    public class GameOverview
    {
        public GameOverview()
        {
            ID = -1;
            Title = "Generic Title";
            Description = "A simple description of a some what long length to see what happens with longer sentences. ";
        }

        public string Description { get; set; }

        public int ID { get; set; }
        public byte[] Image { get; set; }
        public string Title { get; set; }
    }
}
