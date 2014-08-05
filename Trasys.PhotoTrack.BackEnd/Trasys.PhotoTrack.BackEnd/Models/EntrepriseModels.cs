using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Trasys.PhotoTrack.BackEnd.Models
{
    public class EnterpriseModels
    {

        [HiddenInput(DisplayValue = false)]
        [Display(Name = "EnterpriseID")]
        public int EnterpriseID { get; set; }


        [DataType(DataType.Text)]
        [Display(Name = "Enterprise name")]
        public string Name { get; set; }
    }
}