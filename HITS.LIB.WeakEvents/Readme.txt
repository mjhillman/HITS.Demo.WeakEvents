Example of Registering (subscribing) a Weak Event
WeakReferenceMessenger.Default.Register<StandardMessage>(this, SessionLogRequestEventHandler);
SessionLogRequestEventHandler is an Action delegate method that handles the event. i.e...
public async void SessionLogRequestEventHandler(object r, StandardMessage m)
    {
        EventData eventData = m.Value as EventData;
        if (eventData != null && eventData.Sender.ToString() == "SessionLogRequest")
        {
            await WriteLogEntry((SessionModel)eventData.Data);
        }
    }

Example of sending (publishing) a weak event
using (EventData eventData = new EventData("SessionLogRequest", null, _sessionService?.CurrentSession))
{
    WeakReferenceMessenger.Default.Send(new StandardMessage(eventData));
}
When publishing an event you pass along any data required by the event handler.

Example of cleanup (unsubscribing) which should be used in the dispose method of any class with a subscription.
WeakReferenceMessenger.Default.UnregisterAll(this);

Note: I use the EventData class as a generic payload for the message.  
It has sufficient properties for most messaging requirements.
When publishing a message an instance of the EventData class is passed to all subscribers.

Note:  Since the WeakEvents class is a singleton evnets will be published to all sessions.
The session id is used to process the event for the correct session.