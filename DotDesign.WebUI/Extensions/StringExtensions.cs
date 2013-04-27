using System;

namespace DotDesign.WebUI.Extensions
{
    public static class StringExtensions
    {
        private const String ResourceUrlPrefix = "/resources/image/";
        private const Char HyphenCharacter = '-';
        private const Char WhiteSpace = ' ';

        /// <summary>
        /// Shortens given string to the specified length.
        /// </summary>
        /// <returns>
        /// If string's length is bigger than specified maxLength value,
        /// then returns trimmed substring starting at first character
        /// with three dots appended.
        /// Otherwise returns iriginal string without changes.
        /// </returns>
        public static String Shorten(this String originalString, int maxLength)
        {
            if (!String.IsNullOrEmpty(originalString))
            {
                if (originalString.Length > maxLength)
                {
                    return String.Format("{0}...",
                                         originalString.Substring(0, maxLength - 3).TrimEnd());
                }
            }
            return originalString;
        }

        /// <summary>
        /// Converts resource url-type name to usual relaive url.
        /// </summary>
        public static String ToFullRelativeResourceUrl(this String url)
        {
            if (!String.IsNullOrEmpty(url))
            {
                url = ResourceUrlPrefix + url;
            }
            return url;
        }

        public static String ReplaceWhiteSpacesWithHyphens(this String originalString)
        {
            return originalString.Replace(WhiteSpace, HyphenCharacter);
        }
    }
}