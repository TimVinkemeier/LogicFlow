using System;
using LogicFlow.Core.Flows;

namespace LogicFlow.Core.FlowBuilders
{
    internal sealed class AndThenAggregateWithLogicFlowBuilder<TInput, TOutput, TAggregate, TError, TStep> : LogicFlowBuilder<TInput, TOutput, TError>
        where TStep : class, IFlowStep<TOutput, TAggregate, TError>
    {
        private readonly LogicFlowBuilder<TInput, TOutput, TError> _previousBuilder;
        private readonly TStep _step;
        private readonly Action<TOutput, TAggregate> _aggregate;

        public AndThenAggregateWithLogicFlowBuilder(LogicFlowBuilder<TInput, TOutput, TError> previousBuilder, Action<TOutput, TAggregate> aggregate)
            : base(previousBuilder._flowStepFactory)
        {
            _previousBuilder = previousBuilder;
            _aggregate = aggregate;
        }

        public AndThenAggregateWithLogicFlowBuilder(LogicFlowBuilder<TInput, TOutput, TError> previousBuilder, TStep step, Action<TOutput, TAggregate> aggregate)
            : base(previousBuilder._flowStepFactory)
        {
            _previousBuilder = previousBuilder;
            _step = step;
            _aggregate = aggregate;
        }

        public override ILogicFlow<TInput, TOutput, TError> Complete()
        {
            var previousFlow = _previousBuilder.Complete();
            var thisStep = _step ?? Instantiate<TStep, TOutput, TAggregate, TError>();
            return new AndThenAggregateWithLogicFlow<TInput, TOutput, TAggregate, TError>(previousFlow, thisStep, _aggregate);
        }
    }
}