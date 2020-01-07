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
        #region Properties
        private Dictionary<string, OracleParameter> _parameters;
        private Dictionary<string, OracleParameter> _outParameters;
        private Dictionary<string, DataTable> _dtParameters;
        protected string _connectionString { get; }
        protected readonly UnidadNegocioKeys? _unidadNegocio;
        #endregion

        public OracleBase(string connKey, UnidadNegocioKeys? unidadNegocio = null)
        {
            _parameters = new Dictionary<string, OracleParameter>();
            _outParameters = new Dictionary<string, OracleParameter>();
            _dtParameters = new Dictionary<string, DataTable>();
            _connectionString = ConfigAccess.GetValueInConnectionString(connKey);
            _unidadNegocio = unidadNegocio;
        }

        protected void AddParameter(string parameterName, OracleDbType type, object value = null, ParameterDirection parameterDirection = ParameterDirection.Input, int size = 0)
        {
            _outParameters.Clear();
            var parameter = new OracleParameter(parameterName, type, value.Coalesce(), parameterDirection);
            if (size > 0) parameter.Size = size;
            _parameters.Add(parameterName, parameter);
        }

        protected void ExecuteStoredProcedure(string SPName)
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
                                    try
                                    {
                                        (_dtParameters[key] = new DataTable()).Load(((OracleRefCursor)(_parameters[key]).Value).GetDataReader());
                                    }
                                    catch
                                    {
                                        _dtParameters[key] = null;
                                    }
                                }
                                else
                                {
                                    _outParameters[key] = _parameters[key];
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
                return _outParameters[parameterName].Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected DataTable GetDtParameter(string parameterName)
        {
            try
            {
                return _dtParameters[parameterName];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}