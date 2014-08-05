using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Trasys.Dev.Tools.Data;

namespace Trasys.PhotoTrack.Api.Models
{
    public class Factory
    {
        #region DECLARATIONS

        private string _connectionString = string.Empty;
        private static SqlConnection _connection = null;

        #endregion //DECLARATIONS

        #region CONSTRUCTORS

        public Factory()
        {
            if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["ConnectionString"]))
            {
                this._connectionString = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
                if (_connection == null)
                    _connection = new SqlConnection(_connectionString);
            }
            else
            {
                throw new NullReferenceException("Connection is null");
            }
        }

        #endregion

        #region METHODS

        public DashboardSummary GetDashboardSummary()
        {
            DashboardSummary summary = new DashboardSummary();
            using (SqlDatabaseCommand cmd = this.GetDatabaseCommand())
            {
                cmd.CommandText.AppendLine("SELECT Count(*) FROM Site WHERE Status = 'CLSD' ");
                summary.CounterClosedSites = cmd.ExecuteScalar<int>();

                cmd.Clear();
                cmd.CommandText.AppendLine("SELECT Count(*) FROM Site WHERE Status = 'PRG' ");
                summary.CounterInProgressSites = cmd.ExecuteScalar<int>();

                cmd.Clear();
                cmd.CommandText.AppendLine("SELECT Count(*) FROM Site WHERE Status = 'NEW' ");
                summary.CounterNewSites = cmd.ExecuteScalar<int>();
            }

            return summary;
        }

        #region SECURITY

        public ProfileAuthentication Login(string login, string password)
        {
            ProfileAuthentication profileLogged = new ProfileAuthentication();
            using (SqlDatabaseCommand cmd = this.GetDatabaseCommand())
            {
                cmd.CommandText.AppendLine("SELECT [ProfileID] ");
                cmd.CommandText.AppendLine(",[LastName]                           ");
                cmd.CommandText.AppendLine(",[FirstName]                          ");
                cmd.CommandText.AppendLine(",[Type]                               ");
                cmd.CommandText.AppendLine(",[Login]                              ");
                cmd.CommandText.AppendLine(",[Password]                              ");
                cmd.CommandText.AppendLine(",Enterprise.Name as EnterpriseName                                     ");
                cmd.CommandText.AppendLine(",[Email] FROM Profile  ");
                cmd.CommandText.AppendLine("LEFT JOIN Enterprise ON Profile.EnterpriseID = Enterprise.EnterpriseID");
                cmd.CommandText.AppendLine(" WHERE Login=@Login ");
                cmd.Parameters.AddWithValue("@Login", login);

                Profile profileFound = cmd.ExecuteRow<Profile>();

                if (profileFound == null)
                {
                    profileLogged.AuthenticationResult = AuthenticationResult.ProfileNotFound;
                }
                else
                {
                    profileLogged.ProfileLogged = profileFound;
                    if (profileFound.Password != password)
                    {
                        profileLogged.AuthenticationResult = AuthenticationResult.BadCredentials;
                    }
                    else
                    {
                        // Auth success
                        Guid? token = CreateToken(profileFound.ProfileID);
                        if (!token.HasValue)
                        {
                            profileLogged.AuthenticationResult = AuthenticationResult.Unknown;
                        }
                        else
                        {
                            profileLogged.Token = token.Value;
                        }
                    }
                }
            }
            return profileLogged;
        }

        private Guid? CreateToken(long profileID)
        {
            Guid token = Guid.NewGuid();
            using (SqlDatabaseCommand cmd = this.GetDatabaseCommand())
            {
                cmd.CommandText.AppendLine("INSERT INTO [Token] VALUES (@ProfileID,@Token) ");
                cmd.Parameters.AddWithValue("@ProfileID", profileID);
                cmd.Parameters.AddWithValue("@Token", token);

                cmd.ExecuteNonQuery();

                if(cmd.Exception != null)
                {
                    return null;
                }
            }
            return token;
        }

        /// <summary>
        /// Check if the supplied token is correct and returns the profile id associated with it
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public long? CheckToken(Guid token)
        {
            long profileID = -1;
            using (SqlDatabaseCommand cmd = this.GetDatabaseCommand())
            {
                cmd.CommandText.AppendLine("SELECT TOP 1 ProfileID FROM [Token] WHERE Value = @Token "); //ProfileID = (SELECT ProfileID FROM Profile WHERE Login = @Login)
                cmd.Parameters.AddWithValue("@Token", token);
                DataRow rowProfile = cmd.ExecuteRow();

                if (rowProfile == null)
                    return null;

                if(!long.TryParse(Convert.ToString(rowProfile["ProfileID"]),out profileID))
                {
                    return null;
                }

                if (profileID < 0)
                {
                    return null;
                }
            }

            return profileID;
        }

        #endregion

        #region Tools

        /// <summary>
        /// Gets a SqlDatabaseCommand object already configured with Factory's database connection.
        /// </summary>
        /// <returns>A SqlDatabaseCommand object.</returns>
        public SqlDatabaseCommand GetDatabaseCommand()
        {
            return new SqlDatabaseCommand(GetOpenConnection());
        }

        /// <summary>
        /// Returns a sql connection, if the connexion is not open, the method will open it
        /// </summary>
        /// <returns></returns>
        public SqlConnection GetOpenConnection()
        {
            if (_connection == null)
                throw new NullReferenceException("Connection is null");

            //Open the connection 
            if (_connection.State == System.Data.ConnectionState.Closed)
            {
                _connection.Open();
            }

            return _connection;
        }

        #endregion

        #endregion
    }
}
