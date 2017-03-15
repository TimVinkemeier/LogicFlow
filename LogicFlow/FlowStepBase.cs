using System.Threading;
using System.Threading.Tasks;

namespace LogicFlow
{
    /// <summary>
    /// Base class for implementing custom flow steps.
    /// </summary>
    /// <typeparam name="TInput">The input type for this step.</typeparam>
    /// <typeparam name="TOutput">The output type for this step.</typeparam>
    /// <typeparam name="TError">The error type for this step.</typeparam>
    public abstract class FlowStepBase<TInput, TOutput, TError> : IFlowStep<TInput, TOutput, TError>
    {
        public abstract Task<SuccessOrError<TOutput, TError>> ExecuteAsync(TInput input, CancellationToken cancellationToken);

        protected SuccessOrError<TOutput, TError> Cancelled() => SuccessOrError<TOutput, TError>.CreateCancellation();

        protected SuccessOrError<TOutput, TError> Error(TError error) => SuccessOrError<TOutput, TError>.CreateError(error);

        protected SuccessOrError<TOutput, TError> Success(TOutput value) => SuccessOrError<TOutput, TError>.CreateSuccess(value);
    }
}
