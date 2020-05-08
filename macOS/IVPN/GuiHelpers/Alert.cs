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
using AppKit;
using CoreGraphics;
using Foundation;

namespace IVPN.GuiHelpers
{
    public class IVPNAlert
    {
        public static void Show (NSWindow window, string title, string message = null, NSAlertStyle style = NSAlertStyle.Informational)
        {         
            if (string.IsNullOrEmpty (title) && string.IsNullOrEmpty (message))
                return;

            NSRunningApplication.CurrentApplication.Activate (NSApplicationActivationOptions.ActivateIgnoringOtherWindows);

            var alert = new NSAlert ();
            alert.AlertStyle = style;
            if (message != null)
                alert.InformativeText = message;
            if (title != null)
                alert.MessageText = title;
            alert.BeginSheet (window);
        }
      
        public static void Show (string title, string message = null, NSAlertStyle style = NSAlertStyle.Informational)
        {
            if (string.IsNullOrEmpty (title) && string.IsNullOrEmpty (message))
                return;
            
            NSRunningApplication.CurrentApplication.Activate ( NSApplicationActivationOptions.ActivateIgnoringOtherWindows );

            var alert = new NSAlert ();
            alert.AlertStyle = style;
            if (message!=null)
                alert.InformativeText = message;
            if (title!=null)
                alert.MessageText = title;
            alert.RunModal ();
        }

        public static string ShowInputBox (string title, string informativeText, string defaultValue, bool isMultiline = false, string placeholder = "")
        {
            NSAlert alert = new NSAlert ();

            alert.AddButton ("OK");
            alert.AddButton ("Cancel");

            if (!string.IsNullOrEmpty (title))
                alert.MessageText = title;
            
            if (!string.IsNullOrEmpty (informativeText))
                alert.InformativeText = informativeText;

            CGRect textFieldRect;
            if (isMultiline)
                textFieldRect = new CGRect (0, 0, 300, 72);
            else 
                textFieldRect = new CGRect (0, 0, 300, 24);

            NSScrollView sw = new NSScrollView (textFieldRect);
            NSTextView tf = new NSTextView (new CGRect (0, 0, sw.ContentSize.Width, sw.ContentSize.Height));

            sw.DocumentView = tf;
            sw.BorderType = NSBorderType.BezelBorder;

            if (!string.IsNullOrEmpty (defaultValue))
                tf.TextStorage.SetString (new Foundation.NSAttributedString (defaultValue));
            
            alert.AccessoryView = sw;

            nint result = alert.RunModal ();
            if (result == (int)NSAlertButtonReturn.First)
                return tf.String;

            return null;
        }

        public static string ShowInputBoxEx(string title, string informativeText, string defaultValue, string placeholder = "", int width = 0, NSFormatter textFormatter = null)
        {
            NSAlert alert = new NSAlert();

            alert.AddButton("OK");
            alert.AddButton("Cancel");

            if (!string.IsNullOrEmpty(title))
                alert.MessageText = title;

            if (!string.IsNullOrEmpty(informativeText))
                alert.InformativeText = informativeText;

            CGRect textFieldRect;
            if (width <= 0)
                width = 300;
            textFieldRect = new CGRect(0, 0, width, 36);

            CustomTextField tf = new CustomTextField(textFieldRect);
            tf.Alignment = NSTextAlignment.Center;
            tf.Font = NSFont.SystemFontOfSize(16);

            var dc = new NSDateComponentsFormatter();
            dc.UnitsStyle = NSDateComponentsFormatterUnitsStyle.Full;
            dc.AllowedUnits = NSCalendarUnit.Minute;

            if (textFormatter!=null)
                tf.Formatter = textFormatter;

            if (!string.IsNullOrEmpty(defaultValue))
                tf.StringValue = defaultValue;
            if (!string.IsNullOrEmpty(placeholder))
                tf.PlaceholderString = placeholder;

            alert.AccessoryView = tf;
            alert.Window.InitialFirstResponder = tf;


            nint result = alert.RunModal();
            if (result == (int)NSAlertButtonReturn.First)
                return tf.StringValue;

            return null;
        }
    }
}
