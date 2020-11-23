using System;
using System.Collections.Generic;
using Utils.Collections;

namespace Barcode.Converters
{
    internal class OMS62EncodingStringConverter : BaseTypeConverter
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

        internal OMS62EncodingStringConverter()
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
            CBitArray cbitArray = new CBitArray();
            int index1 = 0;
            int index2 = 0;
            for (; index1 < upper.Length; ++index1)
            {
                char key = upper[index1];
                if (!this._encodingChars.ContainsKey(key))
                    key = ' ';
                byte encodingChar = this._encodingChars[key];
                cbitArray.Set(index2, ((int)encodingChar & 32) == 32);
                int num1;
                cbitArray.Set(num1 = index2 + 1, ((int)encodingChar & 16) == 16);
                int num2;
                cbitArray.Set(num2 = num1 + 1, ((int)encodingChar & 8) == 8);
                int num3;
                cbitArray.Set(num3 = num2 + 1, ((int)encodingChar & 4) == 4);
                int num4;
                cbitArray.Set(num4 = num3 + 1, ((int)encodingChar & 2) == 2);
                int num5;
                cbitArray.Set(num5 = num4 + 1, ((int)encodingChar & 1) == 1);
                index2 = num5 + 1;
            }
            return (byte[])cbitArray.ToArray();
        }

        public override object ConvertTo(Type type, byte[] value, int startIndex, int length)
        {
            if (type != typeof(string))
                throw new ArgumentException(string.Format("Невозможно выполнить преобразование в тип: {0}", (object)type.Name), nameof(value));
            byte[] numArray = new byte[length];
            Array.Copy((Array)value, startIndex, (Array)numArray, 0, length);
            CBitArray cbitArray = new CBitArray((CByte)numArray);
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
                byte index3 = (byte)((uint)(byte)((uint)(byte)((uint)(byte)((uint)(byte)((uint)(byte)(0U | (uint)(byte)((uint)this.ToByte(cbitArray.Get(index2)) << 5)) | (uint)(byte)((uint)this.ToByte(cbitArray.Get(num1 = index2 + 1)) << 4)) | (uint)(byte)((uint)this.ToByte(cbitArray.Get(num2 = num1 + 1)) << 3)) | (uint)(byte)((uint)this.ToByte(cbitArray.Get(num3 = num2 + 1)) << 2)) | (uint)(byte)((uint)this.ToByte(cbitArray.Get(num4 = num3 + 1)) << 1)) | (uint)this.ToByte(cbitArray.Get(num5 = num4 + 1)));
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
