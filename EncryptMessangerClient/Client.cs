using System;
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
using EncryptMessanger.dll.Encription;
using EncryptMessanger.dll.Authentification;
using EncryptMessangerClient.Events;
using EncryptMessangerClient.Model;
using EncryptMessanger.dll.SendibleData;

namespace EncryptMessangerClient
{


    public class Client
    {
        private string _serverIP;// = "192.168.0.100";
        private int _serverPort = 11000;
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
        //public EventHandler<EncryptionSettingsEventArgs> RegisteationSuccess;

        private List<ClientClientEncryptedSession> _sessions = new List<ClientClientEncryptedSession>();
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
        private void SessionAddOrReplase(ClientClientEncryptedSession session)
        {
            for (int i = 0; i < _sessions.Count; i++)
            {
                if (_sessions[i].Dialog == session.Dialog)
                {
                    _sessions[i] = session;
                    return;
                }
            }
            _sessions.Add(session);
        }
        private ClientClientEncryptedSession FindSession(long dialogId)
        {
            for (int i = 0; i < _sessions.Count; i++)
            {
                if (_sessions[i].Dialog == dialogId)
                {
                    return _sessions[i];
                }

            }
            return null;
        }
        public Task StartAsync()
        {
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
                                ClientOnline?.Invoke(this, new ClientStatusOnlineEventArgs(clientOnline.Users));
                                break;
                            }
                        case MessageType.ClientExitMessage:
                            {
                                ClientExitMessage clientExit = newMessage as ClientExitMessage;
                                ClientExit?.Invoke(this, new ClientStatusExitEventArgs(clientExit.User));
                                break;
                            }
                        case MessageType.CreateCryptoSessionRequest:
                            {
                                CreateCryptoSessionRequest request = newMessage as CreateCryptoSessionRequest;
                                _messageWriter.WriteMessage(new CreateCryptoSessionResponse(request.Dialog, request.From, _currentUserId, true));
                                break;
                            }
                        case MessageType.CreateCryptoSessionResponse:
                            {
                                CreateCryptoSessionResponse response = newMessage as CreateCryptoSessionResponse;
                                if (response.Response)
                                {
                                    EncryptionProvider enc = new EncryptionProvider();
                                    ClientClientEncryptedSession newSession = enc.ClientClientSenderEncrypt(_messageWriter, _messageReder, response.Dialog, _currentUserId);
                                    _sessions.Add(newSession);
                                }
                                ReleaseClient();
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
                                EncryptionProvider enc = new EncryptionProvider();
                                ClientClientEncryptedSession newSession = enc.ClientClientResiverEncrypt(_messageWriter, _messageReder, _currentUserId, keyMessage);
                                SessionAddOrReplase(newSession);
                                break;
                            }
                        case MessageType.TextMessage:
                            {
                                TextMessage newTextMessage = newMessage as TextMessage;
                                ClientClientEncryptedSession session = FindSession(newTextMessage.From);
                                if (session == null)
                                {
                                    throw new Exception("Невозможно расшифровать входящее сообщение. Сессия не найдена.");
                                }
                                OnResiveMessages(Encoding.UTF8.GetString(session.Dectypt(newTextMessage.byteText)), newTextMessage.From, !session.VerifyData(newTextMessage.byteText, newTextMessage.GetSignature()));
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
                                EncryptionSettingsChanged?.Invoke(this, new EncryptionSettingsEventArgs(settings.UseSign, settings.UseEncrypt));
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

                                UserInfoReceived?.Invoke(this, new DialogUserInfoReceivedEventArgs(userInfoResponce.UserId, userInfoResponce.Login));
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
        }
        private void OnResiveMessages(string message, long from, bool isAltered)
        {
            Resive?.Invoke(this, new NewMessageEventArgs(message, from, isAltered));
        }
        public void SendMessage(string message, long to)
        {
            if (!isLocked)
            {
                ClientClientEncryptedSession session = FindSession(to);
                //сессия найдена
                if (session != null)
                {

                    //_messageList.Add(new TextMessage(_currentUserLogin, to, session.Encrypt(Encoding.UTF8.GetBytes(message))));
                    //TextMessage newTextMessage = new TextMessage(_currentUserLogin, to, session.Encrypt(Encoding.UTF8.GetBytes(message)));
                    //newTextMessage.AddSignature(session.CreateSign(newTextMessage.byteText));
                    TextMessage newTextMessage = new TextMessage(_currentUserId, to, message);
                    session.TransformMessage(newTextMessage);
                    _messageWriter.WriteMessage(newTextMessage);
                }
                else
                {
                    //блокировать отправку сообщений на время установления сессии(сообщения помещаютс в очередь _messageQueue)
                    LockClient();
                    _messageQueue.Enqueue(new TextMessage(_currentUserId, to, message));
                    _messageWriter.WriteMessage(new CreateCryptoSessionRequest(to, _currentUserId));
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
                _messageQueue.Enqueue(new TextMessage(_currentUserId, to, message));
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
                session.ExportKeys(fileName);
            }

        }
        private void SetSessionEncrSettings(long dialog, bool useSign, bool useEncrypt)
        {
            ClientClientEncryptedSession session = FindSession(dialog);
            session.UseEncryption = useEncrypt;
            session.UseSignature = useSign;
        }


        public void UpdateDialogEncryptionKeys(long dialogId, long userId)
        {
            _messageWriter.WriteMessage( new CreateCryptoSessionRequest(dialogId, userId));
        }
        public void ChangeEncryptionSettings(long dialog, bool useSign, bool useEncrypt)
        {
            SetSessionEncrSettings( dialog, useSign, useEncrypt);
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
        public void RequestUserInfo(long userId)
        {
            _messageWriter.WriteMessage(new UserInfoRequestMessage(userId));
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
