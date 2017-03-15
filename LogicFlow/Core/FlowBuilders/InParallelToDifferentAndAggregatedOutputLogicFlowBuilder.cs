using System;
using LogicFlow.Core.Flows;

namespace LogicFlow.Core.FlowBuilders
{
    internal sealed class InParallelToDifferentAndAggregatedOutputLogicFlowBuilder<TInput, TOutput, TOtherOutput, TAggregatedOutput, TError> : LogicFlowBuilder<TInput, TAggregatedOutput, TError>
    {
        private readonly LogicFlowBuilder<TInput, TOutput, TError> _leftBuilder;
        private readonly ILogicFlowBuilder<TInput, TOtherOutput, TError> _rightBuilder;
        private readonly Func<TOutput, TOtherOutput, TAggregatedOutput> _aggregate;

        public InParallelToDifferentAndAggregatedOutputLogicFlowBuilder(
            LogicFlowBuilder<TInput, TOutput, TError> leftBuilder,
            ILogicFlowBuilder<TInput, TOtherOutput, TError> rightBuilder,
            Func<TOutput, TOtherOutput, TAggregatedOutput> aggregate)
            : base(leftBuilder._flowStepFactory)
        {
            _leftBuilder = leftBuilder;
            _rightBuilder = rightBuilder;
            _aggregate = aggregate;
        }

        public override ILogicFlow<TInput, TAggregatedOutput, TError> Complete()
        {
            var leftFlow = _leftBuilder.Complete();
            var rightFlow = _rightBuilder.Complete();
            return new InParallelToDifferentAndAggregatedOutputLogicFlow<TInput, TOutput, TOtherOutput, TAggregatedOutput, TError>(leftFlow, rightFlow, _aggregate);
        }
    }
}