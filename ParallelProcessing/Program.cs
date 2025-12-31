using CSV;
using ParallelProcessing;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace AutoMapper
{
    internal class Program
    {
        private static readonly object key = new object();
        private static readonly SemaphoreSlim Locker = new SemaphoreSlim(1, 1);
        private static readonly CancellationTokenSource cts = new CancellationTokenSource();
        static async Task Main(string[] args)
        {


            //Parallel.For(0, 10000, x =>
            //{
            //    int threadId = Thread.CurrentThread.ManagedThreadId;
            //    counter.Add(threadId);

            //});

            //var threads = counter.GroupBy(x => x).Select(x => new
            //{
            //    ThreadID = x.Key,
            //    Count = x.Count()
            //}).ToList();

            //int ROW_COUNT = 3_000_000;
            //int ROW_COUNT = 4_000_000;
            //int ROW_COUNT = 5_000_000;
            //int ROW_COUNT = 7_500_000;
            //int ROW_COUNT = 10_000_000;
            //int ROW_COUNT = 12_000_000;
            //int ROW_COUNT = 13_000_000;
            //int ROW_COUNT = 14_000_000;
            int ROW_COUNT = 15_000_000;
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
            await Parallel.ForAsync(0, times, (x, token) =>
            {
                int index = x;
                int start = index * batchcount;
                int end = index * batchcount + batchcount;
                Console.WriteLine($"第{index + 1}批資料讀取中，從第{start}到第{end}資料");
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                List<DataModel> datas = CSVHelper.Read<DataModel>(InputFilePath, start, batchcount);
                Console.WriteLine($"第{index + 1}批資料讀取完成");
                stopwatch.Stop();
                readTimes.Add(stopwatch.Elapsed.TotalSeconds);
                //==
                stopwatch.Restart();
                Console.WriteLine($"第{index + 1}批資料寫入中，從第{start}到第{end}資料");
                CSVHelper.Write<DataModel>(@$"{OutputDir}\MOCK_DATA_{index}.csv", datas);

                //lock (key)
                //{
                //    CSVHelper.Write<DataModel>(OutputFilePath, datas);
                //}
                Console.WriteLine($"第{index + 1}批資料寫入完成");
                stopwatch.Stop();
                writeTimes.Add(stopwatch.Elapsed.TotalSeconds);
                return ValueTask.CompletedTask;
            });

            totalStopwatch.Stop();
            Console.WriteLine($"|  {batchcount.ToString("#,##0")}     | {ROW_COUNT.ToString("#,##0")}     |{Math.Round(EnumerableExtension.Median(readTimes), 2)}            |     {Math.Round(EnumerableExtension.Median(writeTimes), 2)}            |     {Math.Round(totalStopwatch.ElapsedMilliseconds / 1000.0, 2)}          |                |");

            #endregion

            #region Mutex Lock SemaphoreSlim
            for (int i = 0; i < times; i++)
            {
                //int index = i;
                //int start = index * batchcount;
                //int end = index * batchcount + batchcount;
                //Console.WriteLine($"第{index + 1}批資料讀取中，從第{start}到第{end}資料");
                #region Mutex
                //tasks.Add(Task.Run(() =>
                //{
                //    Stopwatch stopwatch = new Stopwatch();
                //    stopwatch.Start();
                //    List<DataModel> datas = CSVHelper.Read<DataModel>(InputFilePath, start, batchcount);
                //    Console.WriteLine($"第{index + 1}批資料讀取完成");
                //    stopwatch.Stop();
                //    readTimes.Add(stopwatch.Elapsed.TotalSeconds);
                //    stopwatch.Restart();
                //    Console.WriteLine($"第{index + 1}批資料寫入中，從第{start}到第{end}資料");
                //    // 這裡改成 true，代表「如果是我建立的，就直接鎖起來」
                //    // Change to true here, meaning "if I created it, lock it immediately"
                //    Mutex mutex = new Mutex(false, "Global\\MyAppUniqueMutex_987654321", out _);

                //    // 我是第一個，且因為上面傳 true，我已經拿到鎖了
                //    // I am the first one, and since true was passed above, I already have the lock
                //    mutex.WaitOne();
                //    CSVHelper.Write<DataModel>(OutputFilePath, datas);
                //    // 結束時記得釋放 (Release is handled by Dispose in using or manually)
                //    // 注意：在某些情況下，手動呼叫 mutex.ReleaseMutex() 會更保險
                //    mutex.ReleaseMutex();
                //    mutex.Dispose();

                //    Console.WriteLine($"第{index + 1}批資料寫入完成");
                //    stopwatch.Stop();
                //    writeTimes.Add(stopwatch.Elapsed.TotalSeconds);
                //}));
                #endregion
                #region Lock
                //tasks.Add(Task.Run(() =>
                //{
                //    Stopwatch stopwatch = new Stopwatch();
                //    stopwatch.Start();
                //    List<DataModel> datas = CSVHelper.Read<DataModel>(InputFilePath, start, batchcount);
                //    Console.WriteLine($"第{index + 1}批資料讀取完成");
                //    stopwatch.Stop();
                //    readTimes.Add(stopwatch.Elapsed.TotalSeconds);
                //    //==
                //    stopwatch.Restart();
                //    Console.WriteLine($"第{index + 1}批資料寫入中，從第{start}到第{end}資料");
                //    //lock (key)
                //    //{
                //    CSVHelper.Write<DataModel>(@$"{OutputDir}\MOCK_DATA_{index}.csv", datas);
                //    //}
                //    Console.WriteLine($"第{index + 1}批資料寫入完成");
                //    stopwatch.Stop();
                //    writeTimes.Add(stopwatch.Elapsed.TotalSeconds);
                //}));
                #endregion
                #region SemaphoreSlim
                //tasks.Add(Task.Run(async () =>
                //{

                //    Stopwatch stopwatch = new Stopwatch();
                //    stopwatch.Start();
                //    List<DataModel> datas = await CSVHelper.ReadAsync<DataModel>(InputFilePath, start, batchcount, cts.Token);
                //    Console.WriteLine($"第{index + 1}批資料讀取完成");
                //    stopwatch.Stop();
                //    readTimes.Add(stopwatch.Elapsed.TotalSeconds);
                //    //==
                //    stopwatch.Restart();
                //    Console.WriteLine($"第{index + 1}批資料寫入中，從第{start}到第{end}資料");
                //    await Locker.WaitAsync();//紅綠燈的概念
                //    //CSVHelper.Write<DataModel>(OutputFilePath, datas);
                //    await Task.Run(
                //        () =>
                //        {
                //            CSVHelper.WriteAsync<DataModel>(OutputFilePath, datas, cts.Token);
                //        });
                //    Locker.Release();
                //    Console.WriteLine($"第{index + 1}批資料寫入完成");
                //    stopwatch.Stop();
                //    writeTimes.Add(stopwatch.Elapsed.TotalSeconds);
                //}));
                #endregion
            }
            #endregion
            //await Task.WhenAll(tasks); // Mutex Lock SemaphoreSlim 搭配用
            //totalStopwatch.Stop();
            //Console.WriteLine($"|  {batchcount.ToString("#,##0")}     | {ROW_COUNT.ToString("#,##0")}     |{Math.Round(EnumerableExtension.Median(readTimes), 2)}            |     {Math.Round(EnumerableExtension.Median(writeTimes), 2)}            |     {Math.Round(totalStopwatch.ElapsedMilliseconds / 1000.0, 2)}          |                |");

            Console.ReadKey();
        }

        public async Task<List<DataModel>> ReadDatasAsync(string filePath, int startLine, int count)
        {
            // CSV 讀取
            List<DataModel> datas = CSVHelper.Read<DataModel>(filePath, startLine, count);
            return datas;
        }



    }
}