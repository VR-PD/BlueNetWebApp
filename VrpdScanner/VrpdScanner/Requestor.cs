using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using RestSharp;

namespace VrpdScanner
{
    public static class Requestor
    {
        private const string url = "https://vrpd-webapp.azurewebsites.net/api/Guests";

        public static IRestResponse Send(string key)
        {
            RestClient client = new RestClient(url);
            IRestResponse res = client.Execute(new RestRequest(Method.POST).AddJsonBody(key));
            return res;
        }
    }
}
