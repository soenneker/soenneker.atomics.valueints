[![](https://img.shields.io/nuget/v/soenneker.atomics.valueints.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.atomics.valueints/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.atomics.valueints/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.atomics.valueints/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.atomics.valueints.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.atomics.valueints/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.atomics.valueints/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.atomics.valueints/actions/workflows/codeql.yml)

# Soenneker.Atomics.ValueInts

A lightweight, allocation-free atomic `int` struct backed by `Volatile` and `Interlocked` operations. Intended for use as a private field / inline synchronization primitive. Because this is a mutable `struct`, avoid copying it (e.g., returning it from properties or storing it in collections where it may be copied by value).

## Install

```bash
dotnet add package Soenneker.Atomics.ValueInts
```

## Usage

Use `ValueAtomicInt` as a field, not as a copied local value:

```csharp
using Soenneker.Atomics.ValueInts;

public sealed class QueueMetrics
{
    private ValueAtomicInt _depth;

    public int Enqueued() => _depth.Increment();
    public int Dequeued() => _depth.Decrement();
    public int ReadDepth() => _depth.Read();
}
```

Because this is a mutable struct, returning it from a property or passing it by value creates an independent counter. Use the reference-type `AtomicInt` package when multiple objects must share the wrapper itself.

`Update` and `Accumulate` may invoke their delegate repeatedly during compare-and-exchange retries. Delegates must be side-effect free.

## What you get

- `ValueAtomicInt` — A lightweight, allocation-free atomic `int` struct backed by `Volatile` and `Interlocked` operations. Intended for use as a private field / inline synchronization primitive. Because this is a mutable `struct`, avoid copying it (e.g., returning it from properties or storing it in collections where it may be copied by value).

## API at a glance

| API | What it does | Result / important behavior |
| --- | --- | --- |
| `ValueAtomicInt.Value` | Gets or sets the current value. | Gets or sets the current value. |
| `ValueAtomicInt.Read()` | Reads the current value using acquire semantics. | The current value observed with acquire memory-ordering semantics. |
| `ValueAtomicInt.Exchange(value)` | Atomically replaces the current value with `value` and returns the previous value. | The value that was stored before the atomic update. |
| `ValueAtomicInt.CompareExchange(value, comparand)` | Atomically sets the value to `value` if the current value equals `comparand`. Returns the original value. | The value observed before the compare-and-exchange attempt. |
| `ValueAtomicInt.TryCompareExchange(value, comparand)` | Attempts to set the value to `value` if the current value equals `comparand`. | true if the requested update was applied; otherwise, false. |
| `ValueAtomicInt.Increment()` | Atomically increments the value and returns the incremented value. | The incremented value. |
| `ValueAtomicInt.Decrement()` | Atomically decrements the value and returns the decremented value. | The decremented value. |
| `ValueAtomicInt.Add(delta)` | Atomically adds `delta` and returns the resulting value. | The resulting value. |
| `ValueAtomicInt.Or(mask)` | Atomically applies a bitwise OR mask. | The value observed before the OR operation. Read again when the resulting value is required. |
| `ValueAtomicInt.And(mask)` | Atomically applies a bitwise AND mask. | The value observed before the AND operation. Read again when the resulting value is required. |
| `ValueAtomicInt.GetAndIncrement()` | Atomically increments the value and returns the previous value. | The value that was stored before the atomic update. |
| `ValueAtomicInt.GetAndDecrement()` | Atomically decrements the value and returns the previous value. | The value that was stored before the atomic update. |
| `ValueAtomicInt.GetAndAdd(delta)` | Atomically adds `delta` and returns the previous value. | The value that was stored before the atomic update. |
| `ValueAtomicInt.AddAndGet(delta)` | Atomically adds `delta` and returns the resulting value. | The resulting value. |
| `ValueAtomicInt.IncrementAndGet()` | Atomically increments the value and returns the resulting value. | The resulting value. |
| `ValueAtomicInt.DecrementAndGet()` | Atomically decrements the value and returns the resulting value. | The resulting value. |
| `ValueAtomicInt.TrySetIfGreater(value)` | Attempts to set the value to `value` if it is greater than the current value. | true if the requested update was applied; otherwise, false. |
| `ValueAtomicInt.TrySetIfLess(value)` | Attempts to set the value to `value` if it is less than the current value. | true if the requested update was applied; otherwise, false. |

## Important behavior

- `ValueAtomicInt.Or(mask)`: This method returns the value from before the atomic update, matching `Interlocked.Or` semantics.
- `ValueAtomicInt.And(mask)`: This method returns the value from before the atomic update, matching `Interlocked.And` semantics.
- `ValueAtomicInt.VolatileWrite(value)`: Use this method to update the field in multithreaded scenarios where it is important that the most recent value is observed by all threads. This method provides a memory barrier to prevent certain types of reordering by the compiler or processor.
