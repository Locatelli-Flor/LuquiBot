using Telegram.Bot.Types;
using System.Collections.Generic;
namespace Ucu.Poo.TelegramBot
{
    /// <summary>
    /// Un "handler" del patrón Chain of Responsibility que implementa el comando "registro".
    /// </summary>
    public class RegistroHandler : BaseHandler
    {
        public RegState State { get; set; }
        private DiccionarioEstados diccionarioEstados = new DiccionarioEstados();

        public RegistroHandler(BaseHandler next)
            : base(new string[] { "Registro", "registro" }, next)
        {
            this.State = RegState.Start;
        }

        protected override bool CanHandle(Message message)
        {
            if (this.State ==  RegState.Start)
            {
                this.diccionarioEstados.ActualizarEstado(message.Chat.Id, ((int)this.State));

                return base.CanHandle(message);
            }
            else
            {
                return true;
            }
        }

        protected override void InternalHandle(Message message, out string response)
        {
            if (this.State == RegState.Start)
            {
                // En el estado Start le pide los datos al usuario y pasa al estado RegPrompt
                this.State = RegState.RegPrompt;
                response = "Ingrese el tipo de la cuenta a crear. Opciones: Trabajador o Empleador.";
                this.diccionarioEstados.ActualizarEstado(message.Chat.Id, (int)RegState.RegPrompt);
                
            }

            if(message.Text == "Empleador")
            {
                if(this.diccionarioEstados.Diccionario[message.Chat.Id] == (int)RegState.RegPrompt)
                {
                    this.State = RegState.RegNombreEmpleador;
                    response = "Ingrese el nombre de la cuenta de empleador";
                    this.diccionarioEstados.ActualizarEstado(message.Chat.Id, (int)RegState.RegNombreEmpleador);
                }

                else if(this.diccionarioEstados.Diccionario[message.Chat.Id] == (int)RegState.RegNombreEmpleador)
                {
                    this.State = RegState.RegEmailEmpleador;
                    response = "Ingrese su email";
                    this.diccionarioEstados.ActualizarEstado(message.Chat.Id, (int)RegState.RegEmailEmpleador);
                }

                else if(this.diccionarioEstados.Diccionario[message.Chat.Id] == (int)RegState.RegEmailEmpleador)
                {
                    this.State = RegState.RegTelefonoEmpleador;
                    response = "Ingrese su teléfono";
                    this.diccionarioEstados.ActualizarEstado(message.Chat.Id, (int)RegState.RegTelefonoEmpleador);
                }

                else if(this.diccionarioEstados.Diccionario[message.Chat.Id] == (int)RegState.RegTelefonoEmpleador)
                {
                    this.State = RegState.RegUbicacionEmpleador;
                    response = "Ingrese su ubicación";
                    this.diccionarioEstados.ActualizarEstado(message.Chat.Id, (int)RegState.RegUbicacionEmpleador);
                }

                else if (this.diccionarioEstados.Diccionario[message.Chat.Id] == (int)RegState.RegUbicacionEmpleador)
                {
                    response = "Usuario Empleador creado exitosamente";
                    this.State = RegState.Start;
                    this.diccionarioEstados.ActualizarEstado(message.Chat.Id, (int)RegState.Start);
                    //crear empleador
                }

                else
                {
                    response = string.Empty;
                }
                
            }
            
            if(message.Text == "Trabajador")
            {
                if (this.diccionarioEstados.Diccionario[message.Chat.Id] == (int)RegState.RegPrompt)
                {
                    this.State = RegState.RegNombreTrabajador;
                    response = "Ingrese el nombre de la cuenta de trabajador";
                    this.diccionarioEstados.ActualizarEstado(message.Chat.Id, (int)RegState.RegNombreTrabajador);
                }
                else if(this.diccionarioEstados.Diccionario[message.Chat.Id] == (int)RegState.RegNombreTrabajador)
                {
                    this.State = RegState.RegEmailTrabajador;
                    response = "Ingrese su email";
                    this.diccionarioEstados.ActualizarEstado(message.Chat.Id, (int)RegState.RegEmailTrabajador);
                }
                else if(this.diccionarioEstados.Diccionario[message.Chat.Id] == (int)RegState.RegEmailEmpleador)
                {
                    this.State = RegState.RegTelefonoTrabajador;
                    response = "Ingrese su teléfono";
                    this.diccionarioEstados.ActualizarEstado(message.Chat.Id, (int)RegState.RegTelefonoTrabajador);
                }
                else if(this.diccionarioEstados.Diccionario[message.Chat.Id] == (int)RegState.RegTelefonoEmpleador)
                {
                    this.State = RegState.RegUbicacionTrabajador;
                    response = "Ingrese su ubicación";
                    this.diccionarioEstados.ActualizarEstado(message.Chat.Id, (int)RegState.RegUbicacionTrabajador);
                }
                else if(this.diccionarioEstados.Diccionario[message.Chat.Id] == (int)RegState.RegUbicacionEmpleador)
                {
                    response = "Usuario Trabajador creado exitosamente";
                    this.State = RegState.Start;
                    this.diccionarioEstados.ActualizarEstado(message.Chat.Id, (int)RegState.Start);
                    //crear trabajador
                }

                else
                {
                    response = string.Empty;
                }

            }
            response = string.Empty;
        }
        

        /// <summary>
        /// Retorna este "handler" al estado inicial.
        /// </summary>
        protected override void InternalCancel()
        {
            this.State = RegState.Start;
        }

        /// <summary>
        /// Indica los diferentes estados que puede tener el comando RegistroHandler.
        /// - Start: El estado inicial del comando. En este estado el comando pide el nombre de la categoria y pasa al
        /// siguiente estado.
        /// - RegPrompt: Luego de pedir el nombre. En este estado el comando crea la categoría y vuelve al estado Start.
        /// </summary>
        public enum RegState
        {
            Start,
            RegPrompt,
            RegNombreEmpleador,
            RegEmailEmpleador,
            RegTelefonoEmpleador,
            RegUbicacionEmpleador,
            RegCrearEmpleador,
            RegNombreTrabajador,
            RegEmailTrabajador,
            RegTelefonoTrabajador,
            RegUbicacionTrabajador,
            RegCrearTrabajador,
        }

    }
}