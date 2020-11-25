using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;

namespace iRacingTimings.Shared.Components.Overlay
{
    public class TwitchBase : BaseDomComponent
    {
        private readonly TwitchClient _client;
        public List<ChatMessage> Messages { get; }
        public TwitchBase()
        {
            Messages = new List<ChatMessage>();
            var credentials = new ConnectionCredentials("iRacingOverlay", "oauth:l1ds66fhr08ex7p6ol4q879fflu7wa");
            var clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 750,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            };
            var customClient = new WebSocketClient(clientOptions);
            _client = new TwitchClient(customClient);
            _client.Initialize(credentials, "Weaxle78");

            _client.OnLog += Client_OnLog;
            _client.OnJoinedChannel += Client_OnJoinedChannel;
            _client.OnMessageReceived += Client_OnMessageReceived;
            _client.OnWhisperReceived += Client_OnWhisperReceived;
            _client.OnNewSubscriber += Client_OnNewSubscriber;
            _client.OnConnected += Client_OnConnected;

            ClassMapper.Add("twitch");
        }

        protected override void OnInitialized()
        {
            _client.Connect();
            base.OnInitialized();
        }

        private void Client_OnConnected(object sender, OnConnectedArgs e)
        {
        }

        private void Client_OnNewSubscriber(object sender, OnNewSubscriberArgs e)
        {
        }

        private void Client_OnWhisperReceived(object sender, OnWhisperReceivedArgs e)
        {
        }

        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            Messages.Add(e.ChatMessage);
            if (Messages.Count > 10)
            {
                Messages.RemoveAt(0);
            }

            InvokeAsync(StateHasChanged);
        }

        private void Client_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
        }

        private void Client_OnLog(object sender, OnLogArgs e)
        {
        }
    }
}
