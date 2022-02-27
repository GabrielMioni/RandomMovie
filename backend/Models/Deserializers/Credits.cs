using System.Collections.Generic;

namespace backend.Models.Deserializers
{
    public class Credits
    {
        public int id { get; set; }
        public List<CreditPerson> cast { get; set; }
        public List<CreditPerson> crew { get; set; }
    }
}
