using System;
using LogicFlow.Core.FlowBuilders;

namespace LogicFlow
{
    public static class LogicFlowBuilderExtensions
    {
        public static ILogicFlowBuilder<TInput, TOutput, TNewError> WithError<TInput, TOutput, TError, TNewError>(this ILogicFlowBuilder<TInput, TOutput, TError> builder, Func<TError, TNewError> errorMapper)
            => new WithErrorLogicFlowBuilder<TInput, TOutput, TError, TNewError>(builder, errorMapper);

        public static ILogicFlowBuilder<TInput, TNewOutput, TError> MapTo<TInput, TOutput, TError, TNewOutput>(this ILogicFlowBuilder<TInput, TOutput, TError> builder, Func<TOutput, TNewOutput> outputMapper)
            => new MapToLogicFlowBuilder<TInput, TOutput, TNewOutput, TError>(builder, outputMapper);

        public static ILogicFlowBuilder<TInput, TOutput, TError> ThrowIfErroneous<TInput, TOutput, TError>(this ILogicFlowBuilder<TInput, TOutput, TError> builder)
            => new ThrowIfErroneousLogicFlowBuilder<TInput, TOutput, TError>(builder);

        public static ILogicFlowBuilder<TInput, TOutput, TError> ThrowIfErroneous<TInput, TOutput, TError>(this ILogicFlowBuilder<TInput, TOutput, TError> builder, Func<SuccessOrError<TOutput, TError>, Exception> exceptionBuilder)
            => new ThrowIfErroneousLogicFlowBuilder<TInput, TOutput, TError>(builder, exceptionBuilder);
    }
}
