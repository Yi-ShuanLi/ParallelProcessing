using CSV;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Append檔案
{
    internal class Program
    {
        private static readonly object key = new object();
        private static readonly SemaphoreSlim Locker = new SemaphoreSlim(1, 1);
        private static readonly CancellationTokenSource cts = new CancellationTokenSource();
        static async Task Main(string[] args)
        {
            //int ROW_COUNT = 3_000_000;
            //int ROW_COUNT = 4_000_000;
            //int ROW_COUNT = 5_000_000;
            //int ROW_COUNT = 7_500_000;
            //int ROW_COUNT = 10_000_000;
            //int ROW_COUNT = 12_000_000;
            //int ROW_COUNT = 13_000_000;
            //int ROW_COUNT = 14_000_000;
            //int ROW_COUNT = 15_000_000;



            //int ROW_COUNT = 10_000_000;
            //int ROW_COUNT = 12_000_000;
            int ROW_COUNT = 20_000_000;
            //int ROW_COUNT = 24_000_000;
            //int ROW_COUNT = 30_000_000;
            //int ROW_COUNT = 40_000_000;
            //int ROW_COUNT = 48_000_000;
            //int ROW_COUNT = 54_000_000;
            //int ROW_COUNT = 60_000_000;
            //int ROW_COUNT = 72_000_000;
            //int ROW_COUNT = 80_000_000;
            //int ROW_COUNT = 100_000_000;
            string InputFilePath = $@"D:\c#_Leo老師\CSVData\Input_DATA\MOCK_DATA_{ROW_COUNT}.csv";
            string OutputFilePath = $@"D:\c#_Leo老師\CSVData\Output_DATA\MOCK_DATA_{ROW_COUNT}.csv";

            string OutputDir = $@"D:\c#_Leo老師\CSVData\Output_DATA\{ROW_COUNT}";
            if (File.Exists(OutputFilePath))
            {
                File.Delete(OutputFilePath);
            }

            if (Directory.Exists(OutputDir))
            {
                Directory.Delete(OutputDir, true);
                Directory.CreateDirectory(OutputDir);
            }
            else
            {
                Directory.CreateDirectory(OutputDir);

            }

            //int batchcount = 1_000_000;
            //int batchcount = 1_500_000;
            //int batchcount = 2_000_000;
            int batchcount = 2_500_000;
            //int batchcount = 3_000_000;
            //int batchcount = 3_500_000;
            //int batchcount = 4_000_000;
            //int batchcount = 4_500_000;
            //int batchcount = 5_000_000;
            //int batchcount = 5_500_000;
            //int batchcount = 6_000_000;
            //int batchcount = 6_500_000;
            //int batchcount = 7_000_000;
            //int batchcount = 7_500_000;
            //int batchcount = 8_000_000;
            //int batchcount = 8_500_000;
            //int batchcount = 9_000_000;
            //int batchcount = 9_500_000;
            //int batchcount = 10_000_000;
            //int batchcount = 10_500_000;
            //int batchcount = 11_000_000;
            //int batchcount = 11_500_000;
            //int batchcount = 12_000_000;
            //int batchcount = 12_500_000;
            //int batchcount = 13_000_000;
            //int batchcount = 13_500_000;
            //int batchcount = 14_000_000;
            //int batchcount = 14_500_000;
            //int batchcount = 15_000_000;
            //cts.Token.Register(() =>
            //{
            //    // 這裡通常用來寫 Log 或顯示訊息
            //    // Used to log or show messages
            //    Debug.WriteLine("=== 鬧鐘響了！工作因為超時被取消了！ ===");
            //    Console.WriteLine("=== 鬧鐘響了！工作因為超時被取消了！ ===");
            //    Console.WriteLine("時間到！工作自動停止。(Time is up!)");
            //});
            int times = ROW_COUNT % batchcount == 0 ? ROW_COUNT / batchcount : (ROW_COUNT / batchcount) + 1;
            List<double> readTimes = new List<double>();
            List<double> writeTimes = new List<double>();
            List<Task> tasks = new List<Task>();
            Stopwatch totalStopwatch = new Stopwatch();
            totalStopwatch.Start();
            #region Parallel.For 
            await Parallel.ForAsync(0, times, new ParallelOptions() { MaxDegreeOfParallelism = 6 }, (x, token) =>
            {
                int index = x;
                int start = index * batchcount;
                int end = index * batchcount + batchcount;
                Console.WriteLine($"第{index + 1}批資料讀取中，從第{start}到第{end}資料");
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                List<DataModel> datas = CSVHelper.OptimizeRead<DataModel>(InputFilePath, start, batchcount);
                Console.WriteLine($"第{index + 1}批資料讀取完成");
                stopwatch.Stop();
                readTimes.Add(stopwatch.Elapsed.TotalSeconds);
                //==
                stopwatch.Restart();
                Console.WriteLine($"第{index + 1}批資料寫入中，從第{start}到第{end}資料");
                //CSVHelper.OptimizeWrite<DataModel>(@$"{OutputDir}\MOCK_DATA_{index}.csv", datas);

                lock (key)
                {
                    CSVHelper.OptimizeWrite<DataModel>(OutputFilePath, datas);
                    //CSVHelper.Write<DataModel>(OutputFilePath, datas);
                }
                Console.WriteLine($"第{index + 1}批資料寫入完成");
                stopwatch.Stop();
                writeTimes.Add(stopwatch.Elapsed.TotalSeconds);
                return ValueTask.CompletedTask;
            });


        }
    }
}
