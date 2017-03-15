using LogicFlow.Core.Flows;

namespace LogicFlow.Core.FlowBuilders
{
    internal sealed class AndThenLogicFlowBuilder<TOriginalInput, TInput, TOutput, TError, TStep> : LogicFlowBuilder<TOriginalInput, TOutput, TError>
        where TStep : class, IFlowStep<TInput, TOutput, TError>
    {
        private readonly ILogicFlowBuilder<TOriginalInput, TInput, TError> _previousBuilder;
        private readonly TStep _step;

        public AndThenLogicFlowBuilder(LogicFlowBuilder<TOriginalInput, TInput, TError> previousBuilder) : base(previousBuilder._flowStepFactory)
        {
            _previousBuilder = previousBuilder;
        }

        public AndThenLogicFlowBuilder(LogicFlowBuilder<TOriginalInput, TInput, TError> previousBuilder, TStep step)
            : base(previousBuilder._flowStepFactory)
        {
            _previousBuilder = previousBuilder;
            _step = step;
        }

        public override ILogicFlow<TOriginalInput, TOutput, TError> Complete()
        {
            var previousFlow = _previousBuilder.Complete();
            var thisStep = _step ?? Instantiate<TStep, TInput, TOutput, TError>();
            return new AndThenLogicFlow<TOriginalInput, TInput, TOutput, TError>(previousFlow, thisStep);
        }
    }
}