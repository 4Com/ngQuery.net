namespace ngQuery.Net.Models
{
    internal interface IRule : IRuleExpression
    {
        string SelectedEntry { get; set; }
        string SelectedField { get; set; }
        string SelectedOperator { get; set; }
    }
}