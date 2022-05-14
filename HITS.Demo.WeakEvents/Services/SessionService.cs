namespace HITS.Demo.WeakEvents.Services
{
    public class SessionService
    {
        public SessionModel CurrentSession { get; set; }

        public event EventHandler SessionDataChanged;

        public SessionService()
        {
        }

        public void OnSessionDataChanged(SessionDataChangedEventArgs e)
        {
            EventHandler handler = SessionDataChanged;
            handler?.Invoke(this, new SessionDataChangedEventArgs() { sessionModel = CurrentSession });
        }
    }

    public class SessionDataChangedEventArgs : EventArgs
    {
        public SessionModel sessionModel { get; set; }
    }
}
