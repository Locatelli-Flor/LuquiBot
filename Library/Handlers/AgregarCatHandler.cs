using Telegram.Bot.Types;
using Chatbot;

namespace Ucu.Poo.TelegramBot
{
    /// <summary>
    /// Un "handler" del patrón Chain of Responsibility que implementa el comando "dirección".
    /// </summary>
    public class AgregarCatHandler : BaseHandler
    {
        /// <summary>
        /// El estado del comando.
        /// </summary>
        public CatState State { get; set; }
        private DiccionarioEstados diccionarioEstados = new DiccionarioEstados();


        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="AddressHandler"/>.
        /// </summary>
        /// <param name="next">Un buscador de direcciones.</param>
        /// <param name="next">El próximo "handler".</param>
        public AgregarCatHandler(BaseHandler next)
            : base(new string[] { "Agregar categoria", "agregar categoria" }, next)
        {
            this.State = CatState.Start;
        }

        /// <summary>
        /// <>
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected override bool CanHandle(Message message)
        {
            if (this.State ==  CatState.Start)
            {
                this.diccionarioEstados.ActualizarEstado(message.Chat.Id, ((int)this.State));

                return base.CanHandle(message);
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Procesa todos los mensajes y retorna true siempre.
        /// </summary>
        /// <param name="message">El mensaje a procesar.</param>
        /// <param name="response">La respuesta al mensaje procesado indicando que el mensaje no pudo se procesado.</param>
        /// <returns>true si el mensaje fue procesado; false en caso contrario.</returns>
        protected override void InternalHandle(Message message, out string response)
        {
            if(message.Chat.Id == 5571671723)
            {
                if (this.diccionarioEstados.Diccionario[message.Chat.Id] ==  (int)CatState.Start)
                {
                    
                    // En el estado Start le pide el nombre de la categoría y pasa al estado CatPrompt
                    this.State = CatState.CatPrompt;
                    response = "Ingrese el nombre de la categoría";
                    this.diccionarioEstados.ActualizarEstado(message.Chat.Id, (int)CatState.CatPrompt);
                    
                }
                else if (this.diccionarioEstados.Diccionario[message.Chat.Id] ==  (int)CatState.CatPrompt)
                {
                    // En el estado CatPrompt el mensaje recibido es la respuesta con el nombre de la categoría y pasa al estado Start
                    Categoria nueva = new Categoria(message.Text.ToLower().Trim());
                    Singleton<ListaCategorias>.Instance.ListaDeCategorias.Add(nueva);

                    response = $"Categoría {message.Text} creada exitosamente";
                    string json = Singleton<ListaCategorias>.Instance.ConvertToJson();
                    System.IO.File.WriteAllText(@"..\Library\ListaDeCategorias.json", json);
                    
                    this.State = CatState.Start;
                    this.diccionarioEstados.ActualizarEstado(message.Chat.Id, (int)CatState.Start);

                }
                else
                {
                    response = string.Empty;
                }
            }
            else
            {
                response = "No tiene permisos para realizar esta acción";
            }
        }

        /// <summary>
        /// Retorna este "handler" al estado inicial.
        /// </summary>
        protected override void InternalCancel()
        {
            this.State = CatState.Start;
        }

        /// <summary>
        /// Indica los diferentes estados que puede tener el comando AddressHandler.
        /// - Start: El estado inicial del comando. En este estado el comando pide el nombre de la categoria y pasa al
        /// siguiente estado.
        /// - CatPrompt: Luego de pedir el nombre. En este estado el comando crea la categoría y vuelve al estado Start.
        /// </summary>
        public enum CatState
        {
            Start,
            CatPrompt
        }
    }
}