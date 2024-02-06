using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bulky_Web.Models
{
    public class Category{
        public int CategoryId { get; set; } //need to write key if variable name is id or categoryid

        [Required]
        [DisplayName("Category Name")]
        [MaxLength(30)]
        public string Name { get; set; }

        [DisplayName("Display Order")]
        [Range(1,100,ErrorMessage ="Display Order Must Be Between 1-100")]
        public int DisplayOrder { get; set; }
    }
}