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

﻿namespace IVPN.WiFi
{
    public class WiFiNetwork
    {
        public WiFiNetwork(string ssid)//, byte[] bssid)
        {
            SSID = ssid;

            if (string.IsNullOrEmpty(SSID))
            {
                //throw new IVPNInternalException("Not defined SSID for WiFi network");
                SSID = "";
            }
            //if (bssid == null)
            //    throw new IVPNInternalException("Not defined BSSID for WiFi network");
            //if (bssid.Length!=6)
            //    throw new IVPNInternalException("Unexpected length of BSSID for WiFi network");
            //BSSID = bssid;
        }

        /// <summary>
        /// WiFi network name
        /// </summary>
        public string SSID { get; }
        
        /// <summary>
        /// Unic WiFi identifier (e.g. AP MAC address)
        /// </summary>
        //public byte[] BSSID { get; }

        public override bool Equals(object obj)
        {
            if (!(obj is WiFiNetwork))
                return false;

            return Equals((WiFiNetwork)obj);
        }

        protected bool Equals(WiFiNetwork other)
        {
            return string.Equals(SSID, other.SSID); // && Enumerable.SequenceEqual(BSSID, other.BSSID);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return SSID != null ? SSID.GetHashCode() : 0;
            }
        }

        public override string ToString()
        {
            return SSID;
        }
    }
}
