namespace RequestReply
{
    public interface IListener<Tid>
    {
        Task<IMessage<Tid>> Listen();
    }
}
