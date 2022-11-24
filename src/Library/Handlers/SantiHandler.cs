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
    /// Un "handler" del patrón Chain of Responsibility que implementa el comando "dirección".
    /// </summary>
    public class SantiHandler : BaseHandler
    {
        private TelegramBotClient bot;

         /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="AddressHandler"/>.
        /// </summary>
        /// <param name="next">Un buscador de direcciones.</param>
        /// <param name="next">El próximo "handler".</param>
        public SantiHandler(TelegramBotClient bot, BaseHandler next)
            : base(new string[] { "santi", "Santi" }, next)
        {
            this.bot = bot;
        }

        /// <summary>
        /// <>
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>

        /// <summary>
        /// Procesa todos los mensajes y retorna true siempre.
        /// </summary>
        /// <param name="message">El mensaje a procesar.</param>
        /// <param name="response">La respuesta al mensaje procesado indicando que el mensaje no pudo se procesado.</param>
        /// <returns>true si el mensaje fue procesado; false en caso contrario.</returns>
        protected override void InternalHandle(Message message, out string response)
        {
            AsyncContext.Run(() => this.SendProfileImageSanti(message));
            response = String.Empty;          
        }

        /// <summary>
        /// Retorna este "handler" al estado inicial.
        /// </summary>
        private async Task SendProfileImageSanti(Message message)
        {
            // Can be null during testing
            if (this.bot != null)
            {
                await this.bot.SendChatActionAsync(message.Chat.Id, ChatAction.UploadPhoto);

                const string filePath = @"Santi.jpg";
                using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                var fileName = filePath.Split(Path.DirectorySeparatorChar).Last();

                await this.bot.SendPhotoAsync(
                    chatId: message.Chat.Id,
                    photo: new InputOnlineFile(fileStream, fileName),
                    caption: "Juega dbd, es tremendo pete, aunque a mi también me guste el jueguito");
            }
        }

        /// <summary>
        /// Indica los diferentes estados que puede tener el comando AddressHandler.
        /// - Start: El estado inicial del comando. En este estado el comando pide el nombre de la categoria y pasa al
        /// siguiente estado.
        /// - CatPrompt: Luego de pedir el nombre. En este estado el comando crea la categoría y vuelve al estado Start.
        /// </summary>
    }
}