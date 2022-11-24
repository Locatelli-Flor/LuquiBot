using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Nito.AsyncEx;

namespace Ucu.Poo.TelegramBot
{
    public class OpinionHandler : BaseHandler
    {
        private TelegramBotClient bot;

        public OpiState State { get; set; }

        public OpinionHandler(TelegramBotClient bot, BaseHandler next)
            : base(new string[] { "Qué opinas de las personas presentes?", "Que opinas de las personas presentes?", "qué opinas de las personas presentes?", "que opinas de las personas presentes?" }, next)
        {
            this.State = OpiState.Start;
            this.bot = bot;
        }

        protected override bool CanHandle(Message message)
        {
            if (this.State ==  OpiState.Start)
            {
                return base.CanHandle(message);
            }
            else
            {
                return true;
            }
        }
        protected override void InternalHandle(Message message, out string response)
        {
                if (this.State == OpiState.Start)
                {
                    this.State = OpiState.OpiPrompt;
                    response = "De esta ensalada de gente? A ver, decime nombres";
                }
                else if(this.State == OpiState.OpiPrompt && message.Text == "Jope" || message.Text == "jope")
                {
                    AsyncContext.Run(() => this.SendProfileImageJope(message));
                    response = String.Empty;
                    this.State = OpiState.Start;
                }
                else
                {
                    response = string.Empty;
                }
        }

        protected override void InternalCancel()
        {
            this.State = OpiState.Start;
        }

        private async Task SendProfileImageJope(Message message)
        {
            // Can be null during testing
            if (this.bot != null)
            {
                await this.bot.SendChatActionAsync(message.Chat.Id, ChatAction.UploadPhoto);

                const string filePath = @"Jope.jpg";
                using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                var fileName = filePath.Split(Path.DirectorySeparatorChar).Last();

                await this.bot.SendPhotoAsync(
                    chatId: message.Chat.Id,
                    photo: new InputOnlineFile(fileStream, fileName),
                    caption: "Es una gordita");
            }
        }


        /// </summary>
        public enum OpiState
        {
            Start,
            OpiPrompt
        }
    }
}