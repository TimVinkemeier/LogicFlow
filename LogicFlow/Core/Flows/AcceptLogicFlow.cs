using System.Threading;
using System.Threading.Tasks;

namespace LogicFlow.Core.Flows
{
    internal sealed class AcceptLogicFlow<TInput, TError> : LogicFlowBase<TInput, TInput, TError>
    {
        public override Task<SuccessOrError<TInput, TError>> ExecuteAsync(TInput input, CancellationToken cancellationToken)
            => Task.FromResult(Success(input));
    }
}