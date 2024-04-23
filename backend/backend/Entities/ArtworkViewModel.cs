namespace backend.Entities
{
    public class ArtworkViewModel
    {
        public int ArtworkID { get; set; }
        public int CreatorID { get; set; }
        public string ArtworkName { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public int Likes { get; set; }
        public string image { get; set; }
        public bool Purchasable { get; set; }
        public double? Price { get; set; }
        public Boolean Status { get; set; }
        public ArtworkViewModel()
        {
            Status = false;
        }
    }
}
