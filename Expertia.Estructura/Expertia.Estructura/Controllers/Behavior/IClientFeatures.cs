using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expertia.Estructura.Controllers.Behavior
{
    public interface IClientFeatures
    {
        string IP { get; }
        string Method { get; }
        string URL { get; }
    }
}
