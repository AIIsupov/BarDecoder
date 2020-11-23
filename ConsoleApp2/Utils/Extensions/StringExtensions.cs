
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Utils.Extensions
{
  public static class StringExtensions
  {
    private static List<char> _hex_symbols = new List<char>((IEnumerable<char>) new char[22]
    {
      'a',
      'b',
      'c',
      'd',
      'e',
      'f',
      'A',
      'B',
      'C',
      'D',
      'E',
      'F',
      '0',
      '1',
      '2',
      '3',
      '4',
      '5',
      '6',
      '7',
      '8',
      '9'
    });

    public static XAttribute XMLNS(this string instance, string prefix = null)
    {
      if (string.IsNullOrEmpty(prefix))
        return new XAttribute((XName) "xmlns", (object) instance);
      return new XAttribute(XNamespace.Xmlns + prefix, (object) instance);
    }

    public static XElement NSXE(
      this string instance,
      XNamespace ns = null,
      params object[] children)
    {
      return new XElement(ns != (XNamespace) null ? ns + instance : (XName) instance, children);
    }

    public static XElement XE(this string instance, params object[] children)
    {
      return new XElement((XName) instance, children);
    }

    public static XAttribute XA(this string instance, object value)
    {
      return new XAttribute((XName) instance, value);
    }
    
    public static byte[] GetBytes(this string value, int CodePage = 1251)
    {
      Encoding encoding = Encoding.GetEncoding(CodePage);
      if (encoding != null && !string.IsNullOrEmpty(value))
        return encoding.GetBytes(value);
      return (byte[]) null;
    }

    public static byte[] GetBytes(this string value, Encoding encoding)
    {
      if (encoding != null && !string.IsNullOrEmpty(value))
        return encoding.GetBytes(value);
      return (byte[]) null;
    }

    public static int ToInt32(this string instance)
    {
      return int.Parse(instance);
    }

    public static short ToInt16(this string instance)
    {
      return short.Parse(instance);
    }

    public static long ToInt64(this string instance)
    {
      return long.Parse(instance);
    }

    public static byte ToInt8(this string instance)
    {
      return byte.Parse(instance);
    }

    public static byte[] FromBase64String(this string str)
    {
      return Convert.FromBase64String(str);
    }

    public static byte[] FromHexString(this string str)
    {
      str = str.Replace(" ", "");
      if (str == null)
        return (byte[]) null;
      if (str.Length % 2 > 0)
        throw new Exception("Wrong argument specified. String length must be even.");
      char[] charArray = str.ToCharArray();
      byte[] numArray = new byte[charArray.Length / 2];
      for (int index = 0; index < charArray.Length; index += 2)
      {
        if (!StringExtensions._hex_symbols.Contains(charArray[index]) || !StringExtensions._hex_symbols.Contains(charArray[index + 1]))
          throw new Exception("Bad symbol found in hexidecimal string value [" + (object) charArray[index] + (object) charArray[index + 1]);
        numArray[index / 2] = byte.Parse(charArray[index].ToString() + charArray[index + 1].ToString(), NumberStyles.HexNumber);
      }
      return numArray;
    }

    public static bool IsValidHexString(this string str)
    {
      if (!Regex.IsMatch(str, "^([a-f|A-F|0-9]{2})+$"))
        str = str.Replace(" ", "");
      return Regex.IsMatch(str, "^([a-f|A-F|0-9]{2})+$");
    }
  }
}
