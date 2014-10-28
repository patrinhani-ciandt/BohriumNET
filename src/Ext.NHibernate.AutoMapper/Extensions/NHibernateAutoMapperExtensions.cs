using AutoMapper;
using NHibernate;
using System;
using System.Linq.Expressions;

namespace Bohrium.Ext.NHibernate.AutoMapper.Extensions
{
    /// <summary>
    /// NHibernate Extension Methods for AutoMapper
    /// </summary>
    public static class NHibernateAutoMapperExtensions
    {
        /// <summary>
        /// Ignore the map for a property that is not initialized by the lazy load.
        /// </summary>
        /// <typeparam name="TPersistent">Persistent Object Type</typeparam>
        /// <param name="cfg">AutoMapper Member Configuration</param>
        /// <param name="memberToBeChecked">Lambda expression to access the property to be checked</param>
        public static void IgnoreIfNotInitialized<TPersistent>(this IMemberConfigurationExpression<TPersistent> cfg, Expression<Func<TPersistent, object>> memberToBeChecked)
        {
            cfg.Condition(src => NHibernateUtil.IsInitialized(memberToBeChecked.Compile()(src)));
        }
    }
}
