using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Helpers
{
    public static class StringExtensions
    {
        public static string AsMoney(this decimal val)
        {
            return String.Format("{0:$#,###,###,##0.00}", val);
        }

        public static bool HasValue(this string str)
        {
            return !string.IsNullOrWhiteSpace(str);
        }

        public static bool IsEmpty(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        public static bool Has(this string str, string search)
        {
            return str.HasValue() && str.Contains(search);
        }

        public static bool Is(this string str, string compareTo)
        {
            return str.HasValue() && str.Equals(compareTo, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsAny(this string str, IEnumerable<string> list)
        {
            return str.HasValue() && list.Any(x => x.Is(str));
        }

        public static string Description(this int val)
        {
            if (val != 11 && val % 10 == 1)
                return $"{val}st";

            if (val != 12 && val % 10 == 2)
                return $"{val}nd";

            if (val != 13 && val % 10 == 3)
                return $"{val}rd";

            return $"{val}th";
        }

        public static bool IsEmail(this string str)
        {
            if (str.IsEmpty())
                return false;
            return str.Contains("@") &&
                str.Length > 3
                && str.IndexOf("@") > 0
                && !str.EndsWith("@");
        }

        public static T Copy<T>(this T obj) where T : class
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }

        public static byte[] GetHash(this string inputString)
        {
            HashAlgorithm algorithm = SHA256.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string GetHashString(this string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        public static string NullIf(this string str, string nullIf)
        {
            if (str.HasValue())
                if (str.Equals(nullIf, StringComparison.OrdinalIgnoreCase))
                    return null;
            return str;
        }

        public static string Combine(this string root, string path, params string[] args)
        {
            if (path.StartsWith("/", StringComparison.Ordinal))
            {
                path = path.Substring(1);
            }
            if (args == null || args.Length == 0)
            {
                return Path.Combine(root, path);
            }
            else
            {
                string newRoot = Path.Combine(root, path);
                foreach (var item in args)
                {
                    newRoot = newRoot.Combine(item);
                }
                return newRoot;
            }
        }

        public static string CleanupJson(this string text)
        {
            var tempText = System.Text.RegularExpressions.Regex.Unescape(text);
            if (tempText.StartsWith(@"""") && tempText.EndsWith(@""""))
            {
                tempText = tempText.Substring(1, tempText.Length - 2);
            }
            return tempText;
        }

        public static bool Contains(this string text, string compareText, StringComparer stringComparer)
        {
            return text.ToLower().Contains(compareText.ToLower());
        }


        public static string StripNonASCII(this string source)
        {
            if (source.IsEmpty())
                return source;

            return new string(source.Where(x => x < 128).ToArray());
        }

        /// <summary>
        /// Used for transaction matching
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string StripNonCharacters(this string source)
        {
            if (source.IsEmpty())
                return source;

            var result = new string(source.Where(x => Char.IsLetter(x)).ToArray());

            return result;
        }

        //public static string Enc(this string str)
        //{
        //    return System.Web.HttpUtility.UrlEncode(str);
        //}

        public static int AsInt(this string str)
        {
            int output = 0;
            if (int.TryParse(str, out output))
                return output;
            else
                return -1;
        }

        public static long AsLong(this string str)
        {
            long output = 0;
            if (long.TryParse(str, out output))
                return output;
            else
                return -1;
        }

        public static string FixedDecimals(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return str;

            while (str.Count(x => x == '.') > 1)
            {
                str = str.Remove(str.LastIndexOf('.'), 1);
            }
            return str;
        }

        public static decimal AsDecimal(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return 0;
            }

            var temp = new string(str.Where(x => Char.IsDigit(x) || x == '.' || x == '-').ToArray());
            if (string.IsNullOrWhiteSpace(temp))
            {
                return 0;
            }
            else
            {
                // multiple decimals? Drop the last one
                return decimal.Parse(temp.FixedDecimals());
            }
        }


        /// <summary>
        /// Calculate percentage similarity of two strings
        /// <param name="source">Source String to Compare with</param>
        /// <param name="target">Targeted String to Compare</param>
        /// <returns>Return Similarity between two strings from 0 to 1.0</returns>
        /// </summary>
        public static double CalculateSimilarity(this string source, string target)
        {
            if ((source == null) || (target == null)) return 0.0;
            if ((source.Length == 0) || (target.Length == 0)) return 0.0;
            if (source == target) return 1.0;

            int stepsToSame = ComputeLevenshteinDistance(source, target);
            return (1.0 - ((double)stepsToSame / (double)Math.Max(source.Length, target.Length)));
        }
        /// <summary>
        /// Returns the number of steps required to transform the source string
        /// into the target string.
        /// </summary>
        static int ComputeLevenshteinDistance(string source, string target)
        {
            if ((source == null) || (target == null)) return 0;
            if ((source.Length == 0) || (target.Length == 0)) return 0;
            if (source == target) return source.Length;

            int sourceWordCount = source.Length;
            int targetWordCount = target.Length;

            // Step 1
            if (sourceWordCount == 0)
                return targetWordCount;

            if (targetWordCount == 0)
                return sourceWordCount;

            int[,] distance = new int[sourceWordCount + 1, targetWordCount + 1];

            // Step 2
            for (int i = 0; i <= sourceWordCount; distance[i, 0] = i++) ;
            for (int j = 0; j <= targetWordCount; distance[0, j] = j++) ;

            for (int i = 1; i <= sourceWordCount; i++)
            {
                for (int j = 1; j <= targetWordCount; j++)
                {
                    // Step 3
                    int cost = (target[j - 1] == source[i - 1]) ? 0 : 1;

                    // Step 4
                    distance[i, j] = Math.Min(Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1), distance[i - 1, j - 1] + cost);
                }
            }

            return distance[sourceWordCount, targetWordCount];

        }
    }
}
