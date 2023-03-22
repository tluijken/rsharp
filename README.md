[![Code coverage tests](https://github.com/tluijken/rsharp/actions/workflows/CODE_COVERAGE_TESTS.yml/badge.svg)](https://github.com/tluijken/task-endurer/actions/workflows/CODE_COVERAGE_TESTS.yml)
[![Publish Packages](https://github.com/tluijken/rsharp/actions/workflows/PUBLISH_PACKAGES.yml/badge.svg)](https://github.com/tluijken/task-endurer/actions/workflows/PUBLISH_PACKAGES.yml)
[![codecov](https://codecov.io/gh/tluijken/rsharp/branch/main/graph/badge.svg?token=1KRH6M0ZIK)](https://codecov.io/gh/tluijken/task-endurer)
[![NuGet version (Rsharp)](https://img.shields.io/nuget/v/Rsharp.svg?style=flat-square)](https://www.nuget.org/packages/RSharp/)

# rsharp
Provides functional programming utilities, which should be recognizable for rust developers also using csharp.

## Table of contents
- [Getting started](#getting-started)
- [Usage](#usage)
  - [Option](#option)
  - [Result](#result)
  - [Map](#map)

## Getting started

Installation via Package Manager Console in Visual Studio:

```powershell
PM> Install-Package RSharp
```

Installation via .NET CLI:

```console
> dotnet add <TARGET PROJECT> package RSharp
```

## Usage

### Option
The `Option` type is a discriminated union, which can be either `Some` or `None`. It is used to represent the possibility of a value not being present.

```csharp
// implicit conversion from int to Some
private static Option<int> Foo() => 2;

// implicit conversion from null to None
private static Option<int> Bar() => null;

var foo = Foo();
var result = foo.Match(
    some: x => x + 1, // <= should trigger this line
    none: () => 0
);

var bar = Bar();
var result2 = bar.Match(
    some: x => x + 1,
    none: () => 0 // <= should trigger this line
);
```

### Result
The `Result` type is a discriminated union, which can be either `Ok` or `Err`. It is used to represent the possibility of a value not being present.

```csharp
private static Result<int, Exception> Divide(int a, int b) =>
    b == 0
        ? new DivideByZeroException("Cannot divide by zero")
        : a / b;

var result = Divide(4, 2);

result.Match(
    ok: x => x + 1, // <= should trigger this line
    err: e => 0
);

var result2 = Divide(4, 0);

result2.Match(
    ok: x => x + 1,
    err: e => 0 // <= should trigger this line
);
```

Like in Rust, the `Result` type can also be unwrapped by calling the unwrap method. These methods will throw an exception if the `Result` is an `Err`.
```csharp
var result = Divide(4, 2);
var value = result.Unwrap(); // <= should be 2

var result2 = Divide(4, 0);
var value2 = result2.Unwrap(); // <= should throw an exception
```

There are also methods to unwrap the `Result` type, but provide a default value if the `Result` is an `Err`.
```csharp
var result = Divide(4, 2);
var value = result.UnwrapOr(0); // <= should be 2

var result2 = Divide(4, 0);
var value2 = result2.UnwrapOr(0); // <= should be 0
```

### Map
The 'Map' function is used to map a single or multiple values to a new value. It is similar to the `Select` method in LINQ.

Note that the `Map` function returns a Result type, which can be either `Ok` or `Err`. If the mapping fails, the `Err` type will contain the exception that was thrown while mapping.'
To get the result, you can either call the `Unwrap` method, or the `UnwrapOr` method.
```csharp
// Define a factory method to create a new instance of the target object.
private static readonly Func<SourceObject, TargetObject> Factory = source =>
    new TargetObject(new Guid(source.Id.ToString().PadLeft(32, '0')), source.Name, source.Description,
        source.Value);
            
// Define the source and target objects.
internal record SourceObject(int Id, string Name, string Description, double Value);
internal record TargetObject(Guid Id, string Name, string Description, double Value);

var a = new List<SourceObject>
        {
            new(1, "Object 1", "This is the first element in the list", 1.0),
            new(2, "Object 2", "This is the second element in the list", 2.0),
            new(3, "Object 3", "This is the third element in the list", 3.0)
        };
var mapResult = a.Map(Factory).ToList();

// Use the Match method to get the result, or the Unwrap method to get the result directly.
mapResult.Match(
    ok: b => b, // will be triggered if the mapping succeeds
    err: e => null // will be triggered if the mapping fails
);
```

You can also map a single value to a new value.
```csharp
var a = new SourceObject(1, "Object 1", "This is the first element in the list", 1.0);
var mapResult = a.Map(Factory);

// Use the Match method to get the result, or the Unwrap method to get the result directly.
mapResult.Match(
    ok: b => b, // will be triggered if the mapping succeeds
    err: e => null // will be triggered if the mapping fails
);
```