using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchmarkDotnet效能測試好簡單
{
    internal class Program
    {
        static void Main(string[] args)
        {

            //StringBuilder sb = new StringBuilder("37,Breena,Dumbelton,bdumbelton10@constantcontact.com,Female,15.34.171.241", 90);
            //char[] buffer = new char[90];
            //sb.CopyTo(0, buffer, 0, sb.Length);
            //StreamWriter writer = new StreamWriter("D:\\c#_Leo老師\\CSVData\\test\\test.csv", true);
            //writer.WriteLine(buffer, 0, sb.Length);
            //writer.Flush();
            //writer.Close();
            //sb.Clear();


            //var summary = BenchmarkRunner.Run<Read_VS_OptimizeRead>();
            var summary = BenchmarkRunner.Run<Write_VS_OptimizeWrite>();
            #region 我的方法
            //string data = "37,Breena,Dumbelton,bdumbelton10@constantcontact.com,Female,15.34.171.241";
            //ReadOnlySpan<char> chars = data.AsSpan();
            //var props = typeof(CSVMockData).GetProperties();
            //string[] datas = new string[6];
            //int arrayIndex = 0;
            //int startIndex = 0;
            //int count = 0;
            ////i=2=>  0,2
            ////i=9=>  3,6
            ////i=19=> 10,9
            //for (int i = 0; i < data.Length; i++)
            //{
            //    count++;
            //    if (data[i] == ',')
            //    {
            //        datas[arrayIndex] = chars.Slice(startIndex, count - 1).ToString();
            //        startIndex = i + 1;
            //        count = 0;
            //        arrayIndex++;
            //    }
            //    if (i == data.Length - 1)
            //    {
            //        datas[arrayIndex] = chars.Slice(startIndex).ToString();
            //    }
            //}
            //CSVMockData cSVMockData = new CSVMockData();
            //for (int i = 0; i < props.Length; i++)
            //{
            //    props[i].SetValue(cSVMockData, datas[i]);
            //}
            #endregion
            Console.ReadKey();
        }
    }
}
