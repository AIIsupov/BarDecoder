
using System;

namespace Barcode.Converters
{
    internal class Int24Converter : BaseTypeConverter
    {
        public override int GetLength(Type type)
        {
            return 3;
        }

        public override bool CanConvert(Type type)
        {
            return typeof(byte) == type || typeof(short) == type || typeof(int) == type;
        }

        public override byte[] ConvertFrom(object value)
        {
            if (value == null)
                throw new ArgumentNullException();
            if (!(value.GetType() == typeof(int)))
                throw new ArgumentException(string.Format("Невозможно выполнить преобразование типа: {0}", (object)value.GetType().Name), nameof(value));
            int num = (int)value;
            return new byte[3]
            {
                (byte) ((num & 16711680) >> 16),
                (byte) ((num & 65280) >> 8),
                (byte) (num & (int) byte.MaxValue)
            };
        }

        public override object ConvertTo(Type type, byte[] value, int startIndex, int length)
        {
            base.ConvertTo(type, value, startIndex, length);
            if (type == typeof(int))
                return (object)((int)value[startIndex] << 16 | (int)value[startIndex + 1] << 8 | (int)value[startIndex + 2]);
            throw new ArgumentException(string.Format("Невозможно выполнить преобразование в тип: {0}", (object)type.Name), nameof(value));
        }
    }
}