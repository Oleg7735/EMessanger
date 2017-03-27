using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessangerClient.Events
{
    public class RegistrationErrorEventArgs:EventArgs
    {
        private string errorDescription;

        public string ErrorDescription
        {
            get
            {
                return errorDescription;
            }

            set
            {
                errorDescription = value;
            }
        }
        public RegistrationErrorEventArgs(string error)
        {
            ErrorDescription = error;
        }
    }
}
