// -----------------------------------------------------------------------
// <copyright file="KengicSegmentContextFactory.cs" company="Kengic">
// Copyright (c) Kengic. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

/*
 * Licensed to the SkyAPM under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The SkyAPM licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 */

using SkyApm.Common;
using SkyApm.Config;
using SkyApm.Tracing;
using SkyApm.Tracing.Segments;
using System;
using System.Linq;

namespace Kengic.Shared.Tracing
{
    /// <summary>
    /// KengicSegmentContextFactory.
    /// </summary>
    public class KengicSegmentContextFactory : ISegmentContextFactory
    {
        private readonly IEntrySegmentContextAccessor entrySegmentContextAccessor;
        private readonly ILocalSegmentContextAccessor localSegmentContextAccessor;
        private readonly IExitSegmentContextAccessor exitSegmentContextAccessor;
        private readonly ISamplerChainBuilder samplerChainBuilder;
        private readonly IUniqueIdGenerator uniqueIdGenerator;
        private readonly InstrumentConfig instrumentConfig;

        /// <summary>
        /// Initializes a new instance of the <see cref="KengicSegmentContextFactory"/> class.
        /// </summary>
        /// <param name="samplerChainBuilder">ISamplerChainBuilder.</param>
        /// <param name="uniqueIdGenerator">IUniqueIdGenerator.</param>
        /// <param name="entrySegmentContextAccessor">IEntrySegmentContextAccessor.</param>
        /// <param name="localSegmentContextAccessor">ILocalSegmentContextAccessor.</param>
        /// <param name="exitSegmentContextAccessor">IExitSegmentContextAccessor.</param>
        /// <param name="configAccessor">IConfigAccessor.</param>
        public KengicSegmentContextFactory(
            ISamplerChainBuilder samplerChainBuilder,
            IUniqueIdGenerator uniqueIdGenerator,
            IEntrySegmentContextAccessor entrySegmentContextAccessor,
            ILocalSegmentContextAccessor localSegmentContextAccessor,
            IExitSegmentContextAccessor exitSegmentContextAccessor,
            IConfigAccessor configAccessor)
        {
            this.samplerChainBuilder = samplerChainBuilder;
            this.uniqueIdGenerator = uniqueIdGenerator;
            this.entrySegmentContextAccessor = entrySegmentContextAccessor;
            this.localSegmentContextAccessor = localSegmentContextAccessor;
            this.exitSegmentContextAccessor = exitSegmentContextAccessor;
            instrumentConfig = configAccessor.Get<InstrumentConfig>();
        }

        /// <inheritdoc/>
        public SegmentContext CreateEntrySegment(string operationName, ICarrier carrier)
        {
            var traceId = GetTraceId(carrier);
            var segmentId = GetSegmentId();
            var sampled = GetSampled(carrier, operationName);
            var segmentContext = new SegmentContext(
                traceId,
                segmentId,
                sampled,
                instrumentConfig.ServiceName,
                instrumentConfig.ServiceInstanceName,
                operationName,
                SpanType.Entry);

            if (carrier.HasValue)
            {
                var segmentReference = new SegmentReference
                {
                    Reference = Reference.CrossProcess,
                    EntryEndpoint = carrier.EntryEndpoint,
                    NetworkAddress = carrier.NetworkAddress,
                    ParentEndpoint = carrier.ParentEndpoint,
                    ParentSpanId = carrier.ParentSpanId,
                    ParentSegmentId = carrier.ParentSegmentId,
                    EntryServiceInstanceId = carrier.EntryServiceInstanceId,
                    ParentServiceInstanceId = carrier.ParentServiceInstanceId,
                    TraceId = carrier.TraceId,
                    ParentServiceId = carrier.ParentServiceId,
                };
                segmentContext.References.Add(segmentReference);
            }

            entrySegmentContextAccessor.Context = segmentContext;
            return segmentContext;
        }

        /// <inheritdoc/>
        public SegmentContext CreateLocalSegment(string operationName)
        {
            var parentSegmentContext = GetParentSegmentContext(SpanType.Local);
            var traceId = GetTraceId(parentSegmentContext);
            var segmentId = GetSegmentId();
            var sampled = GetSampled(parentSegmentContext, operationName);
            var segmentContext = new SegmentContext(
                traceId,
                segmentId,
                sampled,
                instrumentConfig.ServiceName,
                instrumentConfig.ServiceInstanceName,
                operationName,
                SpanType.Local);

            if (parentSegmentContext != null)
            {
                var parentReference = parentSegmentContext.References.FirstOrDefault();
                var reference = new SegmentReference
                {
                    Reference = Reference.CrossThread,
                    EntryEndpoint = parentReference?.EntryEndpoint ?? parentSegmentContext.Span.OperationName,
                    NetworkAddress = parentReference?.NetworkAddress ?? parentSegmentContext.Span.OperationName,
                    ParentEndpoint = parentSegmentContext.Span.OperationName,
                    ParentSpanId = parentSegmentContext.Span.SpanId,
                    ParentSegmentId = parentSegmentContext.SegmentId,
                    EntryServiceInstanceId =
                        parentReference?.EntryServiceInstanceId ?? parentSegmentContext.ServiceInstanceId,
                    ParentServiceInstanceId = parentSegmentContext.ServiceInstanceId,
                    ParentServiceId = parentSegmentContext.ServiceId,
                    TraceId = parentSegmentContext.TraceId,
                };
                segmentContext.References.Add(reference);
            }

            localSegmentContextAccessor.Context = segmentContext;
            return segmentContext;
        }

        /// <inheritdoc/>
        public SegmentContext CreateExitSegment(string operationName, StringOrIntValue networkAddress)
        {
            var parentSegmentContext = GetParentSegmentContext(SpanType.Exit);
            var traceId = GetTraceId(parentSegmentContext);
            var segmentId = GetSegmentId();
            var sampled = GetSampled(parentSegmentContext, operationName, networkAddress);
            var segmentContext = new SegmentContext(
                traceId,
                segmentId,
                sampled,
                instrumentConfig.ServiceName,
                instrumentConfig.ServiceInstanceName,
                operationName,
                SpanType.Exit);

            if (parentSegmentContext != null)
            {
                var parentReference = parentSegmentContext.References.FirstOrDefault();
                var reference = new SegmentReference
                {
                    Reference = Reference.CrossThread,
                    EntryEndpoint = parentReference?.EntryEndpoint ?? parentSegmentContext.Span.OperationName,
                    NetworkAddress = parentReference?.NetworkAddress ?? parentSegmentContext.Span.OperationName,
                    ParentEndpoint = parentSegmentContext.Span.OperationName,
                    ParentSpanId = parentSegmentContext.Span.SpanId,
                    ParentSegmentId = parentSegmentContext.SegmentId,
                    EntryServiceInstanceId =
                        parentReference?.EntryServiceInstanceId ?? parentSegmentContext.ServiceInstanceId,
                    ParentServiceInstanceId = parentSegmentContext.ServiceInstanceId,
                    ParentServiceId = parentSegmentContext.ServiceId,
                    TraceId = parentSegmentContext.TraceId,
                };
                segmentContext.References.Add(reference);
            }

            segmentContext.Span.Peer = networkAddress;
            exitSegmentContextAccessor.Context = segmentContext;
            return segmentContext;
        }

        /// <inheritdoc/>
        public void Release(SegmentContext segmentContext)
        {
            segmentContext.Span.Finish();
            switch (segmentContext.Span.SpanType)
            {
                case SpanType.Entry:
                    entrySegmentContextAccessor.Context = null;
                    break;

                case SpanType.Local:
                    localSegmentContextAccessor.Context = null;
                    break;

                case SpanType.Exit:
                    exitSegmentContextAccessor.Context = null;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(SpanType), segmentContext.Span.SpanType, "Invalid SpanType.");
            }
        }

        private string GetTraceId(ICarrier carrier)
        {
            return carrier.HasValue ? carrier.TraceId : uniqueIdGenerator.Generate();
        }

        private string GetTraceId(SegmentContext parentSegmentContext)
        {
            return parentSegmentContext?.TraceId ?? uniqueIdGenerator.Generate();
        }

        private string GetSegmentId()
        {
            return uniqueIdGenerator.Generate();
        }

        private bool GetSampled(ICarrier carrier, string operationName)
        {
            if (carrier.HasValue && carrier.Sampled.HasValue)
            {
                return carrier.Sampled.Value;
            }

            SamplingContext samplingContext;
            if (carrier.HasValue)
            {
                samplingContext = new SamplingContext(operationName, carrier.NetworkAddress, carrier.EntryEndpoint, carrier.ParentEndpoint);
            }
            else
            {
                samplingContext = new SamplingContext(operationName, default, default, default);
            }

            var sampler = samplerChainBuilder.Build();
            return sampler(samplingContext);
        }

        private bool GetSampled(SegmentContext parentSegmentContext, string operationName, StringOrIntValue peer = default)
        {
            if (parentSegmentContext != null)
            {
                return parentSegmentContext.Sampled;
            }

            var sampledContext = new SamplingContext(operationName, peer, new StringOrIntValue(operationName), default);
            var sampler = samplerChainBuilder.Build();
            return sampler(sampledContext);
        }

        private SegmentContext GetParentSegmentContext(SpanType spanType)
        {
            return spanType switch
            {
                SpanType.Entry => null,
                SpanType.Local or SpanType.Exit => localSegmentContextAccessor.Context ?? entrySegmentContextAccessor.Context,
                _ => throw new ArgumentOutOfRangeException(nameof(spanType), spanType, "Invalid SpanType."),
            };
        }
    }
}
