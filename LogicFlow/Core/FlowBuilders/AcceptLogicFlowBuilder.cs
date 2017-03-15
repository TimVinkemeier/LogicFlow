using LogicFlow.Core.Flows;

namespace LogicFlow.Core.FlowBuilders
{
    internal sealed class AcceptLogicFlowBuilder<TInput, TError> : LogicFlowBuilder<TInput, TInput, TError>
    {
        public AcceptLogicFlowBuilder(IFlowStepFactory taskFactory) : base(taskFactory) { }

        public override ILogicFlow<TInput, TInput, TError> Complete()
        {
            return new AcceptLogicFlow<TInput, TError>();
        }
    }
}