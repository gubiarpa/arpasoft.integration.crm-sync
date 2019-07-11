using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expertia.Estructura.Utils.Behavior
{
    public interface IFileIO
    {
        string FullName { get; }
        string LogFormat { get; set; }
        void WriteContent(string content);
        string ReadContent();
    }
}
