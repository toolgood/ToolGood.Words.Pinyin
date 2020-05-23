using System;

namespace ToolGood.Words.Pinyin.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            var r = FirstPinyin.WordsHelper.GetFirstPinyinList("我爱中国");
            //var r = WordsHelper.GetPinyin("我爱中国");
            stopwatch.Stop();
            var s = stopwatch.ElapsedMilliseconds;
            Console.WriteLine(s);
            FirstPinyin.WordsHelper.ClearCache();
            PetaTest.Runner.RunMain(args);
        }
    }
}
