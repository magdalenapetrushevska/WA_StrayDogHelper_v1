using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WA_StrayDogHelper_v1.Models.DomainModels;

namespace WA_StrayDogHelper_v1.Models.IdentityModels
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }





        public virtual ICollection<Dog> Dogs { get; set; }

        public List<FavoriteDog> FavoriteDogs { get; set; }
        public List<FavoriteArticle> FavoriteArticles { get; set; }

        public List<RequestDonationMoney> RequestDonationMoney { get; set; }
        public List<RequestDonationFood> RequestDonationFood { get; set; }

     
    }
}
