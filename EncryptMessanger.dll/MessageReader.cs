using EncryptMessanger.dll.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EncryptMessanger.dll
{
    public class MessageReader
    {
        
        string datas;
        NetworkStream _stream;
        public string LastEncrMessage;
        public Message currentMessage;
        //XmlReader _reader;
        //MemoryStream _readerMs;
        bool encrypted = false;
        ICryptoTransform decryptor;
        //читает из сетевого потока  из записывает в поток XMLReader
        //private bool NetToXml()
        //{
        //    //_readerMs.SetLength(0);
        //    byte[] len = new byte[4];
        //    _stream.Read(len, 0, len.Length);
        //    int messageLength = BitConverter.ToInt32(len,0);

        //    byte[] data = new byte[messageLength];
        //    int bcount = _stream.Read(data, 0, data.Length);
        //    //byte[] realData = new byte[bcount];
        //    //Array.Copy(data, realData, bcount);
        //    if (!encrypted)
        //    {
        //        datas = Encoding.UTF8.GetString(data);               
        //        _readerMs.Write(data, 0, bcount);
        //        _readerMs.Position = _readerMs.Position - bcount;
        //        //_readerMs.Position = 0;
        //    }
        //    else
        //    {
        //        MemoryStream ms = new MemoryStream();
        //        byte[] b = new byte[bcount];
        //        //decryptor.TransformBlock(data, 0, bcount, b, 0);
        //        //string decDataT = Encoding.UTF8.GetString(b);
        //        CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write);
        //        cs.Write(data, 0, bcount);
        //        string decData = Encoding.UTF8.GetString(ms.GetBuffer());
        //        try
        //        {
        //            cs.Close();
        //        }
        //        catch(Exception ex) { }
        //        decData = Encoding.UTF8.GetString(ms.GetBuffer());
        //        decData = decData.Substring(0, decData.IndexOf('\0'));

        //        //cs.FlushFinalBlock();
        //        //b = Encoding.UTF8.GetBytes(decData);
        //        //cs.Close();
        //        _readerMs.Write(ms.GetBuffer(), 0, (int)ms.Length);
        //        _readerMs.Position = _readerMs.Position - ms.Length;
        //        //_readerMs.Write(b, 0, (int)b.Length);
        //        //_readerMs.Position = _readerMs.Position - b.Length;
        //        //ms.Close();
        //    }
        //    //_readerMs.Position = 0;
        //    //_reader = XmlReader.Create(new StreamReader(_readerMs, Encoding.UTF8), new XmlReaderSettings()
        //    //{
        //    //    // ConformanceLevel.Fragment recome 
        //    //    ConformanceLevel = ConformanceLevel.Auto,
        //    //    ValidationType = ValidationType.None,
        //    //    DtdProcessing = DtdProcessing.Ignore
        //    //});

        //    return bcount > 0;
        //}
        public MessageReader(NetworkStream stream)
        {
            //_readerMs = new MemoryStream();
            //byte[] initByte = Encoding.UTF8.GetBytes("<hi />");
            //_readerMs.Write(initByte,0,initByte.Length);
            //_readerMs.Position = 0;
            _stream = stream;
            //_reader = new XmlTextReader(_readerMs);
            //_reader.Settings = new XmlReaderSettings();
            //_reader = XmlReader.Create(new StreamReader(_readerMs, Encoding.UTF8), new XmlReaderSettings()
            //{
            //    // ConformanceLevel.Fragment recome 
            //    ConformanceLevel = ConformanceLevel.Fragment,
            //    ValidationType = ValidationType.None,
            //    DtdProcessing = DtdProcessing.Ignore,
                
            //});
            //_reader.Read();
            //_reader.MoveToContent();
        }
        //private MessageType GetTypeByName(string name)
        //{
        //    switch (name)
        //    {
        //        case "message":
        //            {
        //                return MessageType.TextMessage;
        //                break;
        //            }
        //        case "stream":
        //            {
        //                if (IsStartElement())
        //                {
        //                    return MessageType.StartStreamMessage;
        //                }
        //                else
        //                {
        //                    return MessageType.EndStreamMessage;
        //                }
        //                break;
        //            }
        //        case "auth":
        //            {
        //                return MessageType.AuthMessage;
        //                break;
        //            }
        //        case "authresponce":
        //            {
        //                return MessageType.AuthResponceMessage;
        //                break;
        //            }
        //        case "processed":
        //            {
        //                return MessageType.ProcessedMessage;
        //                break;
        //            }
        //        case "SymetricKey":
        //            {
        //                return MessageType.SymKeyMessage;
        //                break;
        //            }
        //        case "PublicKey":
        //            {
        //                return MessageType.PublicKeyMessage;
        //                break;
        //            }
        //        default:
        //            {
        //                return MessageType.AbstractMessage;
        //                break;
        //            }
        //    }

        //}
        //public MessageType MessageType
        //{
        //    get { return currentMessage.GetType(); }
        //}
        public void ProtectStream(ICryptoTransform transform)
        {
            decryptor = transform;
            encrypted = true;
            //bool b=_stream.CanRead;
            //_reader.Close();
            //b = _stream.CanRead;
            //CryptoStream outStreamEncrypted = new CryptoStream(_stream, transform, CryptoStreamMode.Read);
            
            ////_reader.Close();
            //// _reader.Dispose();
            
            //XmlTextReader reader = new XmlTextReader(outStreamEncrypted);
            //_reader = reader;
            //_reader.Settings.ConformanceLevel = ConformanceLevel.Fragment;
            // _reader = XmlReader.Create(new StreamReader(outStreamEncrypted, Encoding.UTF8), new XmlReaderSettings()
            //{
            //    ConformanceLevel = ConformanceLevel.Auto,
            //    ValidationType = ValidationType.None,
            //    DtdProcessing = DtdProcessing.Ignore

            //});

        }
        //public string ReadInnerXml()
        //{
        //    return _reader.ReadInnerXml();
        //}
        //public void Read()
        //{
        //    //NetToXml();
        //    _reader.Read();
        //}
        public Message ReadNext()
        {
            byte[] len = new byte[4];
            _stream.Read(len, 0, len.Length);
            int messageLength = BitConverter.ToInt32(len, 0);
            
            byte[] data = new byte[messageLength];
            int bcount = _stream.Read(data, 0, data.Length);
            currentMessage = new Message();
            if (encrypted)
            {
                //MemoryStream ms = new MemoryStream();
                //byte[] encryptedBytes = new byte[messageLength];
                LastEncrMessage = BitConverter.ToString(data);
                //LastEncrMessage = Encoding.UTF8.GetString(data);
                byte[] decryptedBytes = decryptor.TransformFinalBlock(data,0, messageLength);
                //decryptor.TransformBlock(data,0, messageLength, decryptedBytes,0);
                //CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write);
                //cs.Write(data, 0, messageLength);
                //string decData = Encoding.UTF8.GetString(ms.GetBuffer());
                //try
                //{
                //    cs.Close();
                //}
                //catch (Exception ex) { }
                //currentMessage.FillFromBytes(ms.GetBuffer());
                currentMessage = Message.CreateMessage(decryptedBytes);
            }
            else
            {
                currentMessage = Message.CreateMessage(data);
            }

            
            return currentMessage;
        }
       
        public byte[] GetAttribute(Atribute atributeName)
        {
            return currentMessage.GetAttribute(atributeName);
        }
        //public bool IsStartElement()
        //{
        //    return _reader.IsStartElement();
        //}
        public void Close()
        {           
            _stream.Close();
        }

    }
}
