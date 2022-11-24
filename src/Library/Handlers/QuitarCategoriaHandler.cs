using Telegram.Bot.Types;
using System.Collections.Generic;
using Chatbot;

namespace Ucu.Poo.TelegramBot
{
    /// <summary>
    /// Un "handler" del patrón Chain of Responsibility que implementa el comando "dirección".
    /// </summary>
    public class QuitarCatHandler : BaseHandler
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
        public QuitarCatHandler(BaseHandler next)
            : base(new string[] { "Quitar categoria", "quitar categoria" }, next)
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
            if (this.diccionarioEstados.Diccionario[message.Chat.Id] ==  (int)CatState.Start)
            {
                // En el estado Start le pide el nombre de la categoría y pasa al estado CatPrompt
                this.State = CatState.CatPrompt;
                response = "Ingrese el nombre de la categoría";
                this.diccionarioEstados.ActualizarEstado(message.Chat.Id, (int)CatState.CatPrompt);
                
            }
            else if (this.diccionarioEstados.Diccionario[message.Chat.Id] == (int)CatState.CatPrompt)
            {
                // En el estado CatPrompt el mensaje recibido es la respuesta con el nombre de la categoría y pasa al estado Start
                List<Categoria> listaCategoria = Singleton<ListaCategorias>.Instance.ListaDeCategorias;
                foreach(Categoria cat in listaCategoria)
                {
                    if (message.Text.ToLower().Trim() == cat.Categ)
                    {
                        Singleton<ListaCategorias>.Instance.ListaDeCategorias.Remove(cat);
                        this.State = CatState.Start;
                        response = $"Categoría {message.Text} eliminada exitosamente";
                        this.diccionarioEstados.ActualizarEstado(message.Chat.Id, (int)CatState.Start);
                        return;
                    }
                }
            response = $"Categoria {message.Text} no encontrada.";
            }
            else
            {
                response = string.Empty;
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