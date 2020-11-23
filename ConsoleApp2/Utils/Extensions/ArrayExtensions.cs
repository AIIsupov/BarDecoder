
using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils.Extensions
{
  public static class ArrayExtensions
  {
    public static byte[] PadLeft(this byte[] instance, int length)
    {
      List<byte> byteList = new List<byte>();
      byteList.AddRange((IEnumerable<byte>) instance);
      if (instance.Length < length)
      {
        byteList.InsertRange(0, (IEnumerable<byte>) new byte[length - instance.Length]);
        instance = byteList.ToArray();
      }
      else
        instance = instance.GetRange<byte>(0, length);
      return instance;
    }

    public static byte[] PadRight(this byte[] instance, int length)
    {
      List<byte> byteList = new List<byte>();
      byteList.AddRange((IEnumerable<byte>) instance);
      if (instance.Length < length)
      {
        byteList.AddRange((IEnumerable<byte>) new byte[length - instance.Length]);
        instance = byteList.ToArray();
      }
      else
        instance = instance.GetRange<byte>(0, length);
      return instance;
    }

    public static byte[] ToArray(this object instance)
    {
      if (instance.GetType() != typeof (byte) && instance.GetType() != typeof (short) && (instance.GetType() != typeof (int) && instance.GetType() != typeof (long)) && (instance.GetType() != typeof (ushort) && instance.GetType() != typeof (uint) && (instance.GetType() != typeof (ulong) && instance.GetType() != typeof (byte[]))) && (instance.GetType() != typeof (byte?) && instance.GetType() != typeof (short?) && (instance.GetType() != typeof (int?) && instance.GetType() != typeof (long?)) && (instance.GetType() != typeof (ushort?) && instance.GetType() != typeof (uint?))) && instance.GetType() != typeof (ulong?))
        throw new ArgumentException(string.Format("Type '{0}' is not valid.", (object) instance.GetType().ToString()));
      if (instance.GetType() == typeof (byte) || instance.GetType() == typeof (byte?))
        return new byte[1]{ (byte) instance };
      int bitLength = Convert.ToUInt64(instance).GetBitLength();
      byte[] numArray = new byte[(bitLength + (bitLength > 8 ? (16 - bitLength % 16 > 0 ? bitLength % 16 : 16) : 0)) / 8];
      int from = 0;
      for (int index = numArray.Length - 1; index >= 0; --index)
      {
        numArray[index] = (byte) Convert.ToUInt64(instance).GetBitRange(from, from + 8);
        from += 8;
      }
      return numArray;
    }

    public static T[] GetRange<T>(this T[] instance, int offset, int length)
    {
      return ((IEnumerable<T>) instance).ToList<T>().GetRange(offset, length).ToArray();
    }

    public static T[] SetRange<T>(this T[] instance, int offset, T[] value)
    {
      for (int index = offset; index < instance.Length && index < offset + value.Length; ++index)
        instance[index] = value[index - offset];
      return instance;
    }

    public static T[] AddRange<T>(this T[] instance, T[] value)
    {
      List<T> list = ((IEnumerable<T>) instance).ToList<T>();
      list.AddRange((IEnumerable<T>) value);
      return list.ToArray();
    }

    public static void SetValueEx<T>(this T[] instance, int index, int length, T value)
    {
      for (int index1 = index; index1 < instance.Length && index1 < index + length; ++index1)
        instance[index1] = value;
    }
  }
}
