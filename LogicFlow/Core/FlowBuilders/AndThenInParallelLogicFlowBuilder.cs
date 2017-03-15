using System;
using LogicFlow.Core.Flows;

namespace LogicFlow.Core.FlowBuilders
{
    internal sealed class AndThenInParallelLogicFlowBuilder<TInput, TOutput, TNewOutput, TError, TStep1, TStep2> : LogicFlowBuilder<TInput, TNewOutput, TError>
        where TStep1 : class, IFlowStep<TOutput, TNewOutput, TError>
        where TStep2 : class, IFlowStep<TOutput, TNewOutput, TError>
    {
        private readonly LogicFlowBuilder<TInput, TOutput, TError> _previousBuilder;
        private readonly TStep1 _step1;
        private readonly TStep2 _step2;
        private readonly Func<TNewOutput, TNewOutput, TNewOutput> _aggregate;

        public AndThenInParallelLogicFlowBuilder(LogicFlowBuilder<TInput, TOutput, TError> previousBuilder, Func<TNewOutput, TNewOutput, TNewOutput> aggregate)
            : base(previousBuilder._flowStepFactory)
        {
            _previousBuilder = previousBuilder;
            _aggregate = aggregate;
        }

        public AndThenInParallelLogicFlowBuilder(LogicFlowBuilder<TInput, TOutput, TError> previousBuilder, TStep1 step1, TStep2 step2, Func<TNewOutput, TNewOutput, TNewOutput> aggregate)
            : base(previousBuilder._flowStepFactory)
        {
            _previousBuilder = previousBuilder;
            _step1 = step1;
            _step2 = step2;
            _aggregate = aggregate;
        }

        public override ILogicFlow<TInput, TNewOutput, TError> Complete()
        {
            var previousFlow = _previousBuilder.Complete();
            var thisStep1 = _step1 ?? Instantiate<TStep1, TOutput, TNewOutput, TError>();
            var thisStep2 = _step2 ?? Instantiate<TStep2, TOutput, TNewOutput, TError>();
            return new AndThenInParallelLogicFlow<TInput, TOutput, TNewOutput, TError>(previousFlow, thisStep1, thisStep2, _aggregate);
        }
    }
}