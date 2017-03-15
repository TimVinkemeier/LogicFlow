using System;
using LogicFlow.Core.Flows;

namespace LogicFlow.Core.FlowBuilders
{
    internal sealed class AndThenInParallelSameOutputLogicFlowBuilder<TInput, TOutput, TError, TStep1, TStep2> : LogicFlowBuilder<TInput, TOutput, TError>
        where TStep1 : class, IFlowStep<TOutput, TOutput, TError>
        where TStep2 : class, IFlowStep<TOutput, TOutput, TError>
    {
        private readonly LogicFlowBuilder<TInput, TOutput, TError> _previousBuilder;
        private readonly TStep1 _step1;
        private readonly TStep2 _step2;
        private readonly Func<TOutput, TOutput, TOutput> _aggregate;

        public AndThenInParallelSameOutputLogicFlowBuilder(LogicFlowBuilder<TInput, TOutput, TError> previousBuilder, Func<TOutput, TOutput, TOutput> aggregate)
            : base(previousBuilder._flowStepFactory)
        {
            _previousBuilder = previousBuilder;
            _aggregate = aggregate;
        }

        public AndThenInParallelSameOutputLogicFlowBuilder(LogicFlowBuilder<TInput, TOutput, TError> previousBuilder,
            TStep1 step1,
            TStep2 step2,
            Func<TOutput, TOutput, TOutput> aggregate)
            : base(previousBuilder._flowStepFactory)
        {
            _previousBuilder = previousBuilder;
            _step1 = step1;
            _step2 = step2;
            _aggregate = aggregate;
        }

        public override ILogicFlow<TInput, TOutput, TError> Complete()
        {
            var previousFlow = _previousBuilder.Complete();
            var thisStep1 = _step1 ?? Instantiate<TStep1, TOutput, TOutput, TError>();
            var thisStep2 = _step2 ?? Instantiate<TStep2, TOutput, TOutput, TError>();
            return new AndThenInParallelSameOutputLogicFlow<TInput, TOutput, TError>(previousFlow, thisStep1, thisStep2, _aggregate);
        }
    }
}