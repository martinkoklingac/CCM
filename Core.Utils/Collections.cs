using System;
using System.Collections.Generic;

namespace Core.Utils
{
    public static class Collections
    {
        #region PUBLIC METHODS
        /// <summary>
        /// Applies <paramref name="action"/> to each item in <paramref name="source"/>
        /// when <paramref name="source"/> is evaluated.
        /// </summary>
        /// <typeparam name="T">Type of items</typeparam>
        /// <param name="source">IEnumerable of type <typeparamref name="T"/></param>
        /// <param name="action">Delegate to apply to each item in <paramref name="source"/></param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when either <paramref name="action"/> or <paramref name="source"/> is null
        /// </exception>
        /// <returns>IEnumerable of type <typeparamref name="T"/></returns>
        public static IEnumerable<T> Apply<T>(this IEnumerable<T> source, Func<T, T> action)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (action == null)
                throw new ArgumentNullException("action");

            foreach (var item in source)
                yield return action(item);
        }

        /// <summary>
        /// Applies <paramref name="action"/> to each item in <paramref name="source"/>
        /// immediately when called.
        /// </summary>
        /// <typeparam name="T">Type of items</typeparam>
        /// <param name="source">IEnumerable of type <typeparamref name="T"/></param>
        /// <param name="action">Delegate to apply to each item in <paramref name="source"/></param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when either <paramref name="action"/> or <paramref name="source"/> is null
        /// </exception>
        public static void Apply<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (action == null)
                throw new ArgumentNullException("action");

            foreach (var item in source)
                action(item);
        }
        #endregion
    }
}
