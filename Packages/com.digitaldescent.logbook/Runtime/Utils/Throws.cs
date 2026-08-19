// Copyright Digital Descent, All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;

#nullable enable
namespace DigitalDescent.Logbook.Framework
{
    /// <summary>
    /// Helper class for throwing exceptions in a more concise manner.
    /// </summary>
    internal static class Throws
    {
        /// <summary>
        /// Throws an exception of type <typeparamref name="TException"/> if the provided array contains the specified value.
        /// </summary>
        /// <typeparam name="TValue">Value contained in the array.</typeparam>
        /// <typeparam name="TException">Exception to throw.</typeparam>
        /// <param name="values">Values to check.</param>
        /// <param name="check">Value to find in the array.</param>
        /// <param name="args">Arguments to pass to <typeparamref name="TException"/>.</param>
        public static void IfContains<TValue, TException>(TValue[] values, TValue check, params object?[] args)
            where TException : Exception
        {
            IfNull<NullReferenceException>(values);
            IfTrue<ArgumentOutOfRangeException>(() => values.Length == 0, args);
            IfTrue<TException>(() => values.Contains(check), args);
        }

        /// <inheritdoc cref="IfContains{TValue, TException}(TValue[], TValue, object[])"/>
        public static void IfContains<TValue, TException>(TValue[] values, TValue[] check, params object?[] args)
            where TException : Exception
        {
            IfNull<NullReferenceException>(values);
            IfTrue<ArgumentOutOfRangeException>(() => values.Length == 0, args);
            IfTrue<TException>(() => values.Any(check.Contains), args);
        }

        /// <inheritdoc cref="IfContains{TValue, TException}(TValue[], TValue, object[])"/>
        public static void IfContains<TValue, TException>(IEnumerable<TValue> values, TValue check, params object?[] args)
            where TException : Exception
        {
            IfNull<NullReferenceException>(values);
            IfFalse<ArgumentOutOfRangeException>(values.Any(), args);
            IfTrue<TException>(() => values.Contains(check), args);
        }

        /// <inheritdoc cref="IfContains{TValue, TException}(TValue[], TValue, object[])"/>
        public static void IfContains<TValue, TException>(IEnumerable<TValue> values, IEnumerable<TValue> check, params object?[] args)
            where TException : Exception
        {
            IfNull<NullReferenceException>(values);
            IfFalse<ArgumentOutOfRangeException>(values.Any(), args);
            IfTrue<TException>(() => values.Any(check.Contains), args);
        }

        /// <summary>
        /// Throws an exception of type <typeparamref name="TException"/> if the provided predicate evaluates to true.
        /// </summary>
        /// <typeparam name="TException">Exception to throw.</typeparam>
        /// <param name="predicate">Condition to evaluate.</param>
        /// <param name="args">Arguments to provide to the exception constructor.</param>
        public static void IfTrue<TException>(Func<bool> predicate, params object?[] args)
            where TException : Exception
        {
            if (predicate.Invoke())
                Throw<TException>(args);
        }

        /// <inheritdoc cref="IfTrue{TException}(Func{bool}, object[])"/>
        /// <param name="condition">Condition to check.</param>
        public static void IfTrue<TException>(bool condition, params object?[] args)
            where TException : Exception => IfTrue<TException>(() => condition, args);

        /// <summary>
        /// Throws an exception of type <typeparamref name="TException"/> if the provided predicate evaluates to false.
        /// </summary>
        /// <typeparam name="TException">Exception to throw.</typeparam>
        /// <param name="predicate">Condition to evaluate.</param>
        /// <param name="args">Arguments to provide to the exception constructor.</param>
        public static void IfFalse<TException>(Func<bool> predicate, params object?[] args)
            where TException : Exception
        {
            if (!predicate.Invoke())
                Throw<TException>(args);
        }

        /// <inheritdoc cref="IfFalse{TException}(Func{bool}, object[])"/>
        /// <param name="condition">Condition to check.</param>
        public static void IfFalse<TException>(bool condition, params object?[] args)
            where TException : Exception => IfFalse<TException>(() => condition, args);

        /// <summary>
        /// Throws an exception of type <typeparamref name="TException"/> if the provided value is null.
        /// </summary>
        /// <typeparam name="TException">Exception to throw.</typeparam>
        /// <param name="value">Value to check.</param>
        /// <param name="args">Arguments to provide to the exception constructor.</param>
        public static void IfNull<TException>([NotNull] object? value, params object?[] args)
            where TException : Exception
        {
            if (value is null)
                Throw<TException>(args);
        }

        /// <inheritdoc cref="IfNull{TException}(object?, object[])"/>
        /// <exception cref="NullReferenceException"></exception>
        public static void IfNull([NotNull] object? value, [CallerArgumentExpression("value")] string? name = null) =>
            IfNull<NullReferenceException>(value, $"Value cannot be null. (Variable: {nameof(value)})");

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if the provided argument is null or whitespace if the value is a string.
        /// </summary>
        /// <param name="value">Argument to evaluate.</param>
        /// <param name="name">Name of the argument being evaluated.</param>
        /// <exception cref="ArgumentNullException">Throwns when an null argument is provided.</exception>
        public static void IfArgumentNull<T>([NotNull] T? value, [CallerArgumentExpression("value")] string? name = null)
        {
            name ??= nameof(value);
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            if (value is string str && string.IsNullOrWhiteSpace(str))
                throw new ArgumentNullException(nameof(value), $"Value cannot be null or whitespace. (Variable: {name})");
        }

        /// <summary>
        /// Throws an exception of type <typeparamref name="TException"/> with the provided arguments.
        /// </summary>
        /// <typeparam name="TException">Exception to raise.</typeparam>
        /// <param name="args">Arguments to pass ot the exception.</param>
        [DoesNotReturn]
        private static void Throw<TException>(params object?[] args) where TException : Exception
        {
            var exception = Activator.CreateInstance(typeof(TException), args);
            throw exception is Exception ex ? ex : throw new InvalidOperationException($"Could not create an instance of {typeof(TException).FullName}.");
        }
    }
}