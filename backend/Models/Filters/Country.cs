using System.ComponentModel.DataAnnotations;

namespace backend.Models.Filters
{
    public class Country
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
