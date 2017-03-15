using System;
using System.Threading;
using System.Threading.Tasks;

namespace LogicFlow.Core.Flows
{
    internal class ThrowIfErroneousLogicFlow<TInput, TOutput, TError> : LogicFlowBase<TInput, TOutput, TError>
    {
        private readonly ILogicFlow<TInput, TOutput, TError> _previousFlow;
        private readonly Func<SuccessOrError<TOutput, TError>, Exception> _exceptionBuilder;

        public ThrowIfErroneousLogicFlow(ILogicFlow<TInput, TOutput, TError> previousFlow, Func<SuccessOrError<TOutput, TError>, Exception> exceptionBuilder)
        {
            _previousFlow = previousFlow;
            _exceptionBuilder = exceptionBuilder;
        }

        public override async Task<SuccessOrError<TOutput, TError>> ExecuteAsync(TInput input, CancellationToken cancellationToken)
        {
            var previousResult = await _previousFlow
                .ExecuteAsync(input, cancellationToken)
                .ConfigureAwait(false);

            if (!previousResult.IsErroneous)
                return Success(previousResult.Value);

            throw _exceptionBuilder(previousResult);
        }
    }
}