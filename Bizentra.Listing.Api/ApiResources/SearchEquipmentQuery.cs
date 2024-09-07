namespace Bizentra.Listing.Api.ApiResources
{
    public class SearchQuery
    {       
        public string? City { get; set; }
        public string? State { get; set; }
        public int? Page { get; set; } = 1;
        public int? PageSize { get; set; } = 50;        
    }
}
