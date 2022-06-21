// -----------------------------------------------------------------------
// <copyright file="TracingInterceptor.cs" company="Kengic">
// Copyright (c) Kengic. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Newtonsoft.Json;
using SkyApm.Tracing;
using SkyApm.Tracing.Segments;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;

namespace Kengic.Shared.Tracing
{
    /// <summary>
    /// TracingInterceptor.
    /// </summary>
    public class TracingInterceptor : AbpInterceptor, ITransientDependency
    {
        private readonly ITracingContext tracingContext;
        private readonly IEntrySegmentContextAccessor segContext;
        private readonly ILocalSegmentContextAccessor localSegmentContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="TracingInterceptor"/> class.
        /// </summary>
        /// <param name="tracingContext">ITracingContext.</param>
        /// <param name="segContext">IEntrySegmentContextAccessor.</param>
        /// <param name="localSegmentContextAccessor">ILocalSegmentContextAccessor.</param>
        public TracingInterceptor(ITracingContext tracingContext, IEntrySegmentContextAccessor segContext, ILocalSegmentContextAccessor localSegmentContextAccessor)
        {
            this.tracingContext = tracingContext;
            this.segContext = segContext;
            this.localSegmentContextAccessor = localSegmentContextAccessor;
        }

        /// <inheritdoc/>
        public override async Task InterceptAsync(IAbpMethodInvocation invocation)
        {
            var segcontext = localSegmentContextAccessor?.Context == null ? segContext.Context : localSegmentContextAccessor.Context;
            var context2 = segcontext == null
                ? tracingContext.CreateEntrySegmentContext(invocation.Method.Name, new TextCarrierHeaderCollection(new Dictionary<string, string>()))
                : tracingContext.CreateLocalSegmentContext(invocation.Method.Name);
            try
            {
                context2.Span.AddTag("ArgumentsDictionary", JsonConvert.SerializeObject(invocation.ArgumentsDictionary));
            }
            catch (JsonSerializationException)
            {
                // noting.
            }

            context2.Span.AddTag("MethodFullName", invocation?.Method?.DeclaringType?.FullName);
            context2.Span.AddLog(LogEvent.Message($"TracingId: {context2.TraceId}"));
            context2.Span.AddLog(LogEvent.Message($"SegmentId: {context2.SegmentId}"));
            context2.Span.AddLog(LogEvent.Message($"Worker running at: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff")}"));
            await invocation.ProceedAsync();
            context2.Span.AddLog(LogEvent.Message($"Worker end at: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff")}"));
            tracingContext.Release(context2);
        }
    }
}
