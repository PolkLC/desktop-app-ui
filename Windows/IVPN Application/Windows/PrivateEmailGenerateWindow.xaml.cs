//
//  IVPN Client Desktop
//  https://github.com/ivpn/desktop-app-ui
//
//  Created by Stelnykovych Alexandr.
//  Copyright (c) 2020 Privatus Limited.
//
//  This file is part of the IVPN Client Desktop.
//
//  The IVPN Client Desktop is free software: you can redistribute it and/or
//  modify it under the terms of the GNU General Public License as published by the Free
//  Software Foundation, either version 3 of the License, or (at your option) any later version.
//
//  The IVPN Client Desktop is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
//  or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more
//  details.
//
//  You should have received a copy of the GNU General Public License
//  along with the IVPN Client Desktop. If not, see <https://www.gnu.org/licenses/>.
//

﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using IVPN.Models.PrivateEmail;
using IVPN.ViewModels;

namespace IVPN.Windows
{
    /// <summary>
    /// Interaction logic for PrivateEmailGenerateWindow.xaml
    /// </summary>
    public partial class PrivateEmailGenerateWindow : Window, INotifyPropertyChanged
    {
        #region Static functionality
        private static PrivateEmailGenerateWindow __Instance;
        public static void GenerateEmail(PrivateEmailsManagerViewModel model)
        {
            __Instance?.Close();
            __Instance = new PrivateEmailGenerateWindow(model);
            __Instance.Show();
        }

        public static void EditEmail(PrivateEmailsManagerViewModel model, PrivateEmailInfo emailToEdit)
        {
            __Instance?.Close();
            __Instance = new PrivateEmailGenerateWindow(model, emailToEdit);
            __Instance.Show();
        }

        public static void CloseAllWindows()
        {
            __Instance?.Close();
        }
        #endregion // Static functionality

        public PrivateEmailsManagerViewModel Model { get; private set; }

        private readonly bool __IsEditing;
        private PrivateEmailInfo __Email;
        public PrivateEmailInfo Email
        {
            get { return __Email; }
            set
            {
                __Email = value;
                NotifyPropertyChanged();
            }
        }

        private PrivateEmailGenerateWindow(PrivateEmailsManagerViewModel model)
        {
            Model = model;
            InitializeComponent();

            MainView.Visibility = Visibility.Collapsed;
            ProgressView.Visibility = Visibility.Visible;

            DataContext = this;
        }

        private PrivateEmailGenerateWindow(PrivateEmailsManagerViewModel model, PrivateEmailInfo emailToEdit)
        {
            Model = model;
            Email = emailToEdit;
            __IsEditing = true;

            InitializeComponent();

            MainView.Visibility = Visibility.Visible;
            ProgressView.Visibility = Visibility.Collapsed;

            GuiDiscardButtonText.Text = StringUtils.String("Button_Cancel");
            
            DataContext = this;
        }

        private async void PrivateEmailGenerateWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            // hide 'Minimize' button
            Button minimizeButton = Template.FindName("GuiTemplateButtonMinimizeInTitle", this) as Button;
            if (minimizeButton != null) minimizeButton.Visibility = Visibility.Collapsed;

            if (!__IsEditing)
            {
                try
                {
                    OnWillExecute();
                    Email = await Model.GenerateNewEmail(OnError);
                }
                finally
                {
                    OnDidExecute();
                }
            }
        }

        private void OnDidExecute()
        {
            GuiProgressText.Text = "";
            ProgressView.Visibility = Visibility.Collapsed;
            MainView.Visibility = Visibility.Visible;
            UIProgressBar.IsIndeterminate = false;
        }

        private void OnWillExecute()
        {
            if (string.IsNullOrEmpty(GuiProgressText.Text))
                GuiProgressText.Text = "Updating data";
            MainView.Visibility = Visibility.Collapsed;
            ProgressView.Visibility = Visibility.Visible;
            UIProgressBar.IsIndeterminate = true;
        }

        private void OnError(string errorText, string errorDescription)
        {
            Close();
            if (Visibility == Visibility.Visible)
                MessageBox.Show(errorText+ Environment.NewLine+errorDescription, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private async void GuiButtonSave_OnClick(object sender, RoutedEventArgs e)
        {
            if (Email != null && !string.Equals(Email.Notes, GuiTextBoxNotes.Text))
            {
                GuiProgressText.Text = "Updating notes";
                
                try
                {
                    OnWillExecute();
                    await Model.UpdateNotes(Email, GuiTextBoxNotes.Text, OnError);
                }
                finally
                {
                    OnDidExecute();
                }
            }

            Close();
        }

        private async void GuiButtonDiscard_OnClick(object sender, RoutedEventArgs e)
        {
            if (__IsEditing)
            {
                Close();
                return;
            }

            GuiProgressText.Text = "Discarding";
            
            try
            {
                OnWillExecute();
                await Model.DeleteEmail(Email, OnError);
            }
            finally
            {
                OnDidExecute();
            }
            Close();
        }


        private void GuiButtonCopy_OnClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Email?.Email))
                Clipboard.SetText(Email.Email);
        }
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion //INotifyPropertyChanged

    }
}
