using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetSite.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        //[Required(ErrorMessage = "Comment text is required.")]
        //[StringLength(500, ErrorMessage = "Comment text cannot exceed 500 characters.")]
        //[BindNever]
        public string CommentText { get; set; }
       /* [ForeignKey("AnimalId")]
        [BindNever]*/
        public Animal CommentAnimal { get; set; }
        public int AnimalId { get; set; }
    }
}
