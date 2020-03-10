﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using IVPN.Models.Session;
using IVPN.ViewModels;
using IVPN.Windows;

namespace IVPN.Views
{
    /// <summary>
    /// Interaction logic for wp_Login.xaml
    /// </summary>
    public partial class wp_Login : Page, INotifyPropertyChanged
    {
        public ViewModelLogIn ViewModel => __ViewModel;
        private readonly ViewModelLogIn __ViewModel;

        public bool IsPasswordEmpty
        {
            get { return __IsPasswordEmpty; }
            private set
            {
                __IsPasswordEmpty = value;
                NotifyPropertyChanged();
            }
        }
        private bool __IsPasswordEmpty;

        public wp_Login()
        {
            InitializeComponent();

            GuiProgressBar.Visibility = Visibility.Collapsed;
            GuiProgressBar.IsIndeterminate = false;
            LoginBtnCaption.Text = StringUtils.String("Button_LogIn");

            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow == null)
                throw new InvalidOperationException("App.Current.MainWindow as MainWindow == null");

            __ViewModel = mainWindow.LogInViewModel;
            __ViewModel.OnError += __ViewModel_OnError;
            __ViewModel.OnAccountCredentailsError += __ViewModel_OnAccountCredentailsError;
            __ViewModel.OnAccountSuspended += __ViewModel_OnAccountSuspended;

            __ViewModel.OnWillExecute += (sender) =>
            {
                LoginBtnCaption.Text = StringUtils.String("Button_Cancel");
                GuiProgressBar.Visibility = Visibility.Visible;
                GuiProgressBar.IsIndeterminate = true;
            };

            __ViewModel.OnDidExecute += (sender) =>
            {
                LoginBtnCaption.Text = StringUtils.String("Button_LogIn");
                
                GuiProgressBar.Visibility = Visibility.Collapsed;
                GuiProgressBar.IsIndeterminate = false;
            };

            DataContext = this;
        }

        void __ViewModel_OnAccountSuspended(AccountStatus session)
        {
            SubscriptionExpireWindow.Show(session, ViewModel.UserName);
        }
        
        void __ViewModel_OnError(string errorText, string errorDescription = "")
        {
            if (!string.IsNullOrEmpty(errorDescription))
                MessageBox.Show(Application.Current.MainWindow, errorDescription, errorText , MessageBoxButton.OK, MessageBoxImage.Error);
            else
                MessageBox.Show(Application.Current.MainWindow, errorText + Environment.NewLine + errorDescription, 
                    StringUtils.String("Error_MessageboxTitle"), 
                    MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void __ViewModel_OnAccountCredentailsError(string errorText, string errorDescription = "")
        {
            GuiTextBlockWrongPassword.Text = errorText + (string.IsNullOrEmpty(errorDescription)? "" : "\n\n" + errorDescription);
            GuiWrongPasswordPopup.IsOpen = true;
        }

        private void PrepareBeforeLogIn()
        {
            GuiWrongPasswordPopup.IsOpen = false;
        }
        
        private void GuiButtonLogin_OnClick(object sender, RoutedEventArgs e)
        {
            PrepareBeforeLogIn();
            ViewModel.LogInCommand.Execute(null);
        }

        private void GuiButtonStartFreeTrial_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.StartFreeTrialCommand.Execute(null);
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion //INotifyPropertyChanged
        
        private void Hyperlink_ClientAreaClick(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.ivpn.net/clientarea/login");
        }
    }
}
