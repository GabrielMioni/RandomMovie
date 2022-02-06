using System.ComponentModel.DataAnnotations;

namespace backend.Models.Filters
{
    public abstract class AbstractFilter
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
