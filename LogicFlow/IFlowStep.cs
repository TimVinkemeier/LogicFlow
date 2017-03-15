using System.Threading;
using System.Threading.Tasks;

namespace LogicFlow
{
    public interface IFlowStep<TInput, TOutput, TError>
    {
        Task<SuccessOrError<TOutput, TError>> ExecuteAsync(TInput input, CancellationToken cancellationToken);
    }
}
