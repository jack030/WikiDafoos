using System.ComponentModel.DataAnnotations;

namespace WikiDafoos.Models.ViewModel
{
    public class ArticleCreateViewModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public string Image { get; set; }

        public bool IsPrivate { get; set; }
        public bool IsSuggested { get; set; }

        public string TagsInput { get; set; } 

        [Display(Name = "Category")]
        public int? CategoryId { get; set; } 
    }
}
