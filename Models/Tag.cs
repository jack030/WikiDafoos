using WikiDafoos.Models.Tracking;

namespace WikiDafoos.Models
{
    public class Tag:TrackableEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
