using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Bohrium.Tools.SpecflowReportTool.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToJSon(this object obj)
        {
            return new JavaScriptSerializer().Serialize(obj);
        }
    }
}
