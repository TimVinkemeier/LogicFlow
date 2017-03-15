using System.Threading;
using System.Threading.Tasks;

namespace LogicFlow
{
    public sealed class AcceptStep<TInput, TError> : IFlowStep<TInput, TInput, TError>
    {
        public Task<SuccessOrError<TInput, TError>> ExecuteAsync(TInput input, CancellationToken cancellationToken)
            => Task.FromResult(SuccessOrError<TInput, TError>.CreateSuccess(input));
    }
}
