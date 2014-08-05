using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trasys.PhotoTrack.BackEnd.Models
{
    public class PhotosModels
    {

        public string ID { get; set; }
        public string Url { get; set; }
        public string Tag { get; set; }
        public DateTime Timestamp { get; set; }
    }
}