using System;

namespace LogicFlow
{
    public interface ILogicFlowBuilder<TInput, TOutput, TError>
    {
        ILogicFlowBuilder<TInput, TOutput, TError> AndThen<TStep>(TStep step)
            where TStep : class, IFlowStep<TOutput, TOutput, TError>;

        ILogicFlowBuilder<TInput, TOutput, TError> AndThen<TStep>()
            where TStep : class, IFlowStep<TOutput, TOutput, TError>;

        ILogicFlowBuilder<TInput, TNewOutput, TError> AndThen<TStep, TNewOutput>(TStep step)
            where TStep : class, IFlowStep<TOutput, TNewOutput, TError>;

        ILogicFlowBuilder<TInput, TNewOutput, TError> AndThen<TStep, TNewOutput>()
            where TStep : class, IFlowStep<TOutput, TNewOutput, TError>;

        ILogicFlowBuilder<TInput, TOutput, TError> AndThenInParallel<TStep1, TStep2>(TStep1 step1, TStep2 step2, Func<TOutput, TOutput, TOutput> aggregate)
            where TStep1 : class, IFlowStep<TOutput, TOutput, TError>
            where TStep2 : class, IFlowStep<TOutput, TOutput, TError>;

        ILogicFlowBuilder<TInput, TOutput, TError> AndThenInParallel<TStep1, TStep2>(Func<TOutput, TOutput, TOutput> aggregate)
            where TStep1 : class, IFlowStep<TOutput, TOutput, TError>
            where TStep2 : class, IFlowStep<TOutput, TOutput, TError>;

        ILogicFlowBuilder<TInput, TNewOutput, TError> AndThenInParallel<TStep1, TStep2, TNewOutput>(TStep1 step1, TStep2 step2, Func<TNewOutput, TNewOutput, TNewOutput> aggregate)
            where TStep1 : class, IFlowStep<TOutput, TNewOutput, TError>
            where TStep2 : class, IFlowStep<TOutput, TNewOutput, TError>;

        ILogicFlowBuilder<TInput, TNewOutput, TError> AndThenInParallel<TStep1, TStep2, TNewOutput>(Func<TNewOutput, TNewOutput, TNewOutput> aggregate)
            where TStep1 : class, IFlowStep<TOutput, TNewOutput, TError>
            where TStep2 : class, IFlowStep<TOutput, TNewOutput, TError>;

        ILogicFlowBuilder<TInput, TOutput, TError> AndThenAggregateWith<TStep, TAggregate>(TStep step, Action<TOutput, TAggregate> aggregate)
            where TStep : class, IFlowStep<TOutput, TAggregate, TError>;

        ILogicFlowBuilder<TInput, TOutput, TError> AndThenAggregateWith<TStep, TAggregate>(Action<TOutput, TAggregate> aggregate)
            where TStep : class, IFlowStep<TOutput, TAggregate, TError>;

        ILogicFlowBuilder<TInput, TOutput, TError> InParallelTo(ILogicFlowBuilder<TInput, TOutput, TError> other, Func<TOutput, TOutput, TOutput> aggregate);

        ILogicFlowBuilder<TInput, TOutput, TError> InParallelTo<TOtherOutput>(ILogicFlowBuilder<TInput, TOtherOutput, TError> other, Func<TOutput, TOtherOutput, TOutput> aggregate);

        ILogicFlowBuilder<TInput, TAggregatedOutput, TError> InParallelTo<TOtherOutput, TAggregatedOutput>(ILogicFlowBuilder<TInput, TOtherOutput, TError> other, Func<TOutput, TOtherOutput, TAggregatedOutput> aggregate);

        ILogicFlow<TInput, TOutput, TError> Complete();
    }
}