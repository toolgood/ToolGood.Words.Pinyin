using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using ToolGood.Words.FirstPinyin.internals;

namespace ToolGood.Words.FirstPinyin
{
    public class WordsHelper
    {
        #region 拼音 操作
        /// <summary>
        /// 获取所有拼音,中文字符集为[0x3400,0x9FD5]，注：偏僻汉字很多未验证
        /// </summary>
        /// <param name="c">原文本</param>
        /// <param name="tone">是否带声调</param>
        /// <returns></returns>
        public static List<string> GetAllFirstPinyin(char c)
        {
            return PinyinDict.GetAllPinyin(c);
        }

        /// <summary>
        /// 获取首字母，中文字符集为[0x3400,0x9FD5]，注：偏僻汉字很多未验证
        /// </summary>
        /// <param name="text">原文本</param>
        /// <returns></returns>
        public static string GetFirstPinyin(string text)
        {
            return string.Join("", PinyinDict.GetPinyinList(text));
        }
        /// <summary>
        /// 获取拼音首字母,支持多音,中文字符集为[0x3400,0x9FD5]，注：偏僻汉字很多未验证
        /// </summary>
        /// <param name="text">原文本</param>
        /// <param name="splitSpan">分隔符</param>
        /// <returns></returns>
        public static string GetFirstPinyin(string text, string splitSpan )
        {
            return string.Join(splitSpan, PinyinDict.GetPinyinList(text ));
        }

        /// <summary>
        /// 获取拼音首字母,支持多音,中文字符集为[0x3400,0x9FD5]，注：偏僻汉字很多未验证
        /// </summary>
        /// <param name="text">原文本</param>
        /// <returns></returns>
        public static string[] GetFirstPinyinList(string text)
        {
            return PinyinDict.GetPinyinList(text);
        }

        /// <summary>
        /// 获取姓名首字母拼音,中文字符集为[0x3400,0x9FD5]，注：偏僻汉字很多未验证
        /// </summary>
        /// <param name="name">姓名</param>
        /// <returns></returns>
        public static string GetFirstPinyinForName(string name)
        {
            return string.Join("", PinyinDict.GetPinyinForName(name));
        }

        /// <summary>
        /// 获取姓名首字母拼音,中文字符集为[0x3400,0x9FD5]，注：偏僻汉字很多未验证
        /// </summary>
        /// <param name="name">姓名</param>
        /// <param name="splitSpan">分隔符</param>
        /// <returns></returns>
        public static string GetFirstPinyinForName(string name, string splitSpan)
        {
            return string.Join(splitSpan, PinyinDict.GetPinyinForName(name));
        }


        #endregion

        #region 判断输入是否为中文
        /// <summary>
        /// 判断输入是否为中文  ,中文字符集为[0x4E00,0x9FA5][0x3400,0x4db5]
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static bool HasChinese(string content)
        {
            if (Regex.IsMatch(content, @"[\u3400-\u4db5\u4e00-\u9fd5]")) {
                return true;
            } else {
                return false;
            }
        }
        /// <summary>
        /// 判断输入是否全为中文,中文字符集为[0x4E00,0x9FA5][0x3400,0x4db5]
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static bool IsAllChinese(string content)
        {
            if (Regex.IsMatch(content, @"^[\u3400-\u4db5\u4e00-\u9fd5]*$")) {
                return true;
            } else {
                return false;
            }
        }
        /// <summary>
        /// 判断含有英语
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static bool HasEnglish(string content)
        {
            if (Regex.IsMatch(content, @"[A-Za-z]")) {
                return true;
            } else {
                return false;
            }
        }
        /// <summary>
        /// 判断是否全部英语
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static bool IsAllEnglish(string content)
        {
            if (Regex.IsMatch(content, @"^[A-Za-z]*$")) {
                return true;
            } else {
                return false;
            }
        }

        #endregion

        #region 清理缓存 ClearCache
        /// <summary>
        /// 清理缓存
        /// </summary>
        public static void ClearCache()
        {
            PinyinDict.ClearCache();
        }

        #endregion



    }
}
