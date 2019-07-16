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
                    string decodeAuthToken = Encoding.UTF8.GetString(Convert.FromBase64String(authToken));
                                    
                    if (!IsAuthorizedUser(decodeAuthToken))
                    {
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, obj);
                        _logFileManager.WriteLine(LogType.Warning, string.Format("Unauthorized: {0}", idOperation.ToString()));
                    }                    
                }
                catch
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, obj);
                    _logFileManager.WriteLine(LogType.Fail, string.Format("Bad Request:{0}", idOperation.ToString()));
                }
                
            }
            else
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, obj);
                _logFileManager.WriteLine(LogType.Warning, string.Format("Unauthorized: {0}", idOperation.ToString()));
            }
        }

        private static bool IsAuthorizedUser(string decodeAuthToken)
        {
            var token = ConfigAccess.GetValueInAppSettings(SecurityKeys.Token);
            // In this method we can handle our database logic here...  
            return (decodeAuthToken.Equals(token));
        }
    }
}