using Expertia.Estructura.Controllers.Behavior;
using System.Web;

namespace Expertia.Estructura.Utils
{
    public class ClientFeatures : IClientFeatures
    {
        public string IP => HttpContext.Current.Request.UserHostAddress;
        public string Method => HttpContext.Current.Request.HttpMethod;
        public string Uri => HttpContext.Current.Request.Url.LocalPath;
    }
}