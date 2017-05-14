using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessangerClient.Events
{
    public class ErrorMessageEventArgs:EventArgs
    {
        private string _error;

        public ErrorMessageEventArgs(string error)
        {
            Error = error;
        }

        public string Error
        {
            get
            {
                return _error;
            }

            set
            {
                _error = value;
            }
        }
    }
}
