using System;
using System.Collections.Generic;
using System.Text;

namespace Trasys.PhotoTrack.Mobile.Model.Entities
{
    public class Photo
    {
        public string ID { get; set; }
        public string Url { get; set; }
        public string Tag { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
