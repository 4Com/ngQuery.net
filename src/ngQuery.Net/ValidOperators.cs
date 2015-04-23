using ngQuery.Net.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace ngQuery.Net
{
    public sealed class ValidOperators
    {
        public IEnumerable<ValidOperatorsResult> GetValid()
        {
            var members = typeof(OperatorEnum).GetMembers(BindingFlags.Static | BindingFlags.Public);
            foreach (var memberInfo in members)
            {
                var customAttributes = memberInfo.GetCustomAttributes().Select(x => new { Attr = x, Type = x.GetType() }).ToArray();

                yield return new ValidOperatorsResult
                {
                    IsTopLevelOperator = customAttributes.Any(x => x.Type == typeof(TopLevelOperatorAttribute)),
                    Display = ((DescriptionAttribute)customAttributes.First(x => x.Type == typeof(DescriptionAttribute)).Attr).Description,
                    Name = memberInfo.Name
                };
            }
        }
    }
}
