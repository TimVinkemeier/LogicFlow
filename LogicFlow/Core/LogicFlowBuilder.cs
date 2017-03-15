using System;
using LogicFlow.Core.Exceptions;
using LogicFlow.Core.FlowBuilders;

namespace LogicFlow.Core
{
    internal abstract class LogicFlowBuilder<TInput, TOutput, TError> : ILogicFlowBuilder<TInput, TOutput, TError>
    {
        protected internal readonly IFlowStepFactory _flowStepFactory;

        internal LogicFlowBuilder(IFlowStepFactory flowStepFactory)
        {
            _flowStepFactory = flowStepFactory;
        }

        protected IFlowStep<TStepInput, TStepOutput, TStepError> Instantiate<TStep, TStepInput, TStepOutput, TStepError>
            () where TStep : class, IFlowStep<TStepInput, TStepOutput, TStepError>
        {
            if (_flowStepFactory == null)
            {
                throw new MissingFlowStepFactoryException($"Unable to instantiate step '{typeof(TStep).FullName}' - no flow step factory has been provided. Use the LogicFlow.Begin<TInput,TError>(IFlowStepFactory) when using injected flow steps.");
            }

            return _flowStepFactory.CreateFlowStep<TStep, TStepInput, TStepOutput, TStepError>();
        }

        public ILogicFlowBuilder<TInput, TNewOutput, TError> AndThen<TStep, TNewOutput>() where TStep : class, IFlowStep<TOutput, TNewOutput, TError>
            => new AndThenLogicFlowBuilder<TInput, TOutput, TNewOutput, TError, TStep>(this);

        public ILogicFlowBuilder<TInput, TOutput, TError> AndThen<TStep>() where TStep : class, IFlowStep<TOutput, TOutput, TError>
            => new AndThenSameOutputLogicFlowBuilder<TInput, TOutput, TError, TStep>(this);

        public ILogicFlowBuilder<TInput, TOutput, TError> AndThenInParallel<TStep1, TStep2>(Func<TOutput, TOutput, TOutput> aggregate)
            where TStep1 : class, IFlowStep<TOutput, TOutput, TError>
            where TStep2 : class, IFlowStep<TOutput, TOutput, TError>
            => new AndThenInParallelSameOutputLogicFlowBuilder<TInput, TOutput, TError, TStep1, TStep2>(this, aggregate);

        public ILogicFlowBuilder<TInput, TNewOutput, TError> AndThenInParallel<TStep1, TStep2, TNewOutput>(Func<TNewOutput, TNewOutput, TNewOutput> aggregate)
            where TStep1 : class, IFlowStep<TOutput, TNewOutput, TError>
            where TStep2 : class, IFlowStep<TOutput, TNewOutput, TError>
            => new AndThenInParallelLogicFlowBuilder<TInput, TOutput, TNewOutput, TError, TStep1, TStep2>(this, aggregate);

        public ILogicFlowBuilder<TInput, TOutput, TError> AndThenAggregateWith<TStep, TAggregate>(Action<TOutput, TAggregate> aggregate)
            where TStep : class, IFlowStep<TOutput, TAggregate, TError>
            => new AndThenAggregateWithLogicFlowBuilder<TInput, TOutput, TAggregate, TError, TStep>(this, aggregate);

        public ILogicFlowBuilder<TInput, TOutput, TError> InParallelTo(ILogicFlowBuilder<TInput, TOutput, TError> other, Func<TOutput, TOutput, TOutput> aggregate)
            => new InParallelToLogicFlowBuilder<TInput, TOutput, TError>(this, other, aggregate);

        public ILogicFlowBuilder<TInput, TOutput, TError> InParallelTo<TOtherOutput>(ILogicFlowBuilder<TInput, TOtherOutput, TError> other, Func<TOutput, TOtherOutput, TOutput> aggregate)
            => new InParallelToDifferentOutputLogicFlowBuilder<TInput, TOutput, TOtherOutput, TError>(this, other, aggregate);

        public ILogicFlowBuilder<TInput, TAggregatedOutput, TError> InParallelTo<TOtherOutput, TAggregatedOutput>(ILogicFlowBuilder<TInput, TOtherOutput, TError> other, Func<TOutput, TOtherOutput, TAggregatedOutput> aggregate)
            => new InParallelToDifferentAndAggregatedOutputLogicFlowBuilder<TInput, TOutput, TOtherOutput, TAggregatedOutput, TError>(this, other, aggregate);

        public ILogicFlowBuilder<TInput, TOutput, TError> AndThen<TStep>(TStep step) where TStep : class, IFlowStep<TOutput, TOutput, TError>
            => new AndThenSameOutputLogicFlowBuilder<TInput, TOutput, TError, TStep>(this, step);

        public ILogicFlowBuilder<TInput, TNewOutput, TError> AndThen<TStep, TNewOutput>(TStep step) where TStep : class, IFlowStep<TOutput, TNewOutput, TError>
            => new AndThenLogicFlowBuilder<TInput, TOutput, TNewOutput, TError, TStep>(this, step);

        public ILogicFlowBuilder<TInput, TOutput, TError> AndThenInParallel<TStep1, TStep2>(TStep1 step1, TStep2 step2, Func<TOutput, TOutput, TOutput> aggregate) where TStep1 : class, IFlowStep<TOutput, TOutput, TError> where TStep2 : class, IFlowStep<TOutput, TOutput, TError>
            => new AndThenInParallelSameOutputLogicFlowBuilder<TInput, TOutput, TError, TStep1, TStep2>(this, step1, step2, aggregate);

        public ILogicFlowBuilder<TInput, TNewOutput, TError> AndThenInParallel<TStep1, TStep2, TNewOutput>(TStep1 step1, TStep2 step2, Func<TNewOutput, TNewOutput, TNewOutput> aggregate) where TStep1 : class, IFlowStep<TOutput, TNewOutput, TError> where TStep2 : class, IFlowStep<TOutput, TNewOutput, TError>
            => new AndThenInParallelLogicFlowBuilder<TInput, TOutput, TNewOutput, TError, TStep1, TStep2>(this, step1, step2, aggregate);

        public ILogicFlowBuilder<TInput, TOutput, TError> AndThenAggregateWith<TStep, TAggregate>(TStep step, Action<TOutput, TAggregate> aggregate) where TStep : class, IFlowStep<TOutput, TAggregate, TError>
            => new AndThenAggregateWithLogicFlowBuilder<TInput, TOutput, TAggregate, TError, TStep>(this, step, aggregate);

        public abstract ILogicFlow<TInput, TOutput, TError> Complete();
    }
}