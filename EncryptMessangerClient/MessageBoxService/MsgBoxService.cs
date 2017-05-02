using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EncryptMessangerClient.MessageBoxService
{
    public class MsgBoxService:IMsgBoxService
    {
        public void ShowNotification(string message)
        {            
            MessageBox.Show(message, "Notification", MessageBoxButton.OK);
        }

        public string ShowOpenFileWindow()
        {
            throw new NotImplementedException();
        }

        public bool ShowQuestion(string message)
        {
            MessageBoxResult result = MessageBox.Show(message, "Are you sure?", MessageBoxButton.YesNo);
            return result.HasFlag(MessageBoxResult.Yes);
        }

        public string ShowSaveFileWindow(string message)
        {
            throw new NotImplementedException();
        }
    }
}
