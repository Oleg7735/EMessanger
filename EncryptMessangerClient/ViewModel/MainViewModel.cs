using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using EncryptMessangerClient.Events;
using System.Collections.ObjectModel;
using EncryptMessangerClient.Model;
using Microsoft.Win32;
using System.IO;

namespace EncryptMessangerClient.ViewModel
{
    class MainViewModel : INotifyPropertyChanged
    {
        private string _currentUserLogin;
        public string CurrentUserLogin
        {
            get { return _currentUserLogin; }
            set { _currentUserLogin = value; }
        }

        private Dialog _currentDialog ;
        public Dialog CurrentDialog
        {
            get { return _currentDialog; }
        }

        private int _dialogSelectedIndex = -1;
        public int DialogSelectedIndex
        {
            get { return _dialogSelectedIndex; }
            set
            {
                if (value < -1)
                {
                    throw new ArgumentOutOfRangeException(nameof(DialogSelectedIndex), value, "Dialog index cannot be less than 0");
                }

                if (_dialogSelectedIndex != value)
                {
                    _dialogSelectedIndex = value;
                    if(_dialogSelectedIndex==-1)
                    {
                        _currentDialog = null;
                    }
                    else
                    {
                        _currentDialog = _dialogs[_dialogSelectedIndex];
                    }
                    OnPropertyChanged();
                    OnPropertyChanged("Messages");
                    MessageSendCommand.RaiseCanExecuteChanged();
                    ExportKeysCommand.RaiseCanExecuteChanged();
                    EncryptSessionCommand.RaiseCanExecuteChanged();
                }
            }
        }
        /// <summary>
        /// список полученных сообщений выбранного диалога
        /// </summary>
        public ObservableCollection<DialogMessage> Messages
        {
            get
            {
                if (CurrentDialog != null)
                {
                    return CurrentDialog.DialogMessages;
                }
                else return null;
            }

        }
        /// <summary>
        /// метод, добавляющийновое сообщение в список полученных сообщений
        /// </summary>
        /// <param name="interlocutor">собеседник, от которого получено сообщение</param>
        /// <param name="text">текст собщения</param>
        /// <param name="isAltered">было ли сообщение изменено при передаче</param>
        public void AddMessage(string interlocutor, string text, bool isAltered)
        {
            int i =_dialogs.IndexOf(new Model.Dialog(interlocutor));
            _dialogs[i].DialogMessages.Add(new DialogMessage(interlocutor,text,isAltered));
            //OnPropertyChanged("Messages");
            
        }
        //список доступных диалогов
        private ObservableCollection<Dialog> _dialogs = new ObservableCollection<Dialog>();
        public ObservableCollection<Dialog> Dialogs
        {
            get { return _dialogs; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected virtual void OnDialogSelected(int selectedIndex)
        {
            _currentDialog = _dialogs[selectedIndex];
        }
        private string _messageBox;

        public event EventHandler<MessageSendEventArgs> MessageSend;
        public event EventHandler StopClient;
        public event EventHandler<ExportKeysEventArgs> ExportKeysEventHandler;
        public event EventHandler EditEncryptionSettings ;
        public event EventHandler<EncryptionSettingsEventArgs> EncryptionSettingsChanged;


        public Command MessageSendCommand { get; private set; }
        public Command ClientStopCommand { get; private set; }
        public Command ExportKeysCommand { get; private set; }
        public Command EncryptSessionCommand { get; private set; }

        public string MessageBox
        {
            get { return _messageBox; }
            set
            {
                if (value != null && !value.Equals(_messageBox))
                {
                    _messageBox = value;
                    OnPropertyChanged();
                    MessageSendCommand.RaiseCanExecuteChanged();
                }
            }
        }
        private bool CanSendMessage()
        {
            return CurrentDialog != null && MessageBox != "" && MessageBox!=null;
        }
        private void SendMessage()
        {
            if(CurrentDialog != null)
            {
                MessageSend?.Invoke(this, new MessageSendEventArgs(_messageBox, CurrentDialog.Login));
                CurrentDialog.DialogMessages.Add(new DialogMessage(CurrentUserLogin, _messageBox, false));
                MessageBox = "";
                
            }

        }

        private void Stop()
        {
            StopClient?.Invoke(this, EventArgs.Empty);
        }
        private bool CanStopClient()
        {
            return true;
        }

        private void ExportKeys()
        {
            SaveFileDialog fd = new SaveFileDialog();
            if(fd.ShowDialog()==true)
            {
                if (CurrentDialog != null)
                {
                    ExportKeysEventHandler?.Invoke(this, new ExportKeysEventArgs(fd.FileName, CurrentDialog.Login));
                }            
            }
        }
        private bool CanExportKeys()
        {
            return CurrentDialog != null;
        }
        private void EditEncryptionSetting()
        {
            EditEncryptionSettings?.Invoke(this, EventArgs.Empty);
        }
        private bool CanEditEncryptionSetting()
        {
            return CurrentDialog != null;

        }
        public MainViewModel()
        {
            MessageSendCommand = new Command(SendMessage, CanSendMessage);
            ClientStopCommand = new Command(Stop, CanStopClient);
            ExportKeysCommand = new Command(ExportKeys, CanExportKeys);
            EncryptSessionCommand = new Command(EditEncryptionSetting, CanEditEncryptionSetting);
            //_dialogs.Add(new Model.Dialog("user3"));
        }
        /// <summary>
        /// метод для получения параметров защиты диалога с заданным пользователем 
        /// </summary>
        /// <param name="login">логин пользователя, с которым происходит диалог </param>
        /// <returns>EncryptionSettingsEventArgs, содержащий параметры шифрования и электронной подписи диалога</returns>
        public EncryptionSettingsEventArgs GetDialogEncryptionSettings(string login)
        {
            lock (_dialogs)
            {
                foreach (Dialog dialog in _dialogs)
                {
                    if(dialog.Login.Equals(login))
                    {
                        return new EncryptionSettingsEventArgs(dialog.Sign, dialog.Encrypt);
                    }
                }
            }
            throw new NullReferenceException("Диалог "+ login+" не найден в списке диалогов");
        }
        public void SetDialogSignSetting(bool isSign)
        {
            _currentDialog.Sign = isSign;
            OnPropertyChanged("Sign");
        }
        public void SetDialogEncryptSetting(bool isEncrypt)
        {
            _currentDialog.Encrypt = isEncrypt;
            OnPropertyChanged("Encrypt");
        }
        /// <summary>
        ///  используется ли электронная цифровая подпись для выбранного пользователем диалога
        /// </summary>
        public bool Sign
        {
            get
            {
                if (CurrentDialog != null)
                {
                    return CurrentDialog.Sign;
                }
                else
                {
                    return true;
                }
                
            }
            set
            {
                if (value != CurrentDialog.Sign)
                {
                    CurrentDialog.Sign = value;
                    OnPropertyChanged();
                    EncryptionSettingsChanged?.Invoke(this, new EncryptionSettingsEventArgs(CurrentDialog.Sign, CurrentDialog.Encrypt, CurrentDialog.Login));
                    //SignSettingChanged?.Invoke(this, new EncryptionSettingsEventArgs(_sign, _encrypt));
                }
            }
        }
        /// <summary>
        /// Используется ли шифрование для выбранного пользователем диалога
        /// </summary>
        public bool Encrypt
        {
            get
            {
                if (CurrentDialog != null)
                {
                    return CurrentDialog.Encrypt;
                }
                else
                {
                    //throw new Exception("Current dialog not found");
                    return true;
                }
            }
            set
            {
                if (value != CurrentDialog.Encrypt)
                {
                    CurrentDialog.Encrypt = value;
                    OnPropertyChanged();
                    EncryptionSettingsChanged?.Invoke(this, new EncryptionSettingsEventArgs( CurrentDialog.Sign, CurrentDialog.Encrypt, CurrentDialog.Login));

                    //EncryptSettingChanged?.Invoke(this, new EncryptionSettingsEventArgs(_sign, _encrypt));
                }
            }
        }
    }
}
