using HITS.Demo.WeakEvents.Services;
using HITS.LIB.WeakEvents;
using Microsoft.AspNetCore.Components;
using Microsoft.Toolkit.Mvvm.Messaging;

namespace HITS.Demo.WeakEvents.Pages
{
    public partial class Counter : ComponentBase, IDisposable
    {
        [Inject]
        private SessionService _sessionService { get; set; }

        public enum CounterEvents { CounterIncrement }

        private int currentCount = 0;

        private void IncrementCount()
        {
            //increment count
            currentCount++;

            //publish an event with the new count
            using (EventData eventData = new EventData("Counter",                   //sender
                                        _sessionService?.CurrentSession?.SessionId, //args
                                        currentCount.ToString(),                    //data
                                        nameof(CounterEvents.CounterIncrement )))   //token
            {
                WeakReferenceMessenger.Default.Send(new StandardMessage(eventData));
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
        // ~Counter()
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
