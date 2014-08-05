using System;
using System.Collections.Generic;
using System.Text;

namespace Trasys.PhotoTrack.Mobile.Model.Entities
{
    public class Profile
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public int ProfileID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public int Type { get; set; }
        public int EnterpriseID { get; set; }
        public string EnterpriseName { get; set; }
    }
}
