namespace HITS.LIB.Ip
{
    public class IpApiRequest
    {
        public enum FormatType { json, jsonp, xml, csv, yaml }
        public string IpAddress { get; set; }
        public FormatType Format { get; set; }
        public string Key { get; set; }

        public IpApiRequest(string ipAddress, string apapikey, FormatType formatType = FormatType.json)
        {
            IpAddress = ipAddress;
            Key = apapikey;
        }
    }
}
