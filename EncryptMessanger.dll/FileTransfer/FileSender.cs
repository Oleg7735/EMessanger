using EncryptMessanger.dll.Encription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using EncryptMessanger.dll.Messages.FileMessages;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Threading;

namespace EncryptMessanger.dll.FileTransfer
{
    class FileSender
    {
        private int _dataDose = 256;
        public async Task SendFileToServerAsync(string fileName, ClientClientEncryptedSession session, IPEndPoint point)
        {
            await Task.Run(() => {
                SendFileToServer(fileName, session, point);
            });
            
        }
        public void SendFileToServer(string fileName, ClientClientEncryptedSession session, IPEndPoint point)
        {
            ICryptoTransform encryptor = session.GetEncryptor();
            FileInfo info = new FileInfo(fileName);
            TcpListener listener = new TcpListener(point);
            listener.Start();
            TcpClient client = listener.AcceptTcpClient();
            listener.Stop();
            NetworkStream netStream = client.GetStream();
            MessageWriter writer = new MessageWriter(netStream);

            byte[] dataBytes = new byte[_dataDose];

            FileStream stream = new FileStream(fileName, FileMode.Open);
            byte[] signature = session.SignFile(stream);
            //byte[] nameBytes = session.Encrypt(Encoding.UTF8.GetBytes(info.Name));
            //byte[] realLength = BitConverter.GetBytes(info.Length);
            stream.Seek(0, SeekOrigin.Begin);//changed
            byte[] encryptedDataBytes;
            //long encryptedDataLength = 0;
            //считываем первый блок и добавляем к ниму реальную длину
            int bytesCount = stream.Read(dataBytes, 8, dataBytes.Length - 8);
            BitConverter.GetBytes(info.Length).CopyTo(dataBytes, 0);
            bytesCount += 8;          
             
            session.InitStreamEncryptor();          
            while (bytesCount > 0)
            {
                //Debug.Write(Encoding.UTF8.GetString(dataBytes));              
                encryptedDataBytes = session.EncryptBytesAsStream(dataBytes);                
                writer.WriteMessage(new FileFragmentMessage(encryptedDataBytes));
                Array.Clear(dataBytes, 0, dataBytes.Length);
                bytesCount = stream.Read(dataBytes, 0, dataBytes.Length);

            } //chenged
            writer.WriteMessage(new EndFileMessage(signature));
            stream.Close();
            //writer.Close();
        }
        public async Task SendFileToClientAsync(string fileName, IPEndPoint point, byte[] signature)
        {
            await Task.Run(() =>
            {           
            FileStream reader = new FileStream(fileName, FileMode.Open);
            TcpClient client = new TcpClient();
            client.Connect(point);
            MessageWriter writer = new MessageWriter(client.GetStream());
            byte[] dataBytes = new byte[_dataDose];
            int bytesCount = reader.Read(dataBytes, 0, dataBytes.Length);
            while (bytesCount > 0)
            {                
                writer.WriteMessage(new FileFragmentMessage(dataBytes));
                Array.Clear(dataBytes, 0, dataBytes.Length);
                bytesCount = reader.Read(dataBytes, 0, dataBytes.Length);
                if(bytesCount != _dataDose)
                {
                    dataBytes = dataBytes.Take(bytesCount).ToArray();
                }
            } 

            writer.WriteMessage(new EndFileMessage(signature));
            reader.Close();
            Thread.Sleep(2000);
            //writer.Close();
            });
        }
    }
}
