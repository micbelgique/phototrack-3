using System;
using System.Text;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Trasys.Dev.Tools.Data
{
    /// <summary>
    /// Database command management
    /// </summary>
    /// <example>
    /// <code>
    /// public class SqlDatabaseCommand : DatabaseCommandBase&lt;SqlConnection, SqlCommand, SqlParameterCollection, SqlTransaction, SqlException&gt;
    /// {
    ///     public SqlDatabaseCommand(SqlConnection connection) : base(connection) { }
    /// }
    /// </code>
    /// </example>
    [SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes")]
    [DebuggerDisplay("{CommandText}")]
    public abstract class DatabaseCommandBase<TConnection, TCommand, TCommandParameters, TTransaction, TException> : IDisposable 
        where TConnection : DbConnection
        where TCommand : DbCommand, new()
        where TCommandParameters : DbParameterCollection        
        where TTransaction : DbTransaction
        where TException : DbException         
    {
        #region DECLARATIONS

        private TConnection _connection;
        private StringBuilder _commandText;
        private TCommand _cmd = new TCommand();
        private TException _exception;

        #endregion

        #region EVENTS

        /// <summary>
        /// Signature for ExceptionOccured event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void ExceptionOccuredEventHandler(object sender, ExceptionOccuredEventArgs e);

        /// <summary>
        /// Event raised when an SQL Exception occured (in Execute Methods)
        /// </summary>
        public event ExceptionOccuredEventHandler ExceptionOccured;

        #endregion
        
        #region CONSTRUCTORS

        /// <summary>
        /// Create a command for a SQL Server connection
        /// </summary>
        /// <param name="connection">Active connection</param>
        /// <param name="commandText">SQl Query</param>
        /// <param name="commandTimeout">Maximum timeout of the queries</param>
        public DatabaseCommandBase(TConnection connection, string commandText, int commandTimeout)
        {
            this.ThrowException = true;
            _connection = connection;

            if (commandTimeout >= 0)
                _cmd.CommandTimeout = commandTimeout;
            
            _cmd.Connection = connection;
            _commandText = new StringBuilder(commandText);
        }

        /// <summary>
        /// Create a command for a a database connection
        /// </summary>
        /// <param name="connection">Active database connection</param>
        /// <param name="commandText">SQL query command</param>
        public DatabaseCommandBase(TConnection connection, string commandText)
            : this(connection, commandText, -1)
        {

        }

        /// <summary>
        /// Create a command for a database connection
        /// </summary>
        /// <param name="connection">Active database connection</param>
        public DatabaseCommandBase(TConnection connection)
            : this(connection, string.Empty, -1)
        {

        }

        #endregion

        #region PROPERTIES

        /// <summary>
        /// Gets or sets the sql query
        /// </summary>
        public virtual System.Text.StringBuilder CommandText
        {
            get
            {
                return _commandText;
            }
            set
            {
                _commandText = value;
            }
        }

        /// <summary>
        /// Gets the CommandText formatted with parameters, in html, ...
        /// </summary>
        public virtual DatabaseCommandFormatted CommandFormatted 
        {
            get
            { 
                _cmd.CommandText = _commandText.ToString();
                return new DatabaseCommandFormatted(_cmd);
            }
        }

        /// <summary>
        /// Gets or sets the command type
        /// </summary>
        public virtual System.Data.CommandType CommandType
        {
            get
            {
                return _cmd.CommandType;
            }
            set
            {
                _cmd.CommandType = value;
            }
        }

        /// <summary>
        /// Gets or sets the active connection
        /// </summary>
        protected virtual TConnection Connection
        {
            get
            {
                return _connection;
            }
            set
            {
                _connection = value;
            }
        }

        /// <summary>
        /// Gets or sets the current DbCommand
        /// </summary>
        protected virtual TCommand Command
        {
            get
            {
                return _cmd;
            }
            set
            {
                _cmd = value;
            }
        }

        /// <summary>
        /// Gets or sets the current transaction
        /// </summary>
        public virtual TTransaction Transaction
        {
            get
            {
                return _cmd.Transaction as TTransaction;
            }
            set
            {
                _cmd.Transaction = value;
            }
        }

        /// <summary>
        /// Gets sql parameters of the query
        /// </summary>
        public virtual TCommandParameters Parameters
        {
            get
            {
                return _cmd.Parameters as TCommandParameters;
            }
        }

        /// <summary>
        /// Enable or disable the raise of exceptions when queries are executed.
        /// Default is True (Enabled).
        /// </summary>
        public virtual bool ThrowException { get; set; }

        /// <summary>
        /// Gets the last raised exception 
        /// </summary>
        public virtual TException Exception
        {
            get
            {
                return _exception;
            }
        }

        #endregion

        #region METHODS

        /// <summary>
        /// Delete the CommandText and the sql parameters of it
        /// </summary>
        public virtual void Clear()
        {
            this.CommandText.Length = 0;
            this.Parameters.Clear();
        }

        /// <summary>
        /// Prepare a query
        /// </summary>
        public virtual void Prepare()
        {
            _cmd.CommandText = _commandText.ToString();
            _cmd.Prepare();
        }

        /// <summary>
        /// Begin a transaction into the database
        /// </summary>
        /// <returns>Transaction</returns>
        public virtual TTransaction TransactionBegin()
        {
            _cmd.Transaction = _cmd.Connection.BeginTransaction();

            return _cmd.Transaction as TTransaction;
        }

        /// <summary>
        /// Commit the current transaction to the database
        /// </summary>
        public virtual void TransactionCommit()
        {
            _cmd.Transaction.Commit();
        }

        /// <summary>
        /// Rollback the current transaction 
        /// </summary>
        public virtual void TransactionRollback()
        {
            _cmd.Transaction.Rollback();
        }

        /// <summary>
        /// Execute query and return results by using a Datatable
        /// </summary>
        /// <returns>DataTable of results</returns>
        public virtual System.Data.DataTable ExecuteTable()
        {
            ResetException();

            try
            {
                lock (_connection)
                {
                    System.Data.DataTable data = new System.Data.DataTable();

                    _cmd.CommandText = _commandText.ToString();
                    using (DbDataReader dr = _cmd.ExecuteReader())
                    {
                        data.Load(dr);                            
                        return data;
                    }
                }
            }
            catch (TException ex)
            {                
                return ThrowSqlExceptionOrDefaultValue<System.Data.DataTable>(ex);

            }

        }

        /// <summary>
        /// Execute the query and return an array of new instances of typed results filled with data table result.
        /// </summary>
        /// <typeparam name="TReturn">Object type</typeparam>
        /// <returns>Array of typed results</returns>
        /// <example>
        /// <code>
        ///   Employee[] emp = cmd.ExecuteTable&lt;Employee&gt;();
        ///   var x = cmd.ExecuteTable&lt;Employee&gt;();
        /// </code>
        /// <remarks>
        ///   Result object property (ex. Employee.Name) may be tagged with the ColumnAttribute 
        ///   to set which column name (ex. [Column("Name")] must be associated to this property.
        /// </remarks>
        /// </example>
        public virtual TReturn[] ExecuteTable<TReturn>()
        {
            System.Data.DataTable table = this.ExecuteTable();

            TReturn[] results = new TReturn[table.Rows.Count];
            for (int i = 0; i < table.Rows.Count; i++)
            {
                results[i] = CreateInstanceAndFillInstance<TReturn>(table.Rows[i]);
            }

            return results;
        }

        /// <summary>
        /// Execute the query and return an array of new instances of typed results filled with data table result.
        /// </summary>
        /// <typeparam name="TReturn">Object type</typeparam>
        /// <param name="itemOftype"></param>
        /// <returns>Array of typed results</returns>
        /// <example>
        /// <code>
        ///   Employee emp = new Employee();
        ///   var x = cmd.ExecuteTable(new { emp.Age, emp.Name });
        ///   var y = cmd.ExecuteTable(new { Age = 0, Name = "" });
        /// </code>
        /// <remarks>
        ///   Result object property (ex. Employee.Name) may be tagged with the ColumnAttribute 
        ///   to set which column name (ex. [Column("Name")] must be associated to this property.
        /// </remarks>
        /// </example>
        public virtual TReturn[] ExecuteTable<TReturn>(TReturn itemOftype)
        {
            return ExecuteTable<TReturn>();
        }

        /// <summary>
        /// Execute the query and return the count of modified rows
        /// </summary>
        /// <returns>Count of modified rows</returns>
        public virtual int ExecuteNonQuery()
        {
            ResetException();

            try
            {
                lock (_connection)
                {
                    string sql = _commandText.ToString();
                                        
                    if (String.CompareOrdinal(sql, _cmd.CommandText) != 0) _cmd.CommandText = sql;

                    return _cmd.ExecuteNonQuery();
                }
            }
            catch (TException ex)
            {
                return ThrowSqlExceptionOrDefaultValue<int>(ex);
                   
            }

        }

        /// <summary>
        /// Execute the query and return the first column of the first row of results
        /// </summary>
        /// <returns>Object - Result</returns>
        public virtual object ExecuteScalar()
        {
            ResetException();

            try
            {
                lock (_connection)
                {
                    string sql = _commandText.ToString();
                    if (String.CompareOrdinal(sql, _cmd.CommandText) != 0) _cmd.CommandText = sql;

                    return _cmd.ExecuteScalar();
                }
            }
            catch (TException ex)
            {
                return ThrowSqlExceptionOrDefaultValue<object>(ex);
            }

        }

        /// <summary>
        /// Execute the query and return the first column of the first row of results
        /// </summary>
        /// <typeparam name="TReturn">Return type</typeparam>
        /// <returns>Result</returns>
        public virtual TReturn ExecuteScalar<TReturn>()
        {
            object scalar = this.ExecuteScalar();

            if (scalar == null || scalar == DBNull.Value)
                return default(TReturn);
            else
                return (TReturn)scalar;

        }

        /// <summary>
        /// Execute the query and return the first row of results    
        /// </summary>
        /// <returns>First row of results</returns>
        public virtual System.Data.DataRow ExecuteRow()
        {
            System.Data.DataTable result = this.ExecuteTable();

            if (result == null)
                return null;

            if (result.Rows.Count > 0)
                return result.Rows[0];
            else
                return null;
        }

        /// <summary>
        /// Execute the query and return a new instance of TReturn with the first row of results
        /// </summary>
        /// <typeparam name="TReturn">Object type</typeparam>
        /// <returns>First row of results</returns>
        /// <example>
        /// <code>
        ///   Employee emp = cmd.ExecuteRow&lt;Employee&gt;();
        /// </code>
        /// <remarks>
        ///   Result object property (ex. Employee.Name) may be tagged with the ColumnAttribute 
        ///   to set which column name (ex. [Column("Name")] must be associated to this property.
        /// </remarks>
        /// </example>
        public virtual TReturn ExecuteRow<TReturn>()
        {
            System.Data.DataRow row = this.ExecuteRow();

            return CreateInstanceAndFillInstance<TReturn>(row);
        }

        /// <summary>
        /// Execute the query and fill the specified TReturn object with the first row of results
        /// </summary>
        /// <typeparam name="TReturn">Object type</typeparam>
        /// <param name="itemOftype"></param>
        /// <returns>First row of results</returns>
        /// <example>
        /// <code>
        ///   Employee emp = new Employee();
        ///   var x = cmd.ExecuteRow(new { emp.Age, emp.Name });
        ///   var y = cmd.ExecuteRow(new { Age = 0, Name = "" });
        ///   var z = cmd.ExecuteRow(emp);
        /// </code>
        /// <remarks>
        ///   Result object property (ex. Employee.Name) may be tagged with the ColumnAttribute 
        ///   to set which column name (ex. [Column("Name")] must be associated to this property.
        /// </remarks>
        /// </example>
        public virtual TReturn ExecuteRow<TReturn>(TReturn itemOftype)
        {
            System.Data.DataRow row = this.ExecuteRow();

            return CreateInstanceAndFillInstance<TReturn>(row, itemOftype);
        }

        /// <summary>
        /// Raises the ExceptionOccured event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnExceptionOccured(ExceptionOccuredEventArgs e)
        {
            if (this.ExceptionOccured != null)
            {
                ExceptionOccured(this, e);
            }
        }

        /// <summary>
        /// Dispose the object and free ressources
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Dispose the object and free ressources
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                
            }

            if (_cmd.Transaction != null)
                _cmd.Transaction.Dispose();

            if (_cmd != null)
                _cmd.Dispose();
        }

        /// <summary>
        /// Dispose the object and free ressources
        /// </summary>
        ~DatabaseCommandBase()
        {
            Dispose(false);
        }

        #endregion

        #region PRIVATE

        /// <summary>
        /// Set the last raised exception to null
        /// </summary>
        protected virtual void ResetException()
        {
            _exception = null;
        }

        /// <summary>
        /// Raise the Exception if the ThrowException property is set to TRUE
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="ex">Exception</param>
        /// <returns></returns>
        protected virtual T ThrowSqlExceptionOrDefaultValue<T>(TException ex)
        {
            _exception = ex;

            OnExceptionOccured(new ExceptionOccuredEventArgs() { Exception = _exception });

            if (ex != null)
            {
                if (this.ThrowException) throw ex;
            }

            return default(T);
        }

        /// <summary>
        /// Creates a new instance of T type and sets all row values to the new T properties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <returns></returns>
        protected virtual T CreateInstanceAndFillInstance<T>(System.Data.DataRow row)
        {
            return CreateInstanceAndFillInstance<T>(row, default(T));
        }

        /// <summary>
        /// If item is null, creates a new instance of T type and sets all row values to the new T properties.
        /// If item is not null, sets all row values to item object properties
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        protected virtual T CreateInstanceAndFillInstance<T>(System.Data.DataRow row, T item)
        {
            if (row != null)
            {
                Type type = typeof(T);
                object[] values = row.ItemArray;

                // For anonymous type, creates a new instance and sets all data row values to this new object.
                if (type.IsAnonymousType())
                {
                    object newItem = Activator.CreateInstance(type, values);
                    return (T)newItem;
                }

                // For defined type, creates a new instance and fill all data row values to this new object.
                else
                {
                    int i = 0;
                    object newItem = null;

                    // Creates or gets an instance of T
                    if (EqualityComparer<T>.Default.Equals(item, default(T)))
                        newItem = Activator.CreateInstance(type, null);
                    else
                        newItem = item;

                    // Disable PropertyChanged event to avoid modification tracing.
                    if (newItem is Entity.DbTable)
                    {
                        ((Entity.DbTable)newItem).ActivatesPropertyChangedEvent(false);
                    }

                    // For each property, assign the value.
                    List<PropertyInfo> properties = new List<PropertyInfo>();
                    properties.AddRange(type.GetProperties(BindingFlags.Public | BindingFlags.Instance));
                    //type.GetInterfaces().ToList().ForEach(p => properties.AddRange(p.GetProperties()));

                    foreach (PropertyInfo property in properties)
                    {
                        bool columnIsNullable = false;
                        if (Annotations.NotMappedAttribute.GetNotMappedAttribute(property) != null) // This column has to ignored
                        {
                            continue;
                        }

                        Annotations.ColumnAttribute columnAttribute = Annotations.ColumnAttribute.GetColumnAttribute(property);
                        string columnName = columnAttribute == null ? String.Empty : columnAttribute.Name;
                        object value = null;

                        if (String.IsNullOrEmpty(columnName))
                        {
                            if (i < values.Length)
                            {
                                value = values[i];
                                columnIsNullable = row.Table.Columns[i].AllowDBNull = true;
                            }
                        }
                        else
                        {
                            if (row.Table.Columns.Contains(columnName))
                            {
                                value = row[columnName];
                                columnIsNullable = row.Table.Columns[columnName].AllowDBNull = true;
                            }
                        }

                        if (value != null && property.PropertyType != value.GetType() && value != System.DBNull.Value)
                        {
                            bool showError = false;
                            // Check for nullable type
                            if ((columnIsNullable && property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                            {
                                if(property.PropertyType.GetGenericArguments()[0] != value.GetType())
                                {
                                    showError = true;    
                                }
                            }
                            else
                            {
                                showError = true;
                            }

                            if(showError)
                            {
                                throw new ArgumentException(String.Format("Column [{0}] with type [{1}] is different from the target type [{2}]", property.Name, property.PropertyType, value.GetType()));
                            }
                        }

                        property.SetValue(newItem, value == System.DBNull.Value ? null : value, null);
                        
                        i++;
                    }

                    // Enable PropertyChanged event to keep modification tracing.
                    if (newItem is Entity.DbTable)
                    {
                        ((Entity.DbTable)newItem).ActivatesPropertyChangedEvent(true);
                    }

                    return (T)newItem;
                }
            }
            else
            {
                return default(T);
            }
        }

        #endregion
    }
}
