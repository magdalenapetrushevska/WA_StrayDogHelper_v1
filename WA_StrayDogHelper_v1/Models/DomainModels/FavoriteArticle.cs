using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WA_StrayDogHelper_v1.Models.IdentityModels;

namespace WA_StrayDogHelper_v1.Models.DomainModels
{
    public class FavoriteArticle
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public Article Article { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
