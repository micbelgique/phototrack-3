using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trasys.PhotoTrack.BackEnd.Models
{
    public class DashboardModel
    {

        public int CounterNewSites { get; set; }
        public int CounterInProgressSites { get; set; }
        public int CounterClosedSites { get; set; }
    }
}