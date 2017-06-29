using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Euroland.NetCore.ToolsFramework.Data
{
    /// <summary>
    /// Abstract class represents the set of multiple result by using Dapper library 
    /// at https://github.com/StackExchange/Dapper
    /// </summary>
    public class DapperMultipleResultSet : IMultipleResultSet
    {
        private readonly IDatabaseContext _dbContext;
        private readonly Dapper.SqlMapper.GridReader _gridReader;
        public DapperMultipleResultSet(IDatabaseContext dbContext, Dapper.SqlMapper.GridReader gridReader)
        {
            if (dbContext == null)
                throw new ArgumentNullException("dbContext");
            if (_gridReader == null)
                throw new ArgumentNullException("_gridReader");

            if (_gridReader.IsConsumed)
                throw new InvalidOperationException("The underlying reader has been consumed or closed");

            _dbContext = dbContext;
            _gridReader = gridReader;
        }
        public IDatabaseContext DbContext => _dbContext;

        /// <inheritdoc />
        public IEnumerable<TResult> Get<TResult>()
        {
            return _gridReader.Read<TResult>();
        }

        /// <inheritdoc />
        public Task<IEnumerable<TResult>> GetAsync<TResult>()
        {
            return _gridReader.ReadAsync<TResult>();
        }

        /// <inheritdoc />
        public TResult GetSingle<TResult>()
        {
            return _gridReader.ReadSingle<TResult>();
        }

        /// <inheritdoc />
        public async Task<TResult> GetSingleAsync<TResult>()
        {
            return await _gridReader.ReadSingleAsync<TResult>();
        }
    }
}
