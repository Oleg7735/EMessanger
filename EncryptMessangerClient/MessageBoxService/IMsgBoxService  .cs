using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessangerClient.MessageBoxService
{
    public interface IMsgBoxService
    {
        void ShowNotification(string message);
        bool ShowQuestion(string message);
        string ShowSaveFileWindow(string message);
        string ShowOpenFileWindow();
    }
}
