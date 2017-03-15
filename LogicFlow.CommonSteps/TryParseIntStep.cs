using System;
using System.Threading;
using System.Threading.Tasks;

namespace LogicFlow.CommonSteps
{
    public sealed class TryParseIntStep : FlowStepBase<string, int, Error>
    {
        public override Task<SuccessOrError<int, Error>> ExecuteAsync(string input, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return Task.FromResult(Success(default(int)));
            }

            try
            {
                var value = int.Parse(input);
                return Task.FromResult(Success(value));
            }
            catch (Exception ex) when (ex is FormatException || ex is OutOfMemoryException)
            {
                return Task.FromResult(Success(default(int)));
            }
        }
    }
}