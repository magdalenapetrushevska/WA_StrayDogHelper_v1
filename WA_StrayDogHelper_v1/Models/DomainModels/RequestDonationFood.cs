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
    public class RequestDonationFood
    {
        public int Id { get; set; }
        
        [Display(Name = "Problem Title")]
        public string ProblemTitle { get; set; }
        [Display(Name = "Problem Description")]
        public string ProblemDescription { get; set; }
        [Display(Name ="Type od food")]
        public FoodTypeEnum FoodType { get; set; }
        [Display(Name = "Required amount of food")]
        public int AmountOfFoodRequired { get; set; }
        [Display(Name = "Name of the dog")]
        public string DogName { get; set; }
        
        public string ImageName { get; set; }
        [Display(Name = "Image of dog")]
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        [Display(Name = "End date")]

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
