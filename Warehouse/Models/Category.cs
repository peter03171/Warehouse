using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse.Models
{
    [Table("Categories")]
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [Required]
        [StringLength(30)]
        public string CategoryName { get; set; }

        [StringLength(80)]
        public string Description { get; set; }
    }
}
