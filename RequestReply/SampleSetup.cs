namespace RequestReply
{
    public static class SerialGenerator
    {
        private static int _serial = 0;

        public static int Serial
        {
            get
            {
                var s = _serial;
                _serial++;
                return s;
            }
        }
    }

    public class RequestMessage : IMessage<int?>
    {
        private int? _id;
        private int? _correlationId;

        public int? Id
        {
            get => _id;
            init => _id = value;
        }

        public int? CorrelationID
        {
            get => _correlationId;
            set => _correlationId = value;
        }

        public string Request { get; set; }

        public RequestMessage()
        {
            _id = SerialGenerator.Serial;
            CorrelationID = null;
            Request = "";
        }

        public RequestMessage(int? id)
        {
            _id = id;
            Request = "";
        }

        public bool IsRepliedToCurrentMessage(IMessage<int?> message)
        {
            return this.CorrelationID == message.Id;
        }
    }

    public class ReplyMessage : IMessage<int?>
    {
        private int? _id;
        private int? _correlationId;

        public int? Id
        {
            get => _id;
            init => _id = value;
        }
        public int? CorrelationID
        {
            get => _correlationId;
            set => _correlationId = value;
        }

        public string Reply { get; set; }

        public ReplyMessage()
        {
            _id = SerialGenerator.Serial;
            CorrelationID = null;
            Reply = "";
        }

        public ReplyMessage(int? id)
        {
            _id = id;
            CorrelationID = null;
            Reply = "";
        }

        public bool IsRepliedToCurrentMessage(IMessage<int?> message)
        {
            return this.CorrelationID == message.Id;
        }
    }

    public class VSender : ISender<int?>
    {
        private readonly IList<MessageRecord> _messageList;

        public VSender(IList<MessageRecord> messageList)
        {
            _messageList = messageList;
        }

        public Task Send(IMessage<int?> message)
        {
            var record = new MessageRecord();
            record.RequestMessage = (RequestMessage) message;
            _messageList.Add(record);
            return Task.CompletedTask;
        }
    }

    public class VListener : IListener<int?>
    {
        private readonly IList<MessageRecord> _messageList;

        public VListener(IList<MessageRecord> messageList)
        {
            _messageList = messageList;
        }

        public async Task<IMessage<int?>> Listen()
        {
            var task = Task.Run(() =>
            {
                ReplyMessage? ret = null;
                Console.Write("Enter the id of the message you want reply to: ");
                var corId = Console.ReadLine();
                Console.Write("Enter reply message: ");
                var replyMessage = Console.ReadLine();

                if (corId != null && replyMessage != null)
                {
                    int correlationId = Convert.ToInt32(corId);
                    var rec = _messageList.Where(e => e.RequestMessage != null && e.RequestMessage.Id == correlationId).FirstOrDefault();

                    if (rec != null)
                    {
                        rec.ReplyMessage = new ReplyMessage() 
                        {
                            CorrelationID = correlationId,
                            Reply = replyMessage
                        };

                        ret = rec.ReplyMessage;
                    }
                }

                return ret;
            });

            return await task;
        }
    }

    public class MessageRecord
    {
        public RequestMessage? RequestMessage { get; set; }
        public ReplyMessage? ReplyMessage { get; set; }
    }

    public class SampleSetup
    {
        private RequestReply<int?> _synm;
        private List<MessageRecord> _messageList;

        public SampleSetup()
        {
            _messageList = new List<MessageRecord>();
            VListener vListener = new VListener(_messageList);
            VSender vSender = new VSender(_messageList);

            _synm = new RequestReply<int?>(vListener, vSender);
        }

        public async Task Run()
        {
            ReplyMessage replyMessage;
            RequestMessage requestMessage;
            Object reply = null;
            for (int i = 0; i < 10; i++)
            {
                requestMessage = new RequestMessage();
                requestMessage.Request = $"My_Request_{requestMessage.Id}";

                Console.WriteLine($"\nMessage with id={requestMessage.Id} and text='{requestMessage.Request}' is sent ...");
                reply = await _synm.SendMessage(requestMessage!);
                if (reply != null)
                {
                    replyMessage = (ReplyMessage)reply;
                    Console.WriteLine($"Reply with id={replyMessage.Id} for message CorrelationID={replyMessage.CorrelationID} and text='{replyMessage.Reply}' is recieved.\n");
                    break;
                }
                else
                    Console.WriteLine("No reply recieved ...");
            }
        }
    }
}
