using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WA_StrayDogHelper_v1.Models.IdentityModels;

namespace WA_StrayDogHelper_v1.Models.DomainModels
{
    public class MakeDonationMoney
    {
        public int Id { get; set; }
        [Display(Name ="Amount of money")]
        public int AmountOfMoney { get; set; }
        public int RequestDonationId { get; set; }
        public RequestDonationMoney Request { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
