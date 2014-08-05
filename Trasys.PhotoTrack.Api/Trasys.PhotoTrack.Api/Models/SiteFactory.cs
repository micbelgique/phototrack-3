using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Trasys.Dev.Tools.Data;

namespace Trasys.PhotoTrack.Api.Models
{
    public class SiteFactory : BaseFactory
    {
        protected string baseUrl = ""; // two definitions of baseurl, added at the last minute :-) not good :-)
        public SiteFactory(Factory factory)
            : base(factory)
        {
        }

        public SiteDetails GetSite(long id)
        {
            using (SqlDatabaseCommand cmd = this.GetDatabaseCommand())
            {
                cmd.CommandText.AppendLine("SELECT TOP 1 Site.[SiteID]                                                                 ");
                cmd.CommandText.AppendLine("    ,Site.[Latitude]                                                                       ");
                cmd.CommandText.AppendLine("    ,Site.[Longitude]                                                                      ");
                cmd.CommandText.AppendLine("    ,Site.[Number]                                                                         ");
                cmd.CommandText.AppendLine("    ,Site.[Adress]                                                                         ");
                cmd.CommandText.AppendLine("    ,Site.[Description]                                                                    ");
                cmd.CommandText.AppendLine("    ,ISNULL(Cast('2099-01-01' AS DateTime) ,PlannedDate) AS PlannedDate                                                                    ");
                cmd.CommandText.AppendLine("    ,Enterprise.Name AS EnterpriseName  ");
                cmd.CommandText.AppendLine("    ,Enterprise.EnterpriseID AS EnterpriseID                                                     ");
                cmd.CommandText.AppendLine("    ,Site.[Status]  FROM [Site]                                                            ");
                cmd.CommandText.AppendLine("    LEFT JOIN SiteEnterprise ON Site.SiteID = SiteEnterprise.SiteID                        ");
                cmd.CommandText.AppendLine("    LEFT JOIN Enterprise ON SiteEnterprise.EnterpriseID = Enterprise.EnterpriseID          ");
                cmd.CommandText.AppendLine("   WHERE Site.SiteID = @SiteID");

                cmd.Parameters.AddWithValue("@SiteID", id);

                SiteDetails siteFound = cmd.ExecuteRow<SiteDetails>();

                if(siteFound !=null)
                {
                    siteFound.Holders = GetHolders(siteFound.SiteID);
                }

                return siteFound;
            }

        }

        public bool CreateSite(Site site)
        {
            using (SqlDatabaseCommand cmd = this.GetDatabaseCommand())
            {
                cmd.CommandText.AppendLine("DECLARE @id DECIMAL    ");
                cmd.CommandText.AppendLine("IF NOT EXISTS (SELECT Number FROM [Site] WHERE SiteID = @SiteID) ");
                cmd.CommandText.AppendLine("BEGIN ");
                cmd.CommandText.AppendLine("INSERT INTO [Site] ");
                cmd.CommandText.AppendLine("            ([Latitude]              ");
                cmd.CommandText.AppendLine("            ,[Longitude]             ");
                cmd.CommandText.AppendLine("            ,[Number]                ");
                cmd.CommandText.AppendLine("            ,[Adress]                ");
                cmd.CommandText.AppendLine("            ,[Description]           ");
                cmd.CommandText.AppendLine("            ,[PlannedDate]           ");
                cmd.CommandText.AppendLine("            ,[Status])               ");
                cmd.CommandText.AppendLine("     VALUES                                      ");
                cmd.CommandText.AppendLine("            (@Latitude                          ");
                cmd.CommandText.AppendLine("            ,@Longitude                         ");
                cmd.CommandText.AppendLine("            ,@Number                            ");
                cmd.CommandText.AppendLine("            ,@Adress                            ");
                cmd.CommandText.AppendLine("            ,@Description                       ");
                cmd.CommandText.AppendLine("            ,@PlannedDate                       ");
                cmd.CommandText.AppendLine("            ,'NEW')  ;                          ");
                cmd.CommandText.AppendLine("    SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY]; ");
                cmd.CommandText.AppendLine("END ");
                cmd.CommandText.AppendLine("ELSE ");
                cmd.CommandText.AppendLine("BEGIN ");
                cmd.CommandText.AppendLine("UPDATE [Site] SET ");
                cmd.CommandText.AppendLine("    @id       = Site.SiteID          ");
                cmd.CommandText.AppendLine("    ,[Latitude]       = @Latitude          ");
                cmd.CommandText.AppendLine("    ,[Longitude]     = @Longitude         ");
                cmd.CommandText.AppendLine("    ,[Number]        = @Number            ");
                cmd.CommandText.AppendLine("    ,[Adress]        = @Adress            ");
                cmd.CommandText.AppendLine("    ,[PlannedDate]        = @PlannedDate            ");
                cmd.CommandText.AppendLine("    ,[Description]   = @Description       ");
                cmd.CommandText.AppendLine("    WHERE  Number = @Number; "); ;
                cmd.CommandText.AppendLine("		SELECT @Id AS [SCOPE_IDENTITY];  ");
                cmd.CommandText.AppendLine("END ");

                cmd.Parameters.AddWithValue("@Latitude", site.Latitude);
                cmd.Parameters.AddWithValue("@Longitude", site.Longitude);
                cmd.Parameters.AddWithValue("@Number", site.Number);
                cmd.Parameters.AddWithValue("@Adress", site.Address);
                cmd.Parameters.AddWithValue("@Description", site.Description);
                cmd.Parameters.AddWithValue("@PlannedDate", site.PlannedDate.Year < 1901 ? (object)DBNull.Value : site.PlannedDate);

                long siteID = Convert.ToInt64(cmd.ExecuteScalar<decimal>());

                if (cmd.Exception != null)
                {
                    return false;
                }

                if(!String.IsNullOrEmpty(site.EnterpriseName))
                {
                    cmd.Clear();
                    cmd.CommandText.AppendLine("DECLARE @id DECIMAL    ");
                    cmd.CommandText.AppendLine("IF NOT EXISTS (SELECT * FROM Enterprise WHERE Name = @Name)  ");
                    cmd.CommandText.AppendLine("    BEGIN    ");
                    cmd.CommandText.AppendLine("      INSERT INTO Enterprise (Name) VALUES (@Name); ");
                    cmd.CommandText.AppendLine("      SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY];");
                    cmd.CommandText.AppendLine("    END     ");
                    cmd.CommandText.AppendLine("ELSE        ");
                    cmd.CommandText.AppendLine("    BEGIN    ");
                    cmd.CommandText.AppendLine("       SELECT convert(decimal,EnterpriseID) FROM Enterprise WHERE Name = 'sample string 5';");
                    cmd.CommandText.AppendLine("    END    ");

                    cmd.Parameters.AddWithValue("@Name", site.EnterpriseName);
                    long enterpriseID = Convert.ToInt64(cmd.ExecuteScalar<decimal>());

                    //Create link
                    if(enterpriseID > 0 && siteID > 0)
                    {
                        cmd.Clear();
                        cmd.CommandText.AppendLine("IF NOT EXISTS (SELECT * FROM SiteEnterprise WHERE SiteID = @SiteID)  ");
                        cmd.CommandText.AppendLine("    BEGIN    ");
                        cmd.CommandText.AppendLine("      INSERT INTO SiteEnterprise (SiteID,EnterpriseID) VALUES (@SiteID,@EnterpriseID); ");
                        cmd.CommandText.AppendLine("    END     ");
                        cmd.CommandText.AppendLine("ELSE        ");
                        cmd.CommandText.AppendLine("    BEGIN    ");
                        cmd.CommandText.AppendLine("      UPDATE SiteEnterprise SET EnterpriseID = @EnterpriseID WHERE SiteID = @SiteID");
                        cmd.CommandText.AppendLine("    END    ");

                        cmd.Parameters.AddWithValue("@SiteID", siteID);
                        cmd.Parameters.AddWithValue("@EnterpriseID", enterpriseID);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            return true;
        }

        public Site[] GetSites(string search)
        {
            using (SqlDatabaseCommand cmd = this.GetDatabaseCommand())
            {
                cmd.CommandText.AppendLine("SELECT Site.[SiteID]                                                                 ");
                cmd.CommandText.AppendLine("    ,Site.[Latitude]                                                                       ");
                cmd.CommandText.AppendLine("    ,Site.[Longitude]                                                                      ");
                cmd.CommandText.AppendLine("    ,Site.[Number]                                                                         ");
                cmd.CommandText.AppendLine("    ,Site.[Adress]                                                                         ");
                cmd.CommandText.AppendLine("    ,Site.[Description]                                                                    ");
                cmd.CommandText.AppendLine("    ,ISNULL(Cast('2099-01-01' AS DateTime) ,PlannedDate) AS PlannedDate                                                                          ");
                cmd.CommandText.AppendLine("    ,Enterprise.Name AS EnterpriseName  ");
                cmd.CommandText.AppendLine("    ,Enterprise.EnterpriseID AS EnterpriseID  ");
                cmd.CommandText.AppendLine("    ,Site.[Status]  FROM [Site]                                                            ");
                cmd.CommandText.AppendLine("    LEFT JOIN SiteEnterprise ON Site.SiteID = SiteEnterprise.SiteID                        ");
                cmd.CommandText.AppendLine("    LEFT JOIN Enterprise ON SiteEnterprise.EnterpriseID = Enterprise.EnterpriseID          ");
                cmd.CommandText.AppendLine("   WHERE Site.Number LIKE @Search ");

                cmd.Parameters.AddWithValue("@Search", String.Concat('%', search, '%'));

                Site[] sitesFound = cmd.ExecuteTable<Site>();

                return sitesFound;
            }

        }

        public Site[] GetSites()
        {
            using (SqlDatabaseCommand cmd = this.GetDatabaseCommand())
            {
                cmd.CommandText.AppendLine("SELECT Site.[SiteID]                                                                 ");
                cmd.CommandText.AppendLine("    ,Site.[Latitude]                                                                       ");
                cmd.CommandText.AppendLine("    ,Site.[Longitude]                                                                      ");
                cmd.CommandText.AppendLine("    ,Site.[Number]                                                                         ");
                cmd.CommandText.AppendLine("    ,Site.[Adress]                                                                         ");
                cmd.CommandText.AppendLine("    ,Site.[Description]                                                                    ");
                cmd.CommandText.AppendLine("    ,ISNULL(Cast('2099-01-01' AS DateTime) ,PlannedDate) AS PlannedDate                                                                         ");
                cmd.CommandText.AppendLine("    ,Enterprise.Name AS EnterpriseName  ");
                cmd.CommandText.AppendLine("    ,Enterprise.EnterpriseID AS EnterpriseID  ");
                cmd.CommandText.AppendLine("    ,Site.[Status]  FROM [Site]                                                            ");
                cmd.CommandText.AppendLine("    LEFT JOIN SiteEnterprise ON Site.SiteID = SiteEnterprise.SiteID                        ");
                cmd.CommandText.AppendLine("    LEFT JOIN Enterprise ON SiteEnterprise.EnterpriseID = Enterprise.EnterpriseID          ");

                Site[] sitesFound = cmd.ExecuteTable<Site>();

                return sitesFound;
            }


        }

        public ProfileSummary[] GetHolders(long siteId)
        {
            using (SqlDatabaseCommand cmd = this.GetDatabaseCommand())
            {
                cmd.CommandText.AppendLine("SELECT                                      ");
                cmd.CommandText.AppendLine("    ProfileSite.ProfileID ,                                                             ");
                cmd.CommandText.AppendLine("    Profile.FirstName,                                                                  ");
                cmd.CommandText.AppendLine("    Profile.LastName FROM Profile                                                       ");
                cmd.CommandText.AppendLine("    INNER JOIN ProfileSite ON Profile.ProfileID = ProfileSite.ProfileID                 ");
                cmd.CommandText.AppendLine("    INNER JOIN Site ON Site.SiteID = ProfileSite.SiteID                                 ");
                cmd.CommandText.AppendLine("WHERE Site.SiteID = @SiteID                                                               ");
                cmd.Parameters.AddWithValue("@SiteID", siteId);

                ProfileSummary[] profilesFound = cmd.ExecuteTable<ProfileSummary>();

                return profilesFound;
            }
        }

        public Site[] GetSites(long profileID)
        {
            using (SqlDatabaseCommand cmd = this.GetDatabaseCommand())
            {
                cmd.CommandText.AppendLine("  SELECT Site.[SiteID]                                                                          ");
                cmd.CommandText.AppendLine("    ,Site.[Latitude]                                                                             ");
                cmd.CommandText.AppendLine("    ,Site.[Longitude]                                                                            ");
                cmd.CommandText.AppendLine("    ,Site.[Number]                                                                               ");
                cmd.CommandText.AppendLine("    ,Site.[Adress]                                                                               ");
                cmd.CommandText.AppendLine("    ,Site.[Description]                                                                          ");
                cmd.CommandText.AppendLine("    ,ISNULL(Cast('2099-01-01' AS DateTime) ,PlannedDate) AS PlannedDate                                                                        ");
                cmd.CommandText.AppendLine("    ,Enterprise.Name AS EnterpriseName                                                           ");
                cmd.CommandText.AppendLine("    ,Enterprise.EnterpriseID AS EnterpriseID                                                     ");
                cmd.CommandText.AppendLine("    ,Site.[Status]  FROM [Site]                                                                  ");
                cmd.CommandText.AppendLine(" LEFT JOIN SiteEnterprise ON Site.SiteID = SiteEnterprise.SiteID                              ");
                cmd.CommandText.AppendLine(" LEFT JOIN Enterprise ON SiteEnterprise.EnterpriseID = Enterprise.EnterpriseID                ");
	            cmd.CommandText.AppendLine(" INNER JOIN ProfileSite ON ProfileSite.SiteID = Site.SiteID                                     ");
                cmd.CommandText.AppendLine("WHERE ProfileSite.ProfileID = @ProfileID                                                               ");

                cmd.Parameters.AddWithValue("@ProfileID", profileID);

                Site[] sitesFound = cmd.ExecuteTable<Site>();

                return sitesFound;
            }

        }

        public bool AddPhoto(long siteID,Guid photo,string[] tags,DateTime timestamp)
        {
            using (SqlDatabaseCommand cmd = this.GetDatabaseCommand())
            {
                cmd.CommandText.AppendLine(" INSERT INTO [Photo] ");
                cmd.CommandText.AppendLine("    ([Timestamp]                ");
                cmd.CommandText.AppendLine("       ,[FileName])             ");
                cmd.CommandText.AppendLine(" VALUES                         ");
                cmd.CommandText.AppendLine("       (@TimeStamp,               ");
                cmd.CommandText.AppendLine("       @FileName);               ");
                cmd.CommandText.AppendLine(" SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY]; ");
                cmd.Parameters.AddWithValue("@FileName", photo);
                cmd.Parameters.AddWithValue("@TimeStamp", timestamp);

                long photoID = Convert.ToInt64(cmd.ExecuteScalar<decimal>());

                if(photoID > 0)
                {
                    //Create the link
                    cmd.CommandText.AppendLine(" INSERT INTO [SitePhoto]        ");
                    cmd.CommandText.AppendLine("       ([PhotoID]               ");
                    cmd.CommandText.AppendLine("       ,[SiteID])               ");
                    cmd.CommandText.AppendLine(" VALUES                         ");
                    cmd.CommandText.AppendLine("       (@PhotoID                ");
                    cmd.CommandText.AppendLine("       ,@SiteID)                ");
                    cmd.Parameters.AddWithValue("@PhotoID", photoID);
                    cmd.Parameters.AddWithValue("@SiteID", siteID);

                    cmd.ExecuteNonQuery();


                    //Create tags
                    foreach(string tag in tags)
                    {

                        decimal id = InsertTag(cmd, tag);

                        //Create link
                        if (id > 0)
                        {
                            CreateTagLink(cmd, photoID, id);
                        }
                    }
                    

                }


            }
            return false;
        }

        public bool UpdateTag(long siteID, string photoID, string newTag)
        {
            using (SqlDatabaseCommand cmd = this.GetDatabaseCommand())
            {
                cmd.CommandText.AppendLine("DELETE FROM PhotoTag WHERE PhotoID = (SELECT TOP 1 PhotoID FROM Photo WHERE FileName = @PhotoID) ");
                cmd.Parameters.AddWithValue("@PhotoID", photoID);
                    cmd.ExecuteNonQuery();
                //Create tags
                decimal id = InsertTag(cmd, newTag);

                //Create link
                if (id > 0)
                {
                    CreateTagLink(cmd, photoID, id);
                }
            }
            return true;
        }

        public Photo[] GetPhotos(long siteID)
        {
            List<Photo> photos = new List<Photo>();
            using (SqlDatabaseCommand cmd = this.GetDatabaseCommand())
            {                                                                                                        
                cmd.CommandText.AppendLine("  SELECT FileName,TimeStamp,Tag.Name FROM Photo                          ");                
                cmd.CommandText.AppendLine("    INNER JOIN SitePhoto ON Photo.PhotoID = SitePhoto.PhotoID               ");
                cmd.CommandText.AppendLine("    LEFT JOIN PhotoTag ON Photo.PhotoID = PhotoTag.PhotoID                  ");
                cmd.CommandText.AppendLine("    LEFT JOIN Tag ON PhotoTag.TagID = Tag.TagID                             ");
                cmd.CommandText.AppendLine("  WHERE SitePhoto.SiteID = @SiteID                             ");
                cmd.Parameters.AddWithValue("@SiteID", siteID);                                                                                                                   

                DataTable results = cmd.ExecuteTable();

                foreach(DataRow row in results.Rows)
                {
                    Photo photo = new Photo();
                    photo.ID = Convert.ToString(row["FileName"]).ToLower();
                    photo.Url = baseUrl + Convert.ToString(row["FileName"]).ToLower() + ".jpg";
                    photo.Timestamp = Convert.ToDateTime(row["Timestamp"]);
                    photo.Tag = Convert.ToString(row["Name"]);
                    var photoFound = photos.FirstOrDefault( p => p.ID == photo.ID);
                    if(photoFound == null)
                    {
                        // Photo has not been added yet
                         photos.Add(photo);
                    }
                    else
                    {
                        photoFound.Tag = String.Concat(",",photoFound.Tag); 
                    }
                   
                }

                return photos.ToArray();
            }
        }

        public bool AssignSite(long enterpriseID,long siteID)
        {
            using (SqlDatabaseCommand cmd = this.GetDatabaseCommand())
            {
                cmd.CommandText.AppendLine("  IF NOT EXISTS (SELECT EnterpriseID FROM SiteEnterprise WHERE SiteEnterprise.SiteID = @SiteID AND  SiteEnterprise.EnterpriseID = @EnterpriseID)                     ");
                cmd.CommandText.AppendLine("    BEGIN ");
                cmd.CommandText.AppendLine("        INSERT INTO SiteEnterprise (EnterpriseID, SiteID) VALUES (@EnterpriseID, @SiteID) ");
                cmd.CommandText.AppendLine("    END ");
                cmd.Parameters.AddWithValue("@SiteID", siteID);
                cmd.Parameters.AddWithValue("@EnterpriseID", enterpriseID);

                cmd.ExecuteNonQuery();

               if(cmd.Exception != null)
               {
                   return false;
               }
            }
            return true;
        }

        private decimal InsertTag(SqlDatabaseCommand cmd,string tag)
        {
            cmd.Clear();

            cmd.CommandText.AppendLine("IF NOT EXISTS (SELECT * FROM Tag WHERE Name = @Name) ");
            cmd.CommandText.AppendLine("  BEGIN ");
            cmd.CommandText.AppendLine("    INSERT INTO [Tag]                  ");
            cmd.CommandText.AppendLine("            ([Name])                ");
            cmd.CommandText.AppendLine("      VALUES                        ");
            cmd.CommandText.AppendLine("            (@Name);              ");
            cmd.CommandText.AppendLine("  SELECT SCOPE_IDENTITY() AS TagID  ");
            cmd.CommandText.AppendLine(" END ");
            cmd.CommandText.AppendLine(" ELSE ");
            cmd.CommandText.AppendLine(" BEGIN ");
            cmd.CommandText.AppendLine("    SELECT TagID FROM Tag WHERE Name = @Name ");
            cmd.CommandText.AppendLine(" END ");
            cmd.Parameters.AddWithValue("@Name", tag);

            DataRow row = cmd.ExecuteRow();
            long id = Convert.ToInt64(row["TagID"]);
            return id;
        }

        private void CreateTagLink(SqlDatabaseCommand cmd, long photoID, decimal tagId)
        {
            cmd.Clear();

            cmd.CommandText.AppendLine(" INSERT INTO [PhotoTag]       ");
            cmd.CommandText.AppendLine("       ([PhotoID]             ");
            cmd.CommandText.AppendLine("       ,[TagID])              ");
            cmd.CommandText.AppendLine(" VALUES                       ");
            cmd.CommandText.AppendLine("       (@PhotoID                  ");
            cmd.CommandText.AppendLine("       ,@TagID)                 ");


            cmd.Parameters.AddWithValue("@PhotoID", photoID);
            cmd.Parameters.AddWithValue("@TagID", tagId);
            cmd.ExecuteNonQuery();
        }

        private void CreateTagLink(SqlDatabaseCommand cmd, string photoID, decimal tagId)
        {
            cmd.Clear();
            cmd.CommandText.AppendLine("SELECT PhotoID FROM Photo WHERE FileName = @PhotoID");
            cmd.Parameters.AddWithValue("@PhotoID", photoID);
            DataRow row = cmd.ExecuteRow();
            long id = Convert.ToInt64(row["PhotoID"]);
            cmd.Clear();

            cmd.CommandText.AppendLine(" INSERT INTO [PhotoTag]       ");
            cmd.CommandText.AppendLine("       ([PhotoID]             ");
            cmd.CommandText.AppendLine("       ,[TagID])              ");
            cmd.CommandText.AppendLine(" VALUES                       ");
            cmd.CommandText.AppendLine("       (@PhotoID                  ");
            cmd.CommandText.AppendLine("       ,@TagID)                 ");


            cmd.Parameters.AddWithValue("@PhotoID", id);
            cmd.Parameters.AddWithValue("@TagID", tagId);
            cmd.ExecuteNonQuery();
        }

    }
}