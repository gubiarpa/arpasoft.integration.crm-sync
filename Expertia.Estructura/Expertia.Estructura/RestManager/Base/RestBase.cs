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
                if (!int.TryParse(ConfigAccess.GetValueInAppSettings(SecurityKeys.AuthTimeoutKey), out int authTimeout)) authTimeout = 0;
                var client = new RestClient(serverName) { Timeout = authTimeout };
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.DefaultConnectionLimit = 9999;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                var request = new RestRequest(methodName, methodType);
                var sf_authgranttype = ConfigAccess.GetValueInAppSettings("AUTH_GRANT_TYPE");
                var sf_authclientid = ConfigAccess.GetValueInAppSettings("AUTH_CLIENT_ID");
                var sf_auth_clientsecret = ConfigAccess.GetValueInAppSettings("AUTH_CLIENT_SECRET");
                var sf_authusername = ConfigAccess.GetValueInAppSettings("AUTH_USERNAME");
                var sf_authpassword = ConfigAccess.GetValueInAppSettings("AUTH_PASSWORD");

                request.AddParameter("grant_type", sf_authgranttype);
                request.AddParameter("client_id", sf_authclientid);
                request.AddParameter("client_secret", sf_auth_clientsecret);
                request.AddParameter("username", sf_authusername);
                request.AddParameter("password", sf_authpassword);
                var response = client.Execute(request);
                if (response.StatusCode != HttpStatusCode.OK && response.ErrorMessage != null) throw new Exception(response.ErrorMessage);
                var content = response.Content;
                JsonManager.LoadText(content);
                return JsonManager.GetSetting("access_token");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetTokenByKey(string serverNameKey, string methodNameKey, Method methodType = Method.POST)
        {
            return GetToken(ConfigAccess.GetValueInAppSettings(serverNameKey), ConfigAccess.GetValueInAppSettings(methodNameKey), methodType);
        }

        public static IRestResponse Execute(string serverName, string methodName, Method methodType = Method.POST, object body = null, bool isAuth = false, string token = "", int timeout = -1)
        {
            try
            {
                var client = new RestClient(serverName) /*{ Timeout = timeout }*/;
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

        public static IRestResponse ExecuteByKey(string serverNameKey, string methodNameKey, Method methodType = Method.POST, object body = null, bool isAuth = false, string token = "")
        {
            return Execute(
                ConfigAccess.GetValueInAppSettings(serverNameKey),
                ConfigAccess.GetValueInAppSettings(methodNameKey),
                methodType,
                body,
                isAuth,
                token,
                int.Parse(ConfigAccess.GetValueInAppSettings(SecurityKeys.CrmTimeoutKey)));
        }
        #endregion
    }
}