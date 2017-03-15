using LogicFlow.Tests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using FluentAssertions;

namespace LogicFlow.Tests
{
    [TestClass]
    public class BasicTests
    {
        [DataTestMethod]
        [DataRow("test")]
        public async Task LogicFlow_EmptyFlow_DoesNotCallStepFactoryAndReturnsInput(string inputAndExpectedOutput)
        {
            // Arrange
            var flow = LogicFlow
                .Begin<string, Error>(new NeverCallFlowStepFactory())
                .Complete();

            // Act
            var result = await flow.ExecuteAsync(inputAndExpectedOutput);

            // Assert
            result.IsSuccessful.Should().BeTrue();
            result.Value.Should().Be(inputAndExpectedOutput);
        }
    }
}
