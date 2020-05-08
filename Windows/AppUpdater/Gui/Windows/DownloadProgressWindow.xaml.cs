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
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace AppUpdater.Gui
{
    /// <summary>
    /// Interaction logic for DownloadProgressWindow.xaml
    /// </summary>
    internal partial class DownloadProgressWindow : INotifyPropertyChanged
    {
        #region Static functionality
        private static DownloadProgressWindow _currWindow;
        public static void ShowWindow()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (_currWindow == null)
                {
                    _currWindow = new DownloadProgressWindow();
                    _currWindow.Owner = Application.Current.MainWindow;
                    _currWindow.Show();
                }
            });
        }

        public static void HideWindow()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (_currWindow != null)
                    _currWindow.DoClose();
                _currWindow = null;
            });
        }

        public static void Progress(long downloadedBytes, long totalBytes)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                ShowWindow();
                _currWindow.UpdateProgress(downloadedBytes, totalBytes);
            });
        }
        #endregion //Static functionality

        public long DownloadedBytes
        {
            get { return _downloadedBytes; }
            set
            {
                _downloadedBytes = value;
                OnPropertyChanged();
                OnPropertyChanged("ProgressText");
            }
        }
        private long _downloadedBytes;

        public long TotalBytes
        {
            get { return _totalBytes; }
            set
            {
                _totalBytes = value;
                OnPropertyChanged();
                OnPropertyChanged("ProgressText");
            }
        }
        private long _totalBytes;

        public string ProgressText
        {
            get { return string.Format("{0} / {1}", BytesToString(DownloadedBytes), BytesToString(TotalBytes)); }
        }

        private bool _isClosed;

        private DownloadProgressWindow()
        {
            InitializeComponent();
            DataContext = this;

            if (GuiController.AppIcon != null)
            {
                ImageSource imSource = GuiController.ToImageSource(GuiController.AppIcon);
                Icon = imSource;
            }
        }
        private void UpdateProgress(long downloadedBytes, long totalBytes)
        {
            TotalBytes = totalBytes;
            DownloadedBytes = downloadedBytes;
        }

        private void DoClose()
        {
            _isClosed = true;
            Close();
        }

        private void GuiButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            Updater.Cancel();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (_isClosed==false) // if user closed window
                Updater.Cancel();
        }

        static string BytesToString(long len)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
            if (len == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(len);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(len) * num).ToString(CultureInfo.InvariantCulture) + suf[place];
        }
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion //INotifyPropertyChanged

    }
}
