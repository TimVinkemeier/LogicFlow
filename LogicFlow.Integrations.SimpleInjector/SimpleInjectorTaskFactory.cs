using SimpleInjector;

namespace LogicFlow.Integrations.SimpleInjector
{
    public sealed class SimpleInjectorTaskFactory : IFlowStepFactory
    {
        private readonly Container _container;

        public SimpleInjectorTaskFactory(Container container)
        {
            _container = container;
        }

        public IFlowStep<TInput, TOutput, TError> CreateFlowStep<TStep, TInput, TOutput, TError>()
            where TStep : class, IFlowStep<TInput, TOutput, TError> => _container.GetInstance<TStep>();
    }
}
