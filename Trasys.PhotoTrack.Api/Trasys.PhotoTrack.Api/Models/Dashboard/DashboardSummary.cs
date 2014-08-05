using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Trasys.PhotoTrack.Api.Models
{
    [DataContract]
    public class DashboardSummary
    {
        [DataMember]
        public int CounterNewSites { get; set; }
         [DataMember]
        public int CounterInProgressSites { get; set; }
         [DataMember]
        public int CounterClosedSites { get; set; }
    }
}
