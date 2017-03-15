using System;
using LogicFlow.Core.Exceptions;
using LogicFlow.Core.Flows;

namespace LogicFlow.Core.FlowBuilders
{
    internal class ThrowIfErroneousLogicFlowBuilder<TInput, TOutput, TError> : LogicFlowBuilder<TInput, TOutput, TError>
    {
        private readonly ILogicFlowBuilder<TInput, TOutput, TError> _previousBuilder;
        private readonly Func<SuccessOrError<TOutput, TError>, Exception> _exceptionBuilder;

        public ThrowIfErroneousLogicFlowBuilder(ILogicFlowBuilder<TInput, TOutput, TError> previousBuilder)
            : base((previousBuilder as LogicFlowBuilder<TInput, TOutput, TError>)?._flowStepFactory)
        {
            _previousBuilder = previousBuilder ?? throw new ArgumentNullException(nameof(previousBuilder));
            _exceptionBuilder = i => new ErroneousFlowResultDetectedException($"The flow was erroneous (Error: '{i.Error}').");
        }

        public ThrowIfErroneousLogicFlowBuilder(ILogicFlowBuilder<TInput, TOutput, TError> previousBuilder, Func<SuccessOrError<TOutput, TError>, Exception> exceptionBuilder)
            : base((previousBuilder as LogicFlowBuilder<TInput, TOutput, TError>)?._flowStepFactory)
        {
            _previousBuilder = previousBuilder ?? throw new ArgumentNullException(nameof(previousBuilder));
            _exceptionBuilder = exceptionBuilder ?? throw new ArgumentNullException(nameof(exceptionBuilder));
        }

        public override ILogicFlow<TInput, TOutput, TError> Complete()
        {
            var previousStep = _previousBuilder.Complete();
            return new ThrowIfErroneousLogicFlow<TInput, TOutput, TError>(previousStep, _exceptionBuilder);
        }
    }
}