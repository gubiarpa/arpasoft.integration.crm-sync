using Expertia.Estructura.Controllers.Behavior;
using Expertia.Estructura.Models;
using Expertia.Estructura.Utils;
using Expertia.Estructura.Utils.Behavior;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
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
        public IHttpActionResult Auth(LoginRequest credentials)
        {
            string errorDescription = string.Empty;
            try
            {
                var username = credentials.UserName;
                var password = credentials.Password;

                if (IsValidCredentials(username, password))
                {
                    return Ok(GenerateToken());
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

        private bool IsValidCredentials(string userName, string password)
        {
            /// Comparación con base de datos
            return (userName.ToLower().Equals("salesforce") && password.Equals("$4l3$f0rc3*"));
        }

        private LoginResponse GenerateToken()
        {
            var expirationInMinutes = ConfigAccess.GetValueInAppSettings(SecurityKeys.ExpirationInMin);
            var now = DateTime.Now; var limitTime = now.AddMinutes(Convert.ToInt32(expirationInMinutes));
            
            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();
            string tokenGenerated = Convert.ToBase64String(time.Concat(key).ToArray());

            #region TESTING
            tokenGenerated = "ECPG35cP10iLFqUONN9IRK2VLweBriPx"; // ¡SÓLO PRUEBAS!
            #endregion

            var token = new LoginResponse()
            {
                Token = tokenGenerated,
                ExpirationInMinutes = limitTime.ToString(FormatTemplate.LongDate) // Agrega media hora por default
            };

            return token;
        }
    }
}
