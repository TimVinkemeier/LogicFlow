using System;
using LogicFlow.Core.FlowBuilders;

namespace LogicFlow
{
    /// <summary>
    /// Provides methods for starting LogicFlow construction.
    /// </summary>
    public static class LogicFlow
    {
        /// <summary>
        /// Use this method to begin a flow construction that does not support injected flow steps.
        /// </summary>
        /// <typeparam name="TInput">The type of the flow input.</typeparam>
        /// <typeparam name="TError">The type that is used for errors.</typeparam>
        /// <returns>A LogicFlowBuilder to further construct the flow.</returns>
        public static ILogicFlowBuilder<TInput, TInput, TError> Begin<TInput, TError>()
            => new AcceptLogicFlowBuilder<TInput, TError>(null);

        /// <summary>
        /// Use this method to begin a flow construction with support for injected flow steps.
        /// </summary>
        /// <typeparam name="TInput">The type of the flow input.</typeparam>
        /// <typeparam name="TError">The type that is used for errors.</typeparam>
        /// <param name="flowStepFactory"></param>
        /// <returns>A LogicFlowBuilder to further construct the flow.</returns>
        public static ILogicFlowBuilder<TInput, TInput, TError> Begin<TInput, TError>(IFlowStepFactory flowStepFactory)
        {
            if (flowStepFactory == null)
                throw new ArgumentNullException(nameof(flowStepFactory), "Cannot create logic flow builder for injected steps without flow step factory. Use the parameterless overload for creating such flows.");

            return new AcceptLogicFlowBuilder<TInput, TError>(flowStepFactory);
        }
    }
}
