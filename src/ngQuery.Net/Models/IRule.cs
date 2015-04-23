namespace ngQuery.Net.Models
{
    public interface IRule : IRuleExpression
    {
        string SelectedEntry { get; set; }
        string SelectedField { get; set; }
        string SelectedOperator { get; set; }
    }
}