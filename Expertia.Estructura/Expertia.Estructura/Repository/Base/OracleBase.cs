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
        private Dictionary<string, OracleParameter> _parameters;
        protected string _connectionString { get; }

        public OracleBase(string connKey)
        {
            _parameters = new Dictionary<string, OracleParameter>();
            _connectionString = ConfigAccess.GetValueInConnectionString(connKey);
        }        

        protected void AddParameter(string parameterName, OracleDbType type, object value = null, ParameterDirection parameterDirection = ParameterDirection.Input)
        {
            _parameters.Add(parameterName, new OracleParameter(parameterName, type, value, parameterDirection));
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
                        foreach (var key in _parameters.Keys)
                        {
                            cmd.Parameters.Add(_parameters[key]);
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
                        foreach (var key in _parameters.Keys)
                        {
                            cmd.Parameters.Add(new OracleParameter(key, _parameters[key].OracleDbType, _parameters[key].Value, _parameters[key].Direction) { });
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
                _parameters.Clear();
            }
        }
    }
}