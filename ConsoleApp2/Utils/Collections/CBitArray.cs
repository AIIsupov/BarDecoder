
using System;

namespace Utils.Collections
{
  public class CBitArray
  {
    private CByte _inner = (CByte) 0;

    public CBitArray()
    {
    }

    public CBitArray(CByte data)
    {
      this._inner = data;
    }

    public bool Get(int index)
    {
      int index1 = (index - index % 8) / 8;
      index -= index1 * 8;
      if (index1 >= this._inner.Length)
        throw new ArgumentException("The index is out of the array bounds.");
      return ((int) this._inner[index1] & 128 >> index) > 0;
    }

    public void Set(int index, bool value = true)
    {
      int index1 = (index - index % 8) / 8;
      index -= index1 * 8;
      if (index1 >= this._inner.Length)
        this._inner += (CByte) (index1 + 1 - this._inner.Length);
      this._inner[index1] = value ? (byte) ((uint) this._inner[index1] | (uint) (128 >> index)) : (byte) ((uint) this._inner[index1] & (uint) (128 >> index ^ (int) byte.MaxValue));
    }

    public override string ToString()
    {
      return this._inner.ToString();
    }

    public CByte ToArray()
    {
      return this._inner;
    }
  }
}
