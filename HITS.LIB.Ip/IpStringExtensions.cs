using System;
using System.Net;
using System.Net.Sockets;

namespace HITS.LIB.Ip
{
    public static class IpStringExtensions
    {
        /// <summary>
        /// This method determines if an IP address is valid.
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public static bool IsValidIPAddress(this string ipAddress)
        {
            return (IsIpV4AddressValid(ipAddress) | IsIpV6AddressValid(ipAddress));
        }

        /// <summary>
        /// This method validates an IPv4 address.
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns>true if valid</returns>
        public static bool IsIpV4AddressValid(this string ipAddress)
        {
            if (!string.IsNullOrWhiteSpace(ipAddress))
            {
                ipAddress = ipAddress.Trim();
                if (!IsPrivateIPAddress(ipAddress))
                {
                    if (IPAddress.TryParse(ipAddress, out IPAddress ip))
                    {
                        return ip.AddressFamily == AddressFamily.InterNetwork;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// This method validates an IPv6 address.
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns>true if valid</returns>
        public static bool IsIpV6AddressValid(this string ipAddress)
        {
            if (!string.IsNullOrWhiteSpace(ipAddress))
            {
                ipAddress = ipAddress.Trim();
                if (!IsPrivateIPAddress(ipAddress))
                {
                    if (IPAddress.TryParse(ipAddress, out IPAddress ip))
                    {
                        return ip.AddressFamily == AddressFamily.InterNetworkV6;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// This method is used to identify private IP addresses.
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        private static bool IsPrivateIPAddress(string ipAddress)
        {
            if (ipAddress.StartsWith("::1")) return true;
            if (ipAddress.StartsWith("0.")) return true;
            if (ipAddress.StartsWith("10.")) return true;
            if (ipAddress.StartsWith("14.")) return true;
            if (ipAddress.StartsWith("24.")) return true;
            if (ipAddress.StartsWith("39.")) return true;
            if (ipAddress.StartsWith("127.")) return true;
            if (ipAddress.StartsWith("128.")) return true;
            if (ipAddress.StartsWith("169.")) return true;
            if (ipAddress.StartsWith("172.")) return true;
            if (ipAddress.StartsWith("191.")) return true;
            if (ipAddress.StartsWith("192.")) return true;
            if (ipAddress.StartsWith("224.")) return true;
            return false;
        }

        /// <summary>
        /// This method converts an IP address to an integer value.
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public static int IpToInt32(this string ipAddress)
        {
            var address = IPAddress.Parse(ipAddress);
            byte[] bytes = address.GetAddressBytes();

            // flip big-endian(network order) to little-endian
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            return BitConverter.ToInt32(bytes, 0);
        }

        /// <summary>
        /// This method converts an integer value to an IP address.
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public static string Int32ToIpAddress(this int ipAddress)
        {
            byte[] bytes = BitConverter.GetBytes(ipAddress);

            // flip little-endian to big-endian(network order)
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            return new IPAddress(bytes).ToString();
        }
    }
}
