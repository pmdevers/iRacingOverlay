using System;

namespace iRacingSDK.Messaging
{
    public class Chat : iRacingMessaging
    {
        internal Chat()
        {
        }

        /// <summary>
        ///     Clear the chat window.
        /// </summary>
        public void Clear()
        {
            SendMessage(BroadcastMessage.ChatCommand, (int) ChatCommandModeTypes.Cancel);
        }

        /// <summary>
        ///     Start a reply to the last private message.
        /// </summary>
        public void Reply()
        {
            SendMessage(BroadcastMessage.ChatCommand, (int) ChatCommandModeTypes.Reply);
        }

        /// <summary>
        ///     Activate the chat window.
        /// </summary>
        public void Activate()
        {
            SendMessage(BroadcastMessage.ChatCommand, (int) ChatCommandModeTypes.BeginChat);
        }

        /// <summary>
        ///     Send a macro to the chat window.
        /// </summary>
        /// <param name="macro">The macro to send.</param>
        public void SendMacro(int macro)
        {
            if (macro < 0 || macro > 14)
                throw new ArgumentOutOfRangeException("macro", "Macro must be between 0 and 14.");
            SendMessage(BroadcastMessage.ChatCommand, (int) ChatCommandModeTypes.Macro, macro, 0);
        }

        private enum ChatCommandModeTypes
        {
            Cancel = 1,
            Reply = 2,
            BeginChat = 4,
            Macro = 8
        }
    }
}