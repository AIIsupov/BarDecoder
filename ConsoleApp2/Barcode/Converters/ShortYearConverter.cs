
using System;

namespace Barcode.Converters
{
    internal class ShortYearConverter : BaseTypeConverter
    {
        public override int GetLength(Type type)
        {
            return 1;
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
            if (dateTime.Year < 1900 || dateTime.Year > 2155)
                throw new ArgumentException("Значение года даты должно быть больше 1900 и меньше 2155");
            return new byte[1] { (byte)(dateTime.Year - 1900) };
        }

        public override object ConvertTo(Type type, byte[] value, int startIndex, int length)
        {
            base.ConvertTo(type, value, startIndex, length);
            if (type != typeof(DateTime))
                throw new ArgumentException(string.Format("Невозможно выполнить преобразование в тип: {0}", (object)type.Name), nameof(value));
            if (length != 1)
                throw new ArgumentException("Длина преобразуемого значения должна равняться 1", nameof(length));
            return (object)new DateTime((int)value[startIndex] + 1900, 1, 1);
        }
    }
}