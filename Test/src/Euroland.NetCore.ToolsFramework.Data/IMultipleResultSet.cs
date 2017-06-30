using System;
using System.Collections.Generic;
using System.Text;

namespace Euroland.NetCore.ToolsFramework.Data
{
    /// <summary>
    /// Abstract class represents the set of multiple result
    /// </summary>
    public interface IMultipleResultSet
    {
        IDatabaseContext DbContext { get; }

        /// <summary>
        /// Get an object from multiple set
        /// </summary>
        /// <typeparam name="TResult">Type of object to map returned result to</typeparam>
        /// <returns>Returns an object from multiple set with properties matching columns</returns>
        TResult GetSingle<TResult>();

        /// <summary>
        /// Get an object from multiple set
        /// </summary>
        /// <typeparam name="TResult">Type of object to map returned result to</typeparam>
        /// <returns>Returns an object from multiple set with properties matching columns</returns>
        System.Threading.Tasks.Task<TResult> GetSingleAsync<TResult>();

        /// <summary>
        /// Gets the result of set of record
        /// </summary>
        /// <typeparam name="TResult">Type of object to map returned result to</typeparam>
        /// <returns>The list of record</returns>
        IEnumerable<TResult> Get<TResult>();

        /// <summary>
        /// Gets asynchronously the result of set of record
        /// </summary>
        /// <typeparam name="TResult">Type of object to map returned result to</typeparam>
        /// <returns>The list of record</returns>
        System.Threading.Tasks.Task<IEnumerable<TResult>> GetAsync<TResult>();
    }
}
