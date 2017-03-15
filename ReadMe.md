# LogicFlow

LogicFlow is a library that allows to model complex logic as a flow of simple, independently testable steps. These steps can be combined using several operators to form complex flows. One level up, these operators also apply to the composition of flows, allowing even more reusability of logic.

## Installation

LogicFlow can be installed via NuGet. Use the Visual Studio NuGet Manager or run the following command in the package manager console to install the latest version:

```
Install-Package LogicFlow
```

## Getting Started

The following examples make use of common steps implemented in the [LogicFlow.CommonSteps](https://github.com/TimVinkemeier/LogicFlow/tree/master/LogicFlow.CommonSteps) library that can also be installed via NuGet.

### Basic Concepts

- A _flow step_ is a unit of logic that takes an input of a type `TInput` and produces an output of a type `TOutput`. In case of an error, it produces and error value of type `TError`.
- A _flow_ is a combination of several _flow steps_ that models some logic. Similar to a single step, a _flow_ takes an input and produces an output (or error value) by calling its _steps_ as defined by the operators.

### Your very first flow

```csharp
var flow = LogicFlow.Begin<string, Error>()
    .AndThen(new ParseIntegerStep())
    .Complete();
```

To begin a flow you use the static generic method `LogicFlow.Begin<TInput,TError>()`. It returns an `ILogicFlowBuilder<TInput, TOutput, TError>` (where `TInput = TOutput`). The `ILogicFlowBuilder<TInput, TOutput, TError>` interface exposes several combination operators to model a flow. In this example, we start a flow that takes a `string` value, passes it to a new instance of the `ParseIntegerStep` (from [LogicFlow.CommonSteps](https://github.com/TimVinkemeier/LogicFlow/tree/master/LogicFlow.CommonSteps)), which does what the name implies and ends the flow creation with a call to the `Complete()` method. After a call to `Complete()`, the flow can no longer be modified or combined. We can now invoke the flow as follows:

```csharp
var result = await flow.ExecuteAsync("42");
```

Note that flow execution is always async (the method returns a `Task<>`). The result is of type `SuccessOrError<TOutput, TError>` (in this case `SuccessOrError<int, Error>`). We can get the value as follows:

```csharp
if (result.IsSuccessful)
{
    var parsedInteger = result.Value;
}
else if (result.IsErroneous)
{
    var error = result.Error;
}
```

A flow can be called any number of times, since it does not keep internal state (except when you implement custom steps that do so) - therefore feel free to create it once and reuse it afterwards.

### Choosing an error type

While the `TInput` and `TOutput` types are usually straight forward to select, since they are dictated by the logic, selecting an error type `TError` may seem more difficult. The error type should be able to express any error that you want to gracefully handle inside the flow and be able to carry a proper error value. But you might also need to consider reusability of your steps. To keep a step reusable, it should use an error type that is also used in other flows.

An example: For a web application you might implement a flow step that takes a string and parses it to a Guid. If the parsing is successful, the step returns the parsed value. In case of an error, you want to generate a `BadRequestResult` to inform the caller of your API about the issue. Therefore, you decide to use `IHttpActionResult` as your error type.
Some time later you would like to reuse the custom step in a mobile app you are building. However, since it is a mobile app, it does not reference the assemblies that contain `IHttpActionResult`. You can see that poor selection of error types prevents efficient reuse.

To circumvent this issue, LogicFlow contains a generic `Error` class that can be used to transport an exception and a message. Using this class as your error type ensures that you can use it in any application and together with the steps from [LogicFlow.CommonSteps](https://github.com/TimVinkemeier/LogicFlow/tree/master/LogicFlow.CommonSteps). You can always write a custom step (or use the `WithError(Func<TError, TNewError>)` operator) to convert the generic type into a representation that suits your application. There is also an `Error<TValue>` class that inherits from `Error` and can transport an error value. If you implemented the guid parsing step with error type `Error`, you can still return an `IHttpActionResult` by using `Error<IHttpActionResult>` - if you really want to - and allow your colleague to reuse your work.

### Creating a custom flow step

To create a custom flow step, you just create a class that implements the `IFlowStep<TInput, TOutput, TError>` interface. To make this easier for typical steps, you can instead inherit from the abstract `FlowStepBase<TInput, TOutput, TError>` class.

This base class provides some convenience methods to create `SuccessOrError<TOutput, TError>` results: `Success(TOutput)` to create a successful result with the given value, `Error(TError)` to create an erroneous result with the given error value and `Cancelled()` to create a cancellation result without any values.

In the following you can see a custom flow step that takes a `string` and tries to parse it into a `Guid`. It uses the `Error` class as its error type to allow for easy interoperability with common steps.

```csharp
public class ParseGuidStep : FlowStepBase<string, Guid, Error> {
    public override Task<SuccessOrError<Guid, Error>> ExecuteAsync(string input, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            // fail fast case
            var errorResult = Error(new Error("Unable to parse empty or null input to Guid."))
            return Task.FromResult(errorResult);
        }

        try
        {
            var value = Guid.Parse(input);
            return Task.FromResult(Success(value));
        }
        catch (Exception ex) when (ex is FormatException)
        {
            return Task.FromResult(Error(new Error($"Unable to parse input '{input}' to Guid.", ex)));
        }
    }
}
```

### Combination operators

*TODO - coming soon*

### Cancellation support

LogicFlow has built-in support for cancellation, making it easy for you to model long-running logic that might need to be interrupted gracefully. Just use the `ExecuteAsync(TInput, CancellationToken)` overload on your logic flow. All built-in operators respect the cancellation token and will produce a cancellation result when cancellation has been requested (you can use the `SuccessOrError<TOutput, TError>.IsCanceled` property to check for a cancellation result).

Make sure to properly implement cancellation support in your custom flow steps if you want to use the feature to its full extend.

### Error Handling

Error handling in a flow can take two forms: short-circuit error handling or graceful error handling.
If a flow step hits an error that prevents the flow from creating any meaningful output, then the flow step should throw an exception. Exceptions are not handled specially inside LogicFlow, so they will just bubble up to the calling method.

However, there might be several reasons why you might not want to throw an exception, but instead return an erroneous result. For example, in an ASP.Net web application that tries to retrieve some data from a backend service, but fails to find it, it might be more appropriate to create an erroneous result with a `NotFoundResult` that the controller can return instead of throwing an exception that needs to be catched by the calling method.

As a best practice, you should try to fail gracefully whenever possible.