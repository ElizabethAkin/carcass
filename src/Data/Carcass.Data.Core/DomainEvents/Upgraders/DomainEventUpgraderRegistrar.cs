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
using Carcass.Core.Dependencies;
using Carcass.Data.Core.DomainEvents.Abstracts;
using Carcass.Data.Core.DomainEvents.Upgraders.Abstracts;
using Microsoft.Extensions.DependencyInjection;

namespace Carcass.Data.Core.DomainEvents.Upgraders;

public sealed class DomainEventUpgraderRegistrar
{
    private readonly DependencyStore<Type> _dependencyStore;

    public DomainEventUpgraderRegistrar()
    {
        _dependencyStore = new DependencyStore<Type>();
    }

    public DomainEventUpgraderRegistrar AddDomainEventUpgrader<TDomainEvent, TDomainEventUpgrader>()
        where TDomainEvent : IDomainEvent
        where TDomainEventUpgrader : class, IDomainEventUpgrader
    {
        string? domainEventFullName = typeof(TDomainEvent).FullName;
        if (!string.IsNullOrWhiteSpace(domainEventFullName))
            _dependencyStore.AddDependency(domainEventFullName, typeof(TDomainEventUpgrader));

        return this;
    }

    public void Register(IServiceCollection services)
    {
        ArgumentVerifier.NotNull(services, nameof(services));

        foreach (KeyValuePair<string, Type> dependency in _dependencyStore.GetDependencies())
            services.AddSingleton(dependency.Value);

        services.AddSingleton(_dependencyStore);
    }
}