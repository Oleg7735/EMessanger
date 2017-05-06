using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using EncryptMessangerClient.Model;
using Microsoft.Win32;
using System.IO;

namespace EncryptMessangerClient.MessageBoxService
{
    public class MsgBoxService:IMsgBoxService
    {
        public Attachment ShowAttachmentOpenDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                FileInfo fileInfo = new FileInfo(openFileDialog.FileName);
                return new Attachment( fileInfo.Name, (long)(fileInfo.Length / 1024), fileInfo.FullName);

            }
            else
            {
                return null;
            }
        }

        public void ShowNotification(string message)
        {            
            MessageBox.Show(message, "Notification", MessageBoxButton.OK);
        }
        /// <summary>
        /// Показывает пользователю диалог открытия файла. 
        /// </summary>
        /// <returns>Возвращает путь к файлу, если файл был выбран. Если файл не выбран возвращает null.</returns>
        public string ShowOpenFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
                
            }
            else
            {
                return null;
            }
            
        }

        public bool ShowQuestion(string message)
        {
            MessageBoxResult result = MessageBox.Show(message, "Are you sure?", MessageBoxButton.YesNo);
            return result.HasFlag(MessageBoxResult.Yes);
        }
        /// <summary>
        /// Показывает пользователю диалог сохранения файла. 
        /// </summary>
        /// <param name="name">имя сохраняемого файла</param>
        /// <returns>Возвращает путь к файлу, если файл был выбран. Если файл не выбран возвращает null.</returns>
        public string ShowSaveFileDialog(string name)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = name;            
            if (saveFileDialog.ShowDialog() == true)
            {
                return saveFileDialog.FileName;
            }
            else
            {
                return null;
            }

        }
        public Attachment ShowSaveAttachDialog(string name)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = name;
            if (saveFileDialog.ShowDialog() == true)
            {
                FileInfo fileInfo = new FileInfo(saveFileDialog.FileName);
                return new Attachment(fileInfo.Name, (long)(fileInfo.Length / 1024), fileInfo.FullName);

            }
            else
            {
                return null;
            }

        }
    }
}
