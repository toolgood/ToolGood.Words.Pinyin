# ToolGood.Words.Pinyin


一款高性能拼音字母转化类，是`ToolGood.Words`拼音分支，追求快速加载速度。

`ToolGood.Words.Pinyin`支持范围 [0x3400,0x4DB5] [0x4E00,0x9FA5]


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
    WordsHelper.GetPinyinForName("单一一")//ShanYiYi
    WordsHelper.GetPinyinForName("单一一",",")//Shan,Yi,Yi
    WordsHelper.GetPinyinForName("单一一",true)//ShànYīYī
``` 
