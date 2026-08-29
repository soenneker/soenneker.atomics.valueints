using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Soenneker.Atomics.ValueInts;

/// <summary>
/// A lightweight, allocation-free atomic <see cref="int"/> struct backed by <see cref="Volatile"/> and
/// <see cref="Interlocked"/> operations.
/// <para/>
/// Intended for use as a private field / inline synchronization primitive. Because this is a mutable
/// <see langword="struct"/>, avoid copying it (e.g., returning it from properties or storing it in collections
/// where it may be copied by value).
/// </summary>
[DebuggerDisplay("{Value}")]
public struct ValueAtomicInt
{
    private int _value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueAtomicInt(int initialValue = 0) => _value = initialValue;

    /// <summary>
    /// Gets or sets the current value.
    /// </summary>
    public int Value
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Volatile.Read(ref _value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Interlocked.Exchange(ref _value, value);
    }

    /// <summary>
    /// Reads the current value using acquire semantics.
    /// </summary>
    /// <returns>The current value observed with acquire memory-ordering semantics.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Read() => Volatile.Read(ref _value);

    /// <summary>
    /// Writes the value atomically.
    /// </summary>
    /// <param name="value">Replacement value to store atomically.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Write(int value) => Interlocked.Exchange(ref _value, value);

    /// <summary>
    /// Atomically replaces the current value with <paramref name="value"/> and returns the previous value.
    /// </summary>
    /// <param name="value">Replacement value to store atomically.</param>
    /// <returns>The value that was stored before the atomic update.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Exchange(int value) => Interlocked.Exchange(ref _value, value);

    /// <summary>
    /// Atomically sets the value to <paramref name="value"/> if the current value equals <paramref name="comparand"/>.
    /// Returns the original value.
    /// </summary>
    /// <param name="value">Replacement value stored only when the expected value matches.</param>
    /// <param name="comparand">Expected current value required for the atomic replacement.</param>
    /// <returns>The value observed before the compare-and-exchange attempt.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CompareExchange(int value, int comparand) =>
        Interlocked.CompareExchange(ref _value, value, comparand);

    /// <summary>
    /// Attempts to set the value to <paramref name="value"/> if the current value equals <paramref name="comparand"/>.
    /// </summary>
    /// <param name="value">Replacement value stored only when the expected value matches.</param>
    /// <param name="comparand">Expected current value required for the atomic replacement.</param>
    /// <returns>true if the requested update was applied; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryCompareExchange(int value, int comparand) =>
        Interlocked.CompareExchange(ref _value, value, comparand) == comparand;

    /// <summary>
    /// Atomically increments the value and returns the incremented value.
    /// </summary>
    /// <returns>The incremented value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Increment() => Interlocked.Increment(ref _value);

    /// <summary>
    /// Atomically decrements the value and returns the decremented value.
    /// </summary>
    /// <returns>The decremented value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Decrement() => Interlocked.Decrement(ref _value);

    /// <summary>
    /// Atomically adds <paramref name="delta"/> and returns the resulting value.
    /// </summary>
    /// <param name="delta">Amount to add atomically.</param>
    /// <returns>The resulting value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Add(int delta) => Interlocked.Add(ref _value, delta);

    /// <summary>
    /// Performs an atomic bitwise OR operation between the current value and the specified mask, updating the value in
    /// a thread-safe manner.
    /// </summary>
    /// <remarks>This method is thread-safe and uses atomic operations to ensure that the update is performed
    /// without interference from other threads.</remarks>
    /// <param name="mask">The bit mask to apply to the current value. Each bit set in this parameter will set the corresponding bit in the
    /// current value.</param>
    /// <returns>The new value after the bitwise OR operation has been applied.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Or(int mask) => Interlocked.Or(ref _value, mask);

    /// <summary>
    /// Performs an atomic bitwise AND operation between the current value and the specified mask.
    /// </summary>
    /// <remarks>This method is thread-safe and uses interlocked operations to ensure atomicity. It can be
    /// used safely in multi-threaded scenarios to update the value without race conditions.</remarks>
    /// <param name="mask">The mask to apply to the current value. Only the bits set in both the current value and the mask are retained.</param>
    /// <returns>The new value resulting from the bitwise AND operation.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int And(int mask) => Interlocked.And(ref _value, mask);

    /// <summary>
    /// Atomically increments the value and returns the previous value.
    /// </summary>
    /// <returns>The value that was stored before the atomic update.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetAndIncrement() => Interlocked.Increment(ref _value) - 1;

    /// <summary>
    /// Atomically decrements the value and returns the previous value.
    /// </summary>
    /// <returns>The value that was stored before the atomic update.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetAndDecrement() => Interlocked.Decrement(ref _value) + 1;

    /// <summary>
    /// Atomically adds <paramref name="delta"/> and returns the previous value.
    /// </summary>
    /// <param name="delta">Amount to add atomically.</param>
    /// <returns>The value that was stored before the atomic update.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetAndAdd(int delta) => Interlocked.Add(ref _value, delta) - delta;

    // ---- And-get (returns current) ----

    /// <summary>
    /// Atomically adds <paramref name="delta"/> and returns the resulting value.
    /// </summary>
    /// <param name="delta">Amount to add atomically.</param>
    /// <returns>The resulting value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int AddAndGet(int delta) => Interlocked.Add(ref _value, delta);

    /// <summary>
    /// Atomically increments the value and returns the resulting value.
    /// </summary>
    /// <returns>The resulting value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int IncrementAndGet() => Interlocked.Increment(ref _value);

    /// <summary>
    /// Atomically decrements the value and returns the resulting value.
    /// </summary>
    /// <returns>The resulting value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int DecrementAndGet() => Interlocked.Decrement(ref _value);

    // ---- Conditional set helpers ----

    /// <summary>
    /// Attempts to set the value to <paramref name="value"/> if it is greater than the current value.
    /// </summary>
    /// <param name="value">Candidate value stored only when it is greater than the current value.</param>
    /// <returns>true if the requested update was applied; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TrySetIfGreater(int value)
    {
        int current = Volatile.Read(ref _value);
        if (value <= current)
            return false;

        return Interlocked.CompareExchange(ref _value, value, current) == current;
    }

    /// <summary>
    /// Attempts to set the value to <paramref name="value"/> if it is less than the current value.
    /// </summary>
    /// <param name="value">Candidate value stored only when it is less than the current value.</param>
    /// <returns>true if the requested update was applied; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TrySetIfLess(int value)
    {
        int current = Volatile.Read(ref _value);
        if (value >= current)
            return false;

        return Interlocked.CompareExchange(ref _value, value, current) == current;
    }

    /// <summary>
    /// Sets the value to <paramref name="value"/> if it is greater than the current value, returning the effective value.
    /// </summary>
    /// <param name="value">Candidate value stored only when it is greater than the current value.</param>
    /// <returns>The effective value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int SetIfGreater(int value)
    {
        var spin = new SpinWait();

        while (true)
        {
            int current = Volatile.Read(ref _value);
            if (value <= current)
                return current;

            int prior = Interlocked.CompareExchange(ref _value, value, current);
            if (prior == current)
                return value;

            spin.SpinOnce();
        }
    }

    /// <summary>
    /// Sets the value to <paramref name="value"/> if it is less than the current value, returning the effective value.
    /// </summary>
    /// <param name="value">Candidate value stored only when it is less than the current value.</param>
    /// <returns>The effective value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int SetIfLess(int value)
    {
        var spin = new SpinWait();

        while (true)
        {
            int current = Volatile.Read(ref _value);
            if (value >= current)
                return current;

            int prior = Interlocked.CompareExchange(ref _value, value, current);
            if (prior == current)
                return value;

            spin.SpinOnce();
        }
    }

    // ---- CAS-loop transforms ----

    /// <summary>
    /// Atomically applies <paramref name="update"/> in a CAS loop and returns the updated value.
    /// </summary>
    /// <param name="update">Function that computes a replacement value from the current value.</param>
    /// <returns>The updated value.</returns>
    public int Update(Func<int, int> update)
    {
        if (update is null)
            throw new ArgumentNullException(nameof(update));

        var spin = new SpinWait();

        while (true)
        {
            int original = Volatile.Read(ref _value);
            int next = update(original);

            int prior = Interlocked.CompareExchange(ref _value, next, original);
            if (prior == original)
                return next;

            spin.SpinOnce();
        }
    }

    /// <summary>
    /// Writes the specified value to the underlying field with volatile semantics, ensuring that the value is
    /// immediately visible to other threads.
    /// </summary>
    /// <remarks>Use this method to update the field in multithreaded scenarios where it is important that the
    /// most recent value is observed by all threads. This method provides a memory barrier to prevent certain types of
    /// reordering by the compiler or processor.</remarks>
    /// <param name="value">The value to write to the field. The written value will be visible to all threads after the operation completes.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void VolatileWrite(int value)
    {
        Volatile.Write(ref _value, value);
    }

    /// <summary>
    /// Atomically combines the current value with <paramref name="x"/> using <paramref name="accumulator"/>
    /// in a CAS loop and returns the resulting value.
    /// </summary>
    /// <param name="x">Operand passed to the accumulator function.</param>
    /// <param name="accumulator">Function that combines the current value with the supplied operand.</param>
    /// <returns>The resulting value.</returns>
    public int Accumulate(int x, Func<int, int, int> accumulator)
    {
        if (accumulator is null)
            throw new ArgumentNullException(nameof(accumulator));

        var spin = new SpinWait();

        while (true)
        {
            int original = Volatile.Read(ref _value);
            int next = accumulator(original, x);

            int prior = Interlocked.CompareExchange(ref _value, next, original);
            if (prior == original)
                return next;

            spin.SpinOnce();
        }
    }

    /// <summary>
    /// Attempts to set the value to <paramref name="value"/> if the current value
    /// equals <paramref name="expected"/>.
    /// </summary>
    /// <param name="value">Replacement value stored only when the expected value matches.</param>
    /// <param name="expected">Value that must currently be stored for the update to succeed.</param>
    /// <returns><see langword="true"/> if the value was updated; otherwise <see langword="false"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TrySet(int value, int expected) => Interlocked.CompareExchange(ref _value, value, expected) == expected;

    /// <summary>
    /// Returns a string representation of the current value.
    /// </summary>
    public override string ToString() => Volatile.Read(ref _value)
                                                 .ToString();
}
