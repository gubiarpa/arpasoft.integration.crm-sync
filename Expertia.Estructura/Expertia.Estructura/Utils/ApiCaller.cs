using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Expertia.Estructura.Utils
{
    public class ApiRequest
    {
        public ApiRequest(string server, string method, bool isAuth = false, string token = "")
        {
            Server = server;
            Method = method;
            IsAuth = isAuth;
            Token = token;
        }

        public bool IsAuth { get; set; }
        public string Server { get; set; }
        public string Method { get; set; }
        public string Token { get; set; }
    }

    public class ApiCaller
    {
        #region Properties
        private ApiRequest _apiRequest;
        private Guid _codeOperation;
        #endregion

        #region Constructor
        public ApiCaller(ApiRequest apiRequest, Guid codeOperation)
        {
            _apiRequest = apiRequest;
            _codeOperation = codeOperation;
        }
        #endregion

        #region Methods
        public HttpStatusCode Execute()
        {
            try
            {
                var client = new RestClient(_apiRequest.Server);
                var request = new RestRequest(_apiRequest.Method, Method.POST, DataFormat.Json);
                if (_apiRequest.IsAuth) request.AddHeader("Authorization", "Bearer " + _apiRequest.Token);
                request.AddJsonBody(new { CodeOperation = _codeOperation.ToString() });
                return client.Execute(request).StatusCode;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}