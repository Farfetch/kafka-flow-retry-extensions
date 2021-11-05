﻿namespace KafkaFlow.Retry.UnitTests.KafkaFlow.Retry.Durable.Definitions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using FluentAssertions;
    using global::KafkaFlow.Retry.Durable.Definitions;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class RetryDurableRetryPlanBeforeDefinitionTests
    {
        public readonly static IEnumerable<object[]> DataTest = new List<object[]>
        {
            new object[]
            {
                 new Func<int, TimeSpan>(_ => new TimeSpan(1)), -1
            },
            new object[]
            {
                null, 1
            }
        };

        [Theory]
        [MemberData(nameof(DataTest))]
        public void RetryDurableRetryPlanBeforeDefinition_Ctor_Validation(Func<int, TimeSpan> timeBetweenTriesPlan,
            int numberOfRetries)
        {
            // Act
            Action act = () => new RetryDurableRetryPlanBeforeDefinition(timeBetweenTriesPlan, numberOfRetries, false);

            // Assert
            act.Should().Throw<ArgumentException>();
        }
    }
}