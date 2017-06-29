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
            using (var connection = Connection)
            {
                connection.Open();
                return connection.Query<T>(storedProcedure, param: paraObj, commandType: CommandType.StoredProcedure);
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<T>> ExecAsync<T>(string storedProcedure, IEnumerable<IDataParameter> parameters)
        {
            dynamic paraObj = GeneratePrameters(parameters);
            return await ExecAsync<T>(storedProcedure, paraObj);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<T>> ExecAsync<T>(string storedProcedure, dynamic parameterObject = null)
        {
            var paraObj = (object)parameterObject;
            using (var connection = Connection)
            {
                connection.Open();
                return await connection.QueryAsync<T>(storedProcedure, param: paraObj, commandType: CommandType.StoredProcedure);
            }
        }

        /// <inheritdoc/>
        /// CUONGNM
        public int ExecNonQuery(string storedProcedure, IEnumerable<IDataParameter> parameters)
        {
            dynamic paraObjQuery = GeneratePrameters(parameters);
            return ExecNonQuery(storedProcedure, paraObjQuery);
        }

        /// <inheritdoc/>
        /// CUONGNM
        public int ExecNonQuery(string storedProcedure, dynamic parameterObject = null)
        {
            var objPara = (object)parameterObject;
            using (var connection = Connection)
            {
                connection.Open();
                return connection.Execute(storedProcedure, param: objPara, commandType: CommandType.StoredProcedure);
            }
        }

        /// <inheritdoc/>
        /// CUONGNM
        public async Task<int> ExecNonQueryAsync(string storedProcedure, IEnumerable<IDataParameter> parameters)
        {
            dynamic paraObjQuery = GeneratePrameters(parameters);
            return await ExecNonQueryAsync(storedProcedure, paraObjQuery);
        }

        /// <inheritdoc/>
        /// CUONGNM
        public async Task<int> ExecNonQueryAsync(string storedProcedure, dynamic parameterObject = null)
        {
            var objPara = (object)parameterObject;
            using (var connection = Connection)
            {
                connection.Open();
                return await connection.ExecuteAsync(storedProcedure, param: objPara, commandType: CommandType.StoredProcedure);
            }
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
            using (var connection = Connection)
            {
                connection.Open();
                if (TypeUtils.IsPrimitive(typeof(T)))
                {
                    return connection.ExecuteScalar<T>(storedProcedure, param: obj, commandType: CommandType.StoredProcedure);
                }
                else
                {
                    return connection.QueryFirst<T>(storedProcedure, param: obj, commandType: CommandType.StoredProcedure);
                }
            }
        }                                                                                                                                                                                                                    

        /// <inheritdoc/>
        /// CUONGNM
        public async Task<T> ExecSingleAsync<T>(string storedProcedure, IEnumerable<IDataParameter> parameters)
        {
            dynamic paramObj = GeneratePrameters(parameters);
            return await ExecSingleAsync<T>(storedProcedure, paramObj);
        }

        /// <inheritdoc/>
        /// CUONGNM
        public async Task<T> ExecSingleAsync<T>(string storedProcedure, dynamic parameterObject = null)
        {
            var objPara = (object)parameterObject;
            using (var connection = Connection)
            {
                connection.Open();
                return await connection.QuerySingleAsync<T>(storedProcedure, param: objPara, commandType: CommandType.StoredProcedure);
            }
        }

        /// <inheritdoc/>
        /// CUONGNM
        public async Task<T> ExecSingleWithTranactionAsync<T>(string storedProcedure, IEnumerable<IDataParameter> parameters)
        {
            dynamic paramObj = GeneratePrameters(parameters);
            return await ExecSingleWithTranactionAsync<T>(storedProcedure, paramObj);
        }

        /// <inheritdoc/>
        /// CUONGNM
        public async Task<T> ExecSingleWithTranactionAsync<T>(string storedProcedure, dynamic parameterObject = null)
        {
            var objPara = (object)parameterObject;
            using (var connection = Connection)
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    var execSingleWithTranactionAsync = await connection.QuerySingleAsync<T>(storedProcedure, param: objPara, commandType: CommandType.StoredProcedure, transaction: transaction);
                    transaction.Commit();
                    return  execSingleWithTranactionAsync;
                }
            }
        }

        /// <inheritdoc/>
        /// CUONGNM
        public T ExecSingleWithTransaction<T>(string storedProcedure, IEnumerable<IDataParameter> parameters)
        {
            dynamic paramObj = GeneratePrameters(parameters);
            return ExecSingleWithTransaction<T>(storedProcedure, paramObj);
        }

        /// <inheritdoc/>
        /// CUONGNM
        public T ExecSingleWithTransaction<T>(string storedProcedure, dynamic parameterObject = null)
        {
            var obj = (object)parameterObject;
            using (var connection = Connection)
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    if (TypeUtils.IsPrimitive(typeof(T)))
                    {
                        var execSingleWithTransaction = connection.ExecuteScalar<T>(storedProcedure, param: obj, commandType: CommandType.StoredProcedure, transaction: transaction);
                        transaction.Commit();
                        return execSingleWithTransaction;
                    }
                    else
                    {
                        var execSingleWithTransaction = connection.QueryFirst<T>(storedProcedure, param: obj, commandType: CommandType.StoredProcedure, transaction: transaction);
                        transaction.Commit();
                        return execSingleWithTransaction;
                    }
                }
            }
        }

        /// <inheritdoc/>
        /// CUONGNM
        public async Task<IEnumerator<T>> ExecWithTranactionAsync<T>(string storedProcedure, IEnumerable<IDataParameter> parameters)
        {
            dynamic paraObj = GeneratePrameters(parameters);
            return await ExecWithTranactionAsync<T>(storedProcedure, paraObj);
        }

        /// <inheritdoc/>
        /// CUONGNM
        public async Task<IEnumerator<T>> ExecWithTranactionAsync<T>(string storedProcedure, dynamic parameterObject = null)
        {
            var paraObj = (object)parameterObject;
            using (var connection = Connection)
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                        var execWithTranactionAsync = await connection.QueryAsync<T>(storedProcedure, param: paraObj, commandType: CommandType.StoredProcedure);
                        transaction.Commit();
                        return execWithTranactionAsync.GetEnumerator();
                }
            }
        }

        /// <inheritdoc/>
        public void ExecWithTransaction(string storedProcedure, IEnumerable<IDataParameter> parameters)
        {
            dynamic paramObj = GeneratePrameters(parameters);
            ExecWithTransaction(storedProcedure, paramObj);
        }

        /// <inheritdoc/>
        public void ExecWithTransaction(string storedProcedure, dynamic parameterObject = null)
        {
            var objPara = (object)parameterObject;
            using (var connection = Connection)
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    connection.Query(storedProcedure, param: objPara, commandType: CommandType.StoredProcedure, transaction: transaction);
                    transaction.Commit();
                }
            }
        }

        /// <inheritdoc/>
        public IEnumerator<T> ExecWithTransaction<T>(string storedProcedure, IEnumerable<IDataParameter> parameters)
        {
            dynamic paramObj = GeneratePrameters(parameters);
            return ExecWithTransaction(storedProcedure, paramObj);
        }

        /// <inheritdoc/>
        public IEnumerator<T> ExecWithTransaction<T>(string storedProcedure, dynamic parameterObject = null)
        {
            var objPara = (object)parameterObject;
            using (var connection = Connection)
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    var ExecWithTransaction = connection.Query<T>(storedProcedure, param: objPara, commandType: CommandType.StoredProcedure, transaction: transaction);
                    transaction.Commit();
                    return ExecWithTransaction.GetEnumerator();
                }
            }
        }

        /// <inheritdoc/>
        public Task ExecWithTransactionAsync(string storedProcedure, IEnumerable<IDataParameter> parameters)
        {
            dynamic paramObj = GeneratePrameters(parameters);
            return ExecWithTransactionAsync(storedProcedure, paramObj);
        }

        /// <inheritdoc/>
        public Task ExecWithTransactionAsync(string storedProcedure, dynamic parameterObject = null)
        {
            var objPara = (object)parameterObject;
            using (var connection = Connection)
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    var ExecWithTransactionAsync = connection.QueryAsync(storedProcedure, param: objPara, commandType: CommandType.StoredProcedure, transaction: transaction);
                    transaction.Commit();
                    return  ExecWithTransactionAsync;
                }
            }
        }

        /// <inheritdoc/>
        public IMultipleResultSet QueryMultipleResult(string storedProcedure, IEnumerable<IDataParameter> parameters)
        {
            dynamic paramObj = GeneratePrameters(parameters);
            return QueryMultipleResult(storedProcedure, paramObj);
        }

        /// <inheritdoc/>
        public IMultipleResultSet QueryMultipleResult(string storedProcedure, dynamic parameterObject = null)
        {
            var paraObj = (object)parameterObject;
            using (var connection = Connection)
            {
                connection.Open();
                var multipleResult = connection.QueryMultiple(storedProcedure, param: paraObj, commandType: CommandType.StoredProcedure);
                return new DapperMultipleResultSet(this, multipleResult);
            }
        }

        /// <inheritdoc/>
        public async Task<IMultipleResultSet> QueryMultipleResultAsync(string storedProcedure, IEnumerable<IDataParameter> parameters)
        {
            dynamic paramObj = GeneratePrameters(parameters);
            return await QueryMultipleResultAsync(storedProcedure, paramObj);
        }

        /// <inheritdoc/>
        public async Task<IMultipleResultSet> QueryMultipleResultAsync(string storedProcedure, dynamic parameterObject = null)
        {
            var paraObj = (object)parameterObject;
            using (var connection = Connection)
            {
                connection.Open();
                var multipleResult = await connection.QueryMultipleAsync(storedProcedure, param: paraObj, commandType: CommandType.StoredProcedure);
                return new DapperMultipleResultSet(this, multipleResult);
            }
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
