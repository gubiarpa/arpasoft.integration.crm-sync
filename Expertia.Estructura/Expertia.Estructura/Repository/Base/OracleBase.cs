using Expertia.Estructura.Utils;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;

namespace Expertia.Estructura.Repository.Base
{
    public abstract class OracleBase : OracleBase<object>
    {
        public OracleBase(string connKey, UnidadNegocioKeys? unidadNegocio = null) : base(connKey, unidadNegocio)
        {
        }
    }

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

        protected void ExecuteStoredProcedure(string SPName, bool withCommit = false)
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

                        OracleTransaction trx = null;
                        if (withCommit) trx = conn.BeginTransaction();

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

                        if (withCommit) trx.Commit();
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

        protected void ExecuteConexionBegin(string CnxName, ref OracleTransaction _oracleTransaction, ref OracleConnection _oracleConnection)
        {
            try
            {
                OracleConnection conn = new OracleConnection();
                conn.ConnectionString = ConfigAccess.GetValueInConnectionString(CnxName);
                conn.Open();

                _oracleConnection = conn;
                _oracleTransaction = conn.BeginTransaction();                                                    
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void ExecuteStorePBeginCommit(string SPName, OracleTransaction TransOrx, OracleConnection _oracleConnection, bool withCommit = false)
        {
            try
            {                
                {
                    using (OracleCommand cmd = new OracleCommand()
                    {
                        CommandText = SPName,
                        CommandType = CommandType.StoredProcedure,
                        Connection = (TransOrx == null ? _oracleConnection : TransOrx.Connection)
                    })
                    {                       
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

                        if (withCommit) TransOrx.Commit();
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