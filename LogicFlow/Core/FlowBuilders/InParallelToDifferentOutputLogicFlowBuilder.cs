using System;
using LogicFlow.Core.Flows;

namespace LogicFlow.Core.FlowBuilders
{
    internal sealed class InParallelToDifferentOutputLogicFlowBuilder<TInput, TOutput, TOtherOutput, TError> : LogicFlowBuilder<TInput, TOutput, TError>
    {
        private readonly LogicFlowBuilder<TInput, TOutput, TError> _leftBuilder;
        private readonly ILogicFlowBuilder<TInput, TOtherOutput, TError> _rightBuilder;
        private readonly Func<TOutput, TOtherOutput, TOutput> _aggregate;

        public InParallelToDifferentOutputLogicFlowBuilder(LogicFlowBuilder<TInput, TOutput, TError> leftBuilder, ILogicFlowBuilder<TInput, TOtherOutput, TError> rightBuilder, Func<TOutput, TOtherOutput, TOutput> aggregate)
            : base(leftBuilder._flowStepFactory)
        {
            _leftBuilder = leftBuilder;
            _rightBuilder = rightBuilder;
            _aggregate = aggregate;
        }

        public override ILogicFlow<TInput, TOutput, TError> Complete()
        {
            var leftFlow = _leftBuilder.Complete();
            var rightFlow = _rightBuilder.Complete();
            return new InParallelToDifferentOutputLogicFlow<TInput, TOutput, TOtherOutput, TError>(leftFlow, rightFlow, _aggregate);
        }
    }
}