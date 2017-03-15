using System;
using System.Threading;
using System.Threading.Tasks;

namespace LogicFlow.Core.Flows
{
    internal sealed class AndThenAggregateWithLogicFlow<TInput, TOutput, TAggregate, TError> : LogicFlowBase<TInput, TOutput, TError>
    {
        private readonly ILogicFlow<TInput, TOutput, TError> _previousFlow;
        private readonly IFlowStep<TOutput, TAggregate, TError> _thisStep;
        private readonly Action<TOutput, TAggregate> _aggregate;

        public AndThenAggregateWithLogicFlow(ILogicFlow<TInput, TOutput, TError> previousFlow, IFlowStep<TOutput, TAggregate, TError> thisStep, Action<TOutput, TAggregate> aggregate)
        {
            _previousFlow = previousFlow;
            _thisStep = thisStep;
            _aggregate = aggregate;
        }

        public override async Task<SuccessOrError<TOutput, TError>> ExecuteAsync(TInput input, CancellationToken cancellationToken)
        {
            var previousResult = await _previousFlow
                .ExecuteAsync(input, cancellationToken)
                .ConfigureAwait(false);

            if (previousResult.IsErroneous)
            {
                return Error(previousResult.Error); // short-circuit
            }

            if (previousResult.IsCanceled || cancellationToken.IsCancellationRequested)
            {
                return Cancelled(); // short-circuit
            }

            var aggregateStepResult = await _thisStep
                .ExecuteAsync(previousResult.Value, cancellationToken)
                .ConfigureAwait(false);

            if (aggregateStepResult.IsErroneous)
            {
                return Error(previousResult.Error); // short-circuit
            }

            if (aggregateStepResult.IsCanceled || cancellationToken.IsCancellationRequested)
            {
                return Cancelled(); // short-circuit
            }

            _aggregate(previousResult.Value, aggregateStepResult.Value);

            return Success(previousResult.Value);
        }
    }
}