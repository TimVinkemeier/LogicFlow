using System;
using System.Threading;
using System.Threading.Tasks;

namespace LogicFlow.CommonSteps
{
    public sealed class ParseIntStep : FlowStepBase<string, int, Error>
    {
        public override Task<SuccessOrError<int, Error>> ExecuteAsync(string input, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return Task.FromResult(Error(new Error($"Unable to parse empty or null input to int.")));
            }

            try
            {
                var value = int.Parse(input);
                return Task.FromResult(Success(value));
            }
            catch (Exception ex) when (ex is FormatException || ex is OutOfMemoryException)
            {
                return Task.FromResult(Error(new Error($"Unable to parse input '{input}' to int.", ex)));
            }
        }
    }
}
