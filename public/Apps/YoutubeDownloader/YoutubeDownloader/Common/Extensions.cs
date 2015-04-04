
namespace YoutubeDownloader.Common {
    using System;

    public static class Extensions {

        public static TArg CheckNonNull<TArg>(this TArg argument, string argumentName)
            where TArg : class {
            if (argument == null) {
                throw new ArgumentNullException(argumentName);
            }

            return argument;
        }

        public static string CheckNonNullOrEmpty(this string argument, string argumentName) {
            argument.CheckNonNull(argumentName);
            if (string.IsNullOrEmpty(argumentName)) {
                throw new ArgumentException(argumentName);
            }

            return argument;
        }
    }
}