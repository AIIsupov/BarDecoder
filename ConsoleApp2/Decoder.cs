using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using Barcode.Converters;


namespace BarDecoder
{
    public class Decoder
    {
        static void Main(string[] args)
        {
            //new Decoder();
        }

        // Класс данных штрих-кода
        public class BarcodeValue
        {
            /// <summary>
            /// Код типа штрих-кода
            /// </summary>
            public int code_type_Barcode { get; set; }

            /// <summary>
            /// Номер полиса
            /// </summary>
            /// 
            public long number_Policy { get; set; }

            /// <summary>
            /// Фамилия Имя Отчество
            /// </summary>
            public string FullName { get; set; }

            /// <summary>
            /// Фамилия
            /// </summary>
            public string Surname { get; set; }

            /// <summary>
            /// Имя
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Отчество
            /// </summary>
            public string Middle_Name { get; set; }

            /// <summary>
            /// Пол
            /// </summary>
            public int Gender { get; set; }

            /// <summary>
            /// Дата рождения
            /// </summary>
            public DateTime Date_of_Birth { get; set; }

            /// <summary>
            /// Дата ???
            /// </summary>
            public DateTime _Date { get; set; }

            /// <summary>
            /// ЭЦП
            /// </summary>
            public byte[] Bytecode { get; set; }
        }

        public delegate void BarcodeDecode(Decoder decoder, BarcodeValue data);
        public event BarcodeDecode onBarcodeDecode;

        private static SerialPort _port;
        public bool validBarcode; // Если штрих-код соответствует формату штрих-кодов Полисов ОМС, то возвращает true 
        public bool IsBarcodeDecode = false;
        public BarcodeValue BarcodeData; // Хранит информацию по последнему прочитанному штрих-коду

        public Decoder()
        {
            //StartDecode();
        }

        public void StartDecode()
        {

            if (_port != null && _port.IsOpen)
            {
                _port.Close();
                _port.Dispose();
            }
            var ports = SerialPort.GetPortNames();
            if (ports.Length != 0)
            {
                //for (int i = 0; i < ports.Length; i++)
                //{
                //    Console.WriteLine("{0}: {1}", i, ports[i]);
                //}
                //Console.Write("Выберите порт:> ");
                //var pi = int.Parse(Console.ReadLine());

                _port = new SerialPort();
                _port.PortName = ports[0];

                _port.Open();
                _port.DataReceived += Receiver;
                //while (!IsBarcodeDecode) ;
            }
        }


        public void StopDecode()
        {
            _port.Close();
            _port.Dispose();
        }

        private void Receiver(object sender, SerialDataReceivedEventArgs e)
        {
            if (e.EventType != SerialData.Eof)
            {
                IsBarcodeDecode = false;

                int bytesToRead = _port.BytesToRead;
                byte[] buf = new byte[bytesToRead];

                _port.Read(buf, 0, bytesToRead);

                validBarcode = BarcodeVersions.IsValidBarcode(buf);
                BarcodeVersion barcodeVersion = BarcodeVersions.GetBarcodeVersion(buf);

                if (validBarcode)
                {
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

                    string[] fullname = values[2].ToString().Split('|');
                    BarcodeData = new BarcodeValue
                    {
                        code_type_Barcode = Convert.ToInt32(values[0]),
                        number_Policy = Convert.ToInt64(values[1]),
                        FullName = String.Format("{0} {1} {2}", fullname[0], fullname[1], fullname[2]),
                        Surname = fullname[0],
                        Name = fullname[1],
                        Middle_Name = fullname[2],
                        Gender = Convert.ToInt32(values[3]),
                        Date_of_Birth = Convert.ToDateTime(values[4]),
                        _Date = Convert.ToDateTime(values[5]),
                        Bytecode = (byte[])values[6]
                    };

                    onBarcodeDecode(this, BarcodeData);
                    IsBarcodeDecode = true;
                    //Console.WriteLine(BarcodeData.code_type_Barcode);
                    //Console.WriteLine(BarcodeData.number_Policy);
                    //Console.WriteLine(BarcodeData.FullName);
                    //Console.WriteLine(BarcodeData.Gender);
                    //Console.WriteLine(BarcodeData.Date_of_Birth);
                    //Console.WriteLine(BarcodeData._Date);
                    //Console.WriteLine(BitConverter.ToString(BarcodeData.Bytecode));
                }
            }

        }

        private static object GetObject(byte[] data,ITypeConverter converter,Type type,ref int offset)
        {
            return GetObject(data, converter, type, ref offset, converter.GetLength(type));
        }

        private static object GetObject(byte[] data,ITypeConverter converter,Type type,ref int offset,int length)
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
