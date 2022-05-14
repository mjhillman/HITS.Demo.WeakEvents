using System;
using System.Text.Json.Serialization;

namespace HITS.LIB.Ip
{
    public partial class IpApiResponse
    {
        [JsonPropertyName("ip")]
        public string Ip { get; set; }

        [JsonPropertyName("hostname")]
        public string Hostname { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("continent_code")]
        public string ContinentCode { get; set; }

        [JsonPropertyName("continent_name")]
        public string ContinentName { get; set; }

        [JsonPropertyName("country_code")]
        public string CountryCode { get; set; }

        [JsonPropertyName("country_name")]
        public string CountryName { get; set; }

        [JsonPropertyName("region_code")]
        public string RegionCode { get; set; }

        [JsonPropertyName("region_name")]
        public string RegionName { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("zip")]
        public string Zip { get; set; }

        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }

        [JsonPropertyName("location")]
        public Location Location { get; set; }

        [JsonPropertyName("time_zone")]
        public TimeZone TimeZone { get; set; }

        [JsonPropertyName("currency")]
        public Currency Currency { get; set; }

        [JsonPropertyName("connection")]
        public Connection Connection { get; set; }

        [JsonPropertyName("security")]
        public Security Security { get; set; }
    }

    public partial class Connection
    {
        [JsonPropertyName("asn")]
        public long Asn { get; set; }

        [JsonPropertyName("isp")]
        public string Isp { get; set; }
    }

    public partial class Currency
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("plural")]
        public string Plural { get; set; }

        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("symbol_native")]
        public string SymbolNative { get; set; }
    }

    public partial class Location
    {
        [JsonPropertyName("geoname_id")]
        public long GeonameId { get; set; }

        [JsonPropertyName("capital")]
        public string Capital { get; set; }

        [JsonPropertyName("languages")]
        public Language[] Languages { get; set; }

        [JsonPropertyName("country_flag")]
        public Uri CountryFlag { get; set; }

        [JsonPropertyName("country_flag_emoji")]
        public string CountryFlagEmoji { get; set; }

        [JsonPropertyName("country_flag_emoji_unicode")]
        public string CountryFlagEmojiUnicode { get; set; }

        [JsonPropertyName("calling_code")]
        public string CallingCode { get; set; }

        [JsonPropertyName("is_eu")]
        public bool IsEu { get; set; }
    }

    public partial class Language
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("native")]
        public string Native { get; set; }
    }

    public partial class Security
    {
        [JsonPropertyName("is_proxy")]
        public bool IsProxy { get; set; }

        [JsonPropertyName("proxy_type")]
        public object ProxyType { get; set; }

        [JsonPropertyName("is_crawler")]
        public bool IsCrawler { get; set; }

        [JsonPropertyName("crawler_name")]
        public object CrawlerName { get; set; }

        [JsonPropertyName("crawler_type")]
        public object CrawlerType { get; set; }

        [JsonPropertyName("is_tor")]
        public bool IsTor { get; set; }

        [JsonPropertyName("threat_level")]
        public string ThreatLevel { get; set; }

        [JsonPropertyName("threat_types")]
        public object ThreatTypes { get; set; }
    }

    public partial class TimeZone
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("current_time")]
        public DateTimeOffset CurrentTime { get; set; }

        [JsonPropertyName("gmt_offset")]
        public long GmtOffset { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("is_daylight_saving")]
        public bool IsDaylightSaving { get; set; }
    }
}
