using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WA_StrayDogHelper_v1.Models.Enums;
using WA_StrayDogHelper_v1.Models.IdentityModels;

namespace WA_StrayDogHelper_v1.Models.DomainModels
{
    public class Article
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Title is required")]
        public string Title { get; set; }
        [StringLength(1500, MinimumLength = 100,
        ErrorMessage = "The content should have no less than 100 characters and up to maximum of 5000 characters")]
        [Required(ErrorMessage = "Content is required")]
        public string Content { get; set; }
        public DateTime Created { get; set; }
        public TagEnum Tag { get; set; }
        public string TimeRequiredToRead { get; set; }
        public int NumberOfApplause { get; set; }
        public string ImageName { get; set; }

        [Display(Name = "Image")]
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
