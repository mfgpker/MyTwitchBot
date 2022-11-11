using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Interfaces;
using TwitchLib.Communication.Models;

namespace TwitchBot2
{
    public class BotClient
    {
        private readonly TwitchClient client;
        private readonly Config _config;

        public BotClient(Config config)
        {
            _config = config;

            ConnectionCredentials credentials = new ConnectionCredentials(config.BotUserName, config.ACCESS_TOKEN);
            var clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 750,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            };
            WebSocketClient customClient = new WebSocketClient(clientOptions);
            client = new TwitchClient(customClient);
            client.Initialize(credentials, config.Channelname);


            client.OnLog += Client_OnLog;
            client.OnJoinedChannel += Client_OnJoinedChannel;
            client.OnMessageReceived += Client_OnMessageReceived;
            client.OnWhisperReceived += Client_OnWhisperReceived;
            client.OnNewSubscriber += Client_OnNewSubscriber;
            client.OnConnected += Client_OnConnected;
            client.OnLeftChannel += Client_OnLeftChannel;
            client.OnChatCommandReceived += Client_OnChatCommandReceived;

            client.AddChatCommandIdentifier('$');

            client.Connect();
        }

        private void Client_OnChatCommandReceived(object sender, OnChatCommandReceivedArgs e)
        {
            switch (e.Command.CommandText.ToLower())
            {
                case "hello":
                    client.SendMessage(e.Command.ChatMessage.Channel, $"Hej {e.Command.ChatMessage.Username} håber du har haft en god dag \r\n:)!");
                    break;
            }
        }

        private void Client_OnLeftChannel(object sender, OnLeftChannelArgs e)
        {
            client.SendMessage(e.Channel, $"Jeg smutter igen {e.BotUsername}!");
        }

        private void Client_OnLog(object sender, OnLogArgs e)
        {
            Console.WriteLine($"LOG: {e.DateTime.ToString()}: {e.BotUsername} - {e.Data}");
        }

        private void Client_OnConnected(object sender, OnConnectedArgs e)
        {
            Console.WriteLine($"Connected to {e.AutoJoinChannel}");
        }

        private void Client_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            // client.SendMessage(e.Channel, $"Hej! jeg er en bot, og jeg hedder ${e.BotUsername}!");
        }

        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            if (e.ChatMessage.Message.Contains("badword"))
                client.TimeoutUser(e.ChatMessage.Channel, e.ChatMessage.Username, TimeSpan.FromMinutes(5), "Bad word! 5 minute timeout!");

            if (e.ChatMessage.Message.ToLower().StartsWith("hello") || e.ChatMessage.Message.ToLower().StartsWith("hej") || e.ChatMessage.Message.ToLower().StartsWith("hey"))
            {
                client.SendMessage(e.ChatMessage.Channel, $"Hej {e.ChatMessage.Username} håber du har haft en god dag :)");

            }
        }

        private void Client_OnWhisperReceived(object sender, OnWhisperReceivedArgs e)
        {
            if (e.WhisperMessage.Username == "mic22s")
                client.SendWhisper(e.WhisperMessage.Username, "Hey! Whispers are so cool!!");
        }

        private void Client_OnNewSubscriber(object sender, OnNewSubscriberArgs e)
        {
            if (e.Subscriber.SubscriptionPlan == SubscriptionPlan.Prime)
                client.SendMessage(e.Channel, $"Welcome {e.Subscriber.DisplayName} to the club! So kind of you to use your Twitch Prime on this channel!");
            else
                client.SendMessage(e.Channel, $"Welcome {e.Subscriber.DisplayName} to the club!");
        }
    }
}
