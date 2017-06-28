using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Linq;

namespace Euroland.NetCore.ToolsFramework.Data
{
    public class DapperDatabaseContext : IDatabaseContext
    {
        private string _connectionString;

        /// <summary>
        /// Instantiate a <see cref="DapperDatabaseContext"/> object
        /// </summary>
        /// <param name="connectionString">The connection string to the database of the current context</param>
        public DapperDatabaseContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <inheritdoc/>
        public IDbConnection Connection => new System.Data.SqlClient.SqlConnection(_connectionString);

        /// <inheritdoc/>
        public IEnumerable<T> Exec<T>(string storedProcedure, IEnumerable<IDataParameter> parameters)
        {
            dynamic paraObj = GeneratePrameters(parameters);
            return Exec<T>(storedProcedure, paraObj);
        }

        /// <inheritdoc/>
        public IEnumerable<T> Exec<T>(string storedProcedure, dynamic parameterObject = null)
        {
            var paraObj = (object)parameterObject;
            return Connection.Query<T>(storedProcedure, param: paraObj, commandType: CommandType.StoredProcedure);
        }

        /// <inheritdoc/>
        public Task<IEnumerable<T>> ExecAsync<T>(string storedProcedure, IEnumerable<IDataParameter> parameters)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<IEnumerable<T>> ExecAsync<T>(string storedProcedure, dynamic parameterObject = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int ExecNonQuery(string storedProcedure, IEnumerable<IDataParameter> parameters)
        {
            dynamic paraObjQuery = GeneratePrameters(parameters);
            return ExecNonQuery(storedProcedure, paraObjQuery);
        }

        /// <inheritdoc/>
        public int ExecNonQuery(string storedProcedure, dynamic parameterObject = null)
        {
            var objPara = (object)parameterObject;
            return Connection.Execute(storedProcedure, param: objPara, commandType: CommandType.StoredProcedure);
        }

        /// <inheritdoc/>
        public Task<int> ExecNonQueryAsync(string storedProcedure, IEnumerable<IDataParameter> parameters)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<int> ExecNonQueryAsync(string storedProcedure, dynamic parameterObject = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public T ExecSingle<T>(string storedProcedure, IEnumerable<IDataParameter> parameters)
        {
            dynamic paramObj = GeneratePrameters(parameters);
            return ExecSingle<T>(storedProcedure, paramObj);
        }

        /// <inheritdoc/>
        public T ExecSingle<T>(string storedProcedure, dynamic parameterObject = null)
        {
            var obj = (object)parameterObject;
            if (TypeUtils.IsPrimitive(typeof(T)))
            {
                return Connection.ExecuteScalar<T>(storedProcedure, param: obj, commandType: CommandType.StoredProcedure);
            }
            else
            {
                return Connection.QueryFirst<T>(storedProcedure, param: obj, commandType: CommandType.StoredProcedure);
            }
        }

        /// <inheritdoc/>
        public Task<T> ExecSingleAsync<T>(string storedProcedure, IEnumerable<IDataParameter> parameters)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<T> ExecSingleAsync<T>(string storedProcedure, dynamic parameterObject = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<T> ExecSingleWithTranactionAsync<T>(string storedProcedure, IEnumerable<IDataParameter> parameters)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<T> ExecSingleWithTranactionAsync<T>(string storedProcedure, dynamic parameterObject = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public T ExecSingleWithTransaction<T>(string storedProcedure, IEnumerable<IDataParameter> parameters)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public T ExecSingleWithTransaction<T>(string storedProcedure, dynamic parameterObject = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<IEnumerator<T>> ExecWithTranactionAsync<T>(string storedProcedure, IEnumerable<IDataParameter> parameters)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<IEnumerator<T>> ExecWithTranactionAsync<T>(string storedProcedure, dynamic parameterObject = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void ExecWithTransaction(string storedProcedure, IEnumerable<IDataParameter> parameters)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void ExecWithTransaction(string storedProcedure, dynamic parameterObject = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IEnumerator<T> ExecWithTransaction<T>(string storedProcedure, IEnumerable<IDataParameter> parameters)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IEnumerator<T> ExecWithTransaction<T>(string storedProcedure, dynamic parameterObject = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task ExecWithTransactionAsync(string storedProcedure, IEnumerable<IDataParameter> parameters)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task ExecWithTransactionAsync(string storedProcedure, dynamic parameterObject = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IMultipleResultSet QueryMultipleResult(string storedProcedure, IEnumerable<IDataParameter> parameters)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IMultipleResultSet QueryMultipleResult(string storedProcedure, dynamic parameterObject = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<IMultipleResultSet> QueryMultipleResultAsync(string storedProcedure, IEnumerable<IDataParameter> parameters)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<IMultipleResultSet> QueryMultipleResultAsync(string storedProcedure, dynamic parameterObject = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Processing parameters prepare execute command
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private dynamic GeneratePrameters(IEnumerable<IDataParameter> parameters)
        {
            dynamic execParamObj = null;

            if (parameters != null && parameters.Count() > 0)
            {
                foreach (var param in parameters)
                {
                    execParamObj[param.ParameterName] = param.Value;
                }
            }
            return execParamObj;
        }
    }
}
