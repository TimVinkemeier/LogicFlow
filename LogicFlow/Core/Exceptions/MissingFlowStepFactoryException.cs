using System;

namespace LogicFlow.Core.Exceptions
{
    public class MissingFlowStepFactoryException : Exception
    {
        public MissingFlowStepFactoryException() : base("FlowStepFactory is missing.")
        {
        }

        public MissingFlowStepFactoryException(string message) : base(message)
        {
        }

        public MissingFlowStepFactoryException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
