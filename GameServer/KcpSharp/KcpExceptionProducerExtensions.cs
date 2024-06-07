namespace EggLink.DanhengServer.KcpSharp
{
    /// <summary>
    /// Helper methods for <see cref="IKcpExceptionProducer{T}"/>.
    /// </summary>
    public static class KcpExceptionProducerExtensions
    {
        /// <summary>
        /// Set the handler to invoke when exception is thrown. Return true in the handler to ignore the error and continue running. Return false in the handler to abort the operation.
        /// </summary>
        /// <param name="producer">The producer instance.</param>
        /// <param name="handler">The exception handler.</param>
        public static void SetExceptionHandler<T>(this IKcpExceptionProducer<T> producer, Func<Exception, T, bool> handler)
        {
            if (producer is null)
            {
                throw new ArgumentNullException(nameof(producer));
            }
            if (handler is null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            producer.SetExceptionHandler(
                (ex, conv, state) => ((Func<Exception, T, bool>?)state)!.Invoke(ex, conv),
                handler
                );
        }

        /// <summary>
        /// Set the handler to invoke when exception is thrown. Return true in the handler to ignore the error and continue running. Return false in the handler to abort the operation.
        /// </summary>
        /// <param name="producer">The producer instance.</param>
        /// <param name="handler">The exception handler.</param>
        public static void SetExceptionHandler<T>(this IKcpExceptionProducer<T> producer, Func<Exception, bool> handler)
        {
            if (producer is null)
            {
                throw new ArgumentNullException(nameof(producer));
            }
            if (handler is null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            producer.SetExceptionHandler(
                (ex, conv, state) => ((Func<Exception, bool>?)state)!.Invoke(ex),
                handler
                );
        }

        /// <summary>
        /// Set the handler to invoke when exception is thrown.
        /// </summary>
        /// <param name="producer">The producer instance.</param>
        /// <param name="handler">The exception handler.</param>
        /// <param name="state">The state object to pass into the exception handler.</param>
        public static void SetExceptionHandler<T>(this IKcpExceptionProducer<T> producer, Action<Exception, T, object?> handler, object? state)
        {
            if (producer is null)
            {
                throw new ArgumentNullException(nameof(producer));
            }
            if (handler is null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            producer.SetExceptionHandler(
                (ex, conv, state) =>
                {
                    var tuple = (Tuple<Action<Exception, T, object?>, object?>)state!;
                    tuple.Item1.Invoke(ex, conv, tuple.Item2);
                    return false;
                },
                Tuple.Create(handler, state)
                );
        }

        /// <summary>
        /// Set the handler to invoke when exception is thrown.
        /// </summary>
        /// <param name="producer">The producer instance.</param>
        /// <param name="handler">The exception handler.</param>
        public static void SetExceptionHandler<T>(this IKcpExceptionProducer<T> producer, Action<Exception, T> handler)
        {
            if (producer is null)
            {
                throw new ArgumentNullException(nameof(producer));
            }
            if (handler is null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            producer.SetExceptionHandler(
                (ex, conv, state) =>
                {
                    var handler = (Action<Exception, T>)state!;
                    handler.Invoke(ex, conv);
                    return false;
                },
                handler
                );
        }

        /// <summary>
        /// Set the handler to invoke when exception is thrown.
        /// </summary>
        /// <param name="producer">The producer instance.</param>
        /// <param name="handler">The exception handler.</param>
        public static void SetExceptionHandler<T>(this IKcpExceptionProducer<T> producer, Action<Exception> handler)
        {
            if (producer is null)
            {
                throw new ArgumentNullException(nameof(producer));
            }
            if (handler is null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            producer.SetExceptionHandler(
                (ex, conv, state) =>
                {
                    var handler = (Action<Exception>)state!;
                    handler.Invoke(ex);
                    return false;
                },
                handler
                );
        }
    }
}
