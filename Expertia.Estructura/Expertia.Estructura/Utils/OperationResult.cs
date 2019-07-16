using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Utils
{
    public class OperationResult
    {
        private Dictionary<string, object> values = new Dictionary<string, object>();

        public new object this[string key]
        {
            get
            {
                return this.values[key];
            }
            set
            {
                this.values.Add(key, value);
            }
        }
    }
}