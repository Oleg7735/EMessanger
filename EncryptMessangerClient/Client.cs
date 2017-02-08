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


namespace EncryptMessangerClient
{


    public class Client
    {
        private string _serverIP = "192.168.56.1";
        private int _serverPort = 11000;
        private TcpClient _client = new TcpClient();
        private MessageWriter _messageWriter;
        private MessageReader _messageReder;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        public EventHandler<NewMessageEventArgs> Resive;
        public EventHandler<AuthErrorEventArgs> AuthError;
        public EventHandler<ClientStatusOnlineEventArgs> ClientOnline;
        public EventHandler<ClientStatusExitEventArgs> ClientExit;
        public GetDialogEncryptionSettings GetDialogSettings;
        public EventHandler<EncryptionSettingsEventArgs> EncryptionSettingsChanged;
        public string Login
        {
            get { return _currentUserLogin; }
        }

        private List<ClientClientEncryptedSession> _sessions = new List<ClientClientEncryptedSession>();
        private string _currentUserLogin = "user2";
        private string _currentUserPassword = "222";
        private Queue<TextMessage> _messageQueue = new Queue<TextMessage>();
        //private List<Message> _messageList = new List<Message>();

        public Client()
        {
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
                if (_sessions[i].Interlocutor.Equals(session.Interlocutor))
                {
                    _sessions[i] = session;
                    return;
                }
            }
            _sessions.Add(session);
        }
        private ClientClientEncryptedSession FindSession(string interlocutor)
        {
            for (int i = 0; i < _sessions.Count; i++)
            {
                if (_sessions[i].Interlocutor.Equals(interlocutor))
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
                                _messageWriter.WriteMessage(new CreateCryptoSessionResponse(request.From, _currentUserLogin, true));
                                break;
                            }
                        case MessageType.CreateCryptoSessionResponse:
                            {
                                CreateCryptoSessionResponse response = newMessage as CreateCryptoSessionResponse;
                                if (response.Response)
                                {
                                    EncryptionProvider enc = new EncryptionProvider();
                                    ClientClientEncryptedSession newSession = enc.ClientClientSenderEncrypt(_messageWriter, _messageReder, _currentUserLogin, response.From);
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
                                ClientClientEncryptedSession newSession = enc.ClientClientResiverEncrypt(_messageWriter, _messageReder, _currentUserLogin, keyMessage);
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
        private void OnResiveMessages(string message, string from, bool isAltered)
        {
            Resive?.Invoke(this, new NewMessageEventArgs(message, from, isAltered));
        }
        public void SendMessage(string message, string to)
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
                    TextMessage newTextMessage = new TextMessage(_currentUserLogin, to, message);
                    session.TransformMessage(newTextMessage);
                    _messageWriter.WriteMessage(newTextMessage);
                }
                else
                {
                    //блокировать отправку сообщений на время установления сессии(сообщения помещаютс в очередь _messageQueue)
                    LockClient();
                    _messageQueue.Enqueue(new TextMessage(_currentUserLogin, to, message));
                    _messageWriter.WriteMessage(new CreateCryptoSessionRequest(to, _currentUserLogin));
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
                _messageQueue.Enqueue(new TextMessage(_currentUserLogin, to, message));
            }

        }
        //private ClientClientEncryptedSession FindSession(string interlocutor)
        //{
        //    ClientClientEncryptedSession session = (from Session in _sessions
        //    where Session.Interlocutor.Equals(interlocutor)
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
        private bool isLocked = false;
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
                SendMessage(message.Text, message.To);
            }

        }

        //public void Prepare()
        //{
        //    _messageReder = new MessageReader(_client.GetStream());

        //}
        public bool Auth(string login, string password)
        {
            Authentificator auth = new Authentificator();
            if (auth.ClientAuth(_messageWriter, _messageReder, login, password))
            {
                _currentUserLogin = login;
                _currentUserPassword = password;
                return true;
            }
            AuthError?.Invoke(this, new AuthErrorEventArgs(auth.CurrentError));
            return false;

        }
        public void ExportKeys(string dialog, string fileName)
        {
            ClientClientEncryptedSession session = FindSession(dialog);
            if (session != null)
            {
                session.ExportKeys(fileName);
            }

        }
        private void SetSessionEncrSettings(string dialog, bool useSign, bool useEncrypt)
        {
            ClientClientEncryptedSession session = FindSession(dialog);
            session.UseEncryption = useEncrypt;
            session.UseSignature = useSign;
        }
        public void ChangeEncryptionSettings(string dialog, bool useSign, bool useEncrypt)
        {
            SetSessionEncrSettings( dialog, useSign, useEncrypt);
            _messageWriter.WriteMessage(new DialogEncryptionSettingsMessage(dialog, _currentUserLogin,useSign,useEncrypt));
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
