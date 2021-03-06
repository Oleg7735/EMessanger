﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Xml;
using System.IO;
using System.Diagnostics;
using System.Threading;
using EncryptMessanger.dll;
using EncryptMessanger.dll.Messages;
using EncryptMessanger.dll.Messages.FileMessages;
using EncryptMessanger.dll.Encription;
using EncryptMessanger.dll.Authentification;
using EncryptMessangerClient.Events;
using EncryptMessangerClient.Model;
using EncryptMessanger.dll.SendibleData;
using EncryptMessanger.dll.FileTransfer;
using System.Net.NetworkInformation;
using EncryptMessanger.dll.Messages.UserSearch;
using EncryptMessanger.dll.Messages.DialogCreation;
using EncryptMessanger.dll.Messages.SingleMessageDeletion;

namespace EncryptMessangerClient
{
    public class Client
    {
        private EncryptedSessionManager _sessionManager;
        private string _sessionSaveFile = AppDomain.CurrentDomain.BaseDirectory + "\\encryptionParams.ekf";
        private string _serverIP;// = "192.168.0.100";
        private int _serverPort = 11000;
        //private const string _clientClientEncryptionIP = ;// = "192.168.0.100";
        private int _clientClientEncryptionPort = 11002;
        private const int _fileSendPortStartIndex = 11100;
        private const int _fileSendPortEndIndex = 11110;
        private const int _fileReceivePortStartIndex = 11111;
        private const int _fileReceivePortEndIndex = 11120;
        private TcpClient _client = new TcpClient();
        private MessageWriter _messageWriter;
        private MessageReader _messageReder;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        public EventHandler<NewMessageEventArgs> Resive;
        public EventHandler<AuthErrorEventArgs> AuthError;
        public EventHandler<RegistrationErrorEventArgs> RegistrationError;
        public EventHandler<RegistrationSuccessEventArgs> RegistrationSuccess;
        public EventHandler<ClientStatusOnlineEventArgs> ClientOnline;
        public EventHandler<ClientStatusExitEventArgs> ClientExit;
        public GetDialogEncryptionSettings GetDialogSettings;
        public EventHandler<EncryptionSettingsEventArgs> EncryptionSettingsChanged;
        //Обработчик при получении диалогов пользователя
        public EventHandler<DialogsReceivedEventArgs> DilogsReceived;
        public EventHandler<ClientAuthSuccessEventArgs> AuthSuccess;
        public EventHandler<DialogUserInfoReceivedEventArgs> UserInfoReceived;
        public EventHandler<DialogSessionFaildEventArgs> DialogSessionFaild;
        public EventHandler<DialogSessionSuccessEventArgs> DialogSessionSuccess;
        public EventHandler<MessagesReceivedEventArgs> MessagesInfoReceived;
        public EventHandler<DialogSessionSuccessEventArgs> SessionUpdated;
        public EventHandler<DialogUserInfoReceivedEventArgs> UserFind;
        public EventHandler<ErrorMessageEventArgs> SendError;
        public EventHandler<MessageDeletedEventArgs> MessageDeleted;
        //public EventHandler<EncryptionSettingsEventArgs> RegisteationSuccess;

        //private List<ClientClientEncryptedSession> _sessions = new List<ClientClientEncryptedSession>();
        private string _currentUserLogin = "user2";
        private long _currentUserId;
        private string _currentUserPassword = "222";
        private Queue<TextMessage> _messageQueue = new Queue<TextMessage>();
        private bool isLocked = false;

        //private List<Message> _messageList = new List<Message>();
        public string Login
        {
            get { return _currentUserLogin; }
        }

        public Client()
        {
            

            _serverIP = GetServerIp();

            ConnectToServer();
        }
        private void SessionAddOrReplase(ClientClientEncryptedSession session)
        {
            _sessionManager.SessionAddOrReplase(session);
        }
        private ClientClientEncryptedSession FindSession(long dialogId)
        {
            return _sessionManager.FindSession(dialogId);
        }
        public Task StartAsync()
        {
            _sessionManager = new EncryptedSessionManager(_sessionSaveFile, _currentUserId);
            return Task.Run(() =>
            {
                CancellationToken token = _cancellationTokenSource.Token;
                do
                {
                    EncryptMessanger.dll.Messages.Message newMessage = _messageReder.ReadNext();
                    switch (newMessage.Type)
                    {
                        case MessageType.ClientOnlineMessage:
                            {
                                ClientOnlineMessage clientOnline = newMessage as ClientOnlineMessage;
                                ClientOnline?.Invoke(this, new ClientStatusOnlineEventArgs(clientOnline.ClientId));
                                break;
                            }
                        case MessageType.ClientExitMessage:
                            {
                                ClientExitMessage clientExit = newMessage as ClientExitMessage;
                                ClientExit?.Invoke(this, new ClientStatusExitEventArgs(clientExit.UserId));
                                break;
                            }
                        case MessageType.CreateCryptoSessionRequest:
                            {
                                CreateCryptoSessionRequest request = newMessage as CreateCryptoSessionRequest;
                                CreateSessionAsReceiver(request);
                                break;
                            }
                        case MessageType.CreateCryptoSessionResponse:
                            {
                                CreateCryptoSessionResponse response = newMessage as CreateCryptoSessionResponse;
                                if (response.Response)
                                {
                                    CreateSessionAsSender(response);
                                }
                                else
                                {
                                    if (!String.IsNullOrEmpty(response.Error))
                                    {
                                        SendError?.Invoke(this, new ErrorMessageEventArgs(response.Error));
                                    }
                                    else
                                    {
                                        SendError?.Invoke(this, new ErrorMessageEventArgs("Не удалось обновить ключи шифрования"));
                                    }
                                }
                                break;
                            }

                        case MessageType.StartStreamMessage:
                            {
                                //EncryptionProvider encryptionProvider = new EncryptionProvider();
                                //encryptionProvider.ClientServerEncrypt(_messageWriter, _messageReder);
                                //Authentificator auth = new Authentificator();
                                //auth.ClientAuth(_messageWriter, _messageReder, _currentUserLogin, _currentUserPassword);
                                break;
                            }
                        case MessageType.PublicKeyMessage:
                            {
                                break;
                            }
                        case MessageType.SymKeyMessage:
                            {
                                break;
                            }
                        case MessageType.ClientPublicKeyMessage:
                            {
                                ClientAKeyMessage keyMessage = newMessage as ClientAKeyMessage;
                                
                                break;
                            }
                        case MessageType.TextMessage:
                            {
                                TextMessage newTextMessage = newMessage as TextMessage;
                                ClientClientEncryptedSession session = FindSession(newTextMessage.Dialog);
                                if (session == null)
                                {
                                    throw new Exception("Невозможно расшифровать входящее сообщение. Сессия не найдена.");                                    
                                }
                                //OnResiveMessages(newTextMessage.MessageId, Encoding.UTF8.GetString(session.Dectypt(newTextMessage.byteText)), newTextMessage.Dialog, newTextMessage.From, newTextMessage.SendDate, !session.VerifyData(newTextMessage.byteText, newTextMessage.GetSignature(), newTextMessage.From));
                                string text;
                                
                                try
                                {
                                    //расшифровываем текст если он был зашифрован
                                    if(newTextMessage.Encrypted)
                                    {
                                        text = Encoding.UTF8.GetString(session.Dectypt(newTextMessage.byteText));
                                    }
                                    else
                                    {
                                        text = Encoding.UTF8.GetString(newTextMessage.byteText);
                                    }
                                    if (!newTextMessage.HasAttach)
                                    {
                                        bool isAltered = false;
                                        if(newTextMessage.Signed)
                                        {
                                            isAltered = !session.VerifyData(newTextMessage.byteText, newTextMessage.GetSignature(), newTextMessage.From);
                                        }
                                        OnResiveMessages(newTextMessage.MessageId, text, newTextMessage.Dialog, newTextMessage.From, newTextMessage.SendDate, isAltered);
                                    }
                                    else
                                    {
                                        OnResiveMessages(newTextMessage.MessageId, text, newTextMessage.Dialog, newTextMessage.From, newTextMessage.SendDate, false, newTextMessage.AttachId);
                                    }
                                }
                                catch (System.Security.Cryptography.CryptographicException)
                                {
                                    text = "";
                                    if (!newTextMessage.HasAttach)
                                    {
                                        OnResiveMessages(newTextMessage.MessageId, text, newTextMessage.Dialog, newTextMessage.From, newTextMessage.SendDate, false, "Не удалось расшифровать данное сообщение!");
                                    }
                                    else
                                    {
                                        OnResiveMessages(newTextMessage.MessageId, Encoding.UTF8.GetString(session.Dectypt(newTextMessage.byteText)), newTextMessage.Dialog, newTextMessage.From, newTextMessage.SendDate, false, newTextMessage.AttachId, "Не удалось расшифровать данное сообщение!");
                                    }
                                }
                                
                                //Debug.WriteLine("Send message signature: "+Encoding.UTF8.GetString(newTextMessage.GetSignature()));
                                //Debug.WriteLine("Send message text: " + Encoding.UTF8.GetString(newTextMessage.byteText));
                                //if(session.VerifyData(newTextMessage.byteText, newTextMessage.GetSignature()))
                                //{
                                //    //код выполняющийся при совпадени хешей
                                //    int p = 0;
                                //}
                                break;
                            }
                        case MessageType.DialogEncryptionSettingsMessage:
                            {
                                DialogEncryptionSettingsMessage settings = newMessage as DialogEncryptionSettingsMessage;
                                //ChangeEncryptionSettings(settings.From, settings.UseSign, settings.UseEncrypt);
                                SetSessionEncrSettings(settings.From, settings.UseSign, settings.UseEncrypt);
                                EncryptionSettingsChanged?.Invoke(this, new EncryptionSettingsEventArgs(settings.UseSign, settings.UseEncrypt, settings.Dialog));
                                break;
                            }
                        case MessageType.DialogResponceMessage:
                            {
                                
                                DialogsResponceMessage dialogsRespoce = newMessage as DialogsResponceMessage;

                                DilogsReceived?.Invoke(this, new DialogsReceivedEventArgs(dialogsRespoce.GetDialogsInfo()));
                                break;
                                
                            }
                        case MessageType.UserInfoResponceMessage:
                            {
                                UserInfoResponceMessage userInfoResponce = newMessage as UserInfoResponceMessage;

                                UserInfoReceived?.Invoke(this, new DialogUserInfoReceivedEventArgs(userInfoResponce.UserId, userInfoResponce.Login, userInfoResponce.State));
                                break;
                            }
                        case MessageType.DialogMessagesResponceMessage:
                            {
                                DialogMessagesResponceMessage responce = newMessage as DialogMessagesResponceMessage;
                                MessageSendibleInfo[] messagesInfo = responce.GetMessagesInfo().ToArray();
                                if (messagesInfo.Length != 0)
                                {
                                    long messagesDialogId = messagesInfo[0].DialogId;
                                    MessagesReceivedEventArgs args = new MessagesReceivedEventArgs(messagesDialogId);
                                    ClientClientEncryptedSession dialogSession = FindSession(messagesDialogId);
                                    if (dialogSession == null)
                                    {                                       
                                        throw new Exception("Невозможно расшифровать входящее сообщение. Сессия не найдена.");
                                    }
                                    string text;
                                    foreach (MessageSendibleInfo info in messagesInfo)
                                    {
                                        //Debug.WriteLine("Load message signature: " + Encoding.UTF8.GetString(info.Signature));
                                        //Debug.WriteLine("Load message text: " + Encoding.UTF8.GetString(info.Text));
                                        try
                                        {
                                            if (info.Encrypted)
                                            {
                                                text = Encoding.UTF8.GetString(dialogSession.Dectypt(info.Text));
                                            }
                                            else
                                            {
                                                text = Encoding.UTF8.GetString(info.Text);
                                            }
                                            if (!info.HasAttach)
                                            {
                                                bool isAltered = false;
                                                if (info.Signed)
                                                {
                                                    isAltered = !dialogSession.VerifyData(info.Text, info.Signature, info.AuthorId);
                                                }
                                                args.AddMessage(info.MessageId, info.AuthorId, text, info.SendTime, isAltered);
                                            }
                                            else
                                            {
                                                args.AddMessage(info.MessageId, info.AuthorId, text, info.SendTime, false, info.AttachId);
                                            }
                                        }
                                        catch (System.Security.Cryptography.CryptographicException)
                                        {
                                            text = "";
                                            if (!info.HasAttach)
                                            {
                                                args.AddMessage(info.MessageId, info.AuthorId, text, info.SendTime, false, "Не удается расшифровать сообщение.");
                                            }
                                            else
                                            {
                                                args.AddMessage(info.MessageId, info.AuthorId, text, info.SendTime, false, info.AttachId, "Не удается расшифровать сообщение.");

                                            }
                                        }
                                    }
                                    MessagesInfoReceived?.Invoke(this, args);
                                    
                                }
                                break;
                            }
                        case MessageType.SearchUserResponceMessage:
                            {
                                SearchUserResponceMessage responce = newMessage as SearchUserResponceMessage;
                                UserFind?.Invoke(this, new DialogUserInfoReceivedEventArgs(responce.UserId, responce.Login, responce.State));
                                break;
                            }
                        case MessageType.MessageDeletedMessage:
                            {
                                MessageDeletedMessage deletedMessage = newMessage as MessageDeletedMessage; 
                                MessageDeleted?.Invoke(this, new MessageDeletedEventArgs(deletedMessage.MessageId));
                                break;
                            }
                        case MessageType.DialogCreatedMessage:
                            {
                                DialogCreatedMessage dialogCreatedMessage = newMessage as DialogCreatedMessage;
                                DilogsReceived?.Invoke(this, new DialogsReceivedEventArgs(new List<DialogSendibleInfo>() { dialogCreatedMessage.Info}));
                                break;
                            }
                        case MessageType.CreateDialogResponceMessage:
                            {
                                CreateDialogResponceMessage responce = newMessage as CreateDialogResponceMessage;
                                UpdateDialogEncryptionKeys(responce.DialogId, _currentUserId);
                                break;
                            }
                        case MessageType.EndStreamMessage:
                            {
                                Close();
                                break;
                            }
                    }
                    //if(_messageList.Count>0)
                    //{
                    //    _messageWriter.WriteMessage(_messageList.First());
                    //}

                } while (!token.IsCancellationRequested);
                _messageReder.Close();
            });
        }

        public void ConnectToServer()
        {
            _client.Connect(_serverIP, _serverPort);
            _messageWriter = new MessageWriter(_client.GetStream());
            _messageWriter.WriteMessage(new StartStreamMessage("123"));
            _messageReder = new MessageReader(_client.GetStream());
            EncryptMessanger.dll.Messages.Message newMessage = _messageReder.ReadNext();
            while (newMessage.Type != MessageType.StartStreamMessage)
            {

            }
            EncryptionProvider encryptionProvider = new EncryptionProvider();
            encryptionProvider.ClientServerEncrypt(_messageWriter, _messageReder);
        }
        private void OnResiveMessages(long messageId, string message, long dialogId, long from, DateTime sendDate, bool isAltered, string error = "")
        {
            NewMessageEventArgs args = new NewMessageEventArgs(messageId, message, dialogId, from, sendDate, isAltered);
            if (!String.IsNullOrEmpty(error))
            {
                args.Error = error;
            }
            Resive?.Invoke(this, args);
        }
        private void OnResiveMessages(long messageId, string message, long dialogId, long from, DateTime sendDate, bool isAltered, long attachId, string error = "")
        {
            NewMessageEventArgs args = new NewMessageEventArgs(messageId, message, dialogId, from, sendDate, isAltered, attachId);
            if(!String.IsNullOrEmpty(error))
            {
                args.Error = error;
            }
            Resive?.Invoke(this, args);
        }
        public void SendMessage(string message, long dialogId)
        {
            if (!isLocked)
            {
                ClientClientEncryptedSession session = FindSession(dialogId);
                //сессия найдена
                if (session != null)
                {
                    //_messageList.Add(new TextMessage(_currentUserLogin, to, session.Encrypt(Encoding.UTF8.GetBytes(message))));
                    //TextMessage newTextMessage = new TextMessage(_currentUserLogin, to, session.Encrypt(Encoding.UTF8.GetBytes(message)));
                    //newTextMessage.AddSignature(session.CreateSign(newTextMessage.byteText));
                    TextMessage newTextMessage = new TextMessage(_currentUserId, dialogId, message);
                    session.TransformMessage(newTextMessage);
                    _messageWriter.WriteMessage(newTextMessage);
                }
                else
                {
                    //блокировать отправку сообщений на время установления сессии(сообщения помещаютс в очередь _messageQueue)
                    LockClient();
                    _messageQueue.Enqueue(new TextMessage(_currentUserId, dialogId, message));
                    _messageWriter.WriteMessage(new CreateCryptoSessionRequest(dialogId, _currentUserId, GetClientIp().GetAddressBytes(), _clientClientEncryptionPort));
                    //WriteClientClientMesssage(new TextMessage(_currentUserLogin, to, message));
                    //_messageList.Add(new CreateCryptoSessionRequest(to, _currentUserLogin));
                    //EncryptionProvider enc = new EncryptionProvider();
                    //ClientClientEncryptedSession newSession = enc.ClientClientSenderEncrypt(_messageWriter, _messageReder, _currentUserLogin, to);
                    //_sessions.Add(newSession);
                    //_messageWriter.WriteMessage(new TextMessage(_currentUserLogin, to, newSession.Encrypt(Encoding.UTF8.GetBytes(message))));

                }
            }
            else
            {
                _messageQueue.Enqueue(new TextMessage(_currentUserId, dialogId, message));
            }

        }
        //private ClientClientEncryptedSession FindSession(string dialogId)
        //{
        //    ClientClientEncryptedSession session = (from Session in _sessions
        //    where Session.dialogId.Equals(dialogId)
        //    select Session).Single();
        //    return session;
        //}
        //private void WriteClientClientMesssage(TextMessage message)
        //{
        //    if (isLocked)
        //    {                
        //        _messageQueue.Enqueue(message);
        //        return;
        //    }
        //    _messageWriter.WriteMessage(message);
        //}
        private string GetServerIp()
        {
            String strHostName = Dns.GetHostName();
            IPHostEntry iphostentry = Dns.GetHostEntry(strHostName);
            IPAddress[] ipv4Adreses = iphostentry.AddressList.Where(x=>x.AddressFamily == AddressFamily.InterNetwork).ToArray();


            return ipv4Adreses[ipv4Adreses.Length - 1].ToString();
        }
        private IPAddress GetClientIp()
        {
            String strHostName = Dns.GetHostName();
            IPHostEntry iphostentry = Dns.GetHostEntry(strHostName);
            IPAddress[] ipv4Adreses = iphostentry.AddressList.Where(x => x.AddressFamily == AddressFamily.InterNetwork).ToArray();


            return ipv4Adreses[ipv4Adreses.Length - 1];
        }
        private int GetFreePort(int startIndex, int endIndex)
        {
            
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] tcpEndPoints = properties.GetActiveTcpListeners();
            
            List<int> usedPorts = tcpEndPoints.Select(p => p.Port).ToList<int>();
            int unusedPort = 0;

            for (int port = startIndex; port < endIndex; port++)
            {
                if (!usedPorts.Contains(port))
                {
                    unusedPort = port;
                    break;
                }
            }
            return unusedPort;
        }
        private void LockClient()
        {
            isLocked = true;
        }
        private void ReleaseClient()
        {
            isLocked = false;
            for (int i = 0; i < _messageQueue.Count; i++)
            {
                // _messageWriter.WriteMessage(_messageQueue.Dequeue());
                TextMessage message = _messageQueue.Dequeue();
                SendMessage(message.Text, message.Dialog);
            }

        }
        private async void CreateSessionAsSender(CreateCryptoSessionResponse response)
        {
            if (response.Response)
            {
                EncryptionProvider enc = new EncryptionProvider();
                try
                {
                    ClientClientEncryptedSession newSession = await enc.ClientClientSenderEncryptAsync(new IPEndPoint(GetClientIp(), _clientClientEncryptionPort), response.Dialog, _currentUserId);
                    SessionAddOrReplase(newSession);
                    _messageWriter.WriteMessage(new DeleteMessagesRequestMessage(response.Dialog, response.To));
                    DialogSessionSuccess?.Invoke(this, new DialogSessionSuccessEventArgs(response.Dialog));
                    SessionUpdated?.Invoke(this, new DialogSessionSuccessEventArgs(response.Dialog));

                }
                catch (Exception ex)
                {
                    DialogSessionFaild?.Invoke(this, new DialogSessionFaildEventArgs(response.Dialog, "Не удалось обновить ключи шифрования"));
                }

            }
            ReleaseClient();
        }
        private async void CreateSessionAsReceiver(CreateCryptoSessionRequest request)
        {
            _messageWriter.WriteMessage(new CreateCryptoSessionResponse(request.Dialog, request.From, _currentUserId, true));
            try
            {
                EncryptionProvider enc = new EncryptionProvider();
                ClientClientEncryptedSession newSession = await enc.ClientClientResiverEncryptAsync(new IPEndPoint(request.Ip, request.Port), _currentUserId);
                SessionAddOrReplase(newSession);
                _messageWriter.WriteMessage(new DeleteMessagesRequestMessage(request.Dialog, _currentUserId));
                DialogSessionSuccess?.Invoke(this, new DialogSessionSuccessEventArgs(request.Dialog));
                SessionUpdated?.Invoke(this, new DialogSessionSuccessEventArgs(request.Dialog));
            }
            catch (Exception ex)
            {
                DialogSessionFaild?.Invoke(this, new DialogSessionFaildEventArgs(request.Dialog, "Не удалось обновить ключи шифрования"));
            }
        }
        //public void Prepare()
        //{
        //    _messageReder = new MessageReader(_client.GetStream());

        //}
        public void RequesForDialog( int dialogsCount, int dialogsOffset)
        {
            DialogsRequestMessage dialogsRequest = new DialogsRequestMessage( dialogsCount, dialogsOffset);
            _messageWriter.WriteMessage(dialogsRequest);
        }
        public bool Auth(string login, string password)
        {
            Authentificator auth = new Authentificator();
            if (auth.ClientAuth(_messageWriter, _messageReder, login, password))
            {
                _currentUserLogin = login;
                _currentUserId = auth.ClientId;
                _currentUserPassword = password;
                AuthSuccess?.Invoke(this, new ClientAuthSuccessEventArgs(auth.ClientId, auth.Login));
                return true;
            }
            AuthError?.Invoke(this, new AuthErrorEventArgs(auth.CurrentError));
            return false;

        }
        public void ExportKeys(long dialog, string fileName)
        {
            ClientClientEncryptedSession session = FindSession(dialog);
            if (session != null)
            {
                session.ExportKeys(fileName, _currentUserId);
            }
        }
        public void ImportKeys(long dialogId, string fileName)
        { 
            ClientClientEncryptedSession session = FindSession(dialogId);
            if (session == null)
            {
                session = ClientClientEncryptedSession.EmptySession(dialogId);
            }
            //Боросает ArgumentException если для данной сессии ключи не подходят
            session.ImportKeys(fileName, _currentUserId);
            SessionAddOrReplase(session);
            DialogSessionSuccess.Invoke(this, new DialogSessionSuccessEventArgs(dialogId));

        }
        private void SetSessionEncrSettings(long dialog, bool useSign, bool useEncrypt)
        {
            ClientClientEncryptedSession session = FindSession(dialog);
            session.UseEncryption = useEncrypt;
            session.UseSignature = useSign;
        }


        public void UpdateDialogEncryptionKeys(long dialogId, long userId)
        {
            _messageWriter.WriteMessage( new CreateCryptoSessionRequest(dialogId, userId, GetClientIp().GetAddressBytes(), _clientClientEncryptionPort));
        }
        public void ChangeEncryptionSettings(long dialog, bool useSign, bool useEncrypt)
        {
            //SetSessionEncrSettings( dialog, useSign, useEncrypt);
            _messageWriter.WriteMessage(new DialogEncryptionSettingsMessage(dialog, _currentUserId,useSign,useEncrypt));
        }
        public bool RegistrateAntAuth(RegistrationInfo registrationInfo)
        {
            string login = registrationInfo.Login;
            string password = registrationInfo.Password;

            ClientRegistrator registrator = new ClientRegistrator();
            if(registrator.Registrate(_messageWriter, _messageReder, login, password))
            {
                RegistrationSuccess?.Invoke(this, new RegistrationSuccessEventArgs(login, registrator.UserId));
                _currentUserId = registrator.UserId;
                _currentUserLogin = login;
                return true;
            }
            else
            {
                RegistrationError?.Invoke(this, new RegistrationErrorEventArgs(registrator.LastError));
                return false;
            }
        }
        public void LoadSession(long dialogId)
        {
            //SessionIO io = new SessionIO();
            try
            {
                _sessionManager.LoadSession(dialogId);
                //SessionAddOrReplase(io.LoadSession(dialogId, _sessionSaveFile));
            }
            catch(Exception)
            {
                DialogSessionFaild?.Invoke(this, new DialogSessionFaildEventArgs(dialogId, "Не удалось загрузить ключи шифрования для данного диалога"));
            }            
        }
        public void LoadSessionAndSetEncryptionSettings(long dialogId, bool encryptMessages, bool signMessages)
        {
            try
            {
                _sessionManager.LoadSession(dialogId);
                SetSessionEncrSettings(dialogId, signMessages, encryptMessages);
                //SessionAddOrReplase(io.LoadSession(dialogId, _sessionSaveFile));
            }
            catch (Exception)
            {
                DialogSessionFaild?.Invoke(this, new DialogSessionFaildEventArgs(dialogId, "Не удалось загрузить ключи шифрования для данного диалога"));
            }
        }
        public void RequestDialogMessages(long dialogId, int count, int offset)
        {
            _messageWriter.WriteMessage(new DialogMessagesRequestMessage(dialogId, count, offset));
        }
        public void RequestUserInfo(long userId)
        {
            _messageWriter.WriteMessage(new UserInfoRequestMessage(userId));
        }
        public void SendFile(long dialogId, long senderId, string path, string name, UpdateProgressBarDelegate updateDelegate = null)
        {
            IPAddress clientAdress = GetClientIp();
            IPEndPoint point = new IPEndPoint(clientAdress, GetFreePort(_fileSendPortStartIndex, _fileSendPortEndIndex));
            FileSender fileSender = new FileSender();
            if(updateDelegate != null)
            {
                fileSender.UpdateProgressDelegate = updateDelegate;
            }
            ClientClientEncryptedSession session = FindSession(dialogId);
            fileSender.SendFileToServerAsync(path, session, point);
            _messageWriter.WriteMessage(new SendFileRequest(clientAdress.GetAddressBytes(), point.Port, senderId, dialogId, session.Encrypt(Encoding.UTF8.GetBytes(name))));
        }
        public void ReceiveFile(long dialogId, long attachId, string saveFileName)
        {
            IPAddress clientAdress = GetClientIp();
            IPEndPoint point = new IPEndPoint(clientAdress, GetFreePort(_fileReceivePortStartIndex, _fileReceivePortEndIndex));
            FileReceiver receiver = new FileReceiver();

            ClientClientEncryptedSession session = FindSession(dialogId);
            receiver.ReceiveFileForClientAsync(saveFileName, point, session);
            _messageWriter.WriteMessage(new ReceiveFileRequest(attachId, clientAdress.GetAddressBytes(), point.Port));
        }
        public void SearchForUser(string login, int offcet, int count)
        {
            _messageWriter.WriteMessage(new SearchUserRequestMessage(login, offcet, count));
        }
        public void CreateDialog(long creatorId, long[] membersId, string dialogName)
        {
            _messageWriter.WriteMessage(new CreateDialogRequestMessage(creatorId, membersId, dialogName));
        }
        public void DeleteMessage(long messageId)
        {
            _messageWriter.WriteMessage(new DeleteMessageRequest(messageId));
        }
        public void Stop()
        {
            _messageWriter.WriteMessage(new EndStreamMessage());

        }
        public void Close()
        {
            _messageWriter.Close();
            _client.Close();
            _cancellationTokenSource.Cancel();
        }
        
    }
}
