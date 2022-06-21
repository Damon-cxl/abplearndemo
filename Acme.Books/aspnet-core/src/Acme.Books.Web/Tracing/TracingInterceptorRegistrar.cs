// -----------------------------------------------------------------------
// <copyright file="TracingInterceptorRegistrar.cs" company="Kengic">
// Copyright (c) Kengic. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.DynamicProxy;

namespace Kengic.Shared.Tracing
{
    /// <summary>
    /// TracingInterceptorRegistrar.
    /// </summary>
    public static class TracingInterceptorRegistrar
    {
        /// <summary>
        /// RegisterIfNeeded.
        /// </summary>
        /// <param name="context">IOnServiceRegistredContext.</param>
        public static void RegisterIfNeeded(IOnServiceRegistredContext context)
        {
            if (ShouldIntercept(context.ImplementationType))
            {
                context.Interceptors.TryAdd<TracingInterceptor>();
            }
        }

        private static bool ShouldIntercept(Type type)
        {
            // ;.
            // return typeof(AbpController).IsAssignableFrom(type) || typeof(ApplicationService).IsAssignableFrom(type) || typeof(AsyncQueryableExecuter).IsAssignableFrom(type);.
            return !DynamicProxyIgnoreTypes.Contains(type)
                && (typeof(IRepository).IsAssignableFrom(type) || typeof(IApplicationService).IsAssignableFrom(type));
        }
    }
}
