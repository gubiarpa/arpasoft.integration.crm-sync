using Expertia.Estructura.Repository.MDM;
using Expertia.Estructura.Utils;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Expertia.Estructura.Repository.Base
{
    public abstract class OracleBase<T>
    {
        protected string _connectionString { get; }

        public OracleBase(string connKey)
        {
            _connectionString = ConfigAccess.GetValueInConnectionString(connKey);
        }

        private Dictionary<string, object> _inParameters = new Dictionary<string, object>();
        private Dictionary<string, object> _outParameters = new Dictionary<string, object>();
        private Dictionary<string, object> _outResultParameters = new Dictionary<string, object>();

        protected void AddParameter(string name, object value = null, ParameterDirection parameterDirection = ParameterDirection.Input)
        {
            switch (parameterDirection)
            {
                case ParameterDirection.Input:
                    _inParameters.Add(name, value);
                    break;
                case ParameterDirection.Output:
                    _outParameters.Add(name, value);
                    break;
            }
        }

        protected object GetOutParameter(string name)
        {
            return _outResultParameters[name];
        }

        protected IEnumerable<T> ExecuteSPWithResults(string SPName)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection())
                {
                    conn.Open();
                    using (OracleCommand cmd = new OracleCommand()
                    {
                        CommandText = SPName,
                        CommandType = CommandType.StoredProcedure,
                        Connection = conn
                    })
                    {
                        foreach (var key in _inParameters.Keys)
                        {
                            cmd.Parameters.Add(new OracleParameter(key, _inParameters[key]) { Direction = ParameterDirection.Input });
                        }
                        cmd.ExecuteNonQuery();
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected abstract IEnumerable<T> DataTableToEnumerable(DataTable dt);

        protected void ExecuteSPWithoutResults(string SPName)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection()
                {
                    ConnectionString = _connectionString
                })
                {
                    conn.Open();
                    using (OracleCommand cmd = new OracleCommand()
                    {
                        CommandText = SPName,
                        CommandType = CommandType.StoredProcedure,
                        Connection = conn
                    })
                    {
                        foreach (var key in _inParameters.Keys)
                        {
                            cmd.Parameters.Add(new OracleParameter(key, _inParameters[key]) { Direction = ParameterDirection.Input });
                        }
                        foreach (var key in _outParameters.Keys)
                        {
                            cmd.Parameters.Add(new OracleParameter(key, _outParameters[key]) { Direction = ParameterDirection.Output});
                        }
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _inParameters.Clear();
                _outParameters.Clear();
            }
        }
    }
}