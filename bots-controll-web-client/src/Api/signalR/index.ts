import { HubConnectionBuilder, HttpTransportType, LogLevel } from '@microsoft/signalr';
import { API_URL } from '..';

export const signalRConnection = new HubConnectionBuilder()
    .configureLogging(LogLevel.Debug)
    .withUrl(API_URL + "/ws/connect", {
        skipNegotiation: true,
        transport: HttpTransportType.WebSockets
    })
    .build()

signalRConnection.start()
    .then(() => console.log("подключено"));

export enum signalRActions {
    ReceiveMessage = "ReceiveMessage",
    OnBotDisconnection = "OnBotDisconnection",
    OnBotConnection = "OnNewConnection",
}
