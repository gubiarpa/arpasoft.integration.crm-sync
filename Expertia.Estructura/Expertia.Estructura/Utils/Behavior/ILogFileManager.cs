using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expertia.Estructura.Utils.Behavior
{
    public interface ILogFileManager
    {
        void WriteLine(LogType logType, string content);
    }
}
