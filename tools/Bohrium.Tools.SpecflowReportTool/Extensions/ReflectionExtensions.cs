using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bohrium.Tools.SpecflowReportTool.Extensions
{
    public static class ReflectionExtensions
    {
        public static string GetMethodSignatureRepesentation(this MethodInfo methodInfo)
        {
            var strBuilder = new StringBuilder();

            strBuilder.AppendFormat("{0} ", methodInfo.ReturnType.Name);

            strBuilder.Append(methodInfo.Name);

            var listOfParameters = new List<string>();

            foreach (var parameterInfo in methodInfo.GetParameters())
            {
                listOfParameters.Add(string.Format("{0} {1}", parameterInfo.ParameterType.Name, parameterInfo.Name));
            }

            strBuilder.AppendFormat("({0})", string.Join(", ", listOfParameters.ToArray()));

            return strBuilder.ToString();
        }
    }
}
