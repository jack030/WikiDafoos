namespace WikiDafoos.Models.Tracking
{
    public abstract class TrackableEntity<Tkey>
    {
        public DateTime CreationTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
