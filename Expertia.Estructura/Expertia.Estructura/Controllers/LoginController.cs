using Expertia.Estructura.Controllers.Behavior;
using Expertia.Estructura.Models;
using Expertia.Estructura.Utils;
using Expertia.Estructura.Utils.Behavior;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Expertia.Estructura.Controllers
{
    [RoutePrefix(RoutePrefix.Login)]
    public class LoginController : ApiController
    {
        #region Properties
        protected ILogFileManager _logFileManager;
        protected IClientFeatures _clientFeatures;
        #endregion

        #region Constructor
        public LoginController()
        {
            _logFileManager = new LogFileManager(LogKeys.LogPath, LogKeys.LogName);
            _clientFeatures = new ClientFeatures();
        }
        #endregion

        [AllowAnonymous]
        [HttpPost]
        [Route(RouteAction.Auth)]
        public IHttpActionResult Auth(Credentials credentials)
        {
            string errorDescription = string.Empty;
            try
            {
                var userName = credentials.UserName;
                var password = credentials.Password;

                if (userName.ToLower().Equals("salesforce") && password.Equals("$4l3$f0rc3*"))
                {
                    var token = new Token()
                    {
                        Key = Guid.NewGuid(),
                        Expiry = DateTime.Now.AddMinutes(30) // Agrega media hora por default
                    };

                    return Ok(token);
                }
                else
                {
                    errorDescription = "Invalid Credentials";
                    _logFileManager.WriteLine(LogType.Warning, errorDescription);
                    return Content(HttpStatusCode.Unauthorized, errorDescription);
                };
            }
            catch (Exception ex)
            {
                errorDescription = ex.Message;
                _logFileManager.WriteLine(LogType.Fail, errorDescription);
                return BadRequest(errorDescription);
            }
        }
    }
}
