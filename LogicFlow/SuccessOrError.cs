using System;

namespace LogicFlow
{
    public sealed class SuccessOrError<TOutput, TError>
    {
        public bool IsSuccessful { get; }

        public bool IsErroneous { get; }

        public bool IsCanceled { get; }

        private readonly TOutput _value;
        public TOutput Value
        {
            get
            {
                if (!IsSuccessful)
                    throw new InvalidOperationException("Value can not be retrieved from a non-successful result.");

                return _value;
            }
        }

        private readonly TError _error;
        public TError Error
        {
            get
            {
                if (!IsErroneous)
                    throw new InvalidOperationException("Error can not be retrieved from a non-erroneous result.");

                return _error;
            }
        }

        private SuccessOrError()
        {
            IsSuccessful = false;
            IsErroneous = false;
            IsCanceled = true;
        }

        private SuccessOrError(TOutput value)
        {
            IsSuccessful = true;
            IsErroneous = false;
            IsCanceled = false;
            _value = value;
        }

        private SuccessOrError(TError error)
        {
            IsSuccessful = false;
            IsErroneous = true;
            IsCanceled = false;
            _error = error;
        }

        public static SuccessOrError<TOutput, TError> CreateSuccess(TOutput value) => new SuccessOrError<TOutput, TError>(value);

        public static SuccessOrError<TOutput, TError> CreateError(TError error) => new SuccessOrError<TOutput, TError>(error);

        public static SuccessOrError<TOutput, TError> CreateCancellation() => new SuccessOrError<TOutput, TError>();
    }
}
