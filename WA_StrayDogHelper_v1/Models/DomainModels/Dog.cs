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
    public class Dog
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name value is required")]
        public string Name { get; set; }
        [Display(Name = "Age years")]
        [Range(0, 30, ErrorMessage = "Age should be between 0 and 30")]
        public int AgeYears { get; set; }
        [Display(Name = "Age months")]
        [Range(0, 11, ErrorMessage = "Age should be between 0 and 11")]
        public int AgeMonths { get; set; }
        [Required(ErrorMessage = "Sex value is required")]
        public SexEnum Sex { get; set; }
        public SizeEnum Size { get; set; }
        public string Breed { get; set; }

        public string ImageName { get; set; }

        [Display(Name="Image")]
        [NotMapped]
        public IFormFile ImageFile { get; set; }

        public BoolEnum Disabled { get; set; }
        public BoolEnum Sterilized { get; set; }
        public BoolEnum Vaccinated { get; set; }
        public BoolEnum Chipped { get; set; }

        [Required(ErrorMessage = "Short info is required")]
        [StringLength(600, MinimumLength = 10,
        ErrorMessage = "The info should have up to maximum of 600 characters")]
        [Display(Name = "Short life story")]
        public string ShortLifeStory { get; set; }

        public string Location { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
