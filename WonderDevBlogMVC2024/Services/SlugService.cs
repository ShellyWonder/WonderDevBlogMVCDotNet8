using Microsoft.EntityFrameworkCore;
using System.Runtime.ConstrainedExecution;
using System.Text;
using WonderDevBlogMVC2024.Data;
using WonderDevBlogMVC2024.Data.Repositories.Interfaces;
using WonderDevBlogMVC2024.Services.Interfaces;

namespace WonderDevBlogMVC2024.Services
{
#region PRIMARY CONSTRUCTOR
    public class SlugService(ISlugRepository slugRepository) : ISlugService
    {
        private readonly ISlugRepository _slugRepository = slugRepository; 
        #endregion

#region IS UNIQUE
        public bool IsUnique(string slug)
        {
            return _slugRepository.IsSlugUnique(slug);
        } 
        #endregion

#region URL FRIENDLY
        public string UrlFriendly(string title)
        {
            if (string.IsNullOrWhiteSpace(title)) return "";

            const int maxlen = 80;
            var sb = new StringBuilder(title.Length);
            bool prevDash = false;

            for (int i = 0; i < title.Length && sb.Length < maxlen; i++)
            {
                char c = title[i];

                // Handle alphanumeric characters
                if (IsAlphanumeric(c))
                {
                    sb.Append(char.ToLowerInvariant(c));
                    prevDash = false;
                }
                // Handle valid separator characters
                else if (IsSeparator(c))
                {
                    if (!prevDash && sb.Length > 0)
                    {
                        sb.Append('-');
                        prevDash = true;
                    }
                }
                // Handle special characters
                else if (c == '#')
                {
                    HandleSharpNotation(title, i, sb);
                }
                else if (c == '+')
                {
                    sb.Append("-plus");
                    prevDash = false;
                }
                // Handle international characters
                else if (IsNonAsciiCharacter(c))
                {
                    AppendAsciiEquivalent(c, sb);
                    prevDash = false;
                }
            }

            // Remove trailing dash if necessary
            return prevDash ? sb.ToString().TrimEnd('-') : sb.ToString();
        }

        #endregion

#region IS ALPHANUMERIC
        private static bool IsAlphanumeric(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9');
        }

        #endregion

#region IS SEPARATOR
        private static bool IsSeparator(char c)
        {
            return c == ' ' || c == ',' || c == '.' || c == '/' || c == '\\' || c == '-' || c == '_' || c == '=';
        }

        #endregion

#region IS NON ASCII CHARACTER
        private static bool IsNonAsciiCharacter(char c)
        {
            return (int)c >= 128;
        } 
        #endregion

#region HANDLE SHARP NOTATION
        private static void HandleSharpNotation(string title, int index, StringBuilder sb)
        {
            if (index > 0 && (title[index - 1] == 'C' || title[index - 1] == 'F'))
            {
                sb.Append("-sharp");
            }
        } 
        #endregion

#region APPEND ASCII EQUIVALENT
        private static void AppendAsciiEquivalent(char c, StringBuilder sb)
        {
            string asciiEquivalent = RemapInternationalCharToAscii(c);
            if (!string.IsNullOrEmpty(asciiEquivalent))
            {
                sb.Append(asciiEquivalent);
            }
        } 
        #endregion

#region DICTIONARY
        //the dictionary charMap acts as a lookup table, mapping international characters to their ASCII equivalents
        private static readonly Dictionary<char, string> charMap = new()
        {
            { 'à', "a" }, { 'å', "a" }, { 'á', "a" }, { 'â', "a" }, { 'ä', "a" }, { 'ã', "a" }, { 'ą', "a" },
            { 'è', "e" }, { 'é', "e" }, { 'ê', "e" }, { 'ë', "e" }, { 'ę', "e" },
            { 'ì', "i" }, { 'í', "i" }, { 'î', "i" }, { 'ï', "i" }, { 'ı', "i" },
            { 'ò', "o" }, { 'ó', "o" }, { 'ô', "o" }, { 'õ', "o" }, { 'ö', "o" }, { 'ø', "o" }, { 'ő', "o" }, { 'ð', "o" },
            { 'ù', "u" }, { 'ú', "u" }, { 'û', "u" }, { 'ü', "u" }, { 'ŭ', "u" }, { 'ů', "u" },
            { 'ç', "c" }, { 'ć', "c" }, { 'č', "c" }, { 'ĉ', "c" },
            { 'ż', "z" }, { 'ź', "z" }, { 'ž', "z" },
            { 'ś', "s" }, { 'ş', "s" }, { 'š', "s" }, { 'ŝ', "s" },
            { 'ñ', "n" }, { 'ń', "n" },
            { 'ý', "y" }, { 'ÿ', "y" },
            { 'ğ', "g" }, { 'ĝ', "g" },
            { 'ř', "r" }, { 'ł', "l" }, { 'đ', "d" }, { 'ß', "ss" },
            { 'Þ', "th" }, { 'ĥ', "h" }, { 'ĵ', "j" }
        }; 
        #endregion

#region REMAP INTERNATIONAL CHARS TO ASCII
        private static string RemapInternationalCharToAscii(char c)
        {
            // ensures that both uppercase and lowercase characters can be handled consistently
            // by the dictionary(which contains lowercase characters only).
            c = char.ToLowerInvariant(c);
            //If the character c is found in the dictionary,
            //it stores the corresponding value in the variable asciiEquivalent and returns true.
            //c is not found, TryGetValue returns false and asciiEquivalent is set to null or default.
            return charMap.TryGetValue(c, out var asciiEquivalent) ? asciiEquivalent : "";
        } 
        #endregion

    }
}
