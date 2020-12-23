using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
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
            public string Date_of_Birth { get; set; }

            /// <summary>
            /// Дата ???
            /// </summary>
            public string _Date { get; set; }

            /// <summary>
            /// ЭЦП
            /// </summary>
            public byte[] Bytecode { get; set; }
        }

        public delegate void BarcodeDecode(Decoder decoder, BarcodeValue data);
        public event BarcodeDecode onBarcodeDecode;

        private static string nameBarDec = "BarDecoder: ";
        private static List<SerialPort> _port;
        public bool validBarcode; // Если штрих-код соответствует формату штрих-кодов Полисов ОМС, то возвращает true 
        public bool IsBarcodeDecode = false;
        public BarcodeValue BarcodeData; // Хранит информацию по последнему прочитанному штрих-коду

        public Decoder()
        {
            //StartDecode();
        }

        public void StartDecode()
        {
            Console.WriteLine(nameBarDec+"Запуск декодера");
            try
            {
                StopDecode();
                string[] ports = SerialPort.GetPortNames();
                if (ports.Length != 0)
                {
                    //Console.WriteLine(nameBarDec + "Доступные порты");
                    //for (int i = 0; i < ports.Length; i++)
                    //{
                    //    Console.WriteLine("{0}: {1}", i, ports[i]);
                    //}
                    //Console.Write(nameBarDec + "Выберите порт:> ");
                    //var pi = int.Parse(Console.ReadLine());

                    _port = new List<SerialPort>();
                    for (int i = 0; i< ports.Length; i++)
                    {
                        
                        _port.Add(new SerialPort());
                        try
                        {
                            _port[i].PortName = ports[i];
                            _port[i].Open();
                            Console.WriteLine(nameBarDec + "{0} Открыт", _port[i].PortName);
                            _port[i].DataReceived += Receiver;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(nameBarDec+ex);
                        }
                    }
                    //while (!IsBarcodeDecode) ;
                }
                else
                {
                    Console.WriteLine(nameBarDec + "Подключения не обнаружены");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(nameBarDec + ex.ToString());
            }
        }

        public int GetPortCount()
        {
            return SerialPort.GetPortNames().Length;
        }

        public void StopDecode()
        {
            if (_port != null && _port[0].IsOpen)
            {
                for(int i = 0; i< _port.Count; i++)
                {
                    _port[i].Close();
                    _port[i].Dispose();
                }
                Console.WriteLine(nameBarDec + "Остановлен");
            }
        }

        private void Receiver(object sender, SerialDataReceivedEventArgs e)
        {
            if (e.EventType != SerialData.Eof)
            {
                IsBarcodeDecode = false;
                byte[] buf = null;
                for (int i = 0; i < _port.Count; i++)
                {
                    if (_port[i].BytesToRead != 0)
                    {
                        int bytesToRead = _port[i].BytesToRead;
                        buf = new byte[bytesToRead];

                        _port[i].Read(buf, 0, bytesToRead);

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
                                Date_of_Birth = Convert.ToDateTime(values[4]).ToShortDateString(),
                                _Date = Convert.ToDateTime(values[5]).ToShortDateString(),
                                Bytecode = (byte[])values[6]
                            };

                            Console.WriteLine(Environment.NewLine+nameBarDec + "Полученные данные от {0}:", _port[i].PortName);
                            Console.WriteLine(nameBarDec + BarcodeData.code_type_Barcode);
                            Console.WriteLine(nameBarDec + BarcodeData.number_Policy);
                            Console.WriteLine(nameBarDec + BarcodeData.FullName);
                            Console.WriteLine(nameBarDec + BarcodeData.Gender);
                            Console.WriteLine(nameBarDec + BarcodeData.Date_of_Birth);
                            Console.WriteLine(nameBarDec + BarcodeData._Date);
                            Console.WriteLine(nameBarDec + BitConverter.ToString(BarcodeData.Bytecode));

                            try
                            {
                                onBarcodeDecode(this, BarcodeData);
                            }
                            catch (NullReferenceException ex)
                            {
                                Console.WriteLine(nameBarDec + ex);
                            }
                            IsBarcodeDecode = true;
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Gabela");
                Console.WriteLine(nameBarDec + @"Что-то не так ¯\_(ツ)_/¯");
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
