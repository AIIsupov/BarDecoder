
using System;

namespace Barcode.Converters
{
    internal interface ITypeConverter
    {
        int GetLength(Type type);

        bool CanConvert(Type type);

        byte[] ConvertFrom(object value);

        object ConvertTo(Type type, byte[] value, int startIndex, int length);
    }
}