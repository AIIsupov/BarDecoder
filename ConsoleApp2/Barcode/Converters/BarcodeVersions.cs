
using System;
using System.Collections.Generic;
using System.Linq;

namespace Barcode.Converters
{
    public static class BarcodeVersions
    {
        public static BarcodeVersion Verion_1 = new BarcodeVersion()
        {
            Version = 1,
            Length = 130,
            DataLength = 65,
            SignatureLength = 65,
            RootTagName = "BarcodeData"//,
            //Schema = Resources.BarcodeSchema
        };
        public static BarcodeVersion Verion_2 = new BarcodeVersion()
        {
            Version = 2,
            Length = 130,
            DataLength = 65,
            SignatureLength = 65,
            RootTagName = "BarcodeData_V2"//,
            //Schema = Resources.BarcodeSchema
        };
        public static List<BarcodeVersion> Versions = new List<BarcodeVersion>()
        {
            BarcodeVersions.Verion_1,
            BarcodeVersions.Verion_2
        };

        public static BarcodeVersion GetBarcodeVersion(byte[] data)
        {
            IEnumerable<BarcodeVersion> source = BarcodeVersions.Versions.Where<BarcodeVersion>((Func<BarcodeVersion, bool>)(v => v.Version == (int)data[0] && data.Length == v.Length));
            return source.Count<BarcodeVersion>() > 0 ? source.First<BarcodeVersion>() : (BarcodeVersion)null;
        }

        public static bool IsValidBarcode(byte[] data)
        {
            return data != null && data.Length > 0 && BarcodeVersions.Versions.Where<BarcodeVersion>((Func<BarcodeVersion, bool>)(v => v.Version == (int)data[0] && data.Length == v.Length)).Count<BarcodeVersion>() > 0;
        }
    }
}