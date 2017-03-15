namespace LogicFlow
{
    public interface IFlowStepFactory
    {
        IFlowStep<TInput, TOutput, TError> CreateFlowStep<TStep, TInput, TOutput, TError>() where TStep : class, IFlowStep<TInput, TOutput, TError>;
    }
}