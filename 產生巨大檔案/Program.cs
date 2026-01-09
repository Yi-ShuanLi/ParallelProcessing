using CSV;
using System.Diagnostics;

namespace 產生巨大檔案
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
            //int ROW_COUNT = 20_000_000;
            //int ROW_COUNT = 24_000_000;
            //int ROW_COUNT = 30_000_000;
            //int ROW_COUNT = 40_000_000;
            //int ROW_COUNT = 48_000_000;
            //int ROW_COUNT = 54_000_000;
            //int ROW_COUNT = 60_000_000;
            //int ROW_COUNT = 72_000_000;
            //int ROW_COUNT = 80_000_000;
            //int ROW_COUNT = 100_000_000;

            int ROW_COUNT1 = 80_000_000;
            string InputFilePath1 = $@"D:\c#_Leo老師\CSVData\Input_DATA\MOCK_DATA_{ROW_COUNT1}.csv";
            int ROW_COUNT2 = 20_000_000;
            string InputFilePath2 = $@"D:\c#_Leo老師\CSVData\Input_DATA\MOCK_DATA_{ROW_COUNT2}.csv";
            string OutputFilePath = $@"D:\c#_Leo老師\CSVData\Input_DATA\MOCK_DATA_{ROW_COUNT1 + ROW_COUNT2}.csv";

            string OutputDir = $@"D:\c#_Leo老師\CSVData\Output_DATA\{ROW_COUNT1 + ROW_COUNT2}";

            List<DataModel> datas = CSVHelper.OptimizeRead<DataModel>(InputFilePath1, 0, ROW_COUNT1);
            List<DataModel> datas2 = CSVHelper.OptimizeRead<DataModel>(InputFilePath2, 0, ROW_COUNT2);
            datas.AddRange(datas2);

            CSVHelper.OptimizeWrite<DataModel>(OutputFilePath, datas);
            //CSVHelper.Write<DataModel>(OutputFilePath, datas);

            //if (File.Exists(OutputFilePath))
            //{
            //    File.Delete(OutputFilePath);
            //}

            //if (Directory.Exists(OutputDir))
            //{
            //    Directory.Delete(OutputDir, true);
            //    Directory.CreateDirectory(OutputDir);
            //}
            //else
            //{
            //    Directory.CreateDirectory(OutputDir);

            //}
            //int batchcount = 2_500_000;
            //int times1 = ROW_COUNT1 % batchcount == 0 ? ROW_COUNT1 / batchcount : (ROW_COUNT1 / batchcount) + 1;

            //#region Parallel.For 
            //await Parallel.ForAsync(0, times1, new ParallelOptions() { MaxDegreeOfParallelism = 6 }, (x, token) =>
            //{
            //    int index = x;
            //    int start = index * batchcount;
            //    int end = index * batchcount + batchcount;
            //    Console.WriteLine($"第{index + 1}批資料讀取中，從第{start}到第{end}資料");

            //    List<DataModel> datas = CSVHelper.OptimizeRead<DataModel>(InputFilePath1, start, batchcount);
            //    Console.WriteLine($"第{index + 1}批資料讀取完成");

            //    //==             
            //    Console.WriteLine($"第{index + 1}批資料寫入中，從第{start}到第{end}資料");
            //    //CSVHelper.OptimizeWrite<DataModel>(@$"{OutputDir}\MOCK_DATA_{index}.csv", datas);
            //    lock (key)
            //    {
            //        CSVHelper.OptimizeWrite<DataModel>(OutputFilePath, datas);
            //        //CSVHelper.Write<DataModel>(OutputFilePath, datas);
            //    }
            //    Console.WriteLine($"第{index + 1}批資料寫入完成");

            //    return ValueTask.CompletedTask;
            //});
            //int times2 = ROW_COUNT2 % batchcount == 0 ? ROW_COUNT2 / batchcount : (ROW_COUNT2 / batchcount) + 1;

            //await Parallel.ForAsync(0, times2, new ParallelOptions() { MaxDegreeOfParallelism = 6 }, (x, token) =>
            //{
            //    int index = x;
            //    int start = index * batchcount;
            //    int end = index * batchcount + batchcount;
            //    Console.WriteLine($"第{index + 1}批資料讀取中，從第{start}到第{end}資料");

            //    List<DataModel> datas = CSVHelper.OptimizeRead<DataModel>(InputFilePath2, start, batchcount);
            //    Console.WriteLine($"第{index + 1}批資料讀取完成");

            //    //==

            //    Console.WriteLine($"第{index + 1}批資料寫入中，從第{start}到第{end}資料");
            //    //CSVHelper.OptimizeWrite<DataModel>(@$"{OutputDir}\MOCK_DATA_{index}.csv", datas);
            //    lock (key)
            //    {
            //        CSVHelper.OptimizeWrite<DataModel>(OutputFilePath, datas);
            //        //CSVHelper.Write<DataModel>(OutputFilePath, datas);
            //    }
            //    Console.WriteLine($"第{index + 1}批資料寫入完成");

            //    return ValueTask.CompletedTask;
            //});


            // #endregion



            //Console.ReadKey();
        }

        public async Task<List<DataModel>> ReadDatasAsync(string filePath, int startLine, int count)
        {
            // CSV 讀取
            List<DataModel> datas = CSVHelper.Read<DataModel>(filePath, startLine, count);
            return datas;
        }



    }
}
