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

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _client = new EncryptMessangerClient.Client();
            _client.AuthError = OnClientAuthError;
            _client.ClientOnline = OnClientOnline;
            _client.ClientExit = OnClientExit;
            _client.GetDialogSettings = new GetDialogEncryptionSettings(GetDialogSettingsFromVm);
            _client.EncryptionSettingsChanged += OnEncryptionSettingChanged;
            MainWindow mainWindow = new MainWindow();
            MainViewModel vm = mainWindow.DataContext as MainViewModel;
            vm.MessageSend += SendMessage;
            CurrentClient.Resive += Client_NewMessage;
            vm.StopClient += OnClientStop;
            vm.ExportKeysEventHandler += OnExportKeys;
            vm.EditEncryptionSettings += OnEditEncryptionSettings;
            vm.EncryptionSettingsChanged += OnEncryptionSettingChangedByUser;
            mainWindow.Closed += MainWindow_Closed;
           
            //mainWindow.Closed += vm.ClientStopCommand;
            this.MainWindow = mainWindow;
            //mainWindow.Show();

            _logInForm = new AuthWindow();
            _logInForm.Closed += MainWindow_Closed;
            

            _encrytionSettingsForm = new EncryptionSettings();
            _encrytionSettingsForm.Closing += OnEncryptionSettingsFormClosed;
            _encrytionSettingsForm.DataContext = vm;
            //EncryptionSettingsViewModel evm = _encrytionSettingsForm.DataContext as EncryptionSettingsViewModel;
            //evm.EncryptSettingChanged += OnEncryptSettingChanged;
            //evm.SignSettingChanged += OnSignSettingChanged;

            LogInViewModel logvm = _logInForm.DataContext as LogInViewModel;
            logvm.AuthClient += OnClientAuthRequest;
            logvm.CloseClient += MainWindow_Closed;

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
                    vm.AddMessage(e.Interlocutor, e.Message, e.IsAltered);
                });
            }
        }
        private void SendMessage(object sender, MessageSendEventArgs e)
        {
            CurrentClient.SendMessage(e.Message, e.Resiver);
        }
        private void OnClientStop(object sender, EventArgs e)
        {
            _client.Stop();
        }
        private void MainWindow_Closed(object sender, EventArgs e)
        {
            _client.Stop();
            Shutdown();
        }
        private async void OnClientAuthRequest(object sender, ClientAuthEventArgs e)
        {
            if (_client.Auth(e.Login, e.Password))
            {
                _logInForm.Hide();
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
                        vm.CurrentUserLogin = e.Login;
                    });
                }
                await _client.StartAsync();
            }
        }
        private void OnClientAuthError(object sender, AuthErrorEventArgs error)
        {
            LogInViewModel vm = _logInForm.DataContext as LogInViewModel;
            vm.AuthError = error.Error;
        }

        private void OnClientOnline(object sender, ClientStatusOnlineEventArgs client)
        {
            MainViewModel vm = null;
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
            }

        }
        private void OnClientExit(object sender, ClientStatusExitEventArgs client)
        {
            MainViewModel vm = null;
            Dispatcher.Invoke(() =>
            {
                vm = MainWindow.DataContext as MainViewModel;
            });
            if (vm != null)
            {
                Dispatcher.Invoke(()=> {
                    vm.Dialogs.Remove(new Dialog(client.Login));
                });                
            }

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

        private void OnRegistration(object sender, ClientAuthEventArgs e)
        {

        }
        private void OnEncryptionSettingChangedByUser(object sender, EncryptionSettingsEventArgs e)
        {
            _client.ChangeEncryptionSettings(e.Dialog,e.Sign,e.Encrypt);
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
