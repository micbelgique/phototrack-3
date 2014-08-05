using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Trasys.PhotoTrack.BackEnd.Models
{
    public class SiteModels
    {
        #region PROPERTIES

        [Required]
        [HiddenInput(DisplayValue = false)]
        [Display(Name = "Site Identification")]
        public string SiteID { get; set; }


        [DataType(DataType.Text)]
        [Display(Name = "Latitude")]
        public decimal Latitude { get; set; }


        [DataType(DataType.Text)]
        [Display(Name = "Longitude")]
        public decimal Longitude { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Reference number")]
        public string Number { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Address")]
        public string Address { get; set; }


        [DataType(DataType.Text)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [HiddenInput(DisplayValue = false)]
        [Display(Name = "Status")]
        public string Status { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Status description")]
        public string StatusDescription
        {
            get
            {
                switch (Status)
                {
                    case "NEW":
                        return "New";
                    case "CLSD":
                        return "Closed";
                    case "PRG":
                        return "In Progress";
                    default:
                        return "Error";
                }
            }
        }

        [DataType(DataType.Text)]
        [Display(Name = "Enterprise name")]
        public string EnterpriseName { get; set; }

        [HiddenInput(DisplayValue = false)]
        [Display(Name = "Enterprise id")]
        public long EnterpriseID
        {
            get
            {
                if (EnterpriseStringID != null)
                {
                    return long.Parse(EnterpriseStringID.ToString());
                }
                return 0;
            }
            set
            {
                EnterpriseStringID = value.ToString();
            }
        }

        [Display(Name = "Enterprise string id")]
        public string EnterpriseStringID
        {
            get;
            set;
        }


        [DataType(DataType.DateTime)]
        [Display(Name = "Planned date")]
        public DateTime PlannedDate { get; set; }


        #endregion



    }
}