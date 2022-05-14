using HITS.Demo.WeakEvents.Pages;
using HITS.Demo.WeakEvents.Services;
using HITS.Extensions.Object;
using HITS.LIB.Ip;
using HITS.LIB.WeakEvents;
using Microsoft.AspNetCore.Components;
using Microsoft.Toolkit.Mvvm.Messaging;

namespace HITS.Demo.WeakEvents.Shared
{
    public partial class MainLayout : LayoutComponentBase, IDisposable
    {
        [CascadingParameter]
        public Error Error { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private IHttpContextAccessor _httpContextAccessor { get; set; }

        [Inject]
        private SessionService _sessionService { get; set; }

        public string Count { get; set; } = "0";

        [Parameter]
        public bool Hide { get; set; } = false;

        //no prerender
        protected override async Task OnInitializedAsync()
        {
            try
            {
                if (_sessionService.CurrentSession == null)
                {
                    _sessionService.CurrentSession = new SessionModel();
                    _sessionService.CurrentSession.IpAddress = _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString();
                    _sessionService.CurrentSession.UserAgent = _httpContextAccessor?.HttpContext?.Request?.Headers["User-Agent"];
                    _sessionService.CurrentSession.Verified = "false";
                    _sessionService.CurrentSession.CountryCode = "US";

                    //_sessionService.CurrentSession.IpAddress = "112.203.116.128"; //russian test
                    //_sessionService.CurrentSession.IpAddress = "123.60.83.110"; //china test
                    _sessionService.CurrentSession.IpAddress = "23.81.0.59"; //us test
                    //_sessionService.CurrentSession.IpAddress = "68.84.230.216"; //self test
                    //_sessionService.CurrentSession.IpAddress = "95.70.128.252"; //asterik test
                }

                await CyberSecurityAsync();

            }
            catch (System.Exception ex)
            {
                await Error.ProcessErrorAsync(ex, _sessionService?.CurrentSession?.SerializeToJson());
            }
        }

        private async Task CyberSecurityAsync()
        {            
            if (_sessionService.CurrentSession.Verified == "false" && _sessionService.CurrentSession.IpAddress.IsValidIPAddress())
            {
                IpApiResponse ipApiResponse = await IpApi.GetLocationInfoAsync(new IpApiRequest(_sessionService.CurrentSession.IpAddress, Program.AppSettings.ipapikey));
                _sessionService.CurrentSession.City = ipApiResponse.City;
                _sessionService.CurrentSession.State = ipApiResponse.RegionName;
                _sessionService.CurrentSession.CountryCode = ipApiResponse.CountryCode;
                _sessionService.CurrentSession.Verified = "true";
            }

            if (IpExclusion.IsExcluded(_sessionService.CurrentSession.IpAddress))
            {
                _sessionService.CurrentSession.IpAddress += ":Ex";
                await LogSessionAsync();
                NavigationManager.NavigateTo(IpExclusion.REDIRECT_ADDRESS, true);
            }
            else if (!IpInclusion.IsIncludedCountry(_sessionService.CurrentSession.CountryCode))
            {
                _sessionService.CurrentSession.IpAddress += ":Foreign";
                await LogSessionAsync();
                NavigationManager.NavigateTo(IpExclusion.REDIRECT_ADDRESS, true);
            }
            else
            {
                _sessionService.CurrentSession.IpAddress += ":Ok";
                await LogSessionAsync();
            }
        }

        /// <summary>
        /// This method sends a request to make a log entry via a weak event.
        /// </summary>
        /// <returns></returns>
        private async Task LogSessionAsync()
        {
            try
            {
                //request a log entry
                using (EventData eventData = new EventData("MainLayout",                            //sender
                                            null,                                                   //args
                                            _sessionService?.CurrentSession,                        //data
                                            nameof(LogDataManager.LogEvents.SessionLogRequest)))    //token
                {
                    WeakReferenceMessenger.Default.Send(new StandardMessage(eventData));
                }
            }
            catch (Exception ex)
            {
                await Error.ProcessErrorAsync(ex, _sessionService.CurrentSession?.SerializeToJson());
            }
        }

        /// <summary>
        /// subscribe to counter event
        /// </summary>
        /// <param name="firstRender"></param>
        /// <returns></returns>
        /// <remarks>
        /// the counter page will raise an event on each counter increment
        /// watch the counter increment in the top row
        /// the sender and session id are used to ensure the event is handled by the right web page function
        /// open another browser window and see that the counts increment independently
        /// </remarks>
        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                //subscribe to the counter event
                //you should only subscribe once
                WeakReferenceMessenger.Default.Register<StandardMessage>(this, WeakEventHandler);
            }
            return base.OnAfterRenderAsync(firstRender);
        }

        /// <summary>
        /// When the "Counter" event is published this code processes the event.
        /// </summary>
        /// <param name="r">this class</param>
        /// <param name="m">instance of the StandardMessage class containing the EventData</param>
        /// <remarks>Since the WeakEvents class is a singleton, this method will be invoked for any 
        /// session where the user clicks the increment button so we need to make sure we are 
        /// processing the event for the correct session. We do this by comparing the session ids.</remarks>
        public async void WeakEventHandler(object r, StandardMessage m)
        {
            EventData eventData = m.Value as EventData;

            if (eventData != null)
            {
                if (eventData.Sender.ToString() == "Counter" &&
                    eventData.Args.ToString() == _sessionService.CurrentSession.SessionId &&
                    eventData.Token == nameof(Counter.CounterEvents.CounterIncrement))
                {
                    Count = eventData.Data.ToString();
                    await InvokeAsync(StateHasChanged);
                }
            }
        }

        #region dispose

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    WeakReferenceMessenger.Default.UnregisterAll(this);
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~MainLayout()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
