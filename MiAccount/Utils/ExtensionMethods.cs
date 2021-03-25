using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MiAccount.Utils
{
    public static class StringExtensions
    {
        public static string Truncate(this string text, int maxLength, bool includeEllipsis = true)
        {
            if (text.Length <= maxLength) return text;

            return includeEllipsis ? text.Substring(0, maxLength - 3).Trim() + "..." : text.Substring(0, maxLength);
        }

        public static Stream ToStream(this string text)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(text ?? string.Empty));
        }
    }

    public static class CollectionExtensions
    {
        public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> source, int batchSize)
        {
            List<T> buffer = new List<T>(batchSize);

            foreach (T item in source)
            {
                buffer.Add(item);

                if (buffer.Count >= batchSize)
                {
                    yield return buffer;
                    buffer = new List<T>();
                }
            }
            if (buffer.Count >= 0)
            {
                yield return buffer;
            }
        }

        public static void BatchExecute<T>(this IEnumerable<T> keys, int batchSize, Action<List<T>> action)
        {
            List<T> batch = new List<T>();
            foreach (var key in keys)
            {
                batch.Add(key);
                if (batch.Count == batchSize)
                {
                    action(batch);
                    batch = new List<T>(); // in case the callback stores it
                }
            }
            if (batch.Count != 0)
            {
                action(batch); // any leftovers
            }
        }

        public static void WriteToConsole(this Exception e, string headline)
        {
            Console.WriteLine();
            Console.WriteLine(headline);
            Console.WriteLine("Error: {0}", e.Message);
            Console.WriteLine(e.Source);
            Console.WriteLine(e.StackTrace);
        }

    }

    public static class EnumerableExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable == null || !enumerable.Any();
        }

        // Making a second one for lists since they can be optimized
        public static bool IsNullOrEmpty<T>(this IList<T> enumerable)
        {
            return enumerable == null || enumerable.Count == 0;
        }
    }
}
