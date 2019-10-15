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

        public static IRestResponse Send(byte[] key)
        {
            var client = new RestClient(url);
            var res = client.Execute(new RestRequest(Method.POST).AddJsonBody(key));
            return res;
        }
    }
}
