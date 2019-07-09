using Expertia.Estructura.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Expertia.Estructura.Filters
{
    public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
    {
        /// <summary>
        /// Método de Validación
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization != null)
            {
                try
                {
                    var authToken = actionContext.Request.Headers.Authorization.Parameter;
                    string decodeAuthToken = Encoding.UTF8.GetString(Convert.FromBase64String(authToken));
                                    
                    if (!IsAuthorizedUser(decodeAuthToken))
                    {
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                    }                    
                }
                catch
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest);
                }
                
            }
            else
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
        }

        private static bool IsAuthorizedUser(string decodeAuthToken)
        {
            var token = ConfigAccess.GetValueInAppSettings("MainToken");
            // In this method we can handle our database logic here...  
            return (decodeAuthToken.Equals(token));
        }
    }
}