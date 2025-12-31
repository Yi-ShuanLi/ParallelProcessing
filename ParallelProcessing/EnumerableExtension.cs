using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelProcessing
{
    internal static class EnumerableExtension
    {
        public static double Median(this IEnumerable<double> source)
        {
            if (source is null || !source.Any())
            {
                throw new InvalidOperationException("Cannot compute median for a null or empty set.");
            }

            var sortedList =
                source.OrderBy(number => number).ToList();

            int itemIndex = sortedList.Count / 2;

            if (sortedList.Count % 2 == 0)
            {
                // Even number of items.
                return (sortedList[itemIndex] + sortedList[itemIndex - 1]) / 2;
            }
            else
            {
                // Odd number of items.
                return sortedList[itemIndex];
            }
        }

        // 這就是我們自己發明的 .StandardDeviation() 方法
        // This is the .StandardDeviation() method we invented
        public static double StandardDeviation(this IEnumerable<double> values)
        {
            // 1. 如果沒有資料，就回傳 0 (防呆機制)
            // If no data, return 0 (Error handling)
            if (!values.Any()) return 0;

            // 2. 算出平均值 (Step 1: Average)
            double avg = values.Average();

            // 3. 算出差距的平方並加總 (Step 2 & 3: Sum of squared differences)
            // 這裡用 LINQ 的 Sum 搭配 Lambda 表達式，寫法超簡潔！
            double sumOfSquares = values.Sum(d => Math.Pow(d - avg, 2));

            // 4. 除以總數 (Step 4: Divide by N)
            // 注意：如果是「樣本標準差」這裡要用 values.Count() - 1
            double variance = sumOfSquares / values.Count();

            // 5. 開根號 (Step 5: Square Root)
            return Math.Sqrt(variance);
        }
    }
}
