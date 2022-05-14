namespace HITS.LIB.WeakEvents
{
    public class EventData : IEventData, IDisposable
    {
        public object Sender { get; set; }
        public object Args { get; set; }
        public object Data { get; set; }
        public string Token { get; set; }

        public EventData()
        {
        }

        public EventData(object sender, object args)
        {
            Args = args;
            Sender = sender;
        }

        public EventData(object sender, object args, object data)
        {
            Args = args;
            Sender = sender;
            Data = data;
        }

        public EventData(object sender, object args, object data, string token)
        {
            Args = args;
            Sender = sender;
            Data = data;
            Token = token;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    if (Sender is IDisposable) ((IDisposable)Sender).Dispose();
                    if (Data is IDisposable) ((IDisposable)Data).Dispose();
                    if (Args is IDisposable) ((IDisposable)Args).Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~EventData()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}
