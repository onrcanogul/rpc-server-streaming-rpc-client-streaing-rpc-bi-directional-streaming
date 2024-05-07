using Grpc.Net.Client;
using grpcMessageService;

var channel = GrpcChannel.ForAddress("http://localhost:5090");
var messageClient = new Message.MessageClient(channel);

#region Server Streaming 
//proto'da response'nin stream ile işaretlenmesi lazim.
//serverdan gelen streamler
// var response = messageClient.SendMessage(new() {
//     Message = "message1",
//     Name = "name of message1"
// });
// CancellationTokenSource cancellationTokenSource = new();

// while(await response.ResponseStream.MoveNext(cancellationTokenSource.Token)) {
//     Console.WriteLine(response.ResponseStream.Current.Message);  
// }

#endregion


#region Client Streaming
//proto dosyasında request stream ile işaretlenmeli
//clientten gelen streamler
// var request = messageClient.SendMessage();

// for(int i = 0 ; i < 10 ; i++) {
//     await Task.Delay(1000);
//     await request.RequestStream.WriteAsync(new() {
//         Message = "message" + i,
//         Name = "name " + i,
//     });
// }
// await request.RequestStream.CompleteAsync();

// Console.WriteLine((await request.ResponseAsync).Message);

#endregion



#region Bi Directional Streaming
//ikisi de stream
var request = messageClient.SendMessage();

var task1 = Task.Run(async() =>
{
    for(int i = 0 ; i<10; i++) {
    await Task.Delay(200);
    await request.RequestStream.WriteAsync(new MessageRequest()
    {
        Message = "message " + i,
        Name = "name " + i, 
    });
}
});
CancellationTokenSource cancellationTokenSource = new();
while(await request.ResponseStream.MoveNext(cancellationTokenSource.Token)) 
{
    Console.WriteLine(request.ResponseStream.Current.Message);
}


await task1;
await request.RequestStream.CompleteAsync();



#endregion