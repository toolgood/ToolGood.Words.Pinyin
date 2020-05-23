using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaTest;
using WordsHelper = ToolGood.Words.FirstPinyin.WordsHelper;

namespace ToolGood.Words.Pinyin2.Test.FirstPinyinTests
{
    [TestFixture]
    class FirstPinyinWordHelperTest
    {
        [Test]
        public void GetPinyin()
        {
            var t = WordsHelper.GetAllFirstPinyin('芃');
            Assert.AreEqual("P", t[0]);


            var b = WordsHelper.GetFirstPinyin("摩擦棒");
            Assert.AreEqual("MCB", b);

            b = WordsHelper.GetFirstPinyin("秘鲁");
            Assert.AreEqual("BL", b);

            b = WordsHelper.GetFirstPinyin("天行");
            Assert.AreEqual("TX", b);

            var py = WordsHelper.GetFirstPinyin("快乐，乐清");
            Assert.AreEqual("KL，YQ", py);

            py = WordsHelper.GetFirstPinyin("快乐清理");
            Assert.AreEqual("KLQL", py);


            py = WordsHelper.GetFirstPinyin("我爱中国");
            Assert.AreEqual("WAZG", py);

            py = WordsHelper.GetFirstPinyin("我爱中国", ",");
            Assert.AreEqual("W,A,Z,G", py);


            py = WordsHelper.GetFirstPinyin("我爱中国");
            Assert.AreEqual("WAZG", py);

            var pys = WordsHelper.GetAllFirstPinyin('传');
            Assert.AreEqual("C", pys[0]);
            Assert.AreEqual("Z", pys[1]);

            py = WordsHelper.GetFirstPinyinForName("单一一");
            Assert.AreEqual("SYY", py);

            py = WordsHelper.GetFirstPinyinForName("单一一", ",");
            Assert.AreEqual("S,Y,Y", py);

            WordsHelper.ClearCache();

        }


        [Test]
        public void HasChinese()
        {
            var b = WordsHelper.HasChinese("xhdsf");
            Assert.AreEqual(false, b);

            var c = WordsHelper.HasChinese("我爱中国");
            Assert.AreEqual(true, c);

            var d = WordsHelper.HasChinese("I爱中国");
            Assert.AreEqual(true, d);
        }


    }
}
