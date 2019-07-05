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
                var authToken = actionContext.Request.Headers
                    .Authorization.Parameter;

                // decoding authToken we get decode value in 'Username:Password' format  
                var decodeAuthToken = System.Text.Encoding.UTF8.GetString(
                    Convert.FromBase64String(authToken));                

                // at 0th postion of array we get username and at 1st we get password  
                if (IsAuthorizedUser(decodeAuthToken))
                {
                    // setting current principle  
                    //Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(decodeAuthToken), null);
                }
                else
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
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