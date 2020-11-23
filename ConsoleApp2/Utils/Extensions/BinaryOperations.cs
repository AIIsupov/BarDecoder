// Decompiled with JetBrains decompiler
// Type: Pc.Shared.Utils.Extensions.BinaryOperations
// Assembly: Pc.Shared.Utils, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9fd711222f1f1cb8
// MVID: AA64AFD9-733B-46FA-9DE7-0367BA6C437D
// Assembly location: C:\Users\WILD\Desktop\сканер\.NET Framework 4.0\Pc.Shared.Utils.dll

using System;
using System.Reflection;
using System.Text;

namespace Utils.Extensions
{
  public static class BinaryOperations
  {
    public static byte[] Xor(this byte[] instance, byte[] array2)
    {
      if (instance == null)
        throw new ArgumentNullException(nameof (instance));
      if (instance == null)
        throw new ArgumentNullException(nameof (array2));
      byte[] range = instance.GetRange<byte>(0, instance.Length);
      if (instance.Length != array2.Length)
        throw new ArgumentOutOfRangeException();
      for (int index = 0; index < range.Length; ++index)
        range[index] ^= array2[index];
      return range;
    }

    public static byte SetBitState(this byte number, int offset, bool IsSet)
    {
      if (offset < 0 || offset > 31)
        throw new ArgumentException("Error offset. Should be in range 0-31", nameof (offset));
      return (byte) ((long) number).SetBitState(offset, IsSet);
    }

    public static short SetBitState(this short number, int offset, bool IsSet)
    {
      if (offset < 0 || offset > 31)
        throw new ArgumentException("Error offset. Should be in range 0-31", nameof (offset));
      return (short) ((long) number).SetBitState(offset, IsSet);
    }

    public static int SetBitState(this int number, int offset, bool IsSet)
    {
      if (offset < 0 || offset > 31)
        throw new ArgumentException("Error offset. Should be in range 0-31", nameof (offset));
      return (int) ((long) number).SetBitState(offset, IsSet);
    }

    public static uint SetBitState(this uint number, int offset, bool IsSet)
    {
      if (offset < 0 || offset > 31)
        throw new ArgumentException("Error offset. Should be in range 0-31", nameof (offset));
      return (uint) ((long) number).SetBitState(offset, IsSet);
    }

    public static long SetBitState(this long number, int offset, bool IsSet)
    {
      if (offset < 0 || offset > 63)
        throw new ArgumentException("Error offset. Should be in range 0-63", nameof (offset));
      long num = (long) (1 << offset);
      return IsSet ? number | num : (number &= num ^ 2305843009213693951L);
    }

    public static byte SetBitRange(this byte number, int from, int to, int NewValue)
    {
      return (byte) ((long) number).SetBitRange(from, to, (long) NewValue);
    }

    public static short SetBitRange(this short number, int from, int to, int NewValue)
    {
      return (short) ((long) number).SetBitRange(from, to, (long) NewValue);
    }

    public static int SetBitRange(this int number, int from, int to, int NewValue)
    {
      return (int) ((long) number).SetBitRange(from, to, (long) NewValue);
    }

    public static uint SetBitRange(this uint number, int from, int to, int NewValue)
    {
      return (uint) ((long) number).SetBitRange(from, to, (long) NewValue);
    }

    public static long SetBitRange(this long number, int from, int to, long NewValue)
    {
      long num1 = 0;
      for (int index = from >= to ? from : to; index > (from >= to ? to - 1 : from - 1); --index)
        num1 = num1 << 1 | 1L;
      NewValue = from >= to ? (NewValue & num1) << to : (NewValue & num1) << from;
      long num2 = 0;
      for (int index = from >= to ? from : to; index > (from >= to ? to - 1 : from - 1); --index)
        num2 |= 1L << index;
      return number & (num2 ^ 9007199254740991L) | NewValue;
    }

    public static ulong SetBitRange(this ulong number, int from, int to, ulong NewValue)
    {
      ulong num1 = 0;
      for (int index = from >= to ? from : to; index > (from >= to ? to - 1 : from - 1); --index)
        num1 = num1 << 1 | 1UL;
      NewValue = from >= to ? (ulong) (((long) NewValue & (long) num1) << to) : (ulong) (((long) NewValue & (long) num1) << from);
      ulong num2 = 0;
      for (int index = from >= to ? from : to; index > (from >= to ? to - 1 : from - 1); --index)
        num2 |= (ulong) (1L << index);
      return number & (num2 ^ 9007199254740991UL) | NewValue;
    }

    public static int GetBitLength(this ushort val)
    {
      int num = 0;
      for (int index = 15; index >= 0; --index)
      {
        if (((int) val >> index & 1) > 0)
        {
          num = index;
          break;
        }
      }
      return num > 0 ? num + (8 - num % 8) : 8;
    }

    public static int GetBitLength(this short val)
    {
      int num = 0;
      for (int index = 15; index >= 0; --index)
      {
        if (((int) val >> index & 1) > 0)
        {
          num = index;
          break;
        }
      }
      return num > 0 ? num + (8 - num % 8) : 8;
    }

    public static int GetBitLength(this int val)
    {
      int num = 0;
      for (int index = 31; index >= 0; --index)
      {
        if ((val >> index & 1) > 0)
        {
          num = index;
          break;
        }
      }
      return num > 0 ? num + (8 - num % 8) : 8;
    }

    public static int GetBitLength(this long val)
    {
      int num = 0;
      for (int index = 63; index >= 0; --index)
      {
        if ((val >> index & 1L) > 0L)
        {
          num = index;
          break;
        }
      }
      return num > 0 ? num + (8 - num % 8) : 8;
    }

    public static int GetBitLength(this ulong val)
    {
      int num = 0;
      for (int index = 63; index >= 0; --index)
      {
        if ((val >> index & 1UL) > 0UL)
        {
          num = index;
          break;
        }
      }
      return num > 0 ? num + (8 - num % 8) : 8;
    }

    public static int GetBitLength(this uint val)
    {
      int num = 0;
      for (int index = 31; index >= 0; --index)
      {
        if ((val >> index & 1U) > 0U)
        {
          num = index;
          break;
        }
      }
      return num > 0 ? num + (8 - num % 8) : 8;
    }

    public static bool GetBitState(this int number, int offset)
    {
      if (offset < 0 || offset > 31)
        throw new ArgumentException("Error offset. Should be in range 0-31", nameof (offset));
      if (offset > 0)
        number >>= offset;
      return (number & 1) == 1;
    }

    public static bool GetBitState(this uint number, int offset)
    {
      if (offset < 0 || offset > 31)
        throw new ArgumentException("Error offset. Should be in range 0-31", nameof (offset));
      if (offset > 0)
        number >>= offset;
      return ((int) number & 1) == 1;
    }

    public static bool GetBitState(this byte number, int offset)
    {
      if (offset < 0 || offset > 7)
        throw new ArgumentException("Error offset. Should be in range 0-7", nameof (offset));
      if (offset > 0)
        number >>= offset;
      return ((int) number & 1) == 1;
    }

    public static bool GetBitState(this short number, int offset)
    {
      if (offset < 0 || offset > 15)
        throw new ArgumentException("Error offset. Should be in range 0-15", nameof (offset));
      if (offset > 0)
        number >>= offset;
      return ((int) number & 1) == 1;
    }

    public static bool GetBitState(this ushort number, int offset)
    {
      if (offset < 0 || offset > 15)
        throw new ArgumentException("Error offset. Should be in range 0-15", nameof (offset));
      if (offset > 0)
        number >>= offset;
      return ((int) number & 1) == 1;
    }

    public static bool GetBitState(this long number, int offset)
    {
      if (offset < 0 || offset > 63)
        throw new ArgumentException("Error offset. Should be in range 0-63", nameof (offset));
      if (offset > 0)
        number >>= offset;
      return (number & 1L) == 1L;
    }

    public static bool GetBitState(this ulong number, int offset)
    {
      if (offset < 0 || offset > 63)
        throw new ArgumentException("Error offset. Should be in range 0-63", nameof (offset));
      if (offset > 0)
        number >>= offset;
      return ((long) number & 1L) == 1L;
    }

    public static int GetBitRange(this byte number, int from, int to)
    {
      return (int) ((long) number).GetBitRange(from, to);
    }

    public static int GetBitRange(this short number, int from, int to)
    {
      return (int) ((long) number).GetBitRange(from, to);
    }

    public static int GetBitRange(this int number, int from, int to)
    {
      return (int) ((long) number).GetBitRange(from, to);
    }

    public static long GetBitRange(this long number, int from, int to)
    {
      long num = 0;
      for (int index = from >= to ? from : to; index > (from >= to ? to - 1 : from - 1); --index)
        num |= 1L << index;
      return from >= to ? number >> to & num >> to : number >> from & num >> from;
    }

    public static ulong GetBitRange(this ulong number, int from, int to)
    {
      ulong num = 0;
      for (int index = from >= to ? from : to; index > (from >= to ? to - 1 : from - 1); --index)
        num |= (ulong) (1L << index);
      return from >= to ? number >> to & num >> to : number >> from & num >> from;
    }

    public static long FromArray(this long instance, byte[] data, int index, int length)
    {
      return BinaryOperations.FromArray<long>(data, index, length);
    }

    public static int FromArray(this int instance, byte[] data, int index, int length)
    {
      return BinaryOperations.FromArray<int>(data, index, length);
    }

    public static uint FromArray(this uint instance, byte[] data, int index, int length)
    {
      return BinaryOperations.FromArray<uint>(data, index, length);
    }

    public static ushort FromArray(this ushort instance, byte[] data, int index, int length)
    {
      return BinaryOperations.FromArray<ushort>(data, index, length);
    }

    public static short FromArray(this short instance, byte[] data, int index, int length)
    {
      return BinaryOperations.FromArray<short>(data, index, length);
    }

    public static T FromArray<T>(byte[] data, int index, int length)
    {
      if (typeof (T) != typeof (byte) && typeof (T) != typeof (short) && (typeof (T) != typeof (int) && typeof (T) != typeof (long)) && (typeof (T) != typeof (ushort) && typeof (T) != typeof (uint) && (typeof (T) != typeof (ulong) && typeof (T) != typeof (byte?))) && (typeof (T) != typeof (short?) && typeof (T) != typeof (int?) && (typeof (T) != typeof (long?) && typeof (T) != typeof (ushort?)) && typeof (T) != typeof (uint?)) && typeof (T) != typeof (ulong?))
        throw new ArgumentException(string.Format("Type '{0}' is not valid.", (object) typeof (T).ToString()));
      ulong number = 0;
      if (length > 8)
        throw new ArgumentException(string.Format("Length '{0}' is not valid.", (object) length));
      if (index + length > data.Length)
        throw new ArgumentException(string.Format("Offset+Length '{0}' is out of array boundaries.", (object) (length + index)));
      int from = 0;
      for (int index1 = index + length - 1; index1 >= index; --index1)
      {
        number = number.SetBitRange(from, from + 8, (ulong) data[index1]);
        from += 8;
      }
      Type underlyingType = Nullable.GetUnderlyingType(typeof (T));
      if (underlyingType != (Type) null)
      {
        ConstructorInfo constructor = typeof (T).GetConstructor(new Type[1]
        {
          underlyingType
        });
        if (constructor != (ConstructorInfo) null)
          return (T) constructor.Invoke(new object[1]
          {
            Convert.ChangeType((object) number, underlyingType)
          });
      }
      return (T) Convert.ChangeType((object) number, typeof (T));
    }

    public static object FromArray(this PropertyInfo info, byte[] data, int index, int length)
    {
      if (info.PropertyType != typeof (byte) && info.PropertyType != typeof (short) && (info.PropertyType != typeof (int) && info.PropertyType != typeof (long)) && (info.PropertyType != typeof (ushort) && info.PropertyType != typeof (uint) && (info.PropertyType != typeof (ulong) && info.PropertyType != typeof (byte?))) && (info.PropertyType != typeof (short?) && info.PropertyType != typeof (int?) && (info.PropertyType != typeof (long?) && info.PropertyType != typeof (ushort?)) && info.PropertyType != typeof (uint?)) && info.PropertyType != typeof (ulong?))
        throw new ArgumentException(string.Format("Type '{0}' is not valid.", (object) info.PropertyType.ToString()));
      ulong number = 0;
      if (length > 8)
        throw new ArgumentException(string.Format("Length '{0}' is not valid.", (object) length));
      if (index + length > data.Length)
        throw new ArgumentException(string.Format("Offset+Length '{0}' is out of array boundaries.", (object) (length + index)));
      int from = 0;
      for (int index1 = index + length - 1; index1 >= index; --index1)
      {
        number = number.SetBitRange(from, from + 8, (ulong) data[index1]);
        from += 8;
      }
      Type underlyingType = Nullable.GetUnderlyingType(info.PropertyType);
      if (underlyingType != (Type) null)
      {
        ConstructorInfo constructor = info.PropertyType.GetConstructor(new Type[1]
        {
          underlyingType
        });
        if (constructor != (ConstructorInfo) null)
          return constructor.Invoke(new object[1]
          {
            Convert.ChangeType((object) number, underlyingType)
          });
      }
      return Convert.ChangeType((object) number, info.PropertyType);
    }

    public static string ToString(this byte[] instance)
    {
      return instance.ToString(Encoding.Default);
    }

    public static string ToString(this byte[] instance, Encoding encoding)
    {
      return encoding.GetString(instance);
    }

    public static string ToString(this byte[] instance, int codepage)
    {
      return Encoding.GetEncoding(codepage).GetString(instance);
    }

    public static string ToHexString(this byte[] Data, int offset = 0, int len = -1)
    {
      if (Data == null)
        return "";
      if (len == -1)
        len = Data.Length;
      if (len > Data.GetLength(0))
        len = Data.GetLength(0);
      return BitConverter.ToString(Data, offset, len).Replace("-", "");
    }
  }
}
