using System;

namespace Barcode.Converters
{
    internal class NumberConverter : BaseTypeConverter
    {
        public override int GetLength(Type type)
        {
            if (type == (Type)null)
                throw new ArgumentNullException();
            if (type == typeof(byte))
                return 1;
            if (type == typeof(short) || type == typeof(ushort))
                return 2;
            if (type == typeof(int) || type == typeof(uint))
                return 4;
            if (type == typeof(ulong) || type == typeof(long))
                return 8;
            throw new ArgumentException("Тип не поддерживается");
        }

        public override bool CanConvert(Type type)
        {
            return typeof(byte) == type || typeof(short) == type || (typeof(int) == type || typeof(ushort) == type) || (typeof(uint) == type || typeof(long) == type) || typeof(ulong) == type;
        }

        public override byte[] ConvertFrom(object value)
        {
            if (value == null)
                throw new ArgumentNullException();
            if (value.GetType() == typeof(byte))
                return new byte[1]
                {
          (byte) ((uint) (byte) value & (uint) byte.MaxValue)
                };
            if (value.GetType() == typeof(short))
            {
                short num = (short)value;
                return new byte[2]
                {
          (byte) (((int) num & 65280) >> 8),
          (byte) ((uint) num & (uint) byte.MaxValue)
                };
            }
            if (value.GetType() == typeof(ushort))
            {
                ushort num = (ushort)value;
                return new byte[2]
                {
          (byte) (((int) num & 65280) >> 8),
          (byte) ((uint) num & (uint) byte.MaxValue)
                };
            }
            if (value.GetType() == typeof(int))
            {
                int num = (int)value;
                return new byte[4]
                {
          (byte) (((long) num & 4278190080L) >> 24),
          (byte) ((num & 16711680) >> 16),
          (byte) ((num & 65280) >> 8),
          (byte) (num & (int) byte.MaxValue)
                };
            }
            if (value.GetType() == typeof(uint))
            {
                uint num = (uint)value;
                return new byte[4]
                {
          (byte) ((num & 4278190080U) >> 24),
          (byte) ((num & 16711680U) >> 16),
          (byte) ((num & 65280U) >> 8),
          (byte) (num & (uint) byte.MaxValue)
                };
            }
            if (value.GetType() == typeof(ulong))
            {
                ulong num = (ulong)value;
                return new byte[8]
                {
          (byte) ((num & 18374686479671623680UL) >> 56),
          (byte) ((num & 71776119061217280UL) >> 48),
          (byte) ((num & 280375465082880UL) >> 40),
          (byte) ((num & 1095216660480UL) >> 32),
          (byte) ((num & 4278190080UL) >> 24),
          (byte) ((num & 16711680UL) >> 16),
          (byte) ((num & 65280UL) >> 8),
          (byte) (num & (ulong) byte.MaxValue)
                };
            }
            if (!(value.GetType() == typeof(long)))
                throw new ArgumentException(string.Format("Невозможно выполнить преобразование типа: {0}", (object)value.GetType().Name), nameof(value));
            long num1 = (long)value;
            return new byte[8]
            {
        (byte) ((ulong) (num1 & -72057594037927936L) >> 56),
        (byte) ((num1 & 71776119061217280L) >> 48),
        (byte) ((num1 & 280375465082880L) >> 40),
        (byte) ((num1 & 1095216660480L) >> 32),
        (byte) ((num1 & 4278190080L) >> 24),
        (byte) ((num1 & 16711680L) >> 16),
        (byte) ((num1 & 65280L) >> 8),
        (byte) ((ulong) num1 & (ulong) byte.MaxValue)
            };
        }

        public override object ConvertTo(Type type, byte[] value, int startIndex, int length)
        {
            base.ConvertTo(type, value, startIndex, length);
            if (type == typeof(byte))
                return (object)value[startIndex];
            if (type == typeof(short))
                return (object)(short)((int)value[startIndex] << 8 | (int)value[startIndex + 1]);
            if (type == typeof(ushort))
                return (object)(ushort)((uint)value[startIndex] << 8 | (uint)value[startIndex + 1]);
            if (type == typeof(int))
                return (object)((int)value[startIndex] << 24 | (int)value[startIndex + 1] << 16 | (int)value[startIndex + 2] << 8 | (int)value[startIndex + 3]);
            if (type == typeof(uint))
                return (object)(uint)((int)value[startIndex] << 24 | (int)value[startIndex + 1] << 16 | (int)value[startIndex + 2] << 8 | (int)value[startIndex + 3]);
            if (type == typeof(ulong))
                return (object)(ulong)((long)value[startIndex] << 56 | (long)value[startIndex + 1] << 48 | (long)value[startIndex + 2] << 40 | (long)value[startIndex + 3] << 32 | (long)value[startIndex + 4] << 24 | (long)value[startIndex + 5] << 16 | (long)value[startIndex + 6] << 8 | (long)value[startIndex + 7]);
            if (type == typeof(long))
                return (object)((long)value[startIndex] << 56 | (long)value[startIndex + 1] << 48 | (long)value[startIndex + 2] << 40 | (long)value[startIndex + 3] << 32 | (long)value[startIndex + 4] << 24 | (long)value[startIndex + 5] << 16 | (long)value[startIndex + 6] << 8 | (long)value[startIndex + 7]);
            throw new ArgumentException(string.Format("Невозможно выполнить преобразование в тип: {0}", (object)type.Name), nameof(value));
        }
    }
}
