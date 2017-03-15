using LogicFlow.Core.Flows;

namespace LogicFlow.Core.FlowBuilders
{
    internal class AndThenSameOutputLogicFlowBuilder<TOriginalInput, TOutput, TError, TStep> : LogicFlowBuilder<TOriginalInput, TOutput, TError>
        where TStep : class, IFlowStep<TOutput, TOutput, TError>
    {
        private readonly ILogicFlowBuilder<TOriginalInput, TOutput, TError> _previousBuilder;
        private readonly TStep _step;

        public AndThenSameOutputLogicFlowBuilder(LogicFlowBuilder<TOriginalInput, TOutput, TError> previousBuilder)
            : base(previousBuilder._flowStepFactory)
        {
            _previousBuilder = previousBuilder;
        }

        public AndThenSameOutputLogicFlowBuilder(LogicFlowBuilder<TOriginalInput, TOutput, TError> previousBuilder, TStep step)
            : base(previousBuilder._flowStepFactory)
        {
            _previousBuilder = previousBuilder;
            _step = step;
        }

        public override ILogicFlow<TOriginalInput, TOutput, TError> Complete()
        {
            var previousFlow = _previousBuilder.Complete();
            var thisStep = _step ?? Instantiate<TStep, TOutput, TOutput, TError>();
            return new AndThenSameOutputLogicFlow<TOriginalInput, TOutput, TError>(previousFlow, thisStep);
        }
    }
}