using System.Threading;
using System.Threading.Tasks;

namespace LogicFlow.Core
{
    internal abstract class LogicFlowBase<TInput, TOutput, TError> : ILogicFlow<TInput, TOutput, TError>
    {
        public Task<SuccessOrError<TOutput, TError>> ExecuteAsync(TInput input)
            => ExecuteAsync(input, CancellationToken.None);

        public abstract Task<SuccessOrError<TOutput, TError>> ExecuteAsync(TInput input, CancellationToken cancellationToken);

        protected SuccessOrError<TOutput, TError> Cancelled() => SuccessOrError<TOutput, TError>.CreateCancellation();

        protected SuccessOrError<TOutput, TError> Error(TError error) => SuccessOrError<TOutput, TError>.CreateError(error);

        protected SuccessOrError<TOutput, TError> Success(TOutput value) => SuccessOrError<TOutput, TError>.CreateSuccess(value);
    }
}
