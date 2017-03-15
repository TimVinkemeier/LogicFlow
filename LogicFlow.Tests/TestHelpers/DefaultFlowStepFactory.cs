using System;

namespace LogicFlow.Tests.TestHelpers
{
    internal sealed class NeverCallFlowStepFactory : IFlowStepFactory
    {
        public IFlowStep<TInput, TOutput, TError> CreateFlowStep<TStep, TInput, TOutput, TError>() where TStep : class, IFlowStep<TInput, TOutput, TError>
        {
            throw new InvalidOperationException("This FlowStepFactory should never be called.");
        }
    }
}
