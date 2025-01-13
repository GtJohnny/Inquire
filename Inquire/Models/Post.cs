using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inquire.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Titlul este obligatoriu")]
        [StringLength(100, ErrorMessage = "Titlul poate avea maxim 100 de caractere")]
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int Likes { get; set; }
        public int? CategoryId { get; set; }
        public string? UserId { get; set; }

        public virtual ApplicationUser? User { get; set; }

        public virtual Category? Category { get; set; }

        public virtual ICollection<Comment>? Comments { get; set; }


        [NotMapped]
        public IEnumerable<SelectListItem>? Categ { get; set; }
    }
}
