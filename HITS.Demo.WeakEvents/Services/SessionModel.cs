using HITS.Blazor.Grid;

namespace HITS.Demo.WeakEvents.Services
{
    public class SessionModel
    {
        [GridAttributes(FilterBy = true, SortBy = true, ShowInGrid = true)]
        public string SessionId { get; set; }

        [GridAttributes(FilterBy = true, SortBy = true, ShowInGrid = false)]
        public string UserName { get; set; }

        [GridAttributes(FilterBy = true, SortBy = true, ShowInGrid = false)]
        public string Role { get; set; }

        [GridAttributes(FilterBy = true, SortBy = true, ShowInGrid = false)]
        public string Verified { get; set; }

        [GridAttributes(FilterBy = true, SortBy = true, ShowInGrid = true)]
        public string IpAddress { get; set; }

        [GridAttributes(FilterBy = true, SortBy = true, ShowInGrid = false)]
        public string UserAgent { get; set; }

        [GridAttributes(FilterBy = true, SortBy = true, ShowInGrid = true)]
        public string City { get; set; }

        [GridAttributes(FilterBy = true, SortBy = true, ShowInGrid = true)]
        public string State { get; set; }

        [GridAttributes(FilterBy = true, SortBy = true, ShowInGrid = true)]
        public string CountryCode { get; set; }

        [GridAttributes(FilterBy = true, SortBy = true, ShowInGrid = true, FormatString = "g")]
        public DateTime CreatedOn { get; set; }

        public SessionModel()
        {
            SessionId = Guid.NewGuid().ToString();
            UserName = "unknown";
            Role = "user";
            Verified = "false";
            IpAddress = "unknown";
            UserAgent = "unknown";
            City = "unknown";
            State = "unknown";
            CountryCode = "US";
            CreatedOn = DateTime.Now;
        }
    }
}
