using System;

namespace LogicFlow.Core.Exceptions
{
    public class ErroneousFlowResultDetectedException : Exception
    {
        public ErroneousFlowResultDetectedException() : base("An erroneous flow was detected.")
        {
        }

        public ErroneousFlowResultDetectedException(string message) : base(message)
        {
        }

        public ErroneousFlowResultDetectedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
