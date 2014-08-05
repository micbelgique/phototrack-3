using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

/// <summary>
/// Extension methods to System.Data
/// </summary>
public static class DataExtension
{
    /// <summary>
    /// Returns the value formatted for SQL request (ex. ABC => 'ABC', 12/01/1972 => '1972-01-12', ...)
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public static string GetFormattedValue(this DbParameter parameter)
    {
        if (parameter.Value == DBNull.Value)
        {
            return "NULL";
        }
        else
        {
            switch (parameter.DbType)
            {
                // String
                case System.Data.DbType.AnsiString:
                case System.Data.DbType.AnsiStringFixedLength:
                case System.Data.DbType.String:
                case System.Data.DbType.StringFixedLength:
                    return String.Format("'{0}'", Convert.ToString(parameter.Value).Replace("'", "''"));

                // Boolean
                case System.Data.DbType.Boolean:
                    return Convert.ToBoolean(parameter.Value) == true ? "1" : "0";

                // Integer
                case System.Data.DbType.Byte:
                case System.Data.DbType.SByte:
                case System.Data.DbType.Int16:
                case System.Data.DbType.UInt16:
                case System.Data.DbType.Int32:
                case System.Data.DbType.UInt32:
                case System.Data.DbType.Int64:
                case System.Data.DbType.UInt64:
                    return Convert.ToString(parameter.Value);

                // Numeric
                case System.Data.DbType.Decimal:
                case System.Data.DbType.Double:
                case System.Data.DbType.Currency:
                case System.Data.DbType.Single:
                case System.Data.DbType.VarNumeric:
                    return String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0.0#####}", parameter.Value);

                // Binary
                case System.Data.DbType.Binary:
                case System.Data.DbType.Object:
                    return "'BINARY_DATA'";

                // Date
                case System.Data.DbType.Date:
                case System.Data.DbType.DateTime:
                case System.Data.DbType.DateTime2:
                case System.Data.DbType.DateTimeOffset:
                case System.Data.DbType.Time:

                    DateTime value = Convert.ToDateTime(parameter.Value);

                    if (value.Hour == 0 && value.Minute == 0 && value.Second == 0 && value.Millisecond == 0)
                    {
                        return String.Format("'{0:yyyy-MM-dd}'", value);
                    }
                    else
                    {
                        if (value.Millisecond == 0)
                        {
                            return String.Format("'{0:yyyy-MM-dd HH:mm:ss}'", value);
                        }
                        else
                        {
                            return String.Format("'{0:yyyy-MM-dd HH:mm:ss.ffff}'", value);
                        }
                    }
                
                // GUID
                case System.Data.DbType.Guid:
                    return String.Format("'{{{0}}}'", parameter.Value).ToUpper();
                
                // XML
                case System.Data.DbType.Xml:
                    return String.Format("'{0}'", parameter.Value);

                default:
                    return String.Empty;
            }
        }
    }

}

