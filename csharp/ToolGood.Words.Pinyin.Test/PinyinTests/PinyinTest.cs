using System;
using System.Collections.Generic;
using System.Text;
using PetaTest;

namespace ToolGood.Words.Pinyin.Test
{
    [TestFixture]
    class WordHelperTest
    {
        [Test]
        public void GetPinyin()
        {
            var t = WordsHelper.GetAllPinyin('芃');
            Assert.AreEqual("Peng", t[0]);


            var b = WordsHelper.GetPinyin("摩擦棒");
            Assert.AreEqual("MoCaBang", b);

            b = WordsHelper.GetPinyin("秘鲁");
            Assert.AreEqual("BiLu", b);

            b = WordsHelper.GetPinyin("天行");
            Assert.AreEqual("TianXing", b);

            var py = WordsHelper.GetPinyin("快乐，乐清");
            Assert.AreEqual("KuaiLe，YueQing", py);

            py = WordsHelper.GetPinyin("快乐清理");
            Assert.AreEqual("KuaiLeQingLi", py);


            py = WordsHelper.GetPinyin("我爱中国");
            Assert.AreEqual("WoAiZhongGuo", py);

            py = WordsHelper.GetPinyin("我爱中国", ",");
            Assert.AreEqual("Wo,Ai,Zhong,Guo", py);

            py = WordsHelper.GetPinyin("我爱中国", true);
            Assert.AreEqual("WǒÀiZhōngGuó", py);

            py = WordsHelper.GetFirstPinyin("我爱中国");
            Assert.AreEqual("WAZG", py);

            var pys = WordsHelper.GetAllPinyin('传');
            Assert.AreEqual("Chuan", pys[0]);
            Assert.AreEqual("Zhuan", pys[1]);

            py = WordsHelper.GetPinyinForName("单一一");
            Assert.AreEqual("ShanYiYi", py);

            py = WordsHelper.GetPinyinForName("单一一", ",");
            Assert.AreEqual("Shan,Yi,Yi", py);

            py = WordsHelper.GetPinyinForName("单一一", true);
            Assert.AreEqual("ShànYīYī", py);

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
