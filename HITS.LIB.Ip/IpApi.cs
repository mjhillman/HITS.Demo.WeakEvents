using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace HITS.LIB.Ip
{
    public class IpApi
    {
        /// <summary>
        /// Go to https://ipapi.com/documentation for details.
        /// </summary>
        /// <param name="ipApiRequest"></param>
        /// <returns>IpApiResponse</returns>
        public static async Task<IpApiResponse> GetLocationInfoAsync(IpApiRequest ipApiRequest)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string url = $"http://api.ipapi.com/api/{ipApiRequest.IpAddress}/?access_key={ipApiRequest.Key}";

                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.Timeout = TimeSpan.FromSeconds(30);
                    //string json = await httpClient.GetStringAsync(url); //test to see returned string in the debug mode
                    return await httpClient.GetFromJsonAsync<IpApiResponse>(url);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
