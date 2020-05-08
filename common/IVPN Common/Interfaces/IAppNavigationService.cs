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
using IVPN.Models;

namespace IVPN.Interfaces
{
    public interface IAppNavigationService
    {
        event EventHandler<NavigationTarget> Navigated;

        void NavigateToMainPage(NavigationAnimation animation);
        void NavigateToInitPage(NavigationAnimation animation);
        void NavigateToLogInPage(NavigationAnimation animation, bool doLogIn = false, bool doForceLogin = false);
        void NavigateToSessionLimitPage(NavigationAnimation animation);
        void NavigateToLogOutPage(NavigationAnimation animation);
        void NavigateToSingUpPage(NavigationAnimation animation);
        
        void NavigateToServerSelection(NavigationAnimation animation = NavigationAnimation.FadeToLeft);
        void NavigateToEntryServerSelection(NavigationAnimation animation = NavigationAnimation.FadeToLeft);
        void NavigateToExitServerSelection(NavigationAnimation animation = NavigationAnimation.FadeToLeft);

        void NavigateToAutomaticServerConfiguration(NavigationAnimation animation = NavigationAnimation.FadeToLeft);

        void OpenUrl(string url);

        void ServerLocationSelectedAutomatic();
        void ServerLocationSelected(ServerLocation serverLocation);

        void GoBack();

        void ShowSettingsWindow();

        NavigationTarget CurrentPage
        {
            get;
        }
    }
}
