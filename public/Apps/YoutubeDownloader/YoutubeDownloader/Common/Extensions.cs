// <copyright file="Extensions.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace YoutubeDownloader.Common {
    using System;

    /// <summary>
    /// Universally-applicable extension methods. Only the most general of methods that have no other logical extension target belong here.
    /// </summary>
    public static class Extensions {
        /// <summary>
        /// Ensures that the specified argument is non-null.
        /// </summary>
        /// <param name="argument">The argument value.</param>
        /// <param name="argumentName">The argument name to use if an exception is thrown.</param>
        /// <typeparam name="TArg">The type of <paramref name="argument"/>, which will also be used as the type of the result if no exception is thrown.</typeparam>
        /// <exception cref="ArgumentNullException">If <paramref name="argument"/> is <c>null</c>.</exception>
        /// <returns>The <paramref name="argument"/> value, which is guaranteed to be non-null.</returns>
        public static TArg CheckNonNull<TArg>(this TArg argument, string argumentName)
            where TArg : class {
            if (argument == null) {
                throw new ArgumentNullException(argumentName);
            }

            return argument;
        }

        /// <summary>
        /// Ensures that the specified string argument is non-null and not empty.
        /// </summary>
        /// <param name="argument">The string argument value.</param>
        /// <param name="argumentName">The argument name to use if an exception is thrown.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="argument"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="argument"/> is an empty string.</exception>
        /// <returns>The string <paramref name="argument"/> value, which is guaranteed to be non-null and not empty.</returns>
        public static string CheckNonNullOrEmpty(this string argument, string argumentName) {
            argument.CheckNonNull(argumentName);
            if (string.IsNullOrEmpty(argumentName)) {
                throw new ArgumentException(argumentName);
            }

            return argument;
        }
    }
}