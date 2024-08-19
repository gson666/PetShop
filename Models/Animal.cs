using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetSite.Models
{
    public class Animal
    {
        public int AnimalId { get; set; }

        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Invalid characters in the Name field.")]
        [Required(ErrorMessage = "The Name field is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The Name field is required.")]
        [RegularExpression(@"^(?!0$)(0|[1-9]|[1-9]\d|150)$", ErrorMessage = "Age must be a positive number 1-150.")]
        public int Age { get; set; }

        [Required(ErrorMessage = "The Description field is required.")]
        public string Description { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category AnimalCategory { get; set; }
        public int? CategoryId { get; set; }
        [BindNever]
        public string ImageUrl { get; set; }  
        public virtual List<Comment> CommentsList { get; set; }
    }
}
