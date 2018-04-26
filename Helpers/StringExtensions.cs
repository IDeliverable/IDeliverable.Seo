using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace IDeliverable.Seo.Helpers
{
    public static class StringExtensions
    {
        public static IEnumerable<string> SplitLines(this string text)
        {
            return Regex.Split(text, "\\n", RegexOptions.Multiline).Select(x => x.Trim()).Where(x => !String.IsNullOrWhiteSpace(x)).ToArray();
        }

        public static string JoinLines(this IEnumerable<string> lines) {
            return String.Join("\r\n", lines);
        }
    }
}