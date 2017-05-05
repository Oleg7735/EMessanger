using EncryptMessanger.dll.Encription;
using EncryptMessanger.dll.Messages;
using EncryptMessanger.dll.Messages.FileMessages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.FileTransfer
{
    public class FileReceiver
    {
        private int _dataDose = 256;
        public async Task<byte[]> ReceiveFileForServerAsync(string fileName, IPEndPoint point)
        {
            
            return await Task.Run(() => {
                return ReceiveFileForServer(fileName, point);
                
            });
            
        }
        public byte[] ReceiveFileForServer(string fileName, IPEndPoint point)
        {
            TcpClient client = new TcpClient();
            client.Connect(point);
            MessageReader reader = new MessageReader(client.GetStream());
            Message newMessage;
            FileStream fileStream = new FileStream(fileName, FileMode.Create);
            while (true)
            {
                newMessage = reader.ReadNext();
                switch (newMessage.Type)
                {
                    case MessageType.FileFragmentMessage:
                        {
                            FileFragmentMessage fileFragment = newMessage as FileFragmentMessage;
                            fileStream.Write(fileFragment.Data, 0, fileFragment.Data.Length);
                            break;
                        }
                    case MessageType.EndFileMessage:
                        {
                            fileStream.Close();
                            reader.Close();
                            return ((EndFileMessage)newMessage).Signature;
                        }
                }
            }
        }
        public async Task<byte[]> ReceiveFileForClientAsync(string fileName, IPEndPoint point, ClientClientEncryptedSession session)
        {
            return await Task.Run(() =>
            {
                return ReceiveFileForClient(fileName, point, session);
            });
        }
        public byte[] ReceiveFileForClient(string fileName, IPEndPoint point, ClientClientEncryptedSession session)
        {
            
            TcpListener listener = new TcpListener(point);
            listener.Start();
            TcpClient client = listener.AcceptTcpClient();
            listener.Stop();
            NetworkStream netStream = client.GetStream();
            MessageReader reader = new MessageReader(netStream);
            FileStream fileStream = new FileStream(fileName, FileMode.Create);
            byte[] decryptedData;
            Message newMessage;
            //newMessage = reader.ReadNext();
            //decryptedData = session.Dectypt(((FileFragmentMessage)newMessage).Data);
            //int realLength = BitConverter.ToInt32(decryptedData.Take(4).ToArray(), 0);
            //decryptedData = decryptedData.Skip(4).ToArray();
            //fileStream.Write(decryptedData, 0, decryptedData.Length);
            session.InitStreamDecryptor();
            long realLength = 0;
            bool firstFaragment = true;
            while (true)
            {
                newMessage = reader.ReadNext();
                switch (newMessage.Type)
                {
                    case MessageType.FileFragmentMessage:
                        {
                            FileFragmentMessage fileFragment = newMessage as FileFragmentMessage;
                            decryptedData = session.DecryptBytesAsStream(fileFragment.Data);
                            
                            if (firstFaragment)
                            {
                                //чтобы избавиться от нулей
                                decryptedData = decryptedData.Take(decryptedData.Length - 16).ToArray();
                                //Debug.WriteLine(" ");
                                //foreach (byte b in fileFragment.Data)
                                //{
                                //    Debug.Write(" " + b.ToString());
                                //}
                                //Debug.WriteLine(" ");
                                //foreach (byte b in decryptedData)
                                //{
                                //    Debug.Write(" " + b.ToString());
                                //}
                                realLength = BitConverter.ToInt64(decryptedData, 0);
                                //realLength += 8;
                                //realLength += 32;
                                if (realLength < _dataDose)
                                {
                                    decryptedData = decryptedData.Take((int)realLength).ToArray();
                                }
                                fileStream.Write(decryptedData, 8, decryptedData.Length - 8);
                                realLength = realLength - _dataDose + 8 + 16;
                                firstFaragment = false;
                            }
                            else
                            {
                                if (realLength < _dataDose)
                                {
                                    decryptedData = decryptedData.Take((int)realLength).ToArray();
                                }
                                fileStream.Write(decryptedData, 0, decryptedData.Length);
                                realLength -= _dataDose;
                            }
                            break;
                        }
                    case MessageType.EndFileMessage:
                        {
                            fileStream.Close();
                            reader.Close();
                            return ((EndFileMessage)newMessage).Signature;
                        }
                }
            }
            
        }
    }
}
