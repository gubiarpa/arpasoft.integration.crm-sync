using Expertia.Estructura.Controllers.Behavior;
using Expertia.Estructura.Utils;
using Expertia.Estructura.Utils.Behavior;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
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

        /// <summary>
        /// Método de Validación
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            Guid idOperation = Guid.NewGuid();

            object obj = new
            {
                IdOperation = idOperation,
                IpClient = _clientFeatures.IP,
                DateResponse = DateTime.Now.ToString(FormatTemplate.LongDate),
                Sender = "Expertia"
            };

            if (actionContext.Request.Headers.Authorization != null)
            {
                try
                {
                    var authToken = actionContext.Request.Headers.Authorization.Parameter;

                    if (!IsAuthorizedUser(authToken))
                    {
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, obj);
                        _logFileManager.WriteLine(LogType.Warning, string.Format("{0}: {1}", LogLineMessage.Unauthorized, idOperation.ToString()));
                    }                    
                }
                catch
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, obj);
                    _logFileManager.WriteLine(LogType.Fail, string.Format("{0}: {1}", LogLineMessage.BadRequest, idOperation.ToString()));
                }
                
            }
            else
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, obj);
                _logFileManager.WriteLine(LogType.Warning, string.Format("{0}: {1}", LogLineMessage.Unauthorized, idOperation.ToString()));
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