using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
#if NETSTANDARD2_1
using ZIPStream = System.IO.Compression.BrotliStream;
#else
using ZIPStream = System.IO.Compression.GZipStream;
#endif

namespace ToolGood.Words.FirstPinyin.internals
{
    public static class PinyinDict
    {
        private static Dictionary<string, byte[]> _pyName;
        private static string[] _pyShow;
        private static ushort[] _pyIndex;
        private static byte[] _pyData;
        private static int[] _wordPyIndex;
        private static byte[] _wordPy;
        private static WordsSearchEx _search;

        public static string[] PyShow {
            get {
                InitPy();
                return _pyShow;
            }
        }


        public static string[] GetPinyinList(string text)
        {
            InitPy();

            List<string> list = new List<string>();
            for (int j = 0; j < text.Length; j++) { list.Add(null); }

            var pos = _search.FindAll(text);
            var pindex = -1;
            foreach (var p in pos) {
                if (p.Start > pindex) {
                    for (int j = 0; j < p.Length; j++) {
                        list[j + p.Start] = _pyShow[_wordPy[_wordPyIndex[p.Index] + j]];
                    }
                    pindex = p.End;
                }
            }
            var i = 0;
            while (i < text.Length) {
                if (list[i] == null) {
                    var c = text[i];
                    if (c >= 0x3400 && c <= 0x9fd5) {
                        var index = c - 0x3400;
                        var start = _pyIndex[index];
                        var end = _pyIndex[index + 1];
                        if (end > start) {
                            list[i] = _pyShow[_pyData[start]];
                        }
                    } else {
                        list[i] = text[i].ToString();
                    }
                }
                i++;
            }
            list.RemoveAll(q => q == null);
            return list.ToArray();
        }

 

        public static List<string> GetAllPinyin(char c)
        {
            InitPy();
            if (c >= 0x3400 && c <= 0x9fd5) {
                var index = c - 0x3400;
                List<string> list = new List<string>();
                var start = _pyIndex[index];
                var end = _pyIndex[index + 1];
                if (end > start) {
                    for (int i = start; i < end; i++) {
                        list.Add(_pyShow[_pyData[i]]);
                    }
                }
                return list.Distinct().ToList();
            }
            return new List<string>();
        }

        public static List<string> GetPinyinForName(string name)
        {
            InitPy();

            List<string> list = new List<string>();
            string xing;
            string ming;
            byte[] indexs;
            if (name.Length > 1) { // 检查复姓
                xing = name.Substring(0, 2);
                if (_pyName.TryGetValue(xing, out indexs)) {
                    foreach (var index in indexs) {
                        list.Add(_pyShow[index]);
                    }
                    if (name.Length > 2) {
                        ming = name.Substring(2);
                        list.AddRange(GetPinyinList(ming));
                    }
                    return list;
                }
            }
            xing = name.Substring(0, 1);
            if (_pyName.TryGetValue(xing, out indexs)) {
                foreach (var index in indexs) {
                    list.Add(_pyShow[index]);
                }
                if (name.Length > 1) {
                    ming = name.Substring(1);
                    list.AddRange(GetPinyinList(ming));
                }
                return list;
            }

            return GetPinyinList(name).ToList();
        }



        #region private
        private static object lockObj = new object();

        private static void InitPy()
        {
            if (_search == null) {
                lock (lockObj) {
                    if (_search == null) {
                        var ass = typeof(WordsHelper).Assembly;
#if NETSTANDARD2_1
                        Stream sm = ass.GetManifestResourceStream("ToolGood.Words.Pinyin.dict.Pinyin.dat.br");
#else
                        Stream sm = ass.GetManifestResourceStream("ToolGood.Words.Pinyin.dict.Pinyin.dat.z");
#endif
                        var sm12 = Decompress(sm);
                        BinaryReader reader = new BinaryReader(sm12);
                        _pyName = new Dictionary<string, byte[]>();
                        var length = reader.ReadInt32();
                        for (int i = 0; i < length; i++) {
                            var key = reader.ReadString();
                            var count = reader.ReadInt32();
                            var ubs = reader.ReadBytes(count);
                            _pyName.Add(key, ubs);
                        }
                        length = reader.ReadInt32();
                        _pyShow = new string[length];
                        for (int i = 0; i < length; i++) {
                            _pyShow[i] = reader.ReadString();
                        }

                        length = reader.ReadInt32();
                        var bs = reader.ReadBytes(length);
                        _pyIndex = ByteArrToUint16Arr(bs);


                        length = reader.ReadInt32();
                        bs = reader.ReadBytes(length);
                        _pyData = bs;// ByteArrToUint16Arr(bs);

                        length = reader.ReadInt32();
                        bs = reader.ReadBytes(length);
                        _wordPyIndex = ByteArrToIntArr(bs);

                        length = reader.ReadInt32();
                        bs = reader.ReadBytes(length);
                        _wordPy = bs;// ByteArrToUint16Arr(bs);

                        var search = new WordsSearchEx();
                        search.Load(reader);

                        reader.Close();
                        sm.Close();
                        _search = search;
                    }
                }
            }
        }

        private static Int32[] ByteArrToIntArr(byte[] btArr)
        {
            Int32 intSize = (int)Math.Ceiling(btArr.Length / (double)sizeof(Int32));
            Int32[] intArr = new Int32[intSize];
            Buffer.BlockCopy(btArr, 0, intArr, 0, btArr.Length);
            return intArr;
        }

        private static ushort[] ByteArrToUint16Arr(byte[] btArr)
        {
            Int32 intSize = (int)Math.Ceiling(btArr.Length / (double)sizeof(UInt16));
            ushort[] intArr = new ushort[intSize];
            Buffer.BlockCopy(btArr, 0, intArr, 0, btArr.Length);
            return intArr;
        }

        private static Stream Decompress(Stream stream)
        {
            MemoryStream resultStream = new MemoryStream();
            using (var zStream = new ZIPStream(stream, CompressionMode.Decompress)) {
                zStream.CopyTo(resultStream);
            }
            resultStream.Position = 0;
            return resultStream;
        }
        #endregion


        public static void ClearCache()
        {
            lock (lockObj) {
                if (_pyName != null) {
                    _pyName.Clear();
                    _pyName = null;
                }
                _pyShow = null;
                _pyIndex = null;
                _pyData = null;
                _wordPyIndex = null;
                _wordPy = null;
                if (_search != null) {
                    _search.Dispose();
                    _search = null;
                }
                GC.Collect();
            }
        }

    }
}
