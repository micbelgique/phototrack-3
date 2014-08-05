using System;
using System.Collections.Generic;
using System.Text;

namespace Trasys.PhotoTrack.Mobile.Model.Entities
{
    public class Site
    {
        public int SiteID { get; set; }
        public string Number { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string EnterpriseName { get; set; }
        public int EnterpriseID { get; set; }
        public string Status { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime PlannedDate { get; set; }
        public bool IsFavorite { get; set; }
    }
}
