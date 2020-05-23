# ToolGood.Words.Pinyin

一款高性能拼音字母转化类，是`ToolGood.Words`拼音分支，追求极致加载速度。

`ToolGood.Words.Pinyin`支持范围 [0x3400,0x4DB5] [0x4E00,0x9FA5]

`ToolGood.Words.FirstPinyin`支持范围 [0x3400,0x4DB5] [0x4E00,0x9FA5]

`ToolGood.Words.Pinyin`、`ToolGood.Words.FirstPinyin`两库内藏着8万组词组，精确度比同一类型拼音转化类库高。

## 为什么要分成2个项目

`ToolGood.Words.Pinyin`支持全拼和首字母，相比`ToolGood.Words`功能更小，第一次加载速度在70ms左右，而`ToolGood.Words`第一次加载需要800ms左右。

`ToolGood.Words.FirstPinyin` 只支持首字母，功能更小，体积更小，第一次加载速度更快(50ms)，占用内存更小（28M）

### ToolGood.Words.Pinyin  拼音模块
``` csharp
    // 获取全拼
    WordsHelper.GetPinyin("我爱中国");//WoAiZhongGuo   
    WordsHelper.GetPinyin("我爱中国",",");//Wo,Ai,Zhong,Guo   
    WordsHelper.GetPinyin("我爱中国",true);//WǒÀiZhōngGuó

    // 获取首字母
    WordsHelper.GetFirstPinyin("我爱中国");//WAZG
    // 获取全部拼音
    WordsHelper.GetAllPinyin('传');//Chuan,Zhuan
    // 获取姓名
    WordsHelper.GetPinyinForName("单一一");//ShanYiYi
    WordsHelper.GetPinyinForName("单一一",",");//Shan,Yi,Yi
    WordsHelper.GetPinyinForName("单一一",true);//ShànYīYī
    // 清理拼音缓存
    WordsHelper.ClearCache();
``` 

### ToolGood.Words.Pinyin 拼音匹配
`PinyinMatch`：方法有`SetKeywords`、`SetIndexs`、`Find`、`FindIndex`。

`PinyinMatch<T>`：方法有`SetKeywordsFunc`、`SetPinyinFunc`、`SetPinyinSplitChar`、`Find`。
``` csharp
    string s = "北京|天津|河北|辽宁|吉林|黑龙江|山东|江苏|上海|浙江|安徽|福建|江西|广东|广西|海南|河南|湖南|湖北|山西|内蒙古|宁夏|青海|陕西|甘肃|新疆|四川|贵州|云南|重庆|西藏|香港|澳门|台湾";

    PinyinMatch match = new PinyinMatch();
    match.SetKeywords(s.Split('|').ToList());

    var all = match.Find("BJ");
    Assert.AreEqual("北京", all[0]);
    Assert.AreEqual(1, all.Count);

    all = match.Find("北J");
    Assert.AreEqual("北京", all[0]);
    Assert.AreEqual(1, all.Count);

    all = match.Find("北Ji");
    Assert.AreEqual("北京", all[0]);
    Assert.AreEqual(1, all.Count);

    all = match.Find("S");
    Assert.AreEqual("山东", all[0]);
    Assert.AreEqual("江苏", all[1]);

    var all2 = match.FindIndex("BJ");
    Assert.AreEqual(0, all2[0]);
    Assert.AreEqual(1, all.Count);
``` 

### ToolGood.Words.FirstPinyin 拼音
``` csharp
    WordsHelper.GetFirstPinyin("我爱中国");//WAZG
    WordsHelper.GetFirstPinyin("我爱中国", ",");//W,A,Z,G

    WordsHelper.GetAllFirstPinyin('传');// "C" "Z"

    WordsHelper.GetFirstPinyinForName("单一一");//SYY
    WordsHelper.GetFirstPinyinForName("单一一", ",");//S,Y,Y

    // 清理拼音缓存
    WordsHelper.ClearCache();
``` 


## 捐赠

如果这个类库有帮助到您，请 Star 这个仓库。

你也可以选择使用支付宝或微信给我捐赠：

![Alipay, WeChat](https://toolgood.github.io/image/toolgood.png)

