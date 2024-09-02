namespace Bizentra.Listing.Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NotUpdatableAttribute : Attribute
    {
        public NotUpdatableAttribute()
        {
            this.IgnoreUpdate = true;
        }

        public bool IgnoreUpdate;
    }
}
