using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using Trasys.Dev.Tools.Data.Entity;

namespace Trasys.Dev.Tools.Data.Entity
{
    /// <summary>
    /// Class to manage classical entity action like Insert or Update a data base table with all property values of a specified object.
    /// </summary>
    /// <typeparam name="TConnection"></typeparam>
    /// <typeparam name="TCommand"></typeparam>
    /// <typeparam name="TCommandParameters"></typeparam>
    /// <typeparam name="TTransaction"></typeparam>
    /// <typeparam name="TException"></typeparam>
    [SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes")]
    public abstract class ActionBase<TConnection, TCommand, TCommandParameters, TTransaction, TException> 
        where TConnection : DbConnection
        where TCommand : DbCommand, new()
        where TCommandParameters : DbParameterCollection        
        where TTransaction : DbTransaction
        where TException : DbException 
    {
        private DatabaseCommandBase<TConnection, TCommand, TCommandParameters, TTransaction, TException> _command = null;
        private string _originalCommandText = String.Empty;
        private System.Data.CommandType _orginalCommandType = System.Data.CommandType.Text;
        private List<TCommandParameters> _originalParameters = new List<TCommandParameters>();
        private DbTable _entity = null;
        private List<ActionColumn> _columns = null;
        private Type _actionType = null;

        /// <summary>
        /// Initializes a new instance of DatabaseCommandAction
        /// </summary>
        /// <param name="command"></param>
        internal ActionBase(DatabaseCommandBase<TConnection, TCommand, TCommandParameters, TTransaction, TException> command, DbTable entity,Type actionType)
        {
            _command = command;
            _entity = entity;
            _actionType = actionType;
        }

        /// <summary>
        /// Initializes a new instance of DatabaseCommandAction
        /// </summary>
        /// <param name="command"></param>
        internal ActionBase(DatabaseCommandBase<TConnection, TCommand, TCommandParameters, TTransaction, TException> command, DbTable entity) : this(command,entity,entity.GetType())
        {
        }

        /// <summary>
        /// Gets the DataBase command to execute the update queries.
        /// </summary>
        protected virtual DatabaseCommandBase<TConnection, TCommand, TCommandParameters, TTransaction, TException> Command
        {
            get
            {
                return _command;
            }
        }

        /// <summary>
        /// Gets the generated INSERT/UPDATE command. 
        /// </summary>
        /// <param name="forceCompleteUpdate">True to update all columns. False to update only modified values.</param>
        /// <returns>The SQL update command text.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly")]
        public virtual string GetUpdateCommandText(bool forceCompleteUpdate)
        {
            throw new ArgumentNullException("Please, overrides this methods.");
        }

        /// <summary>
        /// Gets the generated INSERT/UPDATE command. 
        /// </summary>
        /// <param name="forceCompleteUpdate">True to update all columns. False to update only modified values.</param>
        /// <returns>The SQL update command text.</returns>
        public virtual string GetUpdateCommandText()
        {
            return GetUpdateCommandText(false);
        }

        /// <summary>
        /// Insert or update the associated table, by attribute [Table], with all entity property values.
        /// </summary>
        /// <param name="forceCompleteUpdate">True to update all columns. False to update only modified values.</param>
        /// <returns>Count of modified rows</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly")]
        public virtual long Update(bool forceCompleteUpdate)
        {
            throw new ArgumentNullException("Please, overrides this methods.");
        }

        /// <summary>
        /// Insert or update the associated table, by attribute [Table], with all entity property values.
        /// </summary>
        /// <returns>Count of modified rows</returns>
        public virtual long Update()
        {
            return Update(false);
        }

        /// <summary>
        /// Gets all ActionColumns associated to the entity object.
        /// </summary>
        /// <returns>All Action columns</returns>
        internal virtual ActionColumn[] GetActionColumns()
        {
            if (_columns == null)
            {
                _columns = new List<ActionColumn>();

                Annotations.TableAttribute tableAttribute = Annotations.TableAttribute.GetTableAttribute(_actionType);
                string tableName = tableAttribute == null ? _actionType.GetType().Name : tableAttribute.Name;

                PropertyInfo[] properties = _actionType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                int index = 0;
                foreach (PropertyInfo property in properties)
                {
                    if (property.DeclaringType != _actionType) //Don't use the base properties
                        continue;

                    Annotations.NotMappedAttribute notMappedAttribute = Annotations.NotMappedAttribute.GetNotMappedAttribute(property); // Don't update NotMappedAttribute properties
                    if (notMappedAttribute != null)
                        continue;

                    Annotations.ColumnAttribute columnAttribute = Annotations.ColumnAttribute.GetColumnAttribute(property);
                    string columnName = columnAttribute == null ? property.Name : columnAttribute.Name ;

                    Annotations.PrimaryKeyAttribute primaryKeyAttribute = Annotations.PrimaryKeyAttribute.GetPrimaryKeyAttribute(property);
                    bool isPrimary = primaryKeyAttribute == null ? false : primaryKeyAttribute.IsPrimaryKey;

                    Annotations.IdentityAttribute identityAttribute = Annotations.IdentityAttribute.GetIdentityAttribute(property);
                    bool isIdentity = identityAttribute == null ? false : identityAttribute.IsIdentity;

                    Annotations.AutoGeneratedAttribute autoGeneratedAttribute = Annotations.AutoGeneratedAttribute.GetAutoGeneratedAttribute(property);
                    bool isAutoGenerated = autoGeneratedAttribute == null ? isIdentity : autoGeneratedAttribute.IsAutoGenerated;

                    object value = null;

                    if (!String.IsNullOrEmpty(columnName))
                    {
                        value = property.GetValue(_entity, null);
                    }

                    _columns.Add(new ActionColumn() { TableName = tableName, 
                                                      Index = index, 
                                                      FieldName = columnName, 
                                                      Value = value, 
                                                      IsValueChanged = _entity.IsPropertyChanged(property.Name),
                                                      IsPrimaryKey = isPrimary, 
                                                      IsIdentity = isIdentity, 
                                                      IsAutoGenerated = isAutoGenerated 
                                                    });

                    index++;
                }
            }

            return _columns.ToArray();
        }

        /// <summary>
        /// Keep main DatabaseCommand properties (CommandText, CommandType and Parameters).
        /// </summary>
        protected virtual void KeepCommandProperties()
        { 
            _originalCommandText = _command.CommandText.ToString();
            _orginalCommandType = _command.CommandType;
            _originalParameters.Clear();
            foreach (TCommandParameters param in _command.Parameters)
            {
                _originalParameters.Add(param);
            }
        }

        /// <summary>
        /// Restore main DatabaseCommand properties (CommandText, CommandType and Parameters).
        /// </summary>
        protected virtual void RestoreCommandProperties()
        {
            _command.CommandText = new StringBuilder(_originalCommandText);
            _command.CommandType = _orginalCommandType;
            _command.Parameters.Clear();
            _command.Parameters.AddRange(_originalParameters.ToArray());
        }
    }
}
