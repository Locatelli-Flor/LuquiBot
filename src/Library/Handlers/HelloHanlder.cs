using Telegram.Bot.Types;

namespace Ucu.Poo.TelegramBot
{
    /// <summary>
    /// Un "handler" del patrón Chain of Responsibility que implementa el comando "hola".
    /// </summary>
    public class HelloHandler : BaseHandler
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="HelloHandler"/>. Esta clase procesa el mensaje "hola".
        /// </summary>
        /// <param name="next">El próximo "handler".</param>
        public HelloState State { get; set; }

        public HelloHandler(BaseHandler next)
            : base(new string[] { "hola", "Hola" }, next)
        {
            this.State = HelloState.Start;
        }

        protected override bool CanHandle(Message message)
        {
            if (this.State ==  HelloState.Start)
            {
                return base.CanHandle(message);
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Procesa el mensaje "hola" y retorna true; retorna false en caso contrario.
        /// </summary>
        /// <param name="message">El mensaje a procesar.</param>
        /// <param name="response">La respuesta al mensaje procesado.</param>
        /// <returns>true si el mensaje fue procesado; false en caso contrario.</returns>
        protected override void InternalHandle(Message message, out string response)
        {
            if(this.State == HelloState.Start)
            {
                response = "Hola, ¿Estás soltera?";
                this.State = HelloState.HelloPromt;

            }
            else if(this.State == HelloState.HelloPromt)
            {
                if(message.Text == "si")
                {
                    response = "Opa... me gustó";
                    this.State = HelloState.Start;
                }
                else
                {
                    response = "Querés estarlo?";
                    this.State = HelloState.Start;
                }
                
            }
            else
            {
                response = string.Empty;
            }
        }


        public enum HelloState
        {
            Start,
            HelloPromt
        }
    }
}