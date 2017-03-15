using System;
using System.Threading;
using System.Threading.Tasks;

namespace LogicFlow.Core.Flows
{
    internal sealed class InParallelToDifferentAndAggregatedOutputLogicFlow<TInput, TOutput, TOtherOutput, TAggregatedOutput, TError> : LogicFlowBase<TInput, TAggregatedOutput, TError>
    {
        private readonly ILogicFlow<TInput, TOutput, TError> _leftFlow;
        private readonly ILogicFlow<TInput, TOtherOutput, TError> _rightFlow;
        private readonly Func<TOutput, TOtherOutput, TAggregatedOutput> _aggregate;

        public InParallelToDifferentAndAggregatedOutputLogicFlow(
            ILogicFlow<TInput, TOutput, TError> leftFlow,
            ILogicFlow<TInput, TOtherOutput, TError> rightFlow,
            Func<TOutput, TOtherOutput, TAggregatedOutput> aggregate)
        {
            _leftFlow = leftFlow;
            _rightFlow = rightFlow;
            _aggregate = aggregate;
        }

        public override async Task<SuccessOrError<TAggregatedOutput, TError>> ExecuteAsync(TInput input, CancellationToken cancellationToken)
        {
            var leftTask = _leftFlow.ExecuteAsync(input, cancellationToken);
            var rightTask = _rightFlow.ExecuteAsync(input, cancellationToken);

            await Task.WhenAll(leftTask, rightTask).ConfigureAwait(false);

            if (leftTask.Result.IsErroneous)
            {
                return Error(leftTask.Result.Error);
            }

            if (rightTask.Result.IsErroneous)
            {
                return Error(rightTask.Result.Error);
            }

            if (leftTask.Result.IsCanceled || rightTask.Result.IsCanceled)
            {
                return Cancelled();
            }

            var successValue = _aggregate(leftTask.Result.Value, rightTask.Result.Value);
            return Success(successValue);
        }
    }
}