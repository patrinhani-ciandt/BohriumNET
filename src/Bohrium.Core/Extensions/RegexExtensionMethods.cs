namespace Bohrium.Core.Extensions
{
    using System.Text.RegularExpressions;

    public static class RegexExtensionMethods
    {
        /// <summary>
        /// Performs a
        /// <see cref="System.Text.RegularExpressions.Regex"/>
        /// match against the source string
        /// using the supplied pattern that
        /// returns all matches.
        /// </summary>
        /// <param name="source">The string to parse.</param>
        /// <param name="pattern">The pattern to match.</param>
        /// <returns>A collection of matches.</returns>
        public static MatchCollection Matches(
            this string source,
            string pattern)
        {
            return new Regex(pattern).Matches(source);
        }

        /// <summary>
        /// Performs a
        /// <see cref="System.Text.RegularExpressions.Regex"/>
        /// match against the source string
        /// using the supplied pattern that
        /// returns the first match.
        /// </summary>
        /// <param name="source">The string to parse.</param>
        /// <param name="pattern">The pattern to match.</param>
        /// <returns>The first matching substring.</returns>
        public static Match Match(
            this string source,
            string pattern)
        {
            return new Regex(pattern).Match(source);
        }
    }
}