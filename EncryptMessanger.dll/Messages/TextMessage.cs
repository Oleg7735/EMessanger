using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.Messages
{
    public class TextMessage : ResendibleMessage
    {
        private const string _textName = "text";

        /*public string To
        {
            set { setAtributeValue(new MessageAtribute(Atribute.To, Encoding.UTF8.GetBytes(value))); }
            get { return Encoding.UTF8.GetString(GetAttribute(Atribute.To)); }
        }
        public string From
        {
            set { setAtributeValue(new MessageAtribute(Atribute.From, Encoding.UTF8.GetBytes(value))); }
            get { return Encoding.UTF8.GetString(GetAttribute(Atribute.From)); }
        }*/

        /*public long To
        {            
            get { return BitConverter.ToInt64(GetAttribute(Atribute.To),0); }
        }
        public long From
        {            
            get { return BitConverter.ToInt64(GetAttribute(Atribute.From),0); }
        }*/
        public byte[] byteText
        {
            set { SetAtributeValue(new MessageAtribute(Atribute.Text, value)); }
            get { return GetAttribute(Atribute.Text); }
        }
        public string Text
        {
            set { SetAtributeValue(new MessageAtribute(Atribute.Text, Encoding.UTF8.GetBytes(value))); }
            get { return Encoding.UTF8.GetString(GetAttribute(Atribute.Text)); }
        }
        public DateTime SendDate
        {
            set { SetAtributeValue(new MessageAtribute(Atribute.DateTime, BitConverter.GetBytes(value.ToBinary()))); }
            get { return DateTime.FromBinary(BitConverter.ToInt64(GetAttribute(Atribute.DateTime), 0)); }
        }
        private void init()
        {
            _type = MessageType.TextMessage;
            _tag = "message";
        }
        public TextMessage()
        {
            init();
        }
        public TextMessage(long from, long dialog, string text)
        {
            init();
            SetAtributeValue(new MessageAtribute(Atribute.DialogId, BitConverter.GetBytes(dialog)));
            SetAtributeValue(new MessageAtribute(Atribute.From, BitConverter.GetBytes(from)));
            //_atributes.Add(new MessageAtribute(Atribute.To, Encoding.UTF8.GetBytes(to)));
            //_atributes.Add(new MessageAtribute(Atribute.From, Encoding.UTF8.GetBytes(from)));
            _atributes.Add(new MessageAtribute(Atribute.Text, Encoding.UTF8.GetBytes(text)));
        }
        public TextMessage(string from, string dialog, byte[] text)
        {
            init();
            //_atributes.Add(new MessageAtribute(Atribute.To, Encoding.UTF8.GetBytes(to)));
            //_atributes.Add(new MessageAtribute(Atribute.From, Encoding.UTF8.GetBytes(from)));
            SetAtributeValue(new MessageAtribute(Atribute.DialogId, Encoding.UTF8.GetBytes(dialog)));
            SetAtributeValue(new MessageAtribute(Atribute.From, Encoding.UTF8.GetBytes(from)));
            _atributes.Add(new MessageAtribute(Atribute.Text, text));
        }
        public void AddSignature(byte[] signature)
        {
            _atributes.Add(new MessageAtribute(Atribute.Signature,signature));
        }
        public byte[] GetSignature()
        {
            try
            {
                return GetAttribute(Atribute.Signature);
            }
            catch(Exception ex)
            {
                return new byte[2];
            }
            
        }
        public override string ToString()
        {
            return "Text message from user " + From + " to dalog" + Dialog + " text = " + Text + " Signature = " + Encoding.UTF8.GetString(GetSignature());
        }
    }
}
