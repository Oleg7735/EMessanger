using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll
{
    public class MessageAtribute
    {
        private Atribute _name;

        private byte[] _value;

        public Atribute Name
        {
            get { return _name; }
        }
        public byte[] Value
        {
            get { return _value; }
            set { _value = value; }
        }
        public MessageAtribute(Atribute name, byte[] value)
        {
            _name = name;
            _value = value;
        }
        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            if (object.ReferenceEquals(this, other))
                return true;
            if (this.GetType() != other.GetType())
                return false;

            return this.Equals(other as MessageAtribute);
        }
        public bool Equals(MessageAtribute other)
        {
            if (other == null)
                return false;

            if (object.ReferenceEquals(this, other))
                return true;

            if (this.GetType() != other.GetType())
                return false;
            if (Name.Equals(other.Name) && Value.Equals(other.Value))
                return true;
            else
                return false;
        }
    }
}
