﻿using EncryptMessanger.dll.Messages.DialogCreation;
using EncryptMessanger.dll.Messages.FileMessages;
using EncryptMessanger.dll.Messages.SingleMessageDeletion;
using EncryptMessanger.dll.Messages.UserSearch;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public enum MessageType
{
    StartStreamMessage, EndStreamMessage, TextMessage, AuthMessage,
    AuthResponceMessage, AuthSuccessMessage, PublicKeyMessage, SymKeyMessage,
    ProcessedMessage, AbstractMessage, AuthErrorMessage, ClientPublicKeyMessage,
    ClientSymKeyMessage, CreateCryptoSessionRequest, CreateCryptoSessionResponse,
    ClientClientSignKeyMessage, ClientOnlineMessage, ClientExitMessage, RegistrationMessage,
    DialogEncryptionSettingsMessage, RegistrationSuccessMessage, RegistrationErrorMessage,
    DialogsRequestMessage, DialogResponceMessage, UserInfoRequestMessage, UserInfoResponceMessage, 
    DialogMessagesResponceMessage, DialogMessagesRequestMessage, FileFragmentMessage, EndFileMessage,
    SendFileRequest, ReceiveFileRequest, DeleteMessagesRequestMessage, UserSearchRequestMessage,
    SearchUserResponceMessage, CreateDialogRequestMessage, DialogCreatedMessage, CreateDialogResponceMessage,
    DeleteSingleMessageRequest, MessageDeletedMessage

};
public enum Atribute
{
    Key, To, From, Text, IV, Login, Password, Response, Signature, Clients,
    UseEncryption, UseSignature, DialogInfo, UserId, Count, Offset, DialogId, MessageInfo, DateTime, IP,
    Port, BinaryData, Name, HasAttach, AttachName, AttachId, UserState, CreatorId, MemberId, DialogName,
    ResponceState, Error, MessageId, Signed, Encrypted
};

namespace EncryptMessanger.dll.Messages
{
    public class Message:ISendibleData
    {
        protected List<MessageAtribute> _atributes = new List<MessageAtribute>();
        protected MessageType _type;
        protected string _tag;

        protected void AddAtribute(MessageAtribute messageAtribute)
        {
            _atributes.Add(messageAtribute);
        }
        protected void SetAtributeValue(MessageAtribute messageAtribute)
        {
            for (int i = 0; i < _atributes.Count; i++)
            {
                if (_atributes[i].Name == messageAtribute.Name)
                {
                    if (_atributes[i].Value != messageAtribute.Value)
                    {
                        _atributes[i].Value = messageAtribute.Value;
                    }
                    return;
                }

            }
            AddAtribute(messageAtribute);
        }
        public List<MessageAtribute> Atributes
        {
            get { return _atributes; }
        }
        public MessageType Type
        {
            get { return _type; }
        }
        public string Tag
        {
            get { return _tag; }
        }
        public Message()
        {

        }
        

        //public string StartAtributeToString()
        //{
        //    string m = "<" + Tag + " ";
        //    for (int i = 0; i < Atributes.Count; i++)
        //    {
        //        if (Atributes[i].Name != null && Atributes[i].Value != null)
        //        {
        //            m += Atributes[i].Name.ToString();
        //            m += "=\"";
        //            m += Encoding.UTF8.GetString(Atributes[i].Value);
        //            m += "\" ";
        //        }

        //    }
        //    m += ">";
        //    return m;
        //}
        //public byte[] toByte()
        //{
        //    return ;
        //}
        public string EndAtributeToString()
        {
            return "<" + "/" + Tag + ">";
        }
        //public virtual byte[] StartAtributeToByte()
        //{
        //    return Encoding.UTF8.GetBytes(StartAtributeToString());
        //}
        public virtual byte[] EndAtributeToByte()
        {
            return Encoding.UTF8.GetBytes(EndAtributeToString());
        }
        public virtual byte[] ToByte()
        {
            MemoryStream ms = new MemoryStream();
            ms.Write(BitConverter.GetBytes((int)Type), 0, 4);
            ms.Write(BitConverter.GetBytes(Atributes.Count), 0, 4);
            for (int i = 0; i < Atributes.Count; i++)
            {
                ms.Write(BitConverter.GetBytes((int)Atributes[i].Name), 0, 4);
                ms.Write(BitConverter.GetBytes((int)Atributes[i].Value.Length), 0, 4);
                ms.Write(Atributes[i].Value, 0, Atributes[i].Value.Length);
            }
            //byte[] message = new byte[ms.Length];
            //Array.Copy(ms.GetBuffer(), message, ms.Length);
            return ms.GetBuffer();

        }

        public virtual void FillFromBytes(byte[] bytes)
        {
            _atributes.Clear();
            MemoryStream ms = new MemoryStream(bytes);
            byte[] messageType = new byte[4];
            ms.Read(messageType, 0, 4);
            _type = (MessageType)Enum.ToObject(typeof(MessageType), BitConverter.ToInt32(messageType, 0));

            byte[] atributesCount = new byte[4];
            ms.Read(atributesCount, 0, 4);
            int atrCount =  BitConverter.ToInt32(atributesCount, 0);

            byte[] AtributeType = new byte[4];
            byte[] AtributeLength = new byte[4];
            int len;

            
            while (ms.Read(AtributeType, 0, 4) > 0 && atrCount>0)
            {
                ms.Read(AtributeLength, 0, 4);
                len = BitConverter.ToInt32(AtributeLength, 0);
                byte[] atributeValue = new byte[len];
                ms.Read(atributeValue, 0, len);
                MessageAtribute ma = new MessageAtribute((Atribute)Enum.ToObject(typeof(Atribute), BitConverter.ToInt32(AtributeType, 0)), atributeValue);
                _atributes.Add(ma);
                atrCount--;
            }
        }
        public void SetAtributesFromeByte(byte[] byteAtributes)
        {
            _atributes.Clear();

            MemoryStream ms = new MemoryStream(byteAtributes);
            byte[] AtributeType = new byte[4];
            byte[] AtributeLength = new byte[4];
            int len;
            while (ms.Read(AtributeType, 0, 4) > 0)
            {
                ms.Read(AtributeLength, 0, 4);
                len = BitConverter.ToInt32(AtributeLength, 0);
                byte[] atributeValue = new byte[len];
                ms.Read(atributeValue, 0, len);
                MessageAtribute ma = new MessageAtribute((Atribute)Enum.ToObject(typeof(Atribute), BitConverter.ToInt32(AtributeType, 0)), atributeValue);

            }
        }
        public byte[] GetAttribute(Atribute name)
        {
            for (int i = 0; i < _atributes.Count; i++)
            {
                if (_atributes[i].Name == name)
                {
                    return _atributes[i].Value;
                }
            }
            throw new KeyNotFoundException("Атрибут типа " + name + " не найден в данном сообщении.");
        }
        public static Message CreateMessage(byte[] bytes)
        {
            Message message;
            byte[] mType = new byte[4];
            MemoryStream ms = new MemoryStream(bytes);
            ms.Read(mType, 0, 4);
            MessageType type = (MessageType)Enum.ToObject(typeof(MessageType), BitConverter.ToInt32(mType, 0));
            switch (type)
            {
                case MessageType.AuthSuccessMessage:
                    {
                        message = new AuthSuccessMessage();
                        break;
                    }
                case MessageType.AuthMessage:
                    {
                        message = new AuthMessage();
                        break;
                    }
                case MessageType.AuthResponceMessage:
                    {
                        message = new AuthResponceMessage();
                        break;
                    }
                case MessageType.RegistrationMessage:
                    {
                        message = new RegistrationMessage();
                        break;
                    }
                //case MessageType.AuthSuccessMessage:
                //    {
                //        //message = new AuthS();
                //        break;
                //    }
                //case MessageType.ProcessedMessage:
                //    {
                //        //message = new Pro();
                //        break;
                //    }
                case MessageType.PublicKeyMessage:
                    {
                        message = new AKeyMessage();
                        break;
                    }
                case MessageType.StartStreamMessage:
                    {
                        message = new StartStreamMessage();
                        break;
                    }
                case MessageType.SymKeyMessage:
                    {
                        message = new SKeyMessage();
                        break;
                    }
                case MessageType.TextMessage:
                    {
                        message = new TextMessage();
                        break;
                    }
                case MessageType.ClientPublicKeyMessage:
                    {
                        message = new ClientAKeyMessage();
                        break;
                    }
                case MessageType.ClientSymKeyMessage:
                    {
                        message = new ClientSKeyMessage();
                        break;
                    }
                case MessageType.CreateCryptoSessionRequest:
                    {
                        message = new CreateCryptoSessionRequest();
                        break;
                    }
                case MessageType.CreateCryptoSessionResponse:
                    {                        
                        message = new CreateCryptoSessionResponse();
                        break;
                    }
                case MessageType.ClientClientSignKeyMessage:
                    {
                        message = new ClientClientSignKeyMessage();
                        break;
                    }
                case MessageType.AuthErrorMessage:
                    {
                        message = new AuthErrorMessage();
                        break;
                    }
                case MessageType.ClientOnlineMessage:
                    {
                        message = new ClientOnlineMessage();
                        break;
                    }
                case MessageType.ClientExitMessage:
                    {
                        message = new ClientExitMessage();
                        break;
                    }
                case MessageType.DialogEncryptionSettingsMessage:
                    {
                        message = new DialogEncryptionSettingsMessage();
                        break;
                    }
                case MessageType.RegistrationErrorMessage:
                    {
                        message = new RegistrationErrorMessage();
                        break;
                    }
                case MessageType.RegistrationSuccessMessage:
                    {
                        message = new RegistrationSuccessMessage();
                        break;
                    }
                case MessageType.DialogsRequestMessage:
                    {
                        message = new DialogsRequestMessage();
                        break;
                    }
                case MessageType.DialogResponceMessage:
                    {
                        message = new DialogsResponceMessage();
                        break;
                    }
                case MessageType.UserInfoRequestMessage:
                    {
                        message = new UserInfoRequestMessage();
                        break;
                    }
                case MessageType.UserInfoResponceMessage:
                    {
                        message = new UserInfoResponceMessage();
                        break;
                    }
                case MessageType.EndStreamMessage:
                    {
                        message = new EndStreamMessage();
                        break;
                    }
                case MessageType.DialogMessagesRequestMessage:
                    {
                        message = new DialogMessagesRequestMessage();
                        break;
                    }
                case MessageType.DialogMessagesResponceMessage:
                    {
                        message = new DialogMessagesResponceMessage();
                        break;
                    }
                case MessageType.FileFragmentMessage:
                    {
                        message = new FileFragmentMessage();
                        break;
                    }
                case MessageType.EndFileMessage:
                    {
                        message = new EndFileMessage();
                        break;
                    }
                case MessageType.SendFileRequest:
                    {
                        message = new SendFileRequest();
                        break;
                    }
                case MessageType.ReceiveFileRequest:
                    {
                        message = new ReceiveFileRequest();
                        break;
                    }
                case MessageType.DeleteMessagesRequestMessage:
                    {
                        message = new DeleteMessagesRequestMessage();
                        break;
                    }
                case MessageType.UserSearchRequestMessage:
                    {
                        message = new SearchUserRequestMessage();
                        break;
                    }
                case MessageType.SearchUserResponceMessage:
                    {
                        message = new SearchUserResponceMessage();
                        break;
                    }
                case MessageType.CreateDialogRequestMessage:
                    {
                        message = new CreateDialogRequestMessage();
                        break;
                    }
                case MessageType.DialogCreatedMessage:
                    {
                        message = new DialogCreatedMessage();
                        break;
                    }
                case MessageType.CreateDialogResponceMessage:
                    {
                        message = new CreateDialogResponceMessage();
                        break;
                    }
                case MessageType.DeleteSingleMessageRequest:
                    {
                        message = new DeleteMessageRequest();
                        break;
                    }
                case MessageType.MessageDeletedMessage:
                    {
                        message = new MessageDeletedMessage();
                        break;
                    }
                default:
                    {
                        throw new ArgumentException(String.Format("Не определено действие по созданию объекта сообщения для сообщения типа {0}", type.ToString()));
                        //message = new Message();
                        break;
                    }
            }
            message.FillFromBytes(bytes);
            ms.Close();
            return message;
        }
       
        public override string ToString()
        {
            string rezultString;
            rezultString = _type.ToString("g");
            foreach(MessageAtribute atribute in _atributes)
            {
                rezultString += " "+atribute.Name.ToString("g")+" "+Encoding.UTF8.GetString(atribute.Value);
            }
            return rezultString;
        }
    }
}
