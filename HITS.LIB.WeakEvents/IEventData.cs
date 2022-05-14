namespace HITS.LIB.WeakEvents
{
    public interface IEventData
    {
        object Args { get; set; }
        object Data { get; set; }
        object Sender { get; set; }
        string Token { get; set; }
    }
}