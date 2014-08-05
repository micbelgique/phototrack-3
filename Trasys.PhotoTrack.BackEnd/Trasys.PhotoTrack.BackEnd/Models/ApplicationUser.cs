using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;

namespace Trasys.PhotoTrack.BackEnd.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }


        [DataType(DataType.Text)]
        [Display(Name = "Login")]
        public string Login { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password Confirm")]
        public string PasswordConfirm { get; set; }


        [HiddenInput(DisplayValue = false)]
        [Display(Name = "Profil Id")]
        public int ProfileID { get; set; }


        [DataType(DataType.Text)]
        [Display(Name = "Lastname")]
        public string LastName { get; set; }


        [DataType(DataType.Text)]
        [Display(Name = "Firstname")]
        public string FirstName { get; set; }


        [Display(Name = "Type")]
        public int Type { get; set; }


        [Display(Name = "TypeStringed")]
        public string TypeString
        {
            get
            {
                return Type.ToString();
            }
            set
            {
                Type = int.Parse(value);
            }
        }


        public string Token { get; set; }


        [DataType(DataType.Text)]
        [Display(Name = "Fullname")]
        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", FirstName, LastName);
            }
        }


        [DataType(DataType.Text)]
        [Display(Name = "Type description")]
        public string TypeDescription
        {
            get
            {
                switch (Type)
                {
                    case 0: return "Administrator";
                    case 1: return "Contractor";
                    default: return "Worker";
                }
            }
        }
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


        [DataType(DataType.Text)]
        [Display(Name = "Enterprise")]
        public string EnterpriseName { get; set; }

    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}