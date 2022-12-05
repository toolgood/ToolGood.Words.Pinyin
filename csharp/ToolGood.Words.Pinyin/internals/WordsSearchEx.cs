using System;
using System.Collections.Generic;
using System.IO;

namespace ToolGood.Words.Pinyin.internals
{
    class WordsSearchEx : IDisposable
    {
        private ushort[] _dict;
        private int[] _first;

        private IntDictionary[] _nextIndex;
        private int[] _end;
        private int[] _resultIndex;
        private byte[] _keywordLength;


        #region 加载文件

        /// <summary>
        /// 加载Stream
        /// </summary>
        /// <param name="stream"></param>
        public void Load(Stream stream)
        {
            BinaryReader br = new BinaryReader(stream);
            Load(br);
            br.Close();
        }

        protected internal virtual void Load(BinaryReader br)
        {
            var length = br.ReadInt32();
            _keywordLength = br.ReadBytes(length);

            length = br.ReadInt32();
            var bs = br.ReadBytes(length);
            _dict = ByteArrToUshortArr(bs);

            length = br.ReadInt32();
            bs = br.ReadBytes(length);
            _first = ByteArrToIntArr(bs);

            length = br.ReadInt32();
            bs = br.ReadBytes(length);
            _end = ByteArrToIntArr(bs);

            length = br.ReadInt32();
            bs = br.ReadBytes(length);
            _resultIndex = ByteArrToIntArr(bs);

            var dictLength = br.ReadInt32();
            _nextIndex = new IntDictionary[dictLength];

            for (int i = 0; i < dictLength; i++) {
                length = br.ReadInt32();
                bs = br.ReadBytes(length);
                var keys = ByteArrToUshortArr(bs);

                length = br.ReadInt32();
                bs = br.ReadBytes(length);
                var values = ByteArrToIntArr(bs);

                IntDictionary dictionary = new IntDictionary(keys, values);
                _nextIndex[i] = dictionary;
            }
        }

        protected Int32[] ByteArrToIntArr(byte[] btArr)
        {
            Int32 intSize = (int)Math.Ceiling(btArr.Length / (double)sizeof(Int32));
            Int32[] intArr = new Int32[intSize];
            Buffer.BlockCopy(btArr, 0, intArr, 0, btArr.Length);
            return intArr;
        }
        protected ushort[] ByteArrToUshortArr(byte[] btArr)
        {
            Int32 intSize = (int)Math.Ceiling(btArr.Length / (double)sizeof(ushort));
            ushort[] intArr = new ushort[intSize];
            Buffer.BlockCopy(btArr, 0, intArr, 0, btArr.Length);
            return intArr;
        }

        #endregion



        /// <summary>
        /// 在文本中查找所有的关键字
        /// </summary>
        /// <param name="text">文本</param>
        /// <returns></returns>
        public List<WordsSearchResult> FindAll(string text)
        {
            List<WordsSearchResult> result = new List<WordsSearchResult>();
            var p = 0;

            for (int i = 0; i < text.Length; i++) {
                var t = _dict[text[i]];
                if (t == 0) {
                    p = 0;
                    continue;
                }
                int next;
                if (p == 0 || _nextIndex[p].TryGetValue(t, out next) == false) {
                    next = _first[t];
                }
                if (next != 0) {
                    var start = _end[next];
                    if (start < _end[next + 1]) { //只取第一个
                        var index = _resultIndex[start];
                        var l = _keywordLength[index];
                        var r = new WordsSearchResult(i + 1 - l, i, index, l);
                        result.Add(r);
                    }
                }
                p = next;
            }
            return result;
        }

        public void Dispose()
        {
            _dict = null;
            _first = null;
            _nextIndex = null;
            _end = null;
            _resultIndex = null;
            _keywordLength = null;
            GC.SuppressFinalize(this);

        }
    }
}
