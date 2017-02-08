using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessangerClient.Events
{
    public class ExportKeysEventArgs
    {
        private string _fileName;
        private string _dialog;
        
        public string FileName
        {
            get { return _fileName; }
        }
        public string Dialog
        {
            get { return _dialog; }
        }
        public ExportKeysEventArgs(string fileName, string dialog)
        {
            _fileName = fileName;
            _dialog = dialog;
        }
    }
}
