using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inquire.Models
{
    public class ApplicationUser:IdentityUser
    {

        public virtual ICollection<Comment>? Comments { get; set; }

        public virtual ICollection<Post>? Articles { get; set; }


     //   [Required(ErrorMessage ="Prenumele este obligatoriu")]
        public string? FirstName { get; set; }
      //  [Required(ErrorMessage = "Numele este obligatoriu")]
        public string? LastName { get; set; }

        [MaxLength(400,ErrorMessage ="Bio-ul nu poate fi mai lung de 400 de caractere")]
        public string? Bio { get; set; }

        public string? Image { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? AllRoles { get; set; }
    }
}
