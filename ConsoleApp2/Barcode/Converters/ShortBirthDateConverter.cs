
using System;

namespace Barcode.Converters
{
    internal class ShortBirthDateConverter : BaseTypeConverter
    {
        public override int GetLength(Type type)
        {
            return 2;
        }

        public override bool CanConvert(Type type)
        {
            return typeof(DateTime) == type;
        }

        public override byte[] ConvertFrom(object value)
        {
            if (value == null)
                throw new ArgumentNullException();
            if (value.GetType() != typeof(DateTime))
                throw new ArgumentException(string.Format("Невозможно выполнить преобразование типа: {0}", (object)value.GetType().Name), nameof(value));
            DateTime dateTime = (DateTime)value;
            if (dateTime < new DateTime(1900, 1, 1) || dateTime > new DateTime(2079, 6, 6))
                throw new ArgumentException("Значение даты должно быть больше 01.01.1900 и меньше 06.06.2079");
            int days = (dateTime - new DateTime(1900, 1, 1)).Days;
            return new byte[2]
            {
        (byte) ((days & 65280) >> 8),
        (byte) (days & (int) byte.MaxValue)
            };
        }

        public override object ConvertTo(Type type, byte[] value, int startIndex, int length)
        {
            base.ConvertTo(type, value, startIndex, length);
            if (type != typeof(DateTime))
                throw new ArgumentException(string.Format("Невозможно выполнить преобразование в тип: {0}", (object)type.Name), nameof(value));
            if (length != 2)
                throw new ArgumentException("Длина преобразуемого значения должна равняться 2", nameof(length));
            return (object)new DateTime(1900, 1, 1).AddDays((double)((int)value[startIndex] << 8 | (int)value[startIndex + 1]));
        }
    }
}
