namespace RequestReply
{
    public interface IMessage<Tid>
    {
        Tid Id { get; init; }
        Tid? CorrelationID { get; set; }
        bool IsRepliedToCurrentMessage(IMessage<Tid> message);
    }
}
