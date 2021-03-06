﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace LogicFlow.Core.Flows
{
    internal sealed class InParallelToDifferentOutputLogicFlow<TInput, TOutput, TOtherOutput, TError> : LogicFlowBase<TInput, TOutput, TError>
    {
        private readonly ILogicFlow<TInput, TOutput, TError> _leftFlow;
        private readonly ILogicFlow<TInput, TOtherOutput, TError> _rightFlow;
        private readonly Func<TOutput, TOtherOutput, TOutput> _aggregate;

        public InParallelToDifferentOutputLogicFlow(ILogicFlow<TInput, TOutput, TError> leftFlow, ILogicFlow<TInput, TOtherOutput, TError> rightFlow, Func<TOutput, TOtherOutput, TOutput> aggregate)
        {
            _leftFlow = leftFlow;
            _rightFlow = rightFlow;
            _aggregate = aggregate;
        }

        public override async Task<SuccessOrError<TOutput, TError>> ExecuteAsync(TInput input, CancellationToken cancellationToken)
        {
            var leftTask = _leftFlow.ExecuteAsync(input, cancellationToken);
            var rightTask = _rightFlow.ExecuteAsync(input, cancellationToken);

            await Task.WhenAll(leftTask, rightTask).ConfigureAwait(false);

            if (leftTask.Result.IsErroneous)
            {
                return Error(leftTask.Result.Error);
            }

            if (rightTask.Result.IsErroneous)
            {
                return Error(rightTask.Result.Error);
            }

            if (leftTask.Result.IsCanceled || rightTask.Result.IsCanceled)
            {
                return Cancelled();
            }

            var successValue = _aggregate(leftTask.Result.Value, rightTask.Result.Value);
            return Success(successValue);
        }
    }
}