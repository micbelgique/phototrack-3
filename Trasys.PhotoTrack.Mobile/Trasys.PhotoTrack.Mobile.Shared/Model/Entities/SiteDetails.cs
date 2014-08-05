using System;
using System.Collections.Generic;
using System.Text;

namespace Trasys.PhotoTrack.Mobile.Model.Entities
{
    public class SiteDetails
    {
        public List<ProfileSummary> Holders { get; set; }
        public int SiteID { get; set; }
        public string Number { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string EnterpriseName { get; set; }
        public int EnterpriseID { get; set; }
        public string Status { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public Site ToSite()
        {
            return new Site()
            {
                SiteID = this.SiteID,
                Number = this.Number,
                Address = this.Address,
                Description = this.Description,
                EnterpriseName = this.EnterpriseName,
                EnterpriseID = this.EnterpriseID,
                Status = this.Status,
                Latitude = this.Latitude,
                Longitude = this.Longitude
            };
        }
    }
}
