using Expertia.Estructura.Controllers.Behavior;
using Expertia.Estructura.Utils;
using Expertia.Estructura.Utils.Behavior;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Expertia.Estructura.Filters
{
    public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
    {
        #region Log
        protected ILogFileManager _logFileManager;
        private IClientFeatures _clientFeatures;
        #endregion

        #region Constructor
        public BasicAuthenticationAttribute()
        {
            _logFileManager = new LogFileManager(LogKeys.LogPath, LogKeys.LogName);
            _clientFeatures = new ClientFeatures();
        }
        #endregion

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization != null)
            {
                try
                {
                    var authToken = actionContext.Request.Headers.Authorization.Parameter;

                    if (!IsAuthorizedUser(authToken))
                    {
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                        LogLineMessage.Unauthorized.WriteLogObject(_logFileManager, _clientFeatures, LogType.Warning);
                    }
                }
                catch (Exception ex)
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, ex);
                    LogLineMessage.BadRequest.WriteLogObject(_logFileManager, _clientFeatures, LogType.Fail);
                }
            }
            else
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                LogLineMessage.Unauthorized.WriteLogObject(_logFileManager, _clientFeatures, LogType.Warning);
            }
        }

        private static bool IsAuthorizedUser(string authToken)
        {
            var token = ConfigAccess.GetValueInAppSettings(SecurityKeys.Token);
            // In this method we can handle our database logic here...  
            return (authToken.Equals(token));
        }
    }
}