using RequestReply;

Console.WriteLine("Synchronous Request-Reply over Async messaging Test!");

var ssm = new SampleSetup();
await ssm.Run();
