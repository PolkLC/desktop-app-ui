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

﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace IVPN
{
    public class ErrorReporterEvent
    {
        internal ErrorReporterEvent(SharpRaven.Data.SentryEvent reportEvent)
        {
            Event = reportEvent;
        }

        public SharpRaven.Data.SentryEvent Event { get; }
         
        public override string ToString()
        {
            try
            {
                // The 'Event.Exception.Data' contains big log data which will be sent to Sentry.
                // But the text looks ugly after it's serialization (because all new lines are eacaped there; hard to read) 
                //
                // In order to have nice view of event text, we temporary erasing 'Event.Exception.Data'
                // After object serialization - we restore 'Event.Exception.Data' to previous state.
                // 'Event.Exception.Data' pints separately.

                // Making a copy of 'Data'
                Dictionary<string, string> tmpData = new Dictionary<string, string>();
                if (Event.Exception != null && Event.Exception.Data != null)
                {
                    foreach (DictionaryEntry i in Event.Exception.Data)
                    {
                        try
                        {
                            tmpData.Add(
                                (i.Key == null) ? "null" : i.Key.ToString(),
                                (i.Value == null) ? "null" : i.Value.ToString());
                        }
                        catch
                        {
                        }
                    }

                    // Erasing 'Data' in a event
                    Event.Exception.Data.Clear();
                }

                // Serializing event 
                System.Text.StringBuilder sb = new System.Text.StringBuilder(
                    JsonConvert.SerializeObject(
                        Event,
                        new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.None,
                            Formatting = Formatting.Indented,
                        }
                    ));

                if (Event.Exception != null && Event.Exception.Data != null)
                {
                    // Restore exception data (and print 'Data' items in alphabetical order)
                    var items = from pair in tmpData
                        orderby pair.Key ascending
                        select pair;
                    foreach (KeyValuePair<string, string> i in items)
                    {
                        // restore element
                        Event.Exception.Data.Add(i.Key, i.Value);

                        // print element
                        sb.Append(System.Environment.NewLine
                                  + "----------------------------------------------"
                                  + System.Environment.NewLine);

                        sb.Append($"[{(string) i.Key}] = {(string) i.Value}");
                    }
                }

                return sb.ToString();
            }
            catch 
            {
                return $"Failed to generate readable error-report.";
            }
        }
    }
}
