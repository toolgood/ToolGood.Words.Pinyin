using System;
using System.Collections.Generic;
using System.Linq;

namespace ToolGood.Words.FirstPinyin.internals
{
    public struct IntDictionary
    {
        private ushort[] _keys;
        private int[] _values;
        private int last;
        public IntDictionary(ushort[] keys, int[] values)
        {
            _keys = keys;
            _values = values;
            last = _keys.Length - 1;
        }

        public bool TryGetValue(ushort key, out int value)
        {
            if (last == -1) {
                value = 0;
                return false;
            }
            if (_keys[0] == key) {
                value = _values[0];
                return true;
            } else if (_keys[0] > key) {
                value = 0;
                return false;
            }
            if (_keys[last] == key) {
                value = _values[last];
                return true;
            } else if (_keys[last] < key) {
                value = 0;
                return false;
            }

            var left = 0;
            var right = last;
            while (left + 1 < right) {
                int mid = (left + right) / 2;
                int d = _keys[mid] - key;

                if (d == 0) {
                    value = _values[mid];
                    return true;
                } else if (d > 0) {
                    right = mid;
                } else {
                    left = mid;
                }
            }
            value = 0;
            return false;
        }



    }

}
