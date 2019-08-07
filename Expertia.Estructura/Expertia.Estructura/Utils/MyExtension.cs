using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Utils
{
    public static class MyExtension
    {
        public static object Coalesce(this object obj, object nullValue = null)
        {
            try
            {
                return obj == null ? (nullValue == null ? DBNull.Value : nullValue) : obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static object IfNotNull(this object obj, object notNullValue)
        {
            try
            {
                return obj == null ? DBNull.Value : notNullValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}