using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WA_StrayDogHelper_v1.Models.IdentityModels;

namespace WA_StrayDogHelper_v1.Models.DomainModels
{
    public class FavoriteDog
    {
        public int Id { get; set; }
        public int DogId { get; set; }
        public Dog Dog { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
