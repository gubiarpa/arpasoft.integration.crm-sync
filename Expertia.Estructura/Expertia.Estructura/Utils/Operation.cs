using System;
using System.Collections.Generic;

namespace Expertia.Estructura.Utils
{
    public class Operation : IDisposable
    {
        public const string Result = "OperationResult";
        public const string ErrorMessage = "ErrorMessage";

        private Dictionary<string, object> _values = new Dictionary<string, object>();

        public object this[string key]
        {
            get
            {
                return _values[key];
            }
            set
            {
                _values.Add(key, value);
            }
        }

        public void Dispose()
        {
            _values = null;
        }
    }
}