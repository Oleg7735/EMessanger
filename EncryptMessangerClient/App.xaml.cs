using EncryptMessangerClient.ViewModel;
using EncryptMessangerClient.Events;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using EncryptMessangerClient.Model;
using System.ComponentModel;
using EncryptMessanger.dll.SendibleData;
using EncryptMessangerClient.extensions;
using EncryptMessangerClient.MessageBoxService;

namespace EncryptMessangerClient
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public delegate EncryptionSettingsEventArgs GetDialogEncryptionSettings(string login);

    public partial class App : Application
    {
        private Client _client; //= new Client();
        public Client CurrentClient { get { return _client; } }
        private AuthWindow _logInForm;
        private EncryptionSettings _encrytionSettingsForm;
        private RegistrationForm _registrationWindow;
        private RegistrationViewModel _registrationViewModel;
        private MsgBoxService _messageService = new MsgBoxService();


        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _client = new EncryptMessangerClient.Client();
            _client.AuthError = OnClientAuthError;
            _client.ClientOnline = OnClientOnline;
            _client.ClientExit = OnClientExit;
            _client.GetDialogSettings = new GetDialogEncryptionSettings(GetDialogSettingsFromVm);
            _client.EncryptionSettingsChanged += OnEncryptionSettingChanged;
            _client.RegistrationError += OnRegistrationError;
            _client.RegistrationSuccess += OnRegistrationSuccess;
            _client.DilogsReceived += OnDialogsRecponce;
            _client.AuthSuccess += OnClientAuthSuccess;
            _client.UserInfoReceived += OnUserInfoResponce;
            _client.MessagesInfoReceived += OnDialogMessagesReceived;
            MainWindow mainWindow = new MainWindow();
            MainViewModel vm = mainWindow.DataContext as MainViewModel;
            vm.MessageSend += SendMessage;
            CurrentClient.Resive += Client_NewMessage;
            vm.StopClient += OnClientStop;
            vm.ExportKeysEventHandler += OnExportKeys;
            vm.EditEncryptionSettings += OnEditEncryptionSettings;
            vm.EncryptionSettingsChanged += OnEncryptionSettingChangedByUser;
            vm.DialogsRequest += OnDialogsRequest;
            vm.LoadDialogUserInfo += OnUserInfoRequest;
            vm.UpdateDialogKeys += OnUpdateDialogEncryptionKeys;
            vm.LoadDialogSession += OnLoadDialogSession;
            vm.LoadDialogMessages += OnLoadDialogMessages;
            vm.FileSend += OnSendFile;
            vm.DeleteProgress += DeleteFileSendProgress;
            mainWindow.Closed += MainWindow_Closed;

            _client.DialogSessionFaild += vm.OnDialogSessionFaild;
            _client.DialogSessionSuccess += vm.OnDialogSessionSuccess;
            _client.SessionUpdated += OnSessionUpdated;
            //mainWindow.Closed += vm.ClientStopCommand;
            this.MainWindow = mainWindow;
            //mainWindow.Show();

            _logInForm = new AuthWindow();
            _logInForm.Closed += AuthWindow_Closed;
            

            _encrytionSettingsForm = new EncryptionSettings();
            _encrytionSettingsForm.Closing += OnEncryptionSettingsFormClosed;
            _encrytionSettingsForm.DataContext = vm;
            //EncryptionSettingsViewModel evm = _encrytionSettingsForm.DataContext as EncryptionSettingsViewModel;
            //evm.EncryptSettingChanged += OnEncryptSettingChanged;
            //evm.SignSettingChanged += OnSignSettingChanged;

            LogInViewModel logvm = _logInForm.DataContext as LogInViewModel;
            logvm.AuthClient += OnClientAuthRequest;
            logvm.CloseClient += OnAuthExit;
            logvm.RegistrateClient += OpenRegstrationWindow;

            
            _logInForm.Show();

        }
        
        private void Client_NewMessage(object sender, NewMessageEventArgs e)
        {
            MainViewModel vm = null;
            Window window = null;
            Dispatcher.Invoke(() =>
            {
                vm = MainWindow.DataContext as MainViewModel;
                window = MainWindow;
            });

            if (vm != null)
            {
                window.Dispatcher.Invoke(() =>
                {
                    //vm.MessageBox = e.Message;

                    //vm.Messages.Add(new DialogMessage(e.Interlocutor, e.Message, e.IsAltered));
                    vm.AddMessage(e.Interlocutor, e.DialogId, e.SendDate, e.Message, e.IsAltered);
                });
            }
        }
        private void SendMessage(object sender, MessageSendEventArgs e)
        {
            CurrentClient.SendMessage(e.Message, e.DialogId);
        }
        private void OnClientStop(object sender, EventArgs e)
        {
            _client.Stop();
        }
        private void OnAuthExit(object sender, EventArgs e)
        {
            _logInForm.Close();
        }
        private void AuthWindow_Closed(object sender, EventArgs e)
        {            
            if (_registrationWindow != null)
            {
                _registrationWindow.Close();
            }
            _client.Stop();
            Shutdown();
        }
        private void MainWindow_Closed(object sender, EventArgs e)
        {
            if(_logInForm!=null)
            {
                _logInForm.Close();
            }
            if (_registrationWindow != null)
            {
                _registrationWindow.Close();
            }
            _client.Stop();
            Shutdown();
        }
        private  void OnClientAuthRequest(object sender, ClientAuthEventArgs e)
        {
            _client.Auth(e.Login, e.Password);
        }

        private async void OnClientAuthSuccess(object sender, ClientAuthSuccessEventArgs e)
        {
            _logInForm.Hide();
            StartMainWindow(e.Login, e.Id);
            Dispatcher.Invoke(() =>
            {
                MainViewModel vm = MainWindow.DataContext as MainViewModel;
                //vm.CurrentUserLogin = e.Login;
            });
            _client.RequesForDialog(20, 0);
            await _client.StartAsync();
        }

        private void StartMainWindow(string login, long userId)
        {
            MainWindow.Show();

            MainViewModel vm = null;
            Dispatcher.Invoke(() =>
            {
                vm = MainWindow.DataContext as MainViewModel;
            });

            if (vm != null)
            {
                Dispatcher.Invoke(() =>
                {
                    //vm.MessageBox = e.Message;

                    //vm.Messages.Add(new DialogMessage(e.Interlocutor, e.Message, e.IsAltered));
                    vm.CurrentUserLogin = login;
                    vm.CurrentUserId = userId;
                    vm.MessageService = _messageService;
                });
            }
            
        }

        private void OnClientAuthError(object sender, AuthErrorEventArgs error)
        {
            LogInViewModel vm = _logInForm.DataContext as LogInViewModel;
            vm.AuthError = error.Error;
        }

        private void OnClientOnline(object sender, ClientStatusOnlineEventArgs client)
        {
            /*MainViewModel vm = null;
            Dispatcher.Invoke(() =>
            {
                vm = MainWindow.DataContext as MainViewModel;
            });
            if (vm != null)
            {
                Dialog dialog = null;
                foreach (string s in client.Logins)
                {
                    if (s != "" && s != ((Client)sender).Login)
                    {
                        dialog = new Dialog(s);
                        Dispatcher.Invoke(() =>
                        {

                            if (!vm.Dialogs.Contains(dialog))
                            {
                                vm.Dialogs.Add(new Dialog(s));
                            }
                        });
                    }
                }
            }*/
            //throw new Exception("No implementation of App.Xml.OnClientOnline");

        }
        private void DeleteFileSendProgress(object sender, DeleteProgressEventArgs args)
        {
            MainViewModel vm = null;

            Dispatcher.Invoke(() =>
            {
                vm = MainWindow.DataContext as MainViewModel;
            });
            if (vm != null)
            {
                Dispatcher.Invoke(() => {
                    vm.FileSendProgresses.Remove(args.Progress);
                });
            }
        }
        private void OnClientExit(object sender, ClientStatusExitEventArgs client)
        {
            /*MainViewModel vm = null;
            Dispatcher.Invoke(() =>
            {
                vm = MainWindow.DataContext as MainViewModel;
            });
            if (vm != null)
            {
                Dispatcher.Invoke(()=> {
                    vm.Dialogs.Remove(new Dialog(client.Login));
                });                
            }*/
            //throw new Exception("No implementation of App.Xml.OnClientExit");

        }
        private void OnEditEncryptionSettings(object sender, EventArgs e)
        {
            _encrytionSettingsForm.Show();
        }
        private void OnExportKeys(object sender, ExportKeysEventArgs exportArgs)
        {
            _client.ExportKeys(exportArgs.Dialog, exportArgs.FileName); 
        }
        private void OnEncryptionSettingsFormClosed(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            _encrytionSettingsForm.Hide();
        }
        private EncryptionSettingsEventArgs GetDialogSettingsFromVm(string login)
        {
            MainViewModel vm = null;
            EncryptionSettingsEventArgs settings = null;

            Dispatcher.Invoke(() =>
            {
                vm = MainWindow.DataContext as MainViewModel;
            });
            if (vm != null)
            {
                Dispatcher.Invoke(() => {
                    settings = vm.GetDialogEncryptionSettings(login);
                });
            }
            return settings;
        }

        private void OnEncryptionSettingChanged(object sender, EncryptionSettingsEventArgs e)
        {
            MainViewModel vm = null;

            Dispatcher.Invoke(() =>
            {
                vm = MainWindow.DataContext as MainViewModel;
            });
            if (vm != null)
            {
                Dispatcher.Invoke(() =>
                {
                    vm.SetDialogEncryptSetting(e.Encrypt);
                    vm.SetDialogSignSetting(e.Sign);
                });
            }
        }

        private void OnRegistrationError(Object sender, RegistrationErrorEventArgs e)
        {
            _registrationViewModel.Error = e.ErrorDescription;
        }
        private void OnRegistrationSuccess(Object sender, RegistrationSuccessEventArgs e)
        {
            _registrationWindow.Hide();
            _logInForm.Hide();
            StartMainWindow(e.Login, e.Id);
        }
        //событие запроса на регистрацию на сервере от RegistrationViewModel
        private void OnRegistration(object sender, ClientRegistrationEventArgs e)
        {
            _client.RegistrateAntAuth(e.GetRegistrationInfo());
        }
        //событие отмены регистрации от RegistrationViewModel
        private void OnRegistrationCanseled(object sender, EventArgs e )
        {
            if(_registrationWindow!=null)
            {
                _registrationWindow.Close();
            }
        }
        private void OnEncryptionSettingChangedByUser(object sender, EncryptionSettingsEventArgs e)
        {
            _client.ChangeEncryptionSettings(e.Dialog, e.Sign, e.Encrypt);
        }
        //обрабатывает событие начала регистрации от LogInViewModel. Запускает флрму регистрации
        private void OpenRegstrationWindow(object sender, EventArgs e)
        {
            _registrationWindow = new RegistrationForm();
            _registrationViewModel = _registrationWindow.DataContext as RegistrationViewModel;
            _registrationViewModel.RegistrationEventHandler += OnRegistration;
            _registrationViewModel.CanselEventHandler += OnRegistrationCanseled;
            _registrationWindow.ShowDialog();
        }
        private void OnDialogsRequest(object sender, DialogsRequestEventArgs args)
        {
            _client.RequesForDialog(args.DialogsCount, args.DialogsOffset);
        }
        private void OnDialogsRecponce(object sender, DialogsReceivedEventArgs args)
        {
            if (args.Dialogs.Count != 0)
            {
                MainViewModel vm = null;
                Dispatcher.Invoke(() =>
                {
                    vm = MainWindow.DataContext as MainViewModel;
                });
                if (vm != null)
                {
                    Dispatcher.Invoke(() =>
                    {
                        foreach (DialogSendibleInfo d in args.Dialogs)
                        {
                            vm.AddDialog(new Dialog(d.DialogName, d.DialogId, d.EncryptMessages, d.SignMessages, d.MembersId));
                        }
                    });
                }
            }
        }
        private void OnUpdateDialogEncryptionKeys(object sender, UpdateDialogEncryptionKeysEventArgs args)
        {
            _client.UpdateDialogEncryptionKeys(args.DialogId, args.UserId);
        }

        private void OnUserInfoRequest(object sender, LoadDialogUserInfoEventArgs args)
        {
            _client.RequestUserInfo(args.UserId);
        }
        private void OnUserInfoResponce(object sender, DialogUserInfoReceivedEventArgs args)
        {
            MainViewModel vm = null;
            Dispatcher.Invoke(() =>
            {
                vm = MainWindow.DataContext as MainViewModel;
            });
            if (vm != null)
            {
                Dispatcher.Invoke(() =>
                {
                    vm.AddContact(new UserInfo(args.UserId, args.Login));
                });
            }
        }
        private void OnLoadDialogSession(object sender, LoadDialogSessionEventArgs args)
        {
            _client.LoadSession(args.DialogId);
        }
        private void OnLoadDialogMessages(object sender, LoadDialogMessagesEventArgs args)
        {
            _client.RequestDialogMessages(args.DialogId, args.Count, args.Offset);
        }
        private void OnDialogMessagesReceived(object sender, MessagesReceivedEventArgs args)
        {
            MainViewModel vm = null;
            Dispatcher.Invoke(() =>
            {
                vm = MainWindow.DataContext as MainViewModel;
                vm.AddMessages(args.GetDialogMessages(), args.Dialog);
            });
        }
        private void OnSessionUpdated(object sender, DialogSessionSuccessEventArgs args)
        {
            MainViewModel vm = null;
            Dispatcher.Invoke(() =>
            {
                vm = MainWindow.DataContext as MainViewModel;
                vm.DeleteDialogMessages(args.DialogId);
            });
        }
        private void OnSendFile(object sender, SendFileEventArgs args)
        {
            _client.SendFile(args.DialogId, args.SenderId, args.FilePath, args.FileName, args.UpdateProgressDelegate);
        }
        //    private void OnEncryptSettingChanged(object sender, EncryptionSettingsEventArgs e)
        //    {
        //        MainViewModel vm = null;

        //        Dispatcher.Invoke(() =>
        //        {
        //            vm = MainWindow.DataContext as MainViewModel;
        //        });
        //        if (vm != null)
        //        {
        //            Dispatcher.Invoke(() => {
        //                vm.SetDialogEncryptSetting(e.Encrypt);
        //            });
        //        }
        //    }
        //    private void OnSignSettingChanged(object sender, EncryptionSettingsEventArgs e)
        //    {
        //        MainViewModel vm = null;

        //        Dispatcher.Invoke(() =>
        //        {
        //            vm = MainWindow.DataContext as MainViewModel;
        //        });
        //        if (vm != null)
        //        {
        //            Dispatcher.Invoke(() => {
        //                vm.SetDialogSignSetting(e.Sign);
        //            });
        //        }
        //    }
    }
}
