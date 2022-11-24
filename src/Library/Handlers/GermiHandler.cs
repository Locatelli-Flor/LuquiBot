using Telegram.Bot.Types;
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
    /// <summary>
    /// Un "handler" del patrón Chain of Responsibility que implementa el comando "chau".
    /// </summary>
    public class GermiHandler : BaseHandler
    {
        private TelegramBotClient bot;
        public GermiHandler(TelegramBotClient bot, BaseHandler next)
            : base(new string[] { "Germi", "germi" }, next)
        {
            this.bot = bot;
        }

        protected override void InternalHandle(Message message, out string response)
        {
            AsyncContext.Run(() => this.SendProfileImage(message));
            response = String.Empty;
        }

        private async Task SendProfileImage(Message message)
        {
            // Can be null during testing
            if (this.bot != null)
            {
                await this.bot.SendChatActionAsync(message.Chat.Id, ChatAction.UploadPhoto);

                const string filePath = @"Germi.jpg";
                using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                var fileName = filePath.Split(Path.DirectorySeparatorChar).Last();

                await this.bot.SendPhotoAsync(
                    chatId: message.Chat.Id,
                    photo: new InputOnlineFile(fileStream, fileName),
                    caption: "Es racista");
            }
        }
    }
}