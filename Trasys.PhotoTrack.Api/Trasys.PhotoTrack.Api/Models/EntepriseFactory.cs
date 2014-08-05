using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Trasys.Dev.Tools.Data;

namespace Trasys.PhotoTrack.Api.Models
{
    public class EnterpriseFactory : BaseFactory
    {
        public EnterpriseFactory(Factory factory)
            : base(factory)
        {
        }

        public Enterprise[] GetEnterprises()
        {
            using (SqlDatabaseCommand cmd = this.GetDatabaseCommand())
            {
                cmd.CommandText.AppendLine("SELECT EnterpriseID,Name                                                               ");
                cmd.CommandText.AppendLine("    FROM [Enterprise]                                                            ");

                Enterprise[] enterprisesFound = cmd.ExecuteTable<Enterprise>();

                return enterprisesFound;
            }


        }
    }
}