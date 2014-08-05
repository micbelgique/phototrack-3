using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using Trasys.Dev.Tools.Data.Annotations;

namespace Trasys.PhotoTrack.Api.Models
{
    public class Profile
    {
        #region PROPERTIES

        ///<summary>
        /// Column Login
        ///</summary>
        [DataMember]
        [Column("Login")]
        public virtual string Login
        {
            get;
            set;
        }

        ///<summary>
        /// Column Password
        ///</summary>
        [DataMember]
        [Column("Password")]
        public virtual string Password
        {
            get;
            set;
        }

        ///<summary>
        /// Column ProfileID
        ///</summary>
        [DataMember]
        [Column("ProfileID")]
        public virtual long ProfileID
        {
            get;
            set;
        }

        ///<summary>
        /// Column LastName
        ///</summary>
        [DataMember]
        [Column("LastName")]
        public virtual string LastName
        {
            get;
            set;
        }

        ///<summary>
        /// Column FirstName
        ///</summary>
        [DataMember]
        [Column("FirstName")]
        public virtual string FirstName
        {
            get;
            set;
        }

        ///<summary>
        /// Column IsEnabled
        ///</summary>
        [DataMember]
        [Column("Email")]
        public virtual string Email { get; set; }

        [DataMember]
        [Column("Type")]
        public int Type { get; set; }

        [DataMember]
        [Column("EnterpriseID")]
        public int EnterpriseID { get; set; }

        [DataMember]
        [Column("EnterpriseName")]
        public string EnterpriseName { get; set; }

        #endregion //PROPERTIES
    }
}