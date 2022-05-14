using System.Collections.Generic;

namespace HITS.LIB.Ip
{
    public class IpExclusion
    {
        public const string REDIRECT_ADDRESS = "https://www.dhs.gov/topics/cybersecurity";

        public static bool IsExcluded(string ipAddress)
        {
            if (ipAddress == null || !ipAddress.IsValidIPAddress()) return false;

            List<string> ExclusionList = new List<string>() {
                "1.158.44.32",
                "103.52.193.124",
                "112.203.116.128",
                "129.211.164.19",
                "162.55.81.190",
                "164.160.119.40",
                "168.119.64.246",
                "168.119.68.*",
                "180.163.220.*",
                "183.36.114.97",
                "194.237.137.46",
                "195.154.122.*",
                "2.83.140.177",
                "202.142.85.66",
                "212.192.241.18",
                "51.37.185.179",
                "58.20.199.28",
                "58.251.94.154",
                "62.87.131.65",
                "81.40.154.73",
                "82.11.182.154",
                "83.90.139.*",
                "84.172.99.91",
                "86.84.99.85",
                "91.156.58.*",
                "95.70.128.*"};

            foreach (string excludedIp in ExclusionList)
            {
                if (excludedIp.Contains("*") && ipAddress.Contains(excludedIp.Substring(0, excludedIp.IndexOf("*") - 2)))
                {
                    return true;
                }
                if (ipAddress == excludedIp) return true;
            }
            return false;
        }
    }
}
