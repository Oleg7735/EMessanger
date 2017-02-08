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
            set { setAtributeValue(new MessageAtribute(Atribute.Text, value)); }
            get { return GetAttribute(Atribute.Text); }
        }
        public string Text
        {
            set { setAtributeValue(new MessageAtribute(Atribute.Text, Encoding.UTF8.GetBytes(value))); }
            get { return Encoding.UTF8.GetString(GetAttribute(Atribute.Text)); }
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
        public TextMessage(string from, string to, string text)
        {
            init();
            setAtributeValue(new MessageAtribute(Atribute.To, Encoding.UTF8.GetBytes(to)));
            setAtributeValue(new MessageAtribute(Atribute.From, Encoding.UTF8.GetBytes(from)));
            //_atributes.Add(new MessageAtribute(Atribute.To, Encoding.UTF8.GetBytes(to)));
            //_atributes.Add(new MessageAtribute(Atribute.From, Encoding.UTF8.GetBytes(from)));
            _atributes.Add(new MessageAtribute(Atribute.Text, Encoding.UTF8.GetBytes(text)));
        }
        public TextMessage(string from, string to, byte[] text)
        {
            init();
            //_atributes.Add(new MessageAtribute(Atribute.To, Encoding.UTF8.GetBytes(to)));
            //_atributes.Add(new MessageAtribute(Atribute.From, Encoding.UTF8.GetBytes(from)));
            setAtributeValue(new MessageAtribute(Atribute.To, Encoding.UTF8.GetBytes(to)));
            setAtributeValue(new MessageAtribute(Atribute.From, Encoding.UTF8.GetBytes(from)));
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
            return "Text message from " + From + " to " + To + " text = " + Text + " Signature = " + Encoding.UTF8.GetString(GetSignature());
        }
    }
}
