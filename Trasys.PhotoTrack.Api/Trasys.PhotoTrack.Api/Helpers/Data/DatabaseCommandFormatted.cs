using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Trasys.Dev.Tools.Data
{
    /// <summary>
    /// Class to format the database command text (including parameters, in html, ...)
    /// </summary>
    public class DatabaseCommandFormatted
    {
        private DbCommand _command = null;

        /// <summary>
        /// Initializes a new instance of DatabaseCommandFormatted
        /// </summary>
        /// <param name="command"></param>
        internal DatabaseCommandFormatted(DbCommand command)
        {
            _command = command;
        }

        /// <summary>
        /// Gets the CommandText where all @Params are replaced by values.
        /// </summary>
        public string CommandAsText
        {
            get
            {
                return GetSqlFormatted(Formats.Text);
            }
        }

        /// <summary>
        /// Gets the CommandText where all @Params are replaced by values and keywords are colorized.
        /// </summary>
        public string CommandAsHtml
        {
            get
            {
                return GetSqlFormatted(Formats.Html);
            }
        }

        /// <summary>
        /// Gets the CommandText formatted with specified format
        /// </summary>
        /// <param name="format">Kinds of SQL formats</param>
        /// <returns>Formatted Command Text</returns>
        public string GetSqlFormatted(Formats format)
        {
            switch (format)
            {
                // Returns the SQL Command Text included parameters values
                case Formats.Text:
                    return GetCommandWithParameters(_command);

                // Format the SQL command in HTML
                case Formats.Html:
                    return FormatAsHtml(GetCommandWithParameters(_command));
                    
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Returns the CommandText with all parameter values.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        private string GetCommandWithParameters(DbCommand command)
        {
            string commandText = command.CommandText;

            foreach (DbParameter param in command.Parameters.Cast<DbParameter>().OrderByDescending(i => i.ParameterName))
            {
                commandText = commandText.Replace(param.ParameterName, param.GetFormattedValue());
            }

            return commandText;
        }

        /// <summary>
        /// Format the SQL command in HTML (coloring, ...)
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        private string FormatAsHtml(string commandText)
        {
            string output = HttpUtility.HtmlEncode(commandText).Replace("&#39;", "'");

            // Comments clorized
            output = Regex.Replace(output,
                @"^--(?<comment>[^\r\n]*)(?<post>\r\n|$)",
                @"<span style=""color: Olive; font-weight: bold;"">--${comment}</span>${post}",
                RegexOptions.IgnoreCase | RegexOptions.Multiline
            );

            // Keywords on new line
            output = Regex.Replace(output,
                @"(?<=(\[|\b))(?<keyword>(SELECT|FROM|WHERE|ORDER|INNER|LEFT|RIGHT|CROSS|GROUP|GO|CASE|WHEN|ELSE|IF|BEGIN|END))\b",
                @"<br/>${keyword}",
                RegexOptions.IgnoreCase
            );

            // Keywords clorized
            output = Regex.Replace(output,
                @"(?<=(\[|\b))(?<keyword>(SELECT|FROM|WHERE|ORDER|INNER|JOIN|OUTER|LEFT|RIGHT|CROSS" +
                    @"|DISTINCT|DECLARE|SET|EXEC|NOT|IN|IS|NULL|BETWEEN|GROUP|BY|ASC|DESC|OVER|AS|ON" +
                    @"|AND|OR|TOP|GO|CASE|WHEN|ELSE|THEN|IF|BEGIN|END|LIKE))\b",
                @"<span style=""color: #33f; font-weight: bold;"">${keyword}</span>",
                RegexOptions.IgnoreCase
            );

            // Other keywords clorized
            output = Regex.Replace(output,
                @"(\b(?<keyword>ROW_NUMBER|COUNT|CONVERT|COALESCE|CAST)(?<post>\())",
                @"<span style=""color: #3f6; font-weight: bold;"">${keyword}</span>${post}",
                RegexOptions.IgnoreCase
            );

            // Parameters (@xxx)
            output = Regex.Replace(output,
                @"(?<param>\@[\w\d_]+)",
                @"<span style=""color: #993; font-weight: bold;"">${param}</span>",
                RegexOptions.IgnoreCase
            );

            // Numerics
            output = Regex.Replace(output,
                @"(?<arg>(\b|-)[\d.]+\b)",
                @"<span style=""color: #FF3F00;"">${arg}</span>",
                RegexOptions.IgnoreCase
            );

            // Strings, Dates
            output = Regex.Replace(output,
                @"(?<arg>'([^']|'')*')",
                @"<span style=""color: #FF3F00;"">${arg}</span>",
                RegexOptions.IgnoreCase
            );

            return output;
        }
    }

    /// <summary>
    /// Kinds of SQL formats
    /// </summary>
    public enum Formats
    { 
        /// <summary>
        /// SQL Command Text included parameters values
        /// </summary>
        Text,

        /// <summary>
        /// SQL Command Text formatted in HTML for coloring, ...
        /// </summary>
        Html
    }
}
