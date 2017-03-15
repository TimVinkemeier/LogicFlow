using System;
using System.Threading;
using System.Threading.Tasks;

namespace LogicFlow.Core.Flows
{
    internal sealed class AndThenInParallelLogicFlow<TInput, TOutput, TNewOutput, TError> : LogicFlowBase<TInput, TNewOutput, TError>
    {
        private readonly ILogicFlow<TInput, TOutput, TError> _previousFlow;
        private readonly IFlowStep<TOutput, TNewOutput, TError> _thisStep1;
        private readonly IFlowStep<TOutput, TNewOutput, TError> _thisStep2;
        private readonly Func<TNewOutput, TNewOutput, TNewOutput> _aggregate;

        public AndThenInParallelLogicFlow(ILogicFlow<TInput, TOutput, TError> previousFlow, IFlowStep<TOutput, TNewOutput, TError> thisStep1, IFlowStep<TOutput, TNewOutput, TError> thisStep2, Func<TNewOutput, TNewOutput, TNewOutput> aggregate)
        {
            _previousFlow = previousFlow;
            _thisStep1 = thisStep1;
            _thisStep2 = thisStep2;
            _aggregate = aggregate;
        }

        public override async Task<SuccessOrError<TNewOutput, TError>> ExecuteAsync(TInput input, CancellationToken cancellationToken)
        {
            var previousResult = await _previousFlow
                .ExecuteAsync(input, cancellationToken)
                .ConfigureAwait(false);

            if (previousResult.IsErroneous)
            {
                return Error(previousResult.Error); // short-circuit
            }

            if (previousResult.IsCanceled)
            {
                return Cancelled(); // short-circuit
            }

            var step1Task = _thisStep1.ExecuteAsync(previousResult.Value, cancellationToken);
            var step2Task = _thisStep2.ExecuteAsync(previousResult.Value, cancellationToken);

            await Task.WhenAll(step1Task, step2Task).ConfigureAwait(false);

            if (step1Task.Result.IsErroneous)
            {
                return Error(step1Task.Result.Error);
            }

            if (step2Task.Result.IsErroneous)
            {
                return Error(step2Task.Result.Error);
            }

            if (step1Task.Result.IsCanceled || step2Task.Result.IsCanceled)
            {
                return Cancelled();
            }

            var successValue = _aggregate(step1Task.Result.Value, step2Task.Result.Value);
            return Success(successValue);
        }
    }
}