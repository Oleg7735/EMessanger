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
using EncryptMessangerClient.extensions;
using EncryptMessangerClient.MessageBoxService;
using EncryptMessanger.dll.FileTransfer;
using EncryptMessangerClient.Commands;

namespace EncryptMessangerClient.ViewModel
{
    class MainViewModel : INotifyPropertyChanged
    {
        private const int _dialogsRequestCount = 20;
        private UserInfo _currentUser;
        private List<UserInfo> _contacts = new List<UserInfo>();
        private IMsgBoxService _messageService;
        private ObservableCollection<FileSendProgress> _fileSendProgresses = new ObservableCollection<FileSendProgress>();
        //private List<Attachment> _newMessageAttachments = new List<Attachment>();
        //private string _currentUserLogin;

        public string CurrentUserLogin
        {
            get { return _currentUser.Login; }
            set
            {
                if (!value.Equals(_currentUser.Login) && !String.IsNullOrWhiteSpace(value))
                {
                    _currentUser.Login = value;
                }
            }
        }
        public long CurrentUserId
        {
            get { return _currentUser.Id; }
            set
            {
                if (CurrentUserId != value && value >= 0)
                {
                    _currentUser.Id = value;
                }
            }
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
                        UserInfo author;
                        foreach (long id in _currentDialog.MembersId)
                        {
                            author = _contacts.Find(x => x.Id == id);
                            if (author == null)
                            {
                                RequestUserInfo(id);
                            }
                            else
                            {
                                _currentDialog.BindMessagesToAuthor(author);
                            }
                        }
                        if (!_currentDialog.SessionError)
                        {
                            LoadDialogMessages?.Invoke(this, new LoadDialogMessagesEventArgs(_currentDialog.DialogId, 20, 0));
                        }
                        else
                        {
                            _messageService.ShowNotification("Не удалось расшифровать сообщения для данного диалога. Ключи шифрования не найдены.");
                        }
                    }
                    OnPropertyChanged();
                    OnPropertyChanged("Messages");
                    MessageSendCommand.RaiseCanExecuteChanged();
                    ExportKeysCommand.RaiseCanExecuteChanged();
                    ImportKeysCommand.RaiseCanExecuteChanged();
                    EncryptSessionCommand.RaiseCanExecuteChanged();
                    UpdateDialogEncryptionKeysCommand.RaiseCanExecuteChanged();
                    SendFileCommand.RaiseCanExecuteChanged();
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
        public void AddMessages(DialogMessage[] messages, long dialogId)
        {            
            
            Dialog containerDialog = _dialogs.GetById(dialogId);
            UserInfo authorInfo;
            foreach (DialogMessage message in messages)
            {
                authorInfo = _contacts.Find(x => x.Id == message.AuthorInfo.Id);
                message.AuthorInfo = authorInfo;
                message.LoadFileCommand = LoadFileCommand;
                containerDialog.DialogMessages.Add(message);
            }
            containerDialog.SortMessages();
            OnPropertyChanged("Messages");
        }
        public void AddMessage(long interlocutor, long dialog, DateTime sendDate, string text, bool isAltered)
        {
            //int i =_dialogs.IndexOf(new Model.Dialog(interlocutor));
            if(CurrentDialog == null)
            {
                return;
            }
            //выводим принятые сообщения только для выделенного диалога, для остальных все равно подгрузка при выделении
            if (dialog != CurrentDialog.DialogId)
            {
                return;
            }
            UserInfo authorInfo = _contacts.Find(x => x.Id == interlocutor);
            try
            {
                Dialog containerDialog = _dialogs.GetById(dialog);
            
            if (authorInfo == null)
            {
                //RequestUserInfo(interlocutor);
                
                containerDialog.DialogMessages.Add(new DialogMessage(new UserInfo(interlocutor), text, sendDate, isAltered, LoadFileCommand));
                
            }
            else
            {
                containerDialog.DialogMessages.Add(new DialogMessage(authorInfo, text, sendDate, isAltered, LoadFileCommand));
                
            }
                //OnPropertyChanged("Messages");
            }
            catch (ArgumentException)
            {

            }
        }
        private void RequestUserInfo(long userId)
        {
            LoadDialogUserInfo?.Invoke(this, new LoadDialogUserInfoEventArgs(userId));
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
        public event EventHandler<ImportKeysEventArgs> ImportKeysEventHandler;
        public event EventHandler EditEncryptionSettings ;
        public event EventHandler<EncryptionSettingsEventArgs> EncryptionSettingsChanged;
        public event EventHandler<DialogsRequestEventArgs> DialogsRequest;
        public event EventHandler<UpdateDialogEncryptionKeysEventArgs> UpdateDialogKeys;
        public event EventHandler<LoadDialogUserInfoEventArgs> LoadDialogUserInfo;
        public event EventHandler<LoadDialogMessagesEventArgs> LoadDialogMessages;
        public event EventHandler<LoadDialogSessionEventArgs> LoadDialogSession;
        public event EventHandler<SendFileEventArgs> FileSend;
        public event EventHandler<LoadFileEventArgs> FileLoad;
        public event EventHandler<DeleteProgressEventArgs> DeleteProgress;

        public Command MessageSendCommand { get; private set; }
        public Command MessageUpdateKaysCommand { get; private set; }
        public Command ClientStopCommand { get; private set; }
        public Command ExportKeysCommand { get; private set; }
        public Command ImportKeysCommand { get; private set; }
        public Command EncryptSessionCommand { get; private set; }
        public Command UpdateDialogEncryptionKeysCommand { get; private set; }
        public Command SendFileCommand { get; private set; }
        public CommandWithParametr LoadFileCommand { get; private set; }

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
        private void DeleteFileSendProgress(FileSendProgress progress)
        {
            DeleteProgress?.Invoke(this, new DeleteProgressEventArgs(progress));
        }
        private void LoadFile(object loadFileCommandParams)
        {
            LoadFileCommandParams loadParams = (LoadFileCommandParams)loadFileCommandParams;
            string fileName = _messageService.ShowSaveFileDialog(loadParams.AttachName);
            FileLoad?.Invoke(this, new LoadFileEventArgs(loadParams.AttachId, CurrentDialog.DialogId,fileName));
        }
        private bool CanLoadFile()
        {
            return true;
        }
        private void SendFile()
        {
            if (CurrentDialog != null)
            {
                Attachment newAttachment = _messageService.ShowAttachmentOpenDialog();
                if (newAttachment == null)
                {
                    return;
                }
                FileSendProgress newProgress = new FileSendProgress(newAttachment.FileName, new DeleteProgressDelegate(DeleteFileSendProgress));
                _fileSendProgresses.Add(newProgress);

                FileSend?.Invoke(this, new SendFileEventArgs(newAttachment.Path, newAttachment.FileName, CurrentDialog.DialogId, CurrentUserId, new UpdateProgressBarDelegate(newProgress.SetProgress)));
                //DialogMessage newMessage = new DialogMessage(new UserInfo(CurrentUserId, CurrentUserLogin), newAttachment.FileName, DateTime.Now, false);
                //newMessage.AddAttachment(newAttachment);
                //CurrentDialog.DialogMessages.Add(newMessage);                
            }            
            //_newMessageAttachments.Add(newAttachment);

        }

        private bool CanSendFile()
        {
            return CurrentDialog != null;
        }
        //private void DeattachFile()
        //{
            
        //}
        //private bool CanDeattachFile()
        //{
        //    return _newMessageAttachments.Count != 0;
        //}
        private bool CanSendMessage()
        {
            return CurrentDialog != null && MessageBox != "" && MessageBox!=null;
        }
        private void SendMessage()
        {
            if(CurrentDialog != null)
            {
                MessageSend?.Invoke(this, new MessageSendEventArgs(_messageBox, CurrentDialog.DialogId));
                //CurrentDialog.DialogMessages.Add(new DialogMessage(new UserInfo(CurrentUserId, CurrentUserLogin), _messageBox, DateTime.Now, false));
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
                    ExportKeysEventHandler?.Invoke(this, new ExportKeysEventArgs(fd.FileName, CurrentDialog.DialogId));
                }            
            }
        }
        private bool CanExportKeys()
        {
            return CurrentDialog != null;
        }
        private void ImportKeys()
        {
            string fileName = _messageService.ShowOpenFileDialog();

            if (fileName != null)
            {
                if (CurrentDialog != null)
                {
                    ImportKeysEventHandler?.Invoke(this, new ImportKeysEventArgs(CurrentDialog.DialogId, fileName));
                }
            }
        }
        private bool CanImportKeys()
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
        private bool CanUpdateDialogEncryptionKeys()
        {
            return CurrentDialog != null;
        }
        private void UpdateDialogEncryptionKeys()
        {
            if (_messageService.ShowQuestion("При обновлении ключей старые сообщения будут недоступны. Обновить ключи?"))
            {
                UpdateDialogKeys?.Invoke(this, new UpdateDialogEncryptionKeysEventArgs(Dialogs[DialogSelectedIndex].DialogId, _currentUser.Id));
            }
        }
        

        private void _loadSessionForDialog(long dialogId)
        {
            LoadDialogSession?.Invoke(this, new LoadDialogSessionEventArgs(dialogId));
        }

        public MainViewModel()
        {
            MessageSendCommand = new Command(SendMessage, CanSendMessage);
            ClientStopCommand = new Command(Stop, CanStopClient);
            ExportKeysCommand = new Command(ExportKeys, CanExportKeys);
            EncryptSessionCommand = new Command(EditEncryptionSetting, CanEditEncryptionSetting);
            UpdateDialogEncryptionKeysCommand = new Command(UpdateDialogEncryptionKeys, CanUpdateDialogEncryptionKeys);
            SendFileCommand = new Command(SendFile, CanSendFile);
            LoadFileCommand = new CommandWithParametr(LoadFile, CanLoadFile);
            ImportKeysCommand = new Command(ImportKeys, CanImportKeys);

            _currentUser = new UserInfo();
            //FileSendProgresses.Add(new FileSendProgress("file1", new DeleteProgressDelegate(DeleteFileSendProgress)));
            //OnPropertyChanged("FileSendProgresses");
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
                    if(dialog.Name.Equals(login))
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
        public void AddContact(UserInfo userInfo)
        {
            _contacts.Add(userInfo);
            if (CurrentDialog != null)
            {
                CurrentDialog.BindMessagesToAuthor(userInfo);
            }
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
                    EncryptionSettingsChanged?.Invoke(this, new EncryptionSettingsEventArgs(CurrentDialog.Sign, CurrentDialog.Encrypt, CurrentDialog.DialogId));
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
                    EncryptionSettingsChanged?.Invoke(this, new EncryptionSettingsEventArgs( CurrentDialog.Sign, CurrentDialog.Encrypt, CurrentDialog.DialogId));

                    //EncryptSettingChanged?.Invoke(this, new EncryptionSettingsEventArgs(_sign, _encrypt));
                }
            }
        }

        public IMsgBoxService MessageService
        {
            get
            {
                return _messageService;
            }

            set
            {
                _messageService = value;
            }
        }

        public ObservableCollection<FileSendProgress> FileSendProgresses
        {
            get
            {
                return _fileSendProgresses;
            }

            set
            {
                _fileSendProgresses = value;
            }
        }

        //public List<Attachment> NewMessageAttachments
        //{
        //    get
        //    {
        //        return _newMessageAttachments;
        //    }

        //    set
        //    {
        //        _newMessageAttachments = value;
        //    }
        //}

        public void AddDialog(Dialog dialog)
        {
            if (!_dialogs.Contains(dialog))
            {
                _dialogs.Add(dialog);
                _loadSessionForDialog(dialog.DialogId);
            }
        }
        public void OnDialogSessionFaild(object sender, DialogSessionFaildEventArgs arg)
        {
            Dialogs.GetById(arg.DialogId).AddSessionErrorMessage(arg.ErrorMessage);
        }
        public void OnDialogSessionSuccess(object sender, DialogSessionSuccessEventArgs arg)
        {
            _messageService.ShowNotification("Ключи шифрования успешно обновлены");
            Dialogs.GetById(arg.DialogId).ClearDialogSessionError();
        }
        public void ShowError(string message)
        {
            _messageService.ShowNotification(message);
        }
        public void DeleteDialogMessages(long dialogId)
        {
            Dialog dialog = _dialogs.Where(d => d.DialogId == dialogId).FirstOrDefault();
            if(dialog != null)
            {
                dialog.DialogMessages.Clear();
                OnPropertyChanged("Messages");
            }
        }
    }
}
