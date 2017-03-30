using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml;
using EncryptMessanger.dll.Messages;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.IO;




namespace EncryptMessanger.dll
{
    public class MessageWriter
    {
        //MemoryStream _WriterMs;
        bool encrypted = false;
        ICryptoTransform encryptor;
        //private void XmlToNet()
        //{
        //    //в любом случае получает весь буфер
        //    byte[] data = _WriterMs.GetBuffer();
        //    byte[] realData = new byte[_WriterMs.Length];
        //    Array.Copy(data, realData, realData.Length);
        //    _WriterMs.Position = 0;
        //    _WriterMs.SetLength(0);
        //    if (!encrypted)
        //    {
        //        byte[] dataWithLength = AddLength(realData);
        //        _stream.Write(dataWithLength, 0, dataWithLength.Length);
        //    }
        //    else
        //    {
        //        MemoryStream ms = new MemoryStream();
        //        CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
        //        cs.Write(data, 0, data.Length);
        //        int l = (int)ms.Length;
        //        cs.Close();

        //        byte[] encryptedData = ms.GetBuffer();
        //        byte[] realEncryptedData = new byte[l];
        //        Array.Copy(encryptedData, 0, realEncryptedData, 0, l);
        //        byte[] encryptedDataWithLength = AddLength(realEncryptedData);
        //        _stream.Write(encryptedDataWithLength, 0, encryptedDataWithLength.Length);
        //    }
        //    _stream.Flush();
        //}
        public MessageWriter(NetworkStream stream)
        {
            //_WriterMs = new MemoryStream();
            _stream = stream;
            //_writer = XmlWriter.Create(new System.IO.StreamWriter(_WriterMs, Encoding.UTF8), new XmlWriterSettings()
            //{
            //    ConformanceLevel = ConformanceLevel.Auto,
            //    OmitXmlDeclaration = false,
            //    Indent = true,
            //    NamespaceHandling = NamespaceHandling.OmitDuplicates
            //});
        }
        public void ProtectStream(ICryptoTransform transform)
        {
            encryptor = transform;
            encrypted = true;
            //CryptoStream outStreamEncrypted = new CryptoStream(_stream, transform, CryptoStreamMode.Write);
            //_writer = XmlWriter.Create(new System.IO.StreamWriter(outStreamEncrypted, Encoding.UTF8), new XmlWriterSettings()
            //{
            //    ConformanceLevel = ConformanceLevel.Auto,
            //    OmitXmlDeclaration = false,
            //    Indent = true,
            //    NamespaceHandling = NamespaceHandling.OmitDuplicates
            //});
        }
        //private XmlWriter _writer;
        //public XmlWriter XmlWriter
        //{
        //    get
        //    {
        //        return _writer;
        //    }
        //}
        private NetworkStream _stream;
        public void Close()
        {
            _stream.Close();
        }
        private byte[] AddLength(byte[] message)
        {
            byte[] messageWithLenth = new byte[message.Length + 4];
            Array.Copy(BitConverter.GetBytes(message.Length), messageWithLenth, 4);
            message.CopyTo(messageWithLenth, 4);
            return messageWithLenth;
        }
        public void WriteMessage(Message message)
        {
            byte[] messageBytes = message.ToByte();
            byte[] messageBytesWithLength;
            if (encrypted)
            {
                //MemoryStream ms = new MemoryStream();
                //CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
                //cs.Write(messageBytes, 0, messageBytes.Length);
                //cs.Flush();
                //byte[] mes = new byte[ms.Length];
                //cs.Close();
                byte[] encBytes = encryptor.TransformFinalBlock(messageBytes,0, messageBytes.Length);

                //encryptor.TransformBlock(messageBytes,0, messageBytes.Length, encBytes, 0);
                //Array.Copy(ms.GetBuffer(), mes, mes.Length);
                messageBytesWithLength = AddLength(encBytes);
                //ms.Close();
            }
            else
            {
                messageBytesWithLength = AddLength(messageBytes);
            }            
            _stream.Write(messageBytesWithLength, 0, messageBytesWithLength.Length);
            _stream.Flush();           

        }

    }

}

