
using System;

namespace Barcode.Converters
{
    internal class BaseTypeConverter : ITypeConverter
    {
        public virtual int GetLength(Type type)
        {
            return -1;
        }

        public virtual bool CanConvert(Type type)
        {
            return false;
        }

        public virtual byte[] ConvertFrom(object value)
        {
            if (value == null)
                throw new ArgumentNullException();
            return (byte[])null;
        }

        public virtual object ConvertTo(Type type, byte[] value, int startIndex, int length)
        {
            if (value == null || type == (Type)null)
                throw new ArgumentNullException();
            if (startIndex > value.Length - 1)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (startIndex + length > value.Length)
                throw new ArgumentOutOfRangeException(nameof(length));
            return (object)null;
        }
    }
}