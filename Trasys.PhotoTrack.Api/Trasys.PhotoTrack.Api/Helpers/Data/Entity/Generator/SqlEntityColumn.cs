using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trasys.Dev.Tools.Data.Entity
{
    /// <summary>
    /// Used to creates Entities classes based on a database schema
    /// See EntityColumn for more info.
    /// </summary>
    /// <example>
    /// Column 'ConfigurationID' of type BigInt defined into SQL server will return :
    ///         Column("ConfigurationID")]
    ///     	public long ConfigurationID { get; set; } 
    ///     	
    /// </example>
    internal class SqlEntityColumn
    {
        #region PROPERTIES

        /// <summary>
        /// Get or set Name of the column
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get or set the Name of the type
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// Get or set the flag that indicates if the column can be nullable
        /// </summary>
        public bool IsNullable { get; set; }

        /// <summary>
        /// Get or set the flag that indicates if the column is identity (auto increment)
        /// </summary>
        public bool IsIdentity { get; set; }

        /// <summary>
        /// Get or set the flag that indicates if the column is a primary key 
        /// </summary>
        public bool IsPrimaryKey { get; set; }

        #endregion

        #region METHODS

        /// <summary>
        /// Returns the class definition in string format
        /// </summary>
        /// <returns>
        /// Return code of the property :
        /// <code>
        ///     private long _traceIgnoreID;
        /// 	///<summary>
        ///     Column TraceIgnoreID
        ///     ///</summary>
        ///     [Column("TraceIgnoreID")]
        ///     public long TraceIgnoreID { get; set; } 
        /// </code>
        /// </returns>
        public string GetPropertyDefinition(bool addSerializableAttribute, bool addDataContractAttribute)
        {
            bool nullableType = true;
            string typeOfProperty = "string";
            switch (this.TypeName.ToLower())
            {
                case "bit":
                    typeOfProperty = "bool";
                    break;
                case "float":
                    typeOfProperty = "Single";
                    break;
                case "bigint":
                    typeOfProperty = "long";
                    break;
                case "tinyint":
                    typeOfProperty = "short";
                    break;
                case "int":
                    typeOfProperty = "int";
                    break;
                case "datetimeoffset":
                    typeOfProperty = "DateTimeOffset";
                    break;
                case "smalldatetime": 
                case "datetime":
                    typeOfProperty = "DateTime";
                    break;
                case "char": 
                case "varchar":
                case "nvarchar":
                case "nchar":
                    typeOfProperty = "string";
                    nullableType = false;
                    break;
                case "uniqueidentifier":
                    typeOfProperty = "Guid";
                    break;
                default:
                    break;
            }

            // If the column is nullable, add a '?' at the end of the typename
            //  The .NET type must support nullable type (nullableType flag)
            if (this.IsNullable && nullableType)
            {
                typeOfProperty = String.Format("{0}?", typeOfProperty);
            }
            StringBuilder propertyContent = new StringBuilder();

            // Create private declaration
                propertyContent.AppendLine(String.Format("\t\tprivate {0} _{1};",typeOfProperty,this.Name));

            // Create summary
            propertyContent.AppendLine("\t\t///<summary>");
            propertyContent.AppendLine(String.Format("\t\t/// Column {0}", this.Name));
            propertyContent.AppendLine("\t\t///</summary>");

            //Create IdentityAttribute
            if (this.IsIdentity)
            {
                propertyContent.AppendLine("\t\t[Identity(true)]");
            }

            // Creta PrimaryKey attribute
            if (this.IsPrimaryKey)
            {
                propertyContent.AppendLine("\t\t[PrimaryKey(true)]");
            }

            if (addSerializableAttribute)
            {
                propertyContent.AppendLine("\t\t[Serializable]");
            }

            if (addDataContractAttribute)
            {
                propertyContent.AppendLine("\t\t[DataMember]");
            }

            // Create ColumnAttribute
            propertyContent.AppendLine(String.Format("\t\t[Column(\"{0}\")]", this.Name));
            propertyContent.AppendLine(String.Format("\t\tpublic virtual {0} {1} {2}\t\t{{", typeOfProperty, this.Name, Environment.NewLine));
            propertyContent.AppendLine(String.Format("\t\t\tget {{ return _{0}; }} ",this.Name));
            propertyContent.AppendLine(String.Format("\t\t\tset {{ this.SetProperty(ref _{0}, value, \"{0}\"); }} ",this.Name));
            propertyContent.AppendLine(String.Format("\t\t}}{0}",Environment.NewLine));
            return propertyContent.ToString();
        }

           /// <summary>
        /// Returns the class definition in string format
        /// </summary>
        /// <returns>
        /// Return code of the property :
        /// <code>
        ///     private long _traceIgnoreID;
        /// 	///<summary>
        ///     Column TraceIgnoreID
        ///     ///</summary>
        ///     [Column("TraceIgnoreID")]
        ///     public long TraceIgnoreID { get; set; } 
        /// </code>
        /// </returns>
        public string GetPropertyDefinition()
        {
            return this.GetPropertyDefinition(false,false);
        }

        #endregion
    }
}

