using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WA_StrayDogHelper_v1.Models.IdentityModels;

namespace WA_StrayDogHelper_v1.Models.DomainModels
{
    public class Question
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Posted { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public List<Reply> Replies { get; set; }

        public Question()
        {
            Replies = new List<Reply>();
        }
    }
}
