using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CRUDOperations.Models
{
    public class Car
    {
        [Key]  // Primary Key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CarId { get; set; }

        public string Brand { get; set; }

        public string Classes { get; set; } = string.Empty;

        public string Model { get; set; } = string.Empty;

        public int? Model_No { get; set; } 

        public string Features { get; set; } = string.Empty;

        public int Price { get; set; } 

        public DateTime Date { get; set; }

        public bool Activity { get; set; } = false;
    }

}
