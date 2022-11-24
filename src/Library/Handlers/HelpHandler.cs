using Telegram.Bot.Types;

namespace Ucu.Poo.TelegramBot
{
    /// <summary>
    /// Un "handler" del patrón Chain of Responsibility que implementa el comando "ayuda".
    /// </summary>
    public class HelpHandler : BaseHandler
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="HelloHandler"/>. Esta clase procesa el mensaje "ayuda".
        /// </summary>
        /// <param name="next">El próximo "handler".</param>
        public HelpHandler(BaseHandler next) : base(next)
        {
            this.Keywords = new string[] {"ayuda"};
        }

        /// <summary>
        /// Procesa el mensaje "ayuda" y retorna true; retorna false en caso contrario.
        /// </summary>
        /// <param name="message">El mensaje a procesar.</param>
        /// <param name="response">La respuesta al mensaje procesado.</param>
        /// <returns>true si el mensaje fue procesado; false en caso contrario.</returns>
        protected override void InternalHandle(Message message, out string response)
        {
            response = "Guía de comandos: \n hola - Saluda \n chau - Despedida \n distancia - Calcula la distancia en kilómetros entre dos direcciones \n dirección - Retorna la ubicación de una dirección";
        }
    }
}