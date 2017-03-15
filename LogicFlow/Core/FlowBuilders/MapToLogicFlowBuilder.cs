using System;
using System.Threading;
using System.Threading.Tasks;

namespace LogicFlow.Core.FlowBuilders
{
    internal sealed class MapToLogicFlowBuilder<TInput, TOutput, TNewOutput, TError> : LogicFlowBuilder<TInput, TNewOutput, TError>
    {
        private readonly ILogicFlowBuilder<TInput, TOutput, TError> _previousBuilder;
        private readonly Func<TOutput, TNewOutput> _outputMapper;

        public MapToLogicFlowBuilder(ILogicFlowBuilder<TInput, TOutput, TError> previousBuilder, Func<TOutput, TNewOutput> outputMapper)
            : base((previousBuilder as LogicFlowBuilder<TInput, TOutput, TError>)?._flowStepFactory)
        {
            _previousBuilder = previousBuilder;
            _outputMapper = outputMapper;
        }

        public override ILogicFlow<TInput, TNewOutput, TError> Complete()
        {
            var thisStep = new MapToLogicFlowStep(_outputMapper);
            return _previousBuilder.AndThen<MapToLogicFlowStep, TNewOutput>(thisStep).Complete();
        }

        internal class MapToLogicFlowStep : FlowStepBase<TOutput, TNewOutput, TError>
        {
            private readonly Func<TOutput, TNewOutput> _outputMapper;

            public MapToLogicFlowStep(Func<TOutput, TNewOutput> outputMapper)
            {
                _outputMapper = outputMapper;
            }

            public override Task<SuccessOrError<TNewOutput, TError>> ExecuteAsync(TOutput input, CancellationToken cancellationToken)
                => Task.FromResult(Success(_outputMapper(input)));
        }
    }
}