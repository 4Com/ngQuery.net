using ngQuery.Net.Models;

namespace ngQuery.Net.ExpressionGeneration
{
    internal interface IOperatorParser
    {
        OperatorEnum Parse(string operation);
    }
}
