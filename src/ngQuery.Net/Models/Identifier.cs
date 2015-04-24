namespace ngQuery.Net.Models
{
    public class Identifier
    {
        public string Display { get; set; }
        public string SystemIdentifier { get; set; }
        public string Type { get; set; }
        public string[] ValidEntries { get; set; }
        public Operator[] ValidOperators { get; set; }
    }
}
