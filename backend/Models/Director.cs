using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Director
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
