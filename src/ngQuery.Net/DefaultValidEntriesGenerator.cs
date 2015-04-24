using System;
using System.Reflection;

namespace ngQuery.Net
{
    public sealed class DefaultValidEntriesGenerator : IValidEntriesGenerator
    {
        public string[] Generate(Type entityType, PropertyInfo property)
        {
            if (property.PropertyType == typeof(bool))
            {
                return new string[] { "true", "false" };
            }
            return new string[0];
        }
    }
}
