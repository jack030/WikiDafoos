using WikiDafoos.Models.Tracking;

namespace WikiDafoos.Models
{
    public class Article :TrackableEntity<int>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Views { get; set; }
        public bool IsSuggested {  get; set; }
        public bool IsPrivate {  get; set; }
        public List<Tag> Tags { get; set; } 
        public Category Category { get; set; }  
        // public DateTime CreatedDate { get; set; }
    }
}
