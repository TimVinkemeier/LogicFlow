using System;

namespace LogicFlow
{
    /// <summary>
    /// Represents a generic Error.
    /// </summary>
    public class Error
    {
        public Error(string message) : this(message, null) { }

        public Error(Exception exception) : this(null, exception) { }

        public Error(string message, Exception exception)
        {
            Message = message;
            Exception = exception;
        }

        public string Message { get; }

        public Exception Exception { get; }
    }

    /// <summary>
    /// Represents a generic Error with a strongly-typed error value.
    /// </summary>
    public sealed class Error<TErrorValue> : Error
    {
        public Error(string message) : this(message, null, default(TErrorValue)) { }

        public Error(Exception exception) : this(null, exception, default(TErrorValue)) { }

        public Error(string message, Exception exception) : this(message, exception, default(TErrorValue)) { }

        public Error(string message, TErrorValue errorValue) : this(message, null, errorValue) { }

        public Error(Exception exception, TErrorValue errorValue) : this(null, exception, errorValue) { }

        public Error(string message, Exception exception, TErrorValue errorValue) : base(message, exception)
        {
            ErrorValue = errorValue;
        }

        public TErrorValue ErrorValue { get; }
    }
}