
using System;
using System.Collections;
using System.Collections.Generic;

namespace Barcode.Converters
{
    internal class OMS6EncodingStringConverter : BaseTypeConverter
    {
        private List<string> _charsRawTable = new List<string>()
    {
      " .-‘0123456789АБ",
      "ВГДЕЁЖЗИЙКЛМНОПР",
      "СТУФХЦЧШЩЬЪЫЭЮЯ*",
      "***************|"
    };
        private Dictionary<char, byte> _encodingChars = new Dictionary<char, byte>();
        private Dictionary<byte, char> _encodingBytes = new Dictionary<byte, char>();
        public const char RESERVED = '*';

        internal OMS6EncodingStringConverter()
        {
            for (int index1 = 0; index1 < this._charsRawTable.Count; ++index1)
            {
                char[] charArray = this._charsRawTable[index1].ToCharArray();
                for (int index2 = 0; index2 < 16; ++index2)
                {
                    if (charArray[index2] != '*')
                    {
                        this._encodingChars[charArray[index2]] = (byte)(index1 << 4 | index2);
                        this._encodingBytes[(byte)(index1 << 4 | index2)] = charArray[index2];
                    }
                }
            }
        }

        public override bool CanConvert(Type type)
        {
            return type == typeof(string);
        }

        public override byte[] ConvertFrom(object value)
        {
            base.ConvertFrom(value);
            if (value.GetType() != typeof(string))
                throw new ArgumentException(string.Format("Невозможно выполнить преобразование типа: {0}", (object)value.GetType().Name), nameof(value));
            string upper = (value as string).ToUpper();
            BitArray bitArray = new BitArray(upper.Length * 6);
            int index1 = 0;
            int index2 = 0;
            for (; index1 < upper.Length; ++index1)
            {
                char key = upper[index1];
                if (!this._encodingChars.ContainsKey(key))
                    key = ' ';
                byte encodingChar = this._encodingChars[key];
                bitArray.Set(index2, ((int)encodingChar & 1) == 1);
                int num1;
                bitArray.Set(num1 = index2 + 1, ((int)encodingChar & 2) == 2);
                int num2;
                bitArray.Set(num2 = num1 + 1, ((int)encodingChar & 4) == 4);
                int num3;
                bitArray.Set(num3 = num2 + 1, ((int)encodingChar & 8) == 8);
                int num4;
                bitArray.Set(num4 = num3 + 1, ((int)encodingChar & 16) == 16);
                int num5;
                bitArray.Set(num5 = num4 + 1, ((int)encodingChar & 32) == 32);
                index2 = num5 + 1;
            }
            byte[] numArray = new byte[upper.Length * 6 / 8 + (upper.Length * 6 % 8 == 0 ? 0 : 1)];
            bitArray.CopyTo((Array)numArray, 0);
            return numArray;
        }

        public override object ConvertTo(Type type, byte[] value, int startIndex, int length)
        {
            if (type != typeof(string))
                throw new ArgumentException(string.Format("Невозможно выполнить преобразование в тип: {0}", (object)type.Name), nameof(value));
            byte[] bytes = new byte[length];
            Array.Copy((Array)value, startIndex, (Array)bytes, 0, length);
            BitArray bitArray = new BitArray(bytes);
            char[] chArray = new char[length * 8 / 6];
            int index1 = 0;
            int index2 = 0;
            for (; index1 < chArray.Length; ++index1)
            {
                int num1;
                int num2;
                int num3;
                int num4;
                int num5;
                byte index3 = (byte)((uint)(byte)((uint)(byte)((uint)(byte)((uint)(byte)((uint)(byte)(0U | (uint)this.ToByte(bitArray.Get(index2))) | (uint)(byte)((uint)this.ToByte(bitArray.Get(num1 = index2 + 1)) << 1)) | (uint)(byte)((uint)this.ToByte(bitArray.Get(num2 = num1 + 1)) << 2)) | (uint)(byte)((uint)this.ToByte(bitArray.Get(num3 = num2 + 1)) << 3)) | (uint)(byte)((uint)this.ToByte(bitArray.Get(num4 = num3 + 1)) << 4)) | (uint)(byte)((uint)this.ToByte(bitArray.Get(num5 = num4 + 1)) << 5));
                index2 = num5 + 1;
                chArray[index1] = this._encodingBytes[index3];
            }
            return (object)new string(chArray, 0, chArray.Length);
        }

        private byte ToByte(bool value)
        {
            return value ? (byte)1 : (byte)0;
        }
    }
}
