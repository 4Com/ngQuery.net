namespace ngQuery.Net.Models
{
    public class Identifier
    {
        public string Display { get; set; }
        public string Type { get; set; }
        public Operator[] ValidOperators { get; set; }
    }
}
