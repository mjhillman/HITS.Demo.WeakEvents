using Microsoft.Toolkit.Mvvm.Messaging.Messages;

namespace HITS.LIB.WeakEvents
{
    /// <summary>
    /// This class defines a message
    /// </summary>
    /// <remarks>https://docs.microsoft.com/en-us/windows/communitytoolkit/mvvm/messenger</remarks>
    public class StandardMessage : ValueChangedMessage<EventData>
    {
        public StandardMessage(EventData eventData) : base(eventData)
        {
            
        }
    }
}
