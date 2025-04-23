using WikiDafoos.Models.Tracking;

namespace WikiDafoos.Models
{
    public class Category:TrackableEntity<int>
    {
        public int Id { get; set; } 
        public string Name { get; set; }    

    }
}
