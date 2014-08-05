using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Trasys.Dev.Tools.Data;

namespace Trasys.PhotoTrack.Api.Models
{
    public class BaseFactory
    {
        protected Factory _factory = null;
        public BaseFactory(Factory factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// Gets a SqlDatabaseCommand object already configured with Factory's database connection.
        /// </summary>
        /// <returns>A SqlDatabaseCommand object.</returns>
        public SqlDatabaseCommand GetDatabaseCommand()
        {
            return _factory.GetDatabaseCommand();
        }

    }
}