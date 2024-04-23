namespace backend.Entities
{
    public class ArtworksResponse
    {
        public int TotalPages { get; set; }
        public List<ArtworkViewModel> ArtworkViewModels { get; set; }
    }
}
