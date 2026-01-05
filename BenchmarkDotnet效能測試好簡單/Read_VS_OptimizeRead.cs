using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BenchmarkDotnet效能測試好簡單
{
    [MemoryDiagnoser]
    public class Read_VS_OptimizeRead
    {
        static string data = "37,Breena,Dumbelton,bdumbelton10@constantcontact.com,Female,15.34.171.241";
        static PropertyInfo[] props = typeof(CSVMockData).GetProperties();

        //給方法取用的參數入口，目標物件，值
        delegate void SetterDelegte(object data, object value);
        //propInfo從這帶入，最後包成SetterDelegte結果
        static SetterDelegte[] setters = props.Select(x => CreateSetter(x)).ToArray();

        static SetterDelegte CreateSetter(PropertyInfo propertyInfo)
        {
            var targetParm = Expression.Parameter(typeof(object));
            var valueParm = Expression.Parameter(typeof(object));
            //目標物件的欄位，與物件mapping轉型 => SetterDelegte.id，Func包成Expression
            Expression castTarget = Expression.Convert(targetParm, propertyInfo.DeclaringType);
            //欄位的資料型態，string => 
            Expression castValue = Expression.Convert(valueParm, propertyInfo.PropertyType);
            //建立MethodCallExpression
            MethodCallExpression methodCall = Expression.Call(castTarget, propertyInfo.GetSetMethod(), castValue);
            //把MethodCallExpression，包起來Expression，最後return SetterDelegte 回去變成SetterDelegte array的方法陣列
            SetterDelegte delegte = Expression.Lambda<SetterDelegte>(methodCall, targetParm, valueParm).Compile();
            return delegte;
        }



        [Benchmark]
        public void Read()
        {
            for (int j = 0; j < 2_500_000; j++)
            {
                string[] datas = data.Split(',');
                CSVMockData cSVMockData = new CSVMockData();
                for (int i = 0; i < props.Length; i++)
                {
                    props[i].SetValue(cSVMockData, datas[i]);
                }
            }
        }

        [Benchmark]
        public void OptimizeRead()
        {
            for (int j = 0; j < 2_500_000; j++)
            {
                ReadOnlySpan<char> strings = data.AsSpan();
                int current = 0;
                int field = 0;
                string[] datas = new string[6];
                CSVMockData cSVMockData = new CSVMockData();
                while (true)
                {
                    int num = strings.Slice(current).IndexOf(',');
                    if (num == -1)
                    {
                        datas[field++] = strings.Slice(current).ToString();
                        break;
                    }
                    else
                    {
                        datas[field++] = strings.Slice(current, num).ToString();
                        current += num + 1;
                    }
                }

                for (int i = 0; i < props.Length; i++)
                {
                    setters[i](cSVMockData, datas[i]);
                    //props[i].SetValue();
                }
            }

        }
    }
}
