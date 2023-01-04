﻿// MIT License
//
// Copyright (c) 2022-2023 Serhii Kokhan
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using Carcass.Core;
using Carcass.Core.Conductors.Abstracts;
using Carcass.Data.EventStoreDb.Conductors.Abstracts;
using Carcass.Data.EventStoreDb.Disposers;
using Carcass.Data.EventStoreDb.Options;
using EventStore.Client;
using Microsoft.Extensions.Options;

namespace Carcass.Data.EventStoreDb.Conductors;

public sealed class EventStoreDbConductor
    : InstanceConductor<EventStoreDbOptions, EventStoreClient, EventStoreDbDisposer>, IEventStoreDbConductor
{
    public EventStoreDbConductor(
        IOptionsMonitor<EventStoreDbOptions> optionsMonitorAccessor,
        Func<EventStoreDbOptions, EventStoreClient>? factory = default
    ) : base(optionsMonitorAccessor, factory)
    {
    }

    public EventStoreDbConductor(
        IOptions<EventStoreDbOptions> optionsAccessor,
        Func<EventStoreDbOptions, EventStoreClient>? factory = default
    ) : base(optionsAccessor, factory)
    {
    }

    protected override EventStoreClient CreateInstance(EventStoreDbOptions options)
    {
        ArgumentVerifier.NotNull(options, nameof(options));

        return new EventStoreClient(EventStoreClientSettings.Create(options.ConnectionString));
    }
}