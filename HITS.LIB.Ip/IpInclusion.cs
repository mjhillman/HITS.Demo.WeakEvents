using System.Collections.Generic;

namespace HITS.LIB.Ip
{
    public class IpInclusion
    {
        public static bool IsIncludedCountry(string countryCode)
        {
            List<string> InclusionList = new List<string>() {
                "US", "CA"};

            foreach (string includedCountry in InclusionList)
            {
                if (countryCode.Equals(includedCountry, System.StringComparison.InvariantCultureIgnoreCase)) return true;
            }
            return false;
        }
    }
}
