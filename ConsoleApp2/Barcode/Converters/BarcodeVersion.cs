
namespace Barcode.Converters
{
    public class BarcodeVersion
    {
        public string RootTagName { get; set; }

        public string Schema { get; set; }

        public int Version { get; set; }

        public int Length { get; set; }

        public int DataLength { get; set; }

        public int SignatureLength { get; set; }
    }
}