using System;
using System.Threading;
using System.Threading.Tasks;

namespace LogicFlow.Core.FlowBuilders
{
    internal sealed class WithErrorLogicFlowBuilder<TInput, TOutput, TError, TNewError> : LogicFlowBuilder<TInput, TOutput, TNewError>
    {
        private readonly ILogicFlowBuilder<TInput, TOutput, TError> _previousBuilder;
        private readonly Func<TError, TNewError> _errorMapper;

        public WithErrorLogicFlowBuilder(ILogicFlowBuilder<TInput, TOutput, TError> previousBuilder, Func<TError, TNewError> errorMapper)
            : base((previousBuilder as LogicFlowBuilder<TInput, TOutput, TError>)?._flowStepFactory)
        {
            _previousBuilder = previousBuilder;
            _errorMapper = errorMapper;
        }

        public override ILogicFlow<TInput, TOutput, TNewError> Complete()
        {
            var previousFlow = _previousBuilder.Complete();
            return new WithErrorLogicFlow(previousFlow, _errorMapper);
        }

        internal class WithErrorLogicFlow : ILogicFlow<TInput, TOutput, TNewError>
        {
            private readonly ILogicFlow<TInput, TOutput, TError> _previousFlow;
            private readonly Func<TError, TNewError> _errorMapper;

            public WithErrorLogicFlow(ILogicFlow<TInput, TOutput, TError> previousFlow, Func<TError, TNewError> errorMapper)
            {
                _previousFlow = previousFlow;
                _errorMapper = errorMapper;
            }

            public Task<SuccessOrError<TOutput, TNewError>> ExecuteAsync(TInput input)
                => ExecuteAsync(input, CancellationToken.None);

            public async Task<SuccessOrError<TOutput, TNewError>> ExecuteAsync(TInput input, CancellationToken cancellationToken)
            {
                var previousResult = await _previousFlow
                    .ExecuteAsync(input, cancellationToken)
                    .ConfigureAwait(false);

                if (previousResult.IsErroneous)
                {
                    return SuccessOrError<TOutput, TNewError>.CreateError(_errorMapper(previousResult.Error));
                }

                if (previousResult.IsCanceled)
                {
                    return SuccessOrError<TOutput, TNewError>.CreateCancellation();
                }

                return SuccessOrError<TOutput, TNewError>.CreateSuccess(previousResult.Value);
            }
        }
    }
}