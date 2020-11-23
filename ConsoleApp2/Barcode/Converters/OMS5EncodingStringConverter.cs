
using System;
using System.Collections;

namespace Barcode.Converters
{
    internal class OMS5EncodingStringConverter : BaseTypeConverter
    {
        private Hashtable _encodingChars = new Hashtable()
    {
      {
        (object) ' ',
        (object) (byte) 0
      },
      {
        (object) '-',
        (object) (byte) 1
      },
      {
        (object) 'А',
        (object) (byte) 2
      },
      {
        (object) 'Б',
        (object) (byte) 3
      },
      {
        (object) 'В',
        (object) (byte) 4
      },
      {
        (object) 'Г',
        (object) (byte) 5
      },
      {
        (object) 'Д',
        (object) (byte) 6
      },
      {
        (object) 'Е',
        (object) (byte) 7
      },
      {
        (object) 'Ё',
        (object) (byte) 7
      },
      {
        (object) 'Ж',
        (object) (byte) 8
      },
      {
        (object) 'З',
        (object) (byte) 9
      },
      {
        (object) 'И',
        (object) (byte) 10
      },
      {
        (object) 'Й',
        (object) (byte) 10
      },
      {
        (object) 'К',
        (object) (byte) 11
      },
      {
        (object) 'Л',
        (object) (byte) 12
      },
      {
        (object) 'М',
        (object) (byte) 13
      },
      {
        (object) 'Н',
        (object) (byte) 14
      },
      {
        (object) 'О',
        (object) (byte) 15
      },
      {
        (object) 'П',
        (object) (byte) 16
      },
      {
        (object) 'Р',
        (object) (byte) 17
      },
      {
        (object) 'С',
        (object) (byte) 18
      },
      {
        (object) 'Т',
        (object) (byte) 19
      },
      {
        (object) 'У',
        (object) (byte) 20
      },
      {
        (object) 'Ф',
        (object) (byte) 21
      },
      {
        (object) 'Х',
        (object) (byte) 22
      },
      {
        (object) 'Ц',
        (object) (byte) 23
      },
      {
        (object) 'Ч',
        (object) (byte) 24
      },
      {
        (object) 'Ш',
        (object) (byte) 25
      },
      {
        (object) 'Щ',
        (object) (byte) 26
      },
      {
        (object) 'Ь',
        (object) (byte) 27
      },
      {
        (object) 'Ъ',
        (object) (byte) 27
      },
      {
        (object) 'Ы',
        (object) (byte) 28
      },
      {
        (object) 'Э',
        (object) (byte) 29
      },
      {
        (object) 'Ю',
        (object) (byte) 30
      },
      {
        (object) 'Я',
        (object) (byte) 31
      }
    };
        private Hashtable _encodingBytes = new Hashtable()
    {
      {
        (object) (byte) 0,
        (object) ' '
      },
      {
        (object) (byte) 1,
        (object) '-'
      },
      {
        (object) (byte) 2,
        (object) 'А'
      },
      {
        (object) (byte) 3,
        (object) 'Б'
      },
      {
        (object) (byte) 4,
        (object) 'В'
      },
      {
        (object) (byte) 5,
        (object) 'Г'
      },
      {
        (object) (byte) 6,
        (object) 'Д'
      },
      {
        (object) (byte) 7,
        (object) 'Е'
      },
      {
        (object) (byte) 8,
        (object) 'Ж'
      },
      {
        (object) (byte) 9,
        (object) 'З'
      },
      {
        (object) (byte) 10,
        (object) 'И'
      },
      {
        (object) (byte) 11,
        (object) 'К'
      },
      {
        (object) (byte) 12,
        (object) 'Л'
      },
      {
        (object) (byte) 13,
        (object) 'М'
      },
      {
        (object) (byte) 14,
        (object) 'Н'
      },
      {
        (object) (byte) 15,
        (object) 'О'
      },
      {
        (object) (byte) 16,
        (object) 'П'
      },
      {
        (object) (byte) 17,
        (object) 'Р'
      },
      {
        (object) (byte) 18,
        (object) 'С'
      },
      {
        (object) (byte) 19,
        (object) 'Т'
      },
      {
        (object) (byte) 20,
        (object) 'У'
      },
      {
        (object) (byte) 21,
        (object) 'Ф'
      },
      {
        (object) (byte) 22,
        (object) 'Х'
      },
      {
        (object) (byte) 23,
        (object) 'Ц'
      },
      {
        (object) (byte) 24,
        (object) 'Ч'
      },
      {
        (object) (byte) 25,
        (object) 'Ш'
      },
      {
        (object) (byte) 26,
        (object) 'Щ'
      },
      {
        (object) (byte) 27,
        (object) 'Ь'
      },
      {
        (object) (byte) 28,
        (object) 'Ы'
      },
      {
        (object) (byte) 29,
        (object) 'Э'
      },
      {
        (object) (byte) 30,
        (object) 'Ю'
      },
      {
        (object) (byte) 31,
        (object) 'Я'
      }
    };

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
            BitArray bitArray = new BitArray(upper.Length * 5);
            int index1 = 0;
            int index2 = 0;
            for (; index1 < upper.Length; ++index1)
            {
                char ch = upper[index1];
                if (!this._encodingChars.ContainsKey((object)ch))
                    ch = ' ';
                byte encodingChar = (byte)this._encodingChars[(object)ch];
                bitArray.Set(index2, ((int)encodingChar & 1) == 1);
                int num1;
                bitArray.Set(num1 = index2 + 1, ((int)encodingChar & 2) == 2);
                int num2;
                bitArray.Set(num2 = num1 + 1, ((int)encodingChar & 4) == 4);
                int num3;
                bitArray.Set(num3 = num2 + 1, ((int)encodingChar & 8) == 8);
                int num4;
                bitArray.Set(num4 = num3 + 1, ((int)encodingChar & 16) == 16);
                index2 = num4 + 1;
            }
            byte[] numArray = new byte[upper.Length * 5 / 8 + (upper.Length * 5 % 8 == 0 ? 0 : 1)];
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
            char[] chArray = new char[length * 8 / 5];
            int index1 = 0;
            int index2 = 0;
            for (; index1 < chArray.Length; ++index1)
            {
                int num1;
                int num2;
                int num3;
                int num4;
                byte num5 = (byte)((uint)(byte)((uint)(byte)((uint)(byte)((uint)(byte)(0U | (uint)this.ToByte(bitArray.Get(index2))) | (uint)(byte)((uint)this.ToByte(bitArray.Get(num1 = index2 + 1)) << 1)) | (uint)(byte)((uint)this.ToByte(bitArray.Get(num2 = num1 + 1)) << 2)) | (uint)(byte)((uint)this.ToByte(bitArray.Get(num3 = num2 + 1)) << 3)) | (uint)(byte)((uint)this.ToByte(bitArray.Get(num4 = num3 + 1)) << 4));
                index2 = num4 + 1;
                chArray[index1] = (char)this._encodingBytes[(object)num5];
            }
            return (object)new string(chArray, 0, chArray.Length);
        }

        private byte ToByte(bool value)
        {
            return value ? (byte)1 : (byte)0;
        }
    }
}
