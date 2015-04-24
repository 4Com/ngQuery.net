using System;
using System.Reflection;

namespace ngQuery.Net
{
    public interface IValidEntriesGenerator
    {
        string[] Generate(Type entityType, PropertyInfo property);
    }
}
