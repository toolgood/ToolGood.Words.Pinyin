using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ToolGood.Words.Pinyin.internals
{
    class WordsSearchEx
    {
        protected ushort[] _dict;
        protected int[] _first;
        protected ushort[] _min;
        protected ushort[] _max;

        protected IntDictionary[] _nextIndex;
        protected int[] _end;
        protected int[] _resultIndex;
        protected byte[] _keywordLength;


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
            //_keywordLength = ByteArrToshortArr(bs);


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
            List<ushort> max = new List<ushort>();
            List<ushort> min = new List<ushort>();

            for (int i = 0; i < dictLength; i++) {
                length = br.ReadInt32();
                bs = br.ReadBytes(length);
                var keys = ByteArrToUshortArr(bs);

                length = br.ReadInt32();
                bs = br.ReadBytes(length);
                var values = ByteArrToIntArr(bs);

                var dict = new Dictionary<ushort, int>();
                for (int j = 0; j < keys.Length; j++) {
                    dict[keys[j]] = values[j];
                }
                IntDictionary dictionary = new IntDictionary();
                dictionary.SetDictionary(dict);
                _nextIndex[i] = dictionary;
                if (length == 0) {
                    max.Add(0);
                    min.Add(ushort.MaxValue);
                } else {
                    max.Add(keys.Last());
                    min.Add(keys[0]);
                }
            }
            _max = max.ToArray();
            _min = min.ToArray();
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
                if (p == 0 || t < _min[p] || t > _max[p] || _nextIndex[p].TryGetValue(t, out next) == false) {
                    next = _first[t];
                }
                if (next != 0) {
                    for (int j = _end[next]; j < _end[next + 1]; j++) {
                        var index = _resultIndex[j];
                        var l = _keywordLength[index];
                        var r = new WordsSearchResult(i + 1 - l, i, index, l);
                        result.Add(r);
                    }
                }
                p = next;
            }
            return result;
        }

    }
}
