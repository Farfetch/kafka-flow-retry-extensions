namespace KafkaFlow.Retry.IntegrationTests
{
    using System.Linq;
    using System.Threading.Tasks;
    using AutoFixture;
    using KafkaFlow.Retry.IntegrationTests.Core;
    using KafkaFlow.Retry.IntegrationTests.Core.Messages;
    using KafkaFlow.Retry.IntegrationTests.Core.Producers;
    using KafkaFlow.Retry.IntegrationTests.Core.Storages;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;

    [Collection("BootstrapperHostCollection")]
    public class RetryForeverTests
    {
        private readonly BootstrapperHostFixture bootstrapperHostFixture;
        private readonly Fixture fixture = new Fixture();

        public RetryForeverTests(BootstrapperHostFixture bootstrapperHostFixture)
        {
            this.bootstrapperHostFixture = bootstrapperHostFixture;
            InMemoryAuxiliarStorage.Clear();
            InMemoryAuxiliarStorage.ThrowException = true;
        }

        [Fact]
        public async Task RetryForeverTest()
        {
            // Arrange
            var producer1 = this.bootstrapperHostFixture.ServiceProvider.GetRequiredService<IMessageProducer<RetryForeverProducer>>();
            var messages = this.fixture.CreateMany<RetryForeverTestMessage>(1).ToList();

            // Act
            messages.ForEach(m => producer1.Produce(m.Key, m));

            // Assert
            foreach (var message in messages)
            {
                await InMemoryAuxiliarStorage.AssertCountRetryForeverMessageAsync(message, 20);
            }

            // To avoid a message not committed on the tests topic
            InMemoryAuxiliarStorage.Clear();
            InMemoryAuxiliarStorage.ThrowException = false;

            foreach (var message in messages)
            {
                await InMemoryAuxiliarStorage.AssertCountRetryForeverMessageAsync(message, 1);
            }
        }
    }
}