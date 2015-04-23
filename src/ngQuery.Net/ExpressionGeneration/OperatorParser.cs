using ngQuery.Net.Models;
using System;

namespace ngQuery.Net.ExpressionGeneration
{
    internal class OperatorParser : IOperatorParser
    {
        public OperatorEnum Parse(string operation)
        {
            OperatorEnum result;

            if (!Enum.TryParse(operation, out result))
                throw new NotSupportedException();

            return result;
        }
    }
}
