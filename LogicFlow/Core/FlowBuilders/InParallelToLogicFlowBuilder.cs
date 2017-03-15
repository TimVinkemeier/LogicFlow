using System;
using LogicFlow.Core.Flows;

namespace LogicFlow.Core.FlowBuilders
{
    internal sealed class InParallelToLogicFlowBuilder<TInput, TOutput, TError> : LogicFlowBuilder<TInput, TOutput, TError>
    {
        private readonly LogicFlowBuilder<TInput, TOutput, TError> _leftBuilder;
        private readonly ILogicFlowBuilder<TInput, TOutput, TError> _rightBuilder;
        private readonly Func<TOutput, TOutput, TOutput> _aggregate;

        public InParallelToLogicFlowBuilder(LogicFlowBuilder<TInput, TOutput, TError> leftBuilder, ILogicFlowBuilder<TInput, TOutput, TError> rightBuilder, Func<TOutput, TOutput, TOutput> aggregate)
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
            return new InParallelToLogicFlow<TInput, TOutput, TError>(leftFlow, rightFlow, _aggregate);
        }
    }
}