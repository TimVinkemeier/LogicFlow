using System.Threading;
using System.Threading.Tasks;

namespace LogicFlow.Core.Flows
{
    internal sealed class AndThenLogicFlow<TOriginalInput, TIntermediateInput, TOutput, TError> : LogicFlowBase<TOriginalInput, TOutput, TError>
    {
        private readonly ILogicFlow<TOriginalInput, TIntermediateInput, TError> _previousFlow;
        private readonly IFlowStep<TIntermediateInput, TOutput, TError> _thisStep;

        public AndThenLogicFlow(ILogicFlow<TOriginalInput, TIntermediateInput, TError> previousFlow, IFlowStep<TIntermediateInput, TOutput, TError> thisStep)
        {
            _previousFlow = previousFlow;
            _thisStep = thisStep;
        }

        public override async Task<SuccessOrError<TOutput, TError>> ExecuteAsync(TOriginalInput input, CancellationToken cancellationToken)
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

            return await _thisStep
                .ExecuteAsync(previousResult.Value, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}