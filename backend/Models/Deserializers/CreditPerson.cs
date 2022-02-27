namespace backend.Models.Deserializers
{
    public class CreditPerson
    {
        public int id { get; set; }
        public string known_for_department { get; set; }
        public string name { get; set; }
        public string original_name { get; set; }
        public string profile_path { get; set; }
        public string department { get; set; }
        public string job { get; set; }
    }
}
