using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Trasys.Dev.Tools.Data;

namespace Trasys.PhotoTrack.Api.Models
{
    public class ProfileFactory : BaseFactory
    {
        public ProfileFactory(Factory factory) : base(factory)
        {
        }

        public Profile[] GetProfiles(string search)
        {
            using (SqlDatabaseCommand cmd = this.GetDatabaseCommand())
            {
                cmd.CommandText.AppendLine("SELECT [ProfileID] ");
                cmd.CommandText.AppendLine("    ,[LastName]                           ");
                cmd.CommandText.AppendLine("    ,[FirstName]                          ");
                cmd.CommandText.AppendLine("    ,[Type]                               ");
                cmd.CommandText.AppendLine("    ,[Login]                              ");
                cmd.CommandText.AppendLine("    ,Enterprise.Name as EnterpriseName                                     ");
                cmd.CommandText.AppendLine("    ,[Email] FROM Profile  ");
                cmd.CommandText.AppendLine("LEFT JOIN Enterprise ON Profile.EnterpriseID = Enterprise.EnterpriseID");
                cmd.CommandText.AppendLine(" WHERE ");
                cmd.CommandText.AppendLine("    FirstName LIKE @Search OR LastName LIKE @Search OR Email LIKE @Search");

                cmd.Parameters.AddWithValue("@Search",String.Concat('%',search,'%'));

                Profile[] profilesFound = cmd.ExecuteTable<Profile>();

                return profilesFound;
            }

        }

        public Profile GetProfile(long profileID)
        {
            using (SqlDatabaseCommand cmd = this.GetDatabaseCommand())
            {
                cmd.CommandText.AppendLine("SELECT [ProfileID] ");
                cmd.CommandText.AppendLine("    ,[LastName]                           ");
                cmd.CommandText.AppendLine("    ,[FirstName]                          ");
                cmd.CommandText.AppendLine("    ,[Type]                               ");
                cmd.CommandText.AppendLine("    ,[Login]                              ");
                cmd.CommandText.AppendLine("    ,Enterprise.Name as EnterpriseName                                     ");
                cmd.CommandText.AppendLine("    ,[Email] FROM Profile  ");
                cmd.CommandText.AppendLine("LEFT JOIN Enterprise ON Profile.EnterpriseID = Enterprise.EnterpriseID");
                cmd.CommandText.AppendLine(" WHERE ");
                cmd.CommandText.AppendLine("    Profile.ProfileID = @ProfileID");

                cmd.Parameters.AddWithValue("@ProfileID",profileID );

                Profile profilesFound = cmd.ExecuteRow<Profile>();

                return profilesFound;
            }

        }

        public Profile[] GetProfiles()
        {
            using (SqlDatabaseCommand cmd = this.GetDatabaseCommand())
            {
                cmd.CommandText.AppendLine("SELECT [ProfileID] ");
                cmd.CommandText.AppendLine("    ,[LastName]                           ");
                cmd.CommandText.AppendLine("    ,[FirstName]                          ");
                cmd.CommandText.AppendLine("    ,[Type]                               ");
                cmd.CommandText.AppendLine("    ,[Login]                              ");
                cmd.CommandText.AppendLine("    ,Enterprise.Name as EnterpriseName                                     ");
                cmd.CommandText.AppendLine("    ,[Email] FROM Profile  ");
                cmd.CommandText.AppendLine("LEFT JOIN Enterprise ON Profile.EnterpriseID = Enterprise.EnterpriseID");

                Profile[] profilesFound = cmd.ExecuteTable<Profile>();

                return profilesFound;
            }

        }

        public bool CreateProfile(Profile profile)
        {
            using (SqlDatabaseCommand cmd = this.GetDatabaseCommand())
            {
                cmd.CommandText.AppendLine("INSERT INTO [Profile]                            ");
                cmd.CommandText.AppendLine("           ([LastName]                           ");
                cmd.CommandText.AppendLine("           ,[FirstName]                          ");
                cmd.CommandText.AppendLine("           ,[Type]                               ");
                cmd.CommandText.AppendLine("           ,[Login]                              ");
                cmd.CommandText.AppendLine("           ,[Password]                           ");
                cmd.CommandText.AppendLine("           ,[Email])                             ");
                cmd.CommandText.AppendLine("     VALUES                                      ");
                cmd.CommandText.AppendLine("           (@LastName                            ");
                cmd.CommandText.AppendLine("           ,@FirstName                            ");
                cmd.CommandText.AppendLine("           ,@Type                                    ");
                cmd.CommandText.AppendLine("           ,@Login                               ");
                cmd.CommandText.AppendLine("           ,@Password                               ");
                cmd.CommandText.AppendLine("           ,@Email)       ");

                cmd.Parameters.AddWithValue("@LastName", profile.LastName);
                cmd.Parameters.AddWithValue("@FirstName", profile.FirstName);
                cmd.Parameters.AddWithValue("@Type", profile.Type);
                cmd.Parameters.AddWithValue("@Login", profile.Login);
                cmd.Parameters.AddWithValue("@Password", profile.Password);
                cmd.Parameters.AddWithValue("@Email", profile.Email);

                cmd.ExecuteNonQuery();

                if(cmd.Exception != null)
                {
                    return false;
                }
            }
            return true;
        }

        public bool DeleteProfile(long profileID)
        {
            using (SqlDatabaseCommand cmd = this.GetDatabaseCommand())
            {
                cmd.CommandText.AppendLine("DELETE [Profile] WHERE ProfileID = @ProfileID                           ");
                cmd.Parameters.AddWithValue("@ProfileID", profileID);

                cmd.ExecuteNonQuery();

                if (cmd.Exception != null)
                {
                    return false;
                }
            }
            return true;
        }

    }
}