using Expertia.Estructura.Utils;
using RestSharp;
using System;
using System.Net;

namespace Expertia.Estructura.RestManager.Base
{
    public static class RestBase
    {
        #region Methods
        public static string GetToken(string serverName, string methodName, Method methodType = Method.POST)
        {
            try
            {
                var client = new RestClient(serverName);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.DefaultConnectionLimit = 9999;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                var request = new RestRequest(methodName, methodType);
                request.AddParameter("grant_type", ConfigAccess.GetValueInAppSettings("AUTH_GRANT_TYPE"));
                request.AddParameter("client_id", ConfigAccess.GetValueInAppSettings("AUTH_CLIENT_ID"));
                request.AddParameter("client_secret", ConfigAccess.GetValueInAppSettings("AUTH_CLIENT_SECRET"));
                request.AddParameter("username", ConfigAccess.GetValueInAppSettings("AUTH_USERNAME"));
                request.AddParameter("password", ConfigAccess.GetValueInAppSettings("AUTH_PASSWORD"));
                var response = client.Execute(request);
                var content = response.Content;
                JsonManager.LoadText(content);
                return JsonManager.GetSetting("access_token");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static IRestResponse Execute(string serverName, string methodName, Method methodType = Method.POST, object body = null, bool isAuth = false, string token = "")
        {
            try
            {
                var client = new RestClient(serverName);
                var request = new RestRequest(methodName, Method.POST, DataFormat.Json);
                if (isAuth) request.AddHeader("Authorization", "Bearer " + token);
                request.AddJsonBody(body);
                return client.Execute(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}