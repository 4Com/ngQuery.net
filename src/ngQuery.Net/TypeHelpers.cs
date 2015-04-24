using ngQuery.Net.Models;
using System;
using System.Linq;
using System.Reflection;

namespace ngQuery.Net
{
    internal static class TypeHelpers
    {
        internal static bool CheckThatTypeSupportsOperator(Type memberType, OperatorEnum operation)
        {
            string methodName;

            switch (operation)
            {
                case OperatorEnum.Equals:
                case OperatorEnum.NotEquals:
                case OperatorEnum.In:
                case OperatorEnum.NotIn:
                    return true;

                case OperatorEnum.GreaterThan:
                    if (IsNumericType(memberType)) return true;
                    methodName = "op_GreaterThan";
                    break;
                case OperatorEnum.GreaterThanOrEqualTo:
                    if (IsNumericType(memberType)) return true;
                    methodName = "op_GreaterThanOrEqual";
                    break;
                case OperatorEnum.LessThan:
                    if (IsNumericType(memberType)) return true;
                    methodName = "op_LessThan";
                    break;
                case OperatorEnum.LessThanOrEqualTo:
                    if (IsNumericType(memberType)) return true;
                    methodName = "op_LessThanOrEqual";
                    break;
                default:
                    return false;
            }

            if (memberType.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public) == null)
                return false;

            return true;
        }

        internal static bool IsNumericType(Type memberType)
        {
            return numericTypes.Any(x => x == memberType);
        }

        private static Type[] numericTypes = { typeof(double), typeof(decimal), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong) };
    }
}
