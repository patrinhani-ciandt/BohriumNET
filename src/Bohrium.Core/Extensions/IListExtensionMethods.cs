namespace Bohrium.Core.Extensions
{
    using System;
    using System.Collections.Generic;

    public static class IListExtensionMethods
    {
        public static IList<TK> Map<T, TK>(this IList<T> list, Func<T, TK> function)
        {
            var newList = new List<TK>(list.Count);
            for (var i = 0; i < list.Count; ++i)
            {
                newList.Add(function(list[i]));
            }
            return newList;
        }
    }
}