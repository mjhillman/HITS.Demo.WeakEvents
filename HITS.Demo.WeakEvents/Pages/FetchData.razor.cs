using HITS.Demo.WeakEvents.Services;
using HITS.Demo.WeakEvents.Shared;
using HITS.LIB.WeakEvents;
using Microsoft.AspNetCore.Components;
using Microsoft.Toolkit.Mvvm.Messaging;

namespace HITS.Demo.WeakEvents.Pages
{
    public partial class FetchData : ComponentBase, IDisposable
    {
        private bool disposedValue;

        [Inject]
        private NavigationManager _navigationManager { get; set; }

        [Inject]
        private SessionService _sessionService { get; set; }

        /// <summary>
        /// The view model for the session log grid component
        /// </summary>
        private SessionGridViewModel _gridViewModel { get; set; }

        protected override void OnInitialized()
        {
            WeakReferenceMessenger.Default.Register<StandardMessage>(this, WeakEventHandler);
            _gridViewModel = new SessionGridViewModel();
            
            //publish an event requesting log data
            using (EventData eventData = new EventData("FetchData",                             //sender
                                        _sessionService?.CurrentSession?.SessionId,             //args
                                        null,                                                   //data
                                        nameof(LogDataManager.LogEvents.GetLogDataRequest)))    //token
            {
                WeakReferenceMessenger.Default.Send(new StandardMessage(eventData));
            }
        }

        public void WeakEventHandler(object r, StandardMessage m)
        {
            EventData eventData = m.Value as EventData;

            if (eventData != null)
            {
                //process the GetLogDataResponse
                if (eventData.Token == nameof(LogDataManager.LogEvents.GetLogDataResponse) &&
                        eventData.Args.ToString() == _sessionService.CurrentSession.SessionId)
                {
                    _gridViewModel.GridParameters.DataList = eventData.Data as List<SessionModel>;
                    _gridViewModel.GridParameters.GridTitle = $"{_gridViewModel.GridParameters.DataList.Count.ToString("N0")} entries";
                    StateHasChanged();
                }
            }
        }

        private void EraseLog()
        {
            //publish an event requesting to erase the log records
            using (EventData eventData = new EventData("FetchData",                         //sender
                                        _sessionService?.CurrentSession?.SessionId,         //args
                                        null,                                               //data
                                        nameof(LogDataManager.LogEvents.EraseLogRequest)))  //token
            {
                WeakReferenceMessenger.Default.Send(new StandardMessage(eventData));
            }

            _navigationManager.NavigateTo(@"\", true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    _gridViewModel?.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~FetchData()
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
    }
}
