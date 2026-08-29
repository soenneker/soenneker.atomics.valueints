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
| `ValueAtomicInt.Or(mask)` | Performs an atomic bitwise OR operation between the current value and the specified mask, updating the value in a thread-safe manner. | The new value after the bitwise OR operation has been applied. |
| `ValueAtomicInt.And(mask)` | Performs an atomic bitwise AND operation between the current value and the specified mask. | The new value resulting from the bitwise AND operation. |
| `ValueAtomicInt.GetAndIncrement()` | Atomically increments the value and returns the previous value. | The value that was stored before the atomic update. |
| `ValueAtomicInt.GetAndDecrement()` | Atomically decrements the value and returns the previous value. | The value that was stored before the atomic update. |
| `ValueAtomicInt.GetAndAdd(delta)` | Atomically adds `delta` and returns the previous value. | The value that was stored before the atomic update. |
| `ValueAtomicInt.AddAndGet(delta)` | Atomically adds `delta` and returns the resulting value. | The resulting value. |
| `ValueAtomicInt.IncrementAndGet()` | Atomically increments the value and returns the resulting value. | The resulting value. |
| `ValueAtomicInt.DecrementAndGet()` | Atomically decrements the value and returns the resulting value. | The resulting value. |
| `ValueAtomicInt.TrySetIfGreater(value)` | Attempts to set the value to `value` if it is greater than the current value. | true if the requested update was applied; otherwise, false. |
| `ValueAtomicInt.TrySetIfLess(value)` | Attempts to set the value to `value` if it is less than the current value. | true if the requested update was applied; otherwise, false. |

## Important behavior

- `ValueAtomicInt.Or(mask)`: This method is thread-safe and uses atomic operations to ensure that the update is performed without interference from other threads.
- `ValueAtomicInt.And(mask)`: This method is thread-safe and uses interlocked operations to ensure atomicity. It can be used safely in multi-threaded scenarios to update the value without race conditions.
- `ValueAtomicInt.VolatileWrite(value)`: Use this method to update the field in multithreaded scenarios where it is important that the most recent value is observed by all threads. This method provides a memory barrier to prevent certain types of reordering by the compiler or processor.
