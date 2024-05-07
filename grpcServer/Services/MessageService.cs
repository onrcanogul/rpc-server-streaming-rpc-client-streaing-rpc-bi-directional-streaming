using Grpc.Core;
using grpcMessageService;
using grpcServer;

namespace grpcServer.Services;

public class MessageService : Message.MessageBase{

    public async override Task SendMessage(IAsyncStreamReader<MessageRequest> requestStream, IServerStreamWriter<MessageResponse> responseStream, ServerCallContext context)
    {
        var task1 =  Task.Run(async () => {
            while(await requestStream.MoveNext(context.CancellationToken)) {
                Console.WriteLine($"Message  = {requestStream.Current.Message} | Name = {requestStream.Current.Name}");
            }
        });

        for(int i = 0 ; i< 10; i++) {
            await Task.Delay(200);
            await responseStream.WriteAsync(new MessageResponse() {
                Message = "message " + i
            });

        }

        await task1;
    }
}
    
