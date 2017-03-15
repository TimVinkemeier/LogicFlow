using System.Threading;
using System.Threading.Tasks;

namespace LogicFlow
{
    public interface ILogicFlow<TInput, TOutput, TError>
    {
        Task<SuccessOrError<TOutput, TError>> ExecuteAsync(TInput input);

        Task<SuccessOrError<TOutput, TError>> ExecuteAsync(TInput input, CancellationToken cancellationToken);
    }
}