using System.ComponentModel.DataAnnotations;

namespace backend.Entities
{
    public class CurrentPackage
    {
        [Key]
        public int CurrentPackageID { get; set; }

        public int CreatorID { get; set; }

        public int PackageID { get; set; }

        public DateTime Date { get; set; }

        

        public virtual Package Package { get; set; }

    }
}
