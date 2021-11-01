using System;
using System.Collections.Generic;
using System.Linq;

namespace CountingMatches
{
    public record SearchQuery(int Min, int Max);

    public class UniqueItem
    {
        public int Value { get; set; }
        public int Count { get; set; }
    };

    public enum Side
    {
        Left = 0,
        Right
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Solution(new[] { 5, 4, 3, 1, 9, 5, 9, 6, 3 }, "3-5,7-9,12"));
        }

        private static int Solution(IEnumerable<int> numbers, string search)
        {
            var queries = ParseSearch(search);

            var unique = numbers.ToList()
                .GroupBy(n => n)
                .Select(g => new UniqueItem { Value = g.Key, Count = g.Count() })
                .OrderBy(x => x.Value)
                .ToList();

            for (var i = 1; i < unique.Count; i++)
            {
                unique[i].Count += unique[i - 1].Count;
            }

            var answer = 0;

            var nums = unique.Select(x => x.Value).ToList();

            foreach (var (start, end) in queries)
            {
                var min = LowerIndex(nums, start);
                var max = UpperIndex(nums, end);
                if (min > 0)
                {
                    answer += unique[max].Count - unique[min - 1].Count;
                }
                else
                {
                    answer += unique[max].Count;
                }
            }

            return answer;
        }


        private static int LowerIndex(IList<int> arr,
            int x)
        {
            int l = 0, h = arr.Count - 1;
            while (l <= h)
            {
                var mid = (l + h) / 2;
                if (arr[mid] >= x)
                    h = mid - 1;
                else
                    l = mid + 1;
            }

            return l;
        }

        private static int UpperIndex(IList<int> arr, int x)
        {
            int l = 0, h = arr.Count - 1;
            while (l <= h)
            {
                var mid = (l + h) / 2;
                if (arr[mid] <= x)
                    l = mid + 1;
                else
                    h = mid - 1;
            }

            return h;
        }

        private static IList<SearchQuery> ParseSearch(string search)
        {
            var queries = search.Split(',');

            return queries.Select(x => x.Contains('-')
                ? new SearchQuery(Convert.ToInt32(x.Split('-')[0]), Convert.ToInt32(x.Split('-')[1]))
                : new SearchQuery(Convert.ToInt32(x), Convert.ToInt32(x))
            ).ToList();
        }
    }
}