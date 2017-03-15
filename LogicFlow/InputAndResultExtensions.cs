namespace LogicFlow
{
    public static class InputAndResultExtensions
    {
        public static TErrorValue GetOrDefaultErrorValue<TOutput, TError, TErrorValue>(this SuccessOrError<TOutput, TError> output, TErrorValue defaultValue = default(TErrorValue))
        {
            if (output.IsErroneous && output.Error is Error<TErrorValue>)
            {
                return (output as Error<TErrorValue>).ErrorValue;
            }

            return defaultValue;
        }
    }
}
