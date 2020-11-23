
using Utils.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Utils.Collections
{
  public class CByte
  {
    private byte[] _inner = (byte[]) null;

    public CByte(params byte[] inner)
    {
      this._inner = inner;
      if (this._inner != null)
        return;
      this._inner = new byte[0];
    }

    public void PadRight(int count, byte value = 0)
    {
      int length = this.Length;
      this._inner = this._inner.PadRight(count);
      this._inner.SetValueEx<byte>(length, count - length, value);
    }

    public CByte Reverse()
    {
      return (CByte) ((IEnumerable<byte>) this._inner).Reverse<byte>().ToArray<byte>();
    }

    public void PadLeft(int count, byte value = 0)
    {
      int length = this.Length;
      this._inner = this._inner.PadLeft(count);
      this._inner.SetValueEx<byte>(0, count - length, value);
    }

    public void AddRange(byte[] data)
    {
      this._inner = this._inner.AddRange<byte>(data);
    }

    public CByte GetRange(int index, int length)
    {
      return new CByte(this._inner.GetRange<byte>(index, length));
    }

    public string ToHexString()
    {
      return this._inner.ToHexString(0, -1);
    }

    public static CByte operator |(CByte a, byte[] b)
    {
      return new CByte(a._inner.Xor(b));
    }

    public static CByte operator |(CByte a, CByte b)
    {
      return new CByte(a._inner.Xor((byte[]) b));
    }

    public static CByte operator |(byte[] a, CByte b)
    {
      return new CByte(a.Xor((byte[]) b));
    }

    public static bool operator !=(CByte a, object b)
    {
      if ((object) a == null && b != null || (object) a != null && b == null)
        return true;
      if ((object) a == null && b == null)
        return false;
      if ((object) a != null && b != null)
      {
        if (b is CByte)
          return a.ToHexString() != ((CByte) b).ToHexString();
        if (b is byte[])
          return a.ToHexString() != ((byte[]) b).ToHexString(0, -1);
      }
      return true;
    }

    public static bool operator ==(CByte a, object b)
    {
      if ((object) a == null && b != null || (object) a != null && b == null)
        return false;
      if ((object) a == null && b == null)
        return true;
      if ((object) a != null && b != null)
      {
        if (b is CByte)
          return a.ToHexString() == ((CByte) b).ToHexString();
        if (b is byte[])
          return a.ToHexString() == ((byte[]) b).ToHexString(0, -1);
      }
      return false;
    }

    public static CByte operator +(CByte a, byte[] b)
    {
      a.AddRange(b);
      return a;
    }

    public static CByte operator +(byte[] a, CByte b)
    {
      return new CByte(a.AddRange<byte>((byte[]) b));
    }

    public static CByte operator +(CByte a, CByte b)
    {
      a.AddRange((byte[]) b);
      return a;
    }

    public static implicit operator CByte(int value)
    {
      return new CByte(new byte[value]);
    }

    public static implicit operator CByte(byte[] value)
    {
      return new CByte(value);
    }

    public static implicit operator CByte(string value)
    {
      if (string.IsNullOrEmpty(value))
        return (CByte) null;
      if (value.ToUpper().StartsWith("S:"))
        return new CByte(value.Substring(2).GetBytes(1251));
      if (value.ToUpper().StartsWith("BASE64:"))
        return new CByte(value.Substring(7).FromBase64String());
      if (value.IsValidHexString())
        return new CByte(value.FromHexString());
      return new CByte(value.GetBytes(1251));
    }

    public static implicit operator string(CByte value)
    {
      return value.ToHexString();
    }

    public static implicit operator byte[](CByte value)
    {
      return value._inner;
    }

    public static implicit operator List<byte>(CByte value)
    {
      return ((IEnumerable<byte>) value._inner).ToList<byte>();
    }

    public void Clear()
    {
      this._inner = new byte[0];
    }

    public override bool Equals(object obj)
    {
      if (obj is byte[])
        return this.GetHashCode() == ((byte[]) obj).GetHashCode();
      if (obj is CByte)
        return this.GetHashCode() == ((CByte) obj).GetHashCode();
      return false;
    }

    public void Fill(int index, int length, byte value)
    {
      this._inner.SetValue((object) index, length, (int) value);
    }

    public override int GetHashCode()
    {
      return this._inner.GetHashCode();
    }

    public override string ToString()
    {
      return (string) this;
    }

    public byte this[int index]
    {
      get
      {
        return this._inner[index];
      }
      set
      {
        this._inner[index] = value;
      }
    }

    public CByte this[int index = 0, int length = 0]
    {
      get
      {
        return (CByte) this._inner.GetRange<byte>(index, length);
      }
      set
      {
        this._inner.SetRange<byte>(index, (byte[]) value);
      }
    }

    public List<byte> ToList()
    {
      return ((IEnumerable<byte>) this._inner).ToList<byte>();
    }

    public byte[] ToArray()
    {
      return this._inner;
    }

    public int Length
    {
      get
      {
        return this._inner.Length;
      }
    }

    public int Count
    {
      get
      {
        return this._inner.Length;
      }
    }
  }
}
