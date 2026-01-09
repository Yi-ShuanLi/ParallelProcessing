using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 計算行數
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 請將這裡改成你合併後的檔案路徑
            // Please change this to your merged file path
            //string filePath =@"D:\c#_Leo老師\CSVData\Input_DATA\MOCK_DATA_20000000.csv";
            string filePath = @"D:\c#_Leo老師\CSVData\Input_DATA\MOCK_DATA_30000000.csv";

            if (!File.Exists(filePath))
            {
                Console.WriteLine("找不到檔案！請檢查路徑。");
                return;
            }

            Console.WriteLine($"開始計算檔案行數：{filePath}");

            // 準備計時器 (Stopwatch to measure time)
            Stopwatch sw = Stopwatch.StartNew();

            long count = 0; // 用 long 以防檔案超過 21 億行 (Use long in case lines exceed 2 billion)

            // 建立讀取水管 (Create a reading stream)
            using (StreamReader reader = new StreamReader(filePath))
            {
                // 只要還讀得到東西，就繼續 (While there is content to read)
                while (reader.ReadLine() != null)
                {
                    count++;

                    // 每 1,000,000 行顯示一次進度，讓你知道程式沒當機
                    // Show progress every 1,000,000 lines
                    if (count % 1000000 == 0)
                    {
                        Console.WriteLine($"目前已讀取: {count} 行...");
                    }
                }
            }

            sw.Stop();
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine($"計算完成！(Calculation Complete!)");
            Console.WriteLine($"總行數 (Total Lines): {count}");
            Console.WriteLine($"花費時間 (Time Elapsed): {sw.Elapsed.TotalSeconds} 秒");
        }
    }
}
