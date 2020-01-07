using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace BlueNetWebApp.Controllers
{
    /// <summary>
    /// Flag for pentest to find
    /// </summary>
    public class SecretController : ApiController
    {
        /// <summary>
        /// Flag for pentest hidden behind Authorizer
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult GetFlag() => new Flag("v=-50NdPawLVY", Request);

        private class Flag : IHttpActionResult
        {
            private string _message;
            private HttpRequestMessage _request;
            private HttpStatusCode _statusCode;

            public Flag(string message, HttpRequestMessage request)
            {
                _statusCode = (HttpStatusCode)418;
                _message = message;
                _request = request;
            }

            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                var response = new HttpResponseMessage()
                {
                    Content = new StringContent(_message),
                    RequestMessage = _request,
                    StatusCode = _statusCode
                };
                return Task.FromResult(response);
            }
        }
    }
}
