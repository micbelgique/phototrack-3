using System;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace Trasys.Dev.Tools.Data
{
    /// <summary>
    /// SQL Server Database command management 
    /// </summary>
    public class SqlDatabaseCommand : DatabaseCommandBase<SqlConnection, SqlCommand, SqlParameterCollection, SqlTransaction, SqlException>
    {
        /// <summary>
        /// Create a command for a SQL Server connection
        /// </summary>
        /// <param name="connection">Active SQL Server connection</param>
        public SqlDatabaseCommand(SqlConnection connection) : base(connection) 
        { 
        }

        /// <summary>
        /// Create a command for a SQL Server connection
        /// </summary>
        /// <param name="connection">Active SQL Server connection</param>
        /// <param name="commandText">SQL query</param>
        public SqlDatabaseCommand(SqlConnection connection, string commandText) : base(connection, commandText)
        { 
        
        }

        /// <summary>
        /// Create a command for a SQL Server connection
        /// </summary>
        /// <param name="connection">Active SQL Server connection</param>
        /// <param name="commandText">SQL query</param>
        /// <param name="commandTimeout">Maximum timeout of the queries</param>
        public SqlDatabaseCommand(SqlConnection connection, string commandText, int commandTimeout) : base(connection, commandText, commandTimeout)
        {
        
        }

        /// <summary>
        /// Gets a reference to an Actions manager: INSERT, DELETE, UPDATE commands for a DbTable entity item.
        /// </summary>
        public Entity.SqlAction Actions(Entity.DbTable entity)
        {
            return new Entity.SqlAction(this, entity);
        }

        /// <summary>
        /// Gets a reference to an Actions manager: INSERT, DELETE, UPDATE commands for a DbTable entity item.
        /// </summary>
        public Entity.SqlAction Actions<T>(Entity.DbTable entity)
        {            
            return new Entity.SqlAction(this, entity, typeof(T));
        }

        /// <summary>
        /// Return the query with sql parameters replaced by plain string
        /// </summary>
        /// <returns></returns>
        public string GetRequestWithoutParameters()
        {
            return this.CommandFormatted.CommandAsText;
        }
    }
}
