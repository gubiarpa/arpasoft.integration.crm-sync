using Expertia.Estructura.Utils;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;

namespace Expertia.Estructura.Repository.Base
{
    public abstract class OracleBase<T>
    {
        private Dictionary<string, OracleParameter> _parameters;
        private Dictionary<string, OracleParameter> _resultParameters;
        protected string _connectionString { get; }
        protected readonly UnidadNegocioKeys? _unidadNegocio;

        public OracleBase(string connKey, UnidadNegocioKeys? unidadNegocio = null)
        {
            _parameters = new Dictionary<string, OracleParameter>();
            _resultParameters = new Dictionary<string, OracleParameter>();
            _connectionString = ConfigAccess.GetValueInConnectionString(connKey);
            _unidadNegocio = unidadNegocio;
        }

        protected void AddParameter(string parameterName, OracleDbType type, object value = null, ParameterDirection parameterDirection = ParameterDirection.Input, int size = 0)
        {
            _resultParameters.Clear();
            var parameter = new OracleParameter(parameterName, type, value.Coalesce(), parameterDirection);
            if (size > 0) parameter.Size = size;
            _parameters.Add(parameterName, parameter);
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

                        // Volcamos en parámetros resultantes
                        foreach (var key in _parameters.Keys)
                        {
                            if (_parameters[key].Direction.Equals(ParameterDirection.Output))
                            {
                                if (_parameters[key].OracleDbType.Equals(OracleDbType.RefCursor))
                                {
                                    var output = _parameters[key];
                                }
                                else
                                {
                                    _resultParameters[key] = _parameters[key];
                                }
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void ExecuteSPWithoutResults(string SPName)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection()
                {
                    ConnectionString = _connectionString
                })
                {
                    using (OracleCommand cmd = new OracleCommand()
                    {
                        CommandText = SPName,
                        CommandType = CommandType.StoredProcedure,
                        Connection = conn
                    })
                    {
                        conn.Open();

                        // Agregamos parámetros
                        foreach (var key in _parameters.Keys)
                        {
                            cmd.Parameters.Add(_parameters[key]);
                        }

                        // Ejecutamos el SP
                        cmd.ExecuteNonQuery();

                        // Volcamos en parámetros resultantes
                        foreach (var key in _parameters.Keys)
                        {
                            if (_parameters[key].Direction.Equals(ParameterDirection.Output))
                            {
                                if (_parameters[key].OracleDbType.Equals(OracleDbType.RefCursor))
                                {
                                    var output = _parameters[key];
                                    OracleDataReader reader = ((OracleRefCursor)output.Value).GetDataReader();
                                    while (reader.Read())
                                    {
                                        var _ = reader.GetInt32(0);
                                    }
                                }
                                else
                                {
                                    _resultParameters[key] = _parameters[key];
                                }
                            }
                        }
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

        protected object GetOutParameter(string parameterName)
        {
            try
            {
                return _resultParameters[parameterName].Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected OracleParameter GetCursor(string parameterName)
        {
            try
            {
                return ((OracleParameter)_resultParameters[parameterName]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}