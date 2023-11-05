namespace RequestReply
{
    public class RequestReply<Tid>
    {
        IMessage<Tid>? _sentMessage;
        TaskCompletionSource<IMessage<Tid>> _tcs;
        ISender<Tid> _sender;
        IListener<Tid> _listener;

        public RequestReply(IListener<Tid> listener, ISender<Tid> sender)
        {
            _tcs = new TaskCompletionSource<IMessage<Tid>>();
            _listener = listener;
            _sender = sender;
        }

        public async Task<IMessage<Tid>>? SendMessage(IMessage<Tid> message)
        {
            _sentMessage = message;
            await _sender.Send(_sentMessage);
            await Listen();
            return await _tcs.Task;
        }

        private async Task Listen()
        {
            await Task.Run(async () =>
            {
                IMessage<Tid> replyMessage;
                try
                {
                    do
                    {
                        replyMessage = await _listener.Listen();

                        if (replyMessage != null && replyMessage.IsRepliedToCurrentMessage(_sentMessage!))
                            _tcs.SetResult(replyMessage);
                    } while (replyMessage == null);
                }
                catch (Exception ex)
                {
                    _tcs.SetException(ex);
                }
            });
        }
    }
}
