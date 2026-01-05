using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BenchmarkDotnet效能測試好簡單
{
    [MemoryDiagnoser]
    public class Write_VS_OptimizeWrite
    {
        static CSVMockData cSVMockData = new CSVMockData
        {
            id = "1",
            first_name = "Li",
            last_name = "I-HSUAN",
            email = "apple123@gmail.com",
            gender = "femal",
            ip_address = "192.168.0.1"
        };

        static PropertyInfo[] props = typeof(CSVMockData).GetProperties();
        //給方法取用的參數入口，目標物件，值
        delegate object GetterDelegte(object sourceItem);
        //propInfo從這帶入，最後包成SetterDelegte結果
        static GetterDelegte[] getter = props.Select(x => CreateGetter(x)).ToArray();

        static GetterDelegte CreateGetter(PropertyInfo propertyInfo)
        {
            var sourceItemParm = Expression.Parameter(typeof(object));
            //目標物件的欄位，與物件mapping轉型 => SetterDelegte.id，Func包成Expression
            Expression sourcePropInfoExpression = Expression.Convert(sourceItemParm, propertyInfo.DeclaringType);
            //建立MethodCallExpression
            MethodCallExpression methodCall = Expression.Call(sourcePropInfoExpression, propertyInfo.GetGetMethod());
            //把MethodCallExpression，包起來Expression，最後return SetterDelegte 回去變成SetterDelegte array的方法陣列
            GetterDelegte delegte = Expression.Lambda<GetterDelegte>(methodCall, sourceItemParm).Compile();
            return delegte;
        }

        [Benchmark]
        public void Write()
        {
            string dataLine = "";
            for (int i = 0; i < props.Length; i++)
            {
                dataLine += props[i].GetValue(cSVMockData).ToString() + ",";
                //if(i < props.Length - 1)
                //{
                //    dataLine += ",";
                //}
            }
            dataLine = dataLine.TrimEnd(',');
            //string dataLine = String.Join(",", props.Select(x => x.GetValue(cSVMockData).ToString()));
        }

        [Benchmark]
        public void OptimizeWrite()
        {
            string dataLine = "";
            for (int i = 0; i < props.Length; i++)
            {
                dataLine += getter[i](cSVMockData).ToString() + ",";
                //dataLine += props[i].GetValue(cSVMockData).ToString() + ",";
                //if (i < props.Length - 1)
                //{
                //    dataLine += ",";
                //}
            }
            dataLine = dataLine.TrimEnd(',');
        }

    }
}
