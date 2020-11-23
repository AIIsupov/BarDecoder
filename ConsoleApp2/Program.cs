using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using Barcode.Converters;


namespace BarDecoder
{
    class Program
    {
        static void Main(string[] args)
        {
            var ports = SerialPort.GetPortNames();
            
            for (int i = 0; i < ports.Length; i++)
            {
                Console.WriteLine("{0}   {1}", i, ports[i]);
            }
            Console.Write("Выберите порт:> ");
            var pi = int.Parse(Console.ReadLine());

            _port = new SerialPort();
            _port = new SerialPort();
            _port.PortName = ports[pi];


            _port.Open();
            _port.DataReceived += Receiver0;

            Console.ReadLine();
            _port.Close();
            _port.Dispose();
        }



        private static SerialPort _port;

        public static void Receiver0(object sender, SerialDataReceivedEventArgs e)
        {

            if (e.EventType != SerialData.Eof)
            {

                int bytesToRead = _port.BytesToRead;
                byte[] buf = new byte[bytesToRead];
                ;
                _port.Read(buf, 0, bytesToRead);

                var valid = BarcodeVersions.IsValidBarcode(buf);
                BarcodeVersion barcodeVersion = BarcodeVersions.GetBarcodeVersion(buf);

                Console.WriteLine(valid);
              
                
                int offset = 0;
                object[] values = new object[7]
                {
                    GetObject(buf, _cnvs[typeof (NumberConverter)], typeof (byte), ref offset),
                    GetObject(buf, _cnvs[typeof (NumberConverter)], typeof (ulong), ref offset),
                    GetObject(buf, _cnvs[typeof (OMS62EncodingStringConverter)], typeof (string), ref offset, 51),
                    GetObject(buf, _cnvs[typeof (NumberConverter)], typeof (byte), ref offset),
                    GetObject(buf, _cnvs[typeof (ShortBirthDateConverter)], typeof (DateTime), ref offset),
                    GetObject(buf, _cnvs[typeof (ShortBirthDateConverter)], typeof (DateTime), ref offset),
                    ((IEnumerable<byte>) buf).ToList<byte>().GetRange(offset, barcodeVersion.Length - offset).ToArray() // электронная подпись
                };
                Console.WriteLine(values[0]);
                Console.WriteLine(values[1]);
                Console.WriteLine(values[2]);
                Console.WriteLine(values[3]);
                Console.WriteLine(values[4]);
                Console.WriteLine(values[5]);
                Console.WriteLine(BitConverter.ToString((byte[])values[6]));
            }

        }

        private static object GetObject(
            byte[] data,
            ITypeConverter converter,
            Type type,
            ref int offset)
        {
            return GetObject(data, converter, type, ref offset, converter.GetLength(type));
        }

        private static object GetObject(
            byte[] data,
            ITypeConverter converter,
            Type type,
            ref int offset,
            int length)
        {
            object obj = converter.ConvertTo(type, data, offset, length);
            offset += length;
            return obj;
        }

        private static Dictionary<Type, ITypeConverter> _cnvs = new Dictionary<Type, ITypeConverter>()
        {
            {
                typeof (OMS5EncodingStringConverter),
                (ITypeConverter) new OMS5EncodingStringConverter()
            },
            {
                typeof (OMS6EncodingStringConverter),
                (ITypeConverter) new OMS6EncodingStringConverter()
            },
            {
                typeof (OMS62EncodingStringConverter),
                (ITypeConverter) new OMS62EncodingStringConverter()
            },
            {
                typeof (NumberConverter),
                (ITypeConverter) new NumberConverter()
            },
            {
                typeof (Int24Converter),
                (ITypeConverter) new Int24Converter()
            },
            {
                typeof (ShortDateConverter),
                (ITypeConverter) new ShortDateConverter()
            },
            {
                typeof (ShortYearConverter),
                (ITypeConverter) new ShortYearConverter()
            },
            {
                typeof (ShortBirthDateConverter),
                (ITypeConverter) new ShortBirthDateConverter()
            }
        };


    }
}
