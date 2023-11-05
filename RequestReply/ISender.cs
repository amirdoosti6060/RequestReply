namespace RequestReply
{
    public interface ISender<Tid>
    {
        Task Send(IMessage<Tid> message);
    }
}
