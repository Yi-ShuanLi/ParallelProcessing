using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchmarkDotnet效能測試好簡單
{
    internal class Program
    {
        static void Main(string[] args)
        {
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
